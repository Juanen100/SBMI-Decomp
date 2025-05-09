#define ASSERTS_ON
using System;
using System.Collections.Generic;

public class SellAction : PersistedSimulatedAction
{
	public const string SELL = "s";

	public Cost cost;

	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	public SellAction(Identity id, Cost cost)
		: base("s", id, typeof(SellAction).ToString())
	{
		TFUtils.Assert(cost != null, "Cannot create a sell action with a null selling cost");
		this.cost = cost;
	}

	public SellAction(Simulated simulated, Cost cost)
		: this(simulated.Id, cost)
	{
	}

	public new static SellAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		Cost cost = Cost.FromDict((Dictionary<string, object>)data["cost"]);
		return new SellAction(id, cost);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["cost"] = cost.ToDict();
		return dictionary;
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		string targetString = target.Describe();
		Simulated.Building.RemoveResidentsFromGameState(gameState, targetString);
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["buildings"];
		Predicate<object> match = (object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString);
		foreach (KeyValuePair<int, int> resourceAmount in cost.ResourceAmounts)
		{
			ResourceManager.AddAmountToGameState(resourceAmount.Key, resourceAmount.Value, gameState);
		}
		list.RemoveAll(match);
		base.Confirm(gameState);
	}
}
