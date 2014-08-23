using System;
using System.Diagnostics;
using System.Drawing;
using LOLAccountManagement.Classes;
using LOLCodeLibrary.ErrorsManagement;
using LOLCodeLibrary.LoggingSystem;

namespace Test_Interface_Console
{
    public sealed class Test_UserLogin : TestBase, ITestable
    {
        //UserLogin(string deviceID, Device.DeviceTypes deviceType, Guid accountID, string oAuthID, string oAuthToken, AccountOAuth.OAuthTypes oAuthType, string emailAddress, string password, Guid authenticationToken)
        //we are testing the following scenarios :
        //1. pass a token which is not authenticated yet       - should return AuthenticationTokenNotLoggedIn
        //2. pass a token which has expired                    - should return AuthenticationTokenExpired
        //3. pass a token which is already logged out          - should return AuthenticationTokenLoggedOut
        //4. pass a token which does not exist in the database - should return AuthenticationTokenNotFound
        //5. pass a null or empty DeviceID                     - should return DeviceIDMissing
        //6  pass non existing or empty accountID              - should return NullAccountID
        //6. pass an accountID which is not linked to the token- should return AccountIdDoesNotMatchDatabase
        //7. pass invalid OAuthToken                           - should return OAuthTokenInvalid
        //8. pass invalid OAuthID - should return 

        #region ITestable
        
        public LOLConnect.LOLConnectClient _ws { get;set;}
        public ILogger Logger { get; set; }

        public override void RunTests()
        {
            this.UserLogin_ValidInput_ShouldSucceed();
            this.UserLogin_DeviceIDEmpty_ShouldFail();
            this.UserLogin_LoginWithOAuthID_ShouldSucceed();
            this.UserLogin_TokenNotAuthenticated_ShouldFail();
            
            this.UserLogin_LoginWithOAuth_ShouldSucceed();
            this.UserLogin_LoginWithOAuthID_ShouldSucceed();
        }
        #endregion

        #region Constructor
        public Test_UserLogin(LOLConnect.LOLConnectClient ws, ILogger logger) : base()
        {
            this._ws = ws;
            this.Logger = logger;
        }
        #endregion

        #region Test Methods

        private void UserLogin_TokenNotAuthenticated_ShouldFail()
        {
            this.Logger.LogMessage("Testing UserLogin_TokenNotAuthenticated_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void UserLogin_AlreadyLoggedOut_ShouldFail()
        {
            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            string newEmail = this.GetRandomEmail();

            LOLConnect.User tmpUser = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, "", "", this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);
            LOLConnect.User loggedIn = _ws.UserLogin(RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, Guid.Empty, "", "", 0, newEmail, this.RandomPassword, token);
            _ws.UserLogOut(this.RandomDeviceID, tmpUser.AccountID, token);
            LOLConnect.User loggedInAgain = _ws.UserLogin(RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, Guid.Empty, "", "", 0, newEmail, this.RandomPassword, token);

            if (loggedInAgain.Errors.Count == 1 && loggedInAgain.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenLoggedOut))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else            
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void UserLogin_ValidInput_ShouldSucceed()
        {
            this.Logger.LogMessage("Testing UserLogin_ValidInput_ShouldSucceed ...", true);
            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);
            string newEmail = this.GetRandomEmail();

            LOLConnect.User tmpUserCreate = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, this.RandomOAuthID, this.RandomOAuthToken, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);
            var elapsed = Stopwatch.StartNew();
            LOLConnect.User tmpUser = _ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUserCreate.AccountID, "", "", LOLConnect.AccountOAuth.OAuthTypes.LOL, newEmail, this.RandomPassword, token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (tmpUser.Errors.Count == 0 && !tmpUser.AccountID.Equals(Guid.Empty))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else            
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void UserLogin_LoginWithOAuth_ShouldSucceed()
        {
            this.Logger.LogMessage("Testing UserLogin_LoginWithOAuth_ShouldSucceed ...", true);

            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            string newEmail = this.GetRandomEmail();

            LOLConnect.User tmpUser = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, "", "", this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);
            LOLConnect.User tmpUserLoggedIn = _ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, Guid.Empty, "", "", 0, newEmail, this.RandomPassword, token);
            LOLConnect.AccountOAuth tmpOAuth = _ws.AccountOAuthCreate(tmpUserLoggedIn.AccountID, LOLConnect.AccountOAuth.OAuthTypes.FaceBook, this.RandomOAuthID, this.RandomOAuthToken, token);
            
            var elapsed = Stopwatch.StartNew();
            LOLConnect.User oAuthUser = _ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, Guid.Empty, this.RandomOAuthID, this.RandomOAuthToken, LOLConnect.AccountOAuth.OAuthTypes.FaceBook, newEmail, this.RandomPassword, token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (oAuthUser.Errors.Count == 0 && !oAuthUser.AccountID.Equals(Guid.Empty))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void UserLogin_LoginWithOAuthID_ShouldSucceed()
        {
            this.Logger.LogMessage("Testing UserLogin_LoginWithOAuthID_ShouldSucceed ...", true);

            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            string newEmail = this.GetRandomEmail();

            LOLConnect.User tmpUser = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, this.RandomOAuthID, this.RandomOAuthToken, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.User oAuthUser = _ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, Guid.Empty, this.RandomOAuthID, string.Empty, LOLConnect.AccountOAuth.OAuthTypes.FaceBook, newEmail, this.RandomPassword, token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (oAuthUser.Errors.Count == 0 && oAuthUser.AccountID.Equals(tmpUser.AccountID))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void UserLogin_DeviceIDEmpty_ShouldFail()
        {
            this.Logger.LogMessage("Testing UserLogin_DeviceIDEmpty_ShouldFail ...", true);

            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            string newEmail = this.GetRandomEmail();

            LOLConnect.User tmpUser = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, this.RandomOAuthID, this.RandomOAuthToken, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.User userLoggedIn = _ws.UserLogin(string.Empty, LOLConnect.DeviceDeviceTypes.Windows, tmpUser.AccountID, this.RandomOAuthID, string.Empty, LOLConnect.AccountOAuth.OAuthTypes.FaceBook, newEmail, this.RandomPassword, token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (userLoggedIn.Errors.Count == 1 && userLoggedIn.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.DeviceIDMissing.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }
        #endregion
    }
}
