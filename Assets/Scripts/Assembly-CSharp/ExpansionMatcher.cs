using System.Collections.Generic;

public class ExpansionMatcher : Matcher
{
	public const string EXPANSION_ID = "expansion_id";

	public static ExpansionMatcher FromDict(Dictionary<string, object> dict)
	{
		ExpansionMatcher expansionMatcher = new ExpansionMatcher();
		expansionMatcher.RegisterProperty("expansion_id", dict);
		return expansionMatcher;
	}

	public override string DescribeSubject(Game game)
	{
		return "expand";
	}
}
