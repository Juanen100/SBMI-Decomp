using System;
using System.Collections.Generic;

public class RushRestockAction : PersistedSimulatedAction
{
	public const string RUSH_RESTOCK = "rrs";

	private Cost rushCost;

	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	public RushRestockAction(Identity id, Cost rushCost)
		: base("rrs", id, typeof(RushRestockAction).ToString())
	{
		this.rushCost = rushCost;
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["rush_cost"] = rushCost.ToDict();
		return dictionary;
	}

	public new static RushRestockAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		Cost cost = Cost.FromDict((Dictionary<string, object>)data["rush_cost"]);
		return new RushRestockAction(id, cost);
	}

	public override void Apply(Game game, ulong utcNow)
	{
		Simulated simulated = game.simulation.FindSimulated(target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		VendingDecorator entity = simulated.GetEntity<VendingDecorator>();
		if (entity != null)
		{
			entity.RestockTime = GetTime();
		}
		game.resourceManager.Apply(rushCost, game);
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		ResourceManager.ApplyCostToGameState(rushCost, gameState);
		string targetString = target.Describe();
		Predicate<object> match = (object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString);
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["buildings"];
		Dictionary<string, object> dictionary = (Dictionary<string, object>)list.Find(match);
		dictionary["general_restock"] = GetTime();
		base.Confirm(gameState);
	}
}
