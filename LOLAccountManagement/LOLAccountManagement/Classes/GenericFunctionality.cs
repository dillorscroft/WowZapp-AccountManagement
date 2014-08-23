using System;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System.Web.Configuration;

namespace LOLAccountManagement.Classes
{
    public static class GenericFunctionality
    {
        public static string ToXML(object target_object)
        {
            StringWriter string_writer = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(target_object.GetType());
            MemoryStream stream = new MemoryStream();
            serializer.Serialize(string_writer, target_object);
            string xml = string_writer.ToString();
            //datetime fix for sql 2008
            xml = xml.Replace("0001-01-01T00:00:00", string.Empty);
            return xml;
        }

        public static string ToJson(object target_object)
        {
            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJSON = oSerializer.Serialize(target_object);
            return sJSON;
        }

        public static string GetAppSetting(string name)
        {
            string result = string.Empty;
            if (WebConfigurationManager.AppSettings[name] != null)
                result = WebConfigurationManager.AppSettings[name].ToString();

            return result;
        }        
    }

    public static class ExtensionMethods
    {
        public static bool ToBool(this string input)
        {
            bool result = false;
            bool.TryParse(input, out result);
            return result;
        }

        public static int ToInt(this string input)
        {
            int result = 0;
            int.TryParse(input, out result);
            return result;
        }
    }
}