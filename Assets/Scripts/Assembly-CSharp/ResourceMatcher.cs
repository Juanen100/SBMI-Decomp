using System.Collections.Generic;

public class ResourceMatcher : Matcher
{
	public const string RESOURCE_ID = "resource_id";

	public const string BALANCE = "balance";

	public const string DELTA = "delta";

	public static ResourceMatcher FromDict(Dictionary<string, object> dict)
	{
		return null;
	}

	public override string DescribeSubject(Game game)
	{
		return null;
	}

	private uint ResourceIdMatchFn(MatchableProperty idProperty, Dictionary<string, object> triggerData, Game game)
	{
		return 0u;
	}

	private uint BalanceMatchFn(MatchableProperty balanceProperty, Dictionary<string, object> triggerData, Game game)
	{
		return 0u;
	}

	private uint DeltaMatchFn(MatchableProperty deltaProperty, Dictionary<string, object> triggerData, Game game)
	{
		return 0u;
	}
}
