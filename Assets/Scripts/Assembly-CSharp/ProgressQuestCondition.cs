using System.Collections.Generic;

public class ProgressQuestCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "progress_quest";

	public const int QUEST_MATCHER = 0;

	public static ProgressQuestCondition FromDict(Dictionary<string, object> dict)
	{
		QuestMatcher item = QuestMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item);
		ProgressQuestCondition progressQuestCondition = new ProgressQuestCondition();
		progressQuestCondition.Parse(dict, "progress_quest", new List<string> { typeof(QuestProgressAction).ToString() }, list);
		return progressQuestCondition;
	}

	public override string Description(Game game)
	{
		return string.Format(Language.Get("!!COND_PROGRESS_QUEST"), Language.Get(base.Matchers[0].DescribeSubject(game)));
	}
}
