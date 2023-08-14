public class ObjectBase
{
	/*
		Person p = new Person();
		p["FirstName"] = "addressline2";
		p["AddressLine1"] = "bangalore";
		p["AddressLine2"].dump();
	*/
	
	private void Set(string propname,object value)
	{
		if(this.GetType().GetProperties().Where(_ => _.Name == propname).Any())
		{
			var prop = this.GetType().GetProperties().Where(_ => _.Name == propname).First();
			prop.SetValue(this, value, null);
		}
		else if(this.GetType().GetFields().Where(_ => _.Name == propname).Any())
		{
			var field = this.GetType().GetFields().Where(_ => _.Name == propname).First();
			field.SetValue(this,value);
		}
	}
	
	private object Get(string propname)
	{
		if(this.GetType().GetProperties().Where(_ => _.Name == propname).Any())
		{
			var prop = this.GetType().GetProperties().Where(_ => _.Name == propname).First();
			return prop.GetValue(this);
		}
		else if(this.GetType().GetFields().Where(_ => _.Name == propname).Any())
		{
			var field = this.GetType().GetFields().Where(_ => _.Name == propname).First();
			return field.GetValue(this);
		}
		
		return null;
	}
	
	public object this[string name]
	{
		set { Set(name,value); }
		get { return Get(name); }
	}
}
