using System.Collections.Generic;

public class SpawnResidentAction : PersistedTriggerableAction
{
	public const string SPAWN_RESDIENT = "sr";

	public int residentDID;

	public string residentID;

	public int buildingDID;

	public string buildingID;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public SpawnResidentAction(int residentDID, string residentID, int buildingDID, string buildingID)
		: base(null, null)
	{
	}

	public new static SpawnResidentAction FromDict(Dictionary<string, object> data)
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

	public virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return null;
	}
}
