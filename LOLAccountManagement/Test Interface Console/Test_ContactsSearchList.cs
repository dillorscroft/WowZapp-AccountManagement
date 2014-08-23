using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using LOLAccountManagement.Classes;
using LOLCodeLibrary.LoggingSystem;
using LOLCodeLibrary.ErrorsManagement;

namespace Test_Interface_Console
{
    public sealed class Test_ContactsSearchList : TestBase, ITestable
    {
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
            this.ContactsSearchList_ValidInput_ShouldSucceed();
            this.ContactsSearchList_AccountIdNotLinkedToToken_ShouldFail();
            this.ContactsSearchList_EmptyCriteria_ShouldFail();
            this.ContactsSearchList_NullCriteria_ShouldFail();
            this.ContactsSearchList_TokenExpired_ShouldFail();
            this.ContactsSearchList_TokenLoggedOut_ShouldFail();
            this.ContactsSearchList_TokenNotAuthenticated_ShouldFail();
            this.ContactsSearchList_TokenNotInDatabase_ShouldFail();
        }
        #endregion

        #region Constructor
        public Test_ContactsSearchList(LOLConnect.LOLConnectClient ws, ILogger logger)
            : base()
        {
            this._ws = ws;
            this.Logger = logger;
        }
        #endregion

        #region Test Methods

