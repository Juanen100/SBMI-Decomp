using System.Collections.Generic;

public class Player
{
	private static string mCACHE_ROOT;

	private const string LAST_PLAYED = "lastplayer";

	private const string PLAYER_ID_MAP = "player_map";

	private static string LAST_PLAYED_FILE;

	private static string PLAYER_ID_MAP_FILE;

	private const string USER_FILE = "user.json";

	private const string PLAYER_TIMESTAMP = "timestamp";

	private string cacheDir;

	private long mStagedTimestamp;

	public string playerId;

	private const string CORRUPT_IOS7_DEVICE_ID = "0f607264fc6318a9";

	private static string CACHE_ROOT
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public Player(string playerId)
	{
	}

	public static Player LoadFromSoaringID(string userID)
	{
		return null;
	}

	public static bool CheckSoaringPathExists(string userID)
	{
		return false;
	}

	public static void MigratePlayerData(string soaringUserID, string playerId)
	{
	}

	public static bool ValidTimeStamp(long timestamp)
	{
		return false;
	}

	public void SetStagedTimestamp(long ts)
	{
	}

	public long ReadTimestamp()
	{
		return 0L;
	}

	public void SaveStagedTimestamp()
	{
	}

	public void SaveTimestamp(long timestamp)
	{
	}

	public void DeleteTimestamp()
	{
	}

	public static string RemovePrefix(string playerId)
	{
		return null;
	}

	public static string LastPlayerId()
	{
		return null;
	}

	public string CacheFile(string fileName)
	{
		return null;
	}

	public static string PlayerCacheFile(string player, string fileName)
	{
		return null;
	}

	public string CacheDir()
	{
		return null;
	}

	private string PlayerFolder()
	{
		return null;
	}

	public static Dictionary<string, object> GetPlayerMap()
	{
		return null;
	}

	private static string GetPreviousDeviceIdFromPlayerMap(string currentDeviceId)
	{
		return null;
	}

	private static string _CheckMigrateDirectory(string playerID)
	{
		return null;
	}

	public static void Init()
	{
	}
}
