using System.Collections.Generic;

public class CollectMatchBonusAction : PersistedSimulatedAction
{
	public const string COLLECT_MATCH_BONUS = "cmb";

	public const string PICKUP_TRIGGERTYPE = "BonusPickup";

	public Reward reward;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public CollectMatchBonusAction(Identity id, Reward reward)
		: base(null, null, null)
	{
	}

	public new static CollectMatchBonusAction FromDict(Dictionary<string, object> data)
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
