using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;

public class TFUtils
{
	public enum LogLevel
	{
		INFO = 0,
		WARN = 1,
		ERROR = 2
	}

	public enum LogFilter
	{
		All = 0,
		CraftingManager = 1,
		Resources = 2,
		Assets = 4,
		Paytables = 8,
		Features = 16,
		Tasks = 32,
		Terrain = 64,
		Quests = 128,
		Vending = 256,
		Buildings = 512,
		Residents = 1024,
		None = int.MaxValue
	}

	public class SendLogDumpDelegate : SoaringDelegate
	{
		public override void OnSavingSessionData(bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
		{
		}
	}

	public class GameDetails
	{
		public int lastPlayTime;

		public string dtLastPlayTime;

		public string money;

		public string jelly;

		public string patties;

		public string level;
	}

	public static DateTime EPOCH;

	public static ulong APP_START_TIME;

	public static LogLevel LOG_LEVEL;

	public static StringBuilder PrevLog;

	public static StringBuilder ConsoleLog;

	public static StringBuilder ErrorConsoleLog;

	public const int kMaxSaveLogLength = 131072;

	public static LogFilter LOG_FILTER;

	public static string ApplicationDataPath;

	public static string ApplicationPersistentDataPath;

	public static string DeviceId;

	public static string DeviceName;

	public static ulong DebugTimeOffset;

	private const float MESSAGE_TIME = 1f;

	private static object lastTimedMessage;

	private static float lastTimedMessageTime;

	public static bool isFastForwarding;

	private static float timeMultiplier;

	public static ulong FastForwardOffset;

	public static ulong AddTimeOffset;

	private static DateTime seedUtcNow;

	private const int LOG_TYPE_NONE = -1;

	private const int LOG_TYPE_ERROR = 0;

	private const int LOG_TYPE_WARNING = 1;

	private const int LOG_TYPE_STANDARD = 2;

	public static string playerID;

	public static string playerAlias;

	public static float TimeMultiplier
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public static DateTime UtcNow
	{
		get
		{
			return default(DateTime);
		}
		set
		{
		}
	}

	public static void Init()
	{
	}

	private static void WriteConsoleLog(string tx)
	{
	}

	public static ulong EpochTime()
	{
		return 0uL;
	}

	public static ulong EpochTime(DateTime dt)
	{
		return 0uL;
	}

	public static DateTime EpochToDateTime(ulong seconds)
	{
		return default(DateTime);
	}

	public static string DurationToString(ulong duration)
	{
		return null;
	}

	public static string DurationToString(ulong duration, bool max0)
	{
		return null;
	}

	public static Dictionary<KeyType, ValueType> CloneDictionary<KeyType, ValueType>(Dictionary<KeyType, ValueType> source)
	{
		return null;
	}

	public static void CloneDictionaryInPlace<KeyType, ValueType>(Dictionary<KeyType, ValueType> source, Dictionary<KeyType, ValueType> dest)
	{
	}

	public static Dictionary<KeyType, ValueType> ConcatenateDictionaryInPlace<KeyType, ValueType>(Dictionary<KeyType, ValueType> dest, Dictionary<KeyType, ValueType> source)
	{
		return null;
	}

	public static List<To> CloneAndCastList<From, To>(List<From> list) where From : To
	{
		return null;
	}

	private static T AssertCast<T>(Dictionary<string, object> dict, string key)
	{
		return default(T);
	}

	public static Dictionary<string, object> DeserializeJsonFile(string filePath)
	{
		return null;
	}

	public static string ReadAllText(string filePath)
	{
		return null;
	}

	private static string LoadWWW(string filePath)
	{
		return null;
	}

	public static void AssertKeyExists(Dictionary<string, object> dict, string key)
	{
	}

	public static bool LoadBool(Dictionary<string, object> d, string key)
	{
		return false;
	}

	public static bool? LoadNullableBool(Dictionary<string, object> d, string key)
	{
		return null;
	}

	public static List<T> TryLoadList<T>(Dictionary<string, object> data, string key)
	{
		return null;
	}

	public static List<T> LoadList<T>(Dictionary<string, object> data, string key)
	{
		return null;
	}

	public static Dictionary<string, object> LoadDict(Dictionary<string, object> data, string key)
	{
		return null;
	}

	public static Dictionary<string, object> TryLoadDict(Dictionary<string, object> data, string key)
	{
		return null;
	}

	public static string LoadString(Dictionary<string, object> data, string key)
	{
		return null;
	}

	public static string LoadString(Dictionary<string, object> data, string key, string default_val)
	{
		return null;
	}

	public static string TryLoadString(Dictionary<string, object> data, string key)
	{
		return null;
	}

	public static string LoadNullableString(Dictionary<string, object> data, string key)
	{
		return null;
	}

	public static string TryLoadNullableString(Dictionary<string, object> data, string key)
	{
		return null;
	}

