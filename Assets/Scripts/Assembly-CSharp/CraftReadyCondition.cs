using System.Collections.Generic;

public class CraftReadyCondition : CraftCondition
{
	public const string LOAD_TOKEN = "craft_ready";

	public static CraftReadyCondition FromDict(Dictionary<string, object> dict)
	{
		CraftReadyCondition craftReadyCondition = new CraftReadyCondition();
		CraftCondition.FromDictHelper(dict, craftReadyCondition, "craft_ready", new List<string> { typeof(CraftCompleteAction).ToString() });
		return craftReadyCondition;
	}
}
