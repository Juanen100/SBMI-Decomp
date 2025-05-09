using System;
using System.Collections.Generic;
using System.IO;
using MiniJSON;
using UnityEngine;

public class SBSettings
{
	public const string LAST_PROMPTED_APPSTORE_VERSION_FIELD = "lastASV2";

	private const string LAST_RUN_APP_VERSION = "lastRAV1";

	private const string MUTABLE_SETTINGS_FILE = "app_settings.json";

	private static string cdnUrl;

	private static string manifestUrl;

	private static string manifestFile;

	private static string serverUrl;

	private static int saveInterval;

	private static int patchingFileLimit;

	private static int retryCount;

	private static int? analyticsBufferSize;

	private static bool debugDisplayControllers = false;

	private static string bundleIdentifier;

	private static string bundleVersion;

	private static Version localBundleVersion;

	private static string bundleShortVersion;

	private static string storeAppUrl;

	private static bool trackStatistics = false;

	private static float statisticsTrackingInterval;

	private static bool showDebug = false;

	private static bool enableRandomQuests = false;

	private static bool enableAutoQuests = true;

	private static bool enableShakeLogDump = false;

	private static bool assertDataValidity = false;

	private static bool consoleLogging = false;

	private static bool bypassPatching = false;

	private static bool soaringProduction = false;

	private static bool soaringQA = false;

	private static bool useLegacySaveLoad = false;

	private static string useStoreName;

	private static bool rebootOnFocusChange = false;

	private static bool rebootOnConnectionChange = false;

	private static bool useProductionIAP = true;

	private static float communityEventBannerPing = -1f;

	private static bool dumpLogOnAssert = true;

	private static bool offlineModeFriendPark = false;

	private static bool disableFriendPark = false;

	private static string deltaDNAEnvKey = string.Empty;

	private static Version currentAppStoreBundleVersion;

	private static string billingsKey = null;

	private static string MUTABLE_SETTINGS_PATH = Application.persistentDataPath + Path.DirectorySeparatorChar + "app_settings.json";

	private static Version mutableLastPromptedAppstoreVersion;

	private static Version mutableLastCheckedAppVersion;

	public static Version LOCAL_BUNDLE_VERSION
	{
		get
		{
			return localBundleVersion;
		}
	}

	public static Version CURRENT_APPSTORE_BUNDLE_VERSION
	{
		get
		{
			return currentAppStoreBundleVersion;
		}
	}

	public static string CDN_URL
	{
		get
		{
			return cdnUrl;
		}
	}

	public static string MANIFEST_FILE
	{
		get
		{
			return manifestFile;
		}
	}

	public static string MANIFEST_URL
	{
		get
		{
			return manifestUrl;
		}
	}

	public static string SERVER_URL
	{
		get
		{
			return serverUrl;
		}
	}

	public static string STORE_APP_URL
	{
		get
		{
			return storeAppUrl;
		}
	}

	public static int SAVE_INTERVAL
	{
		get
		{
			return saveInterval;
		}
	}

	public static int PATCHING_FILE_LIMIT
	{
		get
		{
			return patchingFileLimit;
		}
	}

	public static int NETWORK_RETRY_COUNT
	{
		get
		{
			return retryCount;
		}
	}

	public static int? ANALAYTICS_BUFFER_SIZE
	{
		get
		{
			return analyticsBufferSize;
		}
	}

	public static string BundleIdentifier
	{
		get
		{
			return bundleIdentifier;
		}
	}

	public static string BundleVersion
	{
		get
		{
			return bundleVersion;
		}
	}

	public static string BundleShortVersion
	{
		get
		{
			return bundleShortVersion;
		}
	}

	public static string StoreName
	{
		get
		{
			return useStoreName;
		}
	}

	public static bool DebugDisplayControllers
	{
		get
		{
			return debugDisplayControllers;
		}
	}

	public static bool TrackStatistics
	{
		get
		{
			return trackStatistics;
		}
	}

	public static float StatisticsTrackingInterval
	{
		get
		{
			return statisticsTrackingInterval;
		}
	}

	public static bool ShowDebug
	{
		get
		{
			return showDebug;
		}
	}

