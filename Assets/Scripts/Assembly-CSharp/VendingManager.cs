using System;
using System.Collections.Generic;
using UnityEngine;

public class VendingManager
{
	public const ulong DEFAULT_RESTOCK_PERIOD = 3600uL;

	public const ulong DEFAULT_SPECIAL_PERIOD = 86400uL;

	private const string _sVENDORS = "Vendors";

	private const string _sVENDING_STOCKS = "VendingStock";

	private System.Random rand;

	private static readonly string VENDING_PATH = "Vending";

	private Dictionary<int, VendorDefinition> vendorDefinitions;

	private Dictionary<int, VendorStock> stocks;

	private Dictionary<Identity, Dictionary<int, VendingInstance>> instances;

	private Dictionary<Identity, Dictionary<int, VendingInstance>> specialOffers;

	public VendingManager()
	{
		rand = new System.Random(UnityEngine.Random.Range(-100000, 100000));
		vendorDefinitions = new Dictionary<int, VendorDefinition>();
		stocks = new Dictionary<int, VendorStock>();
		instances = new Dictionary<Identity, Dictionary<int, VendingInstance>>();
		specialOffers = new Dictionary<Identity, Dictionary<int, VendingInstance>>();
		LoadVending();
	}

	public VendingInstance GetVendingInstance(Identity target, int slotId)
	{
		Dictionary<int, VendingInstance> value = null;
		if (instances.TryGetValue(target, out value))
		{
			VendingInstance value2 = null;
			value.TryGetValue(slotId, out value2);
			return value2;
		}
		return null;
	}

	public VendingInstance GetSpecialInstance(Identity target)
	{
		Dictionary<int, VendingInstance> value = null;
		if (specialOffers.TryGetValue(target, out value))
		{
			VendingInstance value2 = null;
			value.TryGetValue(0, out value2);
			return value2;
		}
		return null;
	}

	public Dictionary<int, VendingInstance> GetVendingInstances(Identity target)
	{
		Dictionary<int, VendingInstance> value = null;
		instances.TryGetValue(target, out value);
		return value;
	}

	public Dictionary<int, VendingInstance> GetSpecialInstances(Identity target)
	{
		Dictionary<int, VendingInstance> value = null;
		specialOffers.TryGetValue(target, out value);
		return value;
	}

	public VendorDefinition GetVendorDefinition(int did)
	{
		VendorDefinition value;
		vendorDefinitions.TryGetValue(did, out value);
		return value;
	}

	public VendorStock GetStock(int stockId)
	{
		VendorStock value;
		stocks.TryGetValue(stockId, out value);
		return value;
	}

	public void GenerateNewGeneralInstances(VendingDecorator vendor)
	{
		VendorDefinition vendorDefinition = GetVendorDefinition(vendor.VendorId);
		Dictionary<int, VendingInstance> dictionary = new Dictionary<int, VendingInstance>(vendorDefinition.InstanceCount);
		List<int> list = new List<int>(vendorDefinition.generalStock.Count);
		for (int i = 0; i < vendorDefinition.generalStock.Count; i++)
		{
			list.Insert(rand.Next(0, list.Count), i);
		}
		for (int j = 0; j < vendorDefinition.InstanceCount; j++)
		{
			if (j < list.Count)
			{
				dictionary[j] = GetStock(vendorDefinition.generalStock[list[j]]).GenerateVendingInstance(j, false);
			}
			else
			{
				dictionary[j] = GetStock(vendorDefinition.generalStock[rand.Next(vendorDefinition.generalStock.Count)]).GenerateVendingInstance(j, false);
			}
		}
		instances[vendor.Id] = dictionary;
	}

