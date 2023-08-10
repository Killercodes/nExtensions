public static class XElementExtension
{
    /// <summary>
    /// Validate XElement with XSD Schema
    /// </summary>
    /// <param name="xElement"></param>
    /// <param name="xsdSchemeUri"></param>
    /// <returns></returns>
    public static bool Validate(this XElement xElement,string xsdSchemeUri)
    {
        bool resultSet = true;

        XDocument doc = XDocument.Parse(xElement.ToString());
        XmlSchemaSet set = new XmlSchemaSet();
        set.Add(null, xsdSchemeUri);

        StringBuilder errors = new StringBuilder();
        doc.Validate(set, (sender, args) => { 
             
            errors.AppendLine(args.Exception.Message);

            if (args.Severity == XmlSeverityType.Error)
                resultSet = false;
        });
        
        return resultSet;
    }
	
	public static XmlDocument ToXmlDocument(this XElement xElement)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xElement.ToString());
		return xmlDocument;
	}
	
    /// <summary>
    /// Convert To Xml
    /// </summary>
    /// <param name="xElement"></param>
    /// <returns></returns>
    public static string ToXml(this XElement xElement)
    {
        //xElement.SetAttributeValue(XNamespace.Xmlns.GetName("xsi"), "http://www.w3.org/2001/XMLSchema-instance");
        //xElement.SetAttributeValue(XNamespace.Xmlns.GetName("xsd"), "http://www.w3.org/2001/XMLSchema");
        return xElement.ToString();
    }

    /// <summary>
    /// Transform Xelement  with Xslt
    /// </summary>
    /// <param name="xElement"></param>
    /// <param name="xsltPath"></param>
    /// <returns></returns>
    public static XElement Transform(this XElement xElement, string xsltPath)
    {
        Func<string, XmlDocument> GetXmlDocument = (xmlContent) =>{
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlContent);
            return xmlDocument;
        };

        try
        {
            var document = GetXmlDocument(xElement.ToString());
            var style = GetXmlDocument(File.ReadAllText(xsltPath));

            XslCompiledTransform transform = new XslCompiledTransform();
            transform.Load(style); // compiled stylesheet
            StringWriter writer = new StringWriter();
            XmlReader xmlReadB = new XmlTextReader(new StringReader(document.DocumentElement.OuterXml));
            transform.Transform(xmlReadB, null, writer);
            return XElement.Parse(writer.ToString());
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Removes Xml namespaces
    /// </summary>
    /// <param name="xElement"></param>
    /// <returns></returns>
    public static XElement RemoveNamespaces(this XElement xElement)
    {
        if (!xElement.HasElements)
        {
            var newXElement = new XElement(xElement.Name.LocalName);
            newXElement.Value = xElement.Value;

            foreach (XAttribute attribute in xElement.Attributes())
                newXElement.Add(attribute);

            return newXElement;
        }
        return new XElement(xElement.Name.LocalName, xElement.Elements().Select(el => RemoveNamespaces(el)));
    }
}