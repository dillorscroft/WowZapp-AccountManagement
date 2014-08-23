using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using LOLAccountManagement.Classes;
using LOLCodeLibrary.LoggingSystem;
using LOLAccountManagement;
using LOLCodeLibrary.ErrorsManagement;

//TO DO missing AccountID
namespace Test_Interface_Console
{
    public sealed class Test_ContentPackGetItem : TestBase, ITestable
    {
        #region ITestable        
        public LOLConnect.LOLConnectClient _ws { get;set;}
        public ILogger Logger { get; set; }

        public override void RunTests()
        {
            this.ContentPackGetItem_ContentPackItemNotFound_ShouldFail();
            this.ContentPackGetItem_ValidInput_ShouldSucceed();
            this.ContentPackGetItem_AccountIdNotLinkedToToken_ShouldFail();
            this.ContentPackGetItem_ContentPackItemIDMIssing_ShouldFail();
            this.ContentPackGetItem_ItemSizeNone_ShouldFail();
            this.ContentPackGetItem_TokenExpired_ShouldFail();
            this.ContentPackGetItem_TokenLoggedOut_ShouldFail();
            this.ContentPackGetItem_TokenNotAuthenticated_ShouldFail();
            this.ContentPackGetItem_TokenNotInDatabase_ShouldFail();
        }
        #endregion

        #region Constructor
        public Test_ContentPackGetItem(LOLConnect.LOLConnectClient ws, ILogger logger)
            : base()
        {
            this._ws = ws;
            this.Logger = logger;
        }
        #endregion

        #region Test Methods        

        private void ContentPackGetItem_ContentPackItemNotFound_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPackGetItem_ContentPackItemNotFound_ShouldFail ...", true);

            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            string newEmail = this.GetRandomEmail();

            LOLConnect.User tmpUser = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);
            LOLConnect.User loggedIn = _ws.UserLogin(RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, Guid.Empty, string.Empty, string.Empty, LOLConnect.AccountOAuth.OAuthTypes.LOL, newEmail, this.RandomPassword, token);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.ContentPackItem item = _ws.ContentPackGetItem(5000, LOLConnect.ContentPackItem.ItemSize.Tiny, tmpUser.AccountID, token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (item.ContentPackItemID == 0 && item.Errors.Count == 1 && item.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.ContentPackItemNotFound.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPackGetItem_ValidInput_ShouldSucceed()
        {
            this.Logger.LogMessage("Testing ContentPackGetItem_ValidInput_ShouldSucceed ...", true);

            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            string newEmail = this.GetRandomEmail();

            LOLConnect.User tmpUser = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);
            LOLConnect.User loggedIn = _ws.UserLogin(RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, Guid.Empty, string.Empty, string.Empty, LOLConnect.AccountOAuth.OAuthTypes.LOL, newEmail, this.RandomPassword, token);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.ContentPackItem item = _ws.ContentPackGetItem(51, LOLConnect.ContentPackItem.ItemSize.Tiny,tmpUser.AccountID, token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (item.ContentPackItemID == 51 && item.Errors.Count == 0)
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPackGetItem_ContentPackItemIDMIssing_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPackGetItem_ContentPackItemIDMIssing_ShouldFail ...", true);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.ContentPackItem item = _ws.ContentPackGetItem(0, LOLConnect.ContentPackItem.ItemSize.Tiny,Guid.NewGuid(), Guid.NewGuid());
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (item.Errors.Count == 1 && item.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.ContentPackItemIdMissing.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        //----
        private void ContentPackGetItem_TokenNotAuthenticated_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPackGetItem_TokenNotAuthenticated_ShouldFail ...", true);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.ContentPackItem item = _ws.ContentPackGetItem(1, LOLConnect.ContentPackItem.ItemSize.Tiny, Guid.NewGuid(), token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (item.Errors.Count == 1 && item.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenNotLoggedIn.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPackGetItem_ItemSizeNone_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPackGetItem_ItemSizeNone_ShouldFail ...", true);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.ContentPackItem item = _ws.ContentPackGetItem(1, LOLConnect.ContentPackItem.ItemSize.None, Guid.NewGuid(), Guid.NewGuid());
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (item.Errors.Count == 1 && item.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.ItemSizeNone.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPackGetItem_TokenExpired_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPackGetItem_TokenExpired_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPackGetItem_TokenLoggedOut_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPackGetItem_TokenLoggedOut_ShouldFail ...", true);
            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            string newEmail = this.GetRandomEmail();

            LOLConnect.User tmpUser = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);
            LOLConnect.User loggedIn = _ws.UserLogin(RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, Guid.Empty, string.Empty, string.Empty, LOLConnect.AccountOAuth.OAuthTypes.LOL, newEmail, this.RandomPassword, token);
            List<LOLConnect.GeneralError> errors = this._ws.UserLogOut(this.RandomDeviceID, tmpUser.AccountID, token);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.ContentPackItem item = _ws.ContentPackGetItem(1, LOLConnect.ContentPackItem.ItemSize.Tiny, tmpUser.AccountID, token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (item.Errors.Count == 1 && item.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenLoggedOut.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPackGetItem_TokenNotInDatabase_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPackGetItem_TokenNotInDatabase_ShouldFail ...", true);

            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.ContentPackItem item = _ws.ContentPackGetItem(1, LOLConnect.ContentPackItem.ItemSize.Tiny, Guid.NewGuid(), Guid.NewGuid());
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (item.Errors.Count == 1 && item.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenNotFound.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPackGetItem_AccountIdNotLinkedToToken_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPackGetItem_AccountIdNotLinkedToToken_ShouldFail ...", true);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);
            Image profileImage = Image.FromFile(this.ImageFilePath);

            string newEmail = this.GetRandomEmail();

            LOLConnect.User tmpUser = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);
            LOLConnect.User loggedIn = _ws.UserLogin(RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, Guid.Empty, string.Empty, string.Empty, 0, newEmail, this.RandomPassword, token);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.ContentPackItem item = _ws.ContentPackGetItem(1, LOLConnect.ContentPackItem.ItemSize.Tiny, Guid.NewGuid(), token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (item.Errors.Count == 1 && item.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenDoesNotMatchAccountID.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        #endregion
    }
}
