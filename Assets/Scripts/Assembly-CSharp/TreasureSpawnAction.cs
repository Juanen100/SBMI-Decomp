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
		: base(null, null, null)
	{
	}

	public TreasureSpawnAction(Simulated simulated, TreasureSpawner treasureTiming)
		: base(null, null, null)
	{
	}

	public new static TreasureSpawnAction FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public override void Apply(Game game, ulong utcNow)
	{
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
	}
}
