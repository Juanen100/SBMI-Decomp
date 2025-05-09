using System.Collections.Generic;

public class RushHungerAction : PersistedSimulatedAction
{
	public const string RUSH_HUNGER = "rh";

	public const ulong INVALID_ULONG = ulong.MaxValue;

	private Cost rushCost;

	private ulong readyTime;

	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	public RushHungerAction(Identity id, Cost rushCost, ulong nextReadyTime)
		: base("rh", id, typeof(RushHungerAction).ToString())
	{
		this.rushCost = rushCost;
		readyTime = nextReadyTime;
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["rush_cost"] = rushCost.ToDict();
		dictionary["ready_time"] = readyTime;
		return dictionary;
	}

	public new static RushHungerAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		Cost cost = Cost.FromDict((Dictionary<string, object>)data["rush_cost"]);
		ulong nextReadyTime = TFUtils.LoadUlong(data, "ready_time");
		return new RushHungerAction(id, cost, nextReadyTime);
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
		simulated.timebarMixinArgs.hasTimebar = false;
		ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
		entity.HungryAt = readyTime;
		game.resourceManager.Apply(rushCost, game);
		base.Apply(game, utcNow);
	}

	public override void AddEnvelope(ulong time, string tag)
	{
		base.AddEnvelope(time, tag);
		if (readyTime == ulong.MaxValue)
		{
			readyTime = time;
		}
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		ResourceManager.ApplyCostToGameState(rushCost, gameState);
		ResidentEntity.UpdateHungerTimeInGameState(gameState, target, GetTime());
		base.Confirm(gameState);
	}

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
	}
}
