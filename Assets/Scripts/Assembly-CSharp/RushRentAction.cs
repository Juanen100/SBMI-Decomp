using System.Collections.Generic;

public class RushRentAction : PersistedSimulatedAction
{
	public const string RUSH_RENT = "rr";

	public const ulong INVALID_ULONG = ulong.MaxValue;

	private Cost rushCost;

	private ulong rentReadyTime;

	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	public RushRentAction(Identity id, Cost rushCost, ulong nextRentReadyTime)
		: base("rr", id, typeof(RushRentAction).ToString())
	{
		this.rushCost = rushCost;
		rentReadyTime = nextRentReadyTime;
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["cost"] = rushCost.ToDict();
		dictionary["rent_ready_time"] = rentReadyTime;
		return dictionary;
	}

	public new static RushRentAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		Cost cost = Cost.FromDict((Dictionary<string, object>)data["cost"]);
		ulong nextRentReadyTime = TFUtils.LoadUlong(data, "rent_ready_time");
		return new RushRentAction(id, cost, nextRentReadyTime);
	}

	public override void Apply(Game game, ulong utcNow)
	{
		Simulated simulated = game.simulation.FindSimulated(target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		simulated.ClearPendingCommands();
		game.simulation.Router.CancelMatching(Command.TYPE.COMPLETE, simulated.Id, simulated.Id);
		BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
		if (entity.HasDecorator<PeriodicProductionDecorator>())
		{
			entity.GetDecorator<PeriodicProductionDecorator>().ProductReadyTime = rentReadyTime;
			simulated.EnterInitialState(EntityManager.BuildingActions["produced"], game.simulation);
		}
		else
		{
			simulated.EnterInitialState(EntityManager.BuildingActions["active"], game.simulation);
		}
		game.resourceManager.Apply(rushCost, game);
		base.Apply(game, utcNow);
	}

	public override void AddEnvelope(ulong time, string tag)
	{
		base.AddEnvelope(time, tag);
		if (rentReadyTime == ulong.MaxValue)
		{
			rentReadyTime = time;
		}
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		ResourceManager.ApplyCostToGameState(rushCost, gameState);
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["buildings"];
		string targetString = target.Describe();
		Dictionary<string, object> dictionary = (Dictionary<string, object>)list.Find((object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString));
		dictionary["rent_ready_time"] = rentReadyTime;
		base.Confirm(gameState);
	}

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
	}
}
