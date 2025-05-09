using System.Collections.Generic;

public static class ReevaluateTrigger
{
	public const string TYPE = "reevaluate";

	public static ITrigger CreateTrigger()
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		return new Trigger("reevaluate", data);
	}
}
