using System.Collections.Generic;

public class MoveCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "move";

	public const int TARGET_MATCHER = 0;

	public static MoveCondition FromDict(Dictionary<string, object> dict)
	{
		SimulatedMatcher item = SimulatedMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item);
		MoveCondition moveCondition = new MoveCondition();
		moveCondition.Parse(dict, "move", new List<string> { typeof(MoveAction).ToString() }, list);
		return moveCondition;
	}

	public override string Description(Game game)
	{
		return string.Format(Language.Get("!!COND_MOVE_OBJECT"), Language.Get(base.Matchers[0].DescribeSubject(game)));
	}
}
