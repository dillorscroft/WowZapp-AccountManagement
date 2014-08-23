using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;
using LOLAccountManagement;
using LOLAccountManagement.Classes;
using LOLCodeLibrary.LoggingSystem;
using LOLCodeLibrary.ErrorsManagement;

//TO DO ContactsDelete does not take AccountID in
namespace Test_Interface_Console
{
    public sealed class Test_ContactsDelete : TestBase, ITestable
    {
        //1. pass a token which hasn't been logged in yet.
        //1. pass a token which has expired - should fail
        //3. pass a token which is already logged out - should fail
        //4. pass a token which does not exist in the database - should fail        
        //5. pass an accountID which is not linked to the token - should fail
        //6. pass an accountID which is linked to a valid token - should pass

        #region ITestable

        public LOLConnect.LOLConnectClient _ws { get;set;}
        public ILogger Logger { get; set; }

        public override void RunTests()
        {
            this.ContactsDelete_ValidInput_ShouldSucceed();
            this.ContactsDelete_AccountIdNotLinkedToToken_ShouldFail();
            this.ContactsDelete_TokenNotInDatabase_ShouldFail();
            this.ContactsDelete_TokenLoggedOut_ShouldFail();
            this.ContactsDelete_TokenNotAuthenticated_ShouldFail();
            this.ContactsDelete_TokenExpired_ShouldFail();
            this.ContactsDelete_ContactNotFound_ShouldFail();
            this.ContactsDelete_ContactIDEmpty_ShouldFail();
        }
        #endregion

        #region Constructor
        public Test_ContactsDelete(LOLConnect.LOLConnectClient ws, ILogger logger)
            : base()
        {
            this._ws = ws;
            this.Logger = logger;
        }
        #endregion

        #region Test Methods

        private void ContactsDelete_TokenNotAuthenticated_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsDelete_TokenNotAuthenticated_ShouldFail ...", true);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.GeneralError> errors = this._ws.ContactsDelete(Guid.NewGuid(), Guid.NewGuid(), token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (errors.Count == 1 && errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenNotLoggedIn.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsDelete_ContactIDEmpty_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsDelete_ContactIDEmpty_ShouldFail ...", true);

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.GeneralError> errors = this._ws.ContactsDelete(Guid.Empty, Guid.NewGuid(), Guid.Empty);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (errors.Count == 1 && errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.ContactIDEmpty.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsDelete_TokenExpired_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsDelete_TokenExpired_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsDelete_TokenLoggedOut_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsDelete_TokenLoggedOut_ShouldFail ...", true);
            Guid token1 = this._ws.AuthenticationTokenGet(this.RandomDeviceID);

            Image profileImage = Image.FromFile(this.ImageFilePath);
            var newEmail = this.GetRandomEmail();
            LOLConnect.User tmpUser1 = this._ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1970, 2, 5), token1);
            LOLConnect.User tmpUserLoggedIn = this._ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser1.AccountID, string.Empty, string.Empty, 0, newEmail, this.RandomPassword, token1);
            List<LOLConnect.GeneralError> errors = this._ws.UserLogOut(this.RandomDeviceID, tmpUser1.AccountID, token1);

            LOLConnect.Contact tmpContact = new LOLConnect.Contact();

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.GeneralError> errorsDelete = this._ws.ContactsDelete(Guid.NewGuid(), Guid.NewGuid() , token1);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (errorsDelete.Count == 1 && errorsDelete[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenLoggedOut.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsDelete_TokenNotInDatabase_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsDelete_TokenNotInDatabase_ShouldFail ...", true);
            Guid token1 = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            var newEmail = this.GetRandomEmail();
            
            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.GeneralError> tmpContactsDelete = this._ws.ContactsDelete(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (tmpContactsDelete.Count == 1 && tmpContactsDelete[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenNotFound.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsDelete_AccountIdNotLinkedToToken_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsDelete_AccountIdNotLinkedToToken_ShouldFail ...", true);
            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token1 = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            var newEmail = this.GetRandomEmail();
            LOLConnect.User tmpUser1 = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1970, 2, 5), token1);
            LOLConnect.User tmpUserLoggedIn = _ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser1.AccountID, string.Empty, string.Empty, 0, newEmail, this.RandomPassword, token1);

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.GeneralError> tmpContactsDelete = this._ws.ContactsDelete(Guid.NewGuid(), Guid.NewGuid(), token1);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (tmpContactsDelete.Count == 1 && tmpContactsDelete[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenDoesNotMatchAccountID.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsDelete_ValidInput_ShouldSucceed()
        {
            this.Logger.LogMessage("Testing ContactsDelete_ValidInput_ShouldSucceed ...", true);
            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token1 = _ws.AuthenticationTokenGet(this.RandomDeviceID);
            Guid token2 = _ws.AuthenticationTokenGet(this.RandomSecondaryDeviceID);

            var newEmail = this.GetRandomEmail();
            LOLConnect.User tmpUser1 = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1970, 2, 5), token1);
            LOLConnect.User tmpUserLoggedIn = _ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser1.AccountID, string.Empty, string.Empty, 0, newEmail, this.RandomPassword, token1);

            LOLConnect.User tmpUser2 = _ws.UserCreate(this.RandomSecondaryDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, "Contact2", "Contact2", this.GetRandomEmail(), this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1974, 2, 5), token2);

            LOLConnect.Contact tmpContact = new LOLConnect.Contact();
            tmpContact.ContactAccountID = tmpUser2.AccountID;
            tmpContact.OwnerAccountID = tmpUser1.AccountID;
            tmpContact.Blocked = false;

            LOLConnect.Contact contactSaved = _ws.ContactsSave(tmpContact, token1);

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.GeneralError> tmpContactsDelete = this._ws.ContactsDelete(contactSaved.ContactID, tmpUser1.AccountID, token1);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            LOLConnect.Contact contactSearched = this._ws.ContactGet(tmpContact.ContactAccountID, tmpUser1.AccountID, token1);

            if ( tmpContactsDelete.Count == 0 && contactSearched.ContactID.Equals(Guid.Empty))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsDelete_ContactNotFound_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsDelete_ContactNotFound_ShouldFail ...", true);
            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token1 = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            var newEmail = this.GetRandomEmail();
            LOLConnect.User tmpUser1 = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1970, 2, 5), token1);
            LOLConnect.User tmpUserLoggedIn = _ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser1.AccountID, string.Empty, string.Empty, 0, newEmail, this.RandomPassword, token1);

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.GeneralError> tmpContactsDelete = this._ws.ContactsDelete(Guid.NewGuid(), tmpUser1.AccountID, token1);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (tmpContactsDelete.Count == 1 && tmpContactsDelete[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.ContactNotFound.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }
        #endregion
    }
}
