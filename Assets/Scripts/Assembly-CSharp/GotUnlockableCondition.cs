using System.Collections.Generic;

public class GotUnlockableCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "got_unlockable";

	private const int UNLOCKABLE_MATCHER = 0;

	public static GotUnlockableCondition FromDict(Dictionary<string, object> dict)
	{
		return null;
	}

	public override string Description(Game game)
	{
		return null;
	}
}
