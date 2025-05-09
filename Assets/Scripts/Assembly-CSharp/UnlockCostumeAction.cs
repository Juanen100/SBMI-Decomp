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
		: base("uc", Identity.Null())
	{
		m_nCostumeDID = nCostumeDID;
	}

	public new static UnlockCostumeAction FromDict(Dictionary<string, object> pData)
	{
		int nCostumeDID = TFUtils.LoadInt(pData, "costume_did");
		return new UnlockCostumeAction(nCostumeDID);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["costume_did"] = m_nCostumeDID;
		return dictionary;
	}

	public override void Apply(Game pGame, ulong ulUtcNow)
	{
		base.Apply(pGame, ulUtcNow);
		pGame.costumeManager.UnlockCostume(m_nCostumeDID);
	}

	public override void Confirm(Dictionary<string, object> pGameState)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)pGameState["farm"];
		if (!dictionary.ContainsKey("costumes"))
		{
			dictionary["costumes"] = new List<object>();
		}
		List<object> list = (List<object>)dictionary["costumes"];
		if (!list.Contains(m_nCostumeDID))
		{
			list.Add(m_nCostumeDID);
		}
		base.Confirm(pGameState);
	}
}
