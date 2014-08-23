using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using LOLAccountManagement.Classes;
using LOLCodeLibrary.LoggingSystem;
using LOLCodeLibrary.ErrorsManagement;

namespace Test_Interface_Console
{
    public class Test_ContactsSearch : TestBase, ITestable
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
        
        public LOLConnect.LOLConnectClient _ws { get;set;}
        public ILogger Logger { get; set; }        

        public override void RunTests()
        {
            this.ContactsSearch_WithValidName_ShouldSucceed();
            this.ContactsSearch_WithValidOAuth_ShouldSucceed();
            this.ContactsSearch_TokenNotAuthenticated_ShouldFail();
            this.ContactsSearch_TokenLoggedOut_ShouldFail();
            this.ContactsSearch_TokenExpired_ShouldFail();
            this.ContactsSearch_TokenNotInDatabase_ShouldFail();
            this.ContactsSearch_AccountIdNotLinkedToToken_ShouldFail();
        }
        #endregion

        #region Constructor
        public Test_ContactsSearch(LOLConnect.LOLConnectClient ws, ILogger logger) : base()
        {
            this._ws = ws;
            this.Logger = logger;
        }
        #endregion

        #region Test Methods

        private void ContactsSearch_TokenNotAuthenticated_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsSearch_TokenNotAuthenticated_ShouldFail ...", true);

            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token1 = _ws.AuthenticationTokenGet(this.RandomDeviceID);
            Guid token2 = _ws.AuthenticationTokenGet(this.RandomSecondaryDeviceID);

