using System;
using System.Collections.Generic;

public class PurchaseCraftingSlotAction : PersistedSimulatedAction
{
	public const string PURCHASE_CRAFTING_SLOT = "pcs";

	private Cost cost;

	private int slots;

	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	public PurchaseCraftingSlotAction(Identity id, Cost cost, int slots)
		: base("pcs", id, typeof(PurchaseCraftingSlotAction).ToString())
	{
		this.cost = cost;
		this.slots = slots;
	}

	public new static PurchaseCraftingSlotAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		Cost cost = Cost.FromObject(data["cost"]);
		int num = TFUtils.LoadInt(data, "slots");
		return new PurchaseCraftingSlotAction(id, cost, num);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["cost"] = cost.ToDict();
		dictionary["slots"] = slots;
		return dictionary;
	}

	public override void Apply(Game game, ulong utcNow)
	{
		game.resourceManager.Apply(cost, game);
		Simulated simulated = game.simulation.FindSimulated(target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		simulated.GetEntity<BuildingEntity>().AddCraftingSlot();
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		ResourceManager.ApplyCostToGameState(cost, gameState);
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["buildings"];
		string targetString = target.Describe();
		Predicate<object> match = (object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString);
		Dictionary<string, object> dictionary = (Dictionary<string, object>)list.Find(match);
		dictionary["crafting_slots"] = slots;
		base.Confirm(gameState);
	}
}
