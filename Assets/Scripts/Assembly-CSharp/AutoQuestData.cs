using System.Collections.Generic;

public class AutoQuestData
{
	public class DialogData
	{
		public string m_sIntroDialog { get; private set; }

		public string m_sOutroDialog { get; private set; }

		public DialogData(string sIntroDialog, string sOutroDialog)
		{
		}
	}

	public enum eDistributionType
	{
		eEqual = 0,
		eRandom = 1,
		eNumTypes = 2
	}

	private readonly int[] m_pCharacters;

	private readonly string[] m_pItemCategories;

	private readonly bool[] m_pPickOneCategories;

	private readonly Dictionary<int, DialogData> m_pDialogData;

	public int m_nDID { get; private set; }

	public int m_nMinItems { get; private set; }

	public int m_nMaxItems { get; private set; }

	public float m_fExpMultiplier { get; private set; }

	public float m_fGoldMultiplier { get; private set; }

	public string m_sName { get; private set; }

	public string m_sDescription { get; private set; }

	public eDistributionType m_eDistribution { get; private set; }

	public AutoQuestData(Dictionary<string, object> pData)
	{
	}

	public int[] GetCharacters()
	{
		return null;
	}

	public string[] GetItemCategories()
	{
		return null;
	}

	public bool[] GetPickOneCategories()
	{
		return null;
	}

	public DialogData GetDialogData(int nDID)
	{
		return null;
	}
}
