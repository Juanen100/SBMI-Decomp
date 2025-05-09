using System.Collections.Generic;

public class AutoQuestAllDoneCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "auto_quest_all_done";

	public const int QUEST_MATCHER = 0;

	public static AutoQuestAllDoneCondition FromDict(Dictionary<string, object> dict)
	{
		QuestMatcher item = QuestMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item);
		AutoQuestAllDoneCondition autoQuestAllDoneCondition = new AutoQuestAllDoneCondition();
		autoQuestAllDoneCondition.Parse(dict, "auto_quest_all_done", new List<string> { typeof(AutoQuestAllDoneAction).ToString() }, list);
		return autoQuestAllDoneCondition;
	}

	public override string Description(Game game)
	{
		return string.Empty;
	}
}
