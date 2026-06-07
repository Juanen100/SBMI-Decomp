using System;

public static class Soaring
{
	public static SoaringDelegate Delegate
	{
		get
		{
			return null;
		}
	}

	public static SoaringLoginType PreferedDeviceLogin
	{
		get
		{
			return default(SoaringLoginType);
		}
	}

	public static string ServerUrl
	{
		get
		{
			return null;
		}
	}

	public static string ServerContentUrl
	{
		get
		{
			return null;
		}
	}

	public static SoaringPlayer Player
	{
		get
		{
			return null;
		}
	}

	public static SoaringCommunityEventManager CommunityEventManager
	{
		get
		{
			return null;
		}
	}

	public static bool IsOnline
	{
		get
		{
			return false;
		}
	}

	public static bool IsInitialized
	{
		get
		{
			return false;
		}
	}

	public static bool IsAuthorized
	{
		get
		{
			return false;
		}
	}

	public static bool HasAuthorizedCredentials
	{
		get
		{
			return false;
		}
	}

	public static void StartSoaring(string gameID, SoaringDelegate del, SoaringMode mode, SoaringPlatformType platform = SoaringPlatformType.System)
	{
	}

	public static void StopSoaring()
	{
	}

	public static void SetGameVersion(Version version)
	{
	}

	public static void AddDelegate(SoaringDelegate del)
	{
	}

	public static void RemoveDelegate(SoaringDelegate del)
	{
	}

	public static void RemoveDelegate(Type type)
	{
	}

	public static void GenerateUniqueNewUserName(SoaringContext context = null)
	{
	}

	public static void GenerateInviteCode()
	{
	}

	public static void Login(SoaringContext context = null)
	{
	}

	public static void Login(string platformID, SoaringLoginType loginType, SoaringContext context = null)
	{
	}

	public static void Login(string userName, string password, SoaringContext context = null)
	{
	}

	public static void Login(string userName, string password, SoaringLoginType loginType, SoaringContext context = null)
	{
	}

	public static void LookupUser(string platformID, SoaringContext context = null)
	{
	}

	public static void LookupUser(string platformID, SoaringLoginType loginType, SoaringContext context = null)
	{
	}

	public static void LookupUser(SoaringArray identifiers, SoaringContext context = null)
	{
	}

	public static void RetreiveUserProfile(SoaringContext context = null)
	{
	}

	public static void RetreiveUserProfile(string userID, SoaringContext context = null)
	{
	}

	public static void RegisterLiteUser(string userName, SoaringContext context = null)
	{
	}

	public static void RegisterLiteUser(string userName, string platformID, SoaringLoginType loginType, SoaringContext context = null)
	{
	}

	public static void RegisterUser(string userName, string password, SoaringContext context = null)
	{
	}

	public static void RegisterUser(string userName, string password, string platformID, SoaringLoginType loginType, SoaringContext context = null)
	{
	}

	public static void RegisterUser(string userName, string password, bool userCreated, SoaringContext context = null)
	{
	}

	public static void RequestinviteCode()
	{
	}

	public static void RequestFriendship(string tag, string email, string userid, SoaringContext context = null)
	{
	}

	public static void RequestFriendships(SoaringArray userIds, SoaringContext context = null)
	{
	}

	public static void RequestFriendshipWithCode(string code, SoaringContext context = null)
	{
	}

	public static void RemoveFriendship(string tag, string email, string userid, SoaringContext context = null)
	{
	}

	public static void UpdateFriendsListWithLastSettings(SoaringContext context = null)
	{
	}

	public static void UpdateFriendsListWithLastSettings(int start, int end, SoaringContext context = null)
	{
	}

	public static void UpdateFriendList(string order = null, string mode = null, SoaringContext context = null)
	{
	}

	public static void UpdateFriendList(int start, int end, string order = null, string mode = null, SoaringContext context = null)
	{
	}

