using System.Collections.Generic;

public class RushRentCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "rush_rent";

	public const int BUILDING_MATCHER = 0;

	public const int RESOURCE_MATCHER = 1;

	public static RushRentCondition FromDict(Dictionary<string, object> dict)
	{
		return null;
	}

	public override string Description(Game game)
	{
		return null;
	}
}
