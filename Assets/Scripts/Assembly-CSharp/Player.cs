using System.Collections.Generic;
using System.IO;
using MiniJSON;
using UnityEngine;

public class Player
{
	private const string LAST_PLAYED = "lastplayer";

	private const string PLAYER_ID_MAP = "player_map";

	private const string USER_FILE = "user.json";

	private const string PLAYER_TIMESTAMP = "timestamp";

	private const string CORRUPT_IOS7_DEVICE_ID = "0f607264fc6318a9";

	private static string CACHE_ROOT = Application.persistentDataPath + Path.DirectorySeparatorChar;

	private static string LAST_PLAYED_FILE = CACHE_ROOT + "lastplayer";

	private static string PLAYER_ID_MAP_FILE = CACHE_ROOT + "player_map";

	private string cacheDir;

	private long mStagedTimestamp = -1L;

	public string playerId;

	public Player(string playerId)
	{
		TFUtils.DebugLog(string.Format("player created with id: {0} ", playerId));
		this.playerId = playerId;
		cacheDir = CACHE_ROOT + PlayerFolder();
	}

	public static Player LoadFromSoaringID(string userID)
	{
		return new Player(userID);
	}

	public static bool CheckSoaringPathExists(string userID)
	{
		string path = CACHE_ROOT + Path.DirectorySeparatorChar + userID;
		return Directory.Exists(path);
	}

	public static void MigratePlayerData(string soaringUserID, string playerId)
	{
		string text = CACHE_ROOT + Path.DirectorySeparatorChar + soaringUserID;
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		string text2 = null;
		if (TFUtils.FileIsExists(PLAYER_ID_MAP_FILE))
		{
			if (playerId != null)
			{
				string text3 = playerId;
				if (text3.Length > 16)
				{
					text3 = playerId.Substring(0, 16);
				}
				string json = TFUtils.ReadAllText(PLAYER_ID_MAP_FILE).Trim();
				Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(json);
				if (dictionary.ContainsKey(playerId))
				{
					text2 = _CheckMigrateDirectory((string)dictionary[playerId]);
				}
				else if (dictionary.ContainsKey(text3))
				{
					text2 = _CheckMigrateDirectory((string)dictionary[text3]);
				}
				if (text2 == null)
				{
					string previousDeviceIdFromPlayerMap = GetPreviousDeviceIdFromPlayerMap(playerId);
					if (string.IsNullOrEmpty(playerId))
					{
						previousDeviceIdFromPlayerMap = GetPreviousDeviceIdFromPlayerMap(text3);
					}
					if (!string.IsNullOrEmpty(previousDeviceIdFromPlayerMap) && (text2 = _CheckMigrateDirectory(previousDeviceIdFromPlayerMap)) == null && dictionary.ContainsKey(previousDeviceIdFromPlayerMap))
					{
						text2 = _CheckMigrateDirectory((string)dictionary[previousDeviceIdFromPlayerMap]);
					}
				}
				if (text2 == null)
				{
					text2 = _CheckMigrateDirectory("p_" + playerId);
				}
				if (text2 == null && dictionary.ContainsKey("p_" + playerId))
				{
					text2 = _CheckMigrateDirectory("p_" + playerId);
				}
			}
			else
			{
				text2 = CACHE_ROOT + TFUtils.ReadAllText(LAST_PLAYED_FILE).Trim() + Path.DirectorySeparatorChar;
			}
		}
		if (!string.IsNullOrEmpty(text2))
		{
			Debug.LogError("SoaringID: " + soaringUserID + " : " + playerId + " : " + text2);
			if (TFUtils.FileIsExists(text2 + "actions.json"))
			{
				File.Copy(text2 + "actions.json", text + Path.DirectorySeparatorChar + "actions.json", true);
			}
			if (TFUtils.FileIsExists(text2 + "game.json"))
			{
				File.Copy(text2 + "game.json", text + Path.DirectorySeparatorChar + "game.json", true);
			}
			if (TFUtils.FileIsExists(text2 + "lastETag"))
			{
				File.Copy(text2 + "lastETag", text + Path.DirectorySeparatorChar + "lastETag", true);
			}
		}
		else
		{
			SoaringDebug.Log("Invalid Player ID, No Directory Found", LogType.Error);
		}
	}

