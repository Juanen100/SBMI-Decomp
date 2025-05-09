#define ASSERTS_ON
using System;
using System.Collections.Generic;
using System.IO;
using MiniJSON;
using UnityEngine;

public static class InitialGame
{
	private const string INIT_FILE = "starting_bikini_bottom.json";

	public static Dictionary<string, object> Generate(EntityManager entityManager, ResourceManager resourceManager)
	{
		Dictionary<string, object> gamestate = new Dictionary<string, object>();
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		Dictionary<string, object> dictionary2 = LoadInitialGameFromSpreads();
		ulong num = TFUtils.EpochTime();
		PlaytimeRegistrar.ApplyToGameState(ref gamestate, 1, num, num, 0uL);
		List<object> list = new List<object>();
		foreach (Dictionary<string, object> item2 in dictionary2["buildings"] as List<object>)
		{
			int num2 = TFUtils.LoadInt(item2, "did");
			string value = (string)item2["label"];
			EntityType entityType = EntityType.BUILDING;
			if (item2.ContainsKey("extensions"))
			{
				entityType = (EntityType)((int)entityType | (int)TFUtils.LoadUint(item2, "extensions"));
			}
			BuildingEntity decorator = entityManager.Create(entityType, num2, new Identity(value), false).GetDecorator<BuildingEntity>();
			Dictionary<string, object> data = new Dictionary<string, object>();
			data["did"] = num2;
			data["extensions"] = (uint)entityType;
			data["label"] = value;
			data["x"] = TFUtils.LoadInt(item2, "x");
			data["y"] = TFUtils.LoadInt(item2, "y");
			data["flip"] = (bool)item2["flip"];
			ActivatableDecorator.Serialize(ref data, num);
			data["build_finish_time"] = num;
			if (decorator.HasDecorator<PeriodicProductionDecorator>())
			{
				data["rent_ready_time"] = num + decorator.GetDecorator<PeriodicProductionDecorator>().RentProductionTime;
			}
			else
			{
				data["rent_ready_time"] = null;
			}
			list.Add(data);
		}
		List<object> list2 = new List<object>();
		if (dictionary2.ContainsKey("debris"))
		{
			foreach (Dictionary<string, object> item3 in dictionary2["debris"] as List<object>)
			{
				int num3 = TFUtils.LoadInt(item3, "did");
				string value2 = (string)item3["label"];
				Dictionary<string, object> dictionary5 = new Dictionary<string, object>();
				dictionary5["did"] = num3;
				dictionary5["label"] = value2;
				dictionary5["x"] = TFUtils.LoadInt(item3, "x");
				dictionary5["y"] = TFUtils.LoadInt(item3, "y");
				list2.Add(dictionary5);
			}
		}
		List<object> list3 = new List<object>();
		if (dictionary2.ContainsKey("units"))
		{
			foreach (Dictionary<string, object> item4 in dictionary2["units"] as List<object>)
			{
				string value3 = (string)item4["label"];
				Dictionary<string, object> dictionary7 = new Dictionary<string, object>();
				dictionary7["did"] = TFUtils.LoadInt(item4, "did");
				dictionary7["label"] = value3;
				dictionary7["residence"] = item4["residence"];
				dictionary7["feed_ready_time"] = num;
				dictionary7["fullness_length"] = 90;
				dictionary7["waiting"] = ((!item4.ContainsKey("waiting")) ? ((object)false) : item4["waiting"]);
				dictionary7["active"] = true;
				list3.Add(dictionary7);
			}
		}
		List<object> list4 = new List<object>();
		foreach (Dictionary<string, object> item5 in dictionary2["resources"] as List<object>)
		{
			int num4 = TFUtils.LoadInt(item5, "id");
			int amountEarned = TFUtils.LoadInt(item5, "amount_earned");
			list4.Add(MakeResource(num4, resourceManager.Resources[num4].Name, amountEarned, 0, 0));
		}
		List<object> list5 = new List<object>();
		if (dictionary2.ContainsKey("paths"))
		{
			foreach (Dictionary<string, object> item6 in dictionary2["paths"] as List<object>)
			{
				list5.Add(item6);
			}
		}
		List<object> list6 = new List<object>();
		foreach (object item7 in dictionary2["expansions"] as List<object>)
		{
			list6.Add(item7);
		}
		List<object> list7 = new List<object>();
		foreach (object item8 in dictionary2["recipes"] as List<object>)
		{
			list7.Add(item8);
		}
		List<object> list8 = new List<object>();
		foreach (object item9 in dictionary2["movies"] as List<object>)
		{
			list8.Add(item9);
		}
		List<object> value4 = new List<object>();
		List<object> value5 = new List<object>();
		TFUtils.Assert(!dictionary2.ContainsKey("tasks"), "Should not try to define task instances in the initial state");
		List<object> value6 = new List<object>();
		List<object> list9 = new List<object>();
		if (dictionary2.ContainsKey("quests"))
		{
			foreach (object item10 in dictionary2["quests"] as List<object>)
			{
				Dictionary<string, object> dictionary8 = new Dictionary<string, object>();
				dictionary8["did"] = item10;
				dictionary8["start_time"] = num;
				dictionary8["completion_time"] = null;
				dictionary8["reminded"] = false;
				Dictionary<string, object> dictionary9 = new Dictionary<string, object>();
				dictionary9["met_start_condition_ids"] = new List<object>();
				dictionary9["met_end_condition_ids"] = new List<object>();
				dictionary8["conditions"] = dictionary9;
				list9.Add(dictionary8);
			}
		}
		List<object> value7 = new List<object>();
		dictionary.Add("buildings", list);
		dictionary.Add("debris", list2);
		dictionary.Add("units", list3);
		dictionary.Add("resources", list4);
		dictionary.Add("tasks", value6);
		dictionary.Add("quests", list9);
		dictionary.Add("generated_quest_definition", value7);
		dictionary.Add("pavement", list5);
		dictionary.Add("expansions", list6);
		dictionary.Add("recipes", list7);
		dictionary.Add("crafts", value5);
		dictionary.Add("movies", list8);
		dictionary.Add("drop_pickups", value4);
		TFUtils.DebugLog("Completed Init Data Load");
		dictionary.Add("last_action", null);
		gamestate.Add("farm", dictionary);
		List<object> value8 = new List<object>();
		gamestate.Add("dialogs", value8);
		gamestate["protocol_version"] = GamestateMigrator.CURRENT_VERSION;
		return gamestate;
	}

