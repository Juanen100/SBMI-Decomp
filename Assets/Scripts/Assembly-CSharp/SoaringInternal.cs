using System;
using UnityEngine;

public class SoaringInternal : SoaringObjectBase
{
	private class SoaringPlayerValue : SoaringValue
	{
		public SoaringPlayerValue(string key)
			: base(0)
		{
		}

		public override string ToJsonString()
		{
			return null;
		}

		public override string ToString()
		{
			return null;
		}
	}

	private class SoaringStashedCall : SoaringObjectBase
	{
		public string ModuleName;

		public SoaringDictionary CallData;

		public SoaringContext Contex;

		public SoaringStashedCall(string name, SoaringDictionary data, SoaringContext context)
			: base(default(IsType))
		{
		}
	}

	private static SoaringMode SOARING_MODE;

	private const string SDK_VERSION = "2.1.0";

	private static string WEB_SDK;

	private static string WEB_CDN;

	private static Version GAME_VERSION;

	private static SoaringLoginType Login_Type;

	private string mFriendsLastMode;

	private string mFriendsLastOrder;

	private SoaringDictionary mSoaringModules;

	private SoaringDictionary mSoaringData;

	private SoaringDelegateArray mSoaringDelegate;

	private SoaringDictionary mEncryptedModules;

	private SoaringEncryption mSoaringEncryption;

	private SoaringPlayer mPlayerData;

	private string mAuthorizationToken;

	private string mGameID;

	private SoaringArray mSoaringStashedCall;

	private static SoaringInternal gInstance;

	private GameObject mSoaringObject;

	public SCWebQueue mWebQueue;

	public LanguageCode mSoaringLanguage;

	private SoaringVersions mVersions;

	private SoaringAddressKeeper mAddressKeeper;

	private SoaringCommunityEventManager mCommunityEventManager;

	private SoaringAnalytics mAnalytics;

	private SoaringAdServer mAdServer;

	private SoaringEvents mSoaringEvents;

	private SoaringDictionary mGamePurchasables;

	private SoaringCampaign mCampaign;

	private bool mIsInitialized;

	private static bool sIsOffline;

	private static bool S_CacheIsOnline;

	private static float S_CheckUpdateTimer;

	public static Version GameVersion
	{
		get
		{
			return null;
		}
	}

	public static SoaringLoginType LoginType
	{
		get
		{
			return default(SoaringLoginType);
		}
	}

	public static SoaringPlatformType PlatformType
	{
		get
		{
			return default(SoaringPlatformType);
		}
	}

	public static bool IsProductionMode
	{
		get
		{
			return false;
		}
	}

	public string CurrentServer
	{
		get
		{
			return null;
		}
	}

	public string CurrentContentURL
	{
		get
		{
			return null;
		}
	}

	internal static SoaringDelegateArray Delegate
	{
		get
		{
			return null;
		}
	}

	public static SoaringEncryption Encryption
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public static string GameID
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

	public static SoaringInternal instance
	{
		get
		{
			return null;
		}
	}

	public SoaringEvents Events
	{
		get
		{
			return null;
		}
	}

	public static SoaringCampaign Campaign
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public SoaringPlayer Player
	{
		get
		{
			return null;
		}
	}

	public SoaringAdServer AdServer
	{
		get
		{
			return null;
		}
	}

	public SoaringCommunityEventManager CommunityEventManager
	{
		get
		{
			return null;
		}
	}

	public SoaringAnalytics Analytics
	{
		get
		{
			return null;
		}
	}

	public SoaringDictionary Purchasables
	{
		get
		{
			return null;
		}
	}

	internal SoaringAddressKeeper AddressesKeeper
	{
		get
		{
			return null;
		}
	}

	internal SoaringVersions Versions
	{
		get
		{
			return null;
		}
	}

	private SoaringInternal()
		: base(default(IsType))
	{
	}

	internal static void SetGameVersion(Version version)
	{
	}

	public void RegisterDelegate(SoaringDelegate deleg)
	{
	}

	public void UnregisterDelegate(SoaringDelegate deleg)
	{
	}

	public void UnregisterDelegate(Type type)
	{
	}

	public bool IsInitialized()
	{
		return false;
	}

	public bool Initialize(string gameID, SoaringDelegate deleg, SoaringMode mode)
	{
		return false;
	}

	public bool Initialize(string gameID, SoaringDelegate deleg, SoaringMode mode, SoaringPlatformType platform)
	{
		return false;
	}

