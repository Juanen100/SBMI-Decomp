using System;
using System.Collections.Generic;

public class NotificationManager : ITriggerObserver
{
	public const string NOTIFICATION_TIME = "notification_time";

	public const string NOTIFICATION_LABEL = "notification_label";

	private const string NOTIFICATIONS_PATH = "Notifications";

	private List<Notification> notificationList;

	private Dictionary<string, int> sentNotifications;

	private string[] GetFilesToLoad()
	{
		return null;
	}

	private string GetFilePathFromString(string filePath)
	{
		return null;
	}

	private Notification LoadNotificationFromFile(string filePath)
	{
		return null;
	}

	private void LoadNotificationsFromSpread()
	{
	}

	public void ProcessTrigger(ITrigger trigger, Game game)
	{
	}

	public static int SendNotification(string body, long delaySeconds, string label, string sound)
	{
		return 0;
	}

	public static long ConvertDateTimeToTicks(DateTime dtInput)
	{
		return 0L;
	}

	public void CancelNotification(string label)
	{
	}

	public static void CancelAllNotifications()
	{
	}

	public static void AddAnnoyingNotifications(Game game)
	{
	}
}
