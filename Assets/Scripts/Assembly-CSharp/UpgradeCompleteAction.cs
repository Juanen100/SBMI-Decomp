using System.Collections.Generic;

public class UpgradeCompleteAction : PersistedSimulatedAction
{
	public const string COMPLETE_UPGRADING = "cu";

	public ulong completeTime;

	public Reward reward;

	public ulong productReady;

	public int upgradeLevel;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	private UpgradeCompleteAction(Identity target, ulong completeTime, string triggerType)
		: base(null, null, null)
	{
	}

	private UpgradeCompleteAction(Identity target, ulong upgradeCompleteTime, int upgradeLevel, Reward reward)
		: base(null, null, null)
	{
	}

	public UpgradeCompleteAction(Simulated simulated, Reward reward)
		: base(null, null, null)
	{
	}

	public new static UpgradeCompleteAction FromDict(Dictionary<string, object> data)
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
}
