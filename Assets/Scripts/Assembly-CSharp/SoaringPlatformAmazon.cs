public class SoaringPlatformAmazon : SoaringPlatformAndroid
{
	private string mProfileID = string.Empty;

	private string mProfileAlias = string.Empty;

	public override SoaringLoginType PreferedLoginType()
	{
		return SoaringLoginType.Amazon;
	}

	public override string PlatformName()
	{
		return "amazon";
	}

	public override void SetPlatformUserData(string userID, string userAlias)
	{
		mProfileID = userID;
		mProfileAlias = userAlias;
	}

	public override void Init()
	{
		base.Init();
		GameCircleManager.getInstance();
	}

	public override bool PlatformLoginAvailable()
	{
		return true;
	}

	public override bool PlatformAuthenticated()
	{
		return AGSClient.IsServiceReady();
	}

	public override bool PlatformAuthenticate(SoaringContext context)
	{
		bool flag = false;
		if (context == null)
		{
			context = new SoaringContext();
		}
		context.Name = "login";
		mProfileID = string.Empty;
		mProfileAlias = string.Empty;
		try
		{
			SoaringInternal.instance.PushContextEvent(context);
			RegisterServiceEvent();
			AGSClient.Init(false, true, false);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public override string PlatformID()
	{
		return mProfileID;
	}

	public override string PlatformAlias()
	{
		return mProfileAlias;
	}

	private void RegisterServiceEvent()
	{
		AGSClient.ServiceReadyEvent += ServiceReadyHandler;
		AGSClient.ServiceNotReadyEvent += ServiceNotReadyHandler;
	}

	private void UnServiceEvent()
	{
		AGSClient.ServiceReadyEvent -= ServiceReadyHandler;
		AGSClient.ServiceNotReadyEvent -= ServiceNotReadyHandler;
	}

	private void ServiceReadyHandler()
	{
		UnServiceEvent();
		SubscribeToProfileEvents();
		AGSProfilesClient.RequestLocalPlayerProfile();
	}

	private void ServiceNotReadyHandler(string error)
	{
		UnServiceEvent();
		callback_comlete_failed(error);
	}

	private void SubscribeToProfileEvents()
	{
		AGSProfilesClient.PlayerAliasReceivedEvent += PlayerAliasReceived;
		AGSProfilesClient.PlayerAliasFailedEvent += PlayerAliasFailed;
	}

	private void UnsubscribeFromProfileEvents()
	{
		AGSProfilesClient.PlayerAliasReceivedEvent -= PlayerAliasReceived;
		AGSProfilesClient.PlayerAliasFailedEvent -= PlayerAliasFailed;
	}

	private void PlayerAliasReceived(AGSProfile profile)
	{
		UnsubscribeFromProfileEvents();
		SetPlatformUserData(profile.playerId, profile.alias);
		callback_comlete_success();
	}

	private void PlayerAliasFailed(string errorMessage)
	{
		UnsubscribeFromProfileEvents();
		callback_comlete_failed(errorMessage);
	}

	private void callback_comlete_failed(string error)
	{
		SoaringDebug.Log("SoaringPlatformAmazon: error: " + error);
		SoaringInternal.instance.mWebQueue.onExternalMessage("{\"call\":\"login\",\"status\":\"false\"}");
	}

	private void callback_comlete_success()
	{
		string message = "{\"call\":\"login\",\"status\":\"true\",\"id\":\"" + mProfileID + "\",\"name\":\"" + mProfileAlias + "\"}";
		SoaringInternal.instance.mWebQueue.onExternalMessage(message);
	}
}
