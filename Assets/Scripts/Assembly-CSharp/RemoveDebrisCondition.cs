using System.Collections.Generic;

public class RemoveDebrisCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "remove_debris";

	public const int DEBRIS_MATCHER = 0;

	public static RemoveDebrisCondition FromDict(Dictionary<string, object> dict)
	{
		SimulatedMatcher item = SimulatedMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item);
		RemoveDebrisCondition removeDebrisCondition = new RemoveDebrisCondition();
		removeDebrisCondition.Parse(dict, "remove_debris", new List<string> { typeof(DebrisCompleteAction).ToString() }, list);
		return removeDebrisCondition;
	}

	public override string Description(Game game)
	{
		return string.Format(Language.Get("!!COND_REMOVE_DEBRIS_TYPE"), Language.Get(base.Matchers[0].DescribeSubject(game)));
	}
}
