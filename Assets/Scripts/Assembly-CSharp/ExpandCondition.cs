using System.Collections.Generic;

public class ExpandCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "expand";

	public static ExpandCondition FromDict(Dictionary<string, object> dict)
	{
		ExpandCondition expandCondition = new ExpandCondition();
		expandCondition.Parse(dict, "expand", new List<string> { typeof(NewExpansionAction).ToString() }, new List<IMatcher> { ExpansionMatcher.FromDict(dict) });
		return expandCondition;
	}

	public override string Description(Game game)
	{
		return Language.Get("!!COND_BUY_EXPANSION");
	}
}
