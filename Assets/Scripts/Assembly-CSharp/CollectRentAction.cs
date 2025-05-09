using System.Collections.Generic;

public class CollectRentAction : PersistedSimulatedAction
{
	public const string COLLECT_RENT = "cr";

	public const ulong INVALID_ULONG = ulong.MaxValue;

	public const string PICKUP_TRIGGERTYPE = "RentPickup";

	public Reward reward;

	public ulong rentReadyTime = ulong.MaxValue;

	public ulong rentPeriod = ulong.MaxValue;

	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	public CollectRentAction(Simulated building, Reward reward)
		: base("cr", building.Id, "RentPickup")
	{
		PeriodicProductionDecorator entity = building.GetEntity<PeriodicProductionDecorator>();
		this.reward = reward;
		rentPeriod = entity.RentProductionTime;
	}

	public CollectRentAction(Simulated building, Reward reward, ulong rentReadyTime)
		: base("cr", building.Id, "RentPickup")
	{
		PeriodicProductionDecorator entity = building.GetEntity<PeriodicProductionDecorator>();
		this.reward = reward;
		rentPeriod = entity.RentProductionTime;
		this.rentReadyTime = rentReadyTime;
	}

	private CollectRentAction(Identity id, Reward reward, ulong rentReadyTime)
		: base("cr", id, "RentPickup")
	{
		this.reward = reward;
		this.rentReadyTime = rentReadyTime;
	}

	public new static CollectRentAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		Reward reward = Reward.FromObject(data["reward"]);
		ulong num = TFUtils.LoadUlong(data, "rentReadyTime");
		CollectRentAction collectRentAction = new CollectRentAction(id, reward, num);
		collectRentAction.DropTargetDataFromDict(data);
		return collectRentAction;
	}

	public override void AddEnvelope(ulong time, string tag)
	{
		base.AddEnvelope(time, tag);
		if (rentReadyTime == ulong.MaxValue && rentPeriod != ulong.MaxValue)
		{
			rentReadyTime = time + rentPeriod;
		}
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["reward"] = reward.ToDict();
		dictionary["rentReadyTime"] = rentReadyTime;
		DropTargetDataToDict(dictionary);
		return dictionary;
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
		PeriodicProductionDecorator entity = simulated.GetEntity<PeriodicProductionDecorator>();
		if (entity == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		ulong num = (entity.ProductReadyTime = GetTime() + entity.RentProductionTime);
		if (num <= utcNow)
		{
			simulated.EnterInitialState(EntityManager.BuildingActions["produced"], game.simulation);
		}
		else
		{
			simulated.EnterInitialState(EntityManager.BuildingActions["producing"], game.simulation);
		}
		game.ApplyReward(reward, GetTime());
		AddPickup(game.simulation);
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["buildings"];
		string targetString = target.Describe();
		Dictionary<string, object> dictionary = (Dictionary<string, object>)list.Find((object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString));
		dictionary["rent_ready_time"] = rentReadyTime;
		RewardManager.ApplyToGameState(reward, GetTime(), gameState);
		AddPickupToGameState(gameState);
		base.Confirm(gameState);
	}

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
		reward.AddDataToTrigger(ref data);
	}
}
