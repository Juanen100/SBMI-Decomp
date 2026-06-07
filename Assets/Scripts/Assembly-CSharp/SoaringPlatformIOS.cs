public class SoaringPlatformIOS : SoaringPlatform.SoaringPlatformDelegate
{
	private string mADID;

	private string mUDID;

	private string mIDFV;

	private string mOdin1MD5;

	private string mOdin1SH1;

	private string mMacAddress;

	private string mPlatformUserID;

	private string mPlatformUserAlias;

	public string UDID
	{
		get
		{
			return null;
		}
	}

	public string MacAddress
	{
		get
		{
			return null;
		}
	}

	public string Odin1Sha1
	{
		get
		{
			return null;
		}
	}

	public string Odin1Md5
	{
		get
		{
			return null;
		}
	}

	public string AdvertisingIdentifier
	{
		get
		{
			return null;
		}
	}

	public string VendorIdentifier
	{
		get
		{
			return null;
		}
	}

	public bool AdvertisingIdentifierEnabled
	{
		get
		{
			return false;
		}
	}

	public override void Init()
	{
	}

	public override SoaringLoginType PreferedLoginType()
	{
		return default(SoaringLoginType);
	}

	public override string PlatformName()
	{
		return null;
	}

	public override bool PlatformLoginAvailable()
	{
		return false;
	}

	public override bool PlatformAuthenticated()
	{
		return false;
	}

	public override bool PlatformAuthenticate(SoaringContext context)
	{
		return false;
	}

	public override void SetPlatformUserData(string userID, string userAlias)
	{
	}

	public override string PlatformID()
	{
		return null;
	}

	public override string PlatformAlias()
	{
		return null;
	}

	public override string DeviceID()
	{
		return null;
	}

	public override SoaringDictionary GenerateDeviceDictionary()
	{
		return null;
	}

	public override string PushNotificationsProtocol()
	{
		return null;
	}

	private float iOSVersion()
	{
		return 0f;
	}

	public override bool OpenURL(string url)
	{
		return false;
	}

	public override bool SendEmail(string subject, string body, string email)
	{
		return false;
	}

	public override bool OpenPath(string path)
	{
		return false;
	}

	public override long SystemBootTime()
	{
		return 0L;
	}

	public override long SystemTimeSinceBootTime()
	{
		return 0L;
	}
}
