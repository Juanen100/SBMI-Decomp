using System.Collections.Generic;

public class CollectMatchBonusCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "collect_match_bonus";

	public const int RESIDENT_MATCHER = 0;

	public const int RESOURCE_MATCHER = 1;

	public static CollectMatchBonusCondition FromDict(Dictionary<string, object> dict)
	{
		return null;
	}

	public override string Description(Game game)
	{
		return null;
	}
}
