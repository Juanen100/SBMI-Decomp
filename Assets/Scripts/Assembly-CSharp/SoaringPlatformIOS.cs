using UnityEngine;

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
			if (!string.IsNullOrEmpty(mUDID))
			{
				return mUDID;
			}
			return mUDID;
		}
	}

	public string MacAddress
	{
		get
		{
			if (!string.IsNullOrEmpty(mMacAddress))
			{
				return mMacAddress;
			}
			return mMacAddress;
		}
	}

	public string Odin1Sha1
	{
		get
		{
			if (!string.IsNullOrEmpty(mOdin1SH1))
			{
				return mOdin1SH1;
			}
			return mOdin1SH1;
		}
	}

	public string Odin1Md5
	{
		get
		{
			if (!string.IsNullOrEmpty(mOdin1MD5))
			{
				return mOdin1MD5;
			}
			return mOdin1MD5;
		}
	}

	public string AdvertisingIdentifier
	{
		get
		{
			return string.Empty;
		}
	}

	public string VendorIdentifier
	{
		get
		{
			if (!string.IsNullOrEmpty(mIDFV))
			{
				return mIDFV;
			}
			return mIDFV;
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
		mADID = string.Empty;
		mUDID = string.Empty;
		mIDFV = string.Empty;
		mOdin1MD5 = string.Empty;
		mOdin1SH1 = string.Empty;
		mMacAddress = string.Empty;
		mPlatformUserID = string.Empty;
		mPlatformUserAlias = string.Empty;
	}

	public override SoaringLoginType PreferedLoginType()
	{
		return SoaringLoginType.GameCenter;
	}

	public override string PlatformName()
	{
		return "ios";
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
		if (context == null)
		{
			context = new SoaringContext();
		}
		context.Name = "login";
		mPlatformUserID = string.Empty;
		mPlatformUserAlias = string.Empty;
		if (!PlatformLoginAvailable() || PlatformAuthenticated())
		{
			return false;
		}
		return true;
	}

	public override void SetPlatformUserData(string userID, string userAlias)
	{
		mPlatformUserID = userID;
		mPlatformUserAlias = userAlias;
	}

	public override string PlatformID()
	{
		if (!string.IsNullOrEmpty(mPlatformUserID))
		{
			return mPlatformUserID;
		}
		return mPlatformUserID;
	}

	public override string PlatformAlias()
	{
		if (!string.IsNullOrEmpty(mPlatformUserAlias))
		{
			return mPlatformUserAlias;
		}
		return mPlatformUserAlias;
	}

	public override string DeviceID()
	{
		string text = VendorIdentifier;
		if (string.IsNullOrEmpty(text))
		{
			text = SystemInfo.deviceUniqueIdentifier;
		}
		return text;
	}

	public override SoaringDictionary GenerateDeviceDictionary()
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		try
		{
			string text = null;
			int num = (int)iOSVersion();
			if (num <= 6)
			{
				string odin1Md = Odin1Md5;
				if (!string.IsNullOrEmpty(odin1Md))
				{
					soaringDictionary.addValue(odin1Md, "odin1");
					text = odin1Md;
				}
				string uDID = UDID;
				if (!string.IsNullOrEmpty(uDID))
				{
					soaringDictionary.addValue(uDID, "udid");
					text = uDID;
				}
			}
			if (num >= 6)
			{
				string vendorIdentifier = VendorIdentifier;
				if (!string.IsNullOrEmpty(vendorIdentifier))
				{
					soaringDictionary.addValue(vendorIdentifier, "idfv");
					text = vendorIdentifier;
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				text = DeviceID();
			}
			soaringDictionary.addValue(text, "deviceId");
			soaringDictionary.addValue(PlatformName(), "platform");
		}
		catch
		{
		}
		return soaringDictionary;
	}

	public override string PushNotificationsProtocol()
	{
		return "apns";
	}

	private float iOSVersion()
	{
		return -1f;
	}

	public override bool OpenURL(string url)
	{
		Application.OpenURL(url);
		return true;
	}

	public override bool SendEmail(string subject, string body, string email)
	{
		return base.SendEmail(subject, body, email);
	}

	public override bool OpenPath(string path)
	{
		return base.OpenPath(path);
	}

	public override long SystemBootTime()
	{
		return base.SystemBootTime();
	}

	public override long SystemTimeSinceBootTime()
	{
		return base.SystemTimeSinceBootTime();
	}
}
