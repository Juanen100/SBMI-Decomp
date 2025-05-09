using MTools;

internal static class SoaringInternalProperties
{
	private static int mSettings;

	public static int AnalyticsBufferSize;

	public static string DeveloperLoginTag;

	public static string DeveloperLoginPassword;

	public static string SoaringTestingURL = string.Empty;

	public static string SoaringDevelopmentURL = string.Empty;

	public static string SoaringProductionURL = string.Empty;

	public static string SoaringTestingCDN = string.Empty;

	public static string SoaringDevelopmentCDN = string.Empty;

	public static string SoaringProductionCDN = string.Empty;

	private static string DevAuthLoginToken;

	public static bool IsLoaded;

	public static bool EnableAddressKeeper
	{
		get
		{
			return Get(0);
		}
		set
		{
			Set(value, 0);
		}
	}

	public static bool EnableVersions
	{
		get
		{
			return Get(1);
		}
		set
		{
			Set(value, 1);
		}
	}

	public static bool EnableServerTimeVersions
	{
		get
		{
			return Get(2);
		}
		set
		{
			Set(value, 2);
		}
	}

	public static bool EnableDeveloperLogin
	{
		get
		{
			return Get(3);
		}
		set
		{
			Set(value, 3);
		}
	}

	public static bool EnableLocalMode
	{
		get
		{
			return Get(4);
		}
		set
		{
			Set(value, 4);
		}
	}

	public static bool EnableAdServer
	{
		get
		{
			return Get(5);
		}
		set
		{
			Set(value, 5);
		}
	}

	public static bool EnableDeviceData
	{
		get
		{
			return Get(6);
		}
		set
		{
			Set(value, 6);
		}
	}

	public static bool EnableAnalytics
	{
		get
		{
			return Get(7);
		}
		set
		{
			Set(value, 7);
		}
	}

	public static bool LoginOnInitialize
	{
		get
		{
			return Get(8);
		}
		set
		{
			Set(value, 8);
		}
	}

	public static bool SaveUserCredentials
	{
		get
		{
			return Get(9);
		}
		set
		{
			Set(value, 9);
		}
	}

	public static bool SecureCommunication
	{
		get
		{
			return Get(10);
		}
		set
		{
			Set(value, 10);
		}
	}

	public static bool AutoChooseUserPlayer
	{
		get
		{
			return Get(11);
		}
		set
		{
			Set(value, 11);
		}
	}

	public static bool ForceOfflineModeUser
	{
		get
		{
			return Get(12);
		}
		set
		{
			Set(value, 12);
		}
	}

	private static bool Get(int x)
	{
		return (mSettings & (1 << x)) != 0;
	}

	private static void Set(bool v, int x)
	{
		mSettings = ((!v) ? (mSettings & ~(1 << x)) : (mSettings | (1 << x)));
	}

