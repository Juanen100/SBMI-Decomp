#define ASSERTS_ON
using System.Collections.Generic;

public class TreasureManager
{
	private const string TREASURE_PATH = "Treasure";

	private List<TreasureSpawner> treasureSpawners;

	public TreasureManager(Session session)
	{
		treasureSpawners = new List<TreasureSpawner>();
		LoadTreasureSpawnersFromSpread(session);
	}

	private void LoadTreasureSpawnersFromSpread(Session pSession)
	{
		string text = "TreasureSpawn";
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
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, "id").ToString());
			int intCell = instance.GetIntCell(sheetIndex, rowIndex, "is patchy town");
			bool flag = intCell == 1;
			if (flag == pSession.InFriendsGame)
			{
				treasureSpawners.Add(new TreasureSpawner(new List<int> { instance.GetIntCell(sheetIndex, rowIndex, "items to spawn") }, instance.GetStringCell(sheetIndex, rowIndex, "persist name"), instance.GetStringCell(sheetIndex, rowIndex, "feature lock"), instance.GetIntCell(sheetIndex, rowIndex, "spawn limit"), instance.GetIntCell(sheetIndex, rowIndex, "min time"), instance.GetIntCell(sheetIndex, rowIndex, "max time"), intCell, pSession));
			}
		}
	}

	private string[] GetFilesToLoad()
	{
		return Config.TREASURE_PATH;
	}

	private string GetFilePathFromString(string filePath)
	{
		return filePath;
	}

	public void InitializeTreasureTimers(Dictionary<string, object> dict)
	{
		TFUtils.Assert(treasureSpawners.Count > 0, "We need to init TreasureManager before getting data from it");
		foreach (TreasureSpawner treasureSpawner in treasureSpawners)
		{
			ulong? time = TFUtils.TryLoadNullableUlong(dict, treasureSpawner.PersistName);
			treasureSpawner.Reset(time);
		}
	}

	public void OnUpdate(Session session)
	{
		foreach (TreasureSpawner treasureSpawner in treasureSpawners)
		{
			treasureSpawner.UpdateFeatureLock();
			int? num = (int?)session.CheckAsyncRequest(treasureSpawner.SpawnMessage);
			if (num.HasValue)
			{
				int num2 = 0;
				while (num.HasValue && num2 < num.Value)
				{
					treasureSpawner.PlaceTreasure();
					num2++;
				}
			}
		}
	}

	public void StartTreasureTimers()
	{
		foreach (TreasureSpawner treasureSpawner in treasureSpawners)
		{
			treasureSpawner.Start();
		}
	}

	public TreasureSpawner FindTreasureSpawner(string persistName)
	{
		return treasureSpawners.Find((TreasureSpawner t) => t.PersistName == persistName);
	}
}
