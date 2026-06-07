public class SoaringPlatform
{
	public class SoaringPlatformDelegate
	{
		public virtual void Init()
		{
		}

		public virtual SoaringLoginType PreferedLoginType()
		{
			return default(SoaringLoginType);
		}

		public virtual string PlatformName()
		{
			return null;
		}

		public virtual bool PlatformLoginAvailable()
		{
			return false;
		}

		public virtual bool PlatformAuthenticated()
		{
			return false;
		}

		public virtual bool PlatformAuthenticate(SoaringContext context)
		{
			return false;
		}

		public virtual string PlatformID()
		{
			return null;
		}

		public virtual string PlatformAlias()
		{
			return null;
		}

		public virtual string DeviceID()
		{
			return null;
		}

		public virtual SoaringDictionary GenerateDeviceDictionary()
		{
			return null;
		}

		public virtual string PushNotificationsProtocol()
		{
			return null;
		}

		public virtual void SetPlatformUserData(string userID, string userAlias)
		{
		}

		public virtual bool OpenURL(string url)
		{
			return false;
		}

		public virtual bool SendEmail(string subject, string body, string email)
		{
			return false;
		}

		public virtual bool OpenPath(string path)
		{
			return false;
		}

		public virtual long SystemBootTime()
		{
			return 0L;
		}

		public virtual long SystemTimeSinceBootTime()
		{
			return 0L;
		}
	}

	private static SoaringPlatform sInstance;

	private SoaringPlatformDelegate platformDelegate;

	private SoaringPlatformType platformType;

	public static SoaringLoginType PreferedLoginType
	{
		get
		{
			return default(SoaringLoginType);
		}
	}

	public static SoaringPlatformType Platform
	{
		get
		{
			return default(SoaringPlatformType);
		}
	}

	public static bool PlatformLoginAvailable
	{
		get
		{
			return false;
		}
	}

	public static bool PlatformLoginAuthenticated
	{
		get
		{
			return false;
		}
	}

	public static string DeviceID
	{
		get
		{
			return null;
		}
	}

	public static string PlatformUserID
	{
		get
		{
			return null;
		}
	}

	public static string PlatformUserAlias
	{
		get
		{
			return null;
		}
	}

	public static string PushNotificationsProtocol
	{
		get
		{
			return null;
		}
	}

	public static string PrimaryPlatformName
	{
		get
		{
			return null;
		}
	}

	private SoaringPlatform(SoaringPlatformType platform)
	{
	}

	internal static void Init(SoaringPlatformType platform)
	{
	}

	public static void SetPlatformUserData(string userID, string userAlias)
	{
	}

	public static SoaringDictionary GenerateDeviceDictionary()
	{
		return null;
	}

	public static bool AuthenticatedPlatformUser(SoaringContext context)
	{
		return false;
	}

	public static SoaringPlatformDelegate GetDelegate()
	{
		return null;
	}

	public static bool OpenURL(string url)
	{
		return false;
	}

	public static bool SendEmail(string subject, string body, string email)
	{
		return false;
	}

	public static long SystemBootTime()
	{
		return 0L;
	}

	public static long SystemTimeSinceBootTime()
	{
		return 0L;
	}
}
