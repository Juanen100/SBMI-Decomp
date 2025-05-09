public class SoaringDelegate : SoaringObjectBase
{
	public SoaringDelegate()
		: base(IsType.Object)
	{
	}

	public virtual void InternetStateChange(bool state)
	{
	}

	public virtual void OnInitializing(bool success, SoaringError error, SoaringDictionary data)
	{
	}

	public virtual void OnAuthorize(bool success, SoaringError error, SoaringPlayer player, SoaringContext context)
	{
	}

	public virtual void OnLookupUser(bool success, SoaringError error, SoaringContext context)
	{
	}

	public virtual void OnGenerateUserName(bool success, SoaringError error, string nextTag, SoaringContext context)
	{
	}

	public virtual void OnRegisterUser(bool success, SoaringError error, SoaringPlayer player, SoaringContext context)
	{
	}

	public virtual void OnRetrieveUserProfile(bool succes, SoaringError error, SoaringUser user, SoaringContext context)
	{
	}

	public virtual void OnUpdatingUserProfile(bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
	}

	public virtual void OnRetrieveInvitationCode(bool success, SoaringError error, string invite_code)
	{
	}

	public virtual void OnSavingSessionData(bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
	}

	public virtual void OnFindUser(bool success, SoaringError error, SoaringUser[] users, SoaringContext context)
	{
	}

	public virtual void OnRequestFriend(bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
	}

	public virtual void OnRemoveFriend(bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
	}

	public virtual void OnApplyInviteCode(bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
	}

	public virtual void OnUpdateFriendList(bool success, SoaringError error, SoaringUser[] users, SoaringContext context)
	{
	}

	public virtual void OnComponentFinished(bool success, string module, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
	}

	public virtual void OnRequestingSessionData(bool success, SoaringError error, SoaringArray sessions, SoaringDictionary raw_data, SoaringContext context)
	{
	}

	public virtual void OnCheckUserRewards(bool success, SoaringError error, SoaringArray rewards)
	{
	}

	public virtual void OnRedeemUserReward(bool success, SoaringError error, SoaringDictionary data)
	{
	}

	public virtual void OnServerTimeUpdated(bool success, SoaringError error, long timestamp, SoaringContext context)
	{
	}

	public virtual void OnCheckMessages(bool success, SoaringError error, SoaringArray messages)
	{
	}

	public virtual void OnSendMessage(bool success, SoaringError error, SoaringMessage message)
	{
	}

	public virtual void OnMessageStateChanged(bool success, SoaringError error, SoaringDictionary data)
	{
	}

	public virtual void OnFileDownloadUpdate(SoaringState state, SoaringError error, object data, SoaringContext context)
	{
	}

	public virtual void OnFileVersionsUpdated(SoaringState state, SoaringError error, object data)
	{
	}

	public virtual void OnBlockGameSession(bool forceBlock, float version, float minvVer, float maxVer, string message)
	{
	}

	public virtual void OnAdServed(bool success, SoaringAdData adData, SoaringAdServerState state, SoaringContext context)
	{
	}

	public virtual void OnPasswordReset(bool success, SoaringError error)
	{
	}

	public virtual void OnPasswordResetConfirmed(bool success, SoaringError error)
	{
	}

	public virtual void OnPasswordChanged(bool success, SoaringError error, SoaringContext context)
	{
	}

	public virtual void OnDeviceRegistered(bool success, SoaringError error, SoaringContext context)
	{
	}

	public virtual void OnRecieptValidated(bool success, SoaringError error, SoaringContext context)
	{
	}

	public virtual void OnRetrieveProducts(bool success, SoaringError error, SoaringPurchasable[] purchasables, SoaringContext context)
	{
	}

	public virtual void OnRetrievePurchases(bool success, SoaringError error, SoaringPurchase[] purchases, SoaringContext context)
	{
	}

	public virtual void OnSaveStat(bool success, bool anonymous, SoaringError error, SoaringContext context)
	{
	}

	public virtual void OnRetrieveCampaign(bool success, SoaringError error, SoaringArray campaigns, SoaringContext context)
	{
	}

	public virtual void OnPlayerConflict(SoaringPlayerResolver player, SoaringPlayerResolver.SoaringPlayerData platform_player, SoaringPlayerResolver.SoaringPlayerData last_player, SoaringPlayerResolver.SoaringPlayerData device_player, SoaringContext context)
	{
	}

	public virtual void OnRecievedEvent(SoaringEvents manager, SoaringEvent soaringEv)
	{
	}
}
