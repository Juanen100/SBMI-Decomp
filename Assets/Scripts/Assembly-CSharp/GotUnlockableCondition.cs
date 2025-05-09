using System.Collections.Generic;

public class GotUnlockableCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "got_unlockable";

	private const int UNLOCKABLE_MATCHER = 0;

	public static GotUnlockableCondition FromDict(Dictionary<string, object> dict)
	{
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, UnlockableMatcher.FromDict(dict));
		GotUnlockableCondition gotUnlockableCondition = new GotUnlockableCondition();
		gotUnlockableCondition.Parse(dict, "got_unlockable", new List<string> { "unlockable", "reevaluate" }, list);
		return gotUnlockableCondition;
	}

	public override string Description(Game game)
	{
		return string.Format(Language.Get("!!COND_GET_UNLOCK"), Language.Get(base.Matchers[0].DescribeSubject(game)));
	}
}
