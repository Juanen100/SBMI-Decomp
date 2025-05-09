using System;
using System.Collections.Generic;

public class TreasureUncoverAction : PersistedSimulatedAction
{
	public const string TREASURE_UNCOVER = "tu";

	private ulong completionTime;

	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	public TreasureUncoverAction(Identity id, ulong completionTime)
		: base("tu", id, typeof(TreasureUncoverAction).ToString())
	{
		this.completionTime = completionTime;
	}

	public new static TreasureUncoverAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		ulong num = TFUtils.LoadUlong(data, "completion_time");
		return new TreasureUncoverAction(id, num);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["completion_time"] = completionTime;
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
		TreasureEntity entity = simulated.GetEntity<TreasureEntity>();
		simulated.ClearPendingCommands();
		entity.ClearCompleteTime = completionTime;
		simulated.EnterInitialState(EntityManager.TreasureActions["uncovering"], game.simulation);
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		List<object> orCreateList = TFUtils.GetOrCreateList<object>((Dictionary<string, object>)gameState["farm"], "treasure");
		string targetString = target.Describe();
		Predicate<object> match = (object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString);
		Dictionary<string, object> dictionary = (Dictionary<string, object>)orCreateList.Find(match);
		dictionary["clear_complete_time"] = completionTime;
		base.Confirm(gameState);
	}
}
