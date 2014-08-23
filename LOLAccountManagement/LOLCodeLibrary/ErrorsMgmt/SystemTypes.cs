using System.Runtime.Serialization;
namespace LOLCodeLibrary.ErrorsManagement
{
    [DataContract]
    public class SystemTypes
    {
        /// <summary>
        /// all error messages and their codes
        /// </summary>        
        public enum ErrorMessage
        {
            NoErrorDetected = 0,

            AuthenticationTokenInvalid = 1,
            
            DeviceIDMissing = 2,
            
            PasswordsDontMatch = 3,
            
            AccountInformationInvalid = 4,
            
            DuplicateEmailAddress = 6,
            
            AccountIDNull = 7,
            
            AccountIdDoesNotMatchDatabase = 8,
            
            DeviceSaveFailed = 15,
            
            AuthenticationTokenDoesNotMatchAccountID = 28,
            
            AuthenticationTokenNotFound = 29,
            
            AuthenticationTokenLastUsedDateNotSet = 30,
            
            AuthenticationTokenExpired= 31,
            
            AuthenticationTokenNotLoggedIn = 32,
            
            AuthenticationTokenLoadFromDatarowFailed = 33,
            
            AuthenticationTokenLoadFailed = 34,
            
            AuthenticationTokenDeleteFailed = 35,
            
            AuthenticationTokenSaveFailed = 36,
            
            InvalidUserInformation = 52,
            
            NullEmailAddress = 53,
            
            ResetPasswordFailed = 54,
            
            InvalidResetCode = 55,
            
            ResetCodeExpired = 59,
            
            ResetPasswordFailed_NoMatchingEmailAddress = 61,
            
            InvalidEmailAddress = 62,
            
            DeviceNotRegisteredWithAnAccount = 63,
            
            UnknownUserContactRequested = 76,
            
            DuplicateContact = 77,
            
            AccountIDInvalid = 78,
            
            OAuthTokenInvalid = 79,
            
            OAuthIDInvalid = 80,
            
            UserGetSpecificFailed = 81,
            
            AuthenticationTokenLoggedOut = 82,
            
            Unexpected = 100,
            
            CouldNotIdentifyRow = 101,
            
            ContentPackIdMissing = 102,
            
            ContentPackItemNotFound = 103,
            
            ContentPackItemIdMissing = 104,
            
            SearchCriteriaMissing = 105,
            
            ContactAccountIDMissing = 106,
            
            ItemSizeNone = 107,
            
            ContactIDEmpty = 108,
            
            ContactNotFound = 109,
            
            MessageStepsMissing = 200,
            
            ToAccountListEmpty = 201,
            
            MessageSaveFatalError = 202,
            
            MessageStepFatalError = 203,
            
            ToAccountIdIsEmptyGuid = 204
        }
    }
}