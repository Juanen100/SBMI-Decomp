#define ASSERTS_ON
using System.Collections.Generic;

public class Catalog
{
	private Dictionary<string, object> catalogDict;

	private Dictionary<int, Cost> costs;

	private Dictionary<int, Cost> sellCosts;

	private Dictionary<int, string> descriptions;

	private Dictionary<string, Dictionary<string, object>> offersByCode;

	private List<string> premiumCodes;

	private Dictionary<int, bool> canSell;

	private Dictionary<int, string> sellErrors;

	public Dictionary<string, object> CatalogDict
	{
		get
		{
			return catalogDict;
		}
	}

	public List<string> PremiumCodes
	{
		get
		{
			return premiumCodes;
		}
	}

	public Catalog()
	{
		costs = new Dictionary<int, Cost>();
		sellCosts = new Dictionary<int, Cost>();
		descriptions = new Dictionary<int, string>();
		offersByCode = new Dictionary<string, Dictionary<string, object>>();
		premiumCodes = new List<string>();
		canSell = new Dictionary<int, bool>();
		sellErrors = new Dictionary<int, string>();
		LoadCatalog();
	}

	private void LoadCatalog()
	{
		catalogDict = LoadCatalogFromSpread();
		TFUtils.Assert(catalogDict != null, "Catalog Spread failed to read in.");
		List<object> list = TFUtils.LoadList<object>(catalogDict, "offers");
		foreach (Dictionary<string, object> item in list)
		{
			LoadCostsHelper(costs, item, "cost");
			LoadCostsHelper(sellCosts, item, "sell_cost");
			if (item.ContainsKey("can_sell"))
			{
				int key = TFUtils.LoadInt(item, "identity");
				canSell.Add(key, (bool)item["can_sell"]);
				if (item.ContainsKey("sell_error"))
				{
					sellErrors.Add(key, (string)item["sell_error"]);
				}
			}
			if (item.ContainsKey("description"))
			{
				int key2 = TFUtils.LoadInt(item, "identity");
				descriptions.Add(key2, (string)item["description"]);
			}
			if (item.ContainsKey("code"))
			{
				string text = TFUtils.LoadString(item, "code");
				offersByCode.Add(text, item);
				premiumCodes.Add(text);
			}
		}
	}

	private Dictionary<string, object> LoadCatalogFromSpread()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("type", "catalog");
		if (!LoadItemCategoriesFromSpread(dictionary))
		{
			return null;
		}
		if (!LoadItemIdentitiesFromSpread(dictionary))
		{
			return null;
		}
		LoadIAPIdentitiesFromSpread(dictionary);
		return dictionary;
	}

	private bool LoadItemCategoriesFromSpread(Dictionary<string, object> pData)
	{
		string text = "ItemCategories";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return false;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return false;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return false;
		}
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, "id").ToString());
			string stringCell = instance.GetStringCell(text, rowName, "category type");
			List<object> list;
			if (!pData.ContainsKey(stringCell))
			{
				list = new List<object>();
				pData.Add(stringCell, list);
			}
			else
			{
				list = (List<object>)pData[stringCell];
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("micro_event_did", instance.GetIntCell(sheetIndex, rowIndex, "micro event did"));
			dictionary.Add("event_only", instance.GetIntCell(sheetIndex, rowIndex, "event only") == 1);
			dictionary.Add("name", instance.GetStringCell(text, rowName, "name"));
			dictionary.Add("type", instance.GetStringCell(text, rowName, "item type"));
			dictionary.Add("label", instance.GetStringCell(text, rowName, "label"));
			dictionary.Add("display.material", instance.GetStringCell(text, rowName, "display material"));
			list.Add(dictionary);
		}
		return true;
	}

	private bool LoadItemIdentitiesFromSpread(Dictionary<string, object> pData)
	{
		string text = "ItemIdentities";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return false;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return false;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return false;
		}
		List<object> list;
		if (pData.ContainsKey("offers"))
		{
			list = (List<object>)pData["offers"];
		}
		else
		{
			list = new List<object>();
			pData.Add("offers", list);
		}
		if (!pData.ContainsKey("market"))
		{
			TFUtils.Assert(false, "Could not find category group market ");
			return false;
		}
		List<object> list2 = (List<object>)pData["market"];
		int count = list2.Count;
		string text2 = "n/a";
		bool flag = false;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, "id").ToString());
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			int intCell = instance.GetIntCell(sheetIndex, rowIndex, "item did");
			dictionary.Add("identity", intCell);
			dictionary.Add("micro_event_did", instance.GetIntCell(sheetIndex, rowIndex, "micro event did"));
			flag = instance.GetIntCell(sheetIndex, rowIndex, "show in store") == 1;
			dictionary.Add("show_in_store", flag);
			dictionary.Add("event_only", instance.GetIntCell(sheetIndex, rowIndex, "event only") == 1);
			dictionary.Add("sale_banner", instance.GetIntCell(sheetIndex, rowIndex, "sale banner") == 1);
			dictionary.Add("new_banner", instance.GetIntCell(sheetIndex, rowIndex, "new banner") == 1);
			dictionary.Add("sale_percent", instance.GetFloatCell(sheetIndex, rowIndex, "sale percent"));
			dictionary.Add("limited_banner", instance.GetIntCell(sheetIndex, rowIndex, "limitedbanner") == 1);
			string stringCell;
			if (flag)
			{
				stringCell = instance.GetStringCell(text, rowName, "category name");
				for (int j = 0; j < count; j++)
				{
					Dictionary<string, object> dictionary2 = (Dictionary<string, object>)list2[j];
					if ((string)dictionary2["name"] == stringCell)
					{
						if (!dictionary2.ContainsKey("dids"))
						{
							dictionary2.Add("dids", new List<object>());
						}
						((List<object>)dictionary2["dids"]).Add(intCell);
						break;
					}
					if (j == count - 1)
					{
						TFUtils.Assert(false, "Could not find category " + stringCell);
						return false;
					}
				}
			}
			dictionary.Add("cost", new Dictionary<string, object>());
			intCell = instance.GetIntCell(sheetIndex, rowIndex, "cost gold");
			if (intCell >= 0)
			{
				((Dictionary<string, object>)dictionary["cost"]).Add("3", intCell);
			}
			intCell = instance.GetIntCell(sheetIndex, rowIndex, "cost jelly");
			if (intCell >= 0)
			{
				((Dictionary<string, object>)dictionary["cost"]).Add("2", intCell);
			}
			intCell = instance.GetIntCell(sheetIndex, rowIndex, "cost special did");
			int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "cost special amount");
			if (intCell >= 2 && intCell2 > 0)
			{
				((Dictionary<string, object>)dictionary["cost"]).Add(intCell.ToString(), intCell2);
			}
			intCell = instance.GetIntCell(sheetIndex, rowIndex, "sell cost gold");
			if (intCell >= 0)
			{
				dictionary.Add("sell_cost", new Dictionary<string, object> { { "3", intCell } });
			}
			intCell = instance.GetIntCell(sheetIndex, rowIndex, "sell cost jelly");
			if (intCell >= 0)
			{
				if (!dictionary.ContainsKey("sell_cost"))
				{
					dictionary.Add("sell_cost", new Dictionary<string, object>());
				}
				((Dictionary<string, object>)dictionary["sell_cost"]).Add("2", intCell);
			}
			intCell = instance.GetIntCell(sheetIndex, rowIndex, "can sell");
			if (intCell > 0)
			{
				dictionary.Add("can_sell", true);
			}
			else
			{
				dictionary.Add("can_sell", false);
			}
			stringCell = instance.GetStringCell(text, rowName, "can't sell text");
			if (stringCell != text2)
			{
				dictionary.Add("sell_error", stringCell);
			}
			stringCell = instance.GetStringCell(text, rowName, "description");
			if (stringCell != text2)
			{
				dictionary.Add("description", stringCell);
			}
			stringCell = instance.GetStringCell(text, rowName, "type");
			if (stringCell != text2)
			{
				dictionary.Add("type", stringCell);
			}
			stringCell = instance.GetStringCell(text, rowName, "name");
			if (stringCell != text2)
			{
				dictionary.Add("name", stringCell);
			}
			stringCell = instance.GetStringCell(text, rowName, "display model type");
			if (stringCell != text2)
			{
				dictionary.Add("display", new Dictionary<string, object>
				{
					{ "model_type", stringCell },
					{
						"width",
						instance.GetIntCell(sheetIndex, rowIndex, "display width")
					},
					{
						"height",
						instance.GetIntCell(sheetIndex, rowIndex, "display height")
					}
				});
			}
			stringCell = instance.GetStringCell(text, rowName, "display default texture");
			if (stringCell != text2)
			{
				dictionary.Add("display.default", new Dictionary<string, object>
				{
					{ "texture", stringCell },
					{ "name", "default" }
				});
			}
			list.Add(dictionary);
		}
		return true;
	}

	private bool LoadIAPIdentitiesFromSpread(Dictionary<string, object> pData)
	{
		string text = "IAPIdentities";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return false;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return false;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return false;
		}
		List<object> list;
		if (pData.ContainsKey("offers"))
		{
			list = (List<object>)pData["offers"];
		}
		else
		{
			list = new List<object>();
			pData.Add("offers", list);
		}
		if (!pData.ContainsKey("market"))
		{
			TFUtils.Assert(false, "Could not find category group market ");
			return false;
		}
		List<object> list2 = (List<object>)pData["market"];
		int count = list2.Count;
		string text2 = "n/a";
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, "id").ToString());
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			int intCell = instance.GetIntCell(sheetIndex, rowIndex, "iap did");
			dictionary.Add("identity", intCell);
			string stringCell = instance.GetStringCell(text, rowName, "category name");
			for (int j = 0; j < count; j++)
			{
				Dictionary<string, object> dictionary2 = (Dictionary<string, object>)list2[j];
				if ((string)dictionary2["name"] == stringCell)
				{
					if (!dictionary2.ContainsKey("dids"))
					{
						dictionary2.Add("dids", new List<object>());
					}
					((List<object>)dictionary2["dids"]).Add(intCell);
					break;
				}
				if (j == count - 1)
				{
					TFUtils.Assert(false, "Could not find category " + stringCell);
					return false;
				}
			}
			dictionary.Add("cost", new Dictionary<string, object>());
			dictionary.Add("data", new Dictionary<string, object>());
			intCell = instance.GetIntCell(sheetIndex, rowIndex, "gold amount");
			if (intCell > 0)
			{
				((Dictionary<string, object>)dictionary["data"]).Add("3", intCell);
			}
			intCell = instance.GetIntCell(sheetIndex, rowIndex, "jelly amount");
			if (intCell > 0)
			{
				((Dictionary<string, object>)dictionary["data"]).Add("2", intCell);
			}
			stringCell = instance.GetStringCell(text, rowName, "description");
			if (stringCell != text2)
			{
				dictionary.Add("description", stringCell);
			}
			stringCell = instance.GetStringCell(text, rowName, "type");
			if (stringCell != text2)
			{
				dictionary.Add("type", stringCell);
			}
			stringCell = instance.GetStringCell(text, rowName, "name");
			if (stringCell != text2)
			{
				dictionary.Add("name", stringCell);
			}
			stringCell = instance.GetStringCell(text, rowName, "result type");
			if (stringCell != text2)
			{
				dictionary.Add("result_type", stringCell);
			}
			stringCell = instance.GetStringCell(text, rowName, "iap code");
			if (stringCell != text2)
			{
				dictionary.Add("code", stringCell);
			}
			stringCell = instance.GetStringCell(text, rowName, "display model type");
			if (stringCell != text2)
			{
				dictionary.Add("display", new Dictionary<string, object>
				{
					{ "model_type", stringCell },
					{
						"width",
						instance.GetIntCell(sheetIndex, rowIndex, "display width")
					},
					{
						"height",
						instance.GetIntCell(sheetIndex, rowIndex, "display height")
					}
				});
			}
			stringCell = instance.GetStringCell(text, rowName, "display default texture");
			if (stringCell != text2)
			{
				dictionary.Add("display.default", new Dictionary<string, object>
				{
					{ "texture", stringCell },
					{ "name", "default" }
				});
			}
			list.Add(dictionary);
		}
		return true;
	}

	public Dictionary<string, object> GetOfferByCode(string code)
	{
		return (!offersByCode.ContainsKey(code)) ? null : offersByCode[code];
	}

	private void LoadCostsHelper(Dictionary<int, Cost> costsDict, Dictionary<string, object> offerDict, string key)
	{
		if (offerDict.ContainsKey(key))
		{
			int key2 = TFUtils.LoadInt(offerDict, "identity");
			costsDict[key2] = Cost.FromDict((Dictionary<string, object>)offerDict[key]);
		}
	}

	private Cost GetCostHelper(Dictionary<int, Cost> dict, int did)
	{
		return (!dict.ContainsKey(did)) ? null : dict[did];
	}

	public Cost GetCost(int did)
	{
		return GetCostHelper(costs, did);
	}

	public Cost GetSellCost(int did)
	{
		return GetCostHelper(sellCosts, did);
	}

	public string GetDescription(int did)
	{
		if (descriptions.ContainsKey(did))
		{
			return descriptions[did];
		}
		return null;
	}

	public bool CanSell(int did)
	{
		if (canSell.ContainsKey(did))
		{
			return canSell[did];
		}
		return false;
	}

	public string SellError(int did)
	{
		if (sellErrors.ContainsKey(did))
		{
			return sellErrors[did];
		}
		return null;
	}

	public void GetNameAndTypeForDID(int nDID, out string sName, out string sType)
	{
		sName = (sType = string.Empty);
		string primaryType = string.Empty;
		foreach (object item in (List<object>)catalogDict["offers"])
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)item;
			if (!dictionary.ContainsKey("identity"))
			{
				continue;
			}
			int num = TFUtils.LoadInt(dictionary, "identity");
			if (num != nDID)
			{
				continue;
			}
			sType = null;
			if (dictionary.ContainsKey("type"))
			{
				sType = TFUtils.LoadString(dictionary, "type");
			}
			foreach (object item2 in (List<object>)catalogDict["market"])
			{
				Dictionary<string, object> dictionary2 = (Dictionary<string, object>)item2;
				if (dictionary2.ContainsKey("dids") && TFUtils.LoadList<int>(dictionary2, "dids").Contains(nDID))
				{
					if (string.IsNullOrEmpty(sType) && dictionary2.ContainsKey("name"))
					{
						sType = TFUtils.LoadString(dictionary2, "name");
					}
					if (dictionary2.ContainsKey("type"))
					{
						primaryType = TFUtils.LoadString(dictionary2, "type");
					}
					break;
				}
			}
			Blueprint blueprint = EntityManager.GetBlueprint(primaryType, nDID, true);
			if (blueprint == null)
			{
				continue;
			}
			sName = (string)blueprint.Invariable["name"];
			break;
		}
	}

	public static string ConvertTypeToDeltaDNAType(string sType)
	{
		if (string.IsNullOrEmpty(sType))
		{
			return string.Empty;
		}
		switch (sType)
		{
		case "buildings":
			sType = "Characters";
			break;
		case "buildings_no_resident":
			sType = "Buildings";
			break;
		case "production_buildings":
			sType = "Shops";
			break;
		case "trees":
			sType = "Trees";
			break;
		case "decorations":
			sType = "Decorations";
			break;
		case "rmt":
			sType = "Jelly_And_Coins";
			break;
		}
		return sType;
	}
}
