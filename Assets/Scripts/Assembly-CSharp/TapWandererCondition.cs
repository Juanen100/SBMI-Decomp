using System.Collections.Generic;

public class TapWandererCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "tap_wanderer";

	public const int WANDERER_MATCHER = 0;

	public static TapWandererCondition FromDict(Dictionary<string, object> dict)
	{
		SimulatedMatcher item = SimulatedMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item);
		int num = -1;
		if (dict.ContainsKey("simulated_exists"))
		{
			num = TFUtils.LoadInt(dict, "simulated_exists");
		}
		TapWandererCondition tapWandererCondition = new TapWandererCondition();
		tapWandererCondition.Parse(dict, "tap_wanderer", new List<string> { typeof(TapWandererAction).ToString() }, list, num);
		return tapWandererCondition;
	}

	public override string Description(Game game)
	{
		return string.Format(Language.Get("!!COND_TAP"), Language.Get(base.Matchers[0].DescribeSubject(game)));
	}
}
