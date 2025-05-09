using System;
using MTools;
using UnityEngine;

public class SoaringInternal : SoaringObjectBase
{
	private class SoaringPlayerValue : SoaringValue
	{
		public SoaringPlayerValue(string key)
			: base(key)
		{
		}

		public override string ToJsonString()
		{
			return "\"" + ToString() + "\"";
		}

		public override string ToString()
		{
			return Soaring.Player.GetUserInfo(StringVal);
		}
	}

	private class SoaringStashedCall : SoaringObjectBase
	{
		public string ModuleName;

		public SoaringDictionary CallData;

		public SoaringContext Contex;

		public SoaringStashedCall(string name, SoaringDictionary data, SoaringContext context)
			: base(IsType.Object)
		{
			ModuleName = name;
			CallData = data;
			Contex = context;
		}
	}

	private const string SDK_VERSION = "2.1.0";

	private static SoaringMode SOARING_MODE = SoaringMode.Development;

	private static string WEB_SDK = null;

	private static string WEB_CDN = null;

	private static Version GAME_VERSION = new Version(0, 0, 0);

	private static SoaringLoginType Login_Type = SoaringLoginType.Soaring;

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

	private static SoaringInternal gInstance = null;

	private GameObject mSoaringObject;

	public SCWebQueue mWebQueue;

	public LanguageCode mSoaringLanguage = LanguageCode.EN;

	private SoaringVersions mVersions;

	private SoaringAddressKeeper mAddressKeeper;

	private SoaringCommunityEventManager mCommunityEventManager;

	private SoaringAnalytics mAnalytics;

	private SoaringAdServer mAdServer;

	private SoaringEvents mSoaringEvents;

	private SoaringDictionary mGamePurchasables;

	private SoaringCampaign mCampaign;

	private bool mIsInitialized;

	private static bool sIsOffline = false;

	private static bool S_CacheIsOnline = false;

	private static float S_CheckUpdateTimer = 0f;

	public static Version GameVersion
	{
		get
		{
			return GAME_VERSION;
		}
	}

	public static SoaringLoginType LoginType
	{
		get
		{
			return Login_Type;
		}
	}

	public static SoaringPlatformType PlatformType
	{
		get
		{
			return SoaringPlatform.Platform;
		}
	}

	public static bool IsProductionMode
	{
		get
		{
			return SOARING_MODE == SoaringMode.Production;
		}
	}

	public string CurrentServer
	{
		get
		{
			return WEB_SDK;
		}
	}

	public string CurrentContentURL
	{
		get
		{
			return WEB_CDN;
		}
	}

	internal static SoaringDelegateArray Delegate
	{
		get
		{
			if (instance.mSoaringDelegate == null)
			{
				instance.mSoaringDelegate = new SoaringDelegateArray();
			}
			return instance.mSoaringDelegate;
		}
	}

	public static SoaringEncryption Encryption
	{
		get
		{
			return instance.mSoaringEncryption;
		}
		set
		{
			instance.mSoaringEncryption = value;
		}
	}

	public static string GameID
	{
		get
		{
			return instance.mGameID;
		}
	}

	public static bool IsOnline
	{
		get
		{
			return UpdateConnectionStatus();
		}
	}

	public static SoaringInternal instance
	{
		get
		{
			if (gInstance == null)
			{
				gInstance = new SoaringInternal();
			}
			return gInstance;
		}
	}

	public SoaringEvents Events
	{
		get
		{
			return mSoaringEvents;
		}
	}

	public static SoaringCampaign Campaign
	{
		get
		{
			return instance.mCampaign;
		}
		set
		{
			instance.mCampaign = value;
		}
	}

	public SoaringPlayer Player
	{
		get
		{
			return mPlayerData;
		}
	}

	public SoaringAdServer AdServer
	{
		get
		{
			return mAdServer;
		}
	}

	public SoaringCommunityEventManager CommunityEventManager
	{
		get
		{
			return mCommunityEventManager;
		}
	}

	public SoaringAnalytics Analytics
	{
		get
		{
			return mAnalytics;
		}
	}

	public SoaringDictionary Purchasables
	{
		get
		{
			return mGamePurchasables;
		}
	}

	internal SoaringAddressKeeper AddressesKeeper
	{
		get
		{
			return mAddressKeeper;
		}
	}

	internal SoaringVersions Versions
	{
		get
		{
			return mVersions;
		}
	}

	private SoaringInternal()
		: base(IsType.Management)
	{
		mPlayerData = new SoaringPlayer();
		mSoaringModules = new SoaringDictionary();
		mSoaringData = new SoaringDictionary();
		mGamePurchasables = new SoaringDictionary();
		mSoaringData.addValue(new SoaringPlayerValue("authToken"), "authToken");
		mSoaringData.addValue(this, "SCInternal");
		mCommunityEventManager = new SoaringCommunityEventManager();
	}

	internal static void SetGameVersion(Version version)
	{
		GAME_VERSION = version;
	}

	public void RegisterDelegate(SoaringDelegate deleg)
	{
		if (deleg != null)
		{
			if (mSoaringDelegate == null)
			{
				mSoaringDelegate = new SoaringDelegateArray();
			}
			mSoaringDelegate.RegisterDelegate(deleg);
		}
	}

	public void UnregisterDelegate(SoaringDelegate deleg)
	{
		if (deleg != null)
		{
			if (mSoaringDelegate == null)
			{
				mSoaringDelegate = new SoaringDelegateArray();
			}
			mSoaringDelegate.UnregisterDelegate(deleg);
		}
	}

	public void UnregisterDelegate(Type type)
	{
		if (mSoaringDelegate == null)
		{
			mSoaringDelegate = new SoaringDelegateArray();
		}
		mSoaringDelegate.UnregisterDelegate(type);
	}

	public bool IsInitialized()
	{
		return mIsInitialized;
	}

	public bool Initialize(string gameID, SoaringDelegate deleg, SoaringMode mode)
	{
		return Initialize(gameID, deleg, mode, SoaringPlatformType.System);
	}

	public bool Initialize(string gameID, SoaringDelegate deleg, SoaringMode mode, SoaringPlatformType platform)
	{
		try
		{
			RegisterDelegate(deleg);
			if (string.IsNullOrEmpty(gameID))
			{
				Delegate.OnInitializing(false, "Invalid Game ID", null);
				return false;
			}
			if (!string.IsNullOrEmpty(mGameID))
			{
				Delegate.OnInitializing(false, "Soaring is Already Initialized", null);
				return false;
			}
			SoaringInternalProperties.Load();
			if (SoaringInternalProperties.ForceOfflineModeUser)
			{
				TriggerOfflineMode(true);
			}
			SoaringPlatform.Init(platform);
			SOARING_MODE = mode;
			if (SOARING_MODE == SoaringMode.Production)
			{
				WEB_SDK = SoaringInternalProperties.SoaringProductionURL;
				WEB_CDN = SoaringInternalProperties.SoaringProductionCDN;
			}
			else if (SOARING_MODE == SoaringMode.Testing)
			{
				WEB_SDK = SoaringInternalProperties.SoaringTestingURL;
				WEB_CDN = SoaringInternalProperties.SoaringTestingCDN;
			}
			else
			{
				WEB_SDK = SoaringInternalProperties.SoaringDevelopmentURL;
				WEB_CDN = SoaringInternalProperties.SoaringDevelopmentCDN;
			}
			if (string.IsNullOrEmpty(WEB_SDK))
			{
				Delegate.OnInitializing(false, "Invalid Web URL", null);
				return false;
			}
			mPlayerData.CanSaveUserCredentials = SoaringInternalProperties.SaveUserCredentials;
			mGameID = gameID;
			mSoaringData.addValue(gameID, "gameId");
			if (mSoaringObject == null)
			{
				Camera main = Camera.main;
				if (main != null)
				{
					mSoaringObject = main.gameObject;
				}
				if (mSoaringObject == null)
				{
					mSoaringObject = new GameObject("Soaring");
					UnityEngine.Object.DontDestroyOnLoad(mSoaringObject);
				}
			}
			mWebQueue = mSoaringObject.GetComponent<SCWebQueue>();
			if (mWebQueue == null)
			{
				mWebQueue = mSoaringObject.AddComponent<SCWebQueue>();
				mWebQueue.Initialize("2.1.0");
			}
			if (ForceLoginWithSaveCredentials())
			{
				mPlayerData.ClearSavedCredentials();
			}
			RegisterModules();
			mSoaringEvents = new SoaringEvents();
			if (SoaringInternalProperties.EnableVersions)
			{
				mVersions = new SoaringVersions(WEB_CDN);
			}
			if (SoaringInternalProperties.EnableAddressKeeper)
			{
				mAddressKeeper = new SoaringAddressKeeper();
				CheckForSoaringAddresses();
			}
			if (SoaringInternalProperties.EnableAdServer)
			{
				mAdServer = new SoaringAdServer();
				mAdServer.SetAdServerURL(WEB_CDN);
			}
			if (SoaringInternalProperties.EnableServerTimeVersions)
			{
				SoaringTime.Register();
			}
			if (SoaringInternalProperties.EnableAnalytics && mAnalytics == null)
			{
				mAnalytics = new SoaringAnalytics();
				mAnalytics.Initialize();
			}
			if (SoaringInternalProperties.SecureCommunication && !SoaringInternalProperties.ForceOfflineModeUser)
			{
				if (mSoaringStashedCall == null)
				{
					mSoaringStashedCall = new SoaringArray();
				}
				BeginHandshake();
			}
			else
			{
				HandleFinalGameInitialization(true);
			}
		}
		catch (Exception ex)
		{
			string text = ex.Message;
			if (text == null)
			{
				text = string.Empty;
			}
			SoaringDebug.Log("Initialization Failed: " + text + "\n" + ex.StackTrace, LogType.Error);
			TriggerOfflineMode(true);
			Delegate.OnInitializing(false, null, null);
		}
		return true;
	}

