using System;

public static class Soaring
{
	public static SoaringDelegate Delegate
	{
		get
		{
			return SoaringInternal.Delegate;
		}
	}

	public static SoaringLoginType PreferedDeviceLogin
	{
		get
		{
			return SoaringPlatform.PreferedLoginType;
		}
	}

	public static string ServerUrl
	{
		get
		{
			return SoaringInternal.instance.CurrentServer;
		}
	}

	public static string ServerContentUrl
	{
		get
		{
			return SoaringInternal.instance.CurrentContentURL;
		}
	}

	public static SoaringPlayer Player
	{
		get
		{
			return SoaringInternal.instance.Player;
		}
	}

	public static SoaringCommunityEventManager CommunityEventManager
	{
		get
		{
			return SoaringInternal.instance.CommunityEventManager;
		}
	}

	public static bool IsOnline
	{
		get
		{
			return SoaringInternal.IsOnline;
		}
	}

	public static bool IsInitialized
	{
		get
		{
			return SoaringInternal.instance.IsInitialized();
		}
	}

	public static bool IsAuthorized
	{
		get
		{
			return SoaringInternal.instance.IsAuthorized();
		}
	}

	public static bool HasAuthorizedCredentials
	{
		get
		{
			return SoaringInternal.instance.HasAuthorizedCredentials();
		}
	}

	public static void StartSoaring(string gameID, SoaringDelegate del, SoaringMode mode, SoaringPlatformType platform = SoaringPlatformType.System)
	{
		SoaringInternal.instance.Initialize(gameID, del, mode, platform);
	}

	public static void StopSoaring()
	{
	}

	public static void SetGameVersion(Version version)
	{
		SoaringInternal.SetGameVersion(version);
	}

	public static void AddDelegate(SoaringDelegate del)
	{
		SoaringInternal.instance.RegisterDelegate(del);
	}

	public static void RemoveDelegate(SoaringDelegate del)
	{
		SoaringInternal.instance.UnregisterDelegate(del);
	}

	public static void RemoveDelegate(Type type)
	{
		SoaringInternal.instance.UnregisterDelegate(type);
	}

	public static void GenerateUniqueNewUserName(SoaringContext context = null)
	{
		SoaringInternal.instance.GenerateUniqueNewUserName(false, context);
	}

	public static void GenerateInviteCode()
	{
		SoaringInternal.instance.GenerateInviteCode();
	}

	public static void Login(SoaringContext context = null)
	{
		SoaringInternal.instance.Login(context);
	}

	public static void Login(string platformID, SoaringLoginType loginType, SoaringContext context = null)
	{
		SoaringInternal.instance.Login(null, null, platformID, loginType, false, context);
	}

	public static void Login(string userName, string password, SoaringContext context = null)
	{
		SoaringInternal.instance.Login(userName, password, null, SoaringLoginType.Soaring, false, context);
	}

	public static void Login(string userName, string password, SoaringLoginType loginType, SoaringContext context = null)
	{
		SoaringInternal.instance.Login(userName, password, null, loginType, false, context);
	}

	public static void LookupUser(string platformID, SoaringContext context = null)
	{
		LookupUser(platformID, PreferedDeviceLogin, context);
	}

	public static void LookupUser(string platformID, SoaringLoginType loginType, SoaringContext context = null)
	{
		SoaringInternal.instance.LookupUser(platformID, loginType, context);
	}

	public static void LookupUser(SoaringArray identifiers, SoaringContext context = null)
	{
		SoaringInternal.instance.LookupUser(identifiers, context);
	}

	public static void RetreiveUserProfile(SoaringContext context = null)
	{
		SoaringInternal.instance.RetrieveUserProfile(null, context);
	}

	public static void RetreiveUserProfile(string userID, SoaringContext context = null)
	{
		SoaringInternal.instance.RetrieveUserProfile(userID, context);
	}

	public static void RegisterLiteUser(string userName, SoaringContext context = null)
	{
		SoaringInternal.instance.RegisterUser(userName, null, null, true, SoaringLoginType.Soaring, false, context);
	}

	public static void RegisterLiteUser(string userName, string platformID, SoaringLoginType loginType, SoaringContext context = null)
	{
		SoaringInternal.instance.RegisterUser(userName, null, platformID, true, loginType, false, context);
	}

	public static void RegisterUser(string userName, string password, SoaringContext context = null)
	{
		SoaringInternal.instance.RegisterUser(userName, password, null, false, SoaringLoginType.Soaring, false, context);
	}

