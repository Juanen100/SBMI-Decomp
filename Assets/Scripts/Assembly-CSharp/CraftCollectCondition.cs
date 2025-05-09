using System.Collections.Generic;

public class CraftCollectCondition : CraftCondition
{
	public const string LOAD_TOKEN = "craft_collect";

	public static CraftCollectCondition FromDict(Dictionary<string, object> dict)
	{
		CraftCollectCondition craftCollectCondition = new CraftCollectCondition();
		CraftCondition.FromDictHelper(dict, craftCollectCondition, "craft_collect", new List<string> { "CraftPickup" });
		return craftCollectCondition;
	}
}