	private static void WriteInitFile(Session session, string outPath)
	{
		Dictionary<string, object> dictionary = LoadInitialGameFromSpreads();
		Simulation simulation = session.TheGame.simulation;
		List<object> list = new List<object>();
		foreach (Simulated simulated in simulation.GetSimulateds())
		{
			if (simulated.entity is DebrisEntity)
			{
				DebrisEntity debrisEntity = (DebrisEntity)simulated.entity;
				if (!debrisEntity.ExpansionId.HasValue)
				{
					list.Add(new Dictionary<string, object>
					{
						{ "did", debrisEntity.DefinitionId },
						{
							"x",
							simulated.Position.x
						},
						{
							"y",
							simulated.Position.y
						},
						{
							"label",
							debrisEntity.Id.Describe()
						}
					});
				}
			}
		}
		dictionary["debris"] = list;
		List<object> list2 = new List<object>();
		foreach (Simulated simulated2 in simulation.GetSimulateds())
		{
			if ((simulated2.entity.AllTypes & EntityType.BUILDING) != EntityType.INVALID)
			{
				list2.Add(new Dictionary<string, object>
				{
					{
						"did",
						simulated2.entity.DefinitionId
					},
					{
						"x",
						simulated2.Position.x
					},
					{
						"y",
						simulated2.Position.y
					},
					{ "flip", simulated2.Flip },
					{
						"label",
						simulated2.entity.Id.Describe()
					},
					{
						"extensions",
						(uint)simulated2.entity.AllTypes
					}
				});
			}
		}
		dictionary["buildings"] = list2;
		List<object> list3 = new List<object>();
		for (int i = 0; i < session.TheGame.terrain.GridWidth; i++)
		{
			for (int j = 0; j < session.TheGame.terrain.GridWidth; j++)
			{
				GridPosition gridPosition = new GridPosition(j, i);
				TerrainType terrainType = session.TheGame.terrain.GetTerrainType(gridPosition);
				if (terrainType != null && terrainType.IsPath())
				{
					list3.Add(new Dictionary<string, object>
					{
						{ "col", i },
						{ "row", j }
					});
				}
			}
		}
		dictionary["paths"] = list3;
		string path = outPath + Path.DirectorySeparatorChar + "starting_bikini_bottom.json";
		string contents = Json.Serialize(dictionary);
		File.WriteAllText(path, contents);
	}

	private static void WriteSlotFiles(Session session, string terrainPath)
	{
		Simulation simulation = session.TheGame.simulation;
		Dictionary<int, TerrainSlot> expansionSlots = session.TheGame.terrain.ExpansionSlots;
		foreach (Simulated simulated in simulation.GetSimulateds())
		{
			if (!(simulated.entity is DebrisEntity))
			{
				continue;
			}
			DebrisEntity debris = (DebrisEntity)simulated.entity;
			if (debris.ExpansionId.HasValue)
			{
				TerrainSlot terrainSlot = expansionSlots[debris.ExpansionId.Value];
				int num = terrainSlot.debris.FindIndex((TerrainSlotObject obj) => obj.id == debris.Id);
				if (num >= 0)
				{
					TerrainSlotObject value = terrainSlot.debris[num];
					value.position.col = (int)simulated.Position.x;
					value.position.row = (int)simulated.Position.y;
					terrainSlot.debris[num] = value;
				}
			}
		}
		foreach (TerrainSlot value2 in expansionSlots.Values)
		{
			string text = "slot" + value2.Id + ".json";
			string streamingAssetsFileInDirectory = TFUtils.GetStreamingAssetsFileInDirectory("Terrain", text);
			string json = TFUtils.ReadAllText(streamingAssetsFileInDirectory);
			Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(json);
			dictionary["debris"] = TerrainSlot.SerializeExpansionObjectData(value2.debris);
			string path = terrainPath + Path.DirectorySeparatorChar + text;
			string contents = Json.Serialize(dictionary);
			File.WriteAllText(path, contents);
		}
	}

