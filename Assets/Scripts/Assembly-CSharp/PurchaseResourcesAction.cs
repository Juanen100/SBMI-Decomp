using System.Collections.Generic;

public class PurchaseResourcesAction : PersistedTriggerableAction
{
	public const string PURCHASE_RESOURCES = "pr";

	public Cost purchasedResources;

	public Cost rmtCost;

	public TriggerableMixin Triggerable
	{
		get
		{
			return triggerable;
		}
	}

	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	private PurchaseResourcesAction(Identity id, Cost resources, Cost cost)
		: base("pr", id)
	{
		purchasedResources = resources;
		rmtCost = cost;
	}

	public PurchaseResourcesAction(Identity id, int rmtCost, Cost resources)
		: this(id, resources, new Cost(new Dictionary<int, int> { 
		{
			ResourceManager.HARD_CURRENCY,
			rmtCost
		} }))
	{
	}

	public new static PurchaseResourcesAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		Cost resources = Cost.FromObject(data["resources"]);
		Cost cost = Cost.FromObject(data["rmt_cost"]);
		return new PurchaseResourcesAction(id, resources, cost);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["resources"] = purchasedResources.ToDict();
		dictionary["rmt_cost"] = rmtCost.ToDict();
		return dictionary;
	}

	public override void Apply(Game game, ulong utcNow)
	{
		game.resourceManager.Apply(rmtCost, game);
		game.resourceManager.PurchaseResourcesWithHardCurrency(0, purchasedResources, game);
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		ResourceManager.ApplyCostToGameState(rmtCost, gameState);
		foreach (KeyValuePair<int, int> resourceAmount in purchasedResources.ResourceAmounts)
		{
			ResourceManager.AddAmountToGameState(resourceAmount.Key, resourceAmount.Value, gameState);
		}
		base.Confirm(gameState);
	}

	public virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return triggerable.BuildTrigger(GetType().ToString(), AddMoreDataToTrigger);
	}
}
