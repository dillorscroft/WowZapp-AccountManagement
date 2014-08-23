using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text;
using LOLAccountManagement;
using LOLCodeLibrary.ErrorsManagement;
using LOLAccountManagement.Classes.ErrorsMgmt;
using LOLAccountManagement.Classes.DtoObjects;

namespace LOLAccountManagement.Classes
{
    public class LOLAccountRepository
    {
        private DataTools dataTools { get; set; }

        public string LastErrorMessage { get; set; }
        public List<MethodParameter> MethodParameters { get; set; }
        public string MethodName { get; set; }

        public LOLAccountRepository(DataTools dt)
        {
            this.dataTools = dt;
        }

        /// <summary>
        /// check if token and account are already linked
        /// </summary>
        /// <param name="authenticationToken"></param>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public bool CheckAuthenticationTokenLinkedToAccount(Guid authenticationToken, Guid accountID)
        {
            bool result = false;

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = new StringBuilder()
                                .Append("select * from Devices d ")
                                .Append("inner join AuthenticationTokens a on d.DeviceID = a.AuthenticationDeviceID ")
                                .Append("where a.AuthenticationToken = '@AuthenticationToken'")
                                .ToString();

            cmd.Parameters.Add("AuthenticationToken", SqlDbType.UniqueIdentifier).Value = authenticationToken;

            DataSet ds = this.dataTools.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                result = true;

            return result;
        }

        public int AssignAccountToToken(Guid authenticationToken, Guid accountID)
        {
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = new StringBuilder()
                                .Append("UPDATE  AuthenticationTokens ")
                                .Append("SET AuthenticationAccountID = @AccountID ")
                                .Append("where AuthenticationToken = @AuthenticationToken")
                                .ToString();

            cmd.Parameters.Add("@AuthenticationToken", SqlDbType.UniqueIdentifier).Value = authenticationToken;
            cmd.Parameters.Add("@AccountID", SqlDbType.UniqueIdentifier).Value = accountID;

            int rowsAffected = 0;

            try
            {
                rowsAffected = this.dataTools.ExecuteCommand(cmd, DataTools.DataSources.LOLAccountManagement);
            }
            catch (Exception ex)
            {
                this.LastErrorMessage = ex.Message;
                this.MethodName = "AssignAccountToToken";
                this.MethodParameters = new List<MethodParameter>();
                this.MethodParameters.Add(new MethodParameter("AuthenticationToken", authenticationToken.ToString()));
                this.MethodParameters.Add(new MethodParameter("AccountID", accountID.ToString()));

                LogError();

            }

            return rowsAffected;
        }

        public void LogError(List<MethodParameter> parameters, string methodName, General.Error errorObject)
        {
            this.MethodParameters = parameters;
            this.MethodName = methodName;
            this.LastErrorMessage = errorObject.ErrorDescription;
            LogError();
        }

        public void LogError()
        {
            if (!string.IsNullOrEmpty(this.LastErrorMessage) && !string.IsNullOrEmpty(this.MethodName))
            {
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = new StringBuilder()
                                    .Append("INSERT INTO ErrorLogging (MethodName,ErrorMessage,MethodParameters)")
                                    .Append("VALUES (@MethodName, @ErrorMessage, @MethodParameters)")
                                    .ToString();

                StringBuilder parametersData = new StringBuilder();
                if (this.MethodParameters != null && this.MethodParameters.Count > 0)
                {
                    foreach (MethodParameter mp in this.MethodParameters)
                    {
                        parametersData.Append("Name : ").Append(mp.Name).Append(" Value : ").Append(mp.Value).Append("|");
                    }
                }

                cmd.Parameters.Add("@MethodName", SqlDbType.VarChar).Value = this.MethodName;
                cmd.Parameters.Add("@ErrorMessage", SqlDbType.Text).Value = this.LastErrorMessage;
                cmd.Parameters.Add("@MethodParameters", SqlDbType.Text).Value = parametersData.ToString().TrimEnd('|');
                this.dataTools.ExecuteCommand(cmd, DataTools.DataSources.LOLAccountManagement);
            }
            else
                throw new Exception("Please pass in LastErrorMessage and MethodName before raising the logger");
        }

