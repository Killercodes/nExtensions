using System;

public static class BinaryExtended
{
	public static void Save<T>(this T t, Uri filename)
	{
		BinaryFormatter formatter = new BinaryFormatter();
        FileStream output = new FileStream(filename.LocalPath,FileMode.OpenOrCreate, FileAccess.Write);
		formatter.Serialize(output, t);
        output.Close();
	}
	
	public static string ToBase64<T>(this T t)
	{
		using (var stream = new MemoryStream())
	   	{
	       var formatter = new BinaryFormatter();
	       formatter.Serialize(stream, t);
	       stream.Flush();
	       stream.Position = 0;
		  
	       return Convert.ToBase64String(stream.ToArray());
	   	}
	}
	
	public static T FromBase64<T>(string settings)
	{
	   using (var stream = new MemoryStream(Convert.FromBase64String(settings)))
	   {
	       var formatter = new BinaryFormatter();
	       stream.Seek(0, SeekOrigin.Begin);
	       return (T)formatter.Deserialize(stream);
	   }
	}
	
	public static T Load<T>(Uri filename)
	{
		BinaryFormatter reader = new BinaryFormatter();
        FileStream input = new FileStream(filename.LocalPath, FileMode.Open, FileAccess.Read );
        T t2 =(T)reader.Deserialize( input );
		return t2;
	}
}
