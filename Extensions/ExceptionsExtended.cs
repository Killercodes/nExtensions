using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

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
    }
}
