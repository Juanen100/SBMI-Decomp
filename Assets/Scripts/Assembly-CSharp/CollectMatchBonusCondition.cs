using System.Collections.Generic;

public class CollectMatchBonusCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "collect_match_bonus";

	public const int RESIDENT_MATCHER = 0;

	public const int RESOURCE_MATCHER = 1;

	public static CollectMatchBonusCondition FromDict(Dictionary<string, object> dict)
	{
		ResourceMatcher item = ResourceMatcher.FromDict(dict);
		SimulatedMatcher item2 = SimulatedMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item2);
		list.Insert(1, item);
		CollectMatchBonusCondition collectMatchBonusCondition = new CollectMatchBonusCondition();
		collectMatchBonusCondition.Parse(dict, "collect_match_bonus", new List<string> { "BonusPickup" }, list);
		return collectMatchBonusCondition;
	}

	public override string Description(Game game)
	{
		if (base.Matchers[0].HasRequirements())
		{
			if (base.Matchers[1].HasRequirements())
			{
				return string.Format(Language.Get("!!COND_MATCH_WISH_ALL"), Language.Get(base.Matchers[0].DescribeSubject(game)), Language.Get(base.Matchers[1].DescribeSubject(game)));
			}
			return string.Format(Language.Get("!!COND_MATCH_WISH_CHARACTER"), Language.Get(base.Matchers[0].DescribeSubject(game)));
		}
		if (base.Matchers[1].HasRequirements())
		{
			return string.Format(Language.Get("!!COND_MATCH_WISH_RESOURCE"), Language.Get(base.Matchers[1].DescribeSubject(game)));
		}
		return string.Format(Language.Get("!!COND_MATCH_WISH_ANY"));
	}
}
