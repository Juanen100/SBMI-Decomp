using System;

internal class SoaringDelegateArray : SoaringDelegate
{
	private SoaringArray<SoaringDelegate> mDelegateArray;

	public SoaringArray<SoaringDelegate> Modules()
	{
		return null;
	}

	public void RegisterDelegate(SoaringDelegate del)
	{
	}

	public void UnregisterDelegate(SoaringDelegate del)
	{
	}

	public void UnregisterDelegate(Type type)
	{
	}

	public bool UseMainResponder(SoaringContext context)
	{
		return false;
	}

	public override void InternetStateChange(bool state)
	{
	}

	public override void OnInitializing(bool success, SoaringError error, SoaringDictionary data)
	{
	}

	public override void OnAuthorize(bool success, SoaringError error, SoaringPlayer player, SoaringContext context)
	{
	}

	public override void OnLookupUser(bool success, SoaringError error, SoaringContext context)
	{
	}

	public override void OnGenerateUserName(bool success, SoaringError error, string nextTag, SoaringContext context)
	{
	}

	public override void OnRegisterUser(bool success, SoaringError error, SoaringPlayer player, SoaringContext context)
	{
	}

	public override void OnRetrieveUserProfile(bool succes, SoaringError error, SoaringUser user, SoaringContext context)
	{
	}

	public override void OnUpdatingUserProfile(bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
	}

	public override void OnSavingSessionData(bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
	}

	public override void OnRequestingSessionData(bool success, SoaringError error, SoaringArray session_data, SoaringDictionary raw_data, SoaringContext context)
	{
	}

	public override void OnRetrieveInvitationCode(bool success, SoaringError error, string invite_code)
	{
	}

	public override void OnFindUser(bool success, SoaringError error, SoaringUser[] users, SoaringContext context)
	{
	}

	public override void OnRequestFriend(bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
	}

	public override void OnRemoveFriend(bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
	}

	public override void OnApplyInviteCode(bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
	}

	public override void OnUpdateFriendList(bool success, SoaringError error, SoaringUser[] users, SoaringContext context)
	{
	}

	public override void OnComponentFinished(bool success, string module, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
	}

	public override void OnCheckUserRewards(bool success, SoaringError error, SoaringArray rewards)
	{
	}

	public override void OnRedeemUserReward(bool success, SoaringError error, SoaringDictionary data)
	{
	}

	public override void OnServerTimeUpdated(bool success, SoaringError error, long timestamp, SoaringContext context)
	{
	}

	public override void OnCheckMessages(bool success, SoaringError error, SoaringArray messages)
	{
	}

	public override void OnSendMessage(bool success, SoaringError error, SoaringMessage message)
	{
	}

	public override void OnMessageStateChanged(bool success, SoaringError error, SoaringDictionary data)
	{
	}

	public override void OnFileDownloadUpdate(SoaringState state, SoaringError error, object data, SoaringContext context)
	{
	}

	public override void OnFileVersionsUpdated(SoaringState state, SoaringError error, object data)
	{
	}

	public override void OnBlockGameSession(bool forceBlock, float version, float minvVer, float maxVer, string message)
	{
	}

	public override void OnAdServed(bool success, SoaringAdData adData, SoaringAdServerState state, SoaringContext context)
	{
	}

	public override void OnPasswordReset(bool success, SoaringError error)
	{
	}

	public override void OnPasswordResetConfirmed(bool success, SoaringError error)
	{
	}

	public override void OnPasswordChanged(bool success, SoaringError error, SoaringContext context)
	{
	}

	public override void OnDeviceRegistered(bool success, SoaringError error, SoaringContext context)
	{
	}

	public override void OnRecieptValidated(bool success, SoaringError error, SoaringContext context)
	{
	}

	public override void OnSaveStat(bool success, bool anonymous, SoaringError error, SoaringContext context)
	{
	}

	public override void OnPlayerConflict(SoaringPlayerResolver player, SoaringPlayerResolver.SoaringPlayerData platform_player, SoaringPlayerResolver.SoaringPlayerData last_player, SoaringPlayerResolver.SoaringPlayerData device_player, SoaringContext context)
	{
	}

	public override void OnRetrievePurchases(bool success, SoaringError error, SoaringPurchase[] purchases, SoaringContext context)
	{
	}

	public override void OnRetrieveProducts(bool success, SoaringError error, SoaringPurchasable[] purchasables, SoaringContext context)
	{
	}

	public override void OnRetrieveCampaign(bool success, SoaringError error, SoaringArray campaigns, SoaringContext context)
	{
	}

	public override void OnRecievedEvent(SoaringEvents manager, SoaringEvent soaringEv)
	{
	}
}
