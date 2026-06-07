using System;
using System.Collections.Generic;
using UnityEngine;

public class Upsight
{
	private static bool Initialized;

	private static AndroidJavaObject _pluginBase;

	private static AndroidJavaObject _pluginPushExtension;

	private static AndroidJavaObject _pluginMarketingExtension;

	static Upsight()
	{
	}

	public static void init()
	{
	}

	internal static void terminate()
	{
	}

	public static UpsightUserResult setUserID(string id, bool preserve)
	{
		return default(UpsightUserResult);
	}

	public static UpsightUserResult clearUserID()
	{
		return default(UpsightUserResult);
	}

	public static UpsightUserResult deleteUser(string id)
	{
		return default(UpsightUserResult);
	}

	public static string getCurrentUserID()
	{
		return null;
	}

	public static int getCurrentUserSessionNumber()
	{
		return 0;
	}

	public static DateTime getCurrentUserSessionStartTime()
	{
		return default(DateTime);
	}

	public static void resetUserAttributes()
	{
	}

	public static void setUserAttributeString(string key, string value)
	{
	}

	public static void setUserAttributeFloat(string key, float value)
	{
	}

	public static void setUserAttributeInt(string key, int value)
	{
	}

	public static void setUserAttributeBool(string key, bool value)
	{
	}

	public static void setUserAttributeDate(string key, DateTime value)
	{
	}

	public static string getUserAttributeString(string key)
	{
		return null;
	}

	public static float getUserAttributeFloat(string key)
	{
		return 0f;
	}

	public static int getUserAttributeInt(string key)
	{
		return 0;
	}

	public static bool getUserAttributeBool(string key)
	{
		return false;
	}

	public static DateTime getUserAttributeDate(string key)
	{
		return default(DateTime);
	}

	public static void recordSessionlessCustomEvent(string eventName, Dictionary<string, object> properties = null)
	{
	}

	public static void recordCustomEvent(string eventName, Dictionary<string, object> properties = null)
	{
	}

	public static void recordMilestoneEvent(string scope, Dictionary<string, object> properties = null)
	{
	}

	public static bool isContentReadyForBillboardWithScope(string scope)
	{
		return false;
	}

	public static void prepareBillboard(string scope)
	{
	}

	public static void destroyBillboard(string scope)
	{
	}

	public static void recordMonetizationEvent(double totalPrice, string currency, UpsightPurchaseResolution resolution, string product = null, double price = -1.0, int quantity = -1, Dictionary<string, object> properties = null)
	{
	}

	public static void recordGooglePlayPurchase(int quantity, string currency, double price, double totalPrice, string product, int responseCode, string inAppPurchaseData, string inAppDataSignature, Dictionary<string, object> properties = null)
	{
	}

	public static void recordAppleStorePurchase(int quantity, string currency, double price, string transactionIdentifier, string product, UpsightPurchaseResolution resolution, Dictionary<string, object> properties = null)
	{
	}

	public static void recordAttributionEvent(string campaign, string creative, string source, Dictionary<string, object> properties = null)
	{
	}

	public static void registerForPushNotifications()
	{
	}

	public static void unregisterForPushNotifications()
	{
	}

	public static void setShouldSynchronizeManagedVariables(bool shouldSynchronize)
	{
	}

	public static string getManagedString(string key)
	{
		return null;
	}

	public static float getManagedFloat(string key)
	{
		return 0f;
	}

	public static int getManagedInt(string key)
	{
		return 0;
	}

	public static bool getManagedBool(string key)
	{
		return false;
	}

	public static string getAppToken()
	{
		return null;
	}

	public static string getPublicKey()
	{
		return null;
	}

	public static string getSid()
	{
		return null;
	}

	public static void setLoggerLevel(UpsightLoggerLevel loggerLevel)
	{
	}

	public static string getPluginVersion()
	{
		return null;
	}

	public static bool getOptOutStatus()
	{
		return false;
	}

	public static void setOptOutStatus(bool optOutStatus)
	{
	}

	public static void setLocation(double lat, double lon)
	{
	}

	public static void purgeLocation()
	{
	}

	public static int getLatestSessionNumber()
	{
		return 0;
	}

	public static long getLatestSessionStartTimestamp()
	{
		return 0L;
	}

	public static void onPause()
	{
	}

	public static void onResume()
	{
	}

	private static bool initSuccessful()
	{
		return false;
	}
}
