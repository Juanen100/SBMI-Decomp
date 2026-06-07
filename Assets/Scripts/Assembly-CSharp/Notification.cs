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
	}

	public static Notification FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	public void Reset()
	{
	}

	public int Send(DateTime fireDate, string label)
	{
		return 0;
	}
}
