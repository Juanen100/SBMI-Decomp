internal static class SoaringInternalProperties
{
	private static int mSettings;

	public static int AnalyticsBufferSize;

	public static string DeveloperLoginTag;

	public static string DeveloperLoginPassword;

	public static string SoaringTestingURL;

	public static string SoaringDevelopmentURL;

	public static string SoaringProductionURL;

	public static string SoaringTestingCDN;

	public static string SoaringDevelopmentCDN;

	public static string SoaringProductionCDN;

	private static string DevAuthLoginToken;

	public static bool IsLoaded;

	public static bool EnableAddressKeeper
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public static bool EnableVersions
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public static bool EnableServerTimeVersions
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public static bool EnableDeveloperLogin
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public static bool EnableLocalMode
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public static bool EnableAdServer
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public static bool EnableDeviceData
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public static bool EnableAnalytics
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public static bool LoginOnInitialize
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public static bool SaveUserCredentials
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public static bool SecureCommunication
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public static bool AutoChooseUserPlayer
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public static bool ForceOfflineModeUser
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	private static bool Get(int x)
	{
		return false;
	}

	private static void Set(bool v, int x)
	{
	}

	internal static void Load()
	{
	}
}
