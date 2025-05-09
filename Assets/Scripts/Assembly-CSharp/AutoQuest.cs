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
		m_nDID = nDID;
		m_nCharacterDID = nCharacterDID;
		m_pRecipes = pRecipeDIDs;
		m_nGoldReward = nGoldReward;
		m_nXPReward = nXPReward;
		m_sName = sName;
		m_sDescription = sDescription;
	}

	public override string ToString()
	{
		string text = "AutoQuest | did: " + m_nDID + " characterDID: " + m_nCharacterDID + " m_nRecipes: ";
		string text2;
		foreach (KeyValuePair<int, int> pRecipe in m_pRecipes)
		{
			text2 = text;
			text = text2 + "{ did: " + pRecipe.Key + ", count: " + pRecipe.Value + " },";
		}
		text2 = text;
		return text2 + " goldReward: " + m_nGoldReward + " xpReward: " + m_nXPReward + " name: " + m_sName + " description: " + m_sDescription;
	}
}
