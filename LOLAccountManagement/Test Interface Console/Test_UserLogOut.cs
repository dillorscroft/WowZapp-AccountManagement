using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using LOLAccountManagement.Classes;
using LOLCodeLibrary.LoggingSystem;
using LOLCodeLibrary.ErrorsManagement;

namespace Test_Interface_Console
{
    public class Test_UserLogOut : TestBase, ITestable
    {
        //we are testing the following scenarios :
        //1 pass a token what has already been logged in - should fail
        //2. pass a token which has expired - should fail
        //3. pass a token which is already logged out - should fail
        //4. pass a token which does not exist in the database - should fail
        
        //5. pass an accountID which is not linked to the token - should fail
        //6. pass an accountID which is linked to a valid token - should pass

        #region ITestable
        
        public LOLConnect.LOLConnectClient _ws { get;set;}
        public ILogger Logger { get; set; }

        public void RunTests()
        {
            this.UserLogOut_AccountIdNotLinkedToToken_ShouldFail();
            this.UserLogOut_DeviceIDEmpty_ShouldFail();
            this.UserLogOut_DeviceIDNull_ShouldFail();
            this.UserLogOut_TokenExpired_ShouldFail();
            this.UserLogOut_TokenLoggedOut_ShouldFail();
            this.UserLogOut_TokenNotAuthenticated_ShouldFail();
            this.UserLogOut_TokenNotInDatabase_ShouldFail();
            this.UserLogOut_ValidInput_ShouldSucceed();
        }
        #endregion

        #region Constructor
        public Test_UserLogOut(LOLConnect.LOLConnectClient ws, ILogger logger) : base()
        {
            this._ws = ws;
            this.Logger = logger;
        }
        #endregion

        #region Test Methods

        private void UserLogOut_TokenNotAuthenticated_ShouldFail()
        {
            this.Logger.LogMessage("Testing UserLogOut_TokenNotAuthenticated_ShouldFail ...", true);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.GeneralError> errors = _ws.UserLogOut(this.RandomDeviceID, Guid.NewGuid(), token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (errors.Count == 1 && errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenNotLoggedIn.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void UserLogOut_TokenExpired_ShouldFail()
        {
            this.Logger.LogMessage("Testing UserLogOut_TokenExpired_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void UserLogOut_TokenLoggedOut_ShouldFail()
        {
            this.Logger.LogMessage("Testing UserLogOut_TokenLoggedOut_ShouldFail ...", true);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            Image profileImage = Image.FromFile(this.ImageFilePath);
            var tmpUser = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, this.GetRandomEmail(), this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);

            var loggedUser = _ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser.AccountID, string.Empty, string.Empty, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, token);
            var errorsLogOut = _ws.UserLogOut(this.RandomDeviceID, tmpUser.AccountID, token);

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.GeneralError> errors = _ws.UserLogOut(this.RandomDeviceID, tmpUser.AccountID, token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (errors.Count == 1 && errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenLoggedOut.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void UserLogOut_TokenNotInDatabase_ShouldFail()
        {
            this.Logger.LogMessage("Testing UserLogOut_TokenNotInDatabase_ShouldFail ...", true);
            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.GeneralError> errors = _ws.UserLogOut(this.RandomDeviceID, Guid.NewGuid(), Guid.NewGuid());
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (errors.Count == 1 && errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenNotFound.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void UserLogOut_AccountIdNotLinkedToToken_ShouldFail()
        {
            this.Logger.LogMessage("Testing UserLogOut_AccountIdNotLinkedToToken_ShouldFail ...", true);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            Image profileImage = Image.FromFile(this.ImageFilePath);
            var tmpUser = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, this.GetRandomEmail(), this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);
            LOLConnect.User userLoggedIn = _ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser.AccountID, string.Empty, string.Empty, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, token);
            tmpUser.AccountID = Guid.NewGuid();

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.GeneralError> errors = _ws.UserLogOut(this.RandomDeviceID, Guid.NewGuid(), token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (errors.Count == 1 && errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenDoesNotMatchAccountID.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void UserLogOut_DeviceIDEmpty_ShouldFail()
        {
            this.Logger.LogMessage("Testing UserLogOut_DeviceIDEmpty_ShouldFail ...", true);
            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.GeneralError> errors = _ws.UserLogOut(string.Empty, Guid.NewGuid(), Guid.NewGuid());
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (errors.Count == 1 && errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.DeviceIDMissing.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void UserLogOut_DeviceIDNull_ShouldFail()
        {
            this.Logger.LogMessage("Testing UserLogOut_DeviceIDNull_ShouldFail ...", true);
            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.GeneralError> errors = _ws.UserLogOut(null, Guid.NewGuid(), Guid.NewGuid());
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (errors.Count == 1 && errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.DeviceIDMissing.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void UserLogOut_ValidInput_ShouldSucceed()
        {
            this.Logger.LogMessage("Testing UserLogOut_ValidInput_ShouldSucceed ...", true);
            
            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);
            var tmpUser = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, this.GetRandomEmail(), this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);
            LOLConnect.User userLoggedIn = _ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser.AccountID, string.Empty, string.Empty, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, token);
            
            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.GeneralError> results = _ws.UserLogOut(RandomDeviceID, tmpUser.AccountID, token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (results.Count == 0)
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        #endregion
    
    }
}
