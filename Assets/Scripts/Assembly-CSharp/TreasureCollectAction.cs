#define ASSERTS_ON
using System;
using System.Collections.Generic;

public class TreasureCollectAction : PersistedSimulatedAction
{
	public const string TREASURE_COLLECT = "tc";

	private Reward reward;

	private ulong? nextTreasureTime;

	private string persistName;

	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	public TreasureCollectAction(Identity id, Reward reward, string persistName, ulong? timeToTreasure)
		: base("tc", id, typeof(TreasureSpawnAction).ToString())
	{
		this.reward = reward;
		this.persistName = persistName;
		nextTreasureTime = timeToTreasure;
	}

	public new static TreasureCollectAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		Reward reward = Reward.FromObject(data["reward"]);
		string text = "time_to_spawn";
		ulong? timeToTreasure = null;
		if (data.ContainsKey("persist_name"))
		{
			text = TFUtils.LoadString(data, "persist_name");
			timeToTreasure = TFUtils.TryLoadUlong(data, "next_treasure_time");
		}
		TreasureCollectAction treasureCollectAction = new TreasureCollectAction(id, reward, text, timeToTreasure);
		treasureCollectAction.DropTargetDataFromDict(data);
		return treasureCollectAction;
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["reward"] = reward.ToDict();
		dictionary["persist_name"] = persistName;
		dictionary["next_treasure_time"] = nextTreasureTime;
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
		TreasureSpawner treasureTiming = simulated.GetEntity<TreasureEntity>().TreasureTiming;
		treasureTiming.MarkCollected();
		simulated.SetFootprint(game.simulation, false);
		game.simulation.RemoveSimulated(simulated);
		game.entities.Destroy(target);
		game.ApplyReward(reward, GetTime());
		AddPickup(game.simulation);
		simulated.ClearPendingCommands();
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["farm"];
		if (dictionary.ContainsKey("treasure_state"))
		{
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)dictionary["treasure_state"];
			dictionary2[persistName] = nextTreasureTime;
		}
		List<object> orCreateList = TFUtils.GetOrCreateList<object>(dictionary, "treasure");
		string targetString = target.Describe();
		Predicate<object> match = (object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString);
		object obj = orCreateList.Find(match);
		TFUtils.Assert(obj != null, "Trying to Collect a missing treasure!");
		orCreateList.Remove(obj);
		RewardManager.ApplyToGameState(reward, GetTime(), gameState);
		AddPickupToGameState(gameState);
		base.Confirm(gameState);
	}

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
	}
}