	internal void HandleFinalGameInitialization(bool success)
	{
	}

	internal bool HasAuthorizedCredentials()
	{
		return false;
	}

	internal void ClearSoaringWebQueue()
	{
	}

	private void RestartSoaring()
	{
	}

	private void RegisterModules()
	{
	}

	public void RegisterModule(SoaringModule module)
	{
	}

	public void RegisterModule(SoaringModule module, bool safe)
	{
	}

	public void ClearOfflineMode()
	{
	}

	private static bool UpdateConnectionStatus()
	{
		return false;
	}

	public void TriggerOfflineMode(bool trigger)
	{
	}

	private void CheckForSoaringAddresses()
	{
	}

	public void Update(float deltaTime)
	{
	}

	public void HandleOnApplicationPaused(bool paused)
	{
	}

	public void HandleOnApplicationQuit()
	{
	}

	public bool IsAuthorized()
	{
		return false;
	}

	public bool CallModule(string moduleName, SoaringDictionary data, SoaringContext context)
	{
		return false;
	}

	internal bool ValidateUserNameLength(string userName)
	{
		return false;
	}

	internal bool ValidateUserName(string userName, SoaringLoginType type)
	{
		return false;
	}

	internal SoaringDictionary GenerateAppDataDictionary()
	{
		return null;
	}

	internal SoaringDictionary GenerateDeviceDataDictionary()
	{
		return null;
	}

	private void BeginHandshake()
	{
	}

	public void BeginHandshake(SoaringContextDelegate responder)
	{
	}

	internal void RegisterUser(string userName, string password, string platformID, bool liteUser, SoaringLoginType type, bool internalRegister, SoaringContext context)
	{
	}

	private bool _IsAsciiLetterOrDigit(char c)
	{
		return false;
	}

	internal void Login(string userName, string password, string platformID, SoaringLoginType type, bool forceInternalRegister, SoaringContext context)
	{
	}

	internal void LookupUser(string platformID, SoaringLoginType loginType, SoaringContext context)
	{
	}

	internal void LookupUser(SoaringArray identifiers, SoaringContext context)
	{
	}

	private bool ForceLoginWithSaveCredentials()
	{
		return false;
	}

	internal void Login(SoaringContext context)
	{
	}

	internal void HandleLogin(SoaringLoginType type, bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
	}

	internal string GeneratePassword()
	{
		return null;
	}

	internal void GenerateUniqueNewUserName(bool internalRegister, SoaringContext context)
	{
	}

	internal void RetrieveUserProfile(string userID, SoaringContext context)
	{
	}

	internal void GenerateInviteCode()
	{
	}

	internal void ApplyInviteCode(string code, SoaringContext context)
	{
	}

	internal void UpdatePlayerProfile(SoaringDictionary custom, SoaringContext context)
	{
	}

	internal void UpdatePlayerProfile(string tag, string status, SoaringContext context)
	{
	}

	internal void UpdatePlayerProfile(SoaringDictionary userData, SoaringDictionary custom, SoaringContext context)
	{
	}

	internal void UpdatePlayerFacebookID(string facebookID, string icon, SoaringContext context)
	{
	}

	internal void FindUser(string tag, string email, string userId, string facebookId, SoaringContext context)
	{
	}

	internal void FindUsers(SoaringArray tag, SoaringArray email, SoaringArray userId, SoaringArray facebookId, SoaringContext context)
	{
	}

	internal void FindUserWithData(SoaringObjectBase tag, SoaringObjectBase email, SoaringObjectBase userId, SoaringObjectBase facebookId, SoaringContext context)
	{
	}

	internal void RequestFriendships(SoaringArray userId, SoaringContext context)
	{
	}

	internal void RequestFriendship(string tag, string email, string userId, SoaringContext context)
	{
	}

	private void RequestFriendship(string tag, string email, SoaringObjectBase userId, object phld, SoaringContext context)
	{
	}

	internal void RequestFriendshipWithCode(string code, SoaringContext context)
	{
	}

	internal void RemoveFriendship(string tag, string email, string userId, SoaringContext context)
	{
	}

	internal void SendSessionData(SoaringDictionary data, SoaringContext context)
	{
	}

	internal void SendSessionData(string tag, SoaringSession.SessionType sessionType, SoaringDictionary data, SoaringContext context)
	{
	}

	internal void SendSessionData(SoaringSession.SessionType sessionType, string sessionID, SoaringDictionary data, SoaringContext context)
	{
	}

