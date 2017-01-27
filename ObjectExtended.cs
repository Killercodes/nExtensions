using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace nExtensions
{
    public static class ObjectExtended
    {
        public static string SerializeToJson (this object value)
        { 
            //var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(obj);
            throw new NotImplementedException("Feature not available");
            return null;
        }

        public static string SerializeToXml<T> (this T value)
        {
            string xml = null;
            XmlSerializer xsSubmit = new XmlSerializer(typeof(T));
            using (StringWriter sww = new StringWriter())
            using (XmlWriter writer = XmlWriter.Create(sww))
            {
                xsSubmit.Serialize(writer, value);
                xml = sww.ToString(); // Your XML
            }
            return xml;
        }

        public static T DeserializeToObject<T> (this string value)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            var reader = new StringReader(value);
            T obj = (T)serializer.Deserialize(reader);
            reader.Close();
            return obj;
        }

    }
   
}