	public void GenerateNewSpecialInstances(VendingDecorator vendor)
	{
		VendorDefinition vendorDefinition = GetVendorDefinition(vendor.VendorId);
		if (vendorDefinition.specialStock.Count > 0)
		{
			specialOffers[vendor.Id] = new Dictionary<int, VendingInstance> { 
			{
				0,
				GetStock(vendorDefinition.specialStock[rand.Next(vendorDefinition.specialStock.Count)]).GenerateVendingInstance(0, true)
			} };
		}
	}

	private string[] GetFilesToLoad()
	{
		return Config.VENDING_PATH;
	}

	private string GetFilePathFromString(string filePath)
	{
		return filePath;
	}

	private void LoadVending()
	{
		LoadVendorsFromSpreadseet("Vendors");
		LoadVendingStocksFromSpreadseet("VendingStock");
	}

	private void LoadVendorsFromSpreadseet(string sSheetName)
	{
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(sSheetName))
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(sSheetName);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + sSheetName);
			return;
		}
		int num = instance.GetNumRows(sSheetName);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + sSheetName);
			return;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		List<object> list = new List<object>();
		List<object> list2 = new List<object>();
		List<object> list3 = new List<object>();
		string text = "null";
		int num2 = -1;
		int num3 = -1;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			dictionary.Clear();
			list.Clear();
			list2.Clear();
			list3.Clear();
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(sSheetName, rowName, "id").ToString());
			dictionary.Add("type", "vendor");
			if (num2 < 0)
			{
				num2 = instance.GetIntCell(sheetIndex, rowIndex, "general items");
				num3 = instance.GetIntCell(sheetIndex, rowIndex, "special items");
			}
			dictionary.Add("did", instance.GetIntCell(sheetIndex, rowIndex, "did"));
			list.Add(instance.GetIntCell(sheetIndex, rowIndex, "background color r"));
			list.Add(instance.GetIntCell(sheetIndex, rowIndex, "background color g"));
			list.Add(instance.GetIntCell(sheetIndex, rowIndex, "background color b"));
			dictionary.Add("background.color", list);
			dictionary.Add("session_action_id", instance.GetStringCell(sSheetName, rowName, "session action id"));
			dictionary.Add("texture.cancelbutton", instance.GetStringCell(sSheetName, rowName, "cancel button texture"));
			dictionary.Add("texture.title", instance.GetStringCell(sSheetName, rowName, "title texture"));
			dictionary.Add("texture.titleicon", instance.GetStringCell(sSheetName, rowName, "title icon texture"));
			dictionary.Add("button.label", instance.GetStringCell(sSheetName, rowName, "button label"));
			dictionary.Add("open_sound", instance.GetStringCell(sSheetName, rowName, "open sound"));
			dictionary.Add("close_sound", instance.GetStringCell(sSheetName, rowName, "close sound"));
			dictionary.Add("restock_cost", new Dictionary<string, object> { 
			{
				"2",
				instance.GetIntCell(sheetIndex, rowIndex, "jelly restock cost")
			} });
			string text2 = instance.GetStringCell(sSheetName, rowName, "music");
			if (string.IsNullOrEmpty(text2) || text2 == text)
			{
				text2 = null;
			}
			dictionary.Add("music", text2);
			for (int j = 0; j < num2; j++)
			{
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, "general item did " + (j + 1));
				if (intCell >= 0)
				{
					list2.Add(intCell);
				}
			}
			dictionary.Add("general", list2);
			for (int k = 0; k < num3; k++)
			{
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, "special item did " + (k + 1));
				if (intCell >= 0)
				{
					list3.Add(intCell);
				}
			}
			dictionary.Add("specials", list3);
			VendorDefinition vendorDefinition = new VendorDefinition(dictionary);
			if (vendorDefinitions.ContainsKey(vendorDefinition.did))
			{
				TFUtils.ErrorLog(string.Concat("Vendor Definition Collision!\nOld=", vendorDefinitions[vendorDefinition.did], "\nNew=", vendorDefinition));
			}
			else
			{
				vendorDefinitions.Add(vendorDefinition.did, vendorDefinition);
			}
		}
	}

	private void LoadVendingStocksFromSpreadseet(string sSheetName)
	{
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(sSheetName))
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(sSheetName);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + sSheetName);
			return;
		}
		int num = instance.GetNumRows(sSheetName);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + sSheetName);
			return;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		List<object> list = new List<object>();
		List<object> list2 = new List<object>();
		int num2 = -1;
		int num3 = -1;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			dictionary.Clear();
			list.Clear();
			list2.Clear();
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(sSheetName, rowName, "id").ToString());
			dictionary.Add("type", "vendor_stock");
			if (num2 < 0)
			{
				num2 = instance.GetIntCell(sheetIndex, rowIndex, "max instances");
				num3 = instance.GetIntCell(sheetIndex, rowIndex, "max costs");
			}
			dictionary.Add("did", instance.GetIntCell(sheetIndex, rowIndex, "did"));
			dictionary.Add("minimum_level", instance.GetIntCell(sheetIndex, rowIndex, "min level"));
			dictionary.Add("required_recipe", instance.GetIntCell(sheetIndex, rowIndex, "required recipe"));
			dictionary.Add("name", instance.GetStringCell(sSheetName, rowName, "name"));
			dictionary.Add("tag", instance.GetStringCell(sSheetName, rowName, "tag"));
			dictionary.Add("description", instance.GetStringCell(sSheetName, rowName, "description"));
			dictionary.Add("icon", instance.GetStringCell(sSheetName, rowName, "icon"));
			dictionary.Add("reward", new Dictionary<string, object> { 
			{
				"resources",
				new Dictionary<string, object> { 
				{
					instance.GetIntCell(sheetIndex, rowIndex, "recipe did").ToString(),
					1
				} }
			} });
			for (int j = 0; j < num2; j++)
			{
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, "instance " + (j + 1));
				if (intCell >= 0)
				{
					list.Add(intCell);
				}
			}
			dictionary.Add("instances", list);
			for (int k = 0; k < num3; k++)
			{
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, "cost type " + (k + 1));
				if (intCell >= 0)
				{
					float floatCell = instance.GetFloatCell(sheetIndex, rowIndex, "cost odds " + (k + 1));
					int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "cost " + (k + 1));
					list2.Add(new Dictionary<string, object>
					{
						{ "p", floatCell },
						{
							"value",
							new Dictionary<string, object> { 
							{
								intCell.ToString(),
								intCell2
							} }
						}
					});
				}
			}
			dictionary.Add("costs", list2);
			VendorStock vendorStock = VendorStock.FromDict(dictionary);
			if (stocks.ContainsKey(vendorStock.Did))
			{
				TFUtils.ErrorLog(string.Concat("Vending Stock Collision!\nOld=", stocks[vendorStock.Did], "\nNew=", vendorStock));
			}
			else
			{
				stocks.Add(vendorStock.Did, vendorStock);
			}
		}
	}

	public void LoadVendorInstances(Identity target, Dictionary<string, object> generalInstances, Dictionary<string, object> specialInstances)
	{
		if (generalInstances != null)
		{
			Dictionary<int, VendingInstance> dictionary = new Dictionary<int, VendingInstance>(generalInstances.Count);
			foreach (KeyValuePair<string, object> generalInstance in generalInstances)
			{
				dictionary[int.Parse(generalInstance.Key)] = VendingInstance.FromDict((Dictionary<string, object>)generalInstance.Value);
			}
			instances[target] = dictionary;
		}
		if (specialInstances == null)
		{
			return;
		}
		Dictionary<int, VendingInstance> dictionary2 = new Dictionary<int, VendingInstance>(specialInstances.Count);
		foreach (KeyValuePair<string, object> specialInstance in specialInstances)
		{
			dictionary2[int.Parse(specialInstance.Key)] = VendingInstance.FromDict((Dictionary<string, object>)specialInstance.Value);
		}
		specialOffers[target] = dictionary2;
	}
}