        private void ContactsSearchList_TokenNotAuthenticated_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsSearchList_TokenNotAuthenticated_ShouldFail ...", true);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.LOLConnectSearchResult> searchResults = _ws.ContactsSearchList(new List<LOLConnect.LOLConnectSearchCriteria>(), Guid.NewGuid(), token);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (searchResults.Count == 1 && searchResults[0].ContactUser.Errors.Count == 1 && searchResults[0].ContactUser.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenNotLoggedIn.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsSearchList_TokenExpired_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsSearchList_TokenExpired_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsSearchList_TokenLoggedOut_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsSearchList_TokenLoggedOut_ShouldFail ...", true);
            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token1 = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            var newEmail = this.GetRandomEmail();
            LOLConnect.User tmpUser1 = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1970, 2, 5), token1);
            LOLConnect.User loggedIn = this._ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser1.AccountID, string.Empty, string.Empty, LOLConnect.AccountOAuth.OAuthTypes.LOL, newEmail, this.RandomPassword, token1);
            List<LOLConnect.GeneralError> loggedErrors =  this._ws.UserLogOut(this.RandomDeviceID, tmpUser1.AccountID, token1);

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.LOLConnectSearchResult> searchResults = _ws.ContactsSearchList(new List<LOLConnect.LOLConnectSearchCriteria>() , tmpUser1.AccountID, token1);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (searchResults.Count == 1 && searchResults[0].ContactUser.Errors.Count == 1 && searchResults[0].ContactUser.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenLoggedOut.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsSearchList_TokenNotInDatabase_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsSearchList_TokenNotInDatabase_ShouldFail ...", true);

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.LOLConnectSearchResult> searchResults = _ws.ContactsSearchList(new List<LOLConnect.LOLConnectSearchCriteria>(), Guid.NewGuid(), Guid.NewGuid());
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (searchResults.Count == 1 && searchResults[0].ContactUser.Errors.Count == 1 && searchResults[0].ContactUser.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenNotFound.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsSearchList_AccountIdNotLinkedToToken_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsSearchList_AccountIdNotLinkedToToken_ShouldFail ...", true);
            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token1 = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            var newEmail = this.GetRandomEmail();
            LOLConnect.User tmpUser1 = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1970, 2, 5), token1);
            LOLConnect.User loggedUser = this._ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser1.AccountID, string.Empty, string.Empty, LOLConnect.AccountOAuth.OAuthTypes.FaceBook, newEmail, this.RandomPassword, token1);

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.LOLConnectSearchResult> searchResults = _ws.ContactsSearchList(new List<LOLConnect.LOLConnectSearchCriteria>(), Guid.NewGuid(), token1);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (searchResults.Count == 1 && searchResults[0].ContactUser.Errors.Count == 1 && searchResults[0].ContactUser.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenDoesNotMatchAccountID.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsSearchList_EmptyCriteria_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsSearchList_EmptyCriteria_ShouldFail ...", true);

            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token1 = _ws.AuthenticationTokenGet(this.RandomDeviceID);
            var newEmail = this.GetRandomEmail();
            
            LOLConnect.User tmpUser1 = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1970, 2, 5), token1);
            LOLConnect.User loggedUser = this._ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser1.AccountID, string.Empty, string.Empty, LOLConnect.AccountOAuth.OAuthTypes.FaceBook, newEmail, this.RandomPassword, token1);

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.LOLConnectSearchResult> searchResults = _ws.ContactsSearchList(new List<LOLConnect.LOLConnectSearchCriteria>(), tmpUser1.AccountID, token1);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (searchResults[0].ContactUser.Errors.Count == 1 && searchResults[0].ContactUser.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.SearchCriteriaMissing.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsSearchList_NullCriteria_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContactsSearchList_NullCriteria_ShouldFail ...", true);

            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token1 = _ws.AuthenticationTokenGet(this.RandomDeviceID);
            var newEmail = this.GetRandomEmail();

            LOLConnect.User tmpUser1 = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1970, 2, 5), token1);
            LOLConnect.User loggedUser = this._ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser1.AccountID, string.Empty, string.Empty, LOLConnect.AccountOAuth.OAuthTypes.FaceBook, newEmail, this.RandomPassword, token1);

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.LOLConnectSearchResult> searchResults = _ws.ContactsSearchList(new List<LOLConnect.LOLConnectSearchCriteria>(), tmpUser1.AccountID, token1);
            elapsed.Stop();
            this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            if (searchResults[0].ContactUser.Errors.Count == 1 && searchResults[0].ContactUser.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.SearchCriteriaMissing.ToString()))
                this.Logger.LogMessage(this.TestSuccessMessage, true);
            else
                this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContactsSearchList_ValidInput_ShouldSucceed()
        {
            this.Logger.LogMessage("Testing ContactsSearchList_ValidInput_ShouldSucceed ...", true);

            Image profileImage = Image.FromFile(this.ImageFilePath);
            Guid token1 = _ws.AuthenticationTokenGet(this.RandomDeviceID);
            Guid token2 = _ws.AuthenticationTokenGet(this.RandomSecondaryDeviceID);

            var newEmail = this.GetRandomEmail();
            var newEmail2 = this.GetRandomEmail();
            LOLConnect.User tmpUser1 = this._ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, string.Empty, string.Empty, this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1970, 2, 5), token1);
            LOLConnect.User loggedUser = this._ws.UserLogin(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser1.AccountID, string.Empty, string.Empty, LOLConnect.AccountOAuth.OAuthTypes.FaceBook, newEmail, this.RandomPassword, token1);

            LOLConnect.User tmpUser2 = this._ws.UserCreate(this.RandomSecondaryDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, this.RandomOAuthID, this.RandomOAuthToken, "RandomContactZ19444483838", "RandomContactZ19444483838", newEmail2, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1974, 2, 5), token2);
            LOLConnect.User loggedUser2 = this._ws.UserLogin(this.RandomSecondaryDeviceID, LOLConnect.DeviceDeviceTypes.Windows, tmpUser2.AccountID, string.Empty, string.Empty, LOLConnect.AccountOAuth.OAuthTypes.FaceBook, newEmail2, this.RandomPassword, token2);

            LOLConnect.Contact tmpContact = new LOLConnect.Contact();
            tmpContact.ContactAccountID = tmpUser2.AccountID;
            tmpContact.OwnerAccountID = tmpUser1.AccountID;
            tmpContact.Blocked = false;

            LOLConnect.Contact contactSaved = this._ws.ContactsSave(tmpContact, token1);

            LOLConnect.LOLConnectSearchCriteria tmpCriteria = new LOLConnect.LOLConnectSearchCriteria();
            tmpCriteria.FirstName = "RandomContactZ19444483838";
            tmpCriteria.LastName = "RandomContactZ19444483838";

            List<LOLConnect.LOLConnectSearchCriteria> listSearchCriteria = new List<LOLConnect.LOLConnectSearchCriteria>();
            listSearchCriteria.Add(tmpCriteria);

            var elapsed = Stopwatch.StartNew();
            List<LOLConnect.LOLConnectSearchResult> searchResults = this._ws.ContactsSearchList( listSearchCriteria, tmpUser1.AccountID, token1);
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