	public static void RegisterUser(string userName, string password, string platformID, SoaringLoginType loginType, SoaringContext context = null)
	{
		SoaringInternal.instance.RegisterUser(userName, password, platformID, false, loginType, false, context);
	}

	public static void RegisterUser(string userName, string password, bool userCreated, SoaringContext context = null)
	{
		SoaringInternal.instance.RegisterUser(userName, password, null, !userCreated, SoaringLoginType.Soaring, false, context);
	}

	public static void RequestinviteCode()
	{
		SoaringInternal.instance.GenerateInviteCode();
	}

	public static void RequestFriendship(string tag, string email, string userid, SoaringContext context = null)
	{
		SoaringInternal.instance.RequestFriendship(tag, email, userid, context);
	}

	public static void RequestFriendships(SoaringArray userIds, SoaringContext context = null)
	{
		SoaringInternal.instance.RequestFriendships(userIds, context);
	}

	public static void RequestFriendshipWithCode(string code, SoaringContext context = null)
	{
		SoaringInternal.instance.RequestFriendshipWithCode(code, context);
	}

	public static void RemoveFriendship(string tag, string email, string userid, SoaringContext context = null)
	{
		SoaringInternal.instance.RemoveFriendship(tag, email, userid, context);
	}

	public static void UpdateFriendsListWithLastSettings(SoaringContext context = null)
	{
		SoaringInternal.instance.UpdateFriendsListWithLastSettings(-1, -1, context);
	}

	public static void UpdateFriendsListWithLastSettings(int start, int end, SoaringContext context = null)
	{
		SoaringInternal.instance.UpdateFriendsListWithLastSettings(start, end, context);
	}

	public static void UpdateFriendList(string order = null, string mode = null, SoaringContext context = null)
	{
		SoaringInternal.instance.UpdateFriendsList(-1, -1, order, mode, context);
	}

	public static void UpdateFriendList(int start, int end, string order = null, string mode = null, SoaringContext context = null)
	{
		SoaringInternal.instance.UpdateFriendsList(start, end, order, mode, context);
	}

	public static void UpdateUserProfile(SoaringDictionary custom, SoaringContext context = null)
	{
		SoaringInternal.instance.UpdatePlayerProfile(custom, context);
	}

	public static void UpdateUserProfile(SoaringDictionary userData, SoaringDictionary custom, SoaringContext context = null)
	{
		SoaringInternal.instance.UpdatePlayerProfile(userData, custom, context);
	}

	public static void UpdateUserProfile(string tag, string status, SoaringContext context = null)
	{
		SoaringInternal.instance.UpdatePlayerProfile(tag, status, context);
	}

	public static void UpdateUserFacebookInfo(string userId, string icon, SoaringContext context = null)
	{
		SoaringInternal.instance.UpdatePlayerFacebookID(userId, icon, context);
	}

	public static void FindUser(string tag, string email, string userId, string facebookId, SoaringContext context = null)
	{
		SoaringInternal.instance.FindUser(tag, email, userId, facebookId, context);
	}

	public static void FindUsers(SoaringArray tag, SoaringArray email, SoaringArray userIds, SoaringArray facebookIds, SoaringContext context = null)
	{
		SoaringInternal.instance.FindUsers(tag, email, userIds, facebookIds, context);
	}

	public static void SendSessionData(SoaringDictionary data, SoaringContext context = null)
	{
		SoaringInternal.instance.SendSessionData(data, context);
	}

	public static void SendSessionData(string tag, SoaringSession.SessionType sessionType, SoaringDictionary data, SoaringContext context = null)
	{
		SoaringInternal.instance.SendSessionData(tag, sessionType, data, context);
	}

	public static void SendSessionData(SoaringSession.SessionType sessionType, string sessionID, SoaringDictionary data, SoaringContext context = null)
	{
		SoaringInternal.instance.SendSessionData(sessionType, sessionID, data, context);
	}

	public static void ApplyInviteCode(string invite_code)
	{
		SoaringInternal.instance.ApplyInviteCode(invite_code, null);
	}

	public static void RequestSessionData(SoaringContext context = null)
	{
		SoaringInternal.instance.RequestSessionData(Player.UserID, 0L, context);
	}

	public static void RequestSessionData(string session, long timeStamp = 0, SoaringContext context = null)
	{
		SoaringInternal.instance.RequestSessionData(session, timeStamp, context);
	}

	public static void RequestSessionData(SoaringArray identifiers, SoaringDictionary sort, SoaringContext context = null)
	{
		SoaringInternal.instance.RequestSessionData(identifiers, sort, context);
	}

