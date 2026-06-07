using System.Collections.Generic;

public class SimulatedMatcher : Matcher
{
	public const string INSTANCE_ID = "simulated_guid";

	public const string DEFINITION_ID = "simulated_id";

	public const string TYPE = "simulated_type";

	public static SimulatedMatcher FromDict(Dictionary<string, object> dict)
	{
		return null;
	}

	public override string DescribeSubject(Game game)
	{
		return null;
	}
}
