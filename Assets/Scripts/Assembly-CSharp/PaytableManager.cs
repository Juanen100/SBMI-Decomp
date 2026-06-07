using System.Collections.Generic;

public class PaytableManager
{
	private const string BONUS_PAYTABLES = "BonusPaytables";

	private const uint DEFAULT_PAYTABLE = 1u;

	private Dictionary<uint, Paytable> paytableDefinitions;

	public List<int> paytableTaskCheck;

	private string[] GetFilesToLoad()
	{
		return null;
	}

	private string GetFilePathFromString(string filePath)
	{
		return null;
	}

	public void LoadBonusPaytables()
	{
	}

	private void LoadFromSpreadsheet(string pSheetName)
	{
	}

	public Paytable Get(uint did)
	{
		return null;
	}
}