	internal void RequestSessionData(string searchLabel, long timestamp, SoaringContext context)
	{
	}

	internal void RequestSessionData(SoaringArray identifiers, SoaringDictionary sort, SoaringContext context)
	{
	}

	internal void ValidatePurchaseReciept(string reciept, SoaringPurchasable purchasable, string storeName, string userID, bool isProduction, SoaringContext context)
	{
	}

	internal void RequestPurchasables(string store, string language, SoaringContext context)
	{
	}

	internal void RequestPurchases(string store, SoaringContext context)
	{
	}

	internal void CheckUserRewards()
	{
	}

	internal void UpdateFriendsListWithLastSettings(int startRange, int endRange, SoaringContext context)
	{
	}

	internal void UpdateFriendsList(int startRange, int endRange, string order, string mode, SoaringContext context)
	{
	}

	internal void UpdateServerTime(SoaringContext context)
	{
	}

	internal void RedeemRewardCoupons(SoaringCoupon coupons)
	{
	}

	internal void RegisterDevice(string device_token, SoaringContext context)
	{
	}

	internal void RedeemRewardCoupons(SoaringArray coupons)
	{
	}

	internal void CheckUnreadMessages()
	{
	}

	internal void SendMessage(SoaringMessage message)
	{
	}

	internal void FireEvent(string eventName, SoaringDictionary custom, SoaringContext context = null)
	{
	}

	internal void MarkMessageAsRead(SoaringMessage message)
	{
	}

	internal void MarkMessageAsRead(SoaringArray message)
	{
	}

	internal void ResetPassword(string username, string email)
	{
	}

	internal void ResetPasswordConfirm(string username, string confirmCode, string password)
	{
	}

	internal void ChangePassword(string oldPassword, string newPassword, SoaringContext context)
	{
	}

	internal void RequestCampaign(SoaringContext context)
	{
	}

	internal void SaveStat(string key, SoaringObjectBase value)
	{
	}

	internal void SaveStat(SoaringArray entries)
	{
	}

	internal void SaveAnonymousStat(string key, SoaringObjectBase value)
	{
	}

	internal void SaveAnonymousStat(SoaringArray entries)
	{
	}

	internal void internal_SaveStat(string key, SoaringObjectBase value, SoaringContext context)
	{
	}

	internal void internal_SaveStat(SoaringArray entries, SoaringContext context)
	{
	}

	internal void internal_SaveAnonymousStat(string key, SoaringDictionary value, SoaringContext context)
	{
	}

	internal void internal_SaveAnonymousStat(SoaringArray entries, SoaringContext context)
	{
	}

	internal bool ValidateEmailFormat(string email)
	{
		return false;
	}

	internal void CheckFilesForUpdates(bool updateFiles)
	{
	}

	internal bool PushCall(SoaringDictionary callData)
	{
		return false;
	}

	public void PushContextEvent(SoaringContext context)
	{
	}

	internal void UpdatePlayerData(SoaringDictionary data)
	{
	}

	internal void UpdatePlayerData(SoaringDictionary data, bool clearData)
	{
	}

	internal void SetSoaringInternalData(SoaringDictionary data)
	{
	}

	private void CheckModulesForSecureConnection()
	{
	}

	internal string GetSoaringAddress(string key)
	{
		return null;
	}

	internal void DownloadFileWithSoaring(string name, string url, string path, SoaringContext context)
	{
	}

	internal void DownloadFileWithSoaring(string name, string url, string path, SCWebQueue.SCDownloadCallback callback)
	{
	}

	internal void DownloadFileWithSoaring(string name, string url, string path, SCWebQueue.SCDownloadCallback callback, SoaringContext context)
	{
	}

	internal void HandleStashedCalls()
	{
	}

	internal string PlatformKeyWithLoginType(SoaringLoginType type, bool soaringPlatformDefault)
	{
		return null;
	}

	public static string PlatformKeyAbriviationWithLoginType(SoaringLoginType type, bool soaringPlatformDefault)
	{
		return null;
	}

	public static SoaringLoginType PlatformKeyAbriviationWithTag(string userID)
	{
		return default(SoaringLoginType);
	}

	private string PrimaryPlatformName()
	{
		return null;
	}

	private SoaringError CreateInvalidAuthCodeError()
	{
		return null;
	}

	private SoaringError CreateInvalidCredentialsError(string str)
	{
		return null;
	}
}
