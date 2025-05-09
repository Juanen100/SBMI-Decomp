using System.Collections.Generic;

public class ConstructedCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "constructed";

	public static ConstructedCondition FromDict(Dictionary<string, object> dict)
	{
		SimulatedMatcher item = SimulatedMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Add(item);
		ConstructedCondition constructedCondition = new ConstructedCondition();
		constructedCondition.Parse(dict, "constructed", new List<string> { "contruction_complete" }, list);
		return constructedCondition;
	}

	public override string Description(Game game)
	{
		return string.Format(Language.Get("!!PLACE_SIMULATED_TYPE"), Language.Get(base.Matchers[0].DescribeSubject(game)));
	}
}
