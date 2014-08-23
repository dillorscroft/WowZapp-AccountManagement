using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using LOLAccountManagement.Classes;
using LOLCodeLibrary.LoggingSystem;
using LOLCodeLibrary.ErrorsManagement;

namespace Test_Interface_Console
{
    public sealed class Test_ContactsGetUpdated : TestBase, ITestable
    {
        #region ITestable
        
        public LOLConnect.LOLConnectClient _ws { get;set;}
        public ILogger Logger { get; set; }

        public override void RunTests()
        {
            this.ContactsGetUpdated_AccountIdNotLinkedToToken_ShouldFail();
            this.ContactsGetUpdated_TokenLoggedOut_ShouldFail();
            this.ContactsGetUpdated_ValidInput_ShouldSucceed();
            this.ContactsGetUpdated_TokenNotAuthenticated_ShouldFail();
            this.ContactsGetUpdated_TokenExpired_ShouldFail();
            
            this.ContactsGetUpdated_TokenNotInDatabase_ShouldFail();
            
        }
        #endregion

        #region Constructor
        public Test_ContactsGetUpdated(LOLConnect.LOLConnectClient ws, ILogger logger)
            : base()
        {
            this._ws = ws;
            this.Logger = logger;
        }
        #endregion

        #region Test Methods

        private void ContactsGetUpdated_TokenNotAuthenticated_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsGetUpdated_TokenNotAuthenticated_ShouldFail ...", true);
            Guid token1 = this._ws.AuthenticationTokenGet(this.RandomDeviceID);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.LOLConnectContactUpdate tmpContactList = this._ws.ContactsGetUpdated(Guid.NewGuid(), new List<Guid>(), DateTime.Now ,token1);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if ( tmpContactList.Errors.Count == 1 && tmpContactList.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenNotLoggedIn.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsGetUpdated_TokenExpired_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsGetUpdated_TokenExpired_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsGetUpdated_TokenLoggedOut_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsGetUpdated_TokenLoggedOut_ShouldFail ...", true);
            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token1 = this._ws.AuthenticationTokenGet(this.RandomDeviceID);
            
            var newEmail = this.GetRandomEmail();
            LOLConnect.User tmpUser1 = this._ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1970, 2, 5), token1);
            LOLConnect.User tmpUserLoggedIn = this._ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser1.AccountID, string.Empty, string.Empty, 0, newEmail, this.RandomPassword, token1);
            List<LOLConnect.GeneralError> errors = this._ws.UserLogOut(this.RandomDeviceID, tmpUser1.AccountID, token1);            

            var elapsed = Stopwatch.StartNew();
            LOLConnect.LOLConnectContactUpdate tmpContactList = this._ws.ContactsGetUpdated(tmpUser1.AccountID, new List<Guid>(), DateTime.Now, token1);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (tmpContactList.Errors.Count == 1 && tmpContactList.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenLoggedOut.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsGetUpdated_TokenNotInDatabase_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsGetUpdated_TokenNotInDatabase_ShouldFail ...", true);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.LOLConnectContactUpdate tmpContactList = this._ws.ContactsGetUpdated(Guid.NewGuid(), new List<Guid>(), DateTime.Now, Guid.NewGuid());
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (tmpContactList.Errors.Count == 1 && tmpContactList.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenNotFound.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsGetUpdated_AccountIdNotLinkedToToken_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsGetUpdated_AccountIdNotLinkedToToken_ShouldFail ...", true);
            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token1 = _ws.AuthenticationTokenGet(this.RandomDeviceID);
            string email1 = this.GetRandomEmail();

            LOLConnect.User tmpUser1 = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, email1, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1970, 2, 5), token1);
            LOLConnect.User tmpUserLoggedIn = _ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser1.AccountID, string.Empty, string.Empty, 0,email1, this.RandomPassword, token1);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.LOLConnectContactUpdate tmpContactList = this._ws.ContactsGetUpdated(Guid.NewGuid(), new List<Guid>(), DateTime.Now, token1);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (tmpContactList.Errors.Count == 1 && tmpContactList.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenDoesNotMatchAccountID.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsGetUpdated_ValidInput_ShouldSucceed()
        {
            this.Logger.LogMessage("Testing ContactsGetUpdated_ValidInput_ShouldSucceed ...", true);
            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token1 = _ws.AuthenticationTokenGet(this.RandomDeviceID);
            Guid token2 = _ws.AuthenticationTokenGet(this.RandomSecondaryDeviceID);

            string email1 = this.GetRandomEmail();
            string email2 = this.GetRandomEmail();
            LOLConnect.User tmpUser1 = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, email1, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1970, 2, 5), token1);
            LOLConnect.User tmpUserLoggedIn = _ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser1.AccountID, string.Empty, string.Empty, 0, email1, this.RandomPassword, token1);

            LOLConnect.User tmpUser2 = _ws.UserCreate(this.RandomSecondaryDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, email2, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1974, 2, 5), token2);
            LOLConnect.User tmpUserLoggedIn2 = _ws.UserLogin(this.RandomSecondaryDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser2.AccountID, string.Empty, string.Empty, 0, email2, this.RandomPassword, token2);

            LOLConnect.Contact tmpContact = new LOLConnect.Contact();
            tmpContact.ContactAccountID = tmpUser2.AccountID;
            tmpContact.OwnerAccountID = tmpUser1.AccountID;
            tmpContact.Blocked = false;

            LOLConnect.Contact contactSaved = _ws.ContactsSave(tmpContact, token1);

            List<Guid> input = new List<Guid>();
            input.Add(tmpUser2.AccountID);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.LOLConnectContactUpdate tmpContactList = this._ws.ContactsGetUpdated(tmpUser1.AccountID, input, DateTime.Now, token1);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (tmpContactList.Errors.Count == 0 && tmpContactList.UpdatedContacts.Count == 1 && tmpContactList.UpdatedContacts[0].ContactID.Equals(contactSaved.ContactID))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }
        #endregion
    }
}
