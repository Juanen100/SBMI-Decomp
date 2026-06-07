using System.Collections.Generic;

public class TreasureManager
{
	private const string TREASURE_PATH = "Treasure";

	private List<TreasureSpawner> treasureSpawners;

	public TreasureManager(Session session)
	{
	}

	private void LoadTreasureSpawnersFromSpread(Session pSession)
	{
	}

	private string[] GetFilesToLoad()
	{
		return null;
	}

	private string GetFilePathFromString(string filePath)
	{
		return null;
	}

	public void InitializeTreasureTimers(Dictionary<string, object> dict)
	{
	}

	public void OnUpdate(Session session)
	{
	}

	public void StartTreasureTimers()
	{
	}

	public TreasureSpawner FindTreasureSpawner(string persistName)
	{
		return null;
	}
}