	internal void HandleFinalGameInitialization(bool success)
	{
		mIsInitialized = true;
		Delegate.OnInitializing(success, null, null);
		if (SoaringInternalProperties.LoginOnInitialize)
		{
			mPlayerData.Load();
		}
		if (success)
		{
			HandleStashedCalls();
		}
	}

	internal bool HasAuthorizedCredentials()
	{
		if (mPlayerData == null)
		{
			return false;
		}
		SoaringDebug.Log("HasAuthorizedCredentials: " + SoaringPlayer.ValidCredentials + " Save: " + mPlayerData.CanSaveUserCredentials);
		return SoaringPlayer.ValidCredentials && mPlayerData.CanSaveUserCredentials;
	}

	internal void ClearSoaringWebQueue()
	{
		if (mWebQueue != null)
		{
			mWebQueue.ClearConnections();
		}
	}

	private void RestartSoaring()
	{
		ClearSoaringWebQueue();
	}

	private void RegisterModules()
	{
		if (SoaringInternalProperties.SecureCommunication)
		{
			RegisterModule(new SoaringHandshakeGetKeyModule(), false);
			RegisterModule(new SoaringHandshakeTestKeyModule(), false);
			RegisterModule(new SoaringHandshakeFinishKeyModule(), false);
		}
		RegisterModule(new SoaringLoginUserModule(), false);
		RegisterModule(new SoaringCreateUserModule(), false);
		RegisterModule(new SoaringGenerateUserModule(), false);
		RegisterModule(new SoaringGenerateInviteCodeModule(), false);
		RegisterModule(new SoaringUpdateUserModule(), false);
		RegisterModule(new SoaringApplyInviteCodeModule(), false);
		RegisterModule(new SoaringSearchUsersModule(), false);
		RegisterModule(new SoaringRetrieveFriendsModule(), false);
		RegisterModule(new SoaringSaveSessionModule(), false);
		RegisterModule(new SoaringRetrieveSessionModule(), false);
		RegisterModule(new SoaringRemoveFriendModule(), false);
		RegisterModule(new SoaringAddFriendModule(), false);
		RegisterModule(new SoaringCheckRewardModule(), false);
		RegisterModule(new SoaringRedeemRewardModule(), false);
		RegisterModule(new SoaringRetrieveMessagesModule(), false);
		RegisterModule(new SoaringAddFriendWithTagModule(), false);
		RegisterModule(new SoaringSendMessageModule(), false);
		RegisterModule(new SoaringMarkMessageReadModule(), false);
		RegisterModule(new SoaringRemoveFriendWithTagModule(), false);
		RegisterModule(new SoaringRetrieveUserProfileModule(), false);
		RegisterModule(new SoaringDownloadModule(), false);
		RegisterModule(new SoaringResetPasswordModule(), false);
		RegisterModule(new SoaringChangePasswordModule(), false);
		RegisterModule(new SoaringRenewPasswordModule(), false);
		RegisterModule(new SoaringRegisterDeviceModule(), false);
		RegisterModule(new SoaringLookupUserModule(), false);
		if (SoaringInternalProperties.EnableAnalytics)
		{
			RegisterModule(new SoaringSaveStatModule(), false);
			RegisterModule(new SoaringAnonymousSaveStatModule(), false);
		}
		RegisterModule(new SoaringVerifyServerRecieptModule(), false);
		RegisterModule(new SoaringRetrieveProductModule(), false);
		RegisterModule(new SoaringRetrievePurchasesModule(), false);
		RegisterModule(new SoaringRetrieveABCampaigndModule(), false);
		RegisterModule(new SoaringFireEventModule(), false);
	}

	public void RegisterModule(SoaringModule module)
	{
		RegisterModule(module, true);
	}

	public void RegisterModule(SoaringModule module, bool safe)
	{
		if (module == null)
		{
			return;
		}
		string text = module.ModuleName();
		if (!string.IsNullOrEmpty(text))
		{
			if (safe)
			{
				mSoaringModules.addValue(module, text);
			}
			else
			{
				mSoaringModules.addValue_unsafe(module, text);
			}
		}
	}

	public void ClearOfflineMode()
	{
		S_CheckUpdateTimer = -1f;
		sIsOffline = false;
	}

