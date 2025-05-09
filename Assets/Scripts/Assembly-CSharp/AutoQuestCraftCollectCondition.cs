using System.Collections.Generic;

public class AutoQuestCraftCollectCondition : CraftCondition
{
	public const string LOAD_TOKEN = "auto_quest_craft_collect";

	public static AutoQuestCraftCollectCondition FromDict(Dictionary<string, object> dict)
	{
		AutoQuestCraftCollectCondition autoQuestCraftCollectCondition = new AutoQuestCraftCollectCondition();
		CraftCondition.FromDictHelper(dict, autoQuestCraftCollectCondition, "auto_quest_craft_collect", new List<string> { typeof(AutoQuestCraftCollectAction).ToString() });
		return autoQuestCraftCollectCondition;
	}
}