	public static bool EnableRandomQuests
	{
		get
		{
			return enableRandomQuests;
		}
	}

	public static bool EnableAutoQuests
	{
		get
		{
			return enableAutoQuests;
		}
	}

	public static float CommunityEventBannerPing
	{
		get
		{
			return communityEventBannerPing;
		}
	}

	public static bool EnableShakeLogDump
	{
		get
		{
			return enableShakeLogDump;
		}
	}

	public static bool AssertDataValidity
	{
		get
		{
			return assertDataValidity;
		}
	}

	public static bool ConsoleLoggingEnabled
	{
		get
		{
			return consoleLogging;
		}
	}

	public static bool BypassPatching
	{
		get
		{
			return bypassPatching;
		}
	}

	public static bool SoaringProduction
	{
		get
		{
			return soaringProduction;
		}
	}

	public static bool SoaringQA
	{
		get
		{
			return soaringQA;
		}
	}

	public static string BillingKey
	{
		get
		{
			return billingsKey;
		}
	}

	public static bool UseActionFile
	{
		get
		{
			return false;
		}
	}

	public static bool UseLegacyGameLoad
	{
		get
		{
			return useLegacySaveLoad;
		}
	}

	public static bool RebootOnFocusChange
	{
		get
		{
			return rebootOnFocusChange;
		}
	}

	public static bool RebootOnConnectionChange
	{
		get
		{
			return rebootOnConnectionChange;
		}
	}

	public static bool UseProductionIAP
	{
		get
		{
			return useProductionIAP;
		}
	}

	public static bool OfflineModeFriendParks
	{
		get
		{
			return offlineModeFriendPark;
		}
	}

	public static bool DisableFriendPark
	{
		get
		{
			return disableFriendPark;
		}
	}

	public static bool DumpLogOnAssert
	{
		get
		{
			return dumpLogOnAssert;
		}
	}

	public static string DeltaDNAEnvKey
	{
		get
		{
			return deltaDNAEnvKey;
		}
	}

	public static Version LastPromptedAppstoreVersion
	{
		get
		{
			return mutableLastPromptedAppstoreVersion;
		}
	}

	public static bool IsLastRunVersion
	{
		get
		{
			if (mutableLastCheckedAppVersion == null || localBundleVersion == null)
			{
				return false;
			}
			if (mutableLastCheckedAppVersion != localBundleVersion)
			{
				mutableLastCheckedAppVersion = localBundleVersion;
				SaveMutableAppSetting("lastRAV1", mutableLastCheckedAppVersion.ToString());
				return false;
			}
			return true;
		}
	}

	private SBSettings()
	{
	}

	public static void Init()
	{
		_Init(false);
	}

