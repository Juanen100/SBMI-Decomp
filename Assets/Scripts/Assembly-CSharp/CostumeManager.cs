using System.Collections.Generic;

public class CostumeManager
{
	public class Costume
	{
		public int m_nDID { get; private set; }

		public int m_nUnitDID { get; private set; }

		public int m_nWishTableDID { get; private set; }

		public int m_nUnlockLevel { get; private set; }

		public int m_nUnlockAssetDid { get; private set; }

		public int m_nUnlockQuest1 { get; private set; }

		public int m_nUnlockQuest2 { get; private set; }

		public int m_nCriteriaCount { get; private set; }

		public string m_sName { get; private set; }

		public string m_sTexture { get; private set; }

		public string m_sMaterial { get; private set; }

		public string m_sPortrait { get; private set; }

		public string m_sSkeleton { get; private set; }

		public string m_sUnlockText { get; private set; }

		public string m_sUnlockQuest1Descript { get; private set; }

		public string m_sUnlockQuest2Descript { get; private set; }

		public bool m_bHiddenUntilUnlocked { get; private set; }

		public bool m_bLockedViaCSpanel { get; set; }

		public Costume(Dictionary<string, object> pData)
		{
			m_nDID = TFUtils.LoadInt(pData, "did");
			m_nUnitDID = TFUtils.LoadInt(pData, "unit_did");
			m_nWishTableDID = TFUtils.LoadInt(pData, "wish_table_did");
			m_nUnlockLevel = TFUtils.LoadInt(pData, "unlock_level");
			m_nUnlockAssetDid = TFUtils.LoadInt(pData, "unlock_asset_did");
			m_nUnlockQuest1 = TFUtils.LoadInt(pData, "unlock_quest1");
			m_nUnlockQuest2 = TFUtils.LoadInt(pData, "unlock_quest2");
			m_sName = TFUtils.LoadString(pData, "name");
			m_sTexture = TFUtils.LoadString(pData, "texture");
			m_sMaterial = TFUtils.LoadString(pData, "material");
			m_sPortrait = TFUtils.LoadString(pData, "portrait");
			m_sSkeleton = TFUtils.LoadString(pData, "skeleton");
			m_sUnlockText = TFUtils.LoadString(pData, "unlock_text");
			m_sUnlockQuest1Descript = TFUtils.LoadString(pData, "unlock_quest1_description");
			m_sUnlockQuest2Descript = TFUtils.LoadString(pData, "unlock_quest2_description");
			m_bHiddenUntilUnlocked = TFUtils.LoadBool(pData, "hidden_until_unlocked");
			m_nCriteriaCount = 0;
			if (m_nUnlockLevel > 0)
			{
				m_nCriteriaCount++;
			}
			if (m_nUnlockAssetDid > 0)
			{
				m_nCriteriaCount++;
			}
			if (m_nUnlockQuest1 > 0)
			{
				m_nCriteriaCount++;
			}
			if (m_nUnlockQuest2 > 0)
			{
				m_nCriteriaCount++;
			}
			m_bLockedViaCSpanel = false;
		}
	}

	private Dictionary<int, Costume> m_pCostumes;

	private Dictionary<int, List<int>> m_pUnitCostumeMap;

	private List<int> m_pUnlockedCostumes;

	public CostumeManager()
	{
		LoadFromSpreadsheet();
	}

	public Costume GetCostume(int nCostumeDID)
	{
		if (m_pCostumes.ContainsKey(nCostumeDID))
		{
			return m_pCostumes[nCostumeDID];
		}
		return null;
	}

	public List<Costume> GetCostumesForUnit(int nUnitDID, bool bIncludeLocked = true, bool bIncludeHiddenIfLocked = true)
	{
		List<Costume> list = new List<Costume>();
		if (!m_pUnitCostumeMap.ContainsKey(nUnitDID))
		{
			return list;
		}
		List<int> list2 = m_pUnitCostumeMap[nUnitDID];
		int count = list2.Count;
		Costume costume = null;
		bool flag = false;
		for (int i = 0; i < count; i++)
		{
			costume = GetCostume(list2[i]);
			if (!bIncludeLocked || !bIncludeHiddenIfLocked)
			{
				flag = IsCostumeUnlocked(costume.m_nDID);
				if ((!bIncludeLocked && !flag) || (!bIncludeHiddenIfLocked && !flag && costume.m_bHiddenUntilUnlocked))
				{
					continue;
				}
			}
			if ((bIncludeLocked || IsCostumeUnlocked(list2[i])) && costume != null)
			{
				list.Add(costume);
			}
		}
		return list;
	}

	public bool IsCostumeUnlocked(int nCostumeDID)
	{
		return m_pUnlockedCostumes.Contains(nCostumeDID);
	}

	public void UnlockCostume(int nCostumeDID)
	{
		if (!m_pUnlockedCostumes.Contains(nCostumeDID))
		{
			m_pUnlockedCostumes.Add(nCostumeDID);
		}
	}

