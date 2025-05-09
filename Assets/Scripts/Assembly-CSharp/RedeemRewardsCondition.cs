using System.Collections.Generic;

public class RedeemRewardsCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "redeem_reward";

	public const int REDEMPTION_MATCHER = 0;

	public static RedeemRewardsCondition FromDict(Dictionary<string, object> dict)
	{
		RedemptionMatcher item = RedemptionMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item);
		RedeemRewardsCondition redeemRewardsCondition = new RedeemRewardsCondition();
		redeemRewardsCondition.Parse(dict, "redeem_reward", new List<string> { "rra" }, list);
		return redeemRewardsCondition;
	}
}
