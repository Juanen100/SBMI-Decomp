using System.Collections.Generic;

public class FeedUnitCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "feed_unit";

	public const int UNIT_MATCHER = 0;

	public const int RESOURCE_MATCHER = 1;

	public static FeedUnitCondition FromDict(Dictionary<string, object> dict)
	{
		return null;
	}

	public override string Description(Game game)
	{
		return null;
	}
}
