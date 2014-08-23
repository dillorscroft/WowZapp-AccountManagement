using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace LOLAccountManagement.Classes
{
    [DataContract]
    public class WcfBaseObject
    {
        [DataMember]
        public List<General.Error> Errors { get; set; }

        public WcfBaseObject()
        {
            this.Errors = new List<General.Error>();
        }
    }
}