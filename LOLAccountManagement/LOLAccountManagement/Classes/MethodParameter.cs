using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOLAccountManagement.Classes
{
    public class MethodParameter
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public MethodParameter()
        {
            this.Name = string.Empty;
            this.Value = string.Empty;
        }

        public MethodParameter(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
    }

    public class MethodParameters
    {
        public List<MethodParameter> ParametersList { get; set; }

        public MethodParameters()
        {
            this.ParametersList = new List<MethodParameter>();
        }

        /// <summary>
        /// used by UserLogin
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="deviceType"></param>
        /// <param name="accountID"></param>
        /// <param name="oAuthID"></param>
        /// <param name="oAuthToken"></param>
        /// <param name="oAuthType"></param>
        /// <param name="emailAddress"></param>
        /// <param name="password"></param>
        /// <param name="authenticationToken"></param>
        public MethodParameters(string deviceID, Device.DeviceTypes deviceType, Guid accountID, string oAuthID, string oAuthToken, AccountOAuth.OAuthTypes oAuthType, string emailAddress, string password, Guid authenticationToken)
        {
            this.ParametersList = new List<MethodParameter>();
            this.ParametersList.Add(new MethodParameter("DeviceID", deviceID));
            this.ParametersList.Add(new MethodParameter("DeviceType", deviceType.ToString()));
            this.ParametersList.Add(new MethodParameter("AccountID", accountID.ToString()));
            this.ParametersList.Add(new MethodParameter("OAuthID", oAuthID));
            this.ParametersList.Add(new MethodParameter("OAuthToken", oAuthToken));
            this.ParametersList.Add(new MethodParameter("OAuthType", oAuthType.ToString()));
            this.ParametersList.Add(new MethodParameter("EmailAddress", emailAddress));
            this.ParametersList.Add(new MethodParameter("Password", password));
            this.ParametersList.Add(new MethodParameter("AuthenticationToken", authenticationToken.ToString()));
        }

        /// <summary>
        /// used by CreateUser
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="deviceType"></param>
        /// <param name="oAuthType"></param>
        /// <param name="oAuthID"></param>
        /// <param name="oAuthToken"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="emailAddress"></param>
        /// <param name="password"></param>
        /// <param name="picture"></param>
        /// <param name="dateOfBirth"></param>
        /// <param name="authenticationToken"></param>
        public MethodParameters(string deviceID, Device.DeviceTypes deviceType, AccountOAuth.OAuthTypes oAuthType, string oAuthID, string oAuthToken, string firstName, string lastName, string emailAddress, string password, DateTime dateOfBirth, Guid authenticationToken)
        {
            this.ParametersList = new List<MethodParameter>();
            this.ParametersList.Add(new MethodParameter("DeviceID", deviceID));
            this.ParametersList.Add(new MethodParameter("DeviceType", deviceType.ToString()));
            this.ParametersList.Add(new MethodParameter("OAuthType", oAuthType.ToString()));
            this.ParametersList.Add(new MethodParameter("OAuthID", oAuthID));
            this.ParametersList.Add(new MethodParameter("OAuthToken", oAuthToken));
            this.ParametersList.Add(new MethodParameter("FirstName", firstName));
            this.ParametersList.Add(new MethodParameter("LastName", lastName));
            this.ParametersList.Add(new MethodParameter("DateOfBirth", dateOfBirth.ToShortDateString()));
            this.ParametersList.Add(new MethodParameter("EmailAddress", emailAddress));
            this.ParametersList.Add(new MethodParameter("Password", password));
            this.ParametersList.Add(new MethodParameter("AuthenticationToken", authenticationToken.ToString()));
        }

        /// <summary>
        /// used by UserGetSpecific
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="authenticationToken"></param>
        public MethodParameters(Guid accountID, Guid authenticationToken)
        {
            this.ParametersList = new List<MethodParameter>();
            this.ParametersList.Add(new MethodParameter("AccountID", accountID.ToString()));
            this.ParametersList.Add(new MethodParameter("AuthenticationToken", authenticationToken.ToString()));
        }

        public MethodParameters(Guid authenticationToken)
        {
            this.ParametersList = new List<MethodParameter>();
            this.ParametersList.Add(new MethodParameter("AuthenticationToken", authenticationToken.ToString()));
        }

        public MethodParameters(Guid accountID,List<Guid> excludeAccountIDs, Guid authenticationToken)
        {
            this.ParametersList = new List<MethodParameter>();
            this.ParametersList.Add(new MethodParameter("AccountID", accountID.ToString()));
            this.ParametersList.Add(new MethodParameter("AuthenticationToken", authenticationToken.ToString()));

            if (excludeAccountIDs != null && excludeAccountIDs.Count > 0)
                 this.ParametersList.Add(new MethodParameter("ExcludeAccountID" , GenericFunctionality.ToJson(excludeAccountIDs)));
        }

        public MethodParameters(Guid accountID, Guid targetAccountID, Guid authenticationToken)
        {
            this.ParametersList = new List<MethodParameter>();
            this.ParametersList.Add(new MethodParameter("AccountID", accountID.ToString()));
            this.ParametersList.Add(new MethodParameter("TargetAccountID", targetAccountID.ToString()));
            this.ParametersList.Add(new MethodParameter("AuthenticationToken", authenticationToken.ToString()));
        }

        public MethodParameters(Contact contact, Guid authenticationToken)
        {
            this.ParametersList = new List<MethodParameter>();                        
            this.ParametersList.Add(new MethodParameter("Contact", GenericFunctionality.ToJson(contact)));
            this.ParametersList.Add(new MethodParameter("AuthenticationToken", authenticationToken.ToString()));
        }

        public MethodParameters(List<Contact> contacts, Guid authenticationToken)
        {
            this.ParametersList = new List<MethodParameter>();
            this.ParametersList.Add(new MethodParameter("Contacts", GenericFunctionality.ToJson(contacts)));
            this.ParametersList.Add(new MethodParameter("AuthenticationToken", authenticationToken.ToString()));
        }

        public MethodParameters(Guid accountID, ContentPack.ContentPackType contentPackType, Guid authenticationToken)
        {
            this.ParametersList = new List<MethodParameter>();
            this.ParametersList.Add(new MethodParameter("AccountID", accountID.ToString()));
            this.ParametersList.Add(new MethodParameter("AuthenticationToken", authenticationToken.ToString()));
            this.ParametersList.Add(new MethodParameter("ContentPackType", contentPackType.ToString()));
        }
        
        public MethodParameters(string emailAddress,Guid authenticationToken)
        {
            this.ParametersList = new List<MethodParameter>();
            this.ParametersList.Add(new MethodParameter("EmailAddress", emailAddress));
            this.ParametersList.Add(new MethodParameter("AuthenticationToken", authenticationToken.ToString()));
        }

        public MethodParameters(string emailAddress, string resetToken, string password, Guid authenticationToken)
        {
            this.ParametersList = new List<MethodParameter>();
            this.ParametersList.Add(new MethodParameter("EmailAddress", emailAddress.ToString()));
            this.ParametersList.Add(new MethodParameter("EmailAddress", resetToken));
            this.ParametersList.Add(new MethodParameter("password", password));
            this.ParametersList.Add(new MethodParameter("AuthenticationToken", authenticationToken.ToString()));
        }

        public MethodParameters(string emailAddress, string resetToken, Guid authenticationToken)
        {
            this.ParametersList = new List<MethodParameter>();
            this.ParametersList.Add(new MethodParameter("EmailAddress", emailAddress.ToString()));
            this.ParametersList.Add(new MethodParameter("EmailAddress", resetToken));
            this.ParametersList.Add(new MethodParameter("AuthenticationToken", authenticationToken.ToString()));
        }

        public MethodParameters(string deviceID, Device.DeviceTypes deviceType, Guid accountID)
        {
            this.ParametersList = new List<MethodParameter>();
            this.ParametersList.Add(new MethodParameter("DeviceID", deviceID));
            this.ParametersList.Add(new MethodParameter("DeviceType", deviceType.ToString()));
            this.ParametersList.Add(new MethodParameter("AccountID", accountID.ToString()));
        }
    }
}