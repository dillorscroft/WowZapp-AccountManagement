using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using LOLAccountManagement.Classes;
using LOLAccountManagement.DataAccess;
using LOLCodeLibrary.LoggingSystem;

namespace Test_Interface_Console
{
    class Program
    {
        //private const string DeviceID = "12345678-1234-1234-1234-123456789012";        

        static void Main(string[] args)
        {
            LOLConnect.LOLConnectClient ws = new LOLConnect.LOLConnectClient();
            ws.GetVersionNumber(LOLConnect.DeviceDeviceTypes.Windows);
            ShowMenu();
        }

        private static void ShowMenu()
        {
            Console.WriteLine("Which tests do you want to run ?");
            Console.WriteLine("0 - All");
            Console.WriteLine("1 - AccountOAuthCreate");
            Console.WriteLine("2 - AuthenticationTokenGet");
            Console.WriteLine("3 - ContactsGetList");
            Console.WriteLine("4 - ContactsSave");
            Console.WriteLine("5 - ContactsSearch");
            Console.WriteLine("6 - UserCreate");
            Console.WriteLine("7 - UserGetSpecific");
            Console.WriteLine("8 - UserLogin");
            Console.WriteLine("9 - UserLogout");
            Console.WriteLine("10 - UserGetResetToken");
            Console.WriteLine("11 - UserPasswordReset");
            Console.WriteLine("12 - UserValidateResetToken");
            Console.WriteLine("13 - ContactGet");
            Console.WriteLine("14 - UserSave");
            Console.WriteLine("15 - UserGetImageData");
            Console.WriteLine("16 - ContentPackGetItemLight");
            Console.WriteLine("17 - ContentPackGetItemIcon");
            Console.WriteLine("18 - ContentPackGetPackItemsLight");
            Console.WriteLine("19 - ContentPackItemGetData");
            Console.WriteLine("20 - ContactsSearchList");
            Console.WriteLine("21 - ContentPackGetItem");
            Console.WriteLine("22 - ContactsGetUpdated");
            Console.WriteLine("23 - ContentPacksGetByType");
            Console.WriteLine("24 - ContentPackGetPackItems");
            Console.WriteLine("25 - ContactsDelete");
            
            Console.WriteLine("X - Exit");

            int testID = 0;
            var read = Console.ReadLine();
            if (!read.ToLower().Equals("x"))
            {
                int.TryParse(read, out testID);
                RunBattery(testID);
            }
        }

        private static void RunBattery(int id)
        {
            ILogger logger = new LoggerManager(new ConsoleLogger()).GetManager();
            LOLConnect.LOLConnectClient ws = new LOLConnect.LOLConnectClient();
            
            switch (id)
            {
                case 0:
                    Test_AccountOAuthCreate testAccountOAuthCreate = new Test_AccountOAuthCreate(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for AccountOAuthCreate", true);
                    testAccountOAuthCreate.RunTests();
                    logger.LogMessage("================================", true);

                    Test_AuthenticationTokenGet testAuthenticationTokenGet = new Test_AuthenticationTokenGet(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for AuthenticationTokenGet", true);
                    testAuthenticationTokenGet.RunTests();
                    logger.LogMessage("================================", true);

                    Test_ContactsGetList testContactsGetList = new Test_ContactsGetList(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for ContactsGetList", true);
                    testContactsGetList.RunTests();
                    logger.LogMessage("================================", true);

                    Test_ContactsSave testContactsSave = new Test_ContactsSave(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for ContactsSave", true);
                    testContactsSave.RunTests();
                    logger.LogMessage("================================", true);

                    Test_ContactsSearch testContactsSearch = new Test_ContactsSearch(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for ContactsSearch", true);
                    testContactsSearch.RunTests();
                    logger.LogMessage("================================", true);

                    Test_UserCreate testUserCreate = new Test_UserCreate(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for UserCreate", true);
                    testUserCreate.RunTests();
                    logger.LogMessage("================================", true);                    

                     Test_UserGetSpecific testUserGetSpecific = new Test_UserGetSpecific(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for UserGetSpecific", true);
                    testUserGetSpecific.RunTests();
                    logger.LogMessage("================================", true);

                    Test_UserLogin testUserLogin = new Test_UserLogin(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for UserLogin", true);
                    testUserLogin.RunTests();
                    logger.LogMessage("================================", true);

                    Test_UserLogOut testUserLogOut = new Test_UserLogOut(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for UserLogOut", true);
                    testUserLogOut.RunTests();
                    logger.LogMessage("================================", true);

                    Test_UserGetResetToken testUserGetResetToken = new Test_UserGetResetToken(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for UserGetResetToken", true);
                    testUserGetResetToken.RunTests();
                    logger.LogMessage("================================", true);

                    Test_UserPasswordReset testUserPasswordReset = new Test_UserPasswordReset(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for UserPasswordReset", true);
                    testUserPasswordReset.RunTests();
                    logger.LogMessage("================================", true);

                    Test_UserValidateResetToken testUserValidateResetToken = new Test_UserValidateResetToken(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for UserGetResetToken", true);
                    testUserValidateResetToken.RunTests();
                    logger.LogMessage("================================", true);

                    Test_ContactGet testContactGet = new Test_ContactGet(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for UserGetResetToken", true);
                    testContactGet.RunTests();
                    logger.LogMessage("================================", true);

                    Test_UserSave testUserSave = new Test_UserSave(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for UserSave", true);
                    testUserSave.RunTests();
                    logger.LogMessage("================================", true);

                    Test_UserGetImageData testUserGetImageData = new Test_UserGetImageData(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for UserGetImageData", true);
                    testUserGetImageData.RunTests();
                    logger.LogMessage("================================", true);

                    Test_ContentPackGetItemLight testContentPackGetItemLight = new Test_ContentPackGetItemLight(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for ContentPackGetItemLight", true);
                    testContentPackGetItemLight.RunTests();
                    logger.LogMessage("================================", true);

                    Test_ContentPackGetItemIcon testContentPackGetItemIcon = new Test_ContentPackGetItemIcon(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for ContentPackGetItemIcon", true);
                    testContentPackGetItemIcon.RunTests();
                    logger.LogMessage("================================", true);

                    Test_ContentPackGetPackItemsLight testContentPackGetPackItemsLight = new Test_ContentPackGetPackItemsLight(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for ContentPackGetPackItemsLight", true);
                    testContentPackGetPackItemsLight.RunTests();
                    logger.LogMessage("================================", true);

                    Test_ContentPackItemGetData testContentPackItemGetData = new Test_ContentPackItemGetData(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for ContentPackItemGetData", true);
                    testContentPackItemGetData.RunTests();
                    logger.LogMessage("================================", true);

                    Test_ContactsSearchList testContactsSearchList = new Test_ContactsSearchList(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for ContactsSearchList", true);
                    testContactsSearchList.RunTests();
                    logger.LogMessage("================================", true);

                    Test_ContentPackGetItem testContentPackGetItem = new Test_ContentPackGetItem(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for ContentPackGetItem", true);
                    testContentPackGetItem.RunTests();
                    logger.LogMessage("================================", true);

                    Test_ContactsGetUpdated testContactsGetUpdated = new Test_ContactsGetUpdated(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for ContactsGetUpdated", true);
                    testContactsGetUpdated.RunTests();
                    logger.LogMessage("================================", true);

                    Test_ContentPacksGetByType testContentPacksGetByType = new Test_ContentPacksGetByType(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for ContentPacksGetByType", true);
                    testContentPacksGetByType.RunTests();
                    logger.LogMessage("================================", true);

                    Test_ContentPackGetPackItems testContentPackGetPackItems = new Test_ContentPackGetPackItems(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for ContentPackGetPackItems", true);
                    testContentPackGetPackItems.RunTests();
                    logger.LogMessage("================================", true);

                    Test_ContactsDelete testContactsDelete = new Test_ContactsDelete(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for ContactsDelete", true);
                    testContactsDelete.RunTests();
                    logger.LogMessage("================================", true);

                    break;
                case 1:
                    Test_AccountOAuthCreate testAccountOAuthCreate1 = new Test_AccountOAuthCreate(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for AccountOAuthCreate", true);
                    testAccountOAuthCreate1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                case 2:
                    Test_AuthenticationTokenGet testAuthenticationTokenGet1 = new Test_AuthenticationTokenGet(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for AuthenticationTokenGet", true);
                    testAuthenticationTokenGet1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                case 3:
                    Test_ContactsGetList testContactsGetList1 = new Test_ContactsGetList(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for ContactsGetList", true);
                    testContactsGetList1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                case 4:
                    Test_ContactsSave testContactsSave1 = new Test_ContactsSave(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for ContactsSave", true);
                    testContactsSave1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                case 5:
                    Test_ContactsSearch testContactsSearch1 = new Test_ContactsSearch(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for ContactsSearch", true);
                    testContactsSearch1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                case 6:
                    Test_UserCreate testUserCreate1 = new Test_UserCreate(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for UserCreate", true);
                    testUserCreate1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                case 7:
                    Test_UserGetSpecific testUserGetSpecific1 = new Test_UserGetSpecific(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for UserGetSpecific", true);
                    testUserGetSpecific1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                case 8:
                    Test_UserLogin testUserLogin1 = new Test_UserLogin(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for UserLogin", true);
                    testUserLogin1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                case 9:
                    Test_UserLogOut testUserLogOut1 = new Test_UserLogOut(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for UserLogOut", true);
                    testUserLogOut1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                case 10:
                    Test_UserGetResetToken testUserGetResetToken1 = new Test_UserGetResetToken(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for UserGetResetToken", true);
                    testUserGetResetToken1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                case 11:
                    Test_UserPasswordReset testUserPasswordReset1 = new Test_UserPasswordReset(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for UserGetResetToken", true);
                    testUserPasswordReset1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                case 12:
                    Test_UserValidateResetToken testUserValidateResetToken1 = new Test_UserValidateResetToken(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for UserGetResetToken", true);
                    testUserValidateResetToken1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                case 13:
                    Test_ContactGet testContactGet1 = new Test_ContactGet(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for ContactGet", true);
                    testContactGet1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                case 14:
                    Test_UserSave testUserSave1 = new Test_UserSave(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for UserSave", true);
                    testUserSave1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                case 15:
                    Test_UserGetImageData testUserGetImageData1 = new Test_UserGetImageData(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for UserGetImageData", true);
                    testUserGetImageData1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                case 16:
                    Test_ContentPackGetItemLight testContentPackGetItemLight1 = new Test_ContentPackGetItemLight(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for ContentPackGetItemLight", true);
                    testContentPackGetItemLight1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                case 17:
                    Test_ContentPackGetItemIcon testContentPackGetItemIcon1 = new Test_ContentPackGetItemIcon(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for ContentPackGetItemIcon", true);
                    testContentPackGetItemIcon1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                case 18:
                    Test_ContentPackGetPackItemsLight testContentPackGetPackItemsLight1 = new Test_ContentPackGetPackItemsLight(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for ContentPackGetPackItemsLight", true);
                    testContentPackGetPackItemsLight1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                case 19:
                    Test_ContentPackItemGetData testContentPackItemGetData1 = new Test_ContentPackItemGetData(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for ContentPackItemGetData", true);
                    testContentPackItemGetData1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                case 20:
                    Test_ContactsSearchList testContactsSearchList1 = new Test_ContactsSearchList(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for ContactsSearchList", true);
                    testContactsSearchList1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                case 21:
                    Test_ContentPackGetItem testContentPackGetItem1 = new Test_ContentPackGetItem(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for ContentPackGetItem", true);
                    testContentPackGetItem1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                case 22:
                    Test_ContactsGetUpdated testContactsGetUpdated1 = new Test_ContactsGetUpdated(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for ContactsGetUpdated", true);
                    testContactsGetUpdated1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                case 23:
                    Test_ContentPacksGetByType testContentPacksGetByType1 = new Test_ContentPacksGetByType(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for ContentPacksGetByType", true);
                    testContentPacksGetByType1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                case 24:
                    Test_ContentPackGetPackItems testContentPackGetPackItems1 = new Test_ContentPackGetPackItems(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for ContentPackGetPackItems", true);
                    testContentPackGetPackItems1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                case 25:
                    Test_ContactsDelete testContactsDelete1 = new Test_ContactsDelete(ws, logger);
                    logger.LogMessage("================================", true);
                    logger.LogMessage("Running tests for ContactsDelete", true);
                    testContactsDelete1.RunTests();
                    logger.LogMessage("================================", true);
                    break;
                default:
                    logger.LogMessage("Please pass a valid option ( 0-25 )", true);
                    break;
            }            
            
            ShowMenu();
        }
    }
}
