using System.Collections.Generic;

public class PurchaseCraftingSlotCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "purchase_production_slot";

	public const int BUILDING_MATCHER = 0;

	public static PurchaseCraftingSlotCondition FromDict(Dictionary<string, object> dict)
	{
		SimulatedMatcher item = SimulatedMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item);
		PurchaseCraftingSlotCondition purchaseCraftingSlotCondition = new PurchaseCraftingSlotCondition();
		purchaseCraftingSlotCondition.Parse(dict, "purchase_production_slot", new List<string> { typeof(PurchaseCraftingSlotAction).ToString() }, list);
		return purchaseCraftingSlotCondition;
	}

	public override string Description(Game game)
	{
		if (base.Matchers[0].HasRequirements())
		{
			return string.Format(Language.Get("!!COND_PURCHASE_PRODUCTION_SLOT"), Language.Get(base.Matchers[0].DescribeSubject(game)));
		}
		return Language.Get("!!COND_PURCHASE_PRODUCTION_SLOT_GENERIC");
	}
}
