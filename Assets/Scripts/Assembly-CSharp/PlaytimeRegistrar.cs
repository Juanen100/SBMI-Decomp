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
	}

	public ulong GetWalltimeLevelStartUtc(int level)
	{
		return 0uL;
	}

	public ulong GetPlaytimeAtLevelSeconds(int level)
	{
		return 0uL;
	}

	public static bool IsTimeout(ulong utcLast, ulong utcNow, out ulong delta)
	{
		delta = default(ulong);
		return false;
	}

	public void Process(PersistedTriggerableAction action, int levelNow, SBAnalytics analytics)
	{
	}

	public void UpdateLevel(int level, ulong startUtc)
	{
	}

	public void UpdatePlaytime(ulong nowUtc)
	{
	}

	public static PlaytimeRegistrar FromDict(Dictionary<string, object> data)
	{
		return null;
	}

	public static void ApplyToGameState(ref Dictionary<string, object> gamestate, int level, ulong walltimeLevelStartUtc, ulong lastTouchedUtc, ulong playtimeAtLevelSeconds)
	{
	}
}
