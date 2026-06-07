using System;
using System.Collections.Generic;

public class SBSettings
{
	private static string cdnUrl;

	private static string manifestUrl;

	private static string manifestFile;

	private static string serverUrl;

	private static int saveInterval;

	private static int patchingFileLimit;

	private static int retryCount;

	private static int? analyticsBufferSize;

	private static bool debugDisplayControllers;

	private static string bundleIdentifier;

	private static string bundleVersion;

	private static Version localBundleVersion;

	private static string bundleShortVersion;

	private static string storeAppUrl;

	private static bool trackStatistics;

	private static float statisticsTrackingInterval;

	private static bool showDebug;

	private static bool enableRandomQuests;

	private static bool enableAutoQuests;

	private static bool enableShakeLogDump;

	private static bool assertDataValidity;

	private static bool consoleLogging;

	private static bool bypassPatching;

	private static bool soaringProduction;

	private static bool soaringQA;

	private static bool useLegacySaveLoad;

	private static string useStoreName;

	private static bool rebootOnFocusChange;

	private static bool rebootOnConnectionChange;

	private static bool useProductionIAP;

	private static float communityEventBannerPing;

	private static bool dumpLogOnAssert;

	private static bool enableAds;

	private static bool offlineModeFriendPark;

	private static bool disableFriendPark;

	private static string deltaDNAEnvKey;

	private static Version currentAppStoreBundleVersion;

	private static string billingsKey;

	public const string LAST_PROMPTED_APPSTORE_VERSION_FIELD = "lastASV2";

	private const string LAST_RUN_APP_VERSION = "lastRAV1";

	private const string MUTABLE_SETTINGS_FILE = "app_settings.json";

	private static string mMUTABLE_SETTINGS_PATH;

	private static Version mutableLastPromptedAppstoreVersion;

	private static Version mutableLastCheckedAppVersion;

	public static Version LOCAL_BUNDLE_VERSION
	{
		get
		{
			return null;
		}
	}

	public static Version CURRENT_APPSTORE_BUNDLE_VERSION
	{
		get
		{
			return null;
		}
	}

	public static string CDN_URL
	{
		get
		{
			return null;
		}
	}

	public static string MANIFEST_FILE
	{
		get
		{
			return null;
		}
	}

	public static string MANIFEST_URL
	{
		get
		{
			return null;
		}
	}

	public static string SERVER_URL
	{
		get
		{
			return null;
		}
	}

	public static string STORE_APP_URL
	{
		get
		{
			return null;
		}
	}

	public static int SAVE_INTERVAL
	{
		get
		{
			return 0;
		}
	}

	public static int PATCHING_FILE_LIMIT
	{
		get
		{
			return 0;
		}
	}

	public static int NETWORK_RETRY_COUNT
	{
		get
		{
			return 0;
		}
	}

	public static int? ANALAYTICS_BUFFER_SIZE
	{
		get
		{
			return null;
		}
	}

	public static string BundleIdentifier
	{
		get
		{
			return null;
		}
	}

	public static string BundleVersion
	{
		get
		{
			return null;
		}
	}

	public static string BundleShortVersion
	{
		get
		{
			return null;
		}
	}

	public static string StoreName
	{
		get
		{
			return null;
		}
	}

	public static bool DebugDisplayControllers
	{
		get
		{
			return false;
		}
	}

	public static bool TrackStatistics
	{
		get
		{
			return false;
		}
	}

	public static float StatisticsTrackingInterval
	{
		get
		{
			return 0f;
		}
	}

	public static bool ShowDebug
	{
		get
		{
			return false;
		}
	}

	public static bool EnableRandomQuests
	{
		get
		{
			return false;
		}
	}

	public static bool EnableAutoQuests
	{
		get
		{
			return false;
		}
	}

	public static float CommunityEventBannerPing
	{
		get
		{
			return 0f;
		}
	}

	public static bool EnableShakeLogDump
	{
		get
		{
			return false;
		}
	}

	public static bool AssertDataValidity
	{
		get
		{
			return false;
		}
	}

	public static bool ConsoleLoggingEnabled
	{
		get
		{
			return false;
		}
	}

	public static bool BypassPatching
	{
		get
		{
			return false;
		}
	}

	public static bool SoaringProduction
	{
		get
		{
			return false;
		}
	}

	public static bool SoaringQA
	{
		get
		{
			return false;
		}
	}

	public static string BillingKey
	{
		get
		{
			return null;
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
			return false;
		}
	}

	public static bool RebootOnFocusChange
	{
		get
		{
			return false;
		}
	}

	public static bool RebootOnConnectionChange
	{
		get
		{
			return false;
		}
	}

	public static bool UseProductionIAP
	{
		get
		{
			return false;
		}
	}

	public static bool OfflineModeFriendParks
	{
		get
		{
			return false;
		}
	}

	public static bool DisableFriendPark
	{
		get
		{
			return false;
		}
	}

	public static bool DumpLogOnAssert
	{
		get
		{
			return false;
		}
	}

	public static string DeltaDNAEnvKey
	{
		get
		{
			return null;
		}
	}

	public static bool AdsEnabled
	{
		get
		{
			return false;
		}
	}

	private static string MUTABLE_SETTINGS_PATH
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public static Version LastPromptedAppstoreVersion
	{
		get
		{
			return null;
		}
	}

	public static bool IsLastRunVersion
	{
		get
		{
			return false;
		}
	}

	private SBSettings()
	{
	}

	public static void Init()
	{
	}

	private static void _Init(bool isReload)
	{
	}

	private static bool LoadSettings(string name, Dictionary<string, object> data)
	{
		return false;
	}

	private static void LoadAppMutableSettings(Dictionary<string, object> mutableSettingsMap = null)
	{
	}

	private static void SaveMutableAppSetting(string key, object value)
	{
	}

	public static void SaveLastPromptedAppstoreVersion()
	{
	}
}
