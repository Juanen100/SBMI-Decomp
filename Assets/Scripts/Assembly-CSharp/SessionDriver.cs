using System;
using UnityEngine;

public class SessionDriver : MonoBehaviour
{
	[NonSerialized]
	public string androidAppToken;

	[NonSerialized]
	public string androidAppSecret;

	[NonSerialized]
	public string gcmProjectNumber;

	[NonSerialized]
	public string iosAppToken;

	[NonSerialized]
	public string iosAppSecret;

	[NonSerialized]
	public string amazonAppToken;

	[NonSerialized]
	public string amazonAppSecret;

	public bool registerForPushNotifications;

	public const string deltaDNACollectKey = "http://collect3106sbmvg.deltadna.net/collect/api";

	public const string deltaDNAEngageURL = "http://engage3106sbmvg.deltadna.net";

	public const string helpshiftAPIKey = "106f00a34849600771130712c2ccfb30";

	public const string helpshiftDomain = "Viacom.helpshift.com";

	public const string helpshiftAppID = "viacom_platform_20191004045205428-e76ab30c6569a00";

	public int helpshiftNotificationCount;

	private const int currentVersion = 1;

	private static SessionDriver _instance;

	private Session session;

	public static SessionDriver Instance
	{
		get
		{
			return null;
		}
	}

	public static Session session_ref { get; private set; }

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnApplicationPause(bool paused)
	{
	}

	public void OnApplicationQuit()
	{
	}

	public void OnApplicationFocus(bool bFocus)
	{
	}

	public void OnMemoryWarning(string msg)
	{
	}

	private void onExternalMessage(string msg)
	{
	}

	private void LoginAndroid()
	{
	}

	private void ServiceReadyHandler()
	{
	}

	private void ServiceNotReadyHandler(string error)
	{
	}

	private void ServiceEvent()
	{
	}

	private void UnServiceEvent()
	{
	}

	private void PlayerAliasReceived(AGSProfile profile)
	{
	}

	private void PlayerAliasFailed(string errorMessage)
	{
	}

	private void SubscribeToProfileEvents()
	{
	}

	private void UnsubscribeFromProfileEvents()
	{
	}

	private void AuthenticationEvent()
	{
	}

	private void UnAuthenticationEvent()
	{
	}

	private void authenticationSucceededEvent(string param)
	{
	}

	private void authenticationFailedEvent(string error)
	{
	}

	private void OnGUI()
	{
	}

	private void didReceiveUnreadMessagesCount(string message)
	{
	}

	public void helpshiftSessionBegan(string message)
	{
	}

	public void helpshiftSessionEnded(string message)
	{
	}

	public void newConversationStarted(string newConversationMessage)
	{
	}

	public void conversationEnded()
	{
	}
}