	internal static void Load()
	{
		EnableAddressKeeper = true;
		EnableVersions = true;
		EnableServerTimeVersions = true;
		EnableDeveloperLogin = false;
		EnableLocalMode = false;
		EnableAdServer = true;
		EnableDeviceData = true;
		EnableAnalytics = true;
		LoginOnInitialize = true;
		SaveUserCredentials = true;
		SecureCommunication = false;
		AutoChooseUserPlayer = false;
		ForceOfflineModeUser = false;
		MBinaryReader fileStream = ResourceUtils.GetFileStream("SoaringPR", "Soaring", "bytes", 3);
		SoaringDictionary soaringDictionary = null;
		if (fileStream != null)
		{
			string text = string.Empty;
			byte[] array = fileStream.ReadAllBytes();
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					text += (char)array[i];
				}
			}
			soaringDictionary = new SoaringDictionary(text);
			fileStream.Close();
			fileStream = null;
		}
		if (soaringDictionary == null)
		{
			soaringDictionary = new SoaringDictionary();
		}
		if (soaringDictionary != null)
		{
			SoaringValue soaringValue = soaringDictionary.soaringValue("AddressKeeper");
			if (soaringValue != null)
			{
				EnableAddressKeeper = (int)soaringValue != 0;
			}
			soaringValue = soaringDictionary.soaringValue("Versions");
			if (soaringValue != null)
			{
				EnableAddressKeeper = (int)soaringValue != 0;
			}
			soaringValue = soaringDictionary.soaringValue("AdServer");
			if (soaringValue != null)
			{
				EnableAdServer = (int)soaringValue != 0;
			}
			soaringValue = soaringDictionary.soaringValue("DeviceData");
			if (soaringValue != null)
			{
				EnableDeviceData = (int)soaringValue != 0;
			}
			soaringValue = soaringDictionary.soaringValue("Analytics");
			if (soaringValue != null)
			{
				EnableAnalytics = (int)soaringValue != 0;
			}
			soaringValue = soaringDictionary.soaringValue("ServerTime");
			if (soaringValue != null)
			{
				EnableServerTimeVersions = (int)soaringValue != 0;
			}
			soaringValue = soaringDictionary.soaringValue("LocalMode");
			if (soaringValue != null)
			{
				EnableLocalMode = (int)soaringValue != 0;
			}
			soaringValue = soaringDictionary.soaringValue("LoginOnInitialize");
			if (soaringValue != null)
			{
				LoginOnInitialize = (int)soaringValue != 0;
			}
			soaringValue = soaringDictionary.soaringValue("DevLogin");
			if (soaringValue != null)
			{
				EnableDeveloperLogin = (int)soaringValue != 0;
			}
			soaringValue = soaringDictionary.soaringValue("SaveUserCredentials");
			if (soaringValue != null)
			{
				SaveUserCredentials = (int)soaringValue != 0;
			}
			soaringValue = soaringDictionary.soaringValue("SecureCommunication");
			if (soaringValue != null)
			{
				SecureCommunication = (int)soaringValue != 0;
			}
			soaringValue = soaringDictionary.soaringValue("AutoChooseUserPlayer");
			if (soaringValue != null)
			{
				AutoChooseUserPlayer = (int)soaringValue != 0;
			}
			soaringValue = soaringDictionary.soaringValue("ForceOfflineModeUser");
			if (soaringValue != null)
			{
				ForceOfflineModeUser = (int)soaringValue != 0;
			}
			soaringValue = soaringDictionary.soaringValue("AnalyticsBufferSize");
			if (soaringValue != null)
			{
				AnalyticsBufferSize = soaringValue;
			}
			soaringValue = soaringDictionary.soaringValue("DevName");
			if (soaringValue != null)
			{
				DeveloperLoginTag = soaringValue;
				if (string.IsNullOrEmpty(DeveloperLoginTag))
				{
					DeveloperLoginTag = null;
				}
			}
			soaringValue = soaringDictionary.soaringValue("DevPassword");
			if (soaringValue != null)
			{
				DeveloperLoginPassword = soaringValue;
				if (string.IsNullOrEmpty(DeveloperLoginPassword))
				{
					DeveloperLoginPassword = null;
				}
			}
			soaringValue = soaringDictionary.soaringValue("SoaringQAURL");
			if (soaringValue != null)
			{
				SoaringTestingURL = soaringValue;
				if (string.IsNullOrEmpty(SoaringTestingURL))
				{
					SoaringTestingURL = null;
				}
			}
			soaringValue = soaringDictionary.soaringValue("SoaringDevURL");
			if (soaringValue != null)
			{
				SoaringDevelopmentURL = soaringValue;
				if (string.IsNullOrEmpty(SoaringDevelopmentURL))
				{
					SoaringDevelopmentURL = null;
				}
			}
			soaringValue = soaringDictionary.soaringValue("SoaringProductionURL");
			if (soaringValue != null)
			{
				SoaringProductionURL = soaringValue;
				if (string.IsNullOrEmpty(SoaringProductionURL))
				{
					SoaringProductionURL = null;
				}
			}
			soaringValue = soaringDictionary.soaringValue("SoaringQACDN");
			if (soaringValue != null)
			{
				SoaringTestingCDN = soaringValue;
				if (string.IsNullOrEmpty(SoaringTestingCDN))
				{
					SoaringTestingCDN = SoaringTestingURL;
				}
			}
			soaringValue = soaringDictionary.soaringValue("SoaringDevCDN");
			if (soaringValue != null)
			{
				SoaringDevelopmentCDN = soaringValue;
				if (string.IsNullOrEmpty(SoaringDevelopmentCDN))
				{
					SoaringDevelopmentCDN = SoaringDevelopmentURL;
				}
			}
			soaringValue = soaringDictionary.soaringValue("SoaringProductionCDN");
			if (soaringValue != null)
			{
				SoaringProductionCDN = soaringValue;
				if (string.IsNullOrEmpty(SoaringProductionCDN))
				{
					SoaringProductionCDN = SoaringProductionURL;
				}
			}
		}
		IsLoaded = true;
	}
}
