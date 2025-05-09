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
		: base("cca", ID, "ChangeCostume")
	{
		m_nCostumeDID = nCostumeDID;
	}

	public new static ChangeCostumeAction FromDict(Dictionary<string, object> pData)
	{
		Identity iD = new Identity((string)pData["target"]);
		int nCostumeDID = TFUtils.LoadInt(pData, "costume_did");
		ChangeCostumeAction changeCostumeAction = new ChangeCostumeAction(iD, nCostumeDID);
		changeCostumeAction.DropTargetDataFromDict(pData);
		return changeCostumeAction;
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		DropTargetDataToDict(dictionary);
		dictionary.Add("costume_did", m_nCostumeDID);
		return dictionary;
	}

	public override void Apply(Game pGame, ulong ulUtcNow)
	{
		base.Apply(pGame, ulUtcNow);
		Simulated simulated = pGame.simulation.FindSimulated(target);
		if (simulated != null)
		{
			CostumeManager.Costume costume = pGame.costumeManager.GetCostume(m_nCostumeDID);
			if (costume != null)
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				entity.CostumeDID = m_nCostumeDID;
				simulated.SetCostume(costume);
			}
		}
	}

	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> pData)
	{
		base.AddMoreDataToTrigger(ref pData);
		pData.Add("costume_id", m_nCostumeDID);
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> pData)
	{
		return triggerable.BuildTrigger(GetType().ToString(), AddMoreDataToTrigger);
	}

	public override void Confirm(Dictionary<string, object> pGameState)
	{
		Dictionary<string, object> unitGameState = ResidentEntity.GetUnitGameState(pGameState, target);
		unitGameState["costume_did"] = m_nCostumeDID;
		base.Confirm(pGameState);
	}
}
