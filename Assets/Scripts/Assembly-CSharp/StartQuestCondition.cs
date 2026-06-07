using System.Collections.Generic;

public class StartQuestCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "start_quest";

	public const int QUEST_MATCHER = 0;

	public static StartQuestCondition FromDict(Dictionary<string, object> dict)
	{
		return null;
	}

	public override string Description(Game game)
	{
		return null;
	}
}
