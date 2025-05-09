using System.Collections.Generic;

public class StartBuildingCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "start_building";

	public const int BUILDING_MATCHER = 0;

	public const int RESOURCE_MATCHER = 1;

	public static StartBuildingCondition FromDict(Dictionary<string, object> dict)
	{
		SimulatedMatcher item = SimulatedMatcher.FromDict(dict);
		ResourceMatcher item2 = ResourceMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item);
		list.Insert(1, item2);
		StartBuildingCondition startBuildingCondition = new StartBuildingCondition();
		startBuildingCondition.Parse(dict, "start_building", new List<string> { typeof(NewBuildingAction).ToString() }, list);
		return startBuildingCondition;
	}

	public override string Description(Game game)
	{
		return string.Format(Language.Get("!!COND_START_BUILD"), Language.Get(base.Matchers[0].DescribeSubject(game)));
	}
}
