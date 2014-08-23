using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using LOLAccountManagement.Classes;
using LOLCodeLibrary.LoggingSystem;
using LOLCodeLibrary.ErrorsManagement;

namespace Test_Interface_Console
{
    public class Test_ContactsGetList : TestBase, ITestable
    {
        //UserCreate()
        //we are testing the following scenarios :
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
            this.ContactsGetList_ValidInput_ShouldSucceed();
            this.ContactsGetList_TokenNotAuthenticated_ShouldFail();
            this.ContactsGetList_TokenExpired_ShouldFail();
            this.ContactsGetList_TokenLoggedOut_ShouldFail();
            this.ContactsGetList_TokenNotInDatabase_ShouldFail();
            this.ContactsGetList_AccountIdNotLinkedToToken_ShouldFail();
        }
        #endregion

        #region Constructor
        public Test_ContactsGetList(LOLConnect.LOLConnectClient ws, ILogger logger) : base()
        {
            this._ws = ws;
            this.Logger = logger;
        }
        #endregion

        #region Test Methods

        private void ContactsGetList_TokenNotAuthenticated_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsGetList_TokenNotAuthenticated_ShouldFail ...", true);

            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token1 = this._ws.AuthenticationTokenGet(this.RandomDeviceID);
            Guid token2 = this._ws.AuthenticationTokenGet(this.RandomSecondaryDeviceID);

            var newEmail = this.GetRandomEmail();
            LOLConnect.User tmpUser1 = this._ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1970, 2, 5), token1);
            LOLConnect.User tmpUser2 = this._ws.UserCreate(this.RandomSecondaryDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, "Contact2", "Contact2", this.GetRandomEmail(), this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1974, 2, 5), token2);

            LOLConnect.Contact tmpContact = new LOLConnect.Contact();
            tmpContact.ContactAccountID = tmpUser2.AccountID;
            tmpContact.OwnerAccountID = tmpUser1.AccountID;
            tmpContact.Blocked = false;

            LOLConnect.Contact contactSaved = this._ws.ContactsSave(tmpContact, token1);

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.Contact> tmpContactList = this._ws.ContactsGetList(tmpUser1.AccountID, null, token1);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (tmpContactList.Count == 1 && tmpContactList[0].Errors.Count == 1 && tmpContactList[0].Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenNotLoggedIn.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsGetList_TokenExpired_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsGetList_TokenExpired_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsGetList_TokenLoggedOut_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsGetList_TokenLoggedOut_ShouldFail ...", true);
            Guid token1 = this._ws.AuthenticationTokenGet(this.RandomDeviceID);
            Guid token2 = this._ws.AuthenticationTokenGet(this.RandomSecondaryDeviceID);
            
            Image profileImage = Image.FromFile(this.ImageFilePath);
            var newEmail = this.GetRandomEmail();
            LOLConnect.User tmpUser1 = this._ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1970, 2, 5), token1);
            LOLConnect.User tmpUserLoggedIn = this._ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser1.AccountID, string.Empty, string.Empty, 0, newEmail, this.RandomPassword, token1);

            LOLConnect.User tmpUser2 = this._ws.UserCreate(this.RandomSecondaryDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, "Contact2", "Contact2", this.GetRandomEmail(), this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1974, 2, 5), token2);

            LOLConnect.Contact tmpContact = new LOLConnect.Contact();
            tmpContact.ContactAccountID = tmpUser2.AccountID;
            tmpContact.OwnerAccountID = tmpUser1.AccountID;
            tmpContact.Blocked = false;

            LOLConnect.Contact contactSaved = this._ws.ContactsSave(tmpContact, token1);
            this._ws.UserLogOut(this.RandomDeviceID, tmpUser1.AccountID, token1);

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.Contact> tmpContactList = this._ws.ContactsGetList(tmpUser1.AccountID, null, token1);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (tmpContactList.Count == 1 && tmpContactList[0].Errors.Count == 1 && tmpContactList[0].Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenLoggedOut.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsGetList_TokenNotInDatabase_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsGetList_TokenNotInDatabase_ShouldFail ...", true);

            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token1 = this._ws.AuthenticationTokenGet(this.RandomDeviceID);
            Guid token2 = this._ws.AuthenticationTokenGet(this.RandomSecondaryDeviceID);

            var newEmail = this.GetRandomEmail();
            LOLConnect.User tmpUser1 = this._ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1970, 2, 5), token1);
            LOLConnect.User tmpUserLoggedIn = this._ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser1.AccountID, string.Empty, string.Empty, 0, newEmail, this.RandomPassword, token1);

            LOLConnect.User tmpUser2 = this._ws.UserCreate(this.RandomSecondaryDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, "Contact2", "Contact2", this.GetRandomEmail(), this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1974, 2, 5), token2);

            LOLConnect.Contact tmpContact = new LOLConnect.Contact();
            tmpContact.ContactAccountID = tmpUser2.AccountID;
            tmpContact.OwnerAccountID = tmpUser1.AccountID;
            tmpContact.Blocked = false;

            LOLConnect.Contact contactSaved = _ws.ContactsSave(tmpContact, token1);

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.Contact> tmpContactList = this._ws.ContactsGetList(tmpUser1.AccountID, null, Guid.NewGuid());
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (tmpContactList.Count == 1 && tmpContactList[0].Errors.Count == 1 && tmpContactList[0].Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenNotFound.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsGetList_AccountIdNotLinkedToToken_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsGetList_AccountIdNotLinkedToToken_ShouldFail ...", true);
            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token1 = this._ws.AuthenticationTokenGet(this.RandomDeviceID);
            Guid token2 = this._ws.AuthenticationTokenGet(this.RandomSecondaryDeviceID);

            var newEmail = this.GetRandomEmail();
            LOLConnect.User tmpUser1 = this._ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1970, 2, 5), token1);
            LOLConnect.User tmpUserLoggedIn = this._ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser1.AccountID, string.Empty, string.Empty, 0, newEmail, this.RandomPassword, token1);

            LOLConnect.User tmpUser2 = this._ws.UserCreate(this.RandomSecondaryDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, "Contact2", "Contact2", this.GetRandomEmail(), this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1974, 2, 5), token2);

            LOLConnect.Contact tmpContact = new LOLConnect.Contact();
            tmpContact.ContactAccountID = tmpUser2.AccountID;
            tmpContact.OwnerAccountID = tmpUser1.AccountID;
            tmpContact.Blocked = false;

            LOLConnect.Contact contactSaved = _ws.ContactsSave(tmpContact, token1);

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.Contact> tmpContactList = this._ws.ContactsGetList(Guid.NewGuid(), null, token1);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (tmpContactList.Count == 1 && tmpContactList[0].Errors.Count == 1 && tmpContactList[0].Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenDoesNotMatchAccountID.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsGetList_ValidInput_ShouldSucceed()
        {
            this.Logger.LogMessage("Testing ContactsGetList_ValidInput_ShouldSucceed ...", true);
            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token1 = this._ws.AuthenticationTokenGet(this.RandomDeviceID);
            Guid token2 = this._ws.AuthenticationTokenGet(this.RandomSecondaryDeviceID);

            var newEmail = this.GetRandomEmail();
            LOLConnect.User tmpUser1 = this._ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1970, 2, 5), token1);
            LOLConnect.User tmpUserLoggedIn = this._ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser1.AccountID, string.Empty, string.Empty, 0, newEmail, this.RandomPassword, token1);

            LOLConnect.User tmpUser2 = this._ws.UserCreate(this.RandomSecondaryDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, "Contact2", "Contact2", this.GetRandomEmail(), this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1974, 2, 5), token2);

            LOLConnect.Contact tmpContact = new LOLConnect.Contact();
            tmpContact.ContactAccountID = tmpUser2.AccountID;
            tmpContact.OwnerAccountID = tmpUser1.AccountID;
            tmpContact.Blocked = false;

            LOLConnect.Contact contactSaved = this._ws.ContactsSave(tmpContact, token1);

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.Contact> tmpContactList = this._ws.ContactsGetList(tmpUser1.AccountID, null, token1);
            
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (tmpContactList.Count == 1 && 
                tmpContactList[0].ContactAccountID.Equals(tmpUser2.AccountID) && 
                tmpContactList[0].OwnerAccountID.Equals(tmpUser1.AccountID) &&
                tmpContactList[0].ContactUser.Picture.Length == 0 &&
                tmpContactList[0].ContactUser.AccountID == tmpUser2.AccountID
                )
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }
        #endregion
    }
}
