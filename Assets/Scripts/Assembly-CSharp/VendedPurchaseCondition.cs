using System.Collections.Generic;

public class VendedPurchaseCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "vended_purchase";

	public const int BUILDING_MATCHER = 0;

	public const int RESOURCE_MATCHER = 1;

	public static VendedPurchaseCondition FromDict(Dictionary<string, object> dict)
	{
		return null;
	}

	public override string Description(Game game)
	{
		return null;
	}
}
