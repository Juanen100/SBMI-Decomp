using System.Collections.Generic;

public class PushNotificationManager
{
	public const string PUSH_NOTIFICATION_ACTION_VALUE = "push_notification_action";

	public PushNotificationManager(Session session)
	{
		session.RegisterExternalCallback("push_notification_action", HandlePushNotificationAction);
	}

	public void HandlePushNotificationAction(Dictionary<string, object> dict, object userData)
	{
		TFUtils.DebugLog("Received push notification action");
		if (dict.ContainsKey("id"))
		{
			string text = (string)dict["id"];
			if (text == "1")
			{
				TFUtils.GotoAppstore();
			}
		}
	}
}
