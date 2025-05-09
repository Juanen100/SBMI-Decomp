using System.Collections.Generic;

public class StartQuestCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "start_quest";

	public const int QUEST_MATCHER = 0;

	public static StartQuestCondition FromDict(Dictionary<string, object> dict)
	{
		QuestMatcher item = QuestMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item);
		StartQuestCondition startQuestCondition = new StartQuestCondition();
		startQuestCondition.Parse(dict, "start_quest", new List<string> { typeof(QuestStartAction).ToString() }, list);
		return startQuestCondition;
	}

	public override string Description(Game game)
	{
		return string.Format(Language.Get("!!COND_START_QUEST"), Language.Get(base.Matchers[0].DescribeSubject(game)));
	}
}