	public static bool ValidTimeStamp(long timestamp)
	{
		return timestamp > 0;
	}

	public void SetStagedTimestamp(long ts)
	{
		mStagedTimestamp = ts;
		Debug.LogWarning("SetStagedTimestamp: " + mStagedTimestamp);
	}

	public long ReadTimestamp()
	{
		long result = -1L;
		string text = CacheFile("timestamp");
		if (TFUtils.FileIsExists(text))
		{
			try
			{
				string s = TFUtils.ReadAllText(text);
				if (!long.TryParse(s, out result))
				{
					result = -1L;
				}
			}
			catch
			{
				result = -1L;
			}
		}
		Debug.LogWarning("ReadTimestamp: " + result + " : " + text);
		return result;
	}

	public void SaveStagedTimestamp()
	{
		if (ValidTimeStamp(mStagedTimestamp))
		{
			Debug.LogWarning("SaveStagedTimestamp: " + mStagedTimestamp);
			SaveTimestamp(mStagedTimestamp);
		}
		mStagedTimestamp = -1L;
	}

	public void SaveTimestamp(long timestamp)
	{
		File.WriteAllText(CacheFile("timestamp"), timestamp.ToString());
		Debug.LogWarning("SaveTimestamp: " + timestamp);
	}

	public void DeleteTimestamp()
	{
		Debug.LogWarning("DeleteTimestamp");
		string text = CacheFile("timestamp");
		if (TFUtils.FileIsExists(text))
		{
			File.Delete(text);
		}
	}

	public static string RemovePrefix(string playerId)
	{
		if (playerId == null)
		{
			return null;
		}
		if (playerId.StartsWith("p_"))
		{
			return playerId.Substring(2);
		}
		return playerId;
	}

	public static string LastPlayerId()
	{
		string text = TFUtils.ReadAllText(LAST_PLAYED_FILE);
		if (text == null)
		{
			return null;
		}
		return RemovePrefix(text.Trim());
	}

	public string CacheFile(string fileName)
	{
		return cacheDir + Path.DirectorySeparatorChar + fileName;
	}

	public static string PlayerCacheFile(string player, string fileName)
	{
		return CACHE_ROOT + player + Path.DirectorySeparatorChar + fileName;
	}

	public string CacheDir()
	{
		return cacheDir;
	}

	private string PlayerFolder()
	{
		return playerId;
	}

	public static Dictionary<string, object> GetPlayerMap()
	{
		if (File.Exists(PLAYER_ID_MAP_FILE))
		{
			string json = TFUtils.ReadAllText(PLAYER_ID_MAP_FILE);
			return (Dictionary<string, object>)Json.Deserialize(json);
		}
		return new Dictionary<string, object>();
	}

	private static string GetPreviousDeviceIdFromPlayerMap(string currentDeviceId)
	{
		Dictionary<string, object> playerMap = GetPlayerMap();
		foreach (KeyValuePair<string, object> item in playerMap)
		{
			if (!item.Key.Contains("G:") && item.Key != currentDeviceId && item.Key != "0f607264fc6318a9")
			{
				return item.Key;
			}
		}
		return null;
	}

	private static string _CheckMigrateDirectory(string playerID)
	{
		if (string.IsNullOrEmpty(playerID))
		{
			return null;
		}
		string text = CACHE_ROOT + playerID + Path.DirectorySeparatorChar;
		if (Directory.Exists(text))
		{
			return text;
		}
		return null;
	}

	public static void Init()
	{
		CACHE_ROOT = Application.persistentDataPath + Path.DirectorySeparatorChar;
	}
}
