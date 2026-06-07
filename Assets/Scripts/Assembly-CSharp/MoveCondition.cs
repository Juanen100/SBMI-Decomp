using System.Collections.Generic;

public class MoveCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "move";

	public const int TARGET_MATCHER = 0;

	public static MoveCondition FromDict(Dictionary<string, object> dict)
	{
		return null;
	}

	public override string Description(Game game)
	{
		return null;
	}
}
