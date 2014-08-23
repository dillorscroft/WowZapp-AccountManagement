using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LOLAccountManagement.DataAccess;
using System.Runtime.Serialization;

namespace LOLAccountManagement.Classes.DtoObjects
{
    [DataContract]
    public class ContentPackItemListDTO : WcfBaseObject
    {
        [DataMember]
        public List<ContentPackItem> contentPackItems { get; set; }

        public ContentPackItemListDTO()
            : base()
        {
            this.contentPackItems = new List<ContentPackItem>();
        }

        public ContentPackItemListDTO(List<ContentPackItem> input)
        {
            this.contentPackItems = new List<ContentPackItem>();
            this.Errors = new List<General.Error>();
        }
    }
}