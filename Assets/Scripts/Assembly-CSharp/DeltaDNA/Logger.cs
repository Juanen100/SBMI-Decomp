using UnityEngine;

namespace DeltaDNA
{
	public static class Logger
	{
		public enum Level
		{
			DEBUG = 0,
			INFO = 1,
			WARNING = 2,
			ERROR = 3
		}

		public const string PREFIX = "[DDSDK] ";

		private static Level sLogLevel;

		internal static Level LogLevel
		{
			get
			{
				return default(Level);
			}
		}

		public static void SetLogLevel(Level logLevel)
		{
		}

		internal static void LogDebug(string msg)
		{
		}

		internal static void LogInfo(string msg)
		{
		}

		internal static void LogWarning(string msg)
		{
		}

		internal static void LogError(string msg)
		{
		}

		private static void Log(string msg, Level level)
		{
		}

		internal static void HandleLog(string logString, string stackTrace, LogType type)
		{
		}
	}
}
