using System.Collections.Generic;

public class FeedUnitCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "feed_unit";

	public const int UNIT_MATCHER = 0;

	public const int RESOURCE_MATCHER = 1;

	public static FeedUnitCondition FromDict(Dictionary<string, object> dict)
	{
		ResourceMatcher item = ResourceMatcher.FromDict(dict);
		SimulatedMatcher item2 = SimulatedMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item2);
		list.Insert(1, item);
		FeedUnitCondition feedUnitCondition = new FeedUnitCondition();
		feedUnitCondition.Parse(dict, "feed_unit", new List<string> { typeof(FeedUnitAction).ToString() }, list);
		return feedUnitCondition;
	}

	public override string Description(Game game)
	{
		if (base.Matchers[0].HasRequirements())
		{
			if (base.Matchers[1].HasRequirements())
			{
				return string.Format(Language.Get("!!COND_FEED_UNIT_RESOURCE"), Language.Get(base.Matchers[0].DescribeSubject(game)), Language.Get(base.Matchers[1].DescribeSubject(game)));
			}
			return string.Format(Language.Get("!!COND_FEED_UNIT"), Language.Get(base.Matchers[0].DescribeSubject(game)));
		}
		return string.Format(Language.Get("!!COND_FEED_SOMEONE_RESOURCE"), Language.Get(base.Matchers[1].DescribeSubject(game)));
	}
}
