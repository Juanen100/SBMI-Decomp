using System.Collections.Generic;

public class ChangeCostumeAction : PersistedSimulatedAction
{
	public const string CHANGE_COSTUME = "cca";

	public const string TRIGGERTYPE = "ChangeCostume";

	private int m_nCostumeDID;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public ChangeCostumeAction(Identity ID, int nCostumeDID)
		: base(null, null, null)
	{
	}

	public new static ChangeCostumeAction FromDict(Dictionary<string, object> pData)
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

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> pData)
	{
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> pData)
	{
		return null;
	}

	public override void Confirm(Dictionary<string, object> pGameState)
	{
	}
}
