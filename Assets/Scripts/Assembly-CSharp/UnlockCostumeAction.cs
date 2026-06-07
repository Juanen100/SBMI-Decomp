using System.Collections.Generic;

public class UnlockCostumeAction : PersistedTriggerableAction
{
	public const string UNLOCK_COSTUME = "uc";

	private int m_nCostumeDID;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public UnlockCostumeAction(int nCostumeDID)
		: base(null, null)
	{
	}

	public new static UnlockCostumeAction FromDict(Dictionary<string, object> pData)
	{
		return null;
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}

	public override void Apply(Game pGame, ulong ulUtcNow)
	{
	}

	public override void Confirm(Dictionary<string, object> pGameState)
	{
	}
}
