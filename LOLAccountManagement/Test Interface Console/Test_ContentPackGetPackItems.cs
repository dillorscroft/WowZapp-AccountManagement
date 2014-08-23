using System;
using System.Diagnostics;
using LOLAccountManagement.Classes;
using LOLCodeLibrary.LoggingSystem;
using System.Collections.Generic;
using LOLCodeLibrary.ErrorsManagement;
using System.Drawing;

//TO DO missing AccountID
namespace Test_Interface_Console
{
    public sealed class Test_ContentPackGetPackItems : TestBase, ITestable
    {
        #region ITestable        
        public LOLConnect.LOLConnectClient _ws { get;set;}
        public ILogger Logger { get; set; }

        public override void RunTests()
        {
            this.ContentPackGetPackItems_ItemSizeNone_ShouldFail();
            this.ContentPackGetPackItems_TokenNotInDatabase_ShouldFail();
            this.ContentPackGetPackItems_ValidInput_ShouldSucceed();
            this.ContentPackGetPackItems_TokenNotAuthenticated_ShouldFail();
            this.ContentPackGetPackItems_AccountIdNotLinkedToToken_ShouldFail();
            this.ContentPackGetPackItems_TokenExpired_ShouldFail();
            this.ContentPackGetPackItems_TokenLoggedOut_ShouldFail();
            this.ContentPackGetPackItems_ContentPackIDMissing_ShouldFail();
        }
        #endregion

        #region Constructor
        public Test_ContentPackGetPackItems(LOLConnect.LOLConnectClient ws, ILogger logger)
            : base()
        {
            this._ws = ws;
            this.Logger = logger;
        }
        #endregion

        #region Test Methods                

        private void ContentPackGetPackItems_TokenNotAuthenticated_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPackGetPackItems_TokenNotAuthenticated_ShouldFail ...", true);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.ContentPackItem> result = _ws.ContentPackGetPackItems(1, LOLConnect.ContentPackItem.ItemSize.Tiny, null, Guid.NewGuid(), token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (result.Count == 1 && result[0].Errors.Count == 1 && result[0].Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenNotLoggedIn.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPackGetPackItems_ItemSizeNone_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPackGetPackItems_ItemSizeNone_ShouldFail ...", true);

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.ContentPackItem> result = _ws.ContentPackGetPackItems(1, LOLConnect.ContentPackItem.ItemSize.None, null, Guid.NewGuid(), Guid.NewGuid());
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (result.Count == 1 && result[0].Errors.Count == 1 && result[0].Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.ItemSizeNone.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPackGetPackItems_ContentPackIDMissing_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPackGetPackItems_ContentPackIDMissing_ShouldFail ...", true);

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.ContentPackItem> result = _ws.ContentPackGetPackItems(0, LOLConnect.ContentPackItem.ItemSize.Tiny, null, Guid.NewGuid(), Guid.NewGuid());
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (result.Count == 1 && result[0].Errors.Count == 1 && result[0].Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.ContentPackIdMissing.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPackGetPackItems_TokenExpired_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPackGetPackItems_TokenExpired_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPackGetPackItems_TokenLoggedOut_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPackGetPackItems_TokenLoggedOut_ShouldFail ...", true);
            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            string newEmail = this.GetRandomEmail();

            LOLConnect.User tmpUser = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);
            LOLConnect.User loggedIn = _ws.UserLogin(RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, Guid.Empty, string.Empty, string.Empty, LOLConnect.AccountOAuth.OAuthTypes.LOL, newEmail, this.RandomPassword, token);
            List<LOLConnect.GeneralError> errors =  this._ws.UserLogOut(this.RandomDeviceID, tmpUser.AccountID, token);

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.ContentPackItem> result = _ws.ContentPackGetPackItems(1, LOLConnect.ContentPackItem.ItemSize.Tiny, null, Guid.NewGuid(), token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if ( result.Count == 1 && result[0].Errors.Count == 1 && result[0].Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenLoggedOut.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPackGetPackItems_TokenNotInDatabase_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPackGetPackItems_TokenNotInDatabase_ShouldFail ...", true);

            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.ContentPackItem> result = _ws.ContentPackGetPackItems(1, LOLConnect.ContentPackItem.ItemSize.Tiny, null, Guid.NewGuid(), Guid.NewGuid());
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (result.Count == 1 && result[0].Errors.Count == 1 && result[0].Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenNotFound.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPackGetPackItems_AccountIdNotLinkedToToken_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPackGetPackItems_AccountIdNotLinkedToToken_ShouldFail ...", true);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);
            Image profileImage = Image.FromFile(this.ImageFilePath);

            string newEmail = this.GetRandomEmail();

            LOLConnect.User tmpUser = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);
            LOLConnect.User loggedIn = _ws.UserLogin(RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, Guid.Empty, string.Empty, string.Empty, 0, newEmail, this.RandomPassword, token);

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.ContentPackItem> result = _ws.ContentPackGetPackItems(1, LOLConnect.ContentPackItem.ItemSize.Tiny, null, Guid.NewGuid(), token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if ( result.Count == 1 && result[0].Errors.Count == 1 && result[0].Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenDoesNotMatchAccountID.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPackGetPackItems_ValidInput_ShouldSucceed()
        {
            this.Logger.LogMessage("Testing ContentPackGetPackItems_ValidInput_ShouldSucceed ...", true);
            Guid token = this._ws.AuthenticationTokenGet(this.RandomDeviceID);
            Image profileImage = Image.FromFile(this.ImageFilePath);

            string newEmail = this.GetRandomEmail();

            LOLConnect.User tmpUser = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, "", "", this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);
            LOLConnect.User loggedIn = _ws.UserLogin(RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, Guid.Empty, "", "", 0, newEmail, this.RandomPassword, token);

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.ContentPackItem> result = _ws.ContentPackGetPackItems(12, LOLConnect.ContentPackItem.ItemSize.Tiny, null, tmpUser.AccountID, token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if ( result.Count > 0)
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        #endregion
    }
}
