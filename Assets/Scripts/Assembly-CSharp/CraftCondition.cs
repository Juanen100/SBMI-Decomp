using System.Collections.Generic;

public abstract class CraftCondition : MatchableCondition
{
	public const int BUILDING_MATCHER = 0;

	public const int RESOURCE_MATCHER = 1;

	protected static void FromDictHelper(Dictionary<string, object> dict, CraftCondition objectToReturn, string loadToken, List<string> relevantTypes)
	{
		ResourceMatcher item = ResourceMatcher.FromDict(dict);
		SimulatedMatcher item2 = SimulatedMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item2);
		list.Insert(1, item);
		objectToReturn.Parse(dict, loadToken, relevantTypes, list);
	}

	public override string Description(Game game)
	{
		if (base.Matchers[0].HasRequirements())
		{
			if (base.Matchers[1].HasRequirements())
			{
				return string.Format(Language.Get("!!COND_CRAFT_RESOURCE_AT_PLACE"), Language.Get(base.Matchers[1].DescribeSubject(game)), Language.Get(base.Matchers[0].DescribeSubject(game)));
			}
			return string.Format(Language.Get("!!COND_CRAFT_AT"), Language.Get(base.Matchers[0].DescribeSubject(game)));
		}
		if (base.Matchers[1].HasRequirements())
		{
			return string.Format(Language.Get("!!COND_CRAFT"), Language.Get(base.Matchers[1].DescribeSubject(game)));
		}
		return string.Empty;
	}
}
