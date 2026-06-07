using System.Collections.Generic;

public class VendingAction : PersistedSimulatedAction
{
	public const string VENDING_ACTION = "va";

	private int slotId;

	private Reward reward;

	private Cost cost;

	private bool special;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public VendingAction(Identity id, int slotId, bool special, Reward reward, Cost cost)
		: base(null, null, null)
	{
	}

	public new static VendingAction FromDict(Dictionary<string, object> data)
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

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}
}
