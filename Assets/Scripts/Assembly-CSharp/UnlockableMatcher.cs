using System.Collections.Generic;

public class UnlockableMatcher : Matcher
{
	public const string UNLOCKABLE_TYPE = "unlockable_type";

	public const string UNLOCKABLE_ID = "unlockable_id";

	public static UnlockableMatcher FromDict(Dictionary<string, object> dict)
	{
		return null;
	}

	public override string DescribeSubject(Game game)
	{
		return null;
	}

	public override uint MatchAmount(Game game, Dictionary<string, object> data)
	{
		return 0u;
	}
}
