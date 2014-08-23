using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LOLAccountManagement.Classes;
using System.Runtime.Serialization;

namespace LOLAccountManagement.Classes.DtoObjects
{
    [DataContract]
    public class UserLastUpdatedDTO : WcfBaseObject
    {
        [DataMember]
        public Guid AccountID { get; set; }

        [DataMember]
        public DateTime DateLastUpdated { get; set; }

        public UserLastUpdatedDTO()
            : base()
        {
        }

        public UserLastUpdatedDTO(Guid accountID, DateTime lastUpdated)
        {
            this.AccountID = accountID;
            this.DateLastUpdated = lastUpdated;
            this.Errors = new List<General.Error>();
        }
    }
}