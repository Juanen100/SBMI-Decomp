using System.Collections.Generic;

public static class UnlockableTrigger
{
	public const string TYPE = "unlockable";

	public static ITrigger CreateTrigger(string unlockableType, int unlockableDid)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["unlockable_type"] = unlockableType;
		dictionary["unlockable_id"] = unlockableDid;
		return new Trigger("unlockable", dictionary);
	}
}
