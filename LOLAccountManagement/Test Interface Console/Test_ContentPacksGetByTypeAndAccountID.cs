using System;
using System.Diagnostics;
using LOLAccountManagement.Classes;
using LOLCodeLibrary.LoggingSystem;
using System.Collections.Generic;
using LOLCodeLibrary.ErrorsManagement;
using System.Drawing;

namespace Test_Interface_Console
{
    public sealed class Test_ContentPacksGetByTypeAndAccountID : TestBase, ITestable
    {
        #region ITestable        
        public LOLConnect.LOLConnectClient _ws { get;set;}
        public ILogger Logger { get; set; }

        public override void RunTests()
        {
            this.ContentPacksGetByTypeAndAccountID_TokenNotAuthenticated_ShouldFail();
            this.ContentPacksGetByTypeAndAccountID_AccountIdNotLinkedToToken_ShouldFail();
            this.ContentPacksGetByTypeAndAccountID_TokenExpired_ShouldFail();
            this.ContentPacksGetByTypeAndAccountID_TokenLoggedOut_ShouldFail();
            this.ContentPacksGetByTypeAndAccountID_TokenNotInDatabase_ShouldFail();
            this.ContentPacksGetByTypeAndAccountID_ContentPackIdMissing_ShouldFail();
            this.ContentPackGetItemLight_ValidInput_ShouldSucceed();
        }
        #endregion

        #region Constructor
        public Test_ContentPacksGetByTypeAndAccountID(LOLConnect.LOLConnectClient ws, ILogger logger)
            : base()
        {
            this._ws = ws;
            this.Logger = logger;
        }
        #endregion

        #region Test Methods     
   
        private void ContentPacksGetByTypeAndAccountID_ContentPackIdMissing_ShouldFail()
        {
            //this.Logger.LogMessage("Testing ContentPacksGetByTypeAndAccountID_ContentPackIdMissing_ShouldFail ...", true);
            //Image profileImage = Image.FromFile(this.ImageFilePath);
            //Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            //string newEmail = this.GetRandomEmail();

            //LOLConnect.User tmpUser = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, "", "", this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);
            //LOLConnect.User loggedIn = _ws.UserLogin(RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, Guid.Empty, "", "", 0, newEmail, this.RandomPassword, token);

            //var elapsed = Stopwatch.StartNew();
            //LOLConnect.ContentPackItemListDTO result = _ws.ContentPacksGetByTypeAndAccountID(0, LOLConnect.ContentPackItem.ItemSize.None, null, tmpUser.AccountID, token);
            //elapsed.Stop();
            //this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            //if (result.Errors.Count == 1 && result.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.ContentPackIdMissing.ToString()))
            //    this.Logger.LogMessage(this.TestSuccessMessage, true);
            //else
            //    this.Logger.LogMessage(this.TestFailMessage, true);

            //this.Logger.LogMessage(this.Delimiter, true);
            //this.CleanAfterTest(this._ws);
        }

        private void ContentPacksGetByTypeAndAccountID_TokenNotAuthenticated_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPacksGetByTypeAndAccountID_TokenNotAuthenticated_ShouldFail ...", true);
            Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            //var elapsed = Stopwatch.StartNew();
            //List<LOLConnect.ContentPack> result = _ws.ContentPacksGetByTypeAndAccountID(Guid.NewGuid(), LOLConnect.ContentPack.ContentPackType.Emoticon, null, token);
            //elapsed.Stop();
            //this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            //if (result.Errors.Count == 1 && result.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenNotLoggedIn.ToString()))
            //    this.Logger.LogMessage(this.TestSuccessMessage, true);
            //else
            //    this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPacksGetByTypeAndAccountID_TokenExpired_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPacksGetByTypeAndAccountID_TokenExpired_ShouldFail ...", true);
            this.Logger.LogMessage("Not Implemented Yet ...", true);
            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPacksGetByTypeAndAccountID_TokenLoggedOut_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPacksGetByTypeAndAccountID_TokenLoggedOut_ShouldFail ...", true);
            //Image profileImage = Image.FromFile(this.ImageFilePath);
            //Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);

            //string newEmail = this.GetRandomEmail();

            //LOLConnect.User tmpUser = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, "", "", this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);
            //LOLConnect.User loggedIn = _ws.UserLogin(RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, Guid.Empty, "", "", 0, newEmail, this.RandomPassword, token);
            //_ws.UserLogOut(this.RandomDeviceID, tmpUser.AccountID, token);

