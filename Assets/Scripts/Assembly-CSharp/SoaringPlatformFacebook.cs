public class SoaringPlatformFacebook : SoaringPlatform.SoaringPlatformDelegate
{
	private string facebookId;

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

	public override string PlatformID()
	{
		return null;
	}

	public override string PlatformAlias()
	{
		return null;
	}

	public override SoaringLoginType PreferedLoginType()
	{
		return default(SoaringLoginType);
	}

	public override string DeviceID()
	{
		return null;
	}

	public override bool PlatformAuthenticate(SoaringContext context)
	{
		return false;
	}

	public override string PushNotificationsProtocol()
	{
		return null;
	}

	private void InitCallback()
	{
	}

	private void OnHideUnity(bool isGameShown)
	{
	}

	private void callback_comlete_failed(string error)
	{
	}

	private void callback_comlete_success()
	{
	}
}
