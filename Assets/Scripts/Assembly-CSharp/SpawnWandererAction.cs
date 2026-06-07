using System.Collections.Generic;

public class SpawnWandererAction : PersistedTriggerableAction
{
	public const string SPAWN_WANDERER = "sw";

	public int did;

	public string id;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public SpawnWandererAction(int did, string id)
		: base(null, null)
	{
	}

	public new static SpawnWandererAction FromDict(Dictionary<string, object> data)
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
