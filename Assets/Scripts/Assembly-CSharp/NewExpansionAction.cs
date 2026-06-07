using System.Collections.Generic;

public class NewExpansionAction : PersistedTriggerableAction
{
	public const string NEW_EXPANSION = "ne";

	public int did;

	public Cost cost;

	public List<TerrainSlotObject> debris;

	public List<TerrainSlotObject> landmarks;

	public TriggerableMixin Triggerable
	{
		get
		{
			return null;
		}
	}

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public NewExpansionAction(int id, Cost cost, List<TerrainSlotObject> debris, List<TerrainSlotObject> landmarks)
		: base(null, null)
	{
	}

	public new static NewExpansionAction FromDict(Dictionary<string, object> data)
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