        /// <summary>
        /// 1. has the token been logged in yet?
        /// 2. is the token linked to the passed accountID ?
        /// 3. has the token expired already ?
        /// runs 2 checks : is the token linked to the right account and has the token expired yet ?
        /// if any error happens then build a proper error object and return it
        /// </summary>
        /// <param name="autenticationToken"></param>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public General.Error ValidateToken(Guid authenticationToken, Guid accountID)
        {
            General.Error tmpError = new General.Error();

            bool result = false;

            //Until Andrei has this working properly.
            //return tmpError;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = new StringBuilder()
                                .Append("SELECT * FROM AuthenticationTokens at ")
                                .Append("WHERE at.AuthenticationToken = @AuthenticationToken").ToString();

            cmd.Parameters.Add("AuthenticationToken", SqlDbType.UniqueIdentifier).Value = authenticationToken;

            DataSet ds = this.dataTools.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                DateTime lastUsed = DateTime.MinValue;
                DateTime.TryParse(dr["LastUsedDateTime"].ToString(), out lastUsed);
                Guid dbAccountID = Guid.Empty;
                Guid.TryParse(dr["AuthenticationAccountID"].ToString(), out dbAccountID);

                bool tokenLoggdeOut = dr["LoggedOut"].ToString().ToBool();

                if ( accountID == Guid.Empty )
                    ErrorManagement.SetErrorObject(SystemTypes.ErrorMessage.AccountIDNull, ref tmpError, "ValidateToken");
                else if (dbAccountID.Equals(Guid.Empty))
                    ErrorManagement.SetErrorObject(SystemTypes.ErrorMessage.AuthenticationTokenNotLoggedIn, ref tmpError, "ValidateToken");
                else if (tokenLoggdeOut)
                    ErrorManagement.SetErrorObject(SystemTypes.ErrorMessage.AuthenticationTokenLoggedOut, ref  tmpError, "ValidateToken");                
                else if (lastUsed.Equals(DateTime.MinValue))
                    ErrorManagement.SetErrorObject(SystemTypes.ErrorMessage.AuthenticationTokenLastUsedDateNotSet, ref  tmpError, "ValidateToken");
                else
                {
                    if (GenericFunctionality.GetAppSetting("EnableTokenExpiration").ToBool())
                    {
                        TimeSpan diff = (DateTime.Now - lastUsed).Duration();
                        if (diff.TotalMinutes > GenericFunctionality.GetAppSetting("TokenExpirationMinutes").ToInt())
                            ErrorManagement.SetErrorObject(SystemTypes.ErrorMessage.AuthenticationTokenExpired, ref  tmpError, "ValidateToken");
                        else
                            result = true;
                    }
                    else if (!dbAccountID.Equals(accountID))
                        ErrorManagement.SetErrorObject(SystemTypes.ErrorMessage.AuthenticationTokenDoesNotMatchAccountID, ref tmpError, "ValidateToken");
                    else
                        result = true;
                }
            }
            else
                ErrorManagement.SetErrorObject(SystemTypes.ErrorMessage.AuthenticationTokenNotFound, ref  tmpError, "ValidateToken");

            SqlCommand cmdUpdateToken = new SqlCommand();
            cmdUpdateToken.CommandText = new StringBuilder()
                                .Append("Update AuthenticationTokens ")
                                .Append("SET LastUsedDateTime = @LastUsedDateTime ")
                                .Append("WHERE AuthenticationToken = @AuthenticationToken").ToString();

            cmdUpdateToken.Parameters.Add("@AuthenticationToken", SqlDbType.UniqueIdentifier).Value = authenticationToken;
            cmdUpdateToken.Parameters.Add("@LastUsedDateTime", SqlDbType.DateTime).Value = DateTime.Now;
            this.dataTools.ExecuteCommand(cmdUpdateToken, DataTools.DataSources.LOLAccountManagement);

            return tmpError;
        }

