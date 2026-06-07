using System.Collections.Generic;

public class RushCraftAction : PersistedSimulatedAction
{
	public const string RUSH_CRAFT = "rc";

	public const ulong INVALID_ULONG = ulong.MaxValue;

	private Cost rushCost;

	private ulong craftReadyTime;

	private Reward craftReward;

	private int slotId;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public RushCraftAction(Identity id, int slotId, Cost rushCost, ulong newReadyTime, Reward craftReward)
		: base(null, null, null)
	{
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public new static RushCraftAction FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	public override void Apply(Game game, ulong utcNow)
	{
	}

	public override void AddEnvelope(ulong time, string tag)
	{
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
	}

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}
}
