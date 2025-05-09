using System;
using System.Collections.Generic;

public class WishTableManager
{
	private Dictionary<int, CdfDictionary<int>> m_pWishTables;

	public WishTableManager()
	{
		LoadFromSpreadsheet();
	}

	public CdfDictionary<int> GetWishTable(int nDID)
	{
		if (m_pWishTables.ContainsKey(nDID))
		{
			return m_pWishTables[nDID];
		}
		return null;
	}

	private void LoadFromSpreadsheet()
	{
		m_pWishTables = new Dictionary<int, CdfDictionary<int>>();
		DatabaseManager instance = DatabaseManager.Instance;
		string sheetName = "WishTables";
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
		CdfDictionary<int>.ParseT parser = (object val) => Convert.ToInt32(val);
		int num2 = -1;
		for (int num3 = 0; num3 < num; num3++)
		{
			string rowName = num3.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(sheetName, rowName, "id").ToString());
			if (num2 < 0)
			{
				num2 = instance.GetIntCell(sheetIndex, rowIndex, "max wishes");
			}
			int intCell = instance.GetIntCell(sheetIndex, rowIndex, "did");
			List<object> list = new List<object>();
			for (int num4 = 0; num4 < num2; num4++)
			{
				int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "wish did " + (num4 + 1));
				if (intCell2 >= 0)
				{
					list.Add(new Dictionary<string, object>
					{
						{
							"p",
							instance.GetFloatCell(sheetIndex, rowIndex, "wish odds " + (num4 + 1))
						},
						{ "value", intCell2 }
					});
				}
			}
			m_pWishTables.Add(intCell, CdfDictionary<int>.FromList(list, parser));
		}
	}
}