        /// <summary>
        /// runs 2 checks : is the token linked to the right account and has the token expired yet ?
        /// if any error happens then build a proper error object and return it
        /// </summary>
        /// <param name="autenticationToken"></param>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public bool ValidateToken(Guid authenticationToken, out General.Error tmpError)
        {
            tmpError = new General.Error();
            bool result = false;

            //Until Andrei has this working properly.
            //return true;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = new StringBuilder()
                                .Append("SELECT * FROM AuthenticationTokens at ")
                                .Append("WHERE at.AuthenticationToken = @AuthenticationToken").ToString();

            cmd.Parameters.Add("AuthenticationToken", SqlDbType.UniqueIdentifier).Value = authenticationToken;

            DataSet ds = this.dataTools.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                DateTime lastUsed = DateTime.MinValue;
                DateTime.TryParse(dr["LastUsedDateTime"].ToString(), out lastUsed);
                Guid dbAccountID = Guid.Empty;
                Guid.TryParse(dr["AuthenticationAccountID"].ToString(), out dbAccountID);

                bool tokenLoggdeOut = dr["LoggedOut"].ToString().ToBool();

                if (tokenLoggdeOut)
                {
                    tmpError = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.AuthenticationTokenLoggedOut, "ValidateToken");
                }
                else if (dbAccountID.Equals(Guid.Empty))
                {
                    tmpError = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.AuthenticationTokenNotLoggedIn, "ValidateToken");
                }
                else if (lastUsed.Equals(DateTime.MinValue))
                {
                    tmpError = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.AuthenticationTokenLastUsedDateNotSet, "ValidateToken");
                }
                else
                {
                    TimeSpan diff = (DateTime.Now - lastUsed).Duration();

                    if (GenericFunctionality.GetAppSetting("EnableTokenExpiration").ToBool())
                    {
                        if (diff.TotalMinutes > GenericFunctionality.GetAppSetting("TokenExpirationMinutes").ToInt())
                        {
                            tmpError = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.AuthenticationTokenExpired, "ValidateToken");
                        }
                        else
                            result = true;
                    }
                    else
                        result = true;
                }
            }
            else
            {
                tmpError = ErrorManagement.CreateErrorObject(SystemTypes.ErrorMessage.AuthenticationTokenNotFound, "ValidateToken");
            }

            SqlCommand cmdUpdateToken = new SqlCommand();
            cmdUpdateToken.CommandText = new StringBuilder()
                                .Append("Update AuthenticationTokens ")
                                .Append("SET LastUsedDateTime = @LastUsedDateTime ")
                                .Append("WHERE AuthenticationToken = @AuthenticationToken").ToString();

            cmdUpdateToken.Parameters.Add("@AuthenticationToken", SqlDbType.UniqueIdentifier).Value = authenticationToken;
            cmdUpdateToken.Parameters.Add("@LastUsedDateTime", SqlDbType.DateTime).Value = DateTime.Now;
            this.dataTools.ExecuteCommand(cmdUpdateToken, DataTools.DataSources.LOLAccountManagement);

            return result;
        }

        /// <summary>
        /// check if the token exists and it has not expired or has been logged out yet
        /// </summary>
        /// <param name="authenticationToken"></param>
        /// <returns></returns>
        public bool ValidateToken(Guid authenticationToken)
        {
            bool result = false;

            //Until Andrei has this working properly.
            //return true;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM AuthenticationTokens at WHERE at.AuthenticationToken = @AuthenticationToken";
            cmd.Parameters.Add("AuthenticationToken", SqlDbType.UniqueIdentifier).Value = authenticationToken;

            DataSet ds = this.dataTools.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                DateTime created = DateTime.Parse(dr["AuthenticationCreatedDate"].ToString());

                bool tokenLoggdeOut = dr["LoggedOut"].ToString().ToBool();

                if (tokenLoggdeOut)
                    result = false;
                else if (GenericFunctionality.GetAppSetting("EnableTokenExpiration").ToBool())
                {
                    TimeSpan diff = (DateTime.Now - created).Duration();
                    if (diff.TotalMinutes <= GenericFunctionality.GetAppSetting("TokenExpirationMinutes").ToInt())
                        result = true;
                }
                else
                    result = true;
            }

            return result;
        }

        public bool ContactAlreadyLinkedToUser(Guid contactAccountID, Guid ownerAccountID)
        {
            bool result = false;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT c.ContactID FROM Contacts c WHERE c.OwnerAccountID = @OwnerAccountID AND c.ContactAccountID = @ContactAccountID";
            cmd.Parameters.Add("@OwnerAccountID", SqlDbType.UniqueIdentifier).Value = ownerAccountID;
            cmd.Parameters.Add("@ContactAccountID", SqlDbType.UniqueIdentifier).Value = contactAccountID;

            DataSet ds = this.dataTools.GetDataSet(cmd, DataTools.DataSources.LOLAccountManagement);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                result = true;

            return result;
        }
    }
}