	public static int? LoadNullableInt(Dictionary<string, object> d, string key)
	{
		return null;
	}

	public static uint? LoadNullableUInt(Dictionary<string, object> d, string key)
	{
		return null;
	}

	public static ulong? LoadNullableUlong(Dictionary<string, object> d, string key)
	{
		return null;
	}

	public static int? TryLoadNullableInt(Dictionary<string, object> d, string key)
	{
		return null;
	}

	public static uint? TryLoadNullableUInt(Dictionary<string, object> d, string key)
	{
		return null;
	}

	public static ulong? TryLoadNullableUlong(Dictionary<string, object> d, string key)
	{
		return null;
	}

	public static object NullableToObject(ulong? nullable)
	{
		return null;
	}

	public static int? TryLoadInt(Dictionary<string, object> data, string key)
	{
		return null;
	}

	public static int LoadInt(Dictionary<string, object> data, string key, int default_val)
	{
		return 0;
	}

	public static long? TryLoadLong(Dictionary<string, object> data, string key)
	{
		return null;
	}

	public static bool LoadBoolAsInt(Dictionary<string, object> d, string key)
	{
		return false;
	}

	public static bool? TryLoadBool(Dictionary<string, object> data, string key)
	{
		return null;
	}

	public static bool LoadBool(Dictionary<string, object> data, string key, bool default_value)
	{
		return false;
	}

	public static bool? LoadBoolObjectHelper(object obj)
	{
		return null;
	}

	public static long LoadLong(Dictionary<string, object> d, string key)
	{
		return 0L;
	}

	public static int LoadInt(Dictionary<string, object> d, string key)
	{
		return 0;
	}

	private static int LoadIntHelper(Dictionary<string, object> d, string key)
	{
		return 0;
	}

	public static int LoadIntObjectHelper(object obj)
	{
		return 0;
	}

	public static long LoadLongObjectHelper(object obj)
	{
		return 0L;
	}

	private static long LoadLongHelper(Dictionary<string, object> d, string key)
	{
		return 0L;
	}

	public static uint LoadUint(Dictionary<string, object> data, string key)
	{
		return 0u;
	}

	public static uint? TryLoadUint(Dictionary<string, object> data, string key)
	{
		return null;
	}

	private static uint LoadUintHelper(Dictionary<string, object> data, string key)
	{
		return 0u;
	}

	public static ulong LoadUlong(Dictionary<string, object> data, string key, ulong defaultValue = 0uL)
	{
		return 0uL;
	}

	public static ulong? TryLoadUlong(Dictionary<string, object> data, string key, ulong defaultValue = 0uL)
	{
		return null;
	}

	private static ulong LoadUlongHelper(Dictionary<string, object> data, string key, ulong defaultValue)
	{
		return 0uL;
	}

	public static float? TryLoadFloat(Dictionary<string, object> data, string key)
	{
		return null;
	}

	public static float? LoadFloatObjectHelper(object obj)
	{
		return null;
	}

	public static float LoadFloat(Dictionary<string, object> d, string key)
	{
		return 0f;
	}

	public static double LoadDouble(Dictionary<string, object> d, string key)
	{
		return 0.0;
	}

	public static void LoadVector3(out Vector3 v3, Dictionary<string, object> d, float defaultValue)
	{
		v3 = default(Vector3);
	}

	public static void SaveVector3(Vector3 v3, string name, Dictionary<string, object> d)
	{
	}

	public static void LoadVector2(out Vector2 v2, Dictionary<string, object> d, float defaultValue)
	{
		v2 = default(Vector2);
	}

	public static void LoadVector3(out Vector3 v3, Dictionary<string, object> d)
	{
		v3 = default(Vector3);
	}

	public static void LoadVector2(out Vector2 v2, Dictionary<string, object> d)
	{
		v2 = default(Vector2);
	}

	public static Vector3 ExpandVector(Vector2 vector)
	{
		return default(Vector3);
	}

	public static Vector3 ExpandVector(Vector2 vector, float z)
	{
		return default(Vector3);
	}

	public static Vector2 TruncateVector(Vector3 vector)
	{
		return default(Vector2);
	}

	public static List<T> GetOrCreateList<T>(Dictionary<string, object> dict, string target)
	{
		return null;
	}

	public static void TruncateFile(string filePath)
	{
	}

	public static void DeleteFile(string filePath)
	{
	}

	public static void DeleteExistingGameData()
	{
	}

	public static string GetPersistentAssetsPath()
	{
		return null;
	}

	public static string GetStreamingAssetsPath()
	{
		return null;
	}

	public static string GetStreamingAssetsSubfolder(string path)
	{
		return null;
	}

	public static string GetStreamingAssetsFileInDirectory(string path, string filename)
	{
		return null;
	}

	public static void DeletePersistantFile(string fileName)
	{
	}

	public static string GetStreamingAssetsFile(string fileName)
	{
		return null;
	}

