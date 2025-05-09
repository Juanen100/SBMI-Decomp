using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

public class TFAnalytics
{
	private const bool LOG_ANALYTICS = false;

	private const int MAX_NAME_CHARACTERS = 32;

	public static void LogEvent(string eventName, Dictionary<string, object> eventData)
	{
		eventName = ValidateEventName(eventName);
		string text = Json.Serialize(eventData);
	}

	public static void LogRevenueTracking(int valueCents)
	{
		Debug.Log("-------------------valueCents " + valueCents);
		logRevenueTracking(valueCents);
	}

	private static void ThreadedLogEventData(object obj)
	{
		List<string> list = (List<string>)obj;
		logEventWithData(list[0], list[1]);
	}

	private static void ThreadedLogRevenue(object obj)
	{
		logRevenueTracking((int)obj);
	}

	private static void logEventWithData(string eventName, string eventData)
	{
	}

	private static void logRevenueTracking(int valueCents)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("revenue_tracking", valueCents);
		Upsight.reportCustomEvent(dictionary);
	}

	private static string ValidateEventName(string eventName)
	{
		if (eventName.Length > 32)
		{
			TFUtils.WarningLog("Requesting analytics event that exceeds maximum character limit of 32!\neventName=" + eventName);
			eventName = eventName.Substring(0, 32);
		}
		return eventName;
	}
}
