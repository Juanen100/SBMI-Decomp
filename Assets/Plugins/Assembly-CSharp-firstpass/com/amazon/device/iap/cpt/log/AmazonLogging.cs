namespace com.amazon.device.iap.cpt.log
{
	public class AmazonLogging
	{
		public enum AmazonLoggingLevel
		{
			Silent = 0,
			Critical = 1,
			ErrorsAsExceptions = 2,
			Errors = 3,
			Warnings = 4,
			Verbose = 5
		}

		public enum SDKLoggingLevel
		{
			LogOff = 0,
			LogCritical = 1,
			LogError = 2,
			LogWarning = 3
		}

		private const string errorMessage = "{0} error: {1}";

		private const string warningMessage = "{0} warning: {1}";

		private const string logMessage = "{0}: {1}";

		public static void LogError(AmazonLoggingLevel reportLevel, string service, string message)
		{
		}

		public static void LogWarning(AmazonLoggingLevel reportLevel, string service, string message)
		{
		}

		public static void Log(AmazonLoggingLevel reportLevel, string service, string message)
		{
		}

		public static SDKLoggingLevel pluginToSDKLoggingLevel(AmazonLoggingLevel pluginLoggingLevel)
		{
			return default(SDKLoggingLevel);
		}
	}
}
