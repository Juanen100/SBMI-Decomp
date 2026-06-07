using UnityEngine;

public class SoaringPlatformAndroid : SoaringPlatform.SoaringPlatformDelegate
{
	private AndroidJavaClass cls_Soaring;

	private string mAndroidID;

	private string mIMEI;

	private string[] mMacAddresses;

	private long mTotalMemory;

	public string IMEI
	{
		get
		{
			return null;
		}
	}

	public string AndroidID
	{
		get
		{
			return null;
		}
	}

	public string[] MacAddresses
	{
		get
		{
			return null;
		}
	}

	public long TotalMemory
	{
		get
		{
			return 0L;
		}
	}

	public override void Init()
	{
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

	public override bool OpenURL(string url)
	{
		return false;
	}

	public override bool SendEmail(string subject, string body, string email)
	{
		return false;
	}

	public void OpenDialog(string title, string body, string button)
	{
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