	public static string GetStreamingAssetsFile_IgnorePersistant(string fileName)
	{
		return null;
	}

	public static string[] GetFilesInPath(string path, string searchPattern)
	{
		return null;
	}

	public static void DebugDict(Dictionary<string, object> d)
	{
	}

	public static string DebugDictToString(Dictionary<string, object> d)
	{
		return null;
	}

	public static string DebugListToString(List<object> l)
	{
		return null;
	}

	public static string DebugListToString(List<Vector3> list)
	{
		return null;
	}

	public static string DebugListToString(List<Vector2> list)
	{
		return null;
	}

	private static string PrintDict(Dictionary<string, object> d, string lead)
	{
		return null;
	}

	private static string PrintList(List<object> l, string lead)
	{
		return null;
	}

	private static string PrintGenericValue(object v, string lead)
	{
		return null;
	}

	public static void SetLogType(string settings)
	{
	}

	public static void SetLogType(bool crashlytics, int logType)
	{
	}

	public static void DebugLog(object message, LogFilter filter)
	{
	}

	public static void DebugLog(object message)
	{
	}

	public static void DebugLogTimed(object message)
	{
	}

	public static void WarningLog(object message)
	{
	}

	public static void ErrorLog(object message)
	{
	}

	public static void LogFormat(string format, params object[] args)
	{
	}

	public static void UnexpectedEntry()
	{
	}

	public static void NotYetImplemented()
	{
	}

	public static void Assert(bool condition, string message)
	{
	}

	public static GameObject FindGameObjectInHierarchy(GameObject root, string name)
	{
		return null;
	}

	public static GameObject FindParentGameObjectInHierarchy(GameObject root, string name)
	{
		return null;
	}

	public static void PlayMovie(string movie)
	{
	}

	public static byte[] Zip(string str)
	{
		return null;
	}

	public static byte[] Zip(byte[] bytedata)
	{
		return null;
	}

	public static byte[] UnzipToBytes(byte[] input)
	{
		return null;
	}

	public static string Unzip(byte[] input)
	{
		return null;
	}

	public static int BoolToInt(bool myBool)
	{
		return 0;
	}

	public static int KontagentCurrencyLevelIndex(int kRange)
	{
		return 0;
	}

	public static string GetOSVersion()
	{
		return null;
	}

	public static string GetAndroidDeviceTypeString()
	{
		return null;
	}

	public static string GetDeviceLandscapeAspectRatio()
	{
		return null;
	}

	private static int triggerIosUiMessage(string sTitle, string sText, string sOK, string sId)
	{
		return 0;
	}

	public static void TriggerPurchaseWarning()
	{
	}

	public static void TriggerIAPDisabledWarning()
	{
	}

	public static void TriggerIAPOfflineWarning()
	{
	}

	public static string AssignStorePlatformText(string key)
	{
		return null;
	}

	public static void TriggerEULAPopup()
	{
	}

	private static int triggerIosUiError(string sTitle, string sText, string sOK)
	{
		return 0;
	}

	public static void TriggerIosUiError(string title, string text)
	{
	}

	public static void TriggerIosUiChoice(string title, string message, string button1, string button2, string option1, string option2, string callbackId)
	{
	}

	public static string GetDeviceId()
	{
		return null;
	}

	private static string DumpLogPath()
	{
		return null;
	}

	public static void LogDump(Session session, string tag, Exception ex = null, SoaringDictionary logDataDictionary = null)
	{
	}

	public static bool CheckForLogDumps(SoaringContextDelegate context_responder)
	{
		return false;
	}

	public static string GetConsoleOutput()
	{
		return null;
	}

	public static string GetLastSoaringDebugFile(string path)
	{
		return null;
	}

	public static string GetGameJsonFile(Player p)
	{
		return null;
	}

	public static string GetErrorLog()
	{
		return null;
	}

	public static void GotoAppstore()
	{
	}

	public static void SetDefaultHeaders(WebHeaderCollection wc)
	{
	}

	public static string GetPlayerName(SoaringPlayer player, string format = "{0}")
	{
		return null;
	}

	public static bool FileIsExists(string filePath)
	{
		return false;
	}

	private static bool LoadWWWExist(string filePath)
	{
		return false;
	}

	public static bool isAmazon()
	{
		return false;
	}

	public static string ParseGameDetails(Dictionary<string, object> gameData, ref GameDetails details)
	{
		return null;
	}

	private static string resourceValueByDid(int lookupDid, List<object> resources)
	{
		return null;
	}

	public static string GetEULA_Address()
	{
		return null;
	}

	public static string GetLegal_Address()
	{
		return null;
	}

	public static string GetPrivacy_Address()
	{
		return null;
	}

	public static void RefreshSAFiles()
	{
	}

	private static void UpdateSAFilePathRefs(string[] files, string directory)
	{
	}
}
