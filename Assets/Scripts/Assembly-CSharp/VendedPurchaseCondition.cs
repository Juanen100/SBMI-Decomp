using System.Collections.Generic;

public class VendedPurchaseCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "vended_purchase";

	public const int BUILDING_MATCHER = 0;

	public const int RESOURCE_MATCHER = 1;

	public static VendedPurchaseCondition FromDict(Dictionary<string, object> dict)
	{
		ResourceMatcher item = ResourceMatcher.FromDict(dict);
		SimulatedMatcher item2 = SimulatedMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item2);
		list.Insert(1, item);
		VendedPurchaseCondition vendedPurchaseCondition = new VendedPurchaseCondition();
		vendedPurchaseCondition.Parse(dict, "vended_purchase", new List<string> { "va" }, list);
		return vendedPurchaseCondition;
	}

	public override string Description(Game game)
	{
		if (base.Matchers[0].HasRequirements())
		{
			return string.Format(Language.Get("!!COND_VEND_FROM"), Language.Get(base.Matchers[0].DescribeSubject(game)));
		}
		return string.Format(Language.Get("!!COND_VEND_PURCHASE"), Language.Get(base.Matchers[1].DescribeSubject(game)));
	}
}
