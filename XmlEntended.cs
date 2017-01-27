using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Xsl;

namespace nExtensions
{
    public static class XmlEntended
    {
        /// <summary>
        /// Remove the html/xml tags from string
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string StripHTML (this string inputString)
        {
            const string HTML_TAG_PATTERN = "<.*?>";
            return Regex.Replace(inputString, HTML_TAG_PATTERN, string.Empty);
        }

        public static string InnerXML (this XElement xElement)
        {
            var reader = xElement.CreateReader();
            reader.MoveToContent();
            return reader.ReadInnerXml();
        }

        public static string OuterXML (this XElement xElement)
        {
            var reader = xElement.CreateReader();
            reader.MoveToContent();
            return reader.ReadOuterXml();
        }

        public static string XsltTransformation (this string Xml, XsltArgumentList argsList, string Xslt)
        {
            /*
            string man_ID = "manID";

            XsltArgumentList argsList = new XsltArgumentList();
            argsList.AddParam("Boss_ID", "", man_ID);
            */

            // xslt transform engine
            XslCompiledTransform transform = new XslCompiledTransform(true);

            // If the xslt is a file ? Load file : Read tehe file contents and load
            if (Xslt.EndsWith(".xsl", StringComparison.InvariantCultureIgnoreCase) || Xslt.EndsWith(".xslt", StringComparison.InvariantCultureIgnoreCase))
            {
                transform.Load(Xslt, XsltSettings.TrustedXslt, new XmlUrlResolver());
            }
            else
            {
                XmlReader xsltContent = XmlReader.Create(new StringReader(File.ReadAllText(Xslt)));
                transform.Load(xsltContent, XsltSettings.TrustedXslt, new XmlUrlResolver());
            }


            // Xml without namespace
            XElement xmlWithoutNamespace = null;

            // If Xml is file ? Load file : Read the contents and load 
            if (Xml.EndsWith(".xml", StringComparison.InvariantCultureIgnoreCase))
            {
                // Remove the namespaces from xml
                xmlWithoutNamespace = RemoveAllNamespaces(XElement.Parse((File.ReadAllText(Xml))));
            }
            else
            {
                xmlWithoutNamespace = RemoveAllNamespaces(XElement.Parse(Xml));
            }

            XmlReader reader = XmlReader.Create(new StringReader(xmlWithoutNamespace.ToString()));
            StringWriter output = new StringWriter();
            XmlWriter writer = XmlWriter.Create(output, transform.OutputSettings);

            // Transform
            transform.Transform(reader, argsList, writer);

            // Return output
            return output.ToString();
        }

        public static string XsltTransformation (this string Xml, string Xslt)
        {
            /*
            string man_ID = "manID";

            XsltArgumentList argsList = new XsltArgumentList();
            argsList.AddParam("Boss_ID", "", man_ID);
            */

            // xslt transform engine
            XslCompiledTransform transform = new XslCompiledTransform(true);

            // If the xslt is a file ? Load file : Read tehe file contents and load
            if (Xslt.EndsWith(".xsl", StringComparison.InvariantCultureIgnoreCase) || Xslt.EndsWith(".xslt", StringComparison.InvariantCultureIgnoreCase))
            {
                transform.Load(Xslt, XsltSettings.TrustedXslt, new XmlUrlResolver());
            }
            else
            {
                XmlReader xsltContent = XmlReader.Create(new StringReader(File.ReadAllText(Xslt)));
                transform.Load(xsltContent, XsltSettings.TrustedXslt, new XmlUrlResolver());
            }


            // Xml without namespace
            XElement xmlWithoutNamespace = null;

            // If Xml is file ? Load file : Read the contents and load 
            if (Xml.EndsWith(".xml", StringComparison.InvariantCultureIgnoreCase))
            {
                // Remove the namespaces from xml
                xmlWithoutNamespace = RemoveAllNamespaces(XElement.Parse((File.ReadAllText(Xml))));
            }
            else
            {
                xmlWithoutNamespace = RemoveAllNamespaces(XElement.Parse(Xml));
            }

            XmlReader reader = XmlReader.Create(new StringReader(xmlWithoutNamespace.ToString()));
            StringWriter output = new StringWriter();
            XmlWriter writer = XmlWriter.Create(output, transform.OutputSettings);

            // Transform
            transform.Transform(reader, writer);

            // Return output
            return output.ToString();
        }

        public static XElement RemoveAllNamespaces (this XElement xElement)
        {
            if (!xElement.HasElements)
            {
                XElement newXElement = new XElement(xElement.Name.LocalName);
                newXElement.Value = xElement.Value;

                foreach (XAttribute attribute in xElement.Attributes())
                    newXElement.Add(attribute);

                return newXElement;
            }
            return new XElement(xElement.Name.LocalName, xElement.Elements().Select(el => RemoveAllNamespaces(el)));
        }

        public static void XmlSchemaValidationWithCallback (this string xmlFilePath)
        {
            // Set the validation settings.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += new ValidationEventHandler(SchemaValidationCallBack);

            // Create the XmlReader object.
            XmlReader reader = XmlReader.Create(xmlFilePath, settings);

            // Parse the file. 
            while (reader.Read()) ;
        }

        private static void SchemaValidationCallBack (object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                Console.WriteLine("\tWarning: Matching schema not found.  No validation occurred." + args.Message);
            else
                Console.WriteLine("\tValidation error: " + args.Message);

        }

        public static void XmlSchemaValidationWithCallback (string xmlFilePath, string schemaFilePath)
        {
            // Create the XmlSchemaSet class.
            XmlSchemaSet sc = new XmlSchemaSet();
            sc.Add(null, schemaFilePath);

            // Set the validation settings.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = sc;
            settings.ValidationEventHandler += new ValidationEventHandler(SchemaValidationCallBack);

            // Create the XmlReader object.
            XmlReader reader = XmlReader.Create(xmlFilePath, settings);

            // Parse the file. 
            while (reader.Read()) ;

            //Close the reader.
            reader.Close();
        }

        public static bool XmlSchemaValidation (string XmlPath, string XsdPath)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(XmlPath);
            xml.Schemas.Add(null, XsdPath);
            try
            {
                xml.Validate(null);
            }
            catch (XmlSchemaValidationException)
            {
                return false;
            }
            return true;
        }
    }
}
