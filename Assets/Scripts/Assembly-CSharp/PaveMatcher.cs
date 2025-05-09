using System.Collections.Generic;

public class PaveMatcher : Matcher
{
	public const string PAVE_TYPE = "pave_type";

	public static PaveMatcher FromDict(Dictionary<string, object> dict)
	{
		PaveMatcher paveMatcher = new PaveMatcher();
		paveMatcher.RegisterProperty("pave_type", dict);
		return paveMatcher;
	}

	public override string DescribeSubject(Game game)
	{
		return "path";
	}
}
