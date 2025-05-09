using System.Collections.Generic;
using UnityEngine;

public class TreasureSpawnAction : PersistedSimulatedAction
{
	public const string TREASURE_SPAWN = "ts";

	private Vector2 position;

	private EntityType extensions;

	private int did;

	private string persistName;

	private ulong? nextTreasureTime;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public TreasureSpawnAction(Identity id, int did, EntityType extensions, Vector2 position, string persistName, ulong? timeToTreasure)
		: base("ts", id, typeof(TreasureSpawnAction).ToString())
	{
		this.extensions = extensions;
		this.did = did;
		this.position = position;
		this.persistName = persistName;
		nextTreasureTime = timeToTreasure;
	}

	public TreasureSpawnAction(Simulated simulated, TreasureSpawner treasureTiming)
		: this(simulated.Id, simulated.entity.DefinitionId, simulated.entity.AllTypes, simulated.Position, treasureTiming.PersistName, treasureTiming.TimeToTreasure)
	{
	}

	public new static TreasureSpawnAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		EntityType entityType = (EntityType)TFUtils.LoadUint(data, "extensions");
		int num = TFUtils.LoadInt(data, "did");
		Vector2 vector = new Vector2(TFUtils.LoadFloat(data, "x"), TFUtils.LoadFloat(data, "y"));
		string text = "time_to_spawn";
		ulong? timeToTreasure = null;
		if (data.ContainsKey("persist_name"))
		{
			text = TFUtils.LoadString(data, "persist_name");
			timeToTreasure = TFUtils.TryLoadUlong(data, "next_treasure_time");
		}
		return new TreasureSpawnAction(id, num, entityType, vector, text, timeToTreasure);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["extensions"] = (uint)extensions;
		dictionary["did"] = did;
		dictionary["x"] = position.x;
		dictionary["y"] = position.y;
		dictionary["persist_name"] = persistName;
		dictionary["next_treasure_time"] = nextTreasureTime;
		return dictionary;
	}

	public override void Apply(Game game, ulong utcNow)
	{
		Entity entity = game.entities.Create(extensions, did, target, true);
		TreasureEntity decorator = entity.GetDecorator<TreasureEntity>();
		if (decorator.TreasureTiming == null)
		{
			decorator.TreasureTiming = game.treasureManager.FindTreasureSpawner(persistName);
		}
		TreasureSpawner treasureTiming = decorator.TreasureTiming;
		treasureTiming.MarkComplete();
		Simulated simulated = Simulated.Treasure.Load(decorator, game.simulation, position, utcNow);
		simulated.EnterInitialState(EntityManager.TreasureActions["buried"], game.simulation);
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
		Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
		dictionary3["did"] = did;
		dictionary3["extensions"] = (uint)extensions;
		dictionary3["label"] = target.Describe();
		dictionary3["x"] = position.x;
		dictionary3["y"] = position.y;
		dictionary3["treasure_spawner_name"] = persistName;
		orCreateList.Add(dictionary3);
		base.Confirm(gameState);
	}
}
