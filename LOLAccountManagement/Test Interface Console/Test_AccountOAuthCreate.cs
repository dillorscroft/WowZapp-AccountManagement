using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using LOLAccountManagement.Classes;
using LOLCodeLibrary.LoggingSystem;
using LOLCodeLibrary.ErrorsManagement;

namespace Test_Interface_Console
{
    public sealed class Test_AccountOAuthCreate : TestBase, ITestable
    {
        //AccountOAuthCreate(Guid AccountID, LOLConnect.AccountOAuth.OAuthTypes, string OAuthID, string OAuthType, Guid token)
        //we are testing the following scenarios :
        //1. pass a token which is not authenticated yet        - should return AuthenticationTokenNotLoggedIn
        //2. pass a token which has expired                     - should return AuthenticationTokenExpired
        //3. pass a token which is already logged out           - should return AuthenticationTokenLoggedOut
        //4. pass a token which does not exist in the database  - should return AuthenticationTokenNotFound
        //5. pass an accountID which is not linked to the token - should return AuthenticationTokenDoesNotMatchAccountID
        //6. pass valid data - should return valid object and no errors

        #region ITestable
        public LOLConnect.LOLConnectClient _ws { get; set; }
        public ILogger Logger { get; set; }

        public override void RunTests()
        {
            AccountOAuthCreate_TokenNotAuthenticated_ShouldFail();
            AccountOAuthCreate_AccountIdNotLinkedToToken_ShouldFail();
            AccountOAuthCreate_TokenExpired_ShouldFail();
            AccountOAuthCreate_TokenLoggedOut_ShouldFail();
            AccountOAuthCreate_TokenNotInDatabase_ShouldFail();
            AccountOAuthCreate_ValidInput_ShouldSucceed();
        }
        #endregion

        #region Constructor
        public Test_AccountOAuthCreate(LOLConnect.LOLConnectClient ws, ILogger logger) : base()
        {
            this._ws = ws;
            this.Logger = logger;
        }
        #endregion

        #region Test Methods

        private void AccountOAuthCreate_TokenNotAuthenticated_ShouldFail()
        {
            this.Logger.LogMessage("Testing AccountOAuthCreate_TokenNotAuthenticated_ShouldFail ...", true);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            Image profileImage = Image.FromFile(this.ImageFilePath);
            string newEmail = this.GetRandomEmail();
            LOLConnect.User tmpUser = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.AccountOAuth tmpOAuth = _ws.AccountOAuthCreate(tmpUser.AccountID, LOLConnect.AccountOAuth.OAuthTypes.FaceBook, this.RandomOAuthID, this.RandomOAuthToken, token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (tmpOAuth.Errors.Count == 1 && !tmpOAuth.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenNotLoggedIn))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void AccountOAuthCreate_TokenExpired_ShouldFail()
        {
            this.Logger.LogMessage("Testing AccountOAuthCreate_TokenExpired_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void AccountOAuthCreate_TokenLoggedOut_ShouldFail()
        {
            this.Logger.LogMessage("Testing AccountOAuthCreate_TokenLoggedOut_ShouldFail ...", true);
            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);
            string newEmail = this.GetRandomEmail();
            LOLConnect.User tmpUser = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);
            LOLConnect.User tmpUserLoggedIn = _ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser.AccountID, string.Empty, string.Empty, 0, newEmail, this.RandomPassword, token);
            List<LOLConnect.GeneralError> errors = _ws.UserLogOut(this.RandomDeviceID, tmpUserLoggedIn.AccountID, token);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.AccountOAuth tmpOAuth = _ws.AccountOAuthCreate(tmpUserLoggedIn.AccountID, LOLConnect.AccountOAuth.OAuthTypes.FaceBook, this.RandomOAuthID, this.RandomOAuthToken, token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (tmpOAuth.AccountID.Equals(Guid.Empty))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void AccountOAuthCreate_TokenNotInDatabase_ShouldFail()
        {
            this.Logger.LogMessage("Testing AccountOAuthCreate_TokenNotInDatabase_ShouldFail ...", true);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.AccountOAuth tmpOAuth = _ws.AccountOAuthCreate(Guid.NewGuid(), LOLConnect.AccountOAuth.OAuthTypes.FaceBook, this.RandomOAuthID, this.RandomOAuthToken, Guid.NewGuid());
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (tmpOAuth.AccountID.Equals(Guid.Empty))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void AccountOAuthCreate_AccountIdNotLinkedToToken_ShouldFail()
        {
            this.Logger.LogMessage("Testing AccountOAuthCreate_AccountIdNotLinkedToToken_ShouldFail ...", true);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            Image profileImage = Image.FromFile(this.ImageFilePath);
            string newEmail = this.GetRandomEmail();
            var tmpUser = _ws.UserCreate(RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName , this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);
            LOLConnect.User userLoggedIn = _ws.UserLogin(RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser.AccountID, string.Empty, string.Empty, LOLConnect.AccountOAuth.OAuthTypes.LOL, newEmail, this.RandomPassword, token);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.AccountOAuth tmpOAuth = _ws.AccountOAuthCreate(Guid.NewGuid(), LOLConnect.AccountOAuth.OAuthTypes.FaceBook, this.RandomOAuthID, this.RandomOAuthToken, token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (tmpOAuth.AccountID.Equals(Guid.Empty))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void AccountOAuthCreate_ValidInput_ShouldSucceed()
        {
            this.Logger.LogMessage("Testing AccountOAuthCreate_ValidInput_ShouldSucceed ...", true);
            
            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);
            string newEmail = this.GetRandomEmail();
            LOLConnect.User tmpUser = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);
            LOLConnect.User tmpUserLoggedIn = _ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser.AccountID, string.Empty, string.Empty, 0, newEmail, this.RandomPassword, token);
            
            var elapsed = Stopwatch.StartNew();
            LOLConnect.AccountOAuth tmpOAuth = _ws.AccountOAuthCreate(tmpUserLoggedIn.AccountID, LOLConnect.AccountOAuth.OAuthTypes.FaceBook, this.RandomOAuthID, this.RandomOAuthToken, token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (tmpOAuth.Errors.Count == 0 && !tmpOAuth.AccountID.Equals(Guid.Empty))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        #endregion
    
    }
}
