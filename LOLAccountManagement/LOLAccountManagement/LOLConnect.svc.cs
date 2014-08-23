using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using LOLAccountManagement.Classes;
using LOLCodeLibrary.ErrorsManagement;
using LOLCodeLibrary.LoggingSystem;
using LOLAccountManagement.Classes.DtoObjects;
using LOLAccountManagement.Classes.ErrorsMgmt;

namespace LOLAccountManagement
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed),
     Guid("23618A5F-A903-4612-96F7-E28F962CE303")]
    public class LOLConnect
    {
        DataTools _dt = new DataTools();

        #region AuthenticationToken

        [OperationContract]
        public Guid AuthenticationTokenGet(string deviceID)
        {
            AuthenticationToken tmpReturn = new AuthenticationToken(deviceID, OperationContext.Current);
            tmpReturn.Save();
            return tmpReturn.AuthenticationTokenID;
        }

        #endregion AuthenticationToken

        #region User

        [OperationContract]
        public User UserLogin(string deviceID, Device.DeviceTypes deviceType, Guid accountID, string oAuthID, string oAuthToken, AccountOAuth.OAuthTypes oAuthType, string emailAddress, string password, Guid authenticationToken)
        {
            ILogger logger = new LoggerManager(new DebugTextLogger()).GetManager();
            logger.LogMessage("UserLogin called at : " + DateTime.Now.ToString() + ". ", true);
            LOLAccountRepository repo = new LOLAccountRepository(_dt);
            User tmpReturn = new User();

            General.Error errorObject = new General.Error();

            if (string.IsNullOrEmpty(deviceID))
            {
                errorObject = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.DeviceIDMissing, "UserLogin");
                tmpReturn.Errors = new List<General.Error>();
                tmpReturn.Errors.Add(errorObject);
                logger.LogMessage("Ended with DeviceID empty at : " + DateTime.Now.ToString(), false);
                return tmpReturn;
            }

            if (!repo.ValidateToken(authenticationToken))
            {
                errorObject = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.AuthenticationTokenInvalid, "UserLogin");
                tmpReturn.Errors = new List<General.Error>();
                tmpReturn.Errors.Add(errorObject);
                logger.LogMessage("Ended with token error at : " + DateTime.Now.ToString(), false);
                return tmpReturn;
                //errorObject = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.AuthenticationTokenInvalid, "UserLogin");              
            }
            
            if (accountID != Guid.Empty)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT u.* FROM Users (nolock) u INNER JOIN Devices (nolock) d ON d.AccountID = u.AccountID WHERE d.DeviceID = @DeviceID AND u.AccountID = @AccountID;";
                cmd.Parameters.Add("@DeviceID", SqlDbType.NVarChar, 100).Value = deviceID;
                cmd.Parameters.Add("@AccountID", SqlDbType.UniqueIdentifier).Value = accountID;

                DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    tmpReturn = new User(ds.Tables[0].Rows[0]);
                    repo.AssignAccountToToken(authenticationToken, accountID);
                    logger.LogMessage("Ended with AccountID success at : " + DateTime.Now.ToString(), false);
                    return tmpReturn;
                }

                //else
                //  errorObject = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.AccountIDInvalid, "UserLogin");
            }
            
            if (!string.IsNullOrEmpty(oAuthToken))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT u.* FROM Users (nolock) u INNER JOIN AccountOAuth (nolock) da ON u.AccountID = da.AccountID WHERE da.OAuthTypeID = @OAuthType AND da.OAuthToken = @OAuthToken;";
                cmd.Parameters.Add("@OAuthType", SqlDbType.Int).Value = oAuthType;
                cmd.Parameters.Add("@OAuthToken", SqlDbType.NVarChar, 100).Value = oAuthToken;

                DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    tmpReturn = new User(ds.Tables[0].Rows[0]);
                    repo.AssignAccountToToken(authenticationToken, tmpReturn.AccountID);
                    CreateDevice(deviceID, tmpReturn.AccountID, deviceType);
                    logger.LogMessage("Ended with OAuthToken success at : " + DateTime.Now.ToString(), false);
                    return tmpReturn;
                }
            }
            
            if (!string.IsNullOrEmpty(oAuthID))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT u.* FROM Users (nolock) u INNER JOIN AccountOAuth (nolock) da ON u.AccountID = da.AccountID WHERE da.OAuthTypeID = @OAuthType AND da.OAuthID = @OAuthID;";
                cmd.Parameters.Add("@OAuthType", SqlDbType.Int).Value = oAuthType;
                cmd.Parameters.Add("@OAuthID", SqlDbType.NVarChar, 100).Value = oAuthID;

                DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    tmpReturn = new User(ds.Tables[0].Rows[0]);
                    repo.AssignAccountToToken(authenticationToken, tmpReturn.AccountID);
                    CreateDevice(deviceID, tmpReturn.AccountID, deviceType);
                    logger.LogMessage("Ended with OAuthID success at : " + DateTime.Now.ToString(), false);
                    return tmpReturn;
                }
                else if (string.IsNullOrEmpty(emailAddress) || string.IsNullOrEmpty(password))
                {
                    errorObject = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.DeviceNotRegisteredWithAnAccount, "UserLogin");
                    tmpReturn.Errors = new List<General.Error>();
                    tmpReturn.Errors.Add(errorObject);
                    logger.LogMessage("Ended with OAuthID - email/password empty error at : " + DateTime.Now.ToString(), false);
                    return tmpReturn;
                }
            }
            
            if (!string.IsNullOrEmpty(emailAddress) && !string.IsNullOrEmpty(password))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT u.* FROM Users (nolock) u WHERE EmailAddress = @EmailAddress";
                cmd.Parameters.Add("@EmailAddress", SqlDbType.NVarChar, 300).Value = Cryptography.Encrypt(emailAddress.ToLower());

                DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    tmpReturn = new User(ds.Tables[0].Rows[0]);
                    repo.AssignAccountToToken(authenticationToken, tmpReturn.AccountID);

                    if (Cryptography.Decrypt(((string)ds.Tables[0].Rows[0]["password"])).Equals(password.ToLower()))
                    {
                        CreateDevice(deviceID, tmpReturn.AccountID, deviceType);
                        logger.LogMessage("Ended with email/password success at : " + DateTime.Now.ToString(), false);
                        return tmpReturn;
                    }
                    else
                    {
                        errorObject = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.PasswordsDontMatch, "UserLogin");
                        tmpReturn.Errors = new List<General.Error>();
                        tmpReturn.Errors.Add(errorObject);
                        logger.LogMessage("Ended with email/password failed error at : " + DateTime.Now.ToString(), false);
                        return tmpReturn;
                    }
                }
                else
                {
                    errorObject = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.AccountInformationInvalid, "UserLogin");
                    tmpReturn.Errors = new List<General.Error>();
                    tmpReturn.Errors.Add(errorObject);
                    logger.LogMessage("Ended with email/password empty error at : " + DateTime.Now.ToString(), false);
                    return tmpReturn;
                }
            }

            //if (ErrorManagement.IsErrorObjectValid(errorObject))
            //{
            errorObject = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.AccountInformationInvalid, "UserLogin");
            tmpReturn.Errors = new List<General.Error>();
            tmpReturn.Errors.Add(errorObject);
            repo.LogError(new MethodParameters(deviceID, deviceType, accountID, oAuthID, oAuthToken, oAuthType, emailAddress, password, authenticationToken).ParametersList, "UserLogin", errorObject);
            //}
            //else if (tmpReturn.AccountID.Equals(Guid.Empty))
            //{
            //    //we did not get a user back and more importantly we don't know why as no error has been raised ! 
            //    //it means we have missed an error condition and the code needs revisiting.                 

            //    errorObject = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.Unexpected, "UserLogin");
            //    tmpReturn.Errors = new List<General.Error>();
            //    tmpReturn.Errors.Add(errorObject);

            //    repo.LogError(new MethodParameters(deviceID, deviceType, accountID, oAuthID, oAuthToken, oAuthType, emailAddress, password, authenticationToken).ParametersList, "UserLogin", errorObject);
            //}
            logger.LogMessage("Ended with untrapped error at : " + DateTime.Now.ToString(), false);
            return tmpReturn;
        }

        private void CreateDevice(string deviceID, Guid accountID, Device.DeviceTypes deviceType)
        {
            Device tmpDevice = new Device(deviceID);
            if (tmpDevice.AccountID == Guid.Empty)
            {
                tmpDevice.AccountID = accountID;
                tmpDevice.DeviceType = deviceType;
                tmpDevice.Save();
            }
        }

        [OperationContract]
        public User UserCreate(string deviceID, Device.DeviceTypes deviceType, AccountOAuth.OAuthTypes oAuthType, string oAuthID, string oAuthToken, string firstName, string lastName, string emailAddress, string password, byte[] picture, DateTime dateOfBirth, Guid authenticationToken)
        {
            LOLAccountRepository repo = new LOLAccountRepository(_dt);
            User tmpReturn = new User();

            if (string.IsNullOrEmpty(deviceID))
            {
                General.Error tmpError = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.DeviceIDMissing, "UserCreate");
                tmpReturn.Errors = new List<General.Error>();
                tmpReturn.Errors.Add(tmpError);
                return tmpReturn;
            }

            if (!repo.ValidateToken(authenticationToken))
            {
                var errorObject = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.AuthenticationTokenInvalid, "UserCreate");
                tmpReturn.Errors = new List<General.Error>();
                tmpReturn.Errors.Add(errorObject);
                repo.LogError(new MethodParameters(deviceID, deviceType, oAuthType, oAuthID, oAuthToken, firstName, lastName, emailAddress, password, dateOfBirth, authenticationToken).ParametersList, "UserLogin", errorObject);
                return tmpReturn;
            }

            if (!string.IsNullOrEmpty(oAuthID))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT u.* FROM AccountOAuth do INNER JOIN Users u ON do.AccountID = u.AccountID WHERE do.OAuthTypeID = @OAuthType AND do.OAuthID = @OAuthID";
                cmd.Parameters.Add("@OAuthType", SqlDbType.Int).Value = (int)oAuthType;
                cmd.Parameters.Add("@OAuthID", SqlDbType.NVarChar, 100).Value = oAuthID;

                DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    User tmpUser = new User(ds.Tables[0].Rows[0]);
                    CreateDevice(deviceID, tmpUser.AccountID, deviceType);
                    AccountOAuth tmpOAuth = AccountOAuthCreate(tmpUser.AccountID, oAuthType, oAuthID, oAuthToken, authenticationToken);
                    return tmpUser;
                }
                else
                {
                    tmpReturn.AccountActive = true;
                    tmpReturn.DateOfBirth = dateOfBirth;
                    tmpReturn.EmailAddress = emailAddress.ToLower();
                    tmpReturn.FirstName = firstName;
                    tmpReturn.LastName = lastName;
                    tmpReturn.Password = password.ToLower();
                    tmpReturn.Picture = picture;
                    tmpReturn.Save();

                    AccountOAuth tmpAuth = new AccountOAuth();
                    tmpAuth.AccountID = tmpReturn.AccountID;
                    tmpAuth.OAuthType = oAuthType;
                    tmpAuth.OAuthID = oAuthID;
                    tmpAuth.OAuthToken = oAuthToken;
                    tmpAuth.Save();

                    return tmpReturn;
                }
            }

            if (emailAddress.Length > 0)
            {
                //First check for user that already exists.
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM Users (nolock) WHERE EmailAddress = @EmailAddress";
                cmd.Parameters.Add("@emailAddress", SqlDbType.NVarChar, 300).Value = Cryptography.Encrypt(emailAddress.ToLower());

                DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    User tmpUser = new User(ds.Tables[0].Rows[0]);

                    cmd = new SqlCommand();
                    cmd.CommandText = "SELECT * FROM Devices WHERE DeviceID = @DeviceID AND AccountID = @AccountID";
                    cmd.Parameters.Add("@DeviceID", SqlDbType.NVarChar, 100).Value = deviceID;
                    cmd.Parameters.Add("@AccountID", SqlDbType.UniqueIdentifier).Value = tmpUser.AccountID;

                    ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

                    if (ds.Tables[0].Rows.Count > 0)
                        return tmpUser;

                    var errorObject = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.DuplicateEmailAddress, "UserCreate");
                    tmpReturn.Errors = new List<General.Error>();
                    tmpReturn.Errors.Add(errorObject);
                    repo.LogError(new MethodParameters(deviceID, deviceType, oAuthType, oAuthID, oAuthToken, firstName, lastName, emailAddress, password, dateOfBirth, authenticationToken).ParametersList, "UserCreate", errorObject);
                    return tmpReturn;
                }
                else
                {
                    tmpReturn.AccountActive = true;
                    tmpReturn.DateOfBirth = dateOfBirth;
                    tmpReturn.EmailAddress = emailAddress.ToLower();
                    tmpReturn.FirstName = firstName;
                    tmpReturn.LastName = lastName;
                    tmpReturn.Password = password.ToLower();
                    tmpReturn.Picture = picture;
                    tmpReturn.Save();

                    tmpReturn.Password = string.Empty;

                    Device tmpDevice = new Device();
                    tmpDevice.AccountID = tmpReturn.AccountID;
                    tmpDevice.DeviceID = deviceID;
                    tmpDevice.DeviceType = deviceType;
                    tmpDevice.Save();

                    if (tmpDevice.Errors.Count > 0)
                    {
                        tmpReturn.Errors = tmpDevice.Errors;
                        return tmpReturn;
                    }

                    if (oAuthID != null && oAuthID.Length > 0 && oAuthToken != null)
                        AccountOAuthCreate(tmpReturn.AccountID, oAuthType, oAuthID, oAuthToken, authenticationToken);

                    return tmpReturn;
                }
            }
            else
            {

                tmpReturn.AccountActive = true;
                tmpReturn.DateOfBirth = dateOfBirth;
                tmpReturn.EmailAddress = emailAddress.ToLower();
                tmpReturn.FirstName = firstName;
                tmpReturn.LastName = lastName;
                tmpReturn.Password = password.ToLower();
                tmpReturn.Picture = picture;
                tmpReturn.Save();

                tmpReturn.Password = string.Empty;

                Device tmpDevice = new Device();
                tmpDevice.AccountID = tmpReturn.AccountID;
                tmpDevice.DeviceID = deviceID;
                tmpDevice.DeviceType = deviceType;
                tmpDevice.Save();

                if (tmpDevice.Errors.Count > 0)
                {
                    tmpReturn.Errors = tmpDevice.Errors;
                    return tmpReturn;
                }

                if (oAuthID != null && oAuthID.Length > 0 && oAuthToken != null)
                    AccountOAuthCreate(tmpReturn.AccountID, oAuthType, oAuthID, oAuthToken, authenticationToken);

                return tmpReturn;
            }

        }

        [OperationContract]
        public User UserGetSpecific(Guid accountID, Guid targetAccountID, Guid authenticationToken)
        {
            LOLAccountRepository repo = new LOLAccountRepository(_dt);            
            User tmpReturn = new User();

            General.Error tmpError = repo.ValidateToken(authenticationToken, accountID);
            if (!string.IsNullOrEmpty(tmpError.ErrorNumber))
            {
                tmpReturn.Errors = new List<General.Error>();
                tmpReturn.Errors.Add(tmpError);
                repo.LogError(new MethodParameters(accountID, targetAccountID, authenticationToken).ParametersList, "UserGetSpecific", tmpError);
            }
            else
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM Users (nolock) WHERE AccountID = @AccountID";
                cmd.Parameters.Add("@AccountID", SqlDbType.UniqueIdentifier).Value = targetAccountID;

                DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

                if (ds.Tables[0].Rows.Count > 0)
                    tmpReturn = new User(ds.Tables[0].Rows[0]);
                else
                {
                    var errorObject = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.UserGetSpecificFailed, "UserGetSpecific");
                    tmpReturn.Errors = new List<General.Error>();
                    tmpReturn.Errors.Add(errorObject);
                    repo.LogError(new MethodParameters(accountID, authenticationToken).ParametersList, "UserGetSpecific", errorObject);
                }
            }

            return tmpReturn;
        }


        [OperationContract]
        public User UserSave(User user, Guid authenticationToken)
        {
            LOLAccountRepository repo = new LOLAccountRepository(_dt);
            General.Error tmpError = repo.ValidateToken(authenticationToken, user.AccountID);
            if (!string.IsNullOrEmpty(tmpError.ErrorNumber))
            {
                user.Errors = new List<General.Error>();
                user.Errors.Add(tmpError);
                repo.LogError(new MethodParameters(user.AccountID, authenticationToken).ParametersList, "UserSave", tmpError);
            }
            else
            {
                user.Errors = new List<General.Error>();
                user.Save();
                user.Password = string.Empty;
            }

            return user;
        }

        //[OperationContract]
        //public List<General.Error> UserDelete(User user, Guid authenticationToken)
        //{
        //    user.Errors = new List<General.Error>();
        //    user.Delete();
        //    return user.Errors;
        //}

        /// <summary>
        /// Marks the authetication token
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="accountID"></param>
        /// <param name="authenticationToken"></param>
        /// <returns></returns>
        [OperationContract]
        public List<General.Error> UserLogOut(string deviceID, Guid accountID, Guid authenticationToken)
        {
            LOLAccountRepository repo = new LOLAccountRepository(_dt);
            List<General.Error> tmpReturn = new List<General.Error>();
            General.Error tmpError = new General.Error();

            if (string.IsNullOrEmpty(deviceID))
            {
                tmpError = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.DeviceIDMissing, "UserLogOut");
                tmpReturn.Add(tmpError);
                return tmpReturn;
            }

            tmpError = repo.ValidateToken(authenticationToken, accountID);
            if (!string.IsNullOrEmpty(tmpError.ErrorNumber))
            {
                tmpError.ErrorLocation = "UserLogOut";
                tmpReturn.Add(tmpError);

                var paramData = new MethodParameters(accountID, authenticationToken);
                repo.MethodParameters = paramData.ParametersList;
                repo.MethodName = tmpError.ErrorLocation;
                repo.LastErrorMessage = tmpError.ErrorDescription;
                repo.LogError();
                return tmpReturn;
            }
            
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add("@DeviceID", SqlDbType.NVarChar, 100).Value = deviceID;
            cmd.Parameters.Add("@AccountID", SqlDbType.UniqueIdentifier).Value = accountID;

            cmd.CommandText = "UPDATE AuthenticationTokens SET LoggedOut = 1, LogoutDateTime = GetDate()  WHERE AuthenticationDeviceID = @DeviceID";
            _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLAccountManagement);

            cmd.CommandText = "DELETE FROM Devices WHERE DeviceID = @DeviceID";
            _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLAccountManagement);

            return tmpReturn;
        }

        [OperationContract]
        public General.Error UserGetResetToken(string emailAddress, Guid authenticationToken)
        {
            LOLAccountRepository repo = new LOLAccountRepository(_dt);
            General.Error tmpReturn = new General.Error();

            if (!repo.ValidateToken(authenticationToken, out tmpReturn))
            {
                repo.LogError(new MethodParameters(emailAddress, authenticationToken).ParametersList, "UserGetResetToken", tmpReturn);
            }
            else if (!string.IsNullOrEmpty(emailAddress))
            {
                //First check for user that already exists.
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM Users (nolock) WHERE EmailAddress = @EmailAddress";
                cmd.Parameters.Add("@emailAddress", SqlDbType.NVarChar, 300).Value = Cryptography.Encrypt(emailAddress.ToLower());

                DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    User tmpUser = new User(ds.Tables[0].Rows[0]);

                    cmd = new SqlCommand();
                    cmd.CommandText = "INSERT INTO PasswordResetTokens (AccountID, ResetToken) VALUES (@AccountID, @ResetToken)";
                    string ResetToken = System.Guid.NewGuid().ToString().Substring(0, 8);
                    cmd.Parameters.Add("@AccountID", SqlDbType.UniqueIdentifier).Value = tmpUser.AccountID;
                    cmd.Parameters.Add("@ResetToken", SqlDbType.NVarChar, 8).Value = ResetToken;

                    _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLAccountManagement);

                    string Message = "Your LOL Reset Code is:  " + ResetToken;
                    Mail.SendMail(tmpUser.EmailAddress, "LOL Account Password Reset", Message);
                }
                else
                {
                    tmpReturn = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.ResetPasswordFailed, "UserGetResetToken");
                    repo.LogError(new MethodParameters(emailAddress, authenticationToken).ParametersList, "UserGetResetToken", tmpReturn);
                }
            }
            else
            {
                tmpReturn = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.NullEmailAddress, "UserGetResetToken");
                repo.LogError(new MethodParameters(emailAddress, authenticationToken).ParametersList, "UserGetResetToken", tmpReturn);
            }

            return tmpReturn;
        }

        [OperationContract]
        public User UserPasswordReset(string emailAddress, string resetToken, string password, Guid authenticationToken)
        {
            LOLAccountRepository repo = new LOLAccountRepository(_dt);
            User tmpReturn = new User();
            tmpReturn.Errors = new List<General.Error>();
            General.Error tmpError = new General.Error();

            if (!repo.ValidateToken(authenticationToken, out tmpError))
            {
                tmpError.ErrorDescription = "Invalid Authentication Token.";
                tmpError.ErrorNumber = "1";
                tmpError.ErrorLocation = "UserPasswordReset";

                var paramData = new MethodParameters(emailAddress, resetToken, password, authenticationToken);
                repo.MethodParameters = paramData.ParametersList;
                repo.MethodName = tmpError.ErrorLocation;
                repo.LastErrorMessage = tmpError.ErrorDescription;
                repo.LogError();

                tmpReturn.Password = string.Empty;
                return tmpReturn;
            }

            if (string.IsNullOrEmpty(password))
            {
                tmpError.ErrorDescription = "Invalid Password.";
                tmpError.ErrorNumber = "99";
                tmpError.ErrorLocation = "UserPasswordReset";

                var paramData = new MethodParameters(emailAddress, resetToken, password, authenticationToken);
                repo.MethodParameters = paramData.ParametersList;
                repo.MethodName = tmpError.ErrorLocation;
                repo.LastErrorMessage = tmpError.ErrorDescription;
                repo.LogError();

                tmpReturn.Password = string.Empty;
                return tmpReturn;
            }

            if (emailAddress.Length > 0)
            {

                //First check for user that already exists.
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM Users (nolock) WHERE EmailAddress = @EmailAddress";
                cmd.Parameters.Add("@emailAddress", SqlDbType.NVarChar, 300).Value = Cryptography.Encrypt(emailAddress.ToLower());

                DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    User tmpUser = new User(ds.Tables[0].Rows[0]);

                    cmd = new SqlCommand();
                    cmd.CommandText = "SELECT * FROM PasswordResetTokens WHERE AccountID = @AccountID and ResetToken = @ResetToken";
                    cmd.Parameters.Add("@AccountID", SqlDbType.UniqueIdentifier).Value = tmpUser.AccountID;
                    cmd.Parameters.Add("@ResetToken", SqlDbType.NVarChar, 8).Value = resetToken;

                    DataSet tokenDs = _dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

                    if (tokenDs.Tables[0].Rows.Count > 0)
                    {
                        if (((DateTime)tokenDs.Tables[0].Rows[0]["TokenIssueDate"]).AddHours(24) > DateTime.Now)
                        {
                            tmpUser.Password = password;
                            tmpUser.Save();
                            tmpReturn.Password = string.Empty;
                            return tmpUser;
                        }
                        else
                        {
                            tmpError = new General.Error();
                            tmpError.ErrorDescription = "Reset Code is expired, please try reset process again.";
                            tmpError.ErrorLocation = "UserPasswordReset";
                            tmpError.ErrorNumber = "59";
                            tmpError.ErrorTitle = "Unable to Reset Password";
                            tmpReturn.Errors.Add(tmpError);

                            var paramData = new MethodParameters(emailAddress, resetToken, password, authenticationToken);
                            repo.MethodParameters = paramData.ParametersList;
                            repo.MethodName = tmpError.ErrorLocation;
                            repo.LastErrorMessage = tmpError.ErrorDescription;
                            repo.LogError();

                            tmpReturn.Password = string.Empty;
                            return tmpReturn;
                        }

                    }
                    else
                    {
                        tmpError = new General.Error();
                        tmpError.ErrorDescription = "Invalid Reset Code";
                        tmpError.ErrorLocation = "UserPasswordReset";
                        tmpError.ErrorNumber = "60";
                        tmpError.ErrorTitle = "Unable to Reset Password";
                        tmpReturn.Errors.Add(tmpError);

                        var paramData = new MethodParameters(emailAddress, resetToken, password, authenticationToken);
                        repo.MethodParameters = paramData.ParametersList;
                        repo.MethodName = tmpError.ErrorLocation;
                        repo.LastErrorMessage = tmpError.ErrorDescription;
                        repo.LogError();

                        tmpReturn.Password = string.Empty;
                        return tmpReturn;
                    }

                }
                else
                {
                    tmpError = new General.Error();
                    tmpError.ErrorDescription = "Unable to find account matching this email address";
                    tmpError.ErrorLocation = "UserPasswordReset";
                    tmpError.ErrorNumber = "61";
                    tmpError.ErrorTitle = "Unable to Reset Password";
                    tmpReturn.Errors.Add(tmpError);

                    var paramData = new MethodParameters(emailAddress, resetToken, password, authenticationToken);
                    repo.MethodParameters = paramData.ParametersList;
                    repo.MethodName = tmpError.ErrorLocation;
                    repo.LastErrorMessage = tmpError.ErrorDescription;
                    repo.LogError();

                    tmpReturn.Password = string.Empty;
                    return tmpReturn;
                }
            }
            else
            {
                tmpError = new General.Error();
                tmpError.ErrorDescription = "Please provide a valid email address";
                tmpError.ErrorLocation = "UserPasswordReset";
                tmpError.ErrorNumber = "62";
                tmpError.ErrorTitle = "Unable to Reset Password";
                tmpReturn.Errors.Add(tmpError);

                var paramData = new MethodParameters(emailAddress, resetToken, password, authenticationToken);
                repo.MethodParameters = paramData.ParametersList;
                repo.MethodName = tmpError.ErrorLocation;
                repo.LastErrorMessage = tmpError.ErrorDescription;
                repo.LogError();

                tmpReturn.Password = string.Empty;
                return tmpReturn;
            }

            //return tmpReturn;

        }



        [OperationContract]
        public General.Error UserValidateResetToken(string emailAddress, string resetToken, Guid authenticationToken)
        {
            LOLAccountRepository repo = new LOLAccountRepository(_dt);
            General.Error tmpReturn = new General.Error();

            if (!repo.ValidateToken(authenticationToken, out tmpReturn))
            {
                tmpReturn.ErrorDescription = "Invalid Authentication Token.";
                tmpReturn.ErrorNumber = "1";
                tmpReturn.ErrorLocation = "UserValidateResetToken";

                var paramData = new MethodParameters(emailAddress, resetToken, authenticationToken);
                repo.MethodParameters = paramData.ParametersList;
                repo.MethodName = tmpReturn.ErrorLocation;
                repo.LastErrorMessage = tmpReturn.ErrorDescription;
                repo.LogError();

                return tmpReturn;
            }

            if (emailAddress.Length > 0)
            {

                //First check for user that already exists.
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM Users (nolock) WHERE EmailAddress = @EmailAddress";
                cmd.Parameters.Add("@emailAddress", SqlDbType.NVarChar, 300).Value = Cryptography.Encrypt(emailAddress.ToLower());

                DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    User tmpUser = new User(ds.Tables[0].Rows[0]);

                    cmd = new SqlCommand();
                    cmd.CommandText = "SELECT * FROM PasswordResetTokens WHERE AccountID = @AccountID and ResetToken = @ResetToken";
                    cmd.Parameters.Add("@AccountID", SqlDbType.UniqueIdentifier).Value = tmpUser.AccountID;
                    cmd.Parameters.Add("@ResetToken", SqlDbType.NVarChar, 8).Value = resetToken;

                    DataSet tokenDs = _dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

                    if (tokenDs.Tables[0].Rows.Count > 0)
                    {
                        if (((DateTime)tokenDs.Tables[0].Rows[0]["TokenIssueDate"]).AddHours(24) > DateTime.Now)
                        {
                            return tmpReturn;
                        }
                        else
                        {
                            tmpReturn.ErrorDescription = "Reset Code is expired, please try reset process again.";
                            tmpReturn.ErrorLocation = "UserValidateResetToken";
                            tmpReturn.ErrorNumber = "58";
                            tmpReturn.ErrorTitle = "Unable to Reset Password";

                            var paramData = new MethodParameters(emailAddress, resetToken, authenticationToken);
                            repo.MethodParameters = paramData.ParametersList;
                            repo.MethodName = tmpReturn.ErrorLocation;
                            repo.LastErrorMessage = tmpReturn.ErrorDescription;
                            repo.LogError();
                        }

                    }
                    else
                    {
                        tmpReturn.ErrorDescription = "Invalid Reset Code";
                        tmpReturn.ErrorLocation = "UserValidateResetToken";
                        tmpReturn.ErrorNumber = "55";
                        tmpReturn.ErrorTitle = "Unable to Reset Password";

                        var paramData = new MethodParameters(emailAddress, resetToken, authenticationToken);
                        repo.MethodParameters = paramData.ParametersList;
                        repo.MethodName = tmpReturn.ErrorLocation;
                        repo.LastErrorMessage = tmpReturn.ErrorDescription;
                        repo.LogError();
                    }

                }
                else
                {
                    tmpReturn.ErrorDescription = "Unable to find account matching this email address";
                    tmpReturn.ErrorLocation = "UserValidateResetToken";
                    tmpReturn.ErrorNumber = "56";
                    tmpReturn.ErrorTitle = "Unable to Reset Password";

                    var paramData = new MethodParameters(emailAddress, resetToken, authenticationToken);
                    repo.MethodParameters = paramData.ParametersList;
                    repo.MethodName = tmpReturn.ErrorLocation;
                    repo.LastErrorMessage = tmpReturn.ErrorDescription;
                    repo.LogError();
                }
            }
            else
            {
                tmpReturn.ErrorDescription = "Please provide a valid email address";
                tmpReturn.ErrorLocation = "UserValidateResetToken";
                tmpReturn.ErrorNumber = "57";
                tmpReturn.ErrorTitle = "Unable to Reset Password";

                var paramData = new MethodParameters(emailAddress, resetToken, authenticationToken);
                repo.MethodParameters = paramData.ParametersList;
                repo.MethodName = tmpReturn.ErrorLocation;
                repo.LastErrorMessage = tmpReturn.ErrorDescription;
                repo.LogError();
            }

            return tmpReturn;

        }

        #endregion User


        #region Devices
        [OperationContract]
        public AccountOAuth AccountOAuthCreate(Guid accountID, AccountOAuth.OAuthTypes oAuthType, string oAuthID, string oAuthToken, Guid authenticationToken)
        {
            LOLAccountRepository repo = new LOLAccountRepository(_dt);
            AccountOAuth tmpReturn = new AccountOAuth();
            General.Error tmpError = repo.ValidateToken(authenticationToken, accountID);

            if (!string.IsNullOrEmpty(tmpError.ErrorNumber))
            {
                tmpReturn.Errors.Add(tmpError);
            }
            else
            {
                tmpReturn.AccountID = accountID;
                tmpReturn.OAuthType = oAuthType;
                tmpReturn.OAuthID = oAuthID;
                tmpReturn.OAuthToken = oAuthToken;
                tmpReturn.Save();
            }

            return tmpReturn;
        }

        private List<AccountOAuth> AccountOAuthList(Guid accountID)
        {
            List<AccountOAuth> tmpReturn = new List<AccountOAuth>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM AccountOAuth WHERE AccountID = @AccountID";
            cmd.Parameters.Add("@AccountID", SqlDbType.UniqueIdentifier).Value = accountID;

            DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                tmpReturn.Add(new AccountOAuth(dr));
            }
            return tmpReturn;
        }

        #endregion Devices


        #region Contacts

        [DataContract]
        public struct SearchCriteria
        {
            [DataMember]
            public string FirstName { get; set; }

            [DataMember]
            public string LastName { get; set; }

            [DataMember]
            public string EmailAddress { get; set; }

            [DataMember]
            public string OAuthID { get; set; }

            [DataMember]
            public AccountOAuth.OAuthTypes OAuthType { get; set; }
        }

        [DataContract]
        public struct SearchResult
        {
            [DataMember]
            public User ContactUser { get; set; }

            [DataMember]
            public SearchCriteria SearchedCriteria { get; set; }
        }

        [DataContract]
        public struct ContactUpdate
        {
            [DataMember]
            public List<Contact> UpdatedContacts { get; set; }

            [DataMember]
            public List<Guid> DeletedContacts { get; set; }

            [DataMember]
            public List<General.Error> Errors { get; set; }

        }


        [OperationContract]
        public ContactUpdate ContactsGetUpdated(Guid accountID, List<Guid> currentContacts, DateTime lastCallDate, Guid authenticationToken)
        {
            ContactUpdate tmpReturn = new ContactUpdate();
            tmpReturn.DeletedContacts = new List<Guid>();
            tmpReturn.UpdatedContacts = new List<Contact>();
            tmpReturn.Errors = new List<General.Error>();

            LOLAccountRepository repo = new LOLAccountRepository(_dt);
            General.Error tmpError = repo.ValidateToken(authenticationToken, accountID);
            if (!string.IsNullOrEmpty(tmpError.ErrorNumber))
            {
                tmpError.ErrorLocation = "ContactsGetUpdated";
                tmpReturn.Errors.Add(tmpError);

                var paramData = new MethodParameters(accountID, authenticationToken);
                repo.MethodParameters = paramData.ParametersList;
                repo.MethodName = tmpError.ErrorLocation;
                repo.LastErrorMessage = tmpError.ErrorDescription;
                repo.LogError();
                return tmpReturn;
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM Contacts WHERE OwnerAccountID = @AccountID";
            cmd.Parameters.Add("@AccountID", SqlDbType.UniqueIdentifier).Value = accountID;

            DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

            //Get Deleted
            foreach (Guid tmpGuid in currentContacts)
            {
                bool exists = false;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if ((Guid)dr["ContactID"] == tmpGuid)
                    {
                        exists = true;
                        break;
                    }
                }
                if (!exists)
                    tmpReturn.DeletedContacts.Add(tmpGuid);
            }

            //Get Updated & New
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                bool isAdded = false;
                if ((DateTime)dr["DateLastUpdated"] > lastCallDate)
                {
                    isAdded = true;
                    tmpReturn.UpdatedContacts.Add(new Contact(dr));
                }

                if (!isAdded)
                {
                    bool exists = false;
                    foreach (Guid tmpGuid in currentContacts)
                    {
                        if ((Guid)dr["ContactID"] == tmpGuid)
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists)
                        tmpReturn.UpdatedContacts.Add(new Contact(dr));

                }
            }


            return tmpReturn;
        }

        [OperationContract]
        public List<SearchResult> ContactsSearchList(List<SearchCriteria> searchCriteria, Guid accountID, Guid authenticationToken)
        {
            List<SearchResult> tmpReturn = new List<SearchResult>();
            LOLAccountRepository repo = new LOLAccountRepository(_dt);
            General.Error tmpError = repo.ValidateToken(authenticationToken, accountID);
            if (!string.IsNullOrEmpty(tmpError.ErrorNumber))
            {
                tmpError.ErrorLocation = "ContactsSearchList";

                SearchResult tmpResult = new SearchResult();
                tmpResult.ContactUser = new User();
                tmpResult.ContactUser.Errors = new List<General.Error>();
                tmpResult.ContactUser.Errors.Add(tmpError);
                tmpReturn.Add(tmpResult);

                var paramData = new MethodParameters(accountID, authenticationToken);
                repo.MethodParameters = paramData.ParametersList;
                repo.MethodName = tmpError.ErrorLocation;
                repo.LastErrorMessage = tmpError.ErrorDescription;
                repo.LogError();
                return tmpReturn;
            }

            if (searchCriteria == null || searchCriteria.Count == 0)
            {
                tmpError = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.SearchCriteriaMissing, "ContactsSearchList");

                SearchResult tmpResult = new SearchResult();
                tmpResult.ContactUser = new User();
                tmpResult.ContactUser.Errors = new List<General.Error>();
                tmpResult.ContactUser.Errors.Add(tmpError);
                tmpReturn.Add(tmpResult);
                return tmpReturn;
            }

            foreach (SearchCriteria sc in searchCriteria)
            {
                List<SearchResult> tmpResults = ContactsSearch(sc, accountID, authenticationToken);

                foreach (SearchResult sr in tmpResults)
                {
                    var count = tmpReturn.Where(p=>p.ContactUser.AccountID.Equals(sr.ContactUser.AccountID));
                    
                    if ( count.Count() == 0 )
                        tmpReturn.Add(sr);
                }
            }

            return tmpReturn;
        }


        [OperationContract]
        public List<SearchResult> ContactsSearch(SearchCriteria searchCriteria, Guid accountID, Guid authenticationToken)
        {

            List<SearchResult> tmpReturn = new List<SearchResult>();
            LOLAccountRepository repo = new LOLAccountRepository(_dt);
            General.Error tmpError = repo.ValidateToken(authenticationToken, accountID);
            if (!string.IsNullOrEmpty(tmpError.ErrorNumber))
            {
                //General.Error tmpError = new General.Error();
                //tmpError.ErrorDescription = "Invalid Authentication Token.";
                //tmpError.ErrorNumber = "1";
                //tmpError.ErrorLocation = "ContactsSearch";

                SearchResult tmpResult = new SearchResult();
                tmpResult.ContactUser = new User();
                tmpResult.ContactUser.Errors = new List<General.Error>();
                tmpResult.ContactUser.Errors.Add(tmpError);
                tmpReturn.Add(tmpResult);

                var paramData = new MethodParameters(authenticationToken);
                repo.MethodParameters = paramData.ParametersList;
                repo.MethodName = tmpError.ErrorLocation;
                repo.LastErrorMessage = tmpError.ErrorDescription;
                repo.LogError();
                return tmpReturn;
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM Users WHERE AccountID IN ( ";
            cmd.CommandText += "SELECT DISTINCT ";
            cmd.CommandText += "	u.AccountID ";
            cmd.CommandText += "FROM ";
            cmd.CommandText += "	Users u ";
            cmd.CommandText += "LEFT JOIN ";
            cmd.CommandText += "	Devices d ";
            cmd.CommandText += "ON ";
            cmd.CommandText += "	u.AccountID = d.AccountID ";
            cmd.CommandText += "LEFT JOIN ";
            cmd.CommandText += "	AccountOAuth da ";
            cmd.CommandText += "ON ";
            cmd.CommandText += "	da.AccountID = u.AccountID ";
            cmd.CommandText += "	AND da.OAuthTypeID = @OAuthType ";
            cmd.CommandText += "WHERE ";
            if (searchCriteria.OAuthID == null || searchCriteria.OAuthID.Length == 0)
            {
                cmd.CommandText += "	u.firstName LIKE '%' + @FirstName + '%' ";
                cmd.CommandText += "	AND u.lastName LIKE '%' + @LastName + '%' ";
                cmd.CommandText += "	AND u.EmailAddress LIKE '%' + @EmailAddress + '%' ";
            }
            else
            {
                cmd.CommandText += "	da.OAuthID = @OAuthID ";
            }
            cmd.CommandText += ") ";

            if (searchCriteria.FirstName != null)
                cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Value = searchCriteria.FirstName;
            else
                cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Value = "";

            if (searchCriteria.LastName != null)
                cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value = searchCriteria.LastName;
            else
                cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value = "";

            if (searchCriteria.EmailAddress != null)
                cmd.Parameters.Add("@EmailAddress", SqlDbType.NVarChar, 300).Value = Cryptography.Encrypt(searchCriteria.EmailAddress.ToLower());
            else
                cmd.Parameters.Add("@EmailAddress", SqlDbType.NVarChar, 300).Value = "";

            if (searchCriteria.OAuthType != null)
                cmd.Parameters.Add("@OAuthType", SqlDbType.Int).Value = (int)searchCriteria.OAuthType;
            else
                cmd.Parameters.Add("@OAuthType", SqlDbType.Int).Value = (int)AccountOAuth.OAuthTypes.LOL;

            if (searchCriteria.OAuthID != null)
                cmd.Parameters.Add("@OAuthID", SqlDbType.NVarChar, 100).Value = searchCriteria.OAuthID;
            else
                cmd.Parameters.Add("@OAuthID", SqlDbType.NVarChar, 100).Value = "";


            DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                SearchResult tmpResult = new SearchResult();
                tmpResult.ContactUser = new User(dr);
                tmpResult.SearchedCriteria = searchCriteria;
                tmpReturn.Add(tmpResult);
            }

            return tmpReturn;
        }

        [OperationContract]
        public List<Contact> ContactsGetList(Guid accountID, List<Guid> excludeAccountIDs, Guid authenticationToken)
        {
            LOLAccountRepository repo = new LOLAccountRepository(_dt);
            List<Contact> tmpReturn = new List<Contact>();
            General.Error tmpError = repo.ValidateToken(authenticationToken, accountID);
            if (!string.IsNullOrEmpty(tmpError.ErrorNumber))
            {
                tmpError.ErrorLocation = "ContactsGetList";
                tmpReturn.Add(new Contact());
                tmpReturn[0].Errors = new List<General.Error>();
                tmpReturn[0].Errors.Add(tmpError);

                var paramData = new MethodParameters(accountID, excludeAccountIDs, authenticationToken);
                repo.MethodParameters = paramData.ParametersList;
                repo.MethodName = tmpError.ErrorLocation;
                repo.LastErrorMessage = tmpError.ErrorDescription;
                repo.LogError();
                return tmpReturn;
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT c.* FROM Contacts c INNER JOIN Users u ON u.AccountID = c.OwnerAccountID WHERE u.AccountID = @AccountID ORDER BY u.lastname, u.firstname";

            cmd.Parameters.Add("@AccountID", SqlDbType.UniqueIdentifier).Value = accountID;

            DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

            if (excludeAccountIDs == null)
                excludeAccountIDs = new List<Guid>();


            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                bool includeContact = true;

                foreach (Guid excludeAccountID in excludeAccountIDs)
                {

                    if ((Guid)dr["ContactAccountID"] == excludeAccountID)
                    {
                        includeContact = false;
                        break;
                    }

                }

                if (includeContact)
                    tmpReturn.Add(new Contact(dr));

            }

            return tmpReturn;
        }

        [OperationContract]
        public Contact ContactGet(Guid contactAccountID, Guid accountID, Guid authenticationToken)
        {
            Contact tmpReturn = new Contact();
            General.Error tmpError = new General.Error();

            if (contactAccountID.Equals(Guid.Empty))
            {
                tmpError = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.ContactAccountIDMissing, "ContactGet");
                tmpReturn.Errors.Add(tmpError);
                return tmpReturn;
            }

            LOLAccountRepository repo = new LOLAccountRepository(_dt);
            tmpError = repo.ValidateToken(authenticationToken, accountID);            

            if (!string.IsNullOrEmpty(tmpError.ErrorNumber))
            {
                tmpError.ErrorLocation = "ContactGet";
                tmpReturn.Errors = new List<General.Error>();
                tmpReturn.Errors.Add(tmpError);

                var paramData = new MethodParameters(contactAccountID, authenticationToken);
                repo.MethodParameters = paramData.ParametersList;
                repo.MethodName = tmpError.ErrorLocation;
                repo.LastErrorMessage = tmpError.ErrorDescription;
                repo.LogError();
                return tmpReturn;
            }

            SqlCommand cmd = new SqlCommand();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT c.*, u1.* ")
              .Append("FROM Contacts c ")
              .Append("INNER JOIN Users u ON u.AccountID = c.OwnerAccountID ")
              .Append("inner join Users u1 on c.ContactAccountID = u1.AccountID ")
              .Append("WHERE u.AccountID = @AccountID and c.ContactAccountID = @ContactAccountID");

            //cmd.CommandText = "SELECT c.* FROM Contacts c INNER JOIN Users u ON u.AccountID = c.OwnerAccountID WHERE u.AccountID = @AccountID and c.ContactAccountID = @ContactAccountID";

            cmd.CommandText = sb.ToString();
            cmd.Parameters.Add("@ContactAccountID", SqlDbType.UniqueIdentifier).Value = contactAccountID;
            cmd.Parameters.Add("@AccountID", SqlDbType.UniqueIdentifier).Value = accountID;

            DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                tmpReturn = new Contact(dr);
            }
            else
            {
                tmpError = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.UnknownUserContactRequested, "ContactGet");
                tmpReturn.Errors.Add(tmpError);

                var paramData = new MethodParameters(contactAccountID, authenticationToken);
                repo.MethodParameters = paramData.ParametersList;
                repo.MethodName = tmpError.ErrorLocation;
                repo.LastErrorMessage = tmpError.ErrorDescription;
                repo.LogError();
            }
            return tmpReturn;
        }

        [OperationContract]
        public Contact ContactsSave(Contact contact, Guid authenticationToken)
        {
            LOLAccountRepository repo = new LOLAccountRepository(_dt);

            General.Error tmpError = repo.ValidateToken(authenticationToken, contact.OwnerAccountID);
            if (!string.IsNullOrEmpty(tmpError.ErrorNumber))
            {
                //General.Error tmpError = new General.Error();
                //tmpError.ErrorDescription = "Invalid Authentication Token.";
                //tmpError.ErrorNumber = "1";
                //tmpError.ErrorLocation = "ContactsSave";
                contact.Errors = new List<General.Error>();
                contact.Errors.Add(tmpError);

                var paramData = new MethodParameters(contact, authenticationToken);
                repo.MethodParameters = paramData.ParametersList;
                repo.MethodName = tmpError.ErrorLocation;
                repo.LastErrorMessage = tmpError.ErrorDescription;
                repo.LogError();
                return contact;
            }
            else if (contact.ContactID == new Guid("00000000-0000-0000-0000-000000000000") && repo.ContactAlreadyLinkedToUser(contact.ContactAccountID, contact.OwnerAccountID))
            {
                //General.Error tmpError = new General.Error();
                tmpError.ErrorDescription = "Contact already added";
                tmpError.ErrorNumber = "77";
                tmpError.ErrorLocation = "ContactsSave";
                contact.Errors = new List<General.Error>();
                contact.Errors.Add(tmpError);

                var paramData = new MethodParameters(contact, authenticationToken);
                repo.MethodParameters = paramData.ParametersList;
                repo.MethodName = tmpError.ErrorLocation;
                repo.LastErrorMessage = tmpError.ErrorDescription;
                repo.LogError();
                return contact;
            }

            contact.Save();
            return contact;
        }

        [OperationContract]
        public List<Contact> ContactsSaveList(List<Contact> contacts, Guid authenticationToken)
        {
            LOLAccountRepository repo = new LOLAccountRepository(_dt);
            if (!repo.ValidateToken(authenticationToken))
            {
                List<Contact> tmpReturn = new List<Contact>();
                tmpReturn.Add(new Contact());
                General.Error tmpError = new General.Error();
                tmpError.ErrorDescription = "Invalid Authentication Token.";
                tmpError.ErrorNumber = "1";
                tmpError.ErrorLocation = "ContactsSave";
                tmpReturn[0].Errors = new List<General.Error>();
                tmpReturn[0].Errors.Add(tmpError);

                var paramData = new MethodParameters(contacts, authenticationToken);
                repo.MethodParameters = paramData.ParametersList;
                repo.MethodName = tmpError.ErrorLocation;
                repo.LastErrorMessage = tmpError.ErrorDescription;
                repo.LogError();

                return tmpReturn;
            }

            foreach (Contact tmpContact in contacts)
            {
                if (tmpContact.ContactID == new Guid("00000000-0000-0000-0000-000000000000") && repo.ContactAlreadyLinkedToUser(tmpContact.ContactAccountID, tmpContact.OwnerAccountID))
                {
                    General.Error tmpError = new General.Error();
                    tmpError.ErrorDescription = "Contact already added";
                    tmpError.ErrorNumber = "77";
                    tmpError.ErrorLocation = "ContactsSave";
                    tmpContact.Errors = new List<General.Error>();
                    tmpContact.Errors.Add(tmpError);

                    var paramData = new MethodParameters(tmpContact, authenticationToken);
                    repo.MethodParameters = paramData.ParametersList;
                    repo.MethodName = tmpError.ErrorLocation;
                    repo.LastErrorMessage = tmpError.ErrorDescription;
                    repo.LogError();
                }
                else
                    tmpContact.Save();
            }
            return contacts;
        }

        [OperationContract]
        public List<General.Error> ContactsDelete(Guid contactID, Guid accountID, Guid authenticationToken)
        {
            List<General.Error> tmpReturn = new List<General.Error>();
            General.Error tmpError = new General.Error();

            if (contactID.Equals(Guid.Empty))
            {
                tmpError = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.ContactIDEmpty, "ContactsDelete");
                tmpReturn.Add(tmpError);
                return tmpReturn;
            }

            LOLAccountRepository repo = new LOLAccountRepository(_dt);
            tmpError = repo.ValidateToken(authenticationToken, accountID);

            if ( !string.IsNullOrEmpty(tmpError.ErrorNumber) )
            {
                tmpError.ErrorLocation = "ContactsDelete";
                tmpReturn.Add(tmpError);
                return tmpReturn;
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "DELETE FROM Contacts WHERE ContactID = @ContactID";
            cmd.Parameters.Add("@ContactID", SqlDbType.UniqueIdentifier).Value = contactID;
            
            int rowsAffected = this._dt.ExecuteCommand(cmd, DataTools.DataSources.LOLAccountManagement);

            if (rowsAffected == 0)
            {
                tmpError = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.ContactNotFound, "ContactsDelete");
                tmpReturn.Add(tmpError);
                return tmpReturn;
            }

            return tmpReturn;
        }


        [DataContract]
        public struct InviteEmail
        {
            [DataMember]
            public string ContactName { get; set; }

            [DataMember]
            public string EmailAddress { get; set; }

            [DataMember]
            public AccountOAuth.OAuthTypes OAuthType { get; set; }
        }

        [OperationContract]
        public void ContactsSendInviteEmail(List<InviteEmail> inviteEmails)
        {
        }




        #endregion Contacts


        #region ContentPacks

        [OperationContract]
        public List<ContentPack> ContentPacksGetByTypeAndAccountID(Guid accountID, ContentPack.ContentPackType contentPackType, List<int> excludeContentPackIDs, Guid authenticationToken)
        {
            LOLAccountRepository repo = new LOLAccountRepository(_dt);
            List<ContentPack> tmpReturn = new List<ContentPack>();

            General.Error tmpError = repo.ValidateToken(authenticationToken, accountID);
            if (!string.IsNullOrEmpty(tmpError.ErrorNumber))
            {
                tmpError.ErrorLocation = "ContentPacksGetByTypeAndAccountID";

                ContentPack tmpResult = new ContentPack();
                tmpResult.Errors = new List<General.Error>();
                tmpResult.Errors.Add(tmpError);
                tmpReturn.Add(tmpResult);

                var paramData = new MethodParameters(accountID, contentPackType, authenticationToken);
                repo.MethodParameters = paramData.ParametersList;
                repo.MethodName = tmpError.ErrorLocation;
                repo.LastErrorMessage = tmpError.ErrorDescription;
                repo.LogError();
                return tmpReturn;
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM ContentPacks WHERE ContentPackTypeID = @ContentPackType AND (ContentPackID IN (SELECT ContentPackID FROM UserContentPacks WHERE AccountID = @AccountID) OR ContentPackIsFree = 1)";
            cmd.Parameters.Add("@AccountID", SqlDbType.UniqueIdentifier).Value = accountID;
            cmd.Parameters.Add("@ContentPackType", SqlDbType.Int).Value = (int)contentPackType;

            DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                bool add = true;

                if(excludeContentPackIDs != null)
                foreach (int contentPackID in excludeContentPackIDs)
                {
                    if ((int)dr["ContentPackID"] == contentPackID)
                    {
                        add = false;
                        break;
                    }
                }
                if (add)
                    tmpReturn.Add(new ContentPack(dr));
            }

            return tmpReturn;
        }

        [OperationContract]
        public List<ContentPack> ContentPacksGetByType(Guid accountID, ContentPack.ContentPackType contentPackType, Guid authenticationToken)
        {
            LOLAccountRepository repo = new LOLAccountRepository(_dt);
            List<ContentPack> tmpReturn = new List<ContentPack>();

            General.Error tmpError = new General.Error();
            tmpError = repo.ValidateToken(authenticationToken, accountID);

            if (!string.IsNullOrEmpty(tmpError.ErrorNumber))
            {
                tmpError.ErrorLocation = "ContentPacksGetByType";

                ContentPack tmpResult = new ContentPack();
                tmpResult.Errors = new List<General.Error>();
                tmpResult.Errors.Add(tmpError);
                tmpReturn.Add(tmpResult);
                return tmpReturn;
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM ContentPacks WHERE ContentPackTypeID = @ContentPackType";
            cmd.Parameters.Add("@ContentPackType", SqlDbType.Int).Value = (int)contentPackType;

            DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                tmpReturn.Add(new ContentPack(dr));
            }

            return tmpReturn;
        }

        [OperationContract]
        public ContentPackItemDataDTO ContentPackItemGetData(int contentPackItemID, ContentPackItem.ItemSize itemSize, Guid accountID , Guid authenticationToken)
        {
            LOLAccountRepository repo = new LOLAccountRepository(_dt);
            ContentPackItemDataDTO tmpReturn = new ContentPackItemDataDTO();
            General.Error tmpError = repo.ValidateToken(authenticationToken, accountID);

            if (!string.IsNullOrEmpty(tmpError.ErrorNumber))
            {
                tmpError.ErrorLocation = "ContentPackItemGetData";
                tmpReturn.Errors.Add(tmpError);
                return tmpReturn;
            }

            if (contentPackItemID == 0)
            {
                tmpError = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.ContentPackItemIdMissing, "ContentPackItemGetData");
                tmpReturn.Errors.Add(tmpError);
                return tmpReturn;
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM ContentPackItems WHERE ContentPackItemID = @ContentPackItemID";
            cmd.Parameters.Add("@ContentPackItemID", SqlDbType.Int).Value = contentPackItemID;

            DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                var cp = new ContentPackItem(dr, itemSize);
                tmpReturn.ItemData = cp.ContentPackData;
            }
            else
            {
                tmpError = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.ContentPackItemNotFound, "ContentPackItemGetData");
                tmpReturn.Errors.Add(tmpError);
            }
            
            return tmpReturn;
        }

        [OperationContract]
        public ContentPackItemListDTO ContentPackGetPackItemsLight(int contentPackID, ContentPackItem.ItemSize itemSize, List<int> excludeContentPackItemIDs,Guid accountID,  Guid authenticationToken)
        {
            LOLAccountRepository repo = new LOLAccountRepository(_dt);
            ContentPackItemListDTO tmpReturn = new ContentPackItemListDTO();
            General.Error tmpError = repo.ValidateToken(authenticationToken, accountID);

            if (!string.IsNullOrEmpty(tmpError.ErrorNumber))
            {
                tmpError.ErrorLocation = "ContentPackGetPackItemsLight";
                tmpReturn.Errors.Add(tmpError);
                return tmpReturn;
            }

            if (contentPackID == 0)
            {
                tmpError = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.ContentPackIdMissing, "ContentPackGetPackItemsLight");
                tmpReturn.Errors.Add(tmpError);
                return tmpReturn;
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM ContentPackItems WHERE ContentPackID = @ContentPackID";
            cmd.Parameters.Add("@ContentPackID", SqlDbType.Int).Value = contentPackID;

            DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                bool add = true;

                if (excludeContentPackItemIDs != null)
                    foreach (int contentPackItemID in excludeContentPackItemIDs)
                    {
                        if ((int)dr["ContentPackItemID"] == contentPackItemID)
                        {
                            add = false;
                            break;
                        }
                    }
                if (add)
                {
                    var cp = new ContentPackItem(dr, itemSize);
                    cp.ContentPackData = new byte[0];
                    tmpReturn.contentPackItems.Add(cp);
                }
            }

            return tmpReturn;
        }

        [OperationContract]
        public List<ContentPackItem> ContentPackGetPackItems(int contentPackID, ContentPackItem.ItemSize itemSize, List<int> excludeContentPackItemIDs,Guid accountID,  Guid authenticationToken)
        {

            LOLAccountRepository repo = new LOLAccountRepository(_dt);
            List<ContentPackItem> tmpReturn = new List<ContentPackItem>();

            General.Error tmpError = new General.Error();

            if (contentPackID <= 0)
            {
                tmpError = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.ContentPackIdMissing, "ContentPackGetPackItems");
                ContentPackItem tmpResult = new ContentPackItem();
                tmpResult.Errors = new List<General.Error>();
                tmpResult.Errors.Add(tmpError);
                tmpReturn.Add(tmpResult);
                return tmpReturn;
            }

            if (itemSize == ContentPackItem.ItemSize.None)
            {
                tmpError = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.ItemSizeNone, "ContentPackGetPackItems");
                ContentPackItem tmpResult = new ContentPackItem();
                tmpResult.Errors = new List<General.Error>();
                tmpResult.Errors.Add(tmpError);
                tmpReturn.Add(tmpResult);
                return tmpReturn;
            }

            tmpError = repo.ValidateToken(authenticationToken, accountID);

            if (!string.IsNullOrEmpty(tmpError.ErrorNumber))
            {
                tmpError.ErrorLocation = "ContentPackGetPackItems";
                ContentPackItem tmpResult = new ContentPackItem();
                tmpResult.Errors = new List<General.Error>();
                tmpResult.Errors.Add(tmpError);
                tmpReturn.Add(tmpResult);
                return tmpReturn;
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM ContentPackItems WHERE ContentPackID = @ContentPackID";
            cmd.Parameters.Add("@ContentPackID", SqlDbType.Int).Value = contentPackID;

            DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                bool add = true;

                if(excludeContentPackItemIDs != null)
                foreach(int contentPackItemID in excludeContentPackItemIDs)
                {
                    if ((int)dr["ContentPackItemID"] == contentPackItemID)
                    {
                        add = false;
                        break;
                    }
                }
                if(add)
                tmpReturn.Add(new ContentPackItem(dr, itemSize));
            }

            return tmpReturn;
        }

        [OperationContract]
        public ContentPackItem ContentPackGetItem(int contentPackItemID, ContentPackItem.ItemSize itemSize,Guid accountID,  Guid authenticationToken)
        {
            ContentPackItem tmpReturn = new ContentPackItem();
            General.Error tmpError = new General.Error();

            if (contentPackItemID <= 0)
            {
                tmpError = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.ContentPackItemIdMissing, "ContentPackGetItem");
                tmpReturn.Errors.Add(tmpError);
                return tmpReturn;
            }

            if (itemSize == ContentPackItem.ItemSize.None)
            {
                tmpError = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.ItemSizeNone, "ContentPackGetItem");
                tmpReturn.Errors.Add(tmpError);
                return tmpReturn;
            }

            LOLAccountRepository repo = new LOLAccountRepository(this._dt);

            tmpError = repo.ValidateToken(authenticationToken, accountID);
            if (!string.IsNullOrEmpty(tmpError.ErrorNumber))
            {
                tmpError.ErrorLocation = "ContentPackGetItem";
                tmpReturn.Errors.Add(tmpError);
                return tmpReturn;
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM ContentPackItems WHERE ContentPackItemID = @ContentPackItemID";
            cmd.Parameters.Add("@ContentPackItemID", SqlDbType.Int).Value = contentPackItemID;

            DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)            
                tmpReturn = new ContentPackItem(ds.Tables[0].Rows[0], itemSize);
            else
            {
                tmpError = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.ContentPackItemNotFound, "ContentPackGetItem");
                tmpReturn.Errors.Add(tmpError);
            }

            return tmpReturn;
        }

        [OperationContract]
        public ContentPackItem ContentPackGetItemLight(int contentPackItemID, ContentPackItem.ItemSize itemSize,Guid accountID, Guid authenticationToken)
        {
            ContentPackItem tmpReturn = new ContentPackItem();
            General.Error tmpError = new General.Error();

            if (contentPackItemID <= 0)
            {
                tmpError = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.ContentPackItemIdMissing, "ContentPackGetItemLight");
                tmpReturn.Errors.Add(tmpError);
                return tmpReturn;
            }

            if (itemSize == ContentPackItem.ItemSize.None)
            {
                tmpError = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.ItemSizeNone, "ContentPackGetItemLight");
                tmpReturn.Errors.Add(tmpError);
                return tmpReturn;
            }

            LOLAccountRepository repo = new LOLAccountRepository(this._dt);

            tmpError = repo.ValidateToken(authenticationToken, accountID);
            if (!string.IsNullOrEmpty(tmpError.ErrorNumber))
            {
                tmpError.ErrorLocation = "ContentPackGetItemLight";
                tmpReturn.Errors.Add(tmpError);
                return tmpReturn;
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM ContentPackItems WHERE ContentPackItemID = @ContentPackItemID";
            cmd.Parameters.Add("@ContentPackItemID", SqlDbType.Int).Value = contentPackItemID;

            DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
            {
                tmpReturn = new ContentPackItem(ds.Tables[0].Rows[0], itemSize);
                //remove the icon data, that's what makes the object "light"
                tmpReturn.ContentPackItemIcon = new byte[0];
            }
            else
            {
                tmpError = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.ContentPackItemNotFound, "ContentPackGetItemLight");
                tmpReturn.Errors.Add(tmpError);
            }

            return tmpReturn;
        }

        [OperationContract]
        public ContentPackItemDataDTO ContentPackGetItemIcon(int contentPackItemID, ContentPackItem.ItemSize itemSize, Guid accountID, Guid authenticationToken)
        {
            ContentPackItemDataDTO tmpReturn = new ContentPackItemDataDTO();
            General.Error tmpError = new General.Error();

            if (contentPackItemID <= 0)
            {
                tmpError = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.ContentPackItemIdMissing, "ContentPackGetItemIcon");
                tmpReturn.Errors.Add(tmpError);
                return tmpReturn;
            }

            if (itemSize == ContentPackItem.ItemSize.None)
            {
                tmpError = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.ItemSizeNone, "ContentPackGetItemIcon");
                tmpReturn.Errors.Add(tmpError);
                return tmpReturn;
            }

            LOLAccountRepository repo = new LOLAccountRepository(this._dt);

            tmpError = repo.ValidateToken(authenticationToken, accountID);
            if (!string.IsNullOrEmpty(tmpError.ErrorNumber))
            {
                tmpError.ErrorLocation = "ContentPackGetItemIcon";
                tmpReturn.Errors.Add(tmpError);
                return tmpReturn;
            }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT c.ContentPackItemIcon FROM ContentPackItems c WHERE c.ContentPackItemID = @ContentPackItemID";
            cmd.Parameters.Add("@ContentPackItemID", SqlDbType.Int).Value = contentPackItemID;

            DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                tmpReturn.ItemData = (byte[])ds.Tables[0].Rows[0]["ContentPackItemIcon"];
            else
            {
                tmpError = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.ContentPackItemNotFound, "ContentPackGetItemIcon");
                tmpReturn.Errors.Add(tmpError);
            }

            return tmpReturn;
        }

        #endregion ContentPacks

        [OperationContract]
        public void DeleteTestData()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "DELETE FROM AuthenticationTokens WHERE AuthenticationDeviceID in ('12345678-1234-1234-1234-123456789012','12345678-1234-1234-1234-123456789013')";
            _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLAccountManagement);

            cmd.CommandText = "DELETE FROM AccountOAuth WHERE AccountID IN (SELECT AccountID FROM Devices WHERE DeviceID in ('12345678-1234-1234-1234-123456789012','12345678-1234-1234-1234-123456789013'))";
            _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLAccountManagement);

            cmd.CommandText = "DELETE FROM UserSettings WHERE AccountID IN (SELECT AccountID FROM Devices WHERE DeviceID in ('12345678-1234-1234-1234-123456789012','12345678-1234-1234-1234-123456789013'))";
            _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLAccountManagement);

            cmd.CommandText = "DELETE FROM Contacts WHERE OwnerAccountID IN (SELECT AccountID FROM Devices WHERE DeviceID in ('12345678-1234-1234-1234-123456789012','12345678-1234-1234-1234-123456789013'))";
            _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLAccountManagement);

            cmd.CommandText = "DELETE FROM Contacts WHERE ContactAccountID IN (SELECT AccountID FROM Devices WHERE DeviceID in ('12345678-1234-1234-1234-123456789012','12345678-1234-1234-1234-123456789013'))";
            _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLAccountManagement);

            cmd.CommandText = "DELETE FROM PasswordResetTokens WHERE AccountID IN (SELECT AccountID FROM Devices WHERE DeviceID in ('12345678-1234-1234-1234-123456789012','12345678-1234-1234-1234-123456789013'))";
            _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLAccountManagement);

            cmd.CommandText = "DELETE FROM Users WHERE AccountID IN (SELECT AccountID FROM Devices WHERE DeviceID in ('12345678-1234-1234-1234-123456789012','12345678-1234-1234-1234-123456789013'))";
            _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLAccountManagement);

            cmd.CommandText = "DELETE FROM Devices WHERE DeviceID in ('12345678-1234-1234-1234-123456789012','12345678-1234-1234-1234-123456789013')";
            _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLAccountManagement);


            //Added by Andrei 10/07/2012
            cmd.CommandText = "delete from Contacts where OwnerAccountID in ( select AccountID from Users where FirstName like '%Contact%' or FirstName like '%RandomFName%' )";
            _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLAccountManagement);

            cmd.CommandText = "delete from AccountOAuth where AccountID in ( select AccountID from Users where FirstName like '%RandomFName%' or FirstName like '%Contact%' )";
            _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLAccountManagement);

            cmd.CommandText = "delete from PasswordResetTokens where AccountID in (SELECT AccountID FROM Users WHERE FirstName like '%Contact%' or FirstName like '%RandomFName%')";
            _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLAccountManagement);

            cmd.CommandText = "delete from Users where FirstName like '%Contact%' or FirstName like '%RandomFName%'";
            _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLAccountManagement);
            //-----------------------------------------------------------------------------------------------------------------


            cmd.CommandText = "DELETE FROM AuthenticationTokens WHERE AuthenticationDeviceID = '12345678-1234-1234-1234-123456789099'";
            _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLAccountManagement);

            cmd.CommandText = "DELETE FROM AccountOAuth WHERE AccountID IN (SELECT AccountID FROM Devices WHERE DeviceID = '12345678-1234-1234-1234-123456789099')";
            _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLAccountManagement);

            cmd.CommandText = "DELETE FROM UserSettings WHERE AccountID IN (SELECT AccountID FROM Devices WHERE DeviceID = '12345678-1234-1234-1234-123456789099')";
            _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLAccountManagement);

            cmd.CommandText = "DELETE FROM Contacts WHERE OwnerAccountID IN (SELECT AccountID FROM Devices WHERE DeviceID = '12345678-1234-1234-1234-123456789099')";
            _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLAccountManagement);

            cmd.CommandText = "DELETE FROM Contacts WHERE ContactAccountID IN (SELECT AccountID FROM Devices WHERE DeviceID = '12345678-1234-1234-1234-123456789099')";
            _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLAccountManagement);

            cmd.CommandText = "DELETE FROM PasswordResetTokens WHERE AccountID IN (SELECT AccountID FROM Devices WHERE DeviceID = '12345678-1234-1234-1234-123456789099')";
            _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLAccountManagement);

            cmd.CommandText = "DELETE FROM Users WHERE AccountID IN (SELECT AccountID FROM Devices WHERE DeviceID = '12345678-1234-1234-1234-123456789099')";
            _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLAccountManagement);

            cmd.CommandText = "DELETE FROM Devices WHERE DeviceID = '12345678-1234-1234-1234-123456789099'";
            _dt.ExecuteCommand(cmd, DataTools.DataSources.LOLAccountManagement);


        }

        public void CheckProfileImages()
        {
            List<User> users = new List<User>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM Users";
            DataSet ds = _dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                User tmpUser = new User(dr);
                tmpUser.Save();
            }

        }



        [OperationContract]
        public string GetVersionNumber(Device.DeviceTypes deviceType)
        {

            string result = string.Empty;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT Version FROM ApplicationVersion WHERE DeviceTypeID = @DeviceTypeID";
            cmd.Parameters.Add("@DeviceTypeID", SqlDbType.Int).Value = (int)deviceType;
            var ds_ApplicationVersionData = _dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

            if (ds_ApplicationVersionData != null && ds_ApplicationVersionData.Tables.Count > 0 && ds_ApplicationVersionData.Tables[0].Rows.Count == 1)
            {
                result = ds_ApplicationVersionData.Tables[0].Rows[0][0].ToString();
            }

            return result;
        }

        [OperationContract]
        public UserImageDTO UserGetImageData(Guid accountID, Guid requestedAccountID, Guid token)
        {
            UserImageDTO tmpReturn = new UserImageDTO();   
            LOLAccountRepository repo = new LOLAccountRepository(_dt);
            General.Error tmpError = repo.ValidateToken(token, accountID);

            if (!string.IsNullOrEmpty(tmpError.ErrorNumber))
            {
                tmpError.ErrorLocation = "UserGetImageData";
                tmpReturn.Errors.Add(tmpError);
            }
            else
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT u.Picture FROM Users u WHERE u.AccountID = @AccountID";
                cmd.Parameters.Add("@AccountID", SqlDbType.UniqueIdentifier).Value = requestedAccountID;
                var ds_UserImageDto = _dt.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);

                if (ds_UserImageDto != null && ds_UserImageDto.Tables.Count > 0 && ds_UserImageDto.Tables[0].Rows.Count == 1)                
                    tmpReturn = new UserImageDTO(requestedAccountID, (byte[])ds_UserImageDto.Tables[0].Rows[0]["Picture"]);                
                else                
                    tmpReturn.Errors.Add(ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.AccountIDInvalid, "UserGetImageData"));
            }


            return tmpReturn;
        }
    }
}
