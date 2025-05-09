using System.Collections.Generic;

public class PlaceCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "place";

	public static PlaceCondition FromDict(Dictionary<string, object> dict)
	{
		SimulatedMatcher item = SimulatedMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Add(item);
		PlaceCondition placeCondition = new PlaceCondition();
		placeCondition.Parse(dict, "place", new List<string> { typeof(NewBuildingAction).ToString() }, list);
		return placeCondition;
	}

	public override string Description(Game game)
	{
		return string.Format(Language.Get("!!PLACE_SIMULATED_TYPE"), Language.Get(base.Matchers[0].DescribeSubject(game)));
	}
}