	public void RemoveCostume(int nCostumeDID)
	{
		if (m_pUnlockedCostumes.Contains(nCostumeDID))
		{
			m_pUnlockedCostumes.Remove(nCostumeDID);
		}
	}

	public void LockCostumeInStore(int nCostumeDID)
	{
		if (m_pCostumes.ContainsKey(nCostumeDID))
		{
			m_pCostumes[nCostumeDID].m_bLockedViaCSpanel = true;
		}
	}

	public void UnLockCostumeInStore(int nCostumeDID)
	{
		if (m_pCostumes.ContainsKey(nCostumeDID))
		{
			m_pCostumes[nCostumeDID].m_bLockedViaCSpanel = false;
		}
	}

	public bool IsCostumeValidForUnit(int nUnitDID, int nCostumeDID)
	{
		if (m_pCostumes.ContainsKey(nCostumeDID))
		{
			Costume costume = m_pCostumes[nCostumeDID];
			if (costume.m_nUnitDID == nUnitDID)
			{
				return true;
			}
		}
		return false;
	}

	public void UnlockAllCostumes()
	{
		foreach (KeyValuePair<int, Costume> pCostume in m_pCostumes)
		{
			UnlockCostume(pCostume.Value.m_nDID);
		}
	}

	public void UnlockAllCostumesToGamestate(Dictionary<string, object> pGameState)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)pGameState["farm"];
		if (!dictionary.ContainsKey("costumes"))
		{
			dictionary["costumes"] = new List<object>();
		}
		List<object> list = (List<object>)dictionary["costumes"];
		foreach (KeyValuePair<int, Costume> pCostume in m_pCostumes)
		{
			if (!list.Contains(pCostume.Value.m_nDID))
			{
				list.Add(pCostume.Value.m_nDID);
			}
		}
	}

	private void LoadFromSpreadsheet()
	{
		m_pCostumes = new Dictionary<int, Costume>();
		m_pUnitCostumeMap = new Dictionary<int, List<int>>();
		m_pUnlockedCostumes = new List<int>();
		DatabaseManager instance = DatabaseManager.Instance;
		string sheetName = "Costumes";
		int sheetIndex = instance.GetSheetIndex(sheetName);
		if (sheetIndex < 0)
		{
			return;
		}
		int num = instance.GetNumRows(sheetName);
		if (num <= 0)
		{
			return;
		}
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(sheetName, rowName, "id").ToString());
			dictionary.Add("did", instance.GetIntCell(sheetIndex, rowIndex, "did"));
			dictionary.Add("unit_did", instance.GetIntCell(sheetIndex, rowIndex, "unit did"));
			dictionary.Add("wish_table_did", instance.GetIntCell(sheetIndex, rowIndex, "wishtable did"));
			dictionary.Add("unlock_level", instance.GetIntCell(sheetName, rowName, "unlock criteria level"));
			dictionary.Add("unlock_asset_did", instance.GetIntCell(sheetName, rowName, "unlock criteria asset did"));
			dictionary.Add("unlock_quest1", instance.GetIntCell(sheetName, rowName, "unlock criteria quest 1"));
			dictionary.Add("unlock_quest2", instance.GetIntCell(sheetName, rowName, "unlock criteria quest 2"));
			dictionary.Add("hidden_until_unlocked", instance.GetIntCell(sheetIndex, rowIndex, "hidden until unlocked") == 1);
			dictionary.Add("name", instance.GetStringCell(sheetName, rowName, "costume name"));
			dictionary.Add("skeleton", instance.GetStringCell(sheetName, rowName, "costume prefab"));
			dictionary.Add("texture", instance.GetStringCell(sheetName, rowName, "texture"));
			dictionary.Add("portrait", instance.GetStringCell(sheetName, rowName, "portait"));
			dictionary.Add("material", instance.GetStringCell(sheetName, rowName, "material"));
			dictionary.Add("unlock_text", instance.GetStringCell(sheetName, rowName, "unlock criteria text"));
			dictionary.Add("unlock_quest1_description", instance.GetStringCell(sheetName, rowName, "unlock criteria quest description 1"));
			dictionary.Add("unlock_quest2_description", instance.GetStringCell(sheetName, rowName, "unlock criteria quest description 2"));
			Costume costume = new Costume(dictionary);
			m_pCostumes.Add(costume.m_nDID, costume);
			if (m_pUnitCostumeMap.ContainsKey(costume.m_nUnitDID))
			{
				m_pUnitCostumeMap[costume.m_nUnitDID].Add(costume.m_nDID);
			}
			else
			{
				m_pUnitCostumeMap.Add(costume.m_nUnitDID, new List<int> { costume.m_nDID });
			}
			if (instance.GetIntCell(sheetIndex, rowIndex, "default costume") == 1)
			{
				m_pUnlockedCostumes.Add(costume.m_nDID);
			}
		}
	}
}
