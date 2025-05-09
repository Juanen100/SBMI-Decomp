using System.Collections.Generic;

public class CraftStartCondition : CraftCondition
{
	public const string LOAD_TOKEN = "craft_start";

	public static CraftStartCondition FromDict(Dictionary<string, object> dict)
	{
		CraftStartCondition craftStartCondition = new CraftStartCondition();
		CraftCondition.FromDictHelper(dict, craftStartCondition, "craft_start", new List<string> { typeof(CraftStartAction).ToString() });
		return craftStartCondition;
	}
}
