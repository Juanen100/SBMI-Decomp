using System;
using System.Collections.Generic;

public class ResourceMatcher : Matcher
{
	public const string RESOURCE_ID = "resource_id";

	public const string BALANCE = "balance";

	public const string DELTA = "delta";

	public static ResourceMatcher FromDict(Dictionary<string, object> dict)
	{
		ResourceMatcher resourceMatcher = new ResourceMatcher();
		resourceMatcher.RegisterProperty("resource_id", dict, resourceMatcher.ResourceIdMatchFn);
		resourceMatcher.RegisterProperty("balance", dict, resourceMatcher.BalanceMatchFn);
		resourceMatcher.RegisterProperty("delta", dict, resourceMatcher.DeltaMatchFn);
		return resourceMatcher;
	}

	public override string DescribeSubject(Game game)
	{
		if (game != null && game.resourceManager != null)
		{
			Resource resource = game.resourceManager.Resources[int.Parse(GetTarget("resource_id"))];
			if (resource.Amount > 1)
			{
				return resource.Name_Plural;
			}
			return resource.Name;
		}
		return string.Empty;
	}

	private uint ResourceIdMatchFn(MatchableProperty idProperty, Dictionary<string, object> triggerData, Game game)
	{
		int key = int.Parse(idProperty.Target.ToString());
		if (triggerData.ContainsKey("resource_amounts"))
		{
			Dictionary<int, int> dictionary = AmountDictionary.FromJSONDict(TFUtils.LoadDict(triggerData, "resource_amounts"));
			if (dictionary.ContainsKey(key))
			{
				return (uint)Math.Abs(dictionary[key]);
			}
		}
		return 0u;
	}

	private uint BalanceMatchFn(MatchableProperty balanceProperty, Dictionary<string, object> triggerData, Game game)
	{
		int resourceId = int.Parse(GetProperty("resource_id").Target.ToString());
		int amount = game.resourceManager.Query(resourceId);
		return CompareOperandRangesToAmount(balanceProperty.Target, amount);
	}

	private uint DeltaMatchFn(MatchableProperty deltaProperty, Dictionary<string, object> triggerData, Game game)
	{
		int num = int.Parse(GetProperty("resource_id").Target.ToString());
		if (triggerData.ContainsKey("resource_amounts"))
		{
			Dictionary<string, object> dictionary = triggerData["resource_amounts"] as Dictionary<string, object>;
			if (dictionary != null)
			{
				if (dictionary.ContainsKey(num.ToString()))
				{
					return CompareOperandRangesToAmount(deltaProperty.Target, TFUtils.LoadInt(dictionary, num.ToString()));
				}
				return 0u;
			}
		}
		return 0u;
	}
}
