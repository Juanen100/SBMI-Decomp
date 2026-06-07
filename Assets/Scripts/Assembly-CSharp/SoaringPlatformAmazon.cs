public class SoaringPlatformAmazon : SoaringPlatformAndroid
{
	private string mProfileID;

	private string mProfileAlias;

	public override SoaringLoginType PreferedLoginType()
	{
		return default(SoaringLoginType);
	}

	public override string PlatformName()
	{
		return null;
	}

	public override void SetPlatformUserData(string userID, string userAlias)
	{
	}

	public override void Init()
	{
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

	private void RegisterServiceEvent()
	{
	}

	private void UnServiceEvent()
	{
	}

	private void ServiceReadyHandler()
	{
	}

	private void ServiceNotReadyHandler(string error)
	{
	}

	private void SubscribeToProfileEvents()
	{
	}

	private void UnsubscribeFromProfileEvents()
	{
	}

	private void PlayerAliasReceived(AGSProfile profile)
	{
	}

	private void PlayerAliasFailed(string errorMessage)
	{
	}

	private void callback_comlete_failed(string error)
	{
	}

	private void callback_comlete_success()
	{
	}
}
