using System.Collections.Generic;

public class PickupDropAction : PersistedSimulatedAction
{
	public const string PICKUP_DROP = "pd";

	public const int INVALID_INT = -1;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public PickupDropAction(Identity id, Identity dropID)
		: base(null, null, null)
	{
	}

	public new static PickupDropAction FromDict(Dictionary<string, object> data)
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