            var newEmail = this.GetRandomEmail();
            LOLConnect.User tmpUser1 = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1970, 2, 5), token1);
            //LOLConnect.User loggedUser = this._ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser1.AccountID, string.Empty, string.Empty, LOLConnect.AccountOAuth.OAuthTypes.FaceBook, newEmail, this.RandomPassword, token1);

            LOLConnect.User tmpUser2 = _ws.UserCreate(this.RandomSecondaryDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, "Contact2", "Contact2", this.GetRandomEmail(), this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1974, 2, 5), token2);

            LOLConnect.Contact tmpContact = new LOLConnect.Contact();
            tmpContact.ContactAccountID = tmpUser2.AccountID;
            tmpContact.OwnerAccountID = tmpUser1.AccountID;
            tmpContact.Blocked = false;

            LOLConnect.Contact contactSaved = _ws.ContactsSave(tmpContact, token1);

            LOLConnect.LOLConnectSearchCriteria tmpCriteria = new LOLConnect.LOLConnectSearchCriteria();
            tmpCriteria.FirstName = "Contact2";
            tmpCriteria.LastName = "Contact2";
            tmpCriteria.OAuthType = LOLConnect.AccountOAuth.OAuthTypes.LOL;

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.LOLConnectSearchResult> searchResults = _ws.ContactsSearch(tmpCriteria,tmpUser1.AccountID, token1);
            
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (searchResults.Count == 1 && searchResults[0].ContactUser.Errors.Count == 1 && searchResults[0].ContactUser.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenNotLoggedIn.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsSearch_TokenExpired_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsSearch_TokenExpired_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsSearch_TokenLoggedOut_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsSearch_TokenLoggedOut_ShouldFail ...", true);
            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token1 = _ws.AuthenticationTokenGet(this.RandomDeviceID);
            Guid token2 = _ws.AuthenticationTokenGet(this.RandomSecondaryDeviceID);

            var newEmail = this.GetRandomEmail();
            LOLConnect.User tmpUser1 = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1970, 2, 5), token1);
            LOLConnect.User loggedUser = this._ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser1.AccountID, string.Empty, string.Empty, LOLConnect.AccountOAuth.OAuthTypes.FaceBook, newEmail, this.RandomPassword, token1);

            LOLConnect.User tmpUser2 = _ws.UserCreate(this.RandomSecondaryDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, "Contact2", "Contact2", this.GetRandomEmail(), this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1974, 2, 5), token2);

            LOLConnect.Contact tmpContact = new LOLConnect.Contact();
            tmpContact.ContactAccountID = tmpUser2.AccountID;
            tmpContact.OwnerAccountID = tmpUser1.AccountID;
            tmpContact.Blocked = false;

            LOLConnect.Contact contactSaved = _ws.ContactsSave(tmpContact, token1);

            LOLConnect.LOLConnectSearchCriteria tmpCriteria = new LOLConnect.LOLConnectSearchCriteria();
            tmpCriteria.FirstName = "Contact2";
            tmpCriteria.LastName = "Contact2";
            tmpCriteria.OAuthType = LOLConnect.AccountOAuth.OAuthTypes.LOL;

            this._ws.UserLogOut(this.RandomDeviceID, tmpUser1.AccountID, token1);

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.LOLConnectSearchResult> searchResults = _ws.ContactsSearch(tmpCriteria, tmpUser1.AccountID, token1);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (searchResults.Count == 1 && searchResults[0].ContactUser.Errors.Count == 1 && searchResults[0].ContactUser.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenLoggedOut.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsSearch_TokenNotInDatabase_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsSearch_TokenNotInDatabase_ShouldFail ...", true);

            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token1 = _ws.AuthenticationTokenGet(this.RandomDeviceID);
            Guid token2 = _ws.AuthenticationTokenGet(this.RandomSecondaryDeviceID);

            var newEmail = this.GetRandomEmail();
            LOLConnect.User tmpUser1 = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1970, 2, 5), token1);
            LOLConnect.User loggedUser = this._ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser1.AccountID, string.Empty, string.Empty, LOLConnect.AccountOAuth.OAuthTypes.FaceBook, newEmail, this.RandomPassword, token1);

            LOLConnect.User tmpUser2 = _ws.UserCreate(this.RandomSecondaryDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, "Contact2", "Contact2", this.GetRandomEmail(), this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1974, 2, 5), token2);

            LOLConnect.Contact tmpContact = new LOLConnect.Contact();
            tmpContact.ContactAccountID = tmpUser2.AccountID;
            tmpContact.OwnerAccountID = tmpUser1.AccountID;
            tmpContact.Blocked = false;

            LOLConnect.Contact contactSaved = _ws.ContactsSave(tmpContact, token1);

            LOLConnect.LOLConnectSearchCriteria tmpCriteria = new LOLConnect.LOLConnectSearchCriteria();
            tmpCriteria.FirstName = "Contact2";
            tmpCriteria.LastName = "Contact2";
            tmpCriteria.OAuthType = LOLConnect.AccountOAuth.OAuthTypes.LOL;

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.LOLConnectSearchResult> searchResults = _ws.ContactsSearch(tmpCriteria, tmpUser1.AccountID, Guid.NewGuid());
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (searchResults.Count == 1 && searchResults[0].ContactUser.Errors.Count == 1 && searchResults[0].ContactUser.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenNotFound.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsSearch_AccountIdNotLinkedToToken_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsSearch_AccountIdNotLinkedToToken_ShouldFail ...", true);
            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token1 = _ws.AuthenticationTokenGet(this.RandomDeviceID);
            Guid token2 = _ws.AuthenticationTokenGet(this.RandomSecondaryDeviceID);

            var newEmail = this.GetRandomEmail();
            LOLConnect.User tmpUser1 = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1970, 2, 5), token1);
            LOLConnect.User loggedUser = this._ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser1.AccountID, string.Empty, string.Empty, LOLConnect.AccountOAuth.OAuthTypes.FaceBook, newEmail, this.RandomPassword, token1);

            LOLConnect.User tmpUser2 = _ws.UserCreate(this.RandomSecondaryDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, "Contact2", "Contact2", this.GetRandomEmail(), this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1974, 2, 5), token2);

            LOLConnect.Contact tmpContact = new LOLConnect.Contact();
            tmpContact.ContactAccountID = tmpUser2.AccountID;
            tmpContact.OwnerAccountID = tmpUser1.AccountID;
            tmpContact.Blocked = false;

            LOLConnect.Contact contactSaved = _ws.ContactsSave(tmpContact, token1);

            LOLConnect.LOLConnectSearchCriteria tmpCriteria = new LOLConnect.LOLConnectSearchCriteria();
            tmpCriteria.FirstName = "Contact2";
            tmpCriteria.LastName = "Contact2";
            tmpCriteria.OAuthType = LOLConnect.AccountOAuth.OAuthTypes.LOL;

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.LOLConnectSearchResult> searchResults = _ws.ContactsSearch(tmpCriteria, Guid.NewGuid(), token1);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (searchResults.Count == 1 && searchResults[0].ContactUser.Errors.Count == 1 && searchResults[0].ContactUser.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenDoesNotMatchAccountID.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsSearch_WithValidOAuth_ShouldSucceed()
        {
            this.Logger.LogMessage("Testing ContactsGetList_WithValidOAuth_ShouldSucceed ...", true);
            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token1 = _ws.AuthenticationTokenGet(this.RandomDeviceID);
            Guid token2 = _ws.AuthenticationTokenGet(this.RandomSecondaryDeviceID);

            var newEmail = this.GetRandomEmail();
            var newEmail2 = this.GetRandomEmail();
            LOLConnect.User tmpUser1 = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1970, 2, 5), token1);
            LOLConnect.User loggedUser = this._ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser1.AccountID, string.Empty, string.Empty, LOLConnect.AccountOAuth.OAuthTypes.FaceBook, newEmail, this.RandomPassword, token1);

            LOLConnect.User tmpUser2 = _ws.UserCreate(this.RandomSecondaryDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, this.RandomOAuthID, this.RandomOAuthToken, "Contact2", "Contact2", newEmail2, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1974, 2, 5), token2);
            LOLConnect.User loggedUser2 = this._ws.UserLogin(this.RandomSecondaryDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser2.AccountID, string.Empty, string.Empty, LOLConnect.AccountOAuth.OAuthTypes.FaceBook, newEmail2, this.RandomPassword, token2);

            LOLConnect.Contact tmpContact = new LOLConnect.Contact();
            tmpContact.ContactAccountID = tmpUser2.AccountID;
            tmpContact.OwnerAccountID = tmpUser1.AccountID;
            tmpContact.Blocked = false;

            LOLConnect.Contact contactSaved = _ws.ContactsSave(tmpContact, token1);

            LOLConnect.LOLConnectSearchCriteria tmpCriteria = new LOLConnect.LOLConnectSearchCriteria();
            tmpCriteria.OAuthID = this.RandomOAuthID;
            tmpCriteria.OAuthType = LOLConnect.AccountOAuth.OAuthTypes.FaceBook;

            LOLConnect.AccountOAuth tmpOAuth = _ws.AccountOAuthCreate(tmpUser2.AccountID, LOLConnect.AccountOAuth.OAuthTypes.FaceBook, this.RandomOAuthID, this.RandomOAuthToken, token2);

            var elapsed = Stopwatch.StartNew();            
            List<LOLConnect.LOLConnectSearchResult> searchResults = _ws.ContactsSearch(tmpCriteria, tmpUser1.AccountID, token1);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (searchResults.Count == 1 && searchResults[0].ContactUser.AccountID.Equals(tmpUser2.AccountID))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsSearch_WithValidName_ShouldSucceed()
        {
            this.Logger.LogMessage("Testing ContactsGetList_WithValidName_ShouldSucceed ...", true);
            
            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token1 = _ws.AuthenticationTokenGet(this.RandomDeviceID);
            Guid token2 = _ws.AuthenticationTokenGet(this.RandomSecondaryDeviceID);

            var newEmail = this.GetRandomEmail();
            var newEmail2 = this.GetRandomEmail();
            LOLConnect.User tmpUser1 = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1970, 2, 5), token1);
            LOLConnect.User loggedUser = this._ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser1.AccountID, string.Empty, string.Empty, LOLConnect.AccountOAuth.OAuthTypes.FaceBook, newEmail, this.RandomPassword, token1);

            LOLConnect.User tmpUser2 = _ws.UserCreate(this.RandomSecondaryDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, this.RandomOAuthID, this.RandomOAuthToken, "RandomContactZ19444483838", "RandomContactZ19444483838", newEmail2, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1974, 2, 5), token2);
            LOLConnect.User loggedUser2 = this._ws.UserLogin(this.RandomSecondaryDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser2.AccountID, string.Empty, string.Empty, LOLConnect.AccountOAuth.OAuthTypes.FaceBook, newEmail2, this.RandomPassword, token2);

            LOLConnect.Contact tmpContact = new LOLConnect.Contact();
            tmpContact.ContactAccountID = tmpUser2.AccountID;
            tmpContact.OwnerAccountID = tmpUser1.AccountID;
            tmpContact.Blocked = false;

            LOLConnect.Contact contactSaved = _ws.ContactsSave(tmpContact, token1);

            LOLConnect.LOLConnectSearchCriteria tmpCriteria = new LOLConnect.LOLConnectSearchCriteria();
            tmpCriteria.FirstName = "RandomContactZ19444483838";
            tmpCriteria.LastName = "RandomContactZ19444483838";

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.LOLConnectSearchResult> searchResults = _ws.ContactsSearch(tmpCriteria, tmpUser1.AccountID, token1);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (searchResults.Count == 1 && searchResults[0].ContactUser.AccountID.Equals(contactSaved.ContactAccountID))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }
        #endregion
    }
}