	private static bool UpdateConnectionStatus()
	{
		if (sIsOffline)
		{
			return false;
		}
		if (S_CheckUpdateTimer <= 0f || Time.realtimeSinceStartup - S_CheckUpdateTimer > 5f)
		{
			S_CacheIsOnline = true;
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				S_CacheIsOnline = false;
			}
			else if (Application.internetReachability != NetworkReachability.ReachableViaCarrierDataNetwork && Application.internetReachability != NetworkReachability.ReachableViaLocalAreaNetwork)
			{
				S_CacheIsOnline = false;
			}
			else
			{
				NetworkPlayer player = Network.player;
				if (player.ipAddress == "127.0.0.1" || player.ipAddress == "0.0.0.0")
				{
					S_CacheIsOnline = false;
				}
			}
			S_CheckUpdateTimer = Time.realtimeSinceStartup;
		}
		return S_CacheIsOnline;
	}

	public void TriggerOfflineMode(bool trigger)
	{
		if (sIsOffline != trigger && !SoaringInternalProperties.ForceOfflineModeUser)
		{
			SoaringDebug.Log("Soaring: TriggerOfflineMode: " + trigger, LogType.Warning);
			sIsOffline = trigger;
			Soaring.Delegate.InternetStateChange(sIsOffline);
		}
	}

	private void CheckForSoaringAddresses()
	{
		if (!string.IsNullOrEmpty(mGameID))
		{
			SoaringDictionary data = new SoaringDictionary();
			CallModule("retrieveGameLinks", data, null);
		}
	}

	public void Update(float deltaTime)
	{
		if (mAnalytics != null)
		{
			mAnalytics.Update(deltaTime);
		}
	}

	public void HandleOnApplicationPaused(bool paused)
	{
		try
		{
			if (paused)
			{
				S_CheckUpdateTimer = 0f;
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("SoaringInternal: HandleOnApplicationPaused: " + ex.Message);
		}
	}

	public void HandleOnApplicationQuit()
	{
		if (mAnalytics != null)
		{
			mAnalytics.Shutdown();
		}
	}

	public bool IsAuthorized()
	{
		if (mPlayerData == null)
		{
			return false;
		}
		string authToken = mPlayerData.AuthToken;
		if (string.IsNullOrEmpty(authToken))
		{
			return false;
		}
		return true;
	}

	public bool CallModule(string moduleName, SoaringDictionary data, SoaringContext context)
	{
		if (context == null)
		{
			context = new SoaringContext();
		}
		if (string.IsNullOrEmpty(moduleName))
		{
			Delegate.OnComponentFinished(false, moduleName, "No Module Name Found", null, context);
			return false;
		}
		SoaringModule soaringModule = (SoaringModule)mSoaringModules.objectWithKey(moduleName);
		if (soaringModule == null)
		{
			Delegate.OnComponentFinished(false, moduleName, "No Module Found", null, context);
			return false;
		}
		if (soaringModule.ShouldEncryptCall() && Encryption != null && Encryption.HasExpired())
		{
			if (SoaringEncryption.IsEncryptionAvailable())
			{
				BeginHandshake();
			}
			mSoaringStashedCall.addObject(new SoaringStashedCall(moduleName, data, context));
			return true;
		}
		soaringModule.CallModule(mSoaringData, data, context);
		return true;
	}

	internal bool ValidateUserNameLength(string userName)
	{
		if (string.IsNullOrEmpty(userName))
		{
			return false;
		}
		if (userName.Length < 6 || userName.Length >= 32)
		{
			return false;
		}
		return true;
	}

	internal bool ValidateUserName(string userName, SoaringLoginType type)
	{
		int length = userName.Length;
		for (int i = 0; i < length; i++)
		{
			char c = userName[i];
			if (c < '!' || c > 'z' || c == '*' || (c == '@' && type != SoaringLoginType.Email) || c == '\n' || c == '\0' || c == '$' || c == ' ' || c == '~' || c == ',' || (c == '.' && type != SoaringLoginType.Email) || c == '\'' || c == '"' || c == '&' || c == '-')
			{
				return false;
			}
		}
		return true;
	}

	internal SoaringDictionary GenerateAppDataDictionary()
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringDictionary.addValue(GameVersion.ToString(), "version");
		return soaringDictionary;
	}

	internal SoaringDictionary GenerateDeviceDataDictionary()
	{
		SoaringDictionary soaringDictionary = null;
		if (SoaringInternalProperties.EnableDeviceData)
		{
			soaringDictionary = SoaringPlatform.GenerateDeviceDictionary();
		}
		else
		{
			soaringDictionary = new SoaringDictionary(1);
			soaringDictionary.addValue(PlatformType.ToString().ToLower(), "platform");
		}
		return soaringDictionary;
	}

	private void BeginHandshake()
	{
		if (Encryption != null)
		{
			Encryption.SetEncryptionKey(null);
		}
		SoaringDictionary data = new SoaringDictionary();
		CallModule("handshake_pt1", data, null);
	}

	public void BeginHandshake(SoaringContextDelegate responder)
	{
		if (Encryption != null)
		{
			Encryption.SetEncryptionKey(null);
		}
		SoaringContext soaringContext = new SoaringContext();
		soaringContext.ContextResponder = responder;
		SoaringDictionary data = new SoaringDictionary();
		CallModule("handshake_pt1", data, soaringContext);
	}

	internal void RegisterUser(string userName, string password, string platformID, bool liteUser, SoaringLoginType type, bool internalRegister, SoaringContext context)
	{
		if (!IsInitialized())
		{
			return;
		}
		if (ForceLoginWithSaveCredentials())
		{
			Login(null, null, null, SoaringLoginType.Soaring, false, context);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		if (string.IsNullOrEmpty(userName))
		{
			userName = string.Empty;
		}
		if (string.IsNullOrEmpty(password))
		{
			password = string.Empty;
		}
		if (!ValidateUserNameLength(userName))
		{
			Delegate.OnRegisterUser(false, CreateInvalidCredentialsError("Invalid User Name: Must be between 6 and 32 characters"), null, context);
			return;
		}
		if (!ValidateUserName(userName, type))
		{
			Delegate.OnRegisterUser(false, CreateInvalidCredentialsError("Invalid User Name: Must be composed of A-Z, a-z, 0-9, or _"), null, context);
			return;
		}
		string text = PlatformKeyWithLoginType(type, false);
		if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(platformID))
		{
			soaringDictionary.addValue(platformID, text);
		}
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		soaringDictionary.addValue(liteUser, "autoCreated");
		soaringDictionary.addValue(userName, "tag");
		if (liteUser && type == SoaringLoginType.Soaring && string.IsNullOrEmpty(platformID) && string.IsNullOrEmpty(password))
		{
			password = Application.platform.ToString()[0].ToString();
			int length = userName.Length;
			for (int i = 0; i < length; i++)
			{
				int num = length - 1 - i;
				if (num >= 0)
				{
					char c = (char)(userName[num] + i);
					password += ((!_IsAsciiLetterOrDigit(c)) ? string.Empty : c.ToString());
				}
			}
			if (password.Length < 8)
			{
				for (int j = password.Length; j < 8; j++)
				{
					password += "s";
				}
			}
		}
		if (!liteUser || type == SoaringLoginType.Soaring)
		{
			soaringDictionary.addValue(password, "password");
		}
		if (internalRegister)
		{
			soaringDictionary2.addValue("1", "tregister");
		}
		SoaringDictionary soaringDictionary3 = new SoaringDictionary();
		soaringDictionary.addValue(soaringDictionary3, "custom");
		soaringDictionary3.addValue(GenerateDeviceDataDictionary(), "deviceInfo");
		soaringDictionary3.addValue(GenerateAppDataDictionary(), "appInfo");
		SoaringPlayer.ValidCredentials = false;
		mPlayerData.Save();
		CallModule("registerUser", soaringDictionary, context);
	}

	private bool _IsAsciiLetterOrDigit(char c)
	{
		return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9');
	}

	internal void Login(string userName, string password, string platformID, SoaringLoginType type, bool forceInternalRegister, SoaringContext context)
	{
		if (!IsInitialized())
		{
			return;
		}
		if (ForceLoginWithSaveCredentials())
		{
			userName = SoaringInternalProperties.DeveloperLoginTag;
			password = SoaringInternalProperties.DeveloperLoginPassword;
		}
		string text = PlatformKeyWithLoginType(type, false);
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		if (type == SoaringLoginType.Soaring)
		{
			if (string.IsNullOrEmpty(userName))
			{
				userName = string.Empty;
			}
			soaringDictionary.addValue(userName, "tag");
		}
		if (!string.IsNullOrEmpty(platformID) && text != null)
		{
			soaringDictionary.addValue(platformID, text);
		}
		if (!string.IsNullOrEmpty(password))
		{
			soaringDictionary.addValue(password, "password");
		}
		if (!SoaringPlayer.ValidCredentials || Soaring.Player.UserID != userName || Soaring.Player.Password != password)
		{
			SoaringPlayer.ValidCredentials = false;
			mPlayerData.Save();
		}
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		soaringDictionary.addValue(soaringDictionary2, "custom");
		soaringDictionary2.addValue(GenerateDeviceDataDictionary(), "deviceInfo");
		soaringDictionary2.addValue(GenerateAppDataDictionary(), "appInfo");
		Login_Type = type;
		CallModule("loginUser", soaringDictionary, context);
	}

	internal void LookupUser(string platformID, SoaringLoginType loginType, SoaringContext context)
	{
		SoaringDebug.Log("LookupUser: " + platformID, LogType.Warning);
		if (string.IsNullOrEmpty(platformID))
		{
			Delegate.OnLookupUser(false, CreateInvalidCredentialsError("No PlatformID"), context);
			return;
		}
		string key = "deviceId";
		if (loginType != SoaringLoginType.Soaring && loginType != SoaringLoginType.Device)
		{
			key = PlatformKeyWithLoginType(loginType, false);
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(platformID, key);
		SoaringDictionary soaringDictionary2 = new SoaringDictionary(2);
		soaringDictionary.addValue(soaringDictionary2, "custom");
		soaringDictionary2.addValue(GenerateDeviceDataDictionary(), "deviceInfo");
		soaringDictionary2.addValue(GenerateAppDataDictionary(), "appInfo");
		CallModule("lookupUser", soaringDictionary, context);
	}

	internal void LookupUser(SoaringArray identifiers, SoaringContext context)
	{
		if (identifiers == null)
		{
			Delegate.OnLookupUser(false, CreateInvalidCredentialsError("No PlatformIDs"), context);
			return;
		}
		SoaringDebug.Log("LookupUser: " + identifiers.ToJsonString(), LogType.Warning);
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		SoaringDictionary soaringDictionary2 = new SoaringDictionary(2);
		soaringDictionary.addValue(soaringDictionary2, "custom");
		soaringDictionary.addValue(identifiers, "identifiers");
		soaringDictionary2.addValue(GenerateDeviceDataDictionary(), "deviceInfo");
		soaringDictionary2.addValue(GenerateAppDataDictionary(), "appInfo");
		CallModule("lookupUser", soaringDictionary, context);
	}

	private bool ForceLoginWithSaveCredentials()
	{
		if (!string.IsNullOrEmpty(SoaringInternalProperties.DeveloperLoginTag) && !string.IsNullOrEmpty(SoaringInternalProperties.DeveloperLoginPassword) && SoaringInternalProperties.EnableDeveloperLogin)
		{
			return true;
		}
		return false;
	}

	internal void Login(SoaringContext context)
	{
		if (IsInitialized())
		{
			if (ForceLoginWithSaveCredentials())
			{
				Login(null, null, null, SoaringLoginType.Soaring, false, context);
			}
			else if (HasAuthorizedCredentials())
			{
				Login(mPlayerData.UserTag, mPlayerData.Password, null, SoaringLoginType.Soaring, false, context);
			}
			else if (true)
			{
				GenerateUniqueNewUserName(true, context);
			}
		}
	}

	internal void HandleLogin(SoaringLoginType type, bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
		Delegate.OnAuthorize(success, error, Soaring.Player, context);
	}

	internal string GeneratePassword()
	{
		string text = UnityEngine.Random.Range(1, 9).ToString();
		text += SystemInfo.deviceType.ToString()[0];
		text += UnityEngine.Random.Range(11, 99);
		text += ((UnityEngine.Random.Range(0, 1) != 1) ? "!" : "#");
		text += ((UnityEngine.Random.Range(0, 1) != 1) ? "T" : "s");
		return text + UnityEngine.Random.Range(0, 9);
	}

	internal void GenerateUniqueNewUserName(bool internalRegister, SoaringContext context)
	{
		if (!IsInitialized())
		{
			return;
		}
		Login_Type = SoaringLoginType.Soaring;
		SoaringDictionary data = null;
		if (internalRegister)
		{
			data = new SoaringDictionary();
			SoaringDictionary soaringDictionary = new SoaringDictionary();
			if (context == null)
			{
				context = new SoaringContext();
			}
			soaringDictionary.addValue(GeneratePassword(), "password");
			soaringDictionary.addValue((int)Login_Type, "loginType");
			context.addValue(soaringDictionary, "tregister");
		}
		CallModule("retrieveNextAutogeneratedUserTag", data, context);
	}

	internal void RetrieveUserProfile(string userID, SoaringContext context)
	{
		if (!IsAuthorized())
		{
			Delegate.OnRetrieveUserProfile(false, CreateInvalidAuthCodeError(), null, context);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(mPlayerData.AuthToken, "authToken");
		if (!string.IsNullOrEmpty(userID))
		{
			soaringDictionary.addValue(userID, "userId");
		}
		CallModule("retrieveUserProfile", soaringDictionary, context);
	}

	internal void GenerateInviteCode()
	{
		if (!IsAuthorized())
		{
			Delegate.OnRetrieveInvitationCode(false, CreateInvalidAuthCodeError(), null);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringDictionary.addValue(mPlayerData.AuthToken, "authToken");
		CallModule("retrieveInvitationCode", soaringDictionary, null);
	}

	internal void ApplyInviteCode(string code, SoaringContext context)
	{
		if (!IsAuthorized() || string.IsNullOrEmpty(code))
		{
			Delegate.OnApplyInviteCode(false, CreateInvalidAuthCodeError(), null, context);
			return;
		}
		if (!code.Contains("-"))
		{
			Delegate.OnApplyInviteCode(false, "Invalid Invite Code Format.", null, context);
			return;
		}
		if (code.Length != 9)
		{
			Delegate.OnApplyInviteCode(false, "Invalid Invite Code Length.", null, context);
			return;
		}
		code = code.ToUpper();
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringDictionary.addValue(mPlayerData.AuthToken, "authToken");
		soaringDictionary.addValue(code, "invitationCode");
		CallModule("applyInvitationCode", soaringDictionary, null);
	}

	internal void UpdatePlayerProfile(SoaringDictionary custom, SoaringContext context)
	{
		UpdatePlayerProfile(null, custom, context);
	}

	internal void UpdatePlayerProfile(string tag, string status, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		if (!string.IsNullOrEmpty(tag))
		{
			soaringDictionary.addValue(tag, "tag");
		}
		if (!string.IsNullOrEmpty(status))
		{
			soaringDictionary.addValue(status, "status");
		}
		UpdatePlayerProfile(soaringDictionary, null, context);
	}

	internal void UpdatePlayerProfile(SoaringDictionary userData, SoaringDictionary custom, SoaringContext context)
	{
		if (!IsAuthorized())
		{
			Delegate.OnUpdatingUserProfile(false, CreateInvalidAuthCodeError(), null, context);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		if (userData != null)
		{
			string text = userData.soaringValue("emails");
			if (!string.IsNullOrEmpty(text))
			{
				if (!ValidateEmailFormat(text))
				{
					Delegate.OnUpdatingUserProfile(false, "Invalid Email Format", null, context);
					return;
				}
				SoaringArray soaringArray = new SoaringArray();
				soaringArray.addObject(text);
				soaringDictionary.addValue(soaringArray, "emails");
				userData.removeObjectWithKey("emails");
			}
			for (int i = 0; i < userData.count(); i++)
			{
				SoaringObjectBase val = userData.allValues()[i];
				string key = userData.allKeys()[i];
				soaringDictionary.addValue(val, key);
			}
		}
		if (custom != null)
		{
			soaringDictionary.addValue(custom, "custom");
		}
		soaringDictionary.addValue(mPlayerData.AuthToken, "authToken");
		CallModule("updateUserProfile", soaringDictionary, context);
	}

	internal void UpdatePlayerFacebookID(string facebookID, string icon, SoaringContext context)
	{
		if (!IsAuthorized())
		{
			Delegate.OnUpdatingUserProfile(false, CreateInvalidAuthCodeError(), null, context);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		if (string.IsNullOrEmpty(facebookID))
		{
			Delegate.OnUpdatingUserProfile(false, "Invalid Facebook ID.", null, context);
			return;
		}
		if (!string.IsNullOrEmpty(icon))
		{
			soaringDictionary.addValue(icon, "icon");
		}
		soaringDictionary.addValue(facebookID, "facebookId");
		soaringDictionary.addValue(mPlayerData.AuthToken, "authToken");
		CallModule("updateUserProfile", soaringDictionary, context);
	}

	internal void FindUser(string tag, string email, string userId, string facebookId, SoaringContext context)
	{
		SoaringValue email2 = null;
		if (!string.IsNullOrEmpty(email))
		{
			email2 = new SoaringValue(email);
		}
		SoaringValue userId2 = null;
		if (!string.IsNullOrEmpty(userId))
		{
			userId2 = new SoaringValue(userId);
		}
		SoaringValue facebookId2 = null;
		if (!string.IsNullOrEmpty(facebookId))
		{
			facebookId2 = new SoaringValue(facebookId);
		}
		SoaringValue tag2 = null;
		if (!string.IsNullOrEmpty(tag))
		{
			tag2 = new SoaringValue(tag);
		}
		FindUserWithData(tag2, email2, userId2, facebookId2, context);
	}

	internal void FindUsers(SoaringArray tag, SoaringArray email, SoaringArray userId, SoaringArray facebookId, SoaringContext context)
	{
		FindUserWithData(tag, email, userId, facebookId, context);
	}

	internal void FindUserWithData(SoaringObjectBase tag, SoaringObjectBase email, SoaringObjectBase userId, SoaringObjectBase facebookId, SoaringContext context)
	{
		if (!IsAuthorized())
		{
			Delegate.OnFindUser(false, CreateInvalidAuthCodeError(), null, context);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		if (tag != null)
		{
			soaringDictionary.addValue("tag", "type");
			soaringDictionary.addValue(tag, "value");
		}
		else if (email != null)
		{
			soaringDictionary.addValue("email", "type");
			soaringDictionary.addValue(email, "value");
		}
		else if (userId != null)
		{
			if (userId.Type == IsType.Array)
			{
				soaringDictionary.addValue("userIds", "type");
			}
			else
			{
				soaringDictionary.addValue("userId", "type");
			}
			soaringDictionary.addValue(userId, "value");
		}
		else if (facebookId != null)
		{
			if (facebookId.Type == IsType.Array)
			{
				soaringDictionary.addValue("facebookIds", "type");
			}
			else
			{
				soaringDictionary.addValue("facebookId", "type");
			}
			soaringDictionary.addValue(facebookId, "value");
		}
		soaringDictionary.addValue(mPlayerData.AuthToken, "authToken");
		CallModule("searchUsers", soaringDictionary, context);
	}

	internal void RequestFriendships(SoaringArray userId, SoaringContext context)
	{
		RequestFriendship(null, null, userId, null, context);
	}

	internal void RequestFriendship(string tag, string email, string userId, SoaringContext context)
	{
		SoaringValue userId2 = null;
		if (!string.IsNullOrEmpty(userId))
		{
			userId2 = new SoaringValue(userId);
		}
		RequestFriendship(tag, email, userId2, null, context);
	}

	private void RequestFriendship(string tag, string email, SoaringObjectBase userId, object phld, SoaringContext context)
	{
		if (!IsAuthorized())
		{
			Delegate.OnRequestFriend(false, CreateInvalidAuthCodeError(), null, context);
			return;
		}
		string moduleName = "requestFriendship";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		if (!string.IsNullOrEmpty(tag))
		{
			moduleName = "requestFriendshipWithTag";
			soaringDictionary.addValue(tag, "tag");
		}
		else if (!string.IsNullOrEmpty(email))
		{
			soaringDictionary.addValue(email, "email");
		}
		else if (userId != null)
		{
			soaringDictionary.addValue(userId, "userId");
		}
		soaringDictionary.addValue(mPlayerData.AuthToken, "authToken");
		CallModule(moduleName, soaringDictionary, context);
	}

	internal void RequestFriendshipWithCode(string code, SoaringContext context)
	{
		if (!IsAuthorized())
		{
			Delegate.OnRequestFriend(false, CreateInvalidAuthCodeError(), null, context);
		}
		else if (string.IsNullOrEmpty(code))
		{
			Delegate.OnRequestFriend(false, "Invalid User Code", null, context);
		}
		else if (code.Contains("-"))
		{
			ApplyInviteCode(code, context);
		}
		else
		{
			RequestFriendship(code, null, null, context);
		}
	}

	internal void RemoveFriendship(string tag, string email, string userId, SoaringContext context)
	{
		if (!IsAuthorized())
		{
			Delegate.OnRemoveFriend(false, CreateInvalidAuthCodeError(), null, context);
			return;
		}
		string moduleName = "removeFriendship";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		if (!string.IsNullOrEmpty(tag))
		{
			moduleName = "removeFriendshipWithTag";
			soaringDictionary.addValue(tag, "tag");
		}
		else if (!string.IsNullOrEmpty(email))
		{
			soaringDictionary.addValue(email, "email");
		}
		else if (!string.IsNullOrEmpty(userId))
		{
			soaringDictionary.addValue(userId, "userId");
		}
		soaringDictionary.addValue(mPlayerData.AuthToken, "authToken");
		CallModule(moduleName, soaringDictionary, context);
	}

	internal void SendSessionData(SoaringDictionary data, SoaringContext context)
	{
		SendSessionData(SoaringSession.SessionType.OneWay, null, data, context);
	}

	internal void SendSessionData(string tag, SoaringSession.SessionType sessionType, SoaringDictionary data, SoaringContext context)
	{
		if (!IsAuthorized() || data == null)
		{
			Delegate.OnSavingSessionData(false, CreateInvalidAuthCodeError(), null, context);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(data, "custom");
		soaringDictionary.addValue(SoaringSession.GetSoaringSessionTypeString(sessionType), "sessionType");
		if (!string.IsNullOrEmpty(tag))
		{
			soaringDictionary.addValue(tag, "label");
		}
		soaringDictionary.addValue(mPlayerData.AuthToken, "authToken");
		CallModule("saveGameSession", soaringDictionary, context);
	}

	internal void SendSessionData(SoaringSession.SessionType sessionType, string sessionID, SoaringDictionary data, SoaringContext context)
	{
		if (!IsAuthorized() || data == null)
		{
			Delegate.OnSavingSessionData(false, CreateInvalidAuthCodeError(), null, context);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(data, "custom");
		soaringDictionary.addValue(SoaringSession.GetSoaringSessionTypeString(sessionType), "sessionType");
		if (!string.IsNullOrEmpty(sessionID))
		{
			soaringDictionary.addValue(sessionID, "gameSessionId");
		}
		soaringDictionary.addValue(mPlayerData.AuthToken, "authToken");
		CallModule("saveGameSession", soaringDictionary, context);
	}

	internal void RequestSessionData(string searchLabel, long timestamp, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringDictionary.addValue(searchLabel, "label");
		if (timestamp > 0)
		{
			soaringDictionary.addValue(timestamp, "datetime");
		}
		SoaringArray soaringArray = new SoaringArray(1);
		soaringArray.addObject(soaringDictionary);
		RequestSessionData(soaringArray, null, context);
	}

	internal void RequestSessionData(SoaringArray identifiers, SoaringDictionary sort, SoaringContext context)
	{
		if (!IsAuthorized())
		{
			Delegate.OnRequestingSessionData(false, CreateInvalidAuthCodeError(), null, null, context);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		if (sort != null)
		{
			soaringDictionary.addValue(sort, "sort");
		}
		if (identifiers != null)
		{
			soaringDictionary.addValue(identifiers, "identifiers");
		}
		soaringDictionary.addValue(SoaringSession.GetSoaringSessionQueryTypeString(SoaringSession.QueryType.List2), "queryType");
		soaringDictionary.addValue(SoaringSession.GetSoaringSessionTypeString(SoaringSession.SessionType.PersistantOneWay), "sessionType");
		soaringDictionary.addValue(mPlayerData.AuthToken, "authToken");
		CallModule("retrieveGameSession", soaringDictionary, context);
	}

	internal void ValidatePurchaseReciept(string reciept, SoaringPurchasable purchasable, string storeName, string userID, bool isProduction, SoaringContext context)
	{
		if (!IsAuthorized())
		{
			Delegate.OnRecieptValidated(false, CreateInvalidAuthCodeError(), context);
			return;
		}
		if (string.IsNullOrEmpty(reciept))
		{
			Delegate.OnRecieptValidated(false, "Invalid Reciept: " + reciept, context);
			return;
		}
		if (string.IsNullOrEmpty(purchasable.ProductID))
		{
			Delegate.OnRecieptValidated(false, "Invalid Product ID: " + purchasable.ProductID, context);
			return;
		}
		if (context == null)
		{
			context = "ValidatePurchasableReciept";
		}
		context.addValue(purchasable, "purchasable");
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		if (string.IsNullOrEmpty(storeName))
		{
			storeName = PrimaryPlatformName();
		}
		soaringDictionary.addValue(storeName, "storeName");
		if (SoaringPlatform.Platform == SoaringPlatformType.iPhone)
		{
			soaringDictionary.addValue(reciept, "receipt");
		}
		else
		{
			soaringDictionary.addValue(reciept, "token");
			if (SoaringPlatform.Platform == SoaringPlatformType.Amazon)
			{
				soaringDictionary.addValue(userID, "userId");
				soaringDictionary.addValue(2, "version");
			}
		}
		soaringDictionary.addValue(purchasable.ProductID, "productId");
		soaringDictionary.addValue(purchasable.Amount, "amount");
		if (IsProductionMode && !isProduction)
		{
			soaringDictionary.addValue("staging", "__hostType__");
		}
		CallModule("validateIapReceipt", soaringDictionary, context);
	}

	internal void RequestPurchasables(string store, string language, SoaringContext context)
	{
		if (!IsAuthorized())
		{
			Delegate.OnRetrieveProducts(false, CreateInvalidAuthCodeError(), SoaringRetrieveProductModule.LoadCachedProductData(), context);
			return;
		}
		if (string.IsNullOrEmpty(language))
		{
			Delegate.OnRetrieveProducts(false, "Invalid language", SoaringRetrieveProductModule.LoadCachedProductData(), context);
			return;
		}
		if (string.IsNullOrEmpty(store))
		{
			store = PrimaryPlatformName();
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(store, "storeName");
		soaringDictionary.addValue(language, "language");
		CallModule("retrieveIapProducts", soaringDictionary, context);
	}

	internal void RequestPurchases(string store, SoaringContext context)
	{
		if (!IsAuthorized())
		{
			Delegate.OnRetrievePurchases(false, CreateInvalidAuthCodeError(), null, context);
			return;
		}
		if (string.IsNullOrEmpty(store))
		{
			store = PrimaryPlatformName();
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringDictionary.addValue(store, "storeName");
		CallModule("retrieveIapPurchases", soaringDictionary, context);
	}

	internal void CheckUserRewards()
	{
		if (!IsAuthorized())
		{
			Delegate.OnCheckUserRewards(false, CreateInvalidAuthCodeError(), null);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(mPlayerData.AuthToken, "authToken");
		CallModule("retrieveVirtualGoodCoupons", soaringDictionary, null);
	}

	internal void UpdateFriendsListWithLastSettings(int startRange, int endRange, SoaringContext context)
	{
		UpdateFriendsList(startRange, endRange, mFriendsLastOrder, mFriendsLastMode, context);
	}

	internal void UpdateFriendsList(int startRange, int endRange, string order, string mode, SoaringContext context)
	{
		if (!IsAuthorized())
		{
			Delegate.OnUpdateFriendList(false, CreateInvalidAuthCodeError(), null, context);
			return;
		}
		mFriendsLastOrder = order;
		mFriendsLastMode = mode;
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(mPlayerData.AuthToken, "authToken");
		if (!string.IsNullOrEmpty(order) && !string.IsNullOrEmpty(mode))
		{
			SoaringDictionary soaringDictionary2 = new SoaringDictionary();
			soaringDictionary2.addValue(mode, "field");
			soaringDictionary2.addValue(order, "mode");
			soaringDictionary.addValue(soaringDictionary2, "sort");
		}
		if (startRange != -1 && endRange != -1)
		{
			SoaringArray soaringArray = new SoaringArray(2);
			soaringArray.addObject(startRange);
			soaringArray.addObject(endRange);
			soaringDictionary.addValue(soaringArray, "range");
		}
		CallModule("retrieveFriendsList", soaringDictionary, context);
	}

	internal void UpdateServerTime(SoaringContext context)
	{
		if (!IsAuthorized())
		{
			Delegate.OnServerTimeUpdated(false, CreateInvalidAuthCodeError(), 0L, context);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(mPlayerData.AuthToken, "authToken");
		CallModule("retrieveServerTime", soaringDictionary, context);
	}

	internal void RedeemRewardCoupons(SoaringCoupon coupons)
	{
		SoaringArray soaringArray = null;
		if (coupons != null)
		{
			soaringArray = new SoaringArray(1);
			soaringArray.addObject(coupons);
		}
		RedeemRewardCoupons(soaringArray);
	}

	internal void RegisterDevice(string device_token, SoaringContext context)
	{
		if (!IsAuthorized())
		{
			Delegate.OnDeviceRegistered(false, CreateInvalidAuthCodeError(), context);
			return;
		}
		if (string.IsNullOrEmpty(device_token))
		{
			Delegate.OnDeviceRegistered(false, "Invalid Device Token", context);
			return;
		}
		string pushNotificationsProtocol = SoaringPlatform.PushNotificationsProtocol;
		if (string.IsNullOrEmpty(pushNotificationsProtocol))
		{
			Delegate.OnDeviceRegistered(false, "Unsupported Protocal", context);
			return;
		}
		string text = null;
		string text2 = PlayerPrefs.GetString("trToken", string.Empty);
		if (!string.IsNullOrEmpty(text2))
		{
			text = mPlayerData.UserID + "_" + device_token;
			text = MCommon.CreateStringHash(text);
			if (!string.IsNullOrEmpty(text) && text == text2)
			{
				Delegate.OnDeviceRegistered(true, null, context);
				return;
			}
			text = text2;
		}
		if (string.IsNullOrEmpty(text))
		{
			if (context == null)
			{
				context = new SoaringContext();
			}
			context.addValue(text, "trToken");
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(mPlayerData.AuthToken, "authToken");
		soaringDictionary.addValue(device_token, "token");
		soaringDictionary.addValue(pushNotificationsProtocol, "protocol");
		CallModule("registerDevice", soaringDictionary, context);
	}

	internal void RedeemRewardCoupons(SoaringArray coupons)
	{
		if (!IsAuthorized())
		{
			Delegate.OnRedeemUserReward(false, CreateInvalidAuthCodeError(), null);
			return;
		}
		if (coupons == null)
		{
			Delegate.OnRedeemUserReward(false, "Invalid Coupon", null);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(mPlayerData.AuthToken, "authToken");
		soaringDictionary.addValue(coupons, "coupons");
		CallModule("tearVirtualGoodCoupons", soaringDictionary, null);
	}

	internal void CheckUnreadMessages()
	{
		if (!IsAuthorized())
		{
			Delegate.OnCheckMessages(false, CreateInvalidAuthCodeError(), null);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(mPlayerData.AuthToken, "authToken");
		CallModule("retrieveUnreadMessages", soaringDictionary, null);
	}

	internal void SendMessage(SoaringMessage message)
	{
		if (!IsAuthorized())
		{
			Delegate.OnSendMessage(false, CreateInvalidAuthCodeError(), null);
			return;
		}
		if (message == null)
		{
			Delegate.OnSendMessage(false, "Invalid Messages", null);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(mPlayerData.AuthToken, "authToken");
		soaringDictionary.addValue(message, "messages");
		CallModule("sendMessage", soaringDictionary, null);
	}

	internal void FireEvent(string eventName, SoaringDictionary custom, SoaringContext context = null)
	{
		if (!IsAuthorized())
		{
			Delegate.OnSendMessage(false, CreateInvalidAuthCodeError(), null);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(mPlayerData.AuthToken, "authToken");
		soaringDictionary.addValue(eventName, "event_name");
		soaringDictionary.addValue(custom, "custom");
		CallModule("fireEvent", soaringDictionary, context);
	}

	internal void MarkMessageAsRead(SoaringMessage message)
	{
		if (message == null)
		{
			Delegate.OnMessageStateChanged(false, "Invalid Messages", null);
			return;
		}
		SoaringArray soaringArray = new SoaringArray(1);
		soaringArray.addObject(message.MessageID);
		MarkMessageAsRead(soaringArray);
	}

	internal void MarkMessageAsRead(SoaringArray message)
	{
		if (!IsAuthorized())
		{
			Delegate.OnMessageStateChanged(false, CreateInvalidAuthCodeError(), null);
			return;
		}
		if (message == null)
		{
			Delegate.OnMessageStateChanged(false, "Invalid Messages", null);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(mPlayerData.AuthToken, "authToken");
		soaringDictionary.addValue(message, "messages");
		CallModule("markMessagesAsRead", soaringDictionary, null);
	}

	internal void ResetPassword(string username, string email)
	{
		if (string.IsNullOrEmpty(username))
		{
			Delegate.OnPasswordReset(false, CreateInvalidCredentialsError("Invaid Username"));
			return;
		}
		if (!ValidateUserName(username, SoaringLoginType.Soaring))
		{
			Delegate.OnPasswordReset(false, CreateInvalidCredentialsError("Invaid Username"));
			return;
		}
		if (string.IsNullOrEmpty(email))
		{
			Delegate.OnPasswordReset(false, CreateInvalidCredentialsError("Invaid Email"));
			return;
		}
		if (!ValidateEmailFormat(email))
		{
			Delegate.OnPasswordReset(false, CreateInvalidCredentialsError("Invaid Email"));
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(mGameID, "gameId");
		soaringDictionary.addValue(username, "tag");
		soaringDictionary.addValue(email, "email");
		CallModule("resetUserPassword", soaringDictionary, null);
	}

	internal void ResetPasswordConfirm(string username, string confirmCode, string password)
	{
		if (string.IsNullOrEmpty(username))
		{
			Delegate.OnPasswordResetConfirmed(false, CreateInvalidCredentialsError("Invaid Username"));
			return;
		}
		if (!ValidateUserName(username, SoaringLoginType.Soaring))
		{
			Delegate.OnPasswordResetConfirmed(false, CreateInvalidCredentialsError("Invaid Username"));
			return;
		}
		if (string.IsNullOrEmpty(confirmCode))
		{
			Delegate.OnPasswordResetConfirmed(false, "Invaid Confirm Code");
			return;
		}
		if (string.IsNullOrEmpty(password))
		{
			Delegate.OnPasswordResetConfirmed(false, "Invaid Confirm Code");
			return;
		}
		if (password.Length < 6 || password.Length > 16)
		{
			Delegate.OnPasswordResetConfirmed(false, "Invaid Confirm Code");
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(GameID, "gameId");
		soaringDictionary.addValue(username, "tag");
		soaringDictionary.addValue(password, "password");
		soaringDictionary.addValue(confirmCode, "resetToken");
		CallModule("renewUserPassword", soaringDictionary, null);
	}

	internal void ChangePassword(string oldPassword, string newPassword, SoaringContext context)
	{
		if (!IsAuthorized())
		{
			Delegate.OnPasswordChanged(false, CreateInvalidAuthCodeError(), context);
			return;
		}
		if (string.IsNullOrEmpty(oldPassword))
		{
			Delegate.OnPasswordChanged(false, CreateInvalidCredentialsError("Invaid Password"), context);
			return;
		}
		if (string.IsNullOrEmpty(newPassword))
		{
			Delegate.OnPasswordChanged(false, CreateInvalidCredentialsError("Invaid Password"), context);
			return;
		}
		if (newPassword.Length < 6 || newPassword.Length > 16)
		{
			Delegate.OnPasswordChanged(false, CreateInvalidCredentialsError("Password must be between 6 and 16 characters in length"), context);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(mAuthorizationToken, "authToken");
		soaringDictionary.addValue(oldPassword, "oldPassword");
		soaringDictionary.addValue(newPassword, "newPassword");
		CallModule("changeUserPassword", soaringDictionary, context);
	}

	internal void RequestCampaign(SoaringContext context)
	{
		if (!IsAuthorized())
		{
			Delegate.OnRetrieveCampaign(false, CreateInvalidAuthCodeError(), null, context);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(mAuthorizationToken, "authToken");
		CallModule("retrieveABCampaignData", soaringDictionary, context);
	}

	internal void SaveStat(string key, SoaringObjectBase value)
	{
		if (!SoaringInternalProperties.IsLoaded)
		{
			SoaringInternalProperties.Load();
		}
		if (SoaringInternalProperties.EnableAnalytics && mAnalytics == null)
		{
			mAnalytics = new SoaringAnalytics();
			mAnalytics.Initialize();
		}
		if (mAnalytics != null)
		{
			mAnalytics.LogEvent(key, value);
		}
	}

	internal void SaveStat(SoaringArray entries)
	{
		if (!SoaringInternalProperties.IsLoaded)
		{
			SoaringInternalProperties.Load();
		}
		if (SoaringInternalProperties.EnableAnalytics && mAnalytics == null)
		{
			mAnalytics = new SoaringAnalytics();
			mAnalytics.Initialize();
		}
		if (mAnalytics != null)
		{
			mAnalytics.LogEvents(entries);
		}
	}

	internal void SaveAnonymousStat(string key, SoaringObjectBase value)
	{
		if (!SoaringInternalProperties.IsLoaded)
		{
			SoaringInternalProperties.Load();
		}
		if (SoaringInternalProperties.EnableAnalytics && mAnalytics == null)
		{
			mAnalytics = new SoaringAnalytics();
			mAnalytics.Initialize();
		}
		if (mAnalytics != null)
		{
			mAnalytics.LogAnonymousEvent(key, value);
		}
	}

	internal void SaveAnonymousStat(SoaringArray entries)
	{
		if (!SoaringInternalProperties.IsLoaded)
		{
			SoaringInternalProperties.Load();
		}
		if (SoaringInternalProperties.EnableAnalytics && mAnalytics == null)
		{
			mAnalytics = new SoaringAnalytics();
			mAnalytics.Initialize();
		}
		if (mAnalytics != null)
		{
			mAnalytics.LogAnonymousEvents(entries);
		}
	}

	internal void internal_SaveStat(string key, SoaringObjectBase value, SoaringContext context)
	{
		SoaringArray soaringArray = new SoaringArray(1);
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringArray.addObject(soaringDictionary);
		soaringDictionary.addValue(key, "key");
		soaringDictionary.addValue(value, "value");
		internal_SaveStat(soaringArray, context);
	}

	internal void internal_SaveStat(SoaringArray entries, SoaringContext context)
	{
		if (!IsAuthorized())
		{
			Delegate.OnSaveStat(false, false, CreateInvalidAuthCodeError(), context);
			return;
		}
		if (entries == null)
		{
			Delegate.OnSaveStat(false, false, "Invalid Entries", context);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(entries, "entries");
		CallModule("saveStat", soaringDictionary, context);
	}

	internal void internal_SaveAnonymousStat(string key, SoaringDictionary value, SoaringContext context)
	{
		SoaringArray soaringArray = new SoaringArray(1);
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringArray.addObject(soaringDictionary);
		soaringDictionary.addValue(key, "key");
		soaringDictionary.addValue(value, "value");
		internal_SaveAnonymousStat(soaringArray, context);
	}

	internal void internal_SaveAnonymousStat(SoaringArray entries, SoaringContext context)
	{
		if (!SoaringInternalProperties.IsLoaded)
		{
			SoaringInternalProperties.Load();
		}
		if (SoaringInternalProperties.EnableAnalytics && mAnalytics == null)
		{
			mAnalytics = new SoaringAnalytics();
			mAnalytics.Initialize();
		}
		if (entries == null)
		{
			Delegate.OnSaveStat(false, true, "Invalid Entries", context);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(entries, "entries");
		if (!IsAuthorized())
		{
			if (mSoaringStashedCall == null)
			{
				mSoaringStashedCall = new SoaringArray();
			}
			mSoaringStashedCall.addObject(new SoaringStashedCall("saveAnonymousStat", soaringDictionary, context));
		}
		else
		{
			CallModule("saveAnonymousStat", soaringDictionary, context);
		}
	}

	internal bool ValidateEmailFormat(string email)
	{
		if (email == null)
		{
			return false;
		}
		if (email.Length >= 256 || email.Length < 3)
		{
			return false;
		}
		int num = email.IndexOf('@');
		if (num <= 0)
		{
			return false;
		}
		int num2 = email.LastIndexOf('.');
		if (num2 <= 0 || num2 < num || num + 1 > num2 || num2 + 1 >= email.Length)
		{
			return false;
		}
		return true;
	}

	internal void CheckFilesForUpdates(bool updateFiles)
	{
		mVersions.CheckFilesForUpdates(updateFiles);
	}

	internal bool PushCall(SoaringDictionary callData)
	{
		if (!Soaring.IsOnline)
		{
			bool flag = false;
			if (callData != null && callData.containsKey("trIOff"))
			{
				flag = true;
			}
			if (flag)
			{
				SoaringDebug.Log("PushCall: Game is offline", LogType.Error);
				return false;
			}
		}
		if (callData == null)
		{
			SoaringDebug.Log("PushCall: Invalid Call Data", LogType.Error);
			return false;
		}
		SCWebQueue.SCWebCallbackObject sCWebCallbackObject = (SCWebQueue.SCWebCallbackObject)callData.objectWithKey("tcallback");
		if (sCWebCallbackObject == null)
		{
			SoaringDebug.Log("PushCall: Invalid Callback", LogType.Error);
			return false;
		}
		SCWebQueue.SCWebCallbackObject sCWebCallbackObject2 = (SCWebQueue.SCWebCallbackObject)callData.objectWithKey("tvcallback");
		SCWebQueue.SCWebQueueCallback verifyCallback = null;
		if (sCWebCallbackObject2 != null)
		{
			verifyCallback = sCWebCallbackObject2.callback;
		}
		string text = callData.soaringValue("turl");
		if (string.IsNullOrEmpty(text))
		{
			text = WEB_SDK;
		}
		int num = callData.soaringValue("tchannel");
		if (num < 0)
		{
			num = 0;
		}
		string saveData = callData.soaringValue("tsave");
		SoaringDictionary postData = (SoaringDictionary)callData.objectWithKey("tposts");
		SoaringDictionary urlData = (SoaringDictionary)callData.objectWithKey("tgets");
		object userData = callData.objectWithKey("tobject");
		return mWebQueue.StartConnection(num, userData, text, saveData, postData, urlData, sCWebCallbackObject.callback, verifyCallback);
	}

	public void PushContextEvent(SoaringContext context)
	{
		if (context != null)
		{
			mWebQueue.RegisterEventMessage(context);
		}
	}

	internal void UpdatePlayerData(SoaringDictionary data)
	{
		UpdatePlayerData(data, false);
	}

	internal void UpdatePlayerData(SoaringDictionary data, bool clearData)
	{
		if (mPlayerData == null)
		{
			mPlayerData = new SoaringPlayer();
		}
		mPlayerData.SetUserData(data, clearData);
		mAuthorizationToken = mPlayerData.AuthToken;
		mPlayerData.Save();
	}

	internal void SetSoaringInternalData(SoaringDictionary data)
	{
		if (data == null)
		{
			return;
		}
		SoaringDictionary soaringDictionary = (SoaringDictionary)data.objectWithKey("settings");
		if (soaringDictionary == null)
		{
			return;
		}
		SoaringDictionary soaringDictionary2 = (SoaringDictionary)soaringDictionary.objectWithKey("encrypted");
		if (soaringDictionary2 != null)
		{
			mEncryptedModules = soaringDictionary2.makeCopy();
			CheckModulesForSecureConnection();
		}
		if (mSoaringEvents != null)
		{
			SoaringArray soaringArray = (SoaringArray)soaringDictionary.objectWithKey("events");
			if (soaringArray != null)
			{
				mSoaringEvents.LoadEvents(soaringArray);
			}
		}
	}

	private void CheckModulesForSecureConnection()
	{
		if (mEncryptedModules == null || mSoaringModules == null)
		{
			return;
		}
		int num = mSoaringModules.count();
		for (int i = 0; i < num; i++)
		{
			SoaringModule soaringModule = (SoaringModule)mSoaringModules.objectAtIndex(i);
			if (soaringModule != null)
			{
				SoaringValue soaringValue = mEncryptedModules.soaringValue(soaringModule.ModuleName());
				if (soaringValue != null)
				{
					soaringModule.encryptedCall = soaringValue;
				}
			}
		}
	}

	internal string GetSoaringAddress(string key)
	{
		return mAddressKeeper.Address(key);
	}

	internal void DownloadFileWithSoaring(string name, string url, string path, SoaringContext context)
	{
		DownloadFileWithSoaring(name, url, path, null, context);
	}

	internal void DownloadFileWithSoaring(string name, string url, string path, SCWebQueue.SCDownloadCallback callback)
	{
		DownloadFileWithSoaring(name, url, path, callback, null);
	}

	internal void DownloadFileWithSoaring(string name, string url, string path, SCWebQueue.SCDownloadCallback callback, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(name, "tobject");
		soaringDictionary.addValue(url, "turl");
		soaringDictionary.addValue(path, "tsave");
		if (callback != null)
		{
			soaringDictionary.addValue(new SCWebQueue.SCDownloadCallbackObject(callback), "tcallback");
		}
		CallModule("downloadFiles", soaringDictionary, context);
	}

	internal void HandleStashedCalls()
	{
		SoaringArray soaringArray = mSoaringStashedCall;
		if (soaringArray == null || soaringArray.count() == 0)
		{
			return;
		}
		mSoaringStashedCall = new SoaringArray();
		for (int i = 0; i < soaringArray.count(); i++)
		{
			SoaringStashedCall soaringStashedCall = null;
			try
			{
				soaringStashedCall = (SoaringStashedCall)soaringArray.objectAtIndex(i);
				CallModule(soaringStashedCall.ModuleName, soaringStashedCall.CallData, soaringStashedCall.Contex);
			}
			catch (Exception ex)
			{
				SoaringDebug.Log("Error In Module: " + soaringStashedCall.ModuleName + " : " + ex.Message + "\n" + ex.StackTrace, LogType.Error);
				throw ex;
			}
		}
		soaringArray.clear();
	}

	internal string PlatformKeyWithLoginType(SoaringLoginType type, bool soaringPlatformDefault)
	{
		string result = ((!soaringPlatformDefault) ? null : "tag");
		switch (type)
		{
		case SoaringLoginType.Facebook:
			result = "facebookId";
			break;
		case SoaringLoginType.GameCenter:
			result = "gamecenterId";
			break;
		case SoaringLoginType.Amazon:
			result = "amazonId";
			break;
		case SoaringLoginType.GoogleGS:
			result = "googleId";
			break;
		case SoaringLoginType.Device:
			result = "deviceId";
			break;
		}
		return result;
	}

	public static string PlatformKeyAbriviationWithLoginType(SoaringLoginType type, bool soaringPlatformDefault)
	{
		string result = ((!soaringPlatformDefault) ? null : "tag");
		switch (type)
		{
		case SoaringLoginType.Facebook:
			result = "fb1";
			break;
		case SoaringLoginType.GoogleGS:
			result = "gs1";
			break;
		case SoaringLoginType.GameCenter:
			result = "gc1";
			break;
		case SoaringLoginType.Amazon:
			result = "ac1";
			break;
		}
		return result;
	}

	public static SoaringLoginType PlatformKeyAbriviationWithTag(string userID)
	{
		if (string.IsNullOrEmpty(userID))
		{
			return SoaringLoginType.Soaring;
		}
		char[] separator = new char[1] { ':' };
		string[] array = userID.Split(separator);
		if (array == null)
		{
			return SoaringLoginType.Soaring;
		}
		if (array.Length == 0)
		{
			return SoaringLoginType.Soaring;
		}
		string text = array[0];
		SoaringLoginType result = SoaringLoginType.Soaring;
		switch (text)
		{
		case "fb1":
			return SoaringLoginType.Facebook;
		case "gc1":
			return SoaringLoginType.GameCenter;
		case "ac1":
			return SoaringLoginType.Amazon;
		case "gs1":
			return SoaringLoginType.GoogleGS;
		default:
			return result;
		}
	}

	private string PrimaryPlatformName()
	{
		return SoaringPlatform.PrimaryPlatformName;
	}

	private SoaringError CreateInvalidAuthCodeError()
	{
		return new SoaringError("Invalid User Auth Code.", -2);
	}

	private SoaringError CreateInvalidCredentialsError(string str)
	{
		return new SoaringError(str, -3);
	}
}