	public static void UpdateUserProfile(SoaringDictionary custom, SoaringContext context = null)
	{
	}

	public static void UpdateUserProfile(SoaringDictionary userData, SoaringDictionary custom, SoaringContext context = null)
	{
	}

	public static void UpdateUserProfile(string tag, string status, SoaringContext context = null)
	{
	}

	public static void UpdateUserFacebookInfo(string userId, string icon, SoaringContext context = null)
	{
	}

	public static void FindUser(string tag, string email, string userId, string facebookId, SoaringContext context = null)
	{
	}

	public static void FindUsers(SoaringArray tag, SoaringArray email, SoaringArray userIds, SoaringArray facebookIds, SoaringContext context = null)
	{
	}

	public static void SendSessionData(SoaringDictionary data, SoaringContext context = null)
	{
	}

	public static void SendSessionData(string tag, SoaringSession.SessionType sessionType, SoaringDictionary data, SoaringContext context = null)
	{
	}

	public static void SendSessionData(SoaringSession.SessionType sessionType, string sessionID, SoaringDictionary data, SoaringContext context = null)
	{
	}

	public static void ApplyInviteCode(string invite_code)
	{
	}

	public static void RequestSessionData(SoaringContext context = null)
	{
	}

	public static void RequestSessionData(string session, long timeStamp = 0L, SoaringContext context = null)
	{
	}

	public static void RequestSessionData(SoaringArray identifiers, SoaringDictionary sort, SoaringContext context = null)
	{
	}

	public static void UpdateServerTime(SoaringContext context = null)
	{
	}

	public static void CheckUserRewards()
	{
	}

	public static void RedeemUserReward(SoaringArray arr)
	{
	}

	public static void RedeemUserReward(SoaringCoupon coupon)
	{
	}

	public static void CheckUnreadMessages()
	{
	}

	public static void SendMessage(SoaringMessage message)
	{
	}

	public static void MarkMessageAsRead(SoaringMessage message)
	{
	}

	public static void MarkMessageAsRead(SoaringArray messages)
	{
	}

	public static string SoaringAddress(string addresKey)
	{
		return null;
	}

	public static void CheckFilesForUpdates(bool updateFiles)
	{
	}

	public static void SetVersionedFileRepo(string versioning, string contentRepo = null, string fileRepo = null, string versionName = null)
	{
	}

	public static void RequestSoaringAdvert(string adverName = null, bool displayOnComplete = false, SoaringContext context = null)
	{
	}

	public static bool SoaringAdvertAvailable(string adverName = null)
	{
		return false;
	}

	public static bool SoaringDisplayAdvert(string adverName = null)
	{
		return false;
	}

	public static void RequestCampaign(SoaringContext context = null)
	{
	}

	public static void SetAdServerURL(string url)
	{
	}

	public static void ResetPassword(string verifyUsername, string verifyEmail)
	{
	}

	public static void ConfirmResetPassword(string verifyUserName, string confirmCode, string newPassword)
	{
	}

	public static void ChangePassword(string oldPassword, string newPassword, SoaringContext context = null)
	{
	}

	public static void RegisterDevicePushToken(string tokenID)
	{
	}

	public static void SaveStat(string key, SoaringObjectBase value)
	{
	}

	public static void SaveStat(SoaringArray entries)
	{
	}

	public static void SaveAnonymousStat(SoaringArray entries)
	{
	}

	public static void SaveAnonymousStat(string keys, SoaringDictionary entries)
	{
	}

	public static void FireEvent(string eventName, SoaringDictionary custom)
	{
	}

	public static void RequestProducts(string store, string language, SoaringContext context = null)
	{
	}

	public static void RequestPurchases(string store = null, SoaringContext context = null)
	{
	}

	public static void ValidatePurchasableReciept(string reciept, SoaringPurchasable purchasable, string storeName = null, bool isProduction = true, string userID = null, SoaringContext context = null)
	{
	}

	public static void LogOut()
	{
	}
}
