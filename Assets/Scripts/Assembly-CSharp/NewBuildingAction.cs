using System.Collections.Generic;
using UnityEngine;

public class NewBuildingAction : PersistedSimulatedAction
{
	public const string NEW_BUILDING = "nb";

	public Vector2 position;

	public bool flip;

	public string blueprint;

	public bool built;

	public ulong buildCompleteTime;

	public int dId;

	public EntityType extensions;

	public Cost cost;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public NewBuildingAction(Identity id, string blueprint, int did, EntityType types, bool built, ulong buildCompleteTime, Vector2 position, bool flip, Cost cost)
		: base(null, null, null)
	{
	}

	public NewBuildingAction(Simulated simulated, Cost cost)
		: base(null, null, null)
	{
	}

	public new static NewBuildingAction FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	private void Initialize(string blueprint, int did, EntityType types, bool built, ulong buildCompleteTime, Vector2 position, bool flip, Cost cost)
	{
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

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}
}
