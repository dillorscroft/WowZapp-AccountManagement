using System;
using System.Diagnostics;
using System.Drawing;
using LOLAccountManagement.Classes;
using LOLCodeLibrary.LoggingSystem;
using LOLCodeLibrary.ErrorsManagement;

namespace Test_Interface_Console
{
    public sealed class Test_UserCreate : TestBase, ITestable
    {
        //UserCreate()
        //we are testing the following scenarios :
        //1 pass a token what has already been logged in - should fail
        //2. pass a token which has expired - should fail

        #region ITestable

        public LOLConnect.LOLConnectClient _ws { get;set;}
        public ILogger Logger { get; set; }

        public override void RunTests()
        {
            this.UserCreate_ValidInput_ShouldSucceed();
            this.UserCreate_ExistingOAuth_ShouldLoginNotCreate();
            this.UserCreate_DeviceIDEmpty_ShouldFail();
            this.UserCreate_DeviceIDNull_ShouldFail();
            this.UserCreate_AuthenticationTokenExpired_ShouldFail();
            this.UserCreate_AuthenticationTokenLoggedOut_ShouldFail();
        }
        #endregion

        #region Constructor
        public Test_UserCreate(LOLConnect.LOLConnectClient ws, ILogger logger) : base()
        {
            this._ws = ws;
            this.Logger = logger;
        }
        #endregion

        #region Test Methods

        private void UserCreate_AuthenticationTokenLoggedOut_ShouldFail()
        {
            this.Logger.LogMessage("Testing UserCreate_AuthenticationTokenLoggedOut_ShouldFail ...", true);
            this.Logger.LogMessage("Not implemented yet", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void UserCreate_AuthenticationTokenExpired_ShouldFail()
        {
            this.Logger.LogMessage("Testing UserCreate_AuthenticationTokenExpired_ShouldFail ...", true);
            this.Logger.LogMessage("Not implemented yet", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void UserCreate_DeviceIDEmpty_ShouldFail()
        {
            this.Logger.LogMessage("Testing UserCreate_DeviceIDMIssing_ShouldFail ...", true);
            
            var elapsed = Stopwatch.StartNew();
            LOLConnect.User tmpUser = _ws.UserCreate(string.Empty, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, "", "", this.RandomFirstName, this.RandomLastName, this.GetRandomEmail(), this.RandomPassword, null, new DateTime(1978, 2, 5), Guid.NewGuid());
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (tmpUser.Errors.Count == 1 && tmpUser.AccountID.Equals(Guid.Empty) && tmpUser.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.DeviceIDMissing.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void UserCreate_DeviceIDNull_ShouldFail()
        {
            this.Logger.LogMessage("Testing UserCreate_DeviceIDNull_ShouldFail ...", true);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.User tmpUser = _ws.UserCreate(null, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, "", "", this.RandomFirstName, this.RandomLastName, this.GetRandomEmail(), this.RandomPassword, null, new DateTime(1978, 2, 5), Guid.NewGuid());
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (tmpUser.Errors.Count == 1 && tmpUser.AccountID.Equals(Guid.Empty) && tmpUser.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.DeviceIDMissing.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void UserCreate_ValidInput_ShouldSucceed()
        {
            this.Logger.LogMessage("Testing UserCreate_ValidInput_ShouldSucceed ...", true);
            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);
            var elapsed = Stopwatch.StartNew();

            LOLConnect.User tmpUser = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, "", "", this.RandomFirstName, this.RandomLastName , this.GetRandomEmail(), this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (tmpUser.Errors.Count == 0 && !tmpUser.AccountID.Equals(Guid.Empty))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void UserCreate_ExistingOAuth_ShouldLoginNotCreate()
        {
            this.Logger.LogMessage("Testing UserCreate_ValidInput_ShouldSucceed ...", true);
            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);
            string newEmail = this.GetRandomEmail();

            LOLConnect.User tmpUserCreate = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, this.RandomOAuthID, this.RandomOAuthToken, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);
            LOLConnect.User tmpUser = _ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUserCreate.AccountID, "", "", LOLConnect.AccountOAuth.OAuthTypes.LOL, newEmail, this.RandomPassword, token);
            
            this._ws.AccountOAuthCreate(tmpUserCreate.AccountID, LOLConnect.AccountOAuth.OAuthTypes.LinkedIn, "JTKubfWfxi", "eb1b60b3-fb69-43f0-af41-5b165da7dbcd", token);
            
            //get contacts from LinkedIn

            LOLConnect.LOLConnectSearchCriteria criteria = new LOLConnect.LOLConnectSearchCriteria();
            criteria.OAuthType = LOLConnect.AccountOAuth.OAuthTypes.LinkedIn;
            criteria.OAuthID = "JTKubfWfxi";
            var contacts = this._ws.ContactsSearch(criteria, tmpUserCreate.AccountID, token);

            var elapsed = Stopwatch.StartNew();

            LOLConnect.User tmpUserLinkedIn = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.iOS, LOLConnect.AccountOAuth.OAuthTypes.LinkedIn, "JTKubfWfxi", "eb1b60b3-fb69-43f0-af41-5b165da7dbcd", this.RandomFirstName, this.RandomLastName, this.GetRandomEmail(), this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (tmpUserLinkedIn.Errors.Count == 0 && tmpUserLinkedIn.AccountID.Equals(tmpUserCreate.AccountID))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        #endregion
    }
}
