using System.Collections.Generic;

public class CollectRentCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "collect_rent";

	public const int BUILDING_MATCHER = 0;

	public const int RESOURCE_MATCHER = 1;

	public static CollectRentCondition FromDict(Dictionary<string, object> dict)
	{
		ResourceMatcher item = ResourceMatcher.FromDict(dict);
		SimulatedMatcher item2 = SimulatedMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item2);
		list.Insert(1, item);
		CollectRentCondition collectRentCondition = new CollectRentCondition();
		collectRentCondition.Parse(dict, "collect_rent", new List<string> { "RentPickup" }, list);
		return collectRentCondition;
	}

	public override string Description(Game game)
	{
		if (base.Matchers[0].HasRequirements())
		{
			return string.Format(Language.Get("!!COND_COLLECT_FROM"), Language.Get(base.Matchers[0].DescribeSubject(game)));
		}
		return string.Format(Language.Get("!!COND_COLLECT_AS_RENT"), Language.Get(base.Matchers[1].DescribeSubject(game)));
	}
}
