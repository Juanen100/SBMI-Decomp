#define ASSERTS_ON
using System.Collections.Generic;

public class ModifyResourceAmountCondition : MatchableCondition
{
	public const string LOAD_TOKEN = "update_resource";

	public const int RESOURCE_MATCHER = 0;

	public static ModifyResourceAmountCondition FromDict(Dictionary<string, object> dict)
	{
		ResourceMatcher item = ResourceMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item);
		TFUtils.Assert(list[0].IsRequired("delta") || list[0].IsRequired("balance"), string.Format("You must specify either {0} or {1} for this condition!", "delta", "balance"));
		ModifyResourceAmountCondition modifyResourceAmountCondition = new ModifyResourceAmountCondition();
		modifyResourceAmountCondition.Parse(dict, "update_resource", new List<string> { "UpdateResource" }, list);
		return modifyResourceAmountCondition;
	}

	public override string Description(Game game)
	{
		if (base.Matchers[0].IsRequired("balance"))
		{
			return string.Format(Language.Get("!!COND_CHANGE_RESOURCE"), base.Matchers[0].GetTarget("balance"), Language.Get(base.Matchers[0].DescribeSubject(game)));
		}
		if (base.Matchers[0].IsRequired("delta"))
		{
			return string.Format(Language.Get("!!COND_CHANGE_RESOURCE"), base.Matchers[0].GetTarget("delta"), Language.Get(base.Matchers[0].DescribeSubject(game)));
		}
		return string.Empty;
	}
}
