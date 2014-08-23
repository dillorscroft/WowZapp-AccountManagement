using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using LOLAccountManagement.Classes;
using LOLCodeLibrary.LoggingSystem;
using LOLAccountManagement;
using LOLCodeLibrary.ErrorsManagement;

//TO DO Missing AccountID
namespace Test_Interface_Console
{
    public sealed class Test_ContentPackGetItemIcon : TestBase, ITestable
    {
        #region ITestable        
        public LOLConnect.LOLConnectClient _ws { get;set;}
        public ILogger Logger { get; set; }

        public override void RunTests()
        {
            this.ContentPackGetItemIcon_ContentPackItemNotFound_ShouldFail();
            this.ContentPackGetItemIcon_ValidInput_ShouldSucceed();
            this.ContentPackGetItemIcon_AccountIdNotLinkedToToken_ShouldFail();
            this.ContentPackGetItemIcon_ContentPackItemIDMIssing_ShouldFail();
            this.ContentPackGetItemIcon_ItemSizeNone_ShouldFail();
            this.ContentPackGetItemIcon_TokenExpired_ShouldFail();
            this.ContentPackGetItemIcon_TokenLoggedOut_ShouldFail();
            this.ContentPackGetItemIcon_TokenNotAuthenticated_ShouldFail();
            this.ContentPackGetItemIcon_TokenNotInDatabase_ShouldFail();
        }
        #endregion

        #region Constructor
        public Test_ContentPackGetItemIcon(LOLConnect.LOLConnectClient ws, ILogger logger)
            : base()
        {
            this._ws = ws;
            this.Logger = logger;
        }
        #endregion

        #region Test Methods        

        private void ContentPackGetItemIcon_ValidInput_ShouldSucceed()
        {
            this.Logger.LogMessage("Testing ContentPackGetItemIcon_ValidInput_ShouldSucceed ...", true);

            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            string newEmail = this.GetRandomEmail();

            LOLConnect.User tmpUser = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);
            LOLConnect.User loggedIn = _ws.UserLogin(RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, Guid.Empty, string.Empty, string.Empty, LOLConnect.AccountOAuth.OAuthTypes.LOL, newEmail, this.RandomPassword, token);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.ContentPackItemDataDTO item = _ws.ContentPackGetItemIcon(51, LOLConnect.ContentPackItem.ItemSize.Tiny, tmpUser.AccountID, token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (item.ItemData.Length > 0 && item.Errors.Count == 0)
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPackGetItemIcon_ContentPackItemNotFound_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPackGetItemIcon_ContentPackItemIDNotFound_ShouldFail ...", true);

            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            string newEmail = this.GetRandomEmail();

            LOLConnect.User tmpUser = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);
            LOLConnect.User loggedIn = _ws.UserLogin(RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, Guid.Empty, string.Empty, string.Empty, LOLConnect.AccountOAuth.OAuthTypes.LOL, newEmail, this.RandomPassword, token);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.ContentPackItemDataDTO item = _ws.ContentPackGetItemIcon(5000, LOLConnect.ContentPackItem.ItemSize.Tiny, tmpUser.AccountID, token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (item.ItemData.Length == 0 && item.Errors.Count == 1 && item.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.ContentPackItemNotFound.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPackGetItemIcon_ContentPackItemIDMIssing_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPackGetItemIcon_ContentPackItemIDMIssing_ShouldFail ...", true);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.ContentPackItemDataDTO item = _ws.ContentPackGetItemIcon(0, LOLConnect.ContentPackItem.ItemSize.Tiny, Guid.NewGuid(), Guid.NewGuid());
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
        private void ContentPackGetItemIcon_TokenNotAuthenticated_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPackGetItemIcon_TokenNotAuthenticated_ShouldFail ...", true);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.ContentPackItemDataDTO item = _ws.ContentPackGetItemIcon(1, LOLConnect.ContentPackItem.ItemSize.Tiny, Guid.NewGuid(), token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (item.Errors.Count == 1 && item.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenNotLoggedIn.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPackGetItemIcon_ItemSizeNone_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPackGetItemIcon_ItemSizeNone_ShouldFail ...", true);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.ContentPackItemDataDTO item = _ws.ContentPackGetItemIcon(1, LOLConnect.ContentPackItem.ItemSize.None, Guid.NewGuid(), Guid.NewGuid());
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (item.Errors.Count == 1 && item.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.ItemSizeNone.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPackGetItemIcon_TokenExpired_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPackGetItemIcon_TokenExpired_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPackGetItemIcon_TokenLoggedOut_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPackGetItemIcon_TokenLoggedOut_ShouldFail ...", true);
            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            string newEmail = this.GetRandomEmail();

            LOLConnect.User tmpUser = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);
            LOLConnect.User loggedIn = _ws.UserLogin(RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, Guid.Empty, string.Empty, string.Empty, LOLConnect.AccountOAuth.OAuthTypes.LOL, newEmail, this.RandomPassword, token);
            List<LOLConnect.GeneralError> errors = this._ws.UserLogOut(this.RandomDeviceID, tmpUser.AccountID, token);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.ContentPackItemDataDTO item = _ws.ContentPackGetItemIcon(1, LOLConnect.ContentPackItem.ItemSize.Tiny, tmpUser.AccountID, token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (item.Errors.Count == 1 && item.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenLoggedOut.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPackGetItemIcon_TokenNotInDatabase_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPackGetItemIcon_TokenNotInDatabase_ShouldFail ...", true);

            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.ContentPackItemDataDTO item = _ws.ContentPackGetItemIcon(1, LOLConnect.ContentPackItem.ItemSize.Tiny, Guid.NewGuid(), Guid.NewGuid());
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (item.Errors.Count == 1 && item.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenNotFound.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPackGetItemIcon_AccountIdNotLinkedToToken_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPackGetItemIcon_AccountIdNotLinkedToToken_ShouldFail ...", true);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);
            Image profileImage = Image.FromFile(this.ImageFilePath);

            string newEmail = this.GetRandomEmail();

            LOLConnect.User tmpUser = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);
            LOLConnect.User loggedIn = _ws.UserLogin(RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, Guid.Empty, string.Empty, string.Empty, 0, newEmail, this.RandomPassword, token);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.ContentPackItemDataDTO item = _ws.ContentPackGetItemIcon(1, LOLConnect.ContentPackItem.ItemSize.Tiny, Guid.NewGuid(), token);
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
