public class SoaringPlatformWindows : SoaringPlatform.SoaringPlatformDelegate
{
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

	public override string DeviceID()
	{
		return null;
	}

	public override SoaringDictionary GenerateDeviceDictionary()
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

	public override bool OpenPath(string path)
	{
		return false;
	}
}
