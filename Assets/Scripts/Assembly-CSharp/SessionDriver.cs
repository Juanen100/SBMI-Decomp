using System;
using System.Collections.Generic;
using DeltaDNA;
using Helpshift;
using UnityEngine;
using com.amazon.device.iap.cpt;

public class SessionDriver : MonoBehaviour
{
	public const string KontagentAndroidKey = "eb9066e818664a89923bf70626a50288";

	public const string deltaDNACollectKey = "http://collect3106sbmvg.deltadna.net/collect/api";

	public const string deltaDNAEngageURL = "http://engage3106sbmvg.deltadna.net";

	public const string helpshiftAPIKey = "9acc31b4614cba52a05eadb41240f3e5";

	public const string helpshiftDomain = "nick.helpshift.com";

	public const string helpshiftAppID = "nick_platform_20150814162842188-e0b3f9614ea849b";

	private const int currentVersion = 1;

	[NonSerialized]
	public string androidAppToken = "6b8c7716edc0427b85dc87a0b6efad7f";

	[NonSerialized]
	public string androidAppSecret = "bf364b5161a642a6847264f2c9804b6d";

	[NonSerialized]
	public string gcmProjectNumber = string.Empty;

	[NonSerialized]
	public string iosAppToken = "e4e51f061d1c456e987ff997299ca4f8";

	[NonSerialized]
	public string iosAppSecret = "fa95d77b2246444eabf3877890e5ebf7";

	[NonSerialized]
	public string amazonAppToken = "d69c26b6915443a88f3d87d817e68bd9";

	[NonSerialized]
	public string amazonAppSecret = "d16c6edbcb2544cdb692418553d43b6a";

	public bool registerForPushNotifications;

	private Session session;

	public static Session session_ref { get; private set; }

