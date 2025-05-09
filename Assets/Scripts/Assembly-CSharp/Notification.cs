using System;
using System.Collections.Generic;

public class Notification
{
	public string message;

	public string sound;

	public ConditionState conditions;

	private LoadableCondition loadableCondition;

	public Notification(string message, string sound, LoadableCondition loadableCondition)
	{
		this.message = message;
		this.sound = sound;
		this.loadableCondition = loadableCondition;
		Reset();
	}

	public static Notification FromDict(Dictionary<string, object> data)
	{
		LoadableCondition loadableCondition = (LoadableCondition)ConditionFactory.FromDict((Dictionary<string, object>)data["conditions"]);
		string text = Language.Get(TFUtils.LoadString(data, "message"));
		string text2 = TFUtils.TryLoadString(data, "notification_sound");
		return new Notification(text, text2, loadableCondition);
	}

	public void Reset()
	{
		conditions = new ConditionState(loadableCondition);
	}

	public int Send(DateTime fireDate, string label)
	{
		long delaySeconds = (fireDate.Ticks - DateTime.Now.Ticks) / 10000000 + 1;
		return NotificationManager.SendNotification(message, delaySeconds, string.Empty, string.Empty);
	}
}
