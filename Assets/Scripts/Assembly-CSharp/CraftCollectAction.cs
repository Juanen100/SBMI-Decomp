using System.Collections.Generic;

public class CraftCollectAction : PersistedSimulatedAction
{
	public const string CRAFT_COLLECT = "cc";

	public const string PICKUP_TRIGGERTYPE = "CraftPickup";

	private Reward reward;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public CraftCollectAction(Identity id, Reward reward)
		: base(null, null, null)
	{
	}

	public new static CraftCollectAction FromDict(Dictionary<string, object> data)
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
