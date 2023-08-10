public static class TraceSourceExtension
{
	/*
	 Usage: var Logger = new TraceSource("loggername");
	 
	 <system.diagnostics>
		<sources>
		<source name="Config" switchValue="All">
		<listeners>
		  <add name="nlog" />
		</listeners>
		</source>    
	    <sharedListeners>
	      <add name="nlog" type="NLog.NLogTraceListener, NLog" />
	    </sharedListeners>
	    <trace autoflush="true" indentsize="4">
	      <listeners>
	        <add name="nlog" />
	        <remove name="Default" />
	      </listeners>
	    </trace>
	  </system.diagnostics>
	*/

    //Trace
    public static void LogTrace(this TraceSource traceSource, string message)
    {
        traceSource.TraceEvent(TraceEventType.Verbose, 10, message);
    }

    public static void LogTrace(this TraceSource traceSource, params object[] args)
    {
        traceSource.TraceData(TraceEventType.Verbose, 10, args);
    }

    //Info
    public static void LogInformation(this TraceSource traceSource, string message)
    {
        traceSource.TraceEvent(TraceEventType.Information, 8, message);
    }

    public static void LogInformation(this TraceSource traceSource, params object[] args)
    {
        traceSource.TraceData(TraceEventType.Information, 8, args);
    }

    //Warning
    public static void LogWarning(this TraceSource traceSource, string message)
    {
        traceSource.TraceEvent(TraceEventType.Warning, 4, message);
    }

    public static void LogWarning(this TraceSource traceSource, params object[] args)
    {
        traceSource.TraceData(TraceEventType.Warning, 4, args);
    }

    //Error
    public static void LogError(this TraceSource traceSource, string message)
    {
        traceSource.TraceEvent(TraceEventType.Error, 2, message);
    }

    public static void LogError(this TraceSource traceSource, params object[] args)
    {
        traceSource.TraceData(TraceEventType.Error, 2, args);
    }

    public static void LogError(this TraceSource traceSource, Exception exception)
    {
        traceSource.TraceEvent(TraceEventType.Error, 1, exception.Message);

        if (exception.InnerException != null)
            LogException(traceSource, exception.InnerException);
    }

    public static void LogException(this TraceSource traceSource, string message)
    {
        traceSource.TraceEvent(TraceEventType.Critical,1, message);
    }

    public static void LogException(this TraceSource traceSource, params object[] args)
    {
        traceSource.TraceData(TraceEventType.Critical, 1, args);
    }

    public static void LogException(this TraceSource traceSource, Exception exception)
    {
        traceSource.TraceEvent(TraceEventType.Critical, 1, exception.Message);

        if (exception.InnerException != null)
            LogException(traceSource, exception.InnerException);
    }

    public static void LogStart(this TraceSource traceSource,string message)
    {
        traceSource.TraceEvent(TraceEventType.Start, 256, message);
    }
    public static void LogStart(this TraceSource traceSource, params object[] args)
    {
        traceSource.TraceData(TraceEventType.Start, 256, args);
    }

    public static void LogSuspend(this TraceSource traceSource, string message)
    {
        traceSource.TraceEvent(TraceEventType.Suspend, 1024, message);
    }

    public static void LogSuspend(this TraceSource traceSource, params object[] args)
    {
        traceSource.TraceData(TraceEventType.Suspend, 1024, args);
    }

    public static void LogResume(this TraceSource traceSource, string message)
    {
        traceSource.TraceEvent(TraceEventType.Resume, 2048, message);
    }

    public static void LogResume(this TraceSource traceSource, params object[] args)
    {
        traceSource.TraceData(TraceEventType.Resume, 2048, args);
    }

    public static void LogStop(this TraceSource traceSource, string message)
    {
        traceSource.TraceEvent(TraceEventType.Stop, 512, message);
    }

    public static void LogStop(this TraceSource traceSource, params object[] args)
    {
        traceSource.TraceData(TraceEventType.Stop, 512, args);
    }

    public static void LogTransfer(this TraceSource traceSource, string message)
    {
        traceSource.TraceEvent(TraceEventType.Transfer, 4096, message);
    }

    public static void LogTransfer(this TraceSource traceSource, params object[] args)
    {
        traceSource.TraceData(TraceEventType.Transfer, 4096, args);
    }


}
