using System.Collections.Generic;

public class HideWandererAction : PersistedSimulatedAction
{
	public const string HIDE_WANDERER = "hw";

	public ulong hideExpiresAt;

	public int dId;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public HideWandererAction(Identity id, int did, ulong hideExpireAt)
		: base(null, null, null)
	{
	}

	public HideWandererAction(Simulated simulated, ulong hideExpireAt)
		: base(null, null, null)
	{
	}

	public new static HideWandererAction FromDict(Dictionary<string, object> data)
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
