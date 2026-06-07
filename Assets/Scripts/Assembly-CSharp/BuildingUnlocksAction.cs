using System.Collections.Generic;

public class BuildingUnlocksAction : PersistedTriggerableAction
{
	public const string UNLOCK_BUILDING = "ub";

	public List<int> buildings;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public BuildingUnlocksAction(List<int> buildings)
		: base(null, null)
	{
	}

	public new static BuildingUnlocksAction FromDict(Dictionary<string, object> data)
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
