using System;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

public class NotificationManager : ITriggerObserver
{
	public const string NOTIFICATION_TIME = "notification_time";

	public const string NOTIFICATION_LABEL = "notification_label";

	private const string NOTIFICATIONS_PATH = "Notifications";

	private List<Notification> notificationList = new List<Notification>();

	private Dictionary<string, int> sentNotifications = new Dictionary<string, int>();

	public NotificationManager()
	{
		LoadNotificationsFromSpread();
	}

	private string[] GetFilesToLoad()
	{
		return Config.NOTIFICATIONS_PATH;
	}

	private string GetFilePathFromString(string filePath)
	{
		return filePath;
	}

	private Notification LoadNotificationFromFile(string filePath)
	{
		TFUtils.DebugLog("Loading Notification from file " + filePath);
		string json = TFUtils.ReadAllText(filePath);
		Dictionary<string, object> data = (Dictionary<string, object>)Json.Deserialize(json);
		return Notification.FromDict(data);
	}

	private void LoadNotificationsFromSpread()
	{
		string text = "Notifications";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, "id").ToString());
			dictionary.Add("message", instance.GetStringCell(text, rowName, "message"));
			dictionary.Add("notification_sound", instance.GetStringCell(text, rowName, "notification sound"));
			dictionary.Add("conditions", new Dictionary<string, object>
			{
				{ "id", 1 },
				{
					"type",
					instance.GetStringCell(text, rowName, "condition type")
				},
				{
					"simulated_type",
					instance.GetStringCell(text, rowName, "condition simulated type")
				}
			});
			notificationList.Add(Notification.FromDict(dictionary));
		}
	}

	public void ProcessTrigger(ITrigger trigger, Game game)
	{
		bool flag = !SBSettings.SoaringProduction;
		foreach (Notification notification in notificationList)
		{
			notification.conditions.Recalculate(game, trigger);
			ConditionResult conditionResult = notification.conditions.Examine();
			if (conditionResult != ConditionResult.PASS)
			{
				continue;
			}
			Trigger trigger2 = (Trigger)trigger;
			if (trigger2.Data.ContainsKey("notification_time") && trigger2.Data.ContainsKey("notification_label"))
			{
				DateTime fireDate = (DateTime)trigger2.Data["notification_time"];
				string text = (string)trigger2.Data["notification_label"];
				if (flag)
				{
					SoaringDebug.Log(text + ": " + fireDate.ToLongDateString() + " " + fireDate.ToShortTimeString());
				}
				int value = notification.Send(fireDate, text);
				sentNotifications[text] = value;
				notification.Reset();
				if (sentNotifications.Count >= 64 && flag)
				{
					SoaringDebug.Log("Warning: Local Notification Overflow. Can Not exceed 64", LogType.Error);
				}
			}
		}
	}

	public static int SendNotification(string body, long delaySeconds, string label, string sound)
	{
		if(Application.platform == RuntimePlatform.Android)
			return EtceteraAndroid.scheduleNotification(delaySeconds, "spongebob", body, body, string.Empty);
		return -1;
	}

	public static long ConvertDateTimeToTicks(DateTime dtInput)
	{
		long num = 0L;
		return dtInput.Ticks;
	}

	public void CancelNotification(string label)
	{
		if (sentNotifications.ContainsKey(label))
		{
			int notificationId = sentNotifications[label];
			EtceteraAndroid.cancelNotification(notificationId);
			sentNotifications.Remove(label);
		}
	}

	public static void CancelAllNotifications()
	{
	}

	public static void AddAnnoyingNotifications(Game game)
	{
		double num = 9000.0;
		int num2 = 3;
		for (int i = 0; i < 4; i++)
		{
			SendNotification(Language.Get("!!NOTIFY_DAILY_MESSAGE"), ConvertDateTimeToTicks(DateTime.Now.AddHours(2.5)) + ConvertDateTimeToTicks(DateTime.Now.AddDays(i)), string.Empty, null);
			TFUtils.ErrorLog("\nNOTIFY_DAILY_MESSAGE triggered properly");
		}
		SendNotification(Language.Get("!!NOTIFY_SB_MISSES_YOU"), ConvertDateTimeToTicks(DateTime.Now.AddDays(1.0)), string.Empty, null);
		SendNotification(Language.Get("!!NOTIFY_SB_MISSES_YOU"), ConvertDateTimeToTicks(DateTime.Now.AddDays(3.0)), string.Empty, null);
		SendNotification(Language.Get("!!NOTIFY_SB_MISSES_YOU"), ConvertDateTimeToTicks(DateTime.Now.AddDays(7.0)), string.Empty, null);
		if (game.resourceManager.Resources[ResourceManager.LEVEL].Amount >= num2 || game.resourceManager.Query(ResourceManager.LEVEL) >= num2)
		{
			SendNotification(Language.Get("!!NOTIFY_FEED_RESIDENTS"), ConvertDateTimeToTicks(DateTime.Now.AddDays(3.0)), string.Empty, null);
			TFUtils.ErrorLog("\nNOTIFY_FEED_RESIDENTS triggered properly");
		}
		if (game.costumeManager.IsCostumeUnlocked(21) || game.questManager.IsQuestCompleted(3422u))
		{
		}
		if (!game.resourceManager.HasEnough(9020, 1) && !game.questManager.IsQuestCompleted(2428u))
		{
		}
	}
}