	public static void WriteUpdatedFile(Session session)
	{
		string text = TFUtils.ApplicationPersistentDataPath + Path.DirectorySeparatorChar + "freeEdit_" + DateTime.Now.ToString("yyyy'-'MM'-'dd'-'HH'-'mm'-'ss");
		string text2 = text + Path.DirectorySeparatorChar + "Init";
		Directory.CreateDirectory(text2);
		string text3 = text + Path.DirectorySeparatorChar + "Terrain";
		Directory.CreateDirectory(text3);
		WriteInitFile(session, text2);
		WriteSlotFiles(session, text3);
		Debug.Log("wrote file to " + text);
	}

	private static Dictionary<string, object> MakeResource(int did, string name, int amountEarned, int amountSpent, int amountPurchased)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["did"] = did;
		dictionary["name"] = name;
		dictionary["amount_earned"] = amountEarned;
		dictionary["amount_spent"] = amountSpent;
		dictionary["amount_purchased"] = amountPurchased;
		return dictionary;
	}

	private static Dictionary<string, object> LoadInitialGameFromSpreads()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("type", "init");
		LoadInitialBlueprintsFromSpread(dictionary);
		LoadInitialResourcesFromSpread(dictionary);
		LoadInitialPathsFromSpread(dictionary);
		LoadInitialUnlocksFromSpread(dictionary);
		return dictionary;
	}

	private static void LoadInitialBlueprintsFromSpread(Dictionary<string, object> pData)
	{
		string text = "InitialBlueprints";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
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
			string stringCell = instance.GetStringCell(text, rowName, "type");
			if (!string.IsNullOrEmpty(stringCell) && !(stringCell == text2))
			{
				if (!pData.ContainsKey(stringCell))
				{
					pData.Add(stringCell, new List<object>());
				}
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary.Add("did", instance.GetIntCell(sheetIndex, rowIndex, "did"));
				dictionary.Add("label", instance.GetStringCell(text, rowName, "label"));
				dictionary.Add("flip", instance.GetIntCell(sheetIndex, rowIndex, "flip") == 1);
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, "position x");
				if (intCell >= 0)
				{
					dictionary.Add("x", intCell);
				}
				intCell = instance.GetIntCell(sheetIndex, rowIndex, "position y");
				if (intCell >= 0)
				{
					dictionary.Add("y", intCell);
				}
				intCell = instance.GetIntCell(sheetIndex, rowIndex, "extensions");
				if (intCell >= 0)
				{
					dictionary.Add("extensions", intCell);
				}
				string stringCell2 = instance.GetStringCell(text, rowName, "residence");
				if (!string.IsNullOrEmpty(stringCell2) && stringCell2 != text2)
				{
					dictionary.Add("residence", stringCell2);
				}
				((List<object>)pData[stringCell]).Add(dictionary);
			}
		}
	}

	private static void LoadInitialResourcesFromSpread(Dictionary<string, object> pData)
	{
		string text = "InitialResources";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		if (!pData.ContainsKey("resources"))
		{
			pData.Add("resources", new List<object>());
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
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("id", instance.GetIntCell(sheetIndex, rowIndex, "did"));
			dictionary.Add("amount_earned", instance.GetIntCell(sheetIndex, rowIndex, "amount earned"));
			dictionary.Add("amount_purchased", 0);
			dictionary.Add("amount_spent", 0);
			((List<object>)pData["resources"]).Add(dictionary);
		}
	}

	private static void LoadInitialPathsFromSpread(Dictionary<string, object> pData)
	{
		string text = "InitialPaths";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		if (!pData.ContainsKey("paths"))
		{
			pData.Add("paths", new List<object>());
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
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("col", instance.GetIntCell(sheetIndex, rowIndex, "column"));
			dictionary.Add("row", instance.GetIntCell(sheetIndex, rowIndex, "row"));
			((List<object>)pData["paths"]).Add(dictionary);
		}
	}

	private static void LoadInitialUnlocksFromSpread(Dictionary<string, object> pData)
	{
		string text = "InitialUnlocks";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
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
			string stringCell = instance.GetStringCell(text, rowName, "type");
			if (!string.IsNullOrEmpty(stringCell) && !(stringCell == text2))
			{
				if (!pData.ContainsKey(stringCell))
				{
					pData.Add(stringCell, new List<object>());
				}
				((List<object>)pData[stringCell]).Add(instance.GetIntCell(sheetIndex, rowIndex, "did"));
			}
		}
	}
}
