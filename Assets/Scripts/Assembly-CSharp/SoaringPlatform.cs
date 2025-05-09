using System;
using UnityEngine;

public class SoaringPlatform
{
	public class SoaringPlatformDelegate
	{
		public virtual void Init()
		{
		}

		public virtual SoaringLoginType PreferedLoginType()
		{
			return SoaringLoginType.Soaring;
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
			return (long)(DateTime.UtcNow - SoaringTime.Epoch).TotalSeconds - SystemTimeSinceBootTime();
		}

		public virtual long SystemTimeSinceBootTime()
		{
			return (long)Time.realtimeSinceStartup;
		}
	}

	private static SoaringPlatform sInstance;

	private SoaringPlatformDelegate platformDelegate;

	private SoaringPlatformType platformType;

	public static SoaringLoginType PreferedLoginType
	{
		get
		{
			return sInstance.platformDelegate.PreferedLoginType();
		}
	}

	public static SoaringPlatformType Platform
	{
		get
		{
			return sInstance.platformType;
		}
	}

	public static bool PlatformLoginAvailable
	{
		get
		{
			return sInstance.platformDelegate.PlatformLoginAvailable();
		}
	}

	public static bool PlatformLoginAuthenticated
	{
		get
		{
			return sInstance.platformDelegate.PlatformAuthenticated();
		}
	}

	public static string DeviceID
	{
		get
		{
			return sInstance.platformDelegate.DeviceID();
		}
	}

	public static string PlatformUserID
	{
		get
		{
			return sInstance.platformDelegate.PlatformID();
		}
	}

	public static string PlatformUserAlias
	{
		get
		{
			return sInstance.platformDelegate.PlatformAlias();
		}
	}

	public static string PushNotificationsProtocol
	{
		get
		{
			return sInstance.platformDelegate.PushNotificationsProtocol();
		}
	}

	public static string PrimaryPlatformName
	{
		get
		{
			return sInstance.platformDelegate.PlatformName();
		}
	}

	private SoaringPlatform(SoaringPlatformType platform)
	{
		if (platform == SoaringPlatformType.System)
		{
			switch (Application.platform)
			{
			case RuntimePlatform.Android:
				platform = SoaringPlatformType.Android;
				break;
			case RuntimePlatform.IPhonePlayer:
				platform = SoaringPlatformType.iPhone;
				break;
			case RuntimePlatform.BB10Player:
				platform = SoaringPlatformType.Blackberry;
				break;
			case RuntimePlatform.WindowsPlayer:
				platform = SoaringPlatformType.Windows;
				break;
			case RuntimePlatform.OSXPlayer:
				platform = SoaringPlatformType.OSX;
				break;
			}
		}
		switch (platform)
		{
		case SoaringPlatformType.Amazon:
			platformDelegate = new SoaringPlatformAmazon();
			break;
		case SoaringPlatformType.Android:
			platformDelegate = new SoaringPlatformAndroid();
			break;
		case SoaringPlatformType.iPhone:
			platformDelegate = new SBMIIOSPlatformModule();
			break;
		case SoaringPlatformType.Windows:
			platformDelegate = new SoaringPlatformWindows();
			break;
		case SoaringPlatformType.OSX:
			platformDelegate = new SoaringPlatformOSX();
			break;
		default:
			SoaringDebug.Log("Unknown Platform", LogType.Error);
			break;
		}
		platformType = platform;
		if (platformDelegate != null)
		{
			platformDelegate.Init();
		}
	}

	internal static void Init(SoaringPlatformType platform)
	{
		if (sInstance == null)
		{
			sInstance = new SoaringPlatform(platform);
		}
	}

	public static void SetPlatformUserData(string userID, string userAlias)
	{
		sInstance.platformDelegate.SetPlatformUserData(userID, userAlias);
	}

	public static SoaringDictionary GenerateDeviceDictionary()
	{
		return sInstance.platformDelegate.GenerateDeviceDictionary();
	}

	public static bool AuthenticatedPlatformUser(SoaringContext context)
	{
		return sInstance.platformDelegate.PlatformAuthenticate(context);
	}

	public static SoaringPlatformDelegate GetDelegate()
	{
		return sInstance.platformDelegate;
	}

	public static bool OpenURL(string url)
	{
		return sInstance.platformDelegate.OpenURL(url);
	}

	public static bool SendEmail(string subject, string body, string email)
	{
		return sInstance.platformDelegate.SendEmail(subject, body, email);
	}

	public static long SystemBootTime()
	{
		return sInstance.platformDelegate.SystemBootTime();
	}

	public static long SystemTimeSinceBootTime()
	{
		return sInstance.platformDelegate.SystemTimeSinceBootTime();
	}
}
