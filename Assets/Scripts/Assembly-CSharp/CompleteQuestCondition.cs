using System.Collections.Generic;

public class CompleteQuestCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "complete_quest";

	public const int QUEST_MATCHER = 0;

	public static CompleteQuestCondition FromDict(Dictionary<string, object> dict)
	{
		QuestMatcher item = QuestMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item);
		CompleteQuestCondition completeQuestCondition = new CompleteQuestCondition();
		completeQuestCondition.Parse(dict, "complete_quest", new List<string> { typeof(QuestCompleteAction).ToString() }, list);
		return completeQuestCondition;
	}

	public override string Description(Game game)
	{
		return string.Format(Language.Get("!!COND_COMPLETE_QUEST"), Language.Get(base.Matchers[0].DescribeSubject(game)));
	}
}
