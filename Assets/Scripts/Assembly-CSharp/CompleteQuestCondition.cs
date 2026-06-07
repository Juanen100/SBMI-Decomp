using System.Collections.Generic;

public class CompleteQuestCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "complete_quest";

	public const int QUEST_MATCHER = 0;

	public static CompleteQuestCondition FromDict(Dictionary<string, object> dict)
	{
		return null;
	}

	public override string Description(Game game)
	{
		return null;
	}
}
