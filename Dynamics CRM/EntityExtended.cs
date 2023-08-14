
// Extends Dynamic Crm Entity, add reference to Xrm.Sdk dlls
public static class EntityExtended
{
	//Convert Entity To Xml
    public static XDocument ToXml(this Entity entity)
    {
		var attribes = entity.Attributes.Where(a => a.Value.GetType() == typeof(AliasedValue)).ToList();
		foreach(var a in attribes)
		{
			var rt  = ((AliasedValue)entity[a.Key]).Value;
			entity.Attributes.Remove(a.Key);
			entity.Attributes.Add(a.Key,rt);
		}
		
        var xe = new XElement(entity.GetType().Name,
			new XAttribute("LogicalName", entity.LogicalName),new XAttribute("Id", entity.Id), 
				entity.Attributes.Select(kv => new XElement(kv.Key, kv.Value)));
				
		var xd = new XDocument(xe);

        return xd;
    }
	
	//Read Alias Value
	public static T GetAliasValue<T>(this Entity entity, string attributeName)
	{
		var aliasAttribute = entity.Attributes.Where(a => a.Key == attributeName && a.Value.GetType() == typeof(AliasedValue)).FirstOrDefault();
		
		if(aliasAttribute.Value != null)
			return (T)((AliasedValue)entity[aliasAttribute.Key]).Value;
			
		return default(T);
			
	}
}
