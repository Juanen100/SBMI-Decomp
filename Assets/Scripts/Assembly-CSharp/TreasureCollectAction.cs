using System.Collections.Generic;

public class TreasureCollectAction : PersistedSimulatedAction
{
	public const string TREASURE_COLLECT = "tc";

	private Reward reward;

	private ulong? nextTreasureTime;

	private string persistName;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public TreasureCollectAction(Identity id, Reward reward, string persistName, ulong? timeToTreasure)
		: base(null, null, null)
	{
	}

	public new static TreasureCollectAction FromDict(Dictionary<string, object> data)
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
