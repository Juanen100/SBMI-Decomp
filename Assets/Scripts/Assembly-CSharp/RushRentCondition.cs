using System.Collections.Generic;

public class RushRentCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "rush_rent";

	public const int BUILDING_MATCHER = 0;

	public const int RESOURCE_MATCHER = 1;

	public static RushRentCondition FromDict(Dictionary<string, object> dict)
	{
		ResourceMatcher item = ResourceMatcher.FromDict(dict);
		SimulatedMatcher item2 = SimulatedMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item2);
		list.Insert(1, item);
		RushRentCondition rushRentCondition = new RushRentCondition();
		rushRentCondition.Parse(dict, "rush_rent", new List<string> { typeof(RushRentAction).ToString() }, list);
		return rushRentCondition;
	}

	public override string Description(Game game)
	{
		if (base.Matchers[0].HasRequirements())
		{
			return string.Format(Language.Get("!!COND_RUSH_COLLECTION_ON_PLACE"), Language.Get(base.Matchers[0].DescribeSubject(game)));
		}
		return string.Format(Language.Get("!!COND_RUSH_RESOURCE_COLLECTION"), Language.Get(base.Matchers[1].DescribeSubject(game)));
	}
}
