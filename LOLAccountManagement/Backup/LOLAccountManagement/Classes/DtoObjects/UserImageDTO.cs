using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LOLAccountManagement.Classes;
using System.Runtime.Serialization;

namespace LOLAccountManagement.Classes.DtoObjects
{
    [DataContract]
    public class UserImageDTO : WcfBaseObject
    {
        [DataMember]
        public Guid AccountID { get; set; }
        
        [DataMember]
        public byte[] ImageData { get; set; }

        public UserImageDTO()
            : base()
        {
            this.AccountID = Guid.Empty;
            this.ImageData = null;
        }

        public UserImageDTO(Guid accountID, byte[] imageData)
        {
            this.AccountID = accountID;
            this.ImageData = imageData;
            this.Errors = new List<General.Error>();
        }
    }
}