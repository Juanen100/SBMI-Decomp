using System.Collections.Generic;

public class CompleteBuildingCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "complete_building";

	public const int BUILDING_MATCHER = 0;

	public const int RESOURCE_MATCHER = 1;

	public static CompleteBuildingCondition FromDict(Dictionary<string, object> dict)
	{
		return null;
	}

	public override string Description(Game game)
	{
		return null;
	}
}
