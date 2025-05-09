#define ASSERTS_ON
using System;
using System.Collections.Generic;

public class PlaytimeRegistrar
{
	public const string PLAYTIME = "playtime";

	public const string LEVEL = "level";

	public const string WALLTIME_START = "wts_start";

	public const string LAST_TOUCHED = "last";

	public const string PLAYTIME_AT_LEVEL = "time_at";

	private ulong playtimeAtLevel;

	private ulong lastPlaytimeCheckin;

	private ulong walltimeLevelStart;

	private int level;

	public PlaytimeRegistrar(int level, ulong walltimeStartUtc, ulong lastTouchedUtc, ulong accruedPlaytimeAtLevelSeconds)
	{
		playtimeAtLevel = accruedPlaytimeAtLevelSeconds;
		walltimeLevelStart = walltimeStartUtc;
		lastPlaytimeCheckin = lastTouchedUtc;
		this.level = level;
	}

	public ulong GetWalltimeLevelStartUtc(int level)
	{
		TFUtils.Assert(this.level == level, "You can only get walltime for the current/last level! Querying level=" + level);
		return walltimeLevelStart;
	}

	public ulong GetPlaytimeAtLevelSeconds(int level)
	{
		TFUtils.Assert(this.level == level, "You can only get playtime at the current/last level!");
		return playtimeAtLevel;
	}

	public static bool IsTimeout(ulong utcLast, ulong utcNow, out ulong delta)
	{
		delta = utcNow - utcLast;
		return delta > 300;
	}

	public void Process(PersistedTriggerableAction action, int levelNow, SBAnalytics analytics)
	{
		ulong num = TFUtils.EpochTime();
		bool flag = action is LevelUpAction;
		if (action.IsUserInitiated || flag)
		{
			UpdatePlaytime(num);
		}
		while (flag && levelNow > level)
		{
			ulong num2 = num - walltimeLevelStart;
			ulong walltimeMinutes = Convert.ToUInt64((double)num2 / 60.0);
			ulong playtimeMinutes = Convert.ToUInt64((double)playtimeAtLevel / 60.0);
			analytics.LogLevelPlaytime(level, walltimeMinutes, playtimeMinutes);
			level++;
			playtimeAtLevel = 0uL;
			walltimeLevelStart = num;
		}
	}

	public void UpdateLevel(int level, ulong startUtc)
	{
		this.level = level;
		walltimeLevelStart = startUtc;
		playtimeAtLevel = 0uL;
	}

	public void UpdatePlaytime(ulong nowUtc)
	{
		ulong delta;
		if (!IsTimeout(lastPlaytimeCheckin, nowUtc, out delta))
		{
			playtimeAtLevel += delta;
		}
		lastPlaytimeCheckin = nowUtc;
	}

	public static PlaytimeRegistrar FromDict(Dictionary<string, object> data)
	{
		ulong walltimeStartUtc = TFUtils.LoadUlong(data, "wts_start");
		ulong lastTouchedUtc = TFUtils.LoadUlong(data, "last");
		ulong accruedPlaytimeAtLevelSeconds = TFUtils.LoadUlong(data, "time_at");
		int num = TFUtils.LoadInt(data, "level");
		return new PlaytimeRegistrar(num, walltimeStartUtc, lastTouchedUtc, accruedPlaytimeAtLevelSeconds);
	}

	public static void ApplyToGameState(ref Dictionary<string, object> gamestate, int level, ulong walltimeLevelStartUtc, ulong lastTouchedUtc, ulong playtimeAtLevelSeconds)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["level"] = level;
		dictionary["wts_start"] = walltimeLevelStartUtc;
		dictionary["last"] = lastTouchedUtc;
		dictionary["time_at"] = playtimeAtLevelSeconds;
		gamestate["playtime"] = dictionary;
	}
}
