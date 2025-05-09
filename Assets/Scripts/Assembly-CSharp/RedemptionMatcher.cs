using System.Collections.Generic;

public class RedemptionMatcher : Matcher
{
	public const string REDEMPTION_ID = "redemption_id";

	public static RedemptionMatcher FromDict(Dictionary<string, object> dict)
	{
		RedemptionMatcher redemptionMatcher = new RedemptionMatcher();
		redemptionMatcher.RegisterProperty("redemption_id", dict);
		return redemptionMatcher;
	}

	public override string DescribeSubject(Game game)
	{
		return "redeem offer";
	}
}
