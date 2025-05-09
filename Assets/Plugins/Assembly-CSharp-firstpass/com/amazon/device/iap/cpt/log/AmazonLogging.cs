using System;
using UnityEngine;

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
			if (reportLevel != AmazonLoggingLevel.Silent)
			{
				string message2 = string.Format("{0} error: {1}", service, message);
				switch (reportLevel)
				{
				case AmazonLoggingLevel.ErrorsAsExceptions:
					throw new Exception(message2);
				case AmazonLoggingLevel.Critical:
				case AmazonLoggingLevel.Errors:
				case AmazonLoggingLevel.Warnings:
				case AmazonLoggingLevel.Verbose:
					Debug.LogError(message2);
					break;
				}
			}
		}

		public static void LogWarning(AmazonLoggingLevel reportLevel, string service, string message)
		{
			switch (reportLevel)
			{
			case AmazonLoggingLevel.Silent:
			case AmazonLoggingLevel.Critical:
			case AmazonLoggingLevel.ErrorsAsExceptions:
			case AmazonLoggingLevel.Errors:
				break;
			case AmazonLoggingLevel.Warnings:
			case AmazonLoggingLevel.Verbose:
				Debug.LogWarning(string.Format("{0} warning: {1}", service, message));
				break;
			}
		}

		public static void Log(AmazonLoggingLevel reportLevel, string service, string message)
		{
			if (reportLevel == AmazonLoggingLevel.Verbose)
			{
				Debug.Log(string.Format("{0}: {1}", service, message));
			}
		}

		public static SDKLoggingLevel pluginToSDKLoggingLevel(AmazonLoggingLevel pluginLoggingLevel)
		{
			switch (pluginLoggingLevel)
			{
			case AmazonLoggingLevel.Silent:
				return SDKLoggingLevel.LogOff;
			case AmazonLoggingLevel.Critical:
				return SDKLoggingLevel.LogCritical;
			case AmazonLoggingLevel.ErrorsAsExceptions:
			case AmazonLoggingLevel.Errors:
				return SDKLoggingLevel.LogError;
			case AmazonLoggingLevel.Warnings:
			case AmazonLoggingLevel.Verbose:
				return SDKLoggingLevel.LogWarning;
			default:
				return SDKLoggingLevel.LogWarning;
			}
		}
	}
}
