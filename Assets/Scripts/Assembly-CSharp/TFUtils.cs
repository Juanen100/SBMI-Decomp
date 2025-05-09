#define ASSERTS_ON
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using Ionic.Zlib;
using MTools;
using MiniJSON;
using UnityEngine;
using Debug = UnityEngine.Debug;

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
			if (success && error == null)
			{
				string path = DumpLogPath();
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			if (context.ContextResponder != null)
			{
				context.ContextResponder(context);
			}
		}
	}

	public class GameDetails
	{
		public int lastPlayTime;

		public string dtLastPlayTime = string.Empty;

		public string money = string.Empty;

		public string jelly = string.Empty;

		public string patties = string.Empty;

		public string level = string.Empty;
	}

	public const int kMaxSaveLogLength = 131072;

	private const float MESSAGE_TIME = 1f;

	private const int LOG_TYPE_NONE = -1;

	private const int LOG_TYPE_ERROR = 0;

	private const int LOG_TYPE_WARNING = 1;

	private const int LOG_TYPE_STANDARD = 2;

	public static DateTime EPOCH = new DateTime(1970, 1, 1).ToUniversalTime();

	public static ulong APP_START_TIME = EpochTime();

	public static LogLevel LOG_LEVEL = LogLevel.INFO;

	public static StringBuilder PrevLog;

	public static StringBuilder ConsoleLog;

	public static StringBuilder ErrorConsoleLog;

	public static LogFilter LOG_FILTER = LogFilter.None;

	public static string ApplicationDataPath;

	public static string ApplicationPersistentDataPath;

	public static string DeviceId;

	public static string DeviceName;

	public static ulong DebugTimeOffset = 0uL;

	private static object lastTimedMessage;

	private static float lastTimedMessageTime;

	public static bool isFastForwarding = false;

	private static float timeMultiplier = 50f;

	public static ulong FastForwardOffset = 0uL;

	public static ulong AddTimeOffset = 0uL;

	private static DateTime seedUtcNow = new DateTime(1970, 1, 1);

	public static string playerID;

	public static string playerAlias;

	public static float TimeMultiplier
	{
		get
		{
			return timeMultiplier;
		}
		set
		{
			timeMultiplier = value;
		}
	}

	public static DateTime UtcNow
	{
		get
		{
			DateTime utcNow = DateTime.UtcNow;
			if (seedUtcNow.Equals(new DateTime(1970, 1, 1)))
			{
				seedUtcNow = utcNow;
			}
			TimeSpan value = TimeSpan.FromTicks((long)((float)utcNow.Subtract(seedUtcNow).Ticks * timeMultiplier));
			return seedUtcNow.Add(value);
		}
		set
		{
			seedUtcNow = value;
		}
	}

	public static void Init()
	{
		ConsoleLog = new StringBuilder(string.Empty);
		PrevLog = null;
		ErrorConsoleLog = new StringBuilder(string.Empty);
		ApplicationDataPath = Application.dataPath;
		ApplicationPersistentDataPath = Application.persistentDataPath;
		DeviceId = GetDeviceId();
		DeviceName = SystemInfo.deviceName;
		DebugLog("This device is:" + DeviceId);
	}

	private static void WriteConsoleLog(string tx)
	{
		if (ConsoleLog.Length >= 131072)
		{
			PrevLog = ConsoleLog;
			ConsoleLog = new StringBuilder(string.Empty);
		}
		ConsoleLog.Append(tx);
	}

	public static ulong EpochTime()
	{
		if (isFastForwarding)
		{
			return EpochTime(UtcNow);
		}
		return EpochTime(DateTime.UtcNow);
	}

	public static ulong EpochTime(DateTime dt)
	{
		TimeSpan timeSpan = dt - EPOCH;
		if (SBSettings.SoaringProduction)
		{
			return (ulong)timeSpan.TotalSeconds;
		}
		return (ulong)timeSpan.TotalSeconds + FastForwardOffset + AddTimeOffset;
	}

	public static DateTime EpochToDateTime(ulong seconds)
	{
		return DateTime.SpecifyKind(EPOCH.AddSeconds(seconds), DateTimeKind.Utc);
	}

	public static string DurationToString(ulong duration)
	{
		return DurationToString(duration, true);
	}

	public static string DurationToString(ulong duration, bool max0)
	{
		if (max0)
		{
			duration = Math.Max(duration, 0uL);
		}
		if (duration < 60)
		{
			return string.Format("{0}s", duration);
		}
		ulong num = duration % 60;
		duration -= num;
		ulong num2 = duration / 60;
		if (num2 < 60)
		{
			if (num == 0L)
			{
				return string.Format("{0}m", num2);
			}
			return string.Format("{0}m {1}s", num2, num);
		}
		ulong num3 = num2 / 60;
		num2 %= 60;
		if (num3 < 24)
		{
			if (num2 == 0L)
			{
				return string.Format("{0}h", num3);
			}
			return string.Format("{0}h {1}m", num3, num2);
		}
		ulong num4 = num3 / 24;
		num3 %= 24;
		if (num3 == 0L)
		{
			return string.Format("{0}d", num4);
		}
		return string.Format("{0}d {1}h", num4, num3);
	}

	public static Dictionary<KeyType, ValueType> CloneDictionary<KeyType, ValueType>(Dictionary<KeyType, ValueType> source)
	{
		Dictionary<KeyType, ValueType> dictionary = new Dictionary<KeyType, ValueType>();
		foreach (KeyType key in source.Keys)
		{
			dictionary[key] = source[key];
		}
		return dictionary;
	}

	public static void CloneDictionaryInPlace<KeyType, ValueType>(Dictionary<KeyType, ValueType> source, Dictionary<KeyType, ValueType> dest)
	{
		dest.Clear();
		foreach (KeyValuePair<KeyType, ValueType> item in source)
		{
			dest.Add(item.Key, item.Value);
		}
	}

	public static Dictionary<KeyType, ValueType> ConcatenateDictionaryInPlace<KeyType, ValueType>(Dictionary<KeyType, ValueType> dest, Dictionary<KeyType, ValueType> source)
	{
		foreach (KeyType key in source.Keys)
		{
			if (dest.ContainsKey(key))
			{
				throw new ArgumentException("Destination dictionary already contains key " + key.ToString());
			}
			dest[key] = source[key];
		}
		return dest;
	}

	public static List<To> CloneAndCastList<From, To>(List<From> list) where From : To
	{
		List<To> list2 = new List<To>(list.Count);
		foreach (From item in list)
		{
			list2.Add((To)(object)item);
		}
		return list2;
	}

	private static T AssertCast<T>(Dictionary<string, object> dict, string key)
	{
		AssertKeyExists(dict, key);
		Assert(dict[key] is T, string.Format("Could not cast the key({0}) with value({1}) to type({2}) in dictionary{3}", key, dict[key], typeof(T).ToString(), DebugDictToString(dict)));
		return (T)dict[key];
	}

	public static Dictionary<string, object> DeserializeJsonFile(string filePath)
	{
		string json = ReadAllText(filePath);
		return (Dictionary<string, object>)Json.Deserialize(json);
	}

	public static string ReadAllText(string filePath)
	{
		string result = string.Empty;
		try
		{
			DebugLog("filePath " + filePath);
			if (filePath.Contains("://"))
			{
				result = LoadWWW(filePath);
			}
			else
			{
				using (StreamReader streamReader = new StreamReader(filePath))
				{
					result = streamReader.ReadToEnd();
				}
			}
		}
		catch (Exception ex)
		{
			WarningLog(ex.Message);
		}
		return result;
	}

	private static string LoadWWW(string filePath)
	{
		DebugLog("------LoadWWW1-----------");
		WWW wWW = new WWW(filePath);
		DebugLog("------LoadWWW2-----------");
		while (!wWW.isDone)
		{
		}
		return wWW.text;
	}

	public static void AssertKeyExists(Dictionary<string, object> dict, string key)
	{
		if (dict == null)
		{
			Assert(false, string.Format("Can't search for the key '{0}' in a null dictionary", key));
		}
		if (!dict.ContainsKey(key))
		{
			Assert(false, string.Format("Could not find the key '{0}' in the given dictionary:\n{1}", key, DebugDictToString(dict)));
		}
	}

	public static bool LoadBool(Dictionary<string, object> d, string key)
	{
		if (SBSettings.AssertDataValidity)
		{
			AssertCast<bool>(d, key);
		}
		return (bool)d[key];
	}

	public static bool? LoadNullableBool(Dictionary<string, object> d, string key)
	{
		if (SBSettings.AssertDataValidity)
		{
			AssertKeyExists(d, key);
		}
		return (bool?)d[key];
	}

	public static List<T> TryLoadList<T>(Dictionary<string, object> data, string key)
	{
		if (!data.ContainsKey(key))
		{
			return null;
		}
		return LoadList<T>(data, key);
	}

	public static List<T> LoadList<T>(Dictionary<string, object> data, string key)
	{
		if (SBSettings.AssertDataValidity)
		{
			AssertKeyExists(data, key);
		}
		if (data[key] is List<T>)
		{
			return (List<T>)data[key];
		}
		List<object> list = (List<object>)data[key];
		List<T> retval = new List<T>(data.Count);
		list.ForEach(delegate(object obj)
		{
			retval.Add((T)Convert.ChangeType(obj, typeof(T)));
		});
		return retval;
	}

	public static Dictionary<string, object> LoadDict(Dictionary<string, object> data, string key)
	{
		if (SBSettings.AssertDataValidity)
		{
			AssertKeyExists(data, key);
		}
		return (Dictionary<string, object>)data[key];
	}

	public static Dictionary<string, object> TryLoadDict(Dictionary<string, object> data, string key)
	{
		if (!data.ContainsKey(key))
		{
			return null;
		}
		return (Dictionary<string, object>)data[key];
	}

	public static string LoadString(Dictionary<string, object> data, string key)
	{
		if (SBSettings.AssertDataValidity)
		{
			AssertCast<string>(data, key);
		}
		return (string)data[key];
	}

	public static string LoadString(Dictionary<string, object> data, string key, string default_val)
	{
		string text = TryLoadString(data, key);
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		return default_val;
	}

	public static string TryLoadString(Dictionary<string, object> data, string key)
	{
		if (data.ContainsKey(key))
		{
			if (SBSettings.AssertDataValidity)
			{
				return AssertCast<string>(data, key);
			}
			return (string)data[key];
		}
		return null;
	}

	public static string LoadNullableString(Dictionary<string, object> data, string key)
	{
		if (SBSettings.AssertDataValidity)
		{
			AssertKeyExists(data, key);
		}
		return (string)data[key];
	}

	public static string TryLoadNullableString(Dictionary<string, object> data, string key)
	{
		if (data.ContainsKey(key))
		{
			return (string)data[key];
		}
		return null;
	}

	public static int? LoadNullableInt(Dictionary<string, object> d, string key)
	{
		if (SBSettings.AssertDataValidity)
		{
			AssertKeyExists(d, key);
		}
		object obj = d[key];
		if (obj == null)
		{
			return (int?)obj;
		}
		return LoadInt(d, key);
	}

	public static uint? LoadNullableUInt(Dictionary<string, object> d, string key)
	{
		if (SBSettings.AssertDataValidity)
		{
			AssertKeyExists(d, key);
		}
		object obj = d[key];
		if (obj == null)
		{
			return (uint?)obj;
		}
		return LoadUint(d, key);
	}

	public static ulong? LoadNullableUlong(Dictionary<string, object> d, string key)
	{
		if (SBSettings.AssertDataValidity)
		{
			AssertKeyExists(d, key);
		}
		object obj = d[key];
		if (obj == null)
		{
			return (ulong?)obj;
		}
		return LoadUlong(d, key);
	}

	public static int? TryLoadNullableInt(Dictionary<string, object> d, string key)
	{
		if (d.ContainsKey(key))
		{
			return LoadNullableInt(d, key);
		}
		return null;
	}

	public static uint? TryLoadNullableUInt(Dictionary<string, object> d, string key)
	{
		if (d.ContainsKey(key))
		{
			return LoadNullableUInt(d, key);
		}
		return null;
	}

	public static ulong? TryLoadNullableUlong(Dictionary<string, object> d, string key)
	{
		if (d.ContainsKey(key))
		{
			return LoadNullableUlong(d, key);
		}
		return null;
	}

	public static object NullableToObject(ulong? nullable)
	{
		if (nullable.HasValue)
		{
			return nullable.Value;
		}
		return null;
	}

	public static int? TryLoadInt(Dictionary<string, object> data, string key)
	{
		if (data.ContainsKey(key))
		{
			return LoadIntHelper(data, key);
		}
		return null;
	}

	public static int LoadInt(Dictionary<string, object> data, string key, int default_val)
	{
		int? num = TryLoadInt(data, key);
		if (num.HasValue)
		{
			return num.Value;
		}
		return default_val;
	}

	public static long? TryLoadLong(Dictionary<string, object> data, string key)
	{
		if (data.ContainsKey(key))
		{
			return LoadLongHelper(data, key);
		}
		return null;
	}

	public static bool LoadBoolAsInt(Dictionary<string, object> d, string key)
	{
		return (LoadInt(d, key) != 0) ? true : false;
	}

	public static bool? TryLoadBool(Dictionary<string, object> data, string key)
	{
		if (data.ContainsKey(key))
		{
			if (SBSettings.AssertDataValidity)
			{
				return AssertCast<bool>(data, key);
			}
			return (bool)data[key];
		}
		return null;
	}

	public static bool LoadBool(Dictionary<string, object> data, string key, bool default_value)
	{
		bool? flag = TryLoadBool(data, key);
		if (flag.HasValue)
		{
			return flag.Value;
		}
		return default_value;
	}

	public static bool? LoadBoolObjectHelper(object obj)
	{
		if (obj != null)
		{
			return (bool)obj;
		}
		return null;
	}

	public static long LoadLong(Dictionary<string, object> d, string key)
	{
		if (SBSettings.AssertDataValidity)
		{
			AssertKeyExists(d, key);
		}
		return Convert.ToInt64(d[key]);
	}

	public static int LoadInt(Dictionary<string, object> d, string key)
	{
		if (SBSettings.AssertDataValidity)
		{
			AssertKeyExists(d, key);
		}
		return LoadIntHelper(d, key);
	}

	private static int LoadIntHelper(Dictionary<string, object> d, string key)
	{
		return Convert.ToInt32(d[key]);
	}

	public static int LoadIntObjectHelper(object obj)
	{
		return Convert.ToInt32(obj);
	}

	public static long LoadLongObjectHelper(object obj)
	{
		return Convert.ToInt64(obj);
	}

	private static long LoadLongHelper(Dictionary<string, object> d, string key)
	{
		return Convert.ToInt64(d[key]);
	}

	public static uint LoadUint(Dictionary<string, object> data, string key)
	{
		if (SBSettings.AssertDataValidity)
		{
			AssertKeyExists(data, key);
		}
		return LoadUintHelper(data, key);
	}

	public static uint? TryLoadUint(Dictionary<string, object> data, string key)
	{
		if (!data.ContainsKey(key))
		{
			return null;
		}
		return LoadUintHelper(data, key);
	}

	private static uint LoadUintHelper(Dictionary<string, object> data, string key)
	{
		return Convert.ToUInt32(data[key]);
	}

	public static ulong LoadUlong(Dictionary<string, object> data, string key, ulong defaultValue = 0)
	{
		if (SBSettings.AssertDataValidity)
		{
			AssertKeyExists(data, key);
		}
		return LoadUlongHelper(data, key, defaultValue);
	}

	public static ulong? TryLoadUlong(Dictionary<string, object> data, string key, ulong defaultValue = 0)
	{
		if (!data.ContainsKey(key))
		{
			return null;
		}
		return LoadUlongHelper(data, key, defaultValue);
	}

	private static ulong LoadUlongHelper(Dictionary<string, object> data, string key, ulong defaultValue)
	{
		try
		{
			return Convert.ToUInt64(data[key]);
		}
		catch (Exception message)
		{
			Debug.LogWarning(message);
			Debug.LogError(string.Format("Could not convert value to Uint64!\nKey={0}, Value={1}, Dictionary={2}", key, data[key], DebugDictToString(data)));
			return defaultValue;
		}
	}

	public static float? TryLoadFloat(Dictionary<string, object> data, string key)
	{
		if (data.ContainsKey(key))
		{
			if (SBSettings.AssertDataValidity)
			{
				return (float)AssertCast<double>(data, key);
			}
			return Convert.ToSingle(data[key]);
		}
		return null;
	}

	public static float? LoadFloatObjectHelper(object obj)
	{
		if (obj != null)
		{
			return Convert.ToSingle(obj);
		}
		return null;
	}

	public static float LoadFloat(Dictionary<string, object> d, string key)
	{
		if (SBSettings.AssertDataValidity)
		{
			AssertKeyExists(d, key);
		}
		return Convert.ToSingle(d[key]);
	}

	public static double LoadDouble(Dictionary<string, object> d, string key)
	{
		if (SBSettings.AssertDataValidity)
		{
			AssertKeyExists(d, key);
		}
		return Convert.ToDouble(d[key]);
	}

	public static void LoadVector3(out Vector3 v3, Dictionary<string, object> d, float defaultValue)
	{
		v3.x = ((!d.ContainsKey("x")) ? defaultValue : LoadFloat(d, "x"));
		v3.y = ((!d.ContainsKey("y")) ? defaultValue : LoadFloat(d, "y"));
		v3.z = ((!d.ContainsKey("z")) ? defaultValue : LoadFloat(d, "z"));
	}

	public static void SaveVector3(Vector3 v3, string name, Dictionary<string, object> d)
	{
		d[name] = new Dictionary<string, object>
		{
			{ "x", v3.x },
			{ "y", v3.y },
			{ "z", v3.z }
		};
	}

	public static void LoadVector2(out Vector2 v2, Dictionary<string, object> d, float defaultValue)
	{
		if (SBSettings.AssertDataValidity)
		{
			Assert(!d.ContainsKey("z"), "Don't call LoadVector2 on something that has a z value! (do you want to use LoadVector3?)");
		}
		v2.x = ((!d.ContainsKey("x")) ? defaultValue : LoadFloat(d, "x"));
		v2.y = ((!d.ContainsKey("y")) ? defaultValue : LoadFloat(d, "y"));
	}

	public static void LoadVector3(out Vector3 v3, Dictionary<string, object> d)
	{
		LoadVector3(out v3, d, 0f);
	}

	public static void LoadVector2(out Vector2 v2, Dictionary<string, object> d)
	{
		LoadVector2(out v2, d, 0f);
	}

	public static Vector3 ExpandVector(Vector2 vector)
	{
		return ExpandVector(vector, 0f);
	}

	public static Vector3 ExpandVector(Vector2 vector, float z)
	{
		return new Vector3(vector.x, vector.y, z);
	}

	public static Vector2 TruncateVector(Vector3 vector)
	{
		return new Vector2(vector.x, vector.y);
	}

	public static List<T> GetOrCreateList<T>(Dictionary<string, object> dict, string target)
	{
		if (dict.ContainsKey(target))
		{
			if (SBSettings.AssertDataValidity)
			{
				Assert(dict[target] is List<T>, string.Format("Found data at '{0}' but it is not a List<{1}>", target, typeof(T).ToString()));
			}
			if (dict[target] is List<T>)
			{
				return (List<T>)dict[target];
			}
			return new List<T>();
		}
		List<T> list = new List<T>();
		dict.Add(target, list);
		return list;
	}

	public static void TruncateFile(string filePath)
	{
		DeleteFile(filePath);
		using (FileStream fileStream = File.Create(filePath))
		{
			fileStream.Close();
		}
	}

	public static void DeleteFile(string filePath)
	{
		if (FileIsExists(filePath))
		{
			File.Delete(filePath);
		}
	}

	public static void DeleteExistingGameData()
	{
		if (Directory.Exists(GetPersistentAssetsPath()))
		{
			Directory.Delete(GetPersistentAssetsPath(), true);
		}
	}

	public static string GetPersistentAssetsPath()
	{
		return Path.Combine(ApplicationPersistentDataPath, "Contents");
	}

	public static string GetStreamingAssetsPath()
	{
		return Application.persistentDataPath + "/Contents";
		//return "jar:file://" + ApplicationDataPath + Path.DirectorySeparatorChar + "!/assets";
	}

	public static string GetStreamingAssetsSubfolder(string path)
	{
		return GetStreamingAssetsPath() + Path.DirectorySeparatorChar + path;
	}

	public static string GetStreamingAssetsFileInDirectory(string path, string filename)
	{
		return GetStreamingAssetsFile(path + Path.DirectorySeparatorChar + filename);
	}

	public static void DeletePersistantFile(string fileName)
	{
		try
		{
			string text = GetPersistentAssetsPath() + Path.DirectorySeparatorChar + fileName;
			if (FileIsExists(text))
			{
				File.Delete(text);
			}
		}
		catch
		{
		}
	}

	public static string GetStreamingAssetsFile(string fileName)
	{
		string text = GetPersistentAssetsPath() + Path.DirectorySeparatorChar + fileName;
		if (FileIsExists(text))
		{
			return text;
		}
		return GetStreamingAssetsPath() + Path.DirectorySeparatorChar + fileName;
	}

	public static string GetStreamingAssetsFile_IgnorePersistant(string fileName)
	{
		return GetStreamingAssetsPath() + Path.DirectorySeparatorChar + fileName;
	}

	public static string[] GetFilesInPath(string path, string searchPattern)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		string streamingAssetsSubfolder = GetStreamingAssetsSubfolder(path);
		string streamingAssetsPath = GetStreamingAssetsPath();
		string[] files = Directory.GetFiles(streamingAssetsSubfolder, searchPattern, SearchOption.AllDirectories);
		foreach (string text in files)
		{
			string key = text.Substring(streamingAssetsPath.Length);
			dictionary[key] = text;
		}
		string persistentAssetsPath = GetPersistentAssetsPath();
		string path2 = GetPersistentAssetsPath() + Path.DirectorySeparatorChar + path;
		if (Directory.Exists(path2))
		{
			string[] files2 = Directory.GetFiles(path2, searchPattern, SearchOption.AllDirectories);
			foreach (string text2 in files2)
			{
				string key2 = text2.Substring(persistentAssetsPath.Length);
				dictionary[key2] = text2;
			}
		}
		string[] array = new string[dictionary.Count];
		dictionary.Values.CopyTo(array, 0);
		return array;
	}

	[Conditional("DEBUG")]
	public static void DebugDict(Dictionary<string, object> d)
	{
		DebugLog(DebugDictToString(d));
	}

	public static string DebugDictToString(Dictionary<string, object> d)
	{
		return "[Dictionary Debug View]\n" + PrintDict(d, string.Empty);
	}

	public static string DebugListToString(List<object> l)
	{
		return "[List Debug View]\n" + PrintList(l, string.Empty);
	}

	public static string DebugListToString(List<Vector3> list)
	{
		return DebugListToString(list.ConvertAll((Converter<Vector3, object>)((Vector3 v) => "\t(" + v.x + ",\t" + v.y + ",\t" + v.z + ")")));
	}

	public static string DebugListToString(List<Vector2> list)
	{
		return DebugListToString(list.ConvertAll((Vector2 v) => ExpandVector(v)));
	}

	private static string PrintDict(Dictionary<string, object> d, string lead)
	{
		if (d == null)
		{
			return "null";
		}
		string text = "{\n";
		foreach (string key in d.Keys)
		{
			if (d[key] != null)
			{
				string text2 = text;
				text = text2 + lead + key + ":" + PrintGenericValue(d[key], lead + " ") + ",\n";
			}
			else
			{
				string text2 = text;
				text = string.Concat(text2, lead, key, ":", d[key], ",\n");
			}
		}
		return text + lead + "}";
	}

	private static string PrintList(List<object> l, string lead)
	{
		if (l == null)
		{
			return "null";
		}
		string text = "[\n";
		for (int i = 0; i < l.Count; i++)
		{
			string text2 = text;
			text = text2 + lead + i + ":" + PrintGenericValue(l[i], lead + " ") + ",\n";
		}
		return text + lead + "]";
	}

	private static string PrintGenericValue(object v, string lead)
	{
		if (v is Dictionary<string, object>)
		{
			return PrintDict(v as Dictionary<string, object>, lead + " ");
		}
		if (v is Dictionary<int, int>)
		{
			string text = "{ ";
			foreach (KeyValuePair<int, int> item in (Dictionary<int, int>)v)
			{
				string text2 = text;
				text = text2 + "\n " + item.Key + ": " + item.Value + ",";
			}
			text = text.Substring(0, text.Length - 1);
			return text + "\n }";
		}
		if (v is List<object>)
		{
			return PrintList(v as List<object>, lead + " ");
		}
		if (v == null)
		{
			return "null\n";
		}
		return v.ToString();
	}

	public static void SetLogType(string settings)
	{
		if (!string.IsNullOrEmpty(settings))
		{
			settings = settings.ToLower();
			bool crashlytics = settings.Contains("crash");
			int logType = 2;
			if (settings.Contains("none"))
			{
				logType = -1;
			}
			else if (settings.Contains("error"))
			{
				logType = 0;
			}
			else if (settings.Contains("warning"))
			{
				logType = 1;
			}
			SetLogType(crashlytics, logType);
		}
	}

	public static void SetLogType(bool crashlytics, int logType)
	{
	}

	public static void DebugLog(object message, LogFilter filter)
	{
		if ((filter & LOG_FILTER) == filter || message == null)
		{
			return;
		}
		if (SBSettings.ConsoleLoggingEnabled)
		{
			Debug.Log(message);
		}
		if (LOG_LEVEL == LogLevel.INFO && SBSettings.ConsoleLoggingEnabled)
		{
			string text = message.ToString();
			if (text != null && ConsoleLog != null)
			{
				WriteConsoleLog(text);
			}
		}
	}

	public static void DebugLog(object message)
	{
		if (message == null)
		{
			return;
		}
		if (SBSettings.ConsoleLoggingEnabled)
		{
			Debug.Log(message);
		}
		if (LOG_LEVEL == LogLevel.INFO)
		{
			string text = message.ToString();
			if (text != null && ConsoleLog != null)
			{
				WriteConsoleLog(text);
			}
		}
	}

	public static void DebugLogTimed(object message)
	{
		if (lastTimedMessage == null || lastTimedMessage != message || Time.realtimeSinceStartup - lastTimedMessageTime > 1f)
		{
			DebugLog(message);
			lastTimedMessage = message;
			lastTimedMessageTime = Time.realtimeSinceStartup;
		}
	}

	public static void WarningLog(object message)
	{
		Debug.LogWarning(message);
		if (LOG_LEVEL <= LogLevel.WARN)
		{
			string text = message.ToString();
			if (text != null && ConsoleLog != null)
			{
				WriteConsoleLog(text);
			}
		}
	}

	public static void ErrorLog(object message)
	{
		Debug.LogError(message);
		string text = message.ToString();
		if (LOG_LEVEL <= LogLevel.ERROR && text != null && ConsoleLog != null)
		{
			WriteConsoleLog(text);
		}
		if (text != null && ErrorConsoleLog != null)
		{
			ErrorConsoleLog.AppendLine(text);
		}
	}

	[Conditional("DEBUG")]
	public static void LogFormat(string format, params object[] args)
	{
		string text = string.Format(format, args);
		Debug.Log(text);
		if (LOG_LEVEL == LogLevel.INFO && ConsoleLog != null)
		{
			WriteConsoleLog(text);
		}
	}

	[Conditional("DEBUG")]
	public static void UnexpectedEntry()
	{
		throw new Exception("Unexpected path of code execution! You should not be here!");
	}

	[Conditional("DEBUG")]
	public static void NotYetImplemented()
	{
		throw new Exception("This function is not yet implemented.");
	}

	[Conditional("ASSERTS_ON")]
	public static void Assert(bool condition, string message)
	{
		if (!condition)
		{
			Exception ex = new Exception(message);
			if (SBSettings.DumpLogOnAssert)
			{
				LogDump(null, "Assert Error", ex);
			}
			throw ex;
		}
	}

	public static GameObject FindGameObjectInHierarchy(GameObject root, string name)
	{
		if (root.name.Equals(name))
		{
			return root;
		}
		GameObject gameObject = null;
		int childCount = root.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			gameObject = FindGameObjectInHierarchy(root.transform.GetChild(i).gameObject, name);
			if (gameObject != null)
			{
				break;
			}
		}
		return gameObject;
	}

	public static GameObject FindParentGameObjectInHierarchy(GameObject root, string name)
	{
		Transform transform = root.transform;
		while (transform.parent != null)
		{
			if (transform.gameObject.name.Equals(name))
			{
				return transform.gameObject;
			}
			transform = transform.parent;
		}
		return null;
	}

	public static void PlayMovie(string movie)
	{
		Handheld.PlayFullScreenMovie(movie, Color.black, FullScreenMovieControlMode.CancelOnInput);
	}

	public static byte[] Zip(string str)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(str);
		return Zip(bytes);
	}

	public static byte[] Zip(byte[] bytedata)
	{
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
			{
				gZipStream.Write(bytedata, 0, bytedata.Length);
				gZipStream.Close();
			}
			return memoryStream.ToArray();
		}
	}

	public static byte[] UnzipToBytes(byte[] input)
	{
		MemoryStream stream = new MemoryStream(input);
		MemoryStream memoryStream = new MemoryStream();
		using (GZipStream gZipStream = new GZipStream(stream, CompressionMode.Decompress))
		{
			byte[] array = new byte[1024];
			int num = 0;
			while ((num = gZipStream.Read(array, 0, array.Length)) > 0)
			{
				memoryStream.Write(array, 0, num);
			}
		}
		return memoryStream.ToArray();
	}

	public static string Unzip(byte[] input)
	{
		return Encoding.UTF8.GetString(UnzipToBytes(input));
	}

	public static int BoolToInt(bool myBool)
	{
		if (myBool)
		{
			return 1;
		}
		return 0;
	}

	public static int KontagentCurrencyLevelIndex(int kRange)
	{
		if (kRange > 0 && kRange < 10)
		{
			return 1;
		}
		if (kRange > 10 && kRange < 100)
		{
			return 2;
		}
		if (kRange > 100 && kRange < 1000)
		{
			return 3;
		}
		if (kRange > 1000 && kRange < 10000)
		{
			return 4;
		}
		if (kRange > 10000 && kRange < 100000)
		{
			return 5;
		}
		if (kRange > 100000)
		{
			return 6;
		}
		return 0;
	}

	public static string GetOSVersion()
	{
		return SystemInfo.operatingSystem;
	}

	public static string GetAndroidDeviceTypeString()
	{
		return SystemInfo.deviceModel;
	}

	public static string GetDeviceLandscapeAspectRatio()
	{
		return Screen.width + ":" + Screen.height;
	}

	private static int triggerIosUiMessage(string sTitle, string sText, string sOK, string sId)
	{
		EtceteraAndroid.showAlert(sTitle, sText, sOK);
		return 0;
	}

	public static void TriggerPurchaseWarning()
	{
		string sText = AssignStorePlatformText("!!PURCHASE_WARNING_TEXT");
		string sTitle = Language.Get("!!PURCHASE_WARNING_TITLE");
		string sOK = Language.Get("!!PURCHASE_WARNING_OK");
		triggerIosUiMessage(sTitle, sText, sOK, "InApp");
	}

	public static void TriggerIAPDisabledWarning()
	{
	}

	public static void TriggerIAPOfflineWarning()
	{
		string sText = AssignStorePlatformText("!!IAP_OFFLINE_TEXT");
		string sTitle = Language.Get("!!IAP_OFFLINE_TITLE");
		string sOK = Language.Get("!!PURCHASE_WARNING_OK");
		triggerIosUiMessage(sTitle, sText, sOK, "InApp");
	}

	public static string AssignStorePlatformText(string key)
	{
		if (string.IsNullOrEmpty(key))
		{
			return key;
		}
		string text = Language.Get(key);
		if (text == null)
		{
			return text;
		}
		if (text.Contains("{0}"))
		{
			string text2 = "Unknown";
			text2 = ((SoaringInternal.PlatformType != SoaringPlatformType.Amazon) ? Language.Get("!!PLATFORM_GOOGLE") : Language.Get("!!PLATFORM_AMAZON"));
			text = string.Format(text, text2);
		}
		return text;
	}

	public static void TriggerEULAPopup()
	{
		string sText = Language.Get("!!PRIVACY_POLICY_POPUP_TEXT");
		string sTitle = Language.Get("!!PRIVACY_POLICY_POPUP_TITLE");
		string sOK = Language.Get("!!PURCHASE_WARNING_OK");
		triggerIosUiMessage(sTitle, sText, sOK, "PrivacyPolicy");
	}

	private static int triggerIosUiError(string sTitle, string sText, string sOK)
	{
		EtceteraAndroid.showAlert(sTitle, sText, sOK);
		return 0;
	}

	public static void TriggerIosUiError(string title, string text)
	{
		string sOK = Language.Get("!!PREFAB_OK");
		triggerIosUiError(title, text, sOK);
	}

	public static void TriggerIosUiChoice(string title, string message, string button1, string button2, string option1, string option2, string callbackId)
	{
	}

	public static string GetDeviceId()
	{
		return SystemInfo.deviceUniqueIdentifier;
	}

	private static string DumpLogPath()
	{
		return ResourceUtils.GetWritePath("LogDump.dmp", "LogDump", 1);
	}

	public static void LogDump(Session session, string tag, Exception ex = null, SoaringDictionary logDataDictionary = null)
	{
		try
		{
			if (ex != null)
			{
				WarningLog("Exception: " + ex.Message);
				WarningLog("Stack Trace: " + ex.StackTrace);
			}
			if (!Soaring.IsInitialized)
			{
				ErrorLog("Failed to upload log dump to server because soaring isnt initialized");
				return;
			}
			SoaringDictionary soaringDictionary = null;
			soaringDictionary = ((logDataDictionary == null) ? new SoaringDictionary(4) : logDataDictionary);
			if (ex != null)
			{
				soaringDictionary.addValue(Convert.ToBase64String(Encoding.UTF8.GetBytes(ex.Message)), "exception");
				soaringDictionary.addValue(Convert.ToBase64String(Encoding.UTF8.GetBytes(ex.StackTrace)), "stack_trace");
				IDictionary dictionary = ex.Data;
				if (dictionary.Count == 0)
				{
					dictionary = null;
					if (ex.InnerException != null)
					{
						dictionary = ex.InnerException.Data;
						if (dictionary.Count == 0)
						{
							dictionary = null;
						}
					}
				}
				if (dictionary != null)
				{
					SoaringDictionary soaringDictionary2 = new SoaringDictionary(2);
					foreach (DictionaryEntry item in dictionary)
					{
						soaringDictionary2.addValue(item.Value.ToString(), item.Key.ToString());
					}
					soaringDictionary.addValue(soaringDictionary2, "exception_keys");
				}
			}
			soaringDictionary.addValue(GetConsoleOutput(), "console_log");
			if (session != null)
			{
				string gameJsonFile = GetGameJsonFile(session.ThePlayer);
				if (gameJsonFile != null)
				{
					soaringDictionary.addValue(gameJsonFile, "game_json");
				}
			}
			soaringDictionary.addValue(GetErrorLog(), "error_log");
			soaringDictionary.addValue(Soaring.Player.UserTag, "session_username");
			if (!string.IsNullOrEmpty(tag))
			{
				soaringDictionary.addValue(tag, "error_tag");
			}
			soaringDictionary.addValue(EpochTime() - APP_START_TIME, "time_since_start");
			soaringDictionary.addValue(GetAndroidDeviceTypeString(), "device_type");
			soaringDictionary.addValue(GetOSVersion(), "os_version");
			soaringDictionary.addValue(SBSettings.BundleVersion, "bundle_version");
			if (SoaringInternal.IsProductionMode && !string.IsNullOrEmpty(SoaringDebug.DebugFileName))
			{
				string empty = string.Empty;
				string writePath = ResourceUtils.GetWritePath(SoaringDebug.DebugFileName, empty + "Soaring/Logs", 8);
				if (!string.IsNullOrEmpty(writePath))
				{
					string lastSoaringDebugFile = GetLastSoaringDebugFile(writePath);
					if (lastSoaringDebugFile != null)
					{
						soaringDictionary.addValue(lastSoaringDebugFile, "soaring_last_log");
					}
				}
			}
			if (logDataDictionary != null)
			{
				return;
			}
			try
			{
				SoaringFileTools.WriteJsonToFile(DumpLogPath(), soaringDictionary);
				SoaringContext soaringContext = new SoaringContext();
				soaringContext.Responder = new SendLogDumpDelegate();
				soaringContext.addValue(true, "trIOff");
				Soaring.SendSessionData(soaringDictionary, soaringContext);
			}
			catch (Exception ex2)
			{
				ErrorLog("Exception: " + ex2.Message);
				ErrorLog("Failed to upload log dump to server due to Exception when sending.");
			}
		}
		catch (Exception ex3)
		{
			ErrorLog("Exception: " + ex3.Message);
		}
	}

	public static bool CheckForLogDumps(SoaringContextDelegate context_responder)
	{
		bool result = false;
		try
		{
			string text = DumpLogPath();
			if (File.Exists(text))
			{
				MBinaryReader mBinaryReader = new MBinaryReader(text);
				if (mBinaryReader != null)
				{
					if (mBinaryReader.IsOpen())
					{
						try
						{
							SoaringDictionary data = new SoaringDictionary(mBinaryReader.ReadAllBytes());
							SoaringContext soaringContext = new SoaringContext();
							soaringContext.Responder = new SendLogDumpDelegate();
							soaringContext.ContextResponder = context_responder;
							soaringContext.addValue(true, "trIOff");
							Soaring.SendSessionData(data, soaringContext);
							result = true;
						}
						catch (Exception ex)
						{
							ErrorLog("Exception: " + ex.Message);
							ErrorLog("Failed to upload log dump to server due to Exception when sending.");
						}
					}
					mBinaryReader.Close();
					mBinaryReader = null;
					if (context_responder == null)
					{
						File.Delete(text);
					}
				}
				result = true;
			}
		}
		catch (Exception ex2)
		{
			SoaringDebug.Log(ex2.Message, LogType.Error);
			result = false;
		}
		return result;
	}

	public static string GetConsoleOutput()
	{
		string result = null;
		try
		{
			int num = 0;
			if (ConsoleLog.Length > 131072)
			{
				num = ConsoleLog.Length - 131072;
			}
			result = Convert.ToBase64String(Zip(ConsoleLog.ToString(num, ConsoleLog.Length)));
		}
		catch (Exception ex)
		{
			ErrorLog("GetGameJsonFile: Error: " + ex.Message);
		}
		return result;
	}

	public static string GetLastSoaringDebugFile(string path)
	{
		string result = null;
		try
		{
			if (FileIsExists(path))
			{
				result = Convert.ToBase64String(Zip(File.ReadAllText(path)));
			}
		}
		catch (Exception ex)
		{
			ErrorLog("GetGameJsonFile: Error: " + ex.Message);
		}
		return result;
	}

	public static string GetGameJsonFile(Player p)
	{
		string result = null;
		try
		{
			string text = p.CacheFile("game.json");
			if (FileIsExists(text))
			{
				result = Convert.ToBase64String(Zip(File.ReadAllText(text)));
			}
		}
		catch (Exception ex)
		{
			ErrorLog("GetGameJsonFile: Error: " + ex.Message);
		}
		return result;
	}

	public static string GetErrorLog()
	{
		string result = null;
		try
		{
			int num = 0;
			if (ConsoleLog.Length > 131072)
			{
				num = ConsoleLog.Length - 131072;
			}
			result = Convert.ToBase64String(Zip(ErrorConsoleLog.ToString(num, ErrorConsoleLog.Length)));
		}
		catch (Exception ex)
		{
			ErrorLog("GetErrorLog: Error: " + ex.Message);
		}
		return result;
	}

	public static void GotoAppstore()
	{
		DebugLog("Going to " + SBSettings.STORE_APP_URL);
		if (!SoaringPlatform.OpenURL(SBSettings.STORE_APP_URL))
		{
			WarningLog("Failed to open " + SBSettings.STORE_APP_URL + " with native handler");
			Application.OpenURL(SBSettings.STORE_APP_URL);
		}
	}

	public static void SetDefaultHeaders(WebHeaderCollection wc)
	{
		wc.Add("SB-Date", DateTime.UtcNow.ToUniversalTime().ToString("r"));
	}

	public static string GetPlayerName(SoaringPlayer player, string format = "{0}")
	{
		string text = string.Empty;
		if (player.LoginType != SoaringLoginType.Device && player.LoginType != SoaringLoginType.Soaring)
		{
			text = SoaringPlatform.PlatformUserAlias;
		}
		if (string.IsNullOrEmpty(text))
		{
			text = player.UserTag;
		}
		if (string.IsNullOrEmpty(text))
		{
			text = SoaringPlatform.DeviceID;
		}
		return string.Format(format, text);
	}

	public static bool FileIsExists(string filePath)
	{
		DebugLog("judge_filePath " + filePath);
		if (filePath.Contains("://"))
		{
			if (LoadWWWExist(filePath))
			{
				return true;
			}
			return false;
		}
		return File.Exists(filePath);
	}

	private static bool LoadWWWExist(string filePath)
	{
		WWW wWW = new WWW(filePath);
		while (!wWW.isDone)
		{
			if (wWW.error != null)
			{
				return false;
			}
		}
		return true;
	}

	public static bool isAmazon()
	{
		return SystemInfo.deviceModel.StartsWith("Amazon");
	}

	public static string ParseGameDetails(Dictionary<string, object> gameData, ref GameDetails details)
	{
		details = new GameDetails();
		try
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)gameData["playtime"];
			details.level = dictionary["level"].ToString();
			details.lastPlayTime = int.Parse(dictionary["last"].ToString());
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			details.dtLastPlayTime = dateTime.AddSeconds(details.lastPlayTime).ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
			DebugLog("dtDateTime " + details.dtLastPlayTime);
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)gameData["farm"];
			List<object> resources = (List<object>)dictionary2["resources"];
			details.money = resourceValueByDid(3, resources);
			details.jelly = resourceValueByDid(2, resources);
			details.patties = resourceValueByDid(1, resources);
		}
		catch (Exception ex)
		{
			ErrorLog("Failed to parse game details: " + ex.Message + "\n" + ex.StackTrace);
		}
		return string.Format("time: {0} level: {1} money: {2} jelly: {3}", details.lastPlayTime, details.level, details.money, details.jelly);
	}

	private static string resourceValueByDid(int lookupDid, List<object> resources)
	{
		string result = string.Empty;
		foreach (Dictionary<string, object> resource in resources)
		{
			if (!resource.ContainsKey("did"))
			{
				continue;
			}
			int num = int.Parse(resource["did"].ToString());
			if (lookupDid != num)
			{
				continue;
			}
			Dictionary<string, object> dictionary2 = resource;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			if (dictionary2.ContainsKey("amount_earned"))
			{
				num2 = int.Parse(dictionary2["amount_earned"].ToString());
			}
			if (dictionary2.ContainsKey("amount_purchased"))
			{
				num3 = int.Parse(dictionary2["amount_purchased"].ToString());
			}
			if (dictionary2.ContainsKey("amount_spent"))
			{
				num4 = int.Parse(dictionary2["amount_spent"].ToString());
			}
			result = (num2 + num3 - num4).ToString();
			break;
		}
		return result;
	}

	public static string GetEULA_Address()
	{
		string text = Language.Get("!!EULA_URL");
		if (text != null && text.Contains("!!EULA_URL"))
		{
			text = null;
		}
		if (string.IsNullOrEmpty(text))
		{
			switch (Language.CurrentLanguage())
			{
			case LanguageCode.LatAm:
				text = "http://mx.mundonick.com/gsp/international/mundonick.com/AcuerdodeEntregadeContenidosPorPartedeUsuarios.pdf";
				break;
			case LanguageCode.FR:
				text = "http://www.nickelodeon.fr/legal/";
				break;
			case LanguageCode.DE:
				text = "http://www.nick.de/static/info_contests";
				break;
			case LanguageCode.IT:
				text = "http://www.nicktv.it/info/note-legali";
				break;
			case LanguageCode.NL:
				text = "http://www.nickelodeon.nl/static/info_algemene_voorwaarden";
				break;
			case LanguageCode.RU:
				text = "http://www.nickelodeon.ru/gsp/international/nickelodeon.ru/footer/terms-and-conditions.pdf";
				break;
			case LanguageCode.ES:
				text = "http://www.nickelodeon.es/info/aviso-legal";
				break;
			case LanguageCode.PT:
				text = "http://mundonick.uol.com.br/gsp/international/mundonick.com.br/legal/termosdeuso.pdf";
				break;
			}
		}
		if (string.IsNullOrEmpty(text))
		{
			text = "http://www.nick.com/info/eula.html";
		}
		return text;
	}

	public static string GetLegal_Address()
	{
		string text = Language.Get("!!LEGAL_URL");
		if (text != null && text.Contains("!!LEGAL_URL"))
		{
			text = null;
		}
		if (string.IsNullOrEmpty(text))
		{
			switch (Language.CurrentLanguage())
			{
			case LanguageCode.LatAm:
				text = "http://mx.mundonick.com/gsp/international/mundonick.com/AcuerdodeEntregadeContenidosPorPartedeUsuarios.pdf";
				break;
			case LanguageCode.FR:
				text = "http://www.nickelodeon.fr/legal/";
				break;
			case LanguageCode.DE:
				text = "http://www.nick.de/static/info_contests";
				break;
			case LanguageCode.IT:
				text = "http://www.nicktv.it/info/note-legali";
				break;
			case LanguageCode.NL:
				text = "http://www.nickelodeon.nl/static/info_algemene_voorwaarden";
				break;
			case LanguageCode.RU:
				text = "http://www.nickelodeon.ru/gsp/international/nickelodeon.ru/footer/terms-and-conditions.pdf";
				break;
			case LanguageCode.ES:
				text = "http://www.nickelodeon.es/info/aviso-legal";
				break;
			case LanguageCode.PT:
				text = "http://mundonick.uol.com.br/gsp/international/mundonick.com.br/legal/termosdeuso.pdf";
				break;
			}
		}
		if (string.IsNullOrEmpty(text))
		{
			text = "http://www.nick.com/nick-assets/copy/legal.html";
		}
		return text;
	}

	public static string GetPrivacy_Address()
	{
		string text = Language.Get("!!PRIVACY_URL");
		if (text != null && text.Contains("!!PRIVACY_URL"))
		{
			text = null;
		}
		if (string.IsNullOrEmpty(text))
		{
			switch (Language.CurrentLanguage())
			{
			case LanguageCode.LatAm:
				text = "http://mx.mundonick.com/gsp/international/mundonick.com/politica_de_privacidad_de_mundonick.pdf";
				break;
			case LanguageCode.FR:
				text = "http://www.nickelodeon.fr/legal/";
				break;
			case LanguageCode.DE:
				text = "http://www.nick.de/static/info_datenschutz";
				break;
			case LanguageCode.IT:
				text = "http://www.nicktv.it/info/note-legali";
				break;
			case LanguageCode.NL:
				text = "http://www.nickelodeon.nl/static/info_privacy";
				break;
			case LanguageCode.RU:
				text = "http://www.nickelodeon.ru/gsp/international/nickelodeon.ru/footer/privacy-policy.pdf";
				break;
			case LanguageCode.ES:
				text = "Privacy: http://www.nickelodeon.es/info/privacidad";
				break;
			case LanguageCode.PT:
				text = "http://mundonick.uol.com.br/gsp/international/mundonick.com.br/legal/politicadeprivacidade.pdf";
				break;
			}
		}
		if (string.IsNullOrEmpty(text))
		{
			text = "http://www.nick.com/info/privacy-policy.html";
		}
		return text;
	}

	public static void RefreshSAFiles()
	{
		string filePath = GetStreamingAssetsPath() + Path.DirectorySeparatorChar + "sa_file.json";
		string filePath2 = GetPersistentAssetsPath() + "/sa_file.json";
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		string text = null;
		if (FileIsExists(filePath2))
		{
			text = ReadAllText(filePath2);
		}
		else if (FileIsExists(filePath))
		{
			text = ReadAllText(filePath);
		}
		if (text == null)
		{
			return;
		}
		dictionary = (Dictionary<string, object>)Json.Deserialize(text);
		for (int i = 0; i < Config.SA_FILES.Length; i++)
		{
			string text2 = Config.SA_FILES[i];
			if (dictionary.ContainsKey(text2))
			{
				string text3 = dictionary[text2] as string;
				string[] array = text3.Split(',');
				switch (text2)
				{
				case "Crafting":
					Config.CRAFTING_PATH = array;
					break;
				case "Dialogs":
					Config.DIALOG_PACKAGES_PATH = array;
					break;
				case "Features":
					Config.FEATURE_DATA_PATH = array;
					break;
				case "Video":
					Config.MOVIE_PATH = array;
					break;
				case "Notifications":
					Config.NOTIFICATIONS_PATH = array;
					break;
				case "Quests":
					Config.QUESTS_PATH = array;
					break;
				case "BonusPaytables":
					Config.BONUS_PAYTABLES = array;
					break;
				case "Tasks":
					Config.TASKS_PATH = array;
					break;
				case "Treasure":
					Config.TREASURE_PATH = array;
					break;
				case "Vending":
					Config.VENDING_PATH = array;
					break;
				case "Blueprints":
					Config.BLUEPRINT_DIRECTORY_PATH = array;
					break;
				case "Terrain":
					Config.TERRAIN_PATH = array;
					break;
				}
			}
		}
		UpdateSAFilePathRefs(Config.CRAFTING_PATH, "Crafting");
		UpdateSAFilePathRefs(Config.DIALOG_PACKAGES_PATH, "Dialogs");
		UpdateSAFilePathRefs(Config.FEATURE_DATA_PATH, "Features");
		UpdateSAFilePathRefs(Config.MOVIE_PATH, "Video");
		UpdateSAFilePathRefs(Config.NOTIFICATIONS_PATH, "Notifications");
		UpdateSAFilePathRefs(Config.QUESTS_PATH, "Quests");
		UpdateSAFilePathRefs(Config.BONUS_PAYTABLES, "BonusPaytables");
		UpdateSAFilePathRefs(Config.TASKS_PATH, "Tasks");
		UpdateSAFilePathRefs(Config.TREASURE_PATH, "Treasure");
		UpdateSAFilePathRefs(Config.VENDING_PATH, "Vending");
		UpdateSAFilePathRefs(Config.BLUEPRINT_DIRECTORY_PATH, "Blueprints");
		UpdateSAFilePathRefs(Config.TERRAIN_PATH, "Terrain");
	}

	private static void UpdateSAFilePathRefs(string[] files, string directory)
	{
		if (files == null || string.IsNullOrEmpty(directory))
		{
			return;
		}
		int num = files.Length;
		string text = Path.DirectorySeparatorChar + directory + Path.DirectorySeparatorChar;
		string text2 = GetPersistentAssetsPath() + text;
		string text3 = GetStreamingAssetsPath() + text;
		for (int i = 0; i < num; i++)
		{
			string text4 = files[i];
			string text5 = text2 + text4;
			if (File.Exists(text5))
			{
				files[i] = text5;
			}
			else
			{
				files[i] = text3 + text4;
			}
		}
	}
}
