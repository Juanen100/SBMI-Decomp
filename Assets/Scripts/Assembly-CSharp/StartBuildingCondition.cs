using System.Collections.Generic;

public class StartBuildingCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "start_building";

	public const int BUILDING_MATCHER = 0;

	public const int RESOURCE_MATCHER = 1;

	public static StartBuildingCondition FromDict(Dictionary<string, object> dict)
	{
		return null;
	}

	public override string Description(Game game)
	{
		return null;
	}
}
