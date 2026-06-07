using System.Collections.Generic;

public class AutoQuest
{
	public int m_nDID;

	public int m_nCharacterDID;

	public int m_nGoldReward;

	public int m_nXPReward;

	public string m_sName;

	public string m_sDescription;

	public Dictionary<int, int> m_pRecipes;

	public AutoQuest(int nDID, int nCharacterDID, Dictionary<int, int> pRecipeDIDs, int nGoldReward, int nXPReward, string sName, string sDescription)
	{
	}

	public override string ToString()
	{
		return null;
	}
}
