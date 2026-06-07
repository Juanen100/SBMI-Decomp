using System.Collections.Generic;

public class TreasureCooldownAction : PersistedTriggerableAction
{
	public const string TREASURE_TIME = "tt";

	private ulong nextTreasureTime;

	private string persistName;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public TreasureCooldownAction(ulong nextTime, string persistName)
		: base(null, null)
	{
	}

	public new static TreasureCooldownAction FromDict(Dictionary<string, object> data)
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
