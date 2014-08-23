using System;
using System.Diagnostics;
using LOLAccountManagement.Classes;
using LOLCodeLibrary.LoggingSystem;
using System.Collections.Generic;
using LOLCodeLibrary.ErrorsManagement;
using System.Drawing;

namespace Test_Interface_Console
{
    class Test_ContentPackItemGetData : TestBase, ITestable
    {
        #region ITestable        
        public LOLConnect.LOLConnectClient _ws { get;set;}
        public ILogger Logger { get; set; }

        public override void RunTests()
        {
            this.ContentPackItemGetData_ValidInput_ShouldSucceed();
            this.ContentPackItemGetData_ContentPackIdMissing_ShouldFail();
            this.ContentPackItemGetData_TokenNotAuthenticated_ShouldFail();
            this.ContentPackItemGetData_AccountIdNotLinkedToToken_ShouldFail();
            this.ContentPackItemGetData_TokenExpired_ShouldFail();
            this.ContentPackItemGetData_TokenLoggedOut_ShouldFail();
            this.ContentPackItemGetData_TokenNotInDatabase_ShouldFail();
        }
        #endregion

        #region Constructor
        public Test_ContentPackItemGetData(LOLConnect.LOLConnectClient ws, ILogger logger)
            : base()
        {
            this._ws = ws;
            this.Logger = logger;
        }
        #endregion

        #region Test Methods     
   
        private void ContentPackItemGetData_ContentPackIdMissing_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPackItemGetData_ContentPackIdMissing_ShouldFail ...", true);
            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            string newEmail = this.GetRandomEmail();

            LOLConnect.User tmpUser = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, "", "", this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);
            LOLConnect.User loggedIn = _ws.UserLogin(RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, Guid.Empty, "", "", 0, newEmail, this.RandomPassword, token);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.ContentPackItemDataDTO result = _ws.ContentPackItemGetData(0, LOLConnect.ContentPackItem.ItemSize.None, tmpUser.AccountID, token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (result.Errors.Count == 1 && result.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.ContentPackItemIdMissing.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPackItemGetData_TokenNotAuthenticated_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPackItemGetData_TokenNotAuthenticated_ShouldFail ...", true);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.ContentPackItemDataDTO result = _ws.ContentPackItemGetData(0, LOLConnect.ContentPackItem.ItemSize.None, Guid.NewGuid(), token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (result.Errors.Count == 1 && result.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenNotLoggedIn.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPackItemGetData_TokenExpired_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPackItemGetData_TokenExpired_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPackItemGetData_TokenLoggedOut_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPackItemGetData_TokenLoggedOut_ShouldFail ...", true);
            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            string newEmail = this.GetRandomEmail();

            LOLConnect.User tmpUser = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, "", "", this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);
            LOLConnect.User loggedIn = _ws.UserLogin(RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, Guid.Empty, "", "", 0, newEmail, this.RandomPassword, token);
            _ws.UserLogOut(this.RandomDeviceID, tmpUser.AccountID, token);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.ContentPackItemDataDTO result = _ws.ContentPackItemGetData(0, LOLConnect.ContentPackItem.ItemSize.None, Guid.NewGuid(), token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (result.Errors.Count == 1 && result.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenLoggedOut.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPackItemGetData_TokenNotInDatabase_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPackItemGetData_TokenNotInDatabase_ShouldFail ...", true);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.ContentPackItemDataDTO result = _ws.ContentPackItemGetData(0, LOLConnect.ContentPackItem.ItemSize.None, Guid.NewGuid(), Guid.NewGuid());
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (result.Errors.Count == 1 && result.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenNotFound.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPackItemGetData_AccountIdNotLinkedToToken_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPackItemGetData_AccountIdNotLinkedToToken_ShouldFail ...", true);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);
            Image profileImage = Image.FromFile(this.ImageFilePath);

            string newEmail = this.GetRandomEmail();

            LOLConnect.User tmpUser = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, "", "", this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);
            LOLConnect.User loggedIn = _ws.UserLogin(RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, Guid.Empty, "", "", 0, newEmail, this.RandomPassword, token);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.ContentPackItemDataDTO result = _ws.ContentPackItemGetData(0, LOLConnect.ContentPackItem.ItemSize.None, Guid.NewGuid(), token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (result.Errors.Count == 1 && result.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenDoesNotMatchAccountID.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPackItemGetData_ValidInput_ShouldSucceed()
        {
            this.Logger.LogMessage("Testing ContentPackItemGetData_ValidInput_ShouldSucceed ...", true);
            Guid token = this._ws.AuthenticationTokenGet(this.RandomDeviceID);
            Image profileImage = Image.FromFile(this.ImageFilePath);

            string newEmail = this.GetRandomEmail();

            LOLConnect.User tmpUser = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, "", "", this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);
            LOLConnect.User loggedIn = _ws.UserLogin(RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, Guid.Empty, "", "", 0, newEmail, this.RandomPassword, token);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.ContentPackItemDataDTO result = _ws.ContentPackItemGetData(51, LOLConnect.ContentPackItem.ItemSize.Tiny, tmpUser.AccountID, token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (result.ItemData.Length > 0 && result.Errors.Count == 0)
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        #endregion
    }
}
