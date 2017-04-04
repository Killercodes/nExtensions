using System;
using System.IO;
using System.Runtime.Serialization;

namespace nExtensions
{
    public static class ExceptionsExtended
    {
        public static void LogToText (this Exception exc, string logFilePath)
        {
            // Open the log file for append and write the log
            using (StreamWriter streamWriter = new StreamWriter(logFilePath, true))
            {
                streamWriter.WriteLine("********** {0} **********", DateTime.Now);
                if (exc.InnerException != null)
                {
                    streamWriter.Write("Inner Exception Type: ");
                    streamWriter.WriteLine(exc.InnerException.GetType().ToString());
                    streamWriter.Write("Inner Exception: ");
                    streamWriter.WriteLine(exc.InnerException.Message);
                    streamWriter.Write("Inner Source: ");
                    streamWriter.WriteLine(exc.InnerException.Source);
                    if (exc.InnerException.StackTrace != null)
                    {
                        streamWriter.WriteLine("Inner Stack Trace: ");
                        streamWriter.WriteLine(exc.InnerException.StackTrace);
                    }
                }
                streamWriter.Write("Exception Type: ");
                streamWriter.WriteLine(exc.GetType().ToString());
                streamWriter.WriteLine("Exception: " + exc.Message);
                streamWriter.WriteLine("Source: " + exc.Source);
                streamWriter.WriteLine("Stack Trace: ");
                if (exc.StackTrace != null)
                {
                    streamWriter.WriteLine(exc.StackTrace);
                    streamWriter.WriteLine();
                }
            }
        }



		[Serializable]
		public class Error :Exception
		{
			public Error () : base() { }
			public Error (string message) : base(message) { }
			public Error (string format, params object[] args) : base(string.Format(format, args)) { }
			public Error (string message, Exception innerException) : base(message, innerException) { }
			public Error (string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }
			protected Error (SerializationInfo info, StreamingContext context) : base(info, context) { }
		}


    }
}