	private void Start()
	{
		if (TFUtils.isAmazon())
		{
			Upsight.init(amazonAppToken, amazonAppSecret, gcmProjectNumber);
			Upsight.requestAppOpen();
		}
		else
		{
			Upsight.init(androidAppToken, androidAppSecret, gcmProjectNumber);
			Upsight.requestAppOpen();
		}
		TFUtils.Init();
		TFUtils.RefreshSAFiles();
		GameObject gameObject = new GameObject();
		gameObject.name = "googleServiceListener";
		gameObject.AddComponent<GPGSEventListener>();
		if (TFUtils.isAmazon())
		{
			AmazonIAPEventListener.getInstance();
			IAmazonIapV2 instance = AmazonIapV2Impl.Instance;
			if (instance != null)
			{
			}
		}
		session = new Session(1);
		session_ref = session;
		AndroidBack.getInstance().addSession(session);
		SoaringPlatformType soaringPlatformType = SoaringPlatformType.System;
		soaringPlatformType = ((!TFUtils.isAmazon()) ? SoaringPlatformType.Android : SoaringPlatformType.Amazon);
		if (!Singleton<SDK>.Instance.IsInitialised)
		{
			Singleton<SDK>.Instance.ClientVersion = SBSettings.BundleVersion;
			Singleton<SDK>.Instance.Settings.DebugMode = SBSettings.ShowDebug;
			Singleton<SDK>.Instance.StartSDK(SBSettings.DeltaDNAEnvKey, "http://collect3106sbmvg.deltadna.net/collect/api", "http://engage3106sbmvg.deltadna.net", SDK.AUTO_GENERATED_USER_ID);
		}
		SoaringPlatform.Init(soaringPlatformType);
		SoaringTime.Load();
		Soaring.SaveAnonymousStat("anonymous_start", SoaringAnalytics.StampDeviceMetadata());
		if (Language.CurrentLanguage() == LanguageCode.N)
		{
			Language.Init(TFUtils.GetPersistentAssetsPath());
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("unityGameObject", "SessionDriver");
		dictionary.Add("enableInAppNotification", "yes");
		dictionary.Add("enableDialogUIForTablets", "no");
		dictionary.Add("presentFullScreenOniPad", "no");
		HelpshiftSdk.getInstance().install("9acc31b4614cba52a05eadb41240f3e5", "nick.helpshift.com", "nick_platform_20150814162842188-e0b3f9614ea849b", dictionary);
		base.gameObject.AddComponent<HelpshiftNotificationCount>();
	}

	private void Update()
	{
		if (session != null)
		{
			session.ProcessStateChanges();
			session.OnUpdate();
		}
	}

	public void OnApplicationPause(bool paused)
	{
		TFUtils.DebugLog("Application pausing " + paused);
		if (session != null)
		{
			TFUtils.DebugLog("SessionState=" + session.TheState);
			session.OnPause(paused);
		}
	}

	public void OnApplicationQuit()
	{
		TFUtils.DebugLog("Application quitting...");
		if (session != null)
		{
			TFUtils.DebugLog("SessionState=" + session.TheState);
			session.OnApplicationQuit();
		}
		Singleton<SDK>.Instance.StopSDK();
	}

	public void OnApplicationFocus(bool bFocus)
	{
		TFUtils.DebugLog("Application lost focus...");
		if (session != null)
		{
			TFUtils.DebugLog("SessionState=" + session.TheState);
			session.OnApplicationFocus(bFocus);
		}
	}

	public void OnMemoryWarning(string msg)
	{
		SoaringDebug.Log("OnMemoryWarning: Recieved from: " + msg, LogType.Warning);
	}

	private void onExternalMessage(string msg)
	{
		session.onExternalMessage(msg);
	}

	private void LoginAndroid()
	{
		Debug.Log("---------------------loginAndroid");
		/*if (TFUtils.isAmazon())
		{
			ServiceEvent();
		}
		else
		{
			AuthenticationEvent();
		}
		if (TFUtils.isAmazon())
		{
			GameCenterBinding.authenticateLocalPlayer();
		}
		else
		{
			authenticationFailedEvent("NOT VALUE");
		}*/
	}

	private void ServiceReadyHandler()
	{
		Debug.Log("ServiceReadyHandler");
		UnServiceEvent();
		SubscribeToProfileEvents();
		AGSProfilesClient.RequestLocalPlayerProfile();
	}

	private void ServiceNotReadyHandler(string error)
	{
		Debug.Log("ServiceNotReadyHandler");
		UnServiceEvent();
		session = new Session(1);
		session_ref = session;
		AndroidBack.getInstance().addSession(session);
	}

	private void ServiceEvent()
	{
		AGSClient.ServiceReadyEvent += ServiceReadyHandler;
		AGSClient.ServiceNotReadyEvent += ServiceNotReadyHandler;
	}

	private void UnServiceEvent()
	{
		AGSClient.ServiceReadyEvent -= ServiceReadyHandler;
		AGSClient.ServiceNotReadyEvent -= ServiceNotReadyHandler;
	}

	private void PlayerAliasReceived(AGSProfile profile)
	{
		Debug.Log("PlayerAliasReceived");
		Debug.Log("profile.playerId " + profile.playerId);
		UnsubscribeFromProfileEvents();
		session = new Session(1);
		session_ref = session;
		AndroidBack.getInstance().addSession(session);
	}

	private void PlayerAliasFailed(string errorMessage)
	{
		Debug.Log("PlayerAliasFailed " + errorMessage);
		UnsubscribeFromProfileEvents();
		session = new Session(1);
		session_ref = session;
		AndroidBack.getInstance().addSession(session);
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

	private void AuthenticationEvent()
	{
		GPGManager.authenticationSucceededEvent += authenticationSucceededEvent;
		GPGManager.authenticationFailedEvent += authenticationFailedEvent;
	}

	private void UnAuthenticationEvent()
	{
		GPGManager.authenticationSucceededEvent -= authenticationSucceededEvent;
		GPGManager.authenticationFailedEvent -= authenticationFailedEvent;
	}

	private void authenticationSucceededEvent(string param)
	{
		Debug.Log("authenticationSucceededEvent11: " + param);
		GPGPlayerInfo localPlayerInfo = PlayGameServices.getLocalPlayerInfo();
		UnAuthenticationEvent();
		session = new Session(1);
		session_ref = session;
		AndroidBack.getInstance().addSession(session);
	}

	private void authenticationFailedEvent(string error)
	{
		Debug.Log("authenticationFailedEvent: " + error);
		if (!"Unknown error".Equals(error))
		{
			UnAuthenticationEvent();
			session = new Session(1);
			session_ref = session;
			AndroidBack.getInstance().addSession(session);
		}
	}

	private void OnGUI()
	{
		/*AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("android.content.pm.ActivityInfo");
		int num = androidJavaClass2.GetStatic<int>("SCREEN_ORIENTATION_SENSOR_LANDSCAPE");
		androidJavaObject.Call("setRequestedOrientation", num);*/
	}
}
