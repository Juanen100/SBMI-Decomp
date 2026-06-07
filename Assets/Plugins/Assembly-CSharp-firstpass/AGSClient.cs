using System;
using UnityEngine;

public class AGSClient : MonoBehaviour
{
	public const string serviceName = "AmazonGameCircle";

	public const AmazonLogging.AmazonLoggingLevel errorLevel = AmazonLogging.AmazonLoggingLevel.Verbose;

	private static bool IsReady;

	private static AmazonJavaWrapper JavaObject;

	private static readonly string PROXY_CLASS_NAME;

	public static event Action ServiceReadyEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string> ServiceNotReadyEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	static AGSClient()
	{
	}

	public static void Init()
	{
	}

	public static void Init(bool supportsLeaderboards, bool supportsAchievements, bool supportsWhispersync)
	{
	}

	public static void SetPopUpEnabled(bool enabled)
	{
	}

	public static void SetPopUpLocation(GameCirclePopupLocation location)
	{
	}

	public static void ServiceReady(string empty)
	{
	}

	public static bool IsServiceReady()
	{
		return false;
	}

	public static void release()
	{
	}

	public static void ServiceNotReady(string param)
	{
	}

	public static void ShowGameCircleOverlay()
	{
	}

	public static void LogGameCircleError(string errorMessage)
	{
	}

	public static void LogGameCircleWarning(string errorMessage)
	{
	}

	public static void Log(string message)
	{
	}
}
