using System.Collections.Generic;

public class RushDebrisAction : PersistedSimulatedAction
{
	public const string RUSH_DEBRIS = "rd";

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

	public RushDebrisAction(Identity id, Cost rushCost, ulong readyTime)
		: base("rd", id, typeof(RushDebrisAction).ToString())
	{
		this.rushCost = rushCost;
		this.readyTime = readyTime;
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["clear_rush_cost"] = rushCost.ToDict();
		dictionary["ready_time"] = readyTime;
		return dictionary;
	}

	public new static RushDebrisAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		Cost cost = Cost.FromDict((Dictionary<string, object>)data["clear_rush_cost"]);
		ulong num = TFUtils.LoadUlong(data, "ready_time");
		return new RushDebrisAction(id, cost, num);
	}

	public override void Apply(Game game, ulong utcNow)
	{
		Simulated simulated = game.simulation.FindSimulated(target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		ClearableDecorator entity = simulated.GetEntity<ClearableDecorator>();
		simulated.ClearPendingCommands();
		simulated.timebarMixinArgs.hasTimebar = false;
		entity.ClearCompleteTime = readyTime;
		simulated.LoadInitialState(EntityManager.DebrisActions["deleting"]);
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
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["debris"];
		Dictionary<string, object> dictionary = (Dictionary<string, object>)list.Find((object d) => ((string)((Dictionary<string, object>)d)["label"]).Equals(target.Describe()));
		dictionary["clear_complete_time"] = readyTime;
		base.Confirm(gameState);
	}

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
	}
}
