using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class TreasureSpawner
{
	private static bool logDebugging = true;

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
			return spawnMessage;
		}
	}

	public string PersistName
	{
		get
		{
			return persistName;
		}
	}

	public ulong? TimeToTreasure
	{
		get
		{
			return nextTreasureTime;
		}
	}

	public bool IsPatchySpawner
	{
		get
		{
			return isPatchySpawner;
		}
	}

	public int SpawnLimit
	{
		get
		{
			return spawnLimit;
		}
	}

	public int MaxTime
	{
		get
		{
			return maxTime;
		}
	}

	public TreasureSpawner(List<int> didsToSpawn, string persistName, string featureLockName, int spawnLimit, int minTime, int maxTime, int patchySpawner, Session session)
	{
		this.didsToSpawn = didsToSpawn;
		spawnMessage = persistName + "_spawn";
		this.persistName = persistName;
		this.featureLockName = featureLockName;
		featureUnlocked = true;
		this.minTime = minTime;
		this.maxTime = maxTime;
		this.spawnLimit = spawnLimit;
		isPatchySpawner = patchySpawner == 1;
		tickSpawnCount = 1;
		if (isPatchySpawner)
		{
			tickSpawnCount = this.spawnLimit;
		}
		this.session = session;
		timer = new Timer();
		timer.AutoReset = false;
		if (rand == null)
		{
			rand = new System.Random(UnityEngine.Random.Range(-100000, 100000));
		}
	}

	public void UpdateFeatureLock()
	{
		if (!featureUnlocked)
		{
			featureUnlocked = session.TheGame.featureManager.CheckFeature(featureLockName);
			if (featureUnlocked)
			{
				Start();
			}
		}
	}

	private void Stop()
	{
		nextTreasureTime = null;
		timer.Enabled = false;
		if (logDebugging)
		{
			TFUtils.DebugLog("Treasure: " + persistName + " said stop");
		}
	}

	public void Start()
	{
		if (logDebugging)
		{
			TFUtils.DebugLog("Treasure: " + persistName + " said start");
		}
		featureUnlocked = session.TheGame.featureManager.CheckFeature(featureLockName);
		RecalculateCount();
		if (logDebugging)
		{
			TFUtils.DebugLog("Number of " + didsToSpawn[0] + " treasures " + count);
		}
		ulong num = TFUtils.EpochTime();
		if (count < spawnLimit && featureUnlocked)
		{
			if (!nextTreasureTime.HasValue)
			{
				if (logDebugging)
				{
					TFUtils.DebugLog("Create new time");
				}
				Stop();
				nextTreasureTime = num + (ulong)rand.Next(minTime, maxTime);
				if (!session.InFriendsGame)
				{
					session.TheGame.simulation.ModifyGameState(new TreasureCooldownAction(nextTreasureTime.Value, persistName));
				}
			}
			else
			{
				if (nextTreasureTime.Value <= num)
				{
					int num2 = spawnLimit - count;
					ulong num3 = num - nextTreasureTime.Value;
					int num4 = num2;
					if (!session.InFriendsGame)
					{
						int val = 1 + (int)((double)num3 / ((double)(maxTime + minTime) / 2.0));
						num4 = Math.Min(val, num2);
					}
					else
					{
						tickSpawnCount = spawnLimit;
					}
					session.AddAsyncResponse(spawnMessage, num4);
					if (logDebugging)
					{
						TFUtils.DebugLog("Just spawn " + num4 + " items because time was: " + (nextTreasureTime.Value - num));
					}
					return;
				}
				if (logDebugging)
				{
					TFUtils.DebugLog("Current Time: " + num + " Value exists: " + nextTreasureTime.Value);
				}
			}
			timer.Enabled = false;
			double num5 = 0.0;
			if (session.InFriendsGame)
			{
				if (SBMISoaring.PatchTownTreasureSpawnTimestamp > 0)
				{
					num5 = (double)SBMISoaring.PatchTownTreasureSpawnTimestamp - (double)SoaringTime.AdjustedServerTime;
				}
				if (num5 <= 0.0)
				{
					SBMISoaring.PatchTownTreasureSpawnTimestamp = SoaringTime.AdjustedServerTime + maxTime;
					SBMISoaring.PatchTownTreasureCollected = 0;
					Soaring.UpdateUserProfile(Soaring.Player.CustomData);
					num5 = 1.0;
				}
			}
			else
			{
				num5 = ((double)nextTreasureTime.Value - (double)num) * 1000.0;
			}
			if (num5 > 2147483647.0)
			{
				num5 = 2147483647.0;
			}
			timer.Interval = num5;
			timer.Enabled = true;
			timer.Elapsed += delegate
			{
				TimerTick(this);
			};
			if (logDebugging)
			{
				TFUtils.DebugLog("Treasure: " + persistName + " will start in: " + num5);
			}
		}
		else if (logDebugging)
		{
			TFUtils.DebugLog("We are not spawning treasure because: count( " + count + " ) of ( " + spawnLimit + " ) and featureLock is ( " + featureUnlocked + " )");
		}
	}

	public void Reset(ulong? time)
	{
		if (logDebugging)
		{
			TFUtils.DebugLog("RESET TIME");
		}
		Stop();
		nextTreasureTime = time;
		Start();
	}

	public static void TimerTick(TreasureSpawner timing)
	{
		if (logDebugging)
		{
			TFUtils.DebugLog("Timer fired, spawning with: " + timing.spawnMessage);
		}
		timing.session.AddAsyncResponse(timing.spawnMessage, timing.tickSpawnCount);
	}

	public void MarkComplete()
	{
		Stop();
		RecalculateCount();
		if (count < spawnLimit)
		{
			Start();
		}
		if (logDebugging)
		{
			TFUtils.DebugLog("Treasure spawned is done, " + count + " treasures left");
		}
	}

	public void MarkCollected()
	{
		RecalculateCount();
		count--;
		Start();
		if (logDebugging)
		{
			TFUtils.DebugLog("Treasure was collected, " + count + " treasures left");
		}
	}

	private void RecalculateCount()
	{
		count = 0;
		foreach (int item in didsToSpawn)
		{
			List<Simulated> list = session.TheGame.simulation.FindAllSimulateds(item, EntityType.TREASURE);
			count += list.Count;
		}
	}

	public bool PlaceTreasure()
	{
		if (logDebugging)
		{
			TFUtils.DebugLog("Place treasure");
		}
		bool result = false;
		long num = 0L;
		if (session.InFriendsGame)
		{
			num = SBMISoaring.PatchTownTreasureCollected;
		}
		if (count + num < spawnLimit)
		{
			int index = rand.Next(didsToSpawn.Count);
			Entity entity = session.TheGame.entities.Create(EntityType.TREASURE, didsToSpawn[index], true);
			Vector2 position = GenerateLocation();
			Simulated simulated = session.TheGame.simulation.CreateSimulated(entity, EntityManager.TreasureActions["buried"], position);
			simulated.GetEntity<TreasureEntity>().TreasureTiming = this;
			simulated.Warp(simulated.Position, session.TheGame.simulation);
			simulated.Visible = true;
			result = true;
			simulated.SetDisplayOffsetWorld(session.TheGame.simulation);
			if (session.InFriendsGame)
			{
				RecalculateCount();
			}
			else
			{
				MarkComplete();
			}
			Debug.Log("Placing Treasure:" + didsToSpawn[index] + "  how many spawned:" + count + " limit is" + spawnLimit);
			if (!session.InFriendsGame)
			{
				session.TheGame.simulation.ModifyGameStateSimulated(simulated, new TreasureSpawnAction(simulated, this));
			}
		}
		return result;
	}

	public Vector2 GenerateLocation()
	{
		int num = 10;
		bool flag = false;
		AlignedBox purchasedExtent = session.TheGame.terrain.PurchasedExtent;
		Vector2 zero = Vector2.zero;
		int num2 = Mathf.RoundToInt(purchasedExtent.xmin);
		int maxValue = Mathf.RoundToInt(purchasedExtent.xmax);
		int num3 = Mathf.RoundToInt(purchasedExtent.ymin);
		int maxValue2 = Mathf.RoundToInt(purchasedExtent.ymax);
		while (num > 0 && !flag)
		{
			zero.x = rand.Next(num2, maxValue);
			zero.y = rand.Next(num3, maxValue2);
			Vector2 point = zero;
			int num4 = rand.Next(2);
			if (num4 == 1)
			{
				zero.x = num2;
				point.x = zero.x + 1f;
			}
			else
			{
				zero.y = num3;
				point.y = zero.y + 1f;
			}
			flag = session.TheGame.simulation.Terrain.CheckIsPurchasedArea(point);
			num--;
		}
		return zero;
	}
}
