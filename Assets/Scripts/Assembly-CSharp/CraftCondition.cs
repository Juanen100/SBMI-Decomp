using System.Collections.Generic;

public abstract class CraftCondition : MatchableCondition
{
	public const int BUILDING_MATCHER = 0;

	public const int RESOURCE_MATCHER = 1;

	protected static void FromDictHelper(Dictionary<string, object> dict, CraftCondition objectToReturn, string loadToken, List<string> relevantTypes)
	{
	}

	public override string Description(Game game)
	{
		return null;
	}
}
