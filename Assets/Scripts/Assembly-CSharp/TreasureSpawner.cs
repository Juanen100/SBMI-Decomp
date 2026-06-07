using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class TreasureSpawner
{
	private static bool logDebugging;

	private int spawnLimit;

	private int count;

	private List<int> didsToSpawn;

	private string spawnMessage;

	private string featureLockName;

	private string persistName;

	private int minTime;

	private int maxTime;

	private Session session;

	private bool isPatchySpawner;

	private int tickSpawnCount;

	private Timer timer;

	private static System.Random rand;

	private ulong? nextTreasureTime;

	private bool featureUnlocked;

	public string SpawnMessage
	{
		get
		{
			return null;
		}
	}

	public string PersistName
	{
		get
		{
			return null;
		}
	}

	public ulong? TimeToTreasure
	{
		get
		{
			return null;
		}
	}

	public bool IsPatchySpawner
	{
		get
		{
			return false;
		}
	}

	public int SpawnLimit
	{
		get
		{
			return 0;
		}
	}

	public int MaxTime
	{
		get
		{
			return 0;
		}
	}

	public TreasureSpawner(List<int> didsToSpawn, string persistName, string featureLockName, int spawnLimit, int minTime, int maxTime, int patchySpawner, Session session)
	{
	}

	public void UpdateFeatureLock()
	{
	}

	private void Stop()
	{
	}

	public void Start()
	{
	}

	public void Reset(ulong? time)
	{
	}

	public static void TimerTick(TreasureSpawner timing)
	{
	}

	public void MarkComplete()
	{
	}

	public void MarkCollected()
	{
	}

	private void RecalculateCount()
	{
	}

	public bool PlaceTreasure()
	{
		return false;
	}

	public Vector2 GenerateLocation()
	{
		return default(Vector2);
	}
}