	private static void _Init(bool isReload)
	{
		string streamingAssetsFile = TFUtils.GetStreamingAssetsFile("server_settings.json");
		string json = TFUtils.ReadAllText(streamingAssetsFile);
		Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(json);
		serverUrl = (string)dictionary["server_url"];
		cdnUrl = (string)dictionary["cdn_url"];
		manifestFile = (string)dictionary["manifest_file"];
		manifestUrl = cdnUrl + manifestFile;
		if (dictionary.ContainsKey("bypass_patching"))
		{
			string text = (string)dictionary["bypass_patching"];
			if (!string.IsNullOrEmpty(text))
			{
				bypassPatching = char.ToLower(text[0]) == 't';
			}
		}
		if (dictionary.ContainsKey("soaring_live"))
		{
			string text2 = (string)dictionary["soaring_live"];
			if (!string.IsNullOrEmpty(text2))
			{
				soaringProduction = char.ToLower(text2[0]) == 't';
			}
		}
		if (dictionary.ContainsKey("soaring_qa"))
		{
			string text3 = (string)dictionary["soaring_qa"];
			if (!string.IsNullOrEmpty(text3))
			{
				soaringQA = char.ToLower(text3[0]) == 't';
			}
		}
		if (dictionary.ContainsKey("deltaDNAEnv"))
		{
			deltaDNAEnvKey = (string)dictionary["deltaDNAEnv"];
		}
		string text4 = "ios";
		bundleIdentifier = "com.nick.sbappstore";
		bundleVersion = "0.00.00";
		bundleShortVersion = "0.00.00";
		text4 = "googleplay";
		if (TFUtils.isAmazon())
		{
			text4 = "amazon";
		}
		string text5 = null;
		text5 = ((!soaringProduction) ? TFUtils.ReadAllText(TFUtils.GetStreamingAssetsFile("android_version.json")) : TFUtils.ReadAllText(TFUtils.GetStreamingAssetsFile_IgnorePersistant("android_version.json")));
		Dictionary<string, object> dictionary2 = (Dictionary<string, object>)Json.Deserialize(text5);
		if (dictionary2.ContainsKey(text4 + "_bundle_identifier"))
		{
			bundleIdentifier = (string)dictionary2[text4 + "_bundle_identifier"];
		}
		if (dictionary2.ContainsKey(text4 + "_bundle_version"))
		{
			bundleVersion = (string)dictionary2[text4 + "_bundle_version"];
		}
		if (dictionary2.ContainsKey(text4 + "_bundle_short_version"))
		{
			bundleShortVersion = (string)dictionary2[text4 + "_bundle_short_version"];
		}
		if (dictionary2.ContainsKey(text4 + "_billing_key"))
		{
			billingsKey = (string)dictionary2[text4 + "_billing_key"];
		}
		if (bundleShortVersion.Contains("_"))
		{
			bundleShortVersion = bundleShortVersion.Substring(0, bundleShortVersion.IndexOf("_"));
		}
		if (bundleVersion.Contains("_"))
		{
			bundleVersion = bundleVersion.Substring(0, bundleVersion.IndexOf("_"));
		}
		localBundleVersion = new Version(bundleShortVersion);
		streamingAssetsFile = TFUtils.GetStreamingAssetsFile("global_settings.json");
		json = TFUtils.ReadAllText(streamingAssetsFile);
		dictionary = (Dictionary<string, object>)Json.Deserialize(json);
		bool flag = LoadSettings(text4, dictionary);
		LoadAppMutableSettings();
		if (!flag)
		{
			Debug.LogError("Error: Failed to Load Settings!!! : " + isReload);
			if (!isReload)
			{
				TFUtils.DeletePersistantFile("global_settings.json");
				_Init(true);
			}
			else
			{
				Debug.LogError("Critical: Failed to Load Settings!!! : " + isReload);
			}
		}
		streamingAssetsFile = TFUtils.GetStreamingAssetsFile("quality_settings.json");
		json = TFUtils.ReadAllText(streamingAssetsFile);
		dictionary = (Dictionary<string, object>)Json.Deserialize(json);
		CommonUtils.Init(dictionary);
	}

