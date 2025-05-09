using System.Collections.Generic;

public class RushBuildAction : PersistedSimulatedAction
{
	public const string RUSH_BUILD = "rb";

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

	public RushBuildAction(Identity id, Cost rushCost, ulong nextReadyTime)
		: base("rb", id, typeof(RushBuildAction).ToString())
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

	public new static RushBuildAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		Cost cost = Cost.FromDict((Dictionary<string, object>)data["rush_cost"]);
		ulong nextReadyTime = TFUtils.LoadUlong(data, "ready_time");
		return new RushBuildAction(id, cost, nextReadyTime);
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
		simulated.RemoveScaffolding(game.simulation);
		simulated.RemoveFence(game.simulation);
		ErectableDecorator entity = simulated.GetEntity<ErectableDecorator>();
		entity.ErectionCompleteTime = readyTime;
		Identity identity = ((!simulated.Variable.ContainsKey("employee")) ? null : (simulated.Variable["employee"] as Identity));
		if (identity != null)
		{
			Simulated simulated2 = game.simulation.FindSimulated(identity);
			if (simulated2 != null)
			{
				simulated2.ClearPendingCommands();
			}
			game.simulation.Router.Send(ReturnCommand.Create(simulated.Id, identity));
		}
		simulated.EnterInitialState(EntityManager.BuildingActions["inactive"], game.simulation);
		simulated.FirstAnimate(game.simulation);
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
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["buildings"];
		string targetString = target.Describe();
		Dictionary<string, object> dictionary = (Dictionary<string, object>)list.Find((object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString));
		dictionary["build_finish_time"] = readyTime;
		base.Confirm(gameState);
	}

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
	}
}
