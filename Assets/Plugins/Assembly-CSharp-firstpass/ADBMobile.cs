using System.Collections.Generic;
using UnityEngine;

public class ADBMobile
{
	public enum ADBPrivacyStatus
	{
		MOBILE_PRIVACY_STATUS_OPT_IN = 1,
		MOBILE_PRIVACY_STATUS_OPT_OUT = 2,
		MOBILE_PRIVACY_STATUS_UNKNOWN = 3
	}

	public enum ADBMobileVisitorAuthenticationState
	{
		VISITOR_ID_AUTHENTICATION_STATE_UNKNOWN = 0,
		VISITOR_ID_AUTHENTICATION_STATE_AUTHENTICATED = 1,
		VISITOR_ID_AUTHENTICATION_STATE_LOGGED_OUT = 2
	}

	public enum ADBMobileAppExtensionType
	{
		MOBILE_APP_EXTENSION_TYPE_REGULAR = 0,
		MOBILE_APP_EXTENSION_TYPE_STANDALONE = 1
	}

	public enum ADBBeaconProximity
	{
		PROXIMITY_UNKNOWN = 0,
		PROXIMITY_IMMEDIATE = 1,
		PROXIMITY_NEAR = 2,
		PROXIMITY_FAR = 3
	}

	private static AndroidJavaClass analytics;

	private static AndroidJavaClass config;

	private static AndroidJavaClass visitor;

	private static AndroidJavaClass acquisition;

	private static AndroidJavaClass audienceManager;

	private static AndroidJavaClass target;

	public static void CollectLifecycleData()
	{
	}

	public static void CollectLifecycleData(Dictionary<string, object> cdata)
	{
	}

	public static bool GetDebugLogging()
	{
		return false;
	}

	public static double GetLifetimeValue()
	{
		return 0.0;
	}

	public static ADBPrivacyStatus GetPrivacyStatus()
	{
		return default(ADBPrivacyStatus);
	}

	public static string GetUserIdentifier()
	{
		return null;
	}

	public static string GetVersion()
	{
		return null;
	}

	public static void KeepLifecycleSessionAlive()
	{
	}

	public static void OverrideConfigPath(string fileName)
	{
	}

	public static void PauseCollectingLifecycleData()
	{
	}

	public static void SetContext()
	{
	}

	public static void SetDebugLogging(bool enabled)
	{
	}

	public static void SetPrivacyStatus(ADBPrivacyStatus status)
	{
	}

	public static void SetUserIdentifier(string userId)
	{
	}

	public static void EnableLocalNotifications()
	{
	}

	public static void SetAdvertisingIdentifier(string advertisingId)
	{
	}

	public static void SubmitAdvertisingIdentifierTask(SubmitAdIdCallable task)
	{
	}

	public static void SetPushIdentifier(string deviceToken)
	{
	}

	public static void TrackState(string state, Dictionary<string, object> cdata)
	{
	}

	public static void TrackAction(string action, Dictionary<string, object> cdata)
	{
	}

	public static void TrackActionFromBackground(string action, Dictionary<string, object> cdata)
	{
	}

	public static void TrackLocation(float latValue, float lonValue, Dictionary<string, object> cdata)
	{
	}

	public static void TrackBeacon(int major, int minor, string uuid, ADBBeaconProximity proximity, Dictionary<string, object> cdata)
	{
	}

	public static void TrackingClearCurrentBeacon()
	{
	}

	public static void TrackLifetimeValueIncrease(double amount, Dictionary<string, object> cdata)
	{
	}

	public static void TrackTimedActionStart(string action, Dictionary<string, object> cdata)
	{
	}

	public static void TrackTimedActionUpdate(string action, Dictionary<string, object> cdata)
	{
	}

	public static void TrackTimedActionEnd(string action)
	{
	}

	public static bool TrackingTimedActionExists(string action)
	{
		return false;
	}

	public static string GetTrackingIdentifier()
	{
		return null;
	}

	public static void TrackingSendQueuedHits()
	{
	}

	public static void TrackingClearQueue()
	{
	}

	public static int TrackingGetQueueSize()
	{
		return 0;
	}

	public static void TrackAdobeDeepLink(string url)
	{
	}

	public static void TrackPushNotificationClickThrough(Dictionary<string, object> userInfo)
	{
	}

	public static void TrackLocalNotificationClickthrough(Dictionary<string, object> userInfo)
	{
	}

	public static void AcquisitionCampaignStartForApp(string appID, Dictionary<string, object> data)
	{
	}

	public static void TargetLoadRequest(string name, string defaultContent, Dictionary<string, object> profileParameters, Dictionary<string, object> orderParameters, Dictionary<string, object> mboxParameters, Dictionary<string, object> requestLocationParameters, AdobeTargetCallback callback)
	{
	}

	public static string TargetGetThirdPartyId()
	{
		return null;
	}

	public static void TargetSetThirdPartyId(string thirdPartyId)
	{
	}

	public static void TargetClearCookies()
	{
	}

	public static string TargetGetPcId()
	{
		return null;
	}

	public static string TargetGetSessionId()
	{
		return null;
	}

	public static void AudienceSubmitSignal(Dictionary<string, object> data, AdobeAudienceManagerCallback callback)
	{
	}

	public static string AudienceGetVistorProfile()
	{
		return null;
	}

	public static string AudienceGetDpid()
	{
		return null;
	}

	public static string AudienceGetDpuuid()
	{
		return null;
	}

	public static void AudienceReset()
	{
	}

	public static void AudienceSetDpidAndDpuuid(string dpid, string dpuuid)
	{
	}

	public static string GetMarketingCloudID()
	{
		return null;
	}

	public static void VisitorSyncIdentifiers(Dictionary<string, object> identifiers)
	{
	}

	public static void VisitorSyncIdentifiers(Dictionary<string, object> identifier, ADBMobileVisitorAuthenticationState visitorState)
	{
	}

	public static void VisitorSyncIdentifiersWithType(string identifierType, string identifier, ADBMobileVisitorAuthenticationState visitorState)
	{
	}

	public static string VisitorAppendtoURL(string url)
	{
		return null;
	}

	public static List<ADBVisitorID> VisitorGetIds()
	{
		return null;
	}

	public static void CollectPII(Dictionary<string, object> data)
	{
	}

	private static ADBPrivacyStatus ADBPrivacyStatusFromInt(int statusInt)
	{
		return default(ADBPrivacyStatus);
	}

	private static ADBBeaconProximity ADBBeaconProximityFromInt(int proximity)
	{
		return default(ADBBeaconProximity);
	}

	private static ADBMobileVisitorAuthenticationState ADBMobileVisitorAuthenticationStateFromInt(int visitorState)
	{
		return default(ADBMobileVisitorAuthenticationState);
	}

	private static ADBMobileAppExtensionType ADBMobileAppExtensionTypeFromInt(int extensionType)
	{
		return default(ADBMobileAppExtensionType);
	}

	private static AndroidJavaObject GetHashMapFromDictionary(Dictionary<string, object> dict)
	{
		return null;
	}

	internal static string GetJsonStringFromHashMap(AndroidJavaObject hashmap)
	{
		return null;
	}

	private static AndroidJavaObject GetURIFromString(string uriString)
	{
		return null;
	}
}
