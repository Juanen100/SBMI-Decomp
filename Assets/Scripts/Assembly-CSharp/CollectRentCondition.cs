using System.Collections.Generic;

public class CollectRentCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "collect_rent";

	public const int BUILDING_MATCHER = 0;

	public const int RESOURCE_MATCHER = 1;

	public static CollectRentCondition FromDict(Dictionary<string, object> dict)
	{
		return null;
	}

	public override string Description(Game game)
	{
		return null;
	}
}