            //var elapsed = Stopwatch.StartNew();
            //LOLConnect.ContentPackItemListDTO result = _ws.ContentPacksGetByTypeAndAccountID(0, LOLConnect.ContentPackItem.ItemSize.None, null, Guid.NewGuid(), token);
            //elapsed.Stop();
            //this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            //if (result.Errors.Count == 1 && result.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenLoggedOut.ToString()))
            //    this.Logger.LogMessage(this.TestSuccessMessage, true);
            //else
            //    this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPacksGetByTypeAndAccountID_TokenNotInDatabase_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPacksGetByTypeAndAccountID_TokenNotInDatabase_ShouldFail ...", true);

            //var elapsed = Stopwatch.StartNew();
            //LOLConnect.ContentPackItemListDTO result = _ws.ContentPacksGetByTypeAndAccountID(0, LOLConnect.ContentPackItem.ItemSize.None, null, Guid.NewGuid(), Guid.NewGuid());
            //elapsed.Stop();
            //this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            //if (result.Errors.Count == 1 && result.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenNotFound.ToString()))
            //    this.Logger.LogMessage(this.TestSuccessMessage, true);
            //else
            //    this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPacksGetByTypeAndAccountID_AccountIdNotLinkedToToken_ShouldFail()
        {
            this.Logger.LogMessage("Testing ContentPacksGetByTypeAndAccountID_AccountIdNotLinkedToToken_ShouldFail ...", true);
            //Guid token = _ws.AuthenticationTokenGet(this.RandomDeviceID);
            //Image profileImage = Image.FromFile(this.ImageFilePath);

            //string newEmail = this.GetRandomEmail();

            //LOLConnect.User tmpUser = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, "", "", this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);
            //LOLConnect.User loggedIn = _ws.UserLogin(RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, Guid.Empty, "", "", 0, newEmail, this.RandomPassword, token);

            //var elapsed = Stopwatch.StartNew();
            //LOLConnect.ContentPackItemListDTO result = _ws.ContentPacksGetByTypeAndAccountID(0, LOLConnect.ContentPackItem.ItemSize.None, null, Guid.NewGuid(), token);
            //elapsed.Stop();
            //this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            //if (result.Errors.Count == 1 && result.Errors[0].ErrorDescription.Equals(SystemTypes.ErrorMessage.AuthenticationTokenDoesNotMatchAccountID.ToString()))
            //    this.Logger.LogMessage(this.TestSuccessMessage, true);
            //else
            //    this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        private void ContentPackGetItemLight_ValidInput_ShouldSucceed()
        {
            this.Logger.LogMessage("Testing ContentPackGetItemLight_ValidInput_ShouldSucceed ...", true);
            //Guid token = this._ws.AuthenticationTokenGet(this.RandomDeviceID);
            //Image profileImage = Image.FromFile(this.ImageFilePath);

            //string newEmail = this.GetRandomEmail();

            //LOLConnect.User tmpUser = _ws.UserCreate(this.RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, LOLConnect.AccountOAuth.OAuthTypes.LOL, "", "", this.RandomFirstName, this.RandomLastName, newEmail, this.RandomPassword, ImageHandler.imageToByteArray(profileImage), new DateTime(1978, 2, 5), token);
            //LOLConnect.User loggedIn = _ws.UserLogin(RandomDeviceID, LOLConnect.DeviceDeviceTypes.Windows, Guid.Empty, "", "", 0, newEmail, this.RandomPassword, token);

            //var elapsed = Stopwatch.StartNew();
            //LOLConnect.ContentPackItemListDTO tmpResult = _ws.ContentPacksGetByTypeAndAccountID(12, LOLConnect.ContentPackItem.ItemSize.Tiny, null, tmpUser.AccountID, token);
            //elapsed.Stop();
            //this.Logger.LogMessage(PrepareElapsedTimeOutput(elapsed), true);

            //bool noData = true;
            
            //foreach (LOLConnect.ContentPackItem item in tmpResult.contentPackItems)
            //{
            //    if (item.ContentPackData.Length > 0)
            //    {
            //        noData = false;
            //        break;
            //    }                
            //}

            //if (noData && tmpResult.contentPackItems.Count > 0 && tmpResult.Errors.Count == 0)
            //    this.Logger.LogMessage(this.TestSuccessMessage, true);
            //else
            //    this.Logger.LogMessage(this.TestFailMessage, true);

            this.Logger.LogMessage(this.Delimiter, true);
            this.CleanAfterTest(this._ws);
        }

        #endregion
    }
}
