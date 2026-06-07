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
		}
	}

	private Dictionary<int, Costume> m_pCostumes;

	private Dictionary<int, List<int>> m_pUnitCostumeMap;

	private List<int> m_pUnlockedCostumes;

	public Costume GetCostume(int nCostumeDID)
	{
		return null;
	}

	public List<Costume> GetCostumesForUnit(int nUnitDID, bool bIncludeLocked = true, bool bIncludeHiddenIfLocked = true)
	{
		return null;
	}

	public bool IsCostumeUnlocked(int nCostumeDID)
	{
		return false;
	}

	public void UnlockCostume(int nCostumeDID)
	{
	}

	public void RemoveCostume(int nCostumeDID)
	{
	}

	public void LockCostumeInStore(int nCostumeDID)
	{
	}

	public void UnLockCostumeInStore(int nCostumeDID)
	{
	}

	public bool IsCostumeValidForUnit(int nUnitDID, int nCostumeDID)
	{
		return false;
	}

	public void UnlockAllCostumes()
	{
	}

	public void UnlockAllCostumesToGamestate(Dictionary<string, object> pGameState)
	{
	}

	private void LoadFromSpreadsheet()
	{
	}
}
