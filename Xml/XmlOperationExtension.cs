
// Useful xml functions 
public static class XmlOperationExtension
{
	//schema
	public struct XmlSchema
	{
		public string FilePath {get;private set;}
		public XmlSchema(string filePath)
		{
			FilePath = filePath;
		}
		
		public override string ToString()
		{
			if(File.Exists(FilePath))
				return File.ReadAllText(FilePath);
		
			return null;
		}
	}
	
	//stylesheet
	public struct XmlStylesheet
	{
		public string FilePath {get;private set;}
		public XmlStylesheet(string filePath)
		{
			FilePath = filePath;
		}
		
		public override string ToString()
		{
			if(File.Exists(FilePath))
				return File.ReadAllText(FilePath);
		
			return null;
		}
	}
	
	//Validated the Xml against schema
	public static bool Validate(this XDocument xDocument,XmlSchema xsdDocuemnt)
    {
        bool resultSet = true;
        XmlSchemaSet set = new XmlSchemaSet();
        set.Add(null, xsdDocuemnt.FilePath);

        StringBuilder errors = new StringBuilder();
        xDocument.Validate(set, (sender, args) => { 
             
            errors.AppendLine(args.Exception.Message);

            if (args.Severity == XmlSeverityType.Error)
                resultSet = false;
        });
        
		errors.ToString().Dump();
        return resultSet;
    }

	
	public static XElement Transform(this XDocument xDocument, XmlStylesheet xmlStylesheet)
    {
        Func<string, XmlDocument> GetXmlDocument = (xmlContent) =>{
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlContent);
            return xmlDocument;
        };
	

        try
        {
            var document = xDocument;//GetXmlDocument(xElement.ToString());
            var style = GetXmlDocument(xmlStylesheet.ToString());

            XslCompiledTransform transform = new XslCompiledTransform();
            transform.Load(style); // compiled stylesheet
            StringWriter writer = new StringWriter();
            XmlReader xmlReadB = new XmlTextReader(new StringReader(document.ToString()));
            transform.Transform(xmlReadB, null, writer);
            return XElement.Parse(writer.ToString());
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
	
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
