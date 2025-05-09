using System.Collections.Generic;

public class AutoQuestData
{
	public class DialogData
	{
		public string m_sIntroDialog { get; private set; }

		public string m_sOutroDialog { get; private set; }

		public DialogData(string sIntroDialog, string sOutroDialog)
		{
			m_sIntroDialog = sIntroDialog;
			m_sOutroDialog = sOutroDialog;
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
		m_nDID = TFUtils.LoadInt(pData, "did");
		m_nMinItems = TFUtils.LoadInt(pData, "min_items");
		m_nMaxItems = TFUtils.LoadInt(pData, "max_items");
		m_fExpMultiplier = TFUtils.LoadFloat(pData, "exp_multiplier");
		m_fGoldMultiplier = TFUtils.LoadFloat(pData, "gold_multiplier");
		m_sName = TFUtils.LoadString(pData, "name");
		m_sDescription = TFUtils.LoadString(pData, "description");
		m_pCharacters = TFUtils.LoadList<int>(pData, "characters").ToArray();
		m_pItemCategories = TFUtils.LoadList<string>(pData, "item_categories").ToArray();
		m_pPickOneCategories = TFUtils.LoadList<bool>(pData, "pick_one_categories").ToArray();
		m_eDistribution = ((!(TFUtils.LoadString(pData, "distribution") == "equal")) ? eDistributionType.eRandom : eDistributionType.eEqual);
		Dictionary<string, object> dictionary = TFUtils.TryLoadDict(pData, "intro_dialog_data");
		Dictionary<string, object> dictionary2 = TFUtils.TryLoadDict(pData, "outro_dialog_data");
		m_pDialogData = new Dictionary<int, DialogData>();
		if (dictionary == null || dictionary2 == null)
		{
			return;
		}
		foreach (KeyValuePair<string, object> item in dictionary)
		{
			if (dictionary2.ContainsKey(item.Key))
			{
				m_pDialogData.Add(int.Parse(item.Key), new DialogData((string)item.Value, (string)dictionary2[item.Key]));
			}
		}
	}

	public int[] GetCharacters()
	{
		return (int[])m_pCharacters.Clone();
	}

	public string[] GetItemCategories()
	{
		return (string[])m_pItemCategories.Clone();
	}

	public bool[] GetPickOneCategories()
	{
		return (bool[])m_pPickOneCategories.Clone();
	}

	public DialogData GetDialogData(int nDID)
	{
		if (m_pDialogData.ContainsKey(nDID))
		{
			return m_pDialogData[nDID];
		}
		return null;
	}
}
