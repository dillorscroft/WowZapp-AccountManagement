using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOLAccountManagement.Classes.DtoObjects
{
    public class ContentPackItemDataDTO : WcfBaseObject
    {
        public byte[] ItemData { get; set; }
        
        public ContentPackItemDataDTO()
            : base()
        {
            this.ItemData = new byte[0];
        }

        public ContentPackItemDataDTO(byte[] packData)
        {
            this.Errors = new List<General.Error>();
            this.ItemData = packData;
        }
    }
}