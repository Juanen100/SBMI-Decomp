#define ASSERTS_ON
using System.Collections.Generic;

public class PaytableManager
{
	private const string BONUS_PAYTABLES = "BonusPaytables";

	private const uint DEFAULT_PAYTABLE = 1u;

	private Dictionary<uint, Paytable> paytableDefinitions;

	public List<int> paytableTaskCheck = new List<int>();

	public PaytableManager()
	{
		LoadBonusPaytables();
	}

	private string[] GetFilesToLoad()
	{
		return Config.BONUS_PAYTABLES;
	}

	private string GetFilePathFromString(string filePath)
	{
		return filePath;
	}

	public void LoadBonusPaytables()
	{
		TFUtils.Assert(paytableDefinitions == null, "Bonus Paytable Definitions have already been loaded!");
		paytableDefinitions = new Dictionary<uint, Paytable>();
		LoadFromSpreadsheet("BonusPaytables");
	}

	private void LoadFromSpreadsheet(string pSheetName)
	{
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(pSheetName))
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(pSheetName);
		if (sheetIndex < 0)
		{
			return;
		}
		int num = instance.GetNumRows(pSheetName);
		if (num < 0)
		{
			return;
		}
		int intCell = instance.GetIntCell(sheetIndex, 0, "number of reward sets");
		string stringCell = instance.GetStringCell(sheetIndex, 0, "number of rewards per set");
		string[] array = stringCell.Split('|');
		int num2 = array.Length;
		int[] array2 = new int[num2];
		int result = -1;
		for (int i = 0; i < num2; i++)
		{
			if (int.TryParse(array[i], out result))
			{
				array2[i] = result;
			}
			else
			{
				array2[i] = 0;
			}
		}
		int num3 = -1;
		int num4 = -1;
		int num5 = -1;
		int num6 = -1;
		int num7 = -1;
		int num8 = -1;
		int num9 = -1;
		string text = null;
		string text2 = null;
		int num10 = -1;
		float num11 = 0f;
		float num12 = 0f;
		Dictionary<string, object> dictionary = null;
		Dictionary<string, object> dictionary2 = null;
		Dictionary<string, object> dictionary3 = null;
		Dictionary<string, object> dictionary4 = null;
		Dictionary<string, object> dictionary5 = null;
		Dictionary<string, object> dictionary6 = null;
		Dictionary<string, object> dictionary7 = null;
		List<object> list = null;
		for (int j = 0; j < num; j++)
		{
			stringCell = j.ToString();
			if (!instance.HasRow(sheetIndex, stringCell))
			{
				num++;
				continue;
			}
			num5 = instance.GetRowIndex(sheetIndex, instance.GetIntCell(pSheetName, stringCell, "id").ToString());
			num4 = instance.GetIntCell(sheetIndex, num5, "paytable did");
			num6 = instance.GetIntCell(pSheetName, stringCell, "paytable trigger did");
			string key = num6.ToString();
			text2 = instance.GetStringCell(pSheetName, stringCell, "paytable trigger type");
			if (text2 == "task")
			{
				paytableTaskCheck.Add(num6);
			}
			num10 = instance.GetIntCell(pSheetName, stringCell, "one time reward");
			if (num10 == 1)
			{
			}
			if (num4 != num3)
			{
				num3 = num4;
				if (dictionary != null)
				{
					dictionary.Add("wagers", dictionary3);
					Paytable paytable = Paytable.FromDict(dictionary);
					TFUtils.Assert(!paytableDefinitions.ContainsKey(paytable.Did), "Loading duplicate paytable definition! Did=" + paytable.Did);
					paytableDefinitions[paytable.Did] = paytable;
				}
				dictionary = new Dictionary<string, object>();
				dictionary.Add("type", "bonus_paytable");
				dictionary.Add("did", num3);
				dictionary3 = new Dictionary<string, object>();
			}
			if (dictionary3.ContainsKey(key))
			{
				dictionary2 = (Dictionary<string, object>)dictionary3[key];
				list = (List<object>)dictionary2["cdf"];
			}
			else
			{
				dictionary2 = new Dictionary<string, object>();
				list = new List<object>();
			}
			for (int k = 0; k < intCell; k++)
			{
				num11 = instance.GetFloatCell(pSheetName, stringCell, "set odds " + (k + 1));
				string stringCell2 = instance.GetStringCell(pSheetName, stringCell, "set type " + (k + 1));
				if (stringCell2 == "none")
				{
					continue;
				}
				dictionary4 = new Dictionary<string, object>();
				dictionary5 = new Dictionary<string, object>();
				dictionary6 = new Dictionary<string, object>();
				dictionary4.Add("p", num11);
				num7 = array2[k];
				for (int l = 0; l < num7; l++)
				{
					num8 = instance.GetIntCell(pSheetName, stringCell, "set " + (k + 1) + " reward did " + (l + 1));
					if (num8 != -1)
					{
						num9 = instance.GetIntCell(pSheetName, stringCell, "set " + (k + 1) + " reward amount " + (l + 1));
						num12 = instance.GetFloatCell(pSheetName, stringCell, "set " + (k + 1) + " reward odds " + (l + 1));
						string key2 = num8.ToString();
						bool flag = dictionary6.ContainsKey(key2);
						dictionary7 = ((!flag) ? new Dictionary<string, object>() : ((Dictionary<string, object>)dictionary6[key2]));
						dictionary7.Add(num9.ToString(), num12);
						if (flag)
						{
							dictionary6[key2] = dictionary7;
						}
						else
						{
							dictionary6.Add(key2, dictionary7);
						}
					}
				}
				dictionary5.Add(stringCell2, dictionary6);
				dictionary4.Add("value", dictionary5);
				list.Add(dictionary4);
			}
			if (dictionary3.ContainsKey(key))
			{
				dictionary2["cdf"] = list;
				dictionary3[key] = dictionary2;
			}
			else
			{
				dictionary2.Add("cdf", list);
				dictionary3.Add(key, dictionary2);
			}
		}
		if (dictionary != null && num3 >= 0 && !paytableDefinitions.ContainsKey((uint)num3))
		{
			dictionary.Add("wagers", dictionary3);
			Paytable paytable = Paytable.FromDict(dictionary);
			TFUtils.Assert(!paytableDefinitions.ContainsKey(paytable.Did), "Loading duplicate paytable definition! Did=" + paytable.Did);
			paytableDefinitions[paytable.Did] = paytable;
		}
	}

	public Paytable Get(uint did)
	{
		if (!paytableDefinitions.ContainsKey(did))
		{
			TFUtils.ErrorLog("Did not find a paytable with the definition ID=" + did + ". Returning default(" + 1u + ") instead.");
			return paytableDefinitions[1u];
		}
		return paytableDefinitions[did];
	}
}
