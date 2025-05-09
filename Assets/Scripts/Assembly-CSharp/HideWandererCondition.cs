using System.Collections.Generic;

public class HideWandererCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "hide_wanderer";

	public const int WANDERER_MATCHER = 0;

	public static HideWandererCondition FromDict(Dictionary<string, object> dict)
	{
		SimulatedMatcher item = SimulatedMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item);
		int num = -1;
		if (dict.ContainsKey("simulated_exists"))
		{
			num = TFUtils.LoadInt(dict, "simulated_exists");
		}
		HideWandererCondition hideWandererCondition = new HideWandererCondition();
		hideWandererCondition.Parse(dict, "hide_wanderer", new List<string> { typeof(HideWandererAction).ToString() }, list, num);
		return hideWandererCondition;
	}

	public override string Description(Game game)
	{
		return string.Format(Language.Get("!!COND_HIDE"), Language.Get(base.Matchers[0].DescribeSubject(game)));
	}
}
