using System.Collections.Generic;

public class CompleteBuildingCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "complete_building";

	public const int BUILDING_MATCHER = 0;

	public const int RESOURCE_MATCHER = 1;

	public static CompleteBuildingCondition FromDict(Dictionary<string, object> dict)
	{
		SimulatedMatcher item = SimulatedMatcher.FromDict(dict);
		ResourceMatcher item2 = ResourceMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item);
		list.Insert(1, item2);
		CompleteBuildingCondition completeBuildingCondition = new CompleteBuildingCondition();
		completeBuildingCondition.Parse(dict, "complete_building", new List<string> { typeof(CompleteBuildingAction).ToString() }, list);
		return completeBuildingCondition;
	}

	public override string Description(Game game)
	{
		return string.Format(Language.Get("!!COND_BUILD"), Language.Get(base.Matchers[0].DescribeSubject(game)));
	}
}
