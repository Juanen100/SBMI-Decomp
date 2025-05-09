using System;
using System.Diagnostics;
using MTools;
using UnityEngine;
using Debug = UnityEngine.Debug;

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
			return LogToConsole;
		}
	}

	public static bool IsLoggingToFile
	{
		get
		{
			return LogToFile;
		}
	}

	public static bool IsUsingLogToHandler
	{
		get
		{
			return LogToHandler != LogToHandlerType.none;
		}
	}

	public static string DebugFileName
	{
		get
		{
			return debugFileName;
		}
	}

	static SoaringDebug()
	{
		LogToConsole = true;
		LogToFile = true;
		LogTimeStamp = string.Empty;
		LogToHandler = LogToHandlerType.verbose;
		LogTypesName = new string[5];
		LogTypesName[1] = "Assert";
		LogTypesName[0] = "Error";
		LogTypesName[4] = "Exception";
		LogTypesName[3] = "Log";
		LogTypesName[2] = "Warning";
		if (SoaringInternal.IsProductionMode)
		{
			LogToConsole = false;
			LogToFile = false;
			LogToHandler = LogToHandlerType.none;
		}
		EnableLogToConsole(LogToConsole);
		EnableHandler(LogToHandler);
		EnableLogToFile(LogToFile);
		if (!SoaringInternal.IsProductionMode)
		{
			Debug.Log("SoaringDebug: LogToConsole " + LogToConsole + " LogToFile: " + LogToFile + " LogToHandler: " + LogToHandler.ToString());
		}
	}

	public static void EnableLogToConsole(bool log)
	{
		LogToConsole = log;
	}

	public static void EnableHandler(LogToHandlerType log)
	{
		LogToHandler = log;
		if (LogToHandler != LogToHandlerType.none)
		{
			try
			{
				DateTime utcNow = DateTime.UtcNow;
				LogTimeStamp = "_" + utcNow.ToShortDateString() + "_" + utcNow.ToLongTimeString();
				LogTimeStamp = LogTimeStamp.Replace(' ', '_');
				LogTimeStamp = LogTimeStamp.Replace(',', '_');
				LogTimeStamp = LogTimeStamp.Replace('/', '_');
				LogTimeStamp = LogTimeStamp.Replace('\\', '_');
				LogTimeStamp = LogTimeStamp.Replace(':', '_');
				Application.RegisterLogCallback(WriteLoggedCallbackHandler);
			}
			catch
			{
				LogTimeStamp = string.Empty;
			}
		}
	}

	public static void EnableLogToFile(bool log)
	{
		LogToFile = log;
		if (!LogToFile)
		{
			if (Writer != null)
			{
				Writer.Close();
			}
			Writer = null;
			return;
		}
		if (Writer != null)
		{
			Writer.Close();
		}
		Writer = null;
		string empty = string.Empty;
		if (LogTimeStamp == null)
		{
			LogTimeStamp = string.Empty;
		}
		debugFileName = "Soaring" + LogTimeStamp + ".log";
		string writePath = ResourceUtils.GetWritePath(debugFileName, empty + "Soaring/Logs", 8);
		MBinaryWriter mBinaryWriter = new MBinaryWriter();
		if (!mBinaryWriter.Open(writePath, true, true))
		{
			mBinaryWriter = null;
		}
		Writer = mBinaryWriter;
	}

	public static void Log(string text)
	{
		Log(text, LogType.Log);
	}

	public static void Log(string text, LogType lType)
	{
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		if (LogToFile && Writer != null && LogToHandler == LogToHandlerType.none)
		{
			Writer.WriteRawString("-" + LogTypesName[(int)lType] + "\nSoaring: " + text + "\n");
			Writer.Flush();
		}
		if (LogToConsole)
		{
			switch (lType)
			{
			case LogType.Error:
			case LogType.Assert:
			case LogType.Exception:
				Debug.LogError("Soaring: " + LogTypesName[(int)lType] + ": " + text);
				break;
			case LogType.Warning:
				Debug.LogWarning("Soaring: " + LogTypesName[2] + ": " + text);
				break;
			default:
				Debug.Log("Soaring: " + text);
				break;
			}
		}
	}

	private static void WriteLoggedCallbackHandler(string logString, string stackTrace, LogType type)
	{
		if (Writer == null || logString == null)
		{
			return;
		}
		string text = logString;
		if (LogToHandler == LogToHandlerType.verbose)
		{
			if (stackTrace != null)
			{
				text = text + "\n" + stackTrace;
			}
			else
			{
				try
				{
					StackTrace stackTrace2 = new StackTrace();
					text = text + "\n" + stackTrace2.ToString();
				}
				catch
				{
				}
			}
		}
		Writer.WriteRawString("-" + LogTypesName[(int)type] + ": " + text + "\n");
		Writer.Flush();
	}

	public static void DebugListTextures(string stamp)
	{
	}
}
