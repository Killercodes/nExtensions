using System;

public static class MemoryStreamExtended
{
	public static MemoryStream ToMemoryStream<T>(T t)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		MemoryStream stream = new MemoryStream();
		
		formatter.Serialize(stream, t);
		return stream;		
	}
	
	public static MemoryStream FromBase64(string base64String)
	{
		return new MemoryStream(Convert.FromBase64String(base64String));
	}
	
	public static string ToBase64(MemoryStream memStream)
	{
		memStream.Flush();
        memStream.Position = 0;
		return (Convert.ToBase64String(memStream.ToArray()));
	}
}
