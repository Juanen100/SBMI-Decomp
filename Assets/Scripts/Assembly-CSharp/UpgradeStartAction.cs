using System.Collections.Generic;

public class UpgradeStartAction : PersistedSimulatedAction
{
	public const string UPGRADE_BUILDING = "ugb";

	public bool upgraded;

	public ulong upgradeCompleteTime;

	public int dId;

	public EntityType entityType;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public UpgradeStartAction(Identity id, int did, bool upgraded, ulong upgradeCompleteTime, EntityType entityType)
		: base(null, null, null)
	{
	}

	public UpgradeStartAction(Simulated simulated)
		: base(null, null, null)
	{
	}

	public new static UpgradeStartAction FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	private void Initialize(int did, bool upgraded, ulong upgradeCompleteTime, EntityType entityType)
	{
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
}
