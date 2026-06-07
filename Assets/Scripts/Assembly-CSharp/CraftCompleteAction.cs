using System.Collections.Generic;

public class CraftCompleteAction : PersistedSimulatedAction
{
	public const string CRAFT_FINISHED = "cf";

	private Reward reward;

	private int slotId;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public CraftCompleteAction(Identity id, int slotId, Reward reward)
		: base(null, null, null)
	{
	}

	public new static CraftCompleteAction FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	public override void Apply(Game game, ulong utcNow)
	{
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}
}
