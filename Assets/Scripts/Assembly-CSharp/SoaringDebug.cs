using MTools;
using UnityEngine;

public static class SoaringDebug
{
	public enum LogToHandlerType
	{
		none = 0,
		verbose = 1,
		brief = 2
	}

	private static string[] LogTypesName;

	private static bool LogToConsole;

	private static string debugFileName;

	private static bool LogToFile;

	private static string LogTimeStamp;

	private static MBinaryWriter Writer;

	private static LogToHandlerType LogToHandler;

	public static bool IsLoggingToConsole
	{
		get
		{
			return false;
		}
	}

	public static bool IsLoggingToFile
	{
		get
		{
			return false;
		}
	}

	public static bool IsUsingLogToHandler
	{
		get
		{
			return false;
		}
	}

	public static string DebugFileName
	{
		get
		{
			return null;
		}
	}

	static SoaringDebug()
	{
	}

	public static void EnableLogToConsole(bool log)
	{
	}

	public static void EnableHandler(LogToHandlerType log)
	{
	}

	public static void EnableLogToFile(bool log)
	{
	}

	public static void Log(string text)
	{
	}

	public static void Log(string text, LogType lType)
	{
	}

	private static void WriteLoggedCallbackHandler(string logString, string stackTrace, LogType type)
	{
	}

	public static void DebugListTextures(string stamp)
	{
	}
}