	private static bool LoadSettings(string name, Dictionary<string, object> data)
	{
		if (name == null || data == null)
		{
			Debug.LogError("Null Or Invalid Settings Data!");
			return false;
		}
		if (data.ContainsKey(name))
		{
			data = (Dictionary<string, object>)data[name];
		}
		else if (!data.ContainsKey("file_version"))
		{
			Debug.LogError("Invalid Global Settings File Version!");
			return false;
		}
		saveInterval = TFUtils.LoadInt(data, "save_interval");
		if (bypassPatching)
		{
			saveInterval = -1;
		}
		storeAppUrl = (string)data["store_url"];
		if (data.ContainsKey("log_settings"))
		{
			TFUtils.SetLogType((string)data["log_settings"]);
		}
		object value;
		if (data.TryGetValue("debugDisplayControllers", out value))
		{
			debugDisplayControllers = (bool)value;
		}
		patchingFileLimit = TFUtils.LoadInt(data, "patching_file_limit", 10);
		retryCount = TFUtils.LoadInt(data, "network_retry_count", 2);
		float? num = TFUtils.TryLoadFloat(data, "statistics_tracking_interval");
		if (num.HasValue)
		{
			statisticsTrackingInterval = num.Value;
		}
		else
		{
			statisticsTrackingInterval = 60f;
		}
		analyticsBufferSize = TFUtils.TryLoadInt(data, "analytics_buffer_size");
		trackStatistics = TFUtils.LoadBool(data, "track_statistics", false);
		useProductionIAP = TFUtils.LoadBool(data, "production_iap", true);
		consoleLogging = TFUtils.LoadBool(data, "console_logging", false);
		rebootOnFocusChange = TFUtils.LoadBool(data, "reboot_on_focus", false);
		rebootOnConnectionChange = TFUtils.LoadBool(data, "reboot_on_connection_change", false);
		offlineModeFriendPark = TFUtils.LoadBool(data, "offline_mode_visit_friends", false);
		disableFriendPark = TFUtils.LoadBool(data, "disable_visit_friends", false);
		assertDataValidity = TFUtils.LoadBool(data, "assert_valid_data", false);
		enableShakeLogDump = TFUtils.LoadBool(data, "shake_log_dump", false);
		dumpLogOnAssert = TFUtils.LoadBool(data, "assert_log_dump", true);
		showDebug = TFUtils.LoadBool(data, "show_debug", false);
		enableRandomQuests = TFUtils.LoadBool(data, "enable_random_quests", false);
		enableAutoQuests = TFUtils.LoadBool(data, "enable_auto_quests", true);
		num = TFUtils.TryLoadFloat(data, "community_event_banner_ping");
		if (num.HasValue)
		{
			communityEventBannerPing = num.Value;
		}
		else
		{
			communityEventBannerPing = -1f;
		}
		enableShakeLogDump = TFUtils.LoadBool(data, "shake_log_dump", false);
		useLegacySaveLoad = TFUtils.LoadBool(data, "legacy_game_load", false);
		string key = "current_bundle_version";
		if (data.ContainsKey(key))
		{
			currentAppStoreBundleVersion = new Version((string)data[key]);
		}
		else
		{
			currentAppStoreBundleVersion = localBundleVersion;
		}
		string text = "store_name";
		if (TFUtils.isAmazon())
		{
			useStoreName = "amazon";
		}
		else
		{
			useStoreName = "google_play";
		}
		if (UseProductionIAP && !ConsoleLoggingEnabled)
		{
			TFUtils.LOG_LEVEL = TFUtils.LogLevel.WARN;
		}
		if (text != null && data.ContainsKey(text))
		{
			useStoreName = (string)data[text];
		}
		return true;
	}

	private static void LoadAppMutableSettings(Dictionary<string, object> mutableSettingsMap = null)
	{
		if (mutableSettingsMap == null && TFUtils.FileIsExists(MUTABLE_SETTINGS_PATH))
		{
			string json = TFUtils.ReadAllText(MUTABLE_SETTINGS_PATH);
			mutableSettingsMap = (Dictionary<string, object>)Json.Deserialize(json);
		}
		if (mutableSettingsMap != null)
		{
			object value = null;
			if (mutableSettingsMap.TryGetValue("lastASV2", out value))
			{
				mutableLastPromptedAppstoreVersion = new Version((string)value);
			}
			else
			{
				mutableLastPromptedAppstoreVersion = new Version("0.0.0");
			}
			value = null;
			if (mutableSettingsMap.TryGetValue("lastRAV1", out value))
			{
				mutableLastCheckedAppVersion = new Version((string)value);
			}
			else
			{
				mutableLastCheckedAppVersion = new Version("0.0.0");
			}
		}
		else
		{
			mutableLastPromptedAppstoreVersion = new Version("0.0.0");
			mutableLastCheckedAppVersion = new Version("0.0.0");
		}
	}

	private static void SaveMutableAppSetting(string key, object value)
	{
		Dictionary<string, object> dictionary;
		if (TFUtils.FileIsExists(MUTABLE_SETTINGS_PATH))
		{
			string json = TFUtils.ReadAllText(MUTABLE_SETTINGS_PATH);
			dictionary = (Dictionary<string, object>)Json.Deserialize(json);
		}
		else
		{
			dictionary = new Dictionary<string, object>();
		}
		dictionary[key] = value;
		string contents = Json.Serialize(dictionary);
		File.WriteAllText(MUTABLE_SETTINGS_PATH, contents);
		LoadAppMutableSettings(dictionary);
	}

	public static void SaveLastPromptedAppstoreVersion()
	{
		SaveMutableAppSetting("lastASV2", CURRENT_APPSTORE_BUNDLE_VERSION.ToString());
	}
}
