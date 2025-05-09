using System;
using System.Collections.Generic;

public class DebrisStartAction : PersistedSimulatedAction
{
	public const string DEBRIS_START = "ds";

	private ulong completionTime;

	private Cost cost;

	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	public DebrisStartAction(Identity target, ulong completeTime, Cost cost)
		: base("ds", target, typeof(DebrisStartAction).ToString())
	{
		this.cost = cost;
		completionTime = completeTime;
	}

	public new static DebrisStartAction FromDict(Dictionary<string, object> data)
	{
		Identity identity = new Identity((string)data["target"]);
		ulong completeTime = TFUtils.LoadUlong(data, "completion_time");
		Cost cost = Cost.FromObject(data["cost"]);
		return new DebrisStartAction(identity, completeTime, cost);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["completion_time"] = completionTime;
		dictionary["cost"] = cost.ToDict();
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
		ClearableDecorator entity = simulated.GetEntity<ClearableDecorator>();
		simulated.ClearPendingCommands();
		game.resourceManager.Apply(cost, game);
		simulated.EnterInitialState(EntityManager.DebrisActions["clearing"], game.simulation);
		entity.ClearCompleteTime = completionTime;
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		try
		{
			List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["debris"];
			string targetString = target.Describe();
			Predicate<object> match = (object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString);
			Dictionary<string, object> dictionary = (Dictionary<string, object>)list.Find(match);
			if (dictionary == null)
			{
				string message = "DebrisStartAction.Confirm - No Debris Found: " + targetString;
				TFUtils.ErrorLog(message);
				throw new Exception(message);
			}
			dictionary["clear_complete_time"] = completionTime;
			ResourceManager.ApplyCostToGameState(cost, gameState);
			base.Confirm(gameState);
		}
		catch (Exception message2)
		{
			TFUtils.ErrorLog(message2);
		}
	}
}