	public static void UpdateServerTime(SoaringContext context = null)
	{
		SoaringInternal.instance.UpdateServerTime(context);
	}

	public static void CheckUserRewards()
	{
		SoaringInternal.instance.CheckUserRewards();
	}

	public static void RedeemUserReward(SoaringArray arr)
	{
		SoaringInternal.instance.RedeemRewardCoupons(arr);
	}

	public static void RedeemUserReward(SoaringCoupon coupon)
	{
		SoaringInternal.instance.RedeemRewardCoupons(coupon);
	}

	public static void CheckUnreadMessages()
	{
		SoaringInternal.instance.CheckUnreadMessages();
	}

	public static void SendMessage(SoaringMessage message)
	{
		SoaringInternal.instance.SendMessage(message);
	}

	public static void MarkMessageAsRead(SoaringMessage message)
	{
		SoaringInternal.instance.MarkMessageAsRead(message);
	}

	public static void MarkMessageAsRead(SoaringArray messages)
	{
		SoaringInternal.instance.MarkMessageAsRead(messages);
	}

	public static string SoaringAddress(string addresKey)
	{
		return SoaringInternal.instance.GetSoaringAddress(addresKey);
	}

	public static void CheckFilesForUpdates(bool updateFiles)
	{
		SoaringInternal.instance.CheckFilesForUpdates(updateFiles);
	}

	public static void SetVersionedFileRepo(string versioning, string contentRepo = null, string fileRepo = null, string versionName = null)
	{
		SoaringInternal.instance.Versions.SetVersionServer(versioning, contentRepo, fileRepo, versionName);
	}

	public static void RequestSoaringAdvert(string adverName = null, bool displayOnComplete = false, SoaringContext context = null)
	{
		SoaringInternal.instance.AdServer.RequestAd(adverName, displayOnComplete, context);
	}

	public static bool SoaringAdvertAvailable(string adverName = null)
	{
		return SoaringInternal.instance.AdServer.AdAvailable(adverName);
	}

	public static bool SoaringDisplayAdvert(string adverName = null)
	{
		return SoaringInternal.instance.AdServer.DisplayAd(adverName);
	}

	public static void RequestCampaign(SoaringContext context = null)
	{
		SoaringInternal.instance.RequestCampaign(context);
	}

	public static void SetAdServerURL(string url)
	{
		SoaringInternal.instance.AdServer.SetAdServerURL(url);
	}

	public static void ResetPassword(string verifyUsername, string verifyEmail)
	{
		SoaringInternal.instance.ResetPassword(verifyUsername, verifyEmail);
	}

	public static void ConfirmResetPassword(string verifyUserName, string confirmCode, string newPassword)
	{
		SoaringInternal.instance.ResetPasswordConfirm(verifyUserName, confirmCode, newPassword);
	}

	public static void ChangePassword(string oldPassword, string newPassword, SoaringContext context = null)
	{
		SoaringInternal.instance.ChangePassword(oldPassword, newPassword, context);
	}

	public static void RegisterDevicePushToken(string tokenID)
	{
		SoaringInternal.instance.RegisterDevice(tokenID, null);
	}

	public static void SaveStat(string key, SoaringObjectBase value)
	{
		SoaringInternal.instance.SaveStat(key, value);
	}

	public static void SaveStat(SoaringArray entries)
	{
		SoaringInternal.instance.SaveStat(entries);
	}

	public static void SaveAnonymousStat(SoaringArray entries)
	{
		SoaringInternal.instance.SaveAnonymousStat(entries);
	}

	public static void SaveAnonymousStat(string keys, SoaringDictionary entries)
	{
		SoaringInternal.instance.SaveAnonymousStat(keys, entries);
	}

	public static void FireEvent(string eventName, SoaringDictionary custom)
	{
		SoaringInternal.instance.FireEvent(eventName, custom);
	}

	public static void RequestProducts(string store, string language, SoaringContext context = null)
	{
		SoaringInternal.instance.RequestPurchasables(store, language, context);
	}

	public static void RequestPurchases(string store = null, SoaringContext context = null)
	{
		SoaringInternal.instance.RequestPurchases(store, context);
	}

	public static void ValidatePurchasableReciept(string reciept, SoaringPurchasable purchasable, string storeName = null, bool isProduction = true, string userID = null, SoaringContext context = null)
	{
		SoaringInternal.instance.ValidatePurchaseReciept(reciept, purchasable, storeName, userID, isProduction, context);
	}

	public static void LogOut()
	{
	}
}
