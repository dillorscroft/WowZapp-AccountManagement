using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace LOLCodeLibrary
{
    public static class GenericFunctionality
    {
        public static string ToXML(object target_object)
        {
            string xml = string.Empty;
            if (target_object == null)
                xml = "null";
            else
            {
                StringWriter string_writer = new StringWriter();
                XmlSerializer serializer = new XmlSerializer(target_object.GetType());
                MemoryStream stream = new MemoryStream();
                serializer.Serialize(string_writer, target_object);
                xml = string_writer.ToString();
                //datetime fix for sql 2008
                xml = xml.Replace("0001-01-01T00:00:00", string.Empty);
            }
            return xml;
        }    
    }
}
