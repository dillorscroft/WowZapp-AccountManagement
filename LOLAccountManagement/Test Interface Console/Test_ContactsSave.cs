using System;
using System.Diagnostics;
using System.Drawing;
using LOLAccountManagement.Classes;
using LOLCodeLibrary.LoggingSystem;
using LOLCodeLibrary.ErrorsManagement;

namespace Test_Interface_Console
{
    public class Test_ContactsSave : TestBase, ITestable
    {
        //UserCreate()
        //we are testing the following scenarios :
        //1 pass a token what has already been logged in - should fail
        //2. pass a token which has expired - should fail
        //3. pass a token which is already logged out - should fail
        //4. pass a token which does not exist in the database - should fail
        
        //5. pass an accountID which is not linked to the token - should fail
        //6. pass an accountID which is linked to a valid token - should pass

        #region ITestable

        public LOLConnect.LOLConnectClient _ws { get; set; }
        public ILogger Logger { get; set; }

        public override void RunTests()
        {
            this.ContactsSave_ContactAccountIDMissing_ShouldFail();
            this.ContactsSave_ValidInput_ShouldSucceed();
            this.ContactsSave_TokenNotAuthenticated_ShouldFail();
            this.ContactsSave_TokenExpired_ShouldFail();
            this.ContactsSave_TokenLoggedOut_ShouldFail();
            this.ContactsSave_TokenNotInDatabase_ShouldFail();
            this.ContactsSave_AccountIdNotLinkedToToken_ShouldFail();
        }
        #endregion

        #region Constructor
        public Test_ContactsSave(LOLConnect.LOLConnectClient ws, ILogger logger) : base()
        {
            this._ws = ws;
            this.Logger = logger;
        }
        #endregion

        #region Test Methods

        private void ContactsSave_TokenNotAuthenticated_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsSave_TokenNotAuthenticated_ShouldFail ...", true);

            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token1 = _ws.AuthenticationTokenGet(this.RandomDeviceID);
            Guid token2 = _ws.AuthenticationTokenGet(this.RandomSecondaryDeviceID);

            var newEmail = this.GetRandomEmail();
            LOLConnect.User tmpUser1 = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1970, 2, 5), token1);
            LOLConnect.User tmpUser2 = _ws.UserCreate(this.RandomSecondaryDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, "Contact2", "Contact2", this.GetRandomEmail(), this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1974, 2, 5), token2);

            LOLConnect.Contact tmpContact = new LOLConnect.Contact();
            tmpContact.ContactAccountID = tmpUser2.AccountID;
            tmpContact.OwnerAccountID = tmpUser1.AccountID;
            tmpContact.Blocked = false;

            var elapsed = Stopwatch.StartNew();
            LOLConnect.Contact contactSaved = _ws.ContactsSave(tmpContact, token1);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (contactSaved.Errors.Count == 1 && contactSaved.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenNotLoggedIn.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsSave_TokenExpired_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsSave_TokenExpired_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsSave_TokenLoggedOut_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsSave_TokenLoggedOut_ShouldFail ...", true);
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
            
            this._ws.UserLogOut(this.RandomDeviceID, tmpUser1.AccountID, token1);

            var elapsed = Stopwatch.StartNew();
            LOLConnect.Contact contactSaved = this._ws.ContactsSave(tmpContact, token1);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (contactSaved.Errors.Count == 1 && contactSaved.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenLoggedOut.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsSave_TokenNotInDatabase_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsSave_TokenNotInDatabase_ShouldFail ...", true);

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

            var elapsed = Stopwatch.StartNew();
            LOLConnect.Contact contactSaved = _ws.ContactsSave(tmpContact, Guid.NewGuid());
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (contactSaved.Errors.Count == 1 && contactSaved.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenNotFound.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsSave_AccountIdNotLinkedToToken_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsSave_AccountIdNotLinkedToToken_ShouldFail ...", true);
            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token1 = _ws.AuthenticationTokenGet(this.RandomDeviceID);
            Guid token2 = _ws.AuthenticationTokenGet(this.RandomSecondaryDeviceID);

            var newEmail = this.GetRandomEmail();
            LOLConnect.User tmpUser1 = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1970, 2, 5), token1);
            LOLConnect.User tmpUserLoggedIn = _ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser1.AccountID, string.Empty, string.Empty, 0, newEmail, this.RandomPassword, token1);

            LOLConnect.User tmpUser2 = _ws.UserCreate(this.RandomSecondaryDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, "Contact2", "Contact2", this.GetRandomEmail(), this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1974, 2, 5), token2);

            LOLConnect.Contact tmpContact = new LOLConnect.Contact();
            tmpContact.ContactAccountID = tmpUser2.AccountID;
            tmpContact.OwnerAccountID = Guid.NewGuid();
            tmpContact.Blocked = false;

            var elapsed = Stopwatch.StartNew();
            LOLConnect.Contact contactSaved = _ws.ContactsSave(tmpContact, token1);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (contactSaved.Errors.Count == 1 && contactSaved.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenDoesNotMatchAccountID.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsSave_ValidInput_ShouldSucceed()
        {
            this.Logger.LogMessage("Testing ContactsSave_ValidInput_ShouldSucceed ...", true);
            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token1 = _ws.AuthenticationTokenGet(this.RandomDeviceID);
            Guid token2 = _ws.AuthenticationTokenGet(this.RandomSecondaryDeviceID);

            string email1 = this.GetRandomEmail();
            string email2 = this.GetRandomEmail();
            LOLConnect.User tmpUser1 = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, email1, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1970, 2, 5), token1);
            LOLConnect.User tmpUserLoggedIn = _ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser1.AccountID, string.Empty, string.Empty, 0,email1, this.RandomPassword, token1);

            LOLConnect.User tmpUser2 = _ws.UserCreate(this.RandomSecondaryDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, email2, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1974, 2, 5), token2);
            LOLConnect.User tmpUserLoggedIn2 = _ws.UserLogin(this.RandomSecondaryDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser2.AccountID, string.Empty, string.Empty, 0, email2, this.RandomPassword, token2);
            
            LOLConnect.Contact tmpContact = new LOLConnect.Contact();
            tmpContact.ContactAccountID = tmpUser2.AccountID;
            tmpContact.OwnerAccountID = tmpUser1.AccountID;
            tmpContact.Blocked = false;

            var elapsed = Stopwatch.StartNew();
            LOLConnect.Contact contactSaved = _ws.ContactsSave(tmpContact, token1);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (contactSaved.Errors.Count == 0 && !contactSaved.ContactID.Equals(Guid.Empty) && contactSaved.ContactAccountID.Equals(tmpUser2.AccountID) && contactSaved.OwnerAccountID.Equals(tmpUser1.AccountID))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsSave_ContactAccountIDMissing_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsSave_ContactAccountIDMissing_ShouldFail ...", true);
            this.Logger.LogMessage("Not implemented yet", true);
            //Image profileImage = Image.FromFile(this.ImageFilePath);
            //Guid token1 = _ws.AuthenticationTokenGet(this.RandomDeviceID);
            //Guid token2 = _ws.AuthenticationTokenGet(this.RandomSecondaryDeviceID);

            //string email1 = this.GetRandomEmail();
            //string email2 = this.GetRandomEmail();
            //LOLConnect.User tmpUser1 = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, email1, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1970, 2, 5), token1);
            //LOLConnect.User tmpUserLoggedIn = _ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser1.AccountID, string.Empty, string.Empty, 0, email1, this.RandomPassword, token1);

            //LOLConnect.User tmpUser2 = _ws.UserCreate(this.RandomSecondaryDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, email2, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1974, 2, 5), token2);
            //LOLConnect.User tmpUserLoggedIn2 = _ws.UserLogin(this.RandomSecondaryDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser2.AccountID, string.Empty, string.Empty, 0, email2, this.RandomPassword, token2);

            //LOLConnect.Contact tmpContact = new LOLConnect.Contact();
            //tmpContact.ContactAccountID = Guid.Empty;
            //tmpContact.OwnerAccountID = tmpUser1.AccountID;
            //tmpContact.Blocked = false;

            //var elapsed = Stopwatch.StartNew();
            //LOLConnect.Contact contactSaved = _ws.ContactsSave(tmpContact, token1);
            //elapsed.Stop();
            //this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            //if (contactSaved.Errors.Count == 0 && !contactSaved.ContactID.Equals(Guid.Empty) && contactSaved.ContactAccountID.Equals(tmpUser2.AccountID) && contactSaved.OwnerAccountID.Equals(tmpUser1.AccountID))
            //    this.Logger.LogMessage(this.TestSuccessMessage, true);
            //else
            //    this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }
        #endregion
    }
}
