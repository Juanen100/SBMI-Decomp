using System.Collections.Generic;
using Prime31;
using UnityEngine;

public class PlayGameServices
{
	private static AndroidJavaObject _plugin;

	static PlayGameServices()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.prime31.PlayGameServicesPlugin"))
		{
			_plugin = androidJavaClass.CallStatic<AndroidJavaObject>("instance", new object[0]);
		}
	}

	public static void setAchievementToastSettings(GPGToastPlacement placement, int offset)
	{
		setToastSettings(placement);
	}

	public static void setWelcomeBackToastSettings(GPGToastPlacement placement, int offset)
	{
		setToastSettings(placement);
	}

	public static void enableDebugLog(bool shouldEnable)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("enableDebugLog", shouldEnable);
		}
	}

	public static void setToastSettings(GPGToastPlacement placement)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("setToastSettings", (int)placement);
		}
	}

	public static void init(string clientId, bool requestAppStateScope, bool fetchMetadataAfterAuthentication = true, bool pauseUnityWhileShowingFullScreenViews = true)
	{
	}

	public static void authenticate()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("authenticate");
		}
	}

	public static void signOut()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("signOut");
		}
	}

	public static bool isSignedIn()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return false;
		}
		return _plugin.Call<bool>("isSignedIn", new object[0]);
	}

	public static GPGPlayerInfo getLocalPlayerInfo()
	{
		GPGPlayerInfo gPGPlayerInfo = new GPGPlayerInfo();
		if (Application.platform != RuntimePlatform.Android)
		{
			return gPGPlayerInfo;
		}
		gPGPlayerInfo.setDataFromJson(_plugin.Call<string>("getLocalPlayerInfo", new object[0]));
		return gPGPlayerInfo;
	}

	public static void reloadAchievementAndLeaderboardData()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("loadBasicModelData");
		}
	}

	public static void loadProfileImageForUri(string uri)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("loadProfileImageForUri", uri);
		}
	}

	public static void setStateData(string data, int key)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("setStateData", data, key);
		}
	}

	public static string stateDataForKey(int key)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return null;
		}
		return _plugin.Call<string>("stateDataForKey", new object[1] { key });
	}

	public static void loadCloudDataForKey(int key, bool useRemoteDataForConflictResolution = true)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("loadCloudDataForKey", key, useRemoteDataForConflictResolution);
		}
	}

	public static void deleteCloudDataForKey(int key, bool useRemoteDataForConflictResolution = true)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("deleteCloudDataForKey", key, useRemoteDataForConflictResolution);
		}
	}

	public static void clearCloudDataForKey(int key, bool useRemoteDataForConflictResolution = true)
	{
	}

	public static void updateCloudDataForKey(int key, bool useRemoteDataForConflictResolution = true)
	{
		loadCloudDataForKey(key, useRemoteDataForConflictResolution);
	}

	public static void showAchievements()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("showAchievements");
		}
	}

	public static void revealAchievement(string achievementId)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("revealAchievement", achievementId);
		}
	}

	public static void unlockAchievement(string achievementId, bool showsCompletionNotification = true)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("unlockAchievement", achievementId, showsCompletionNotification);
		}
	}

	public static void incrementAchievement(string achievementId, int numSteps)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("incrementAchievement", achievementId, numSteps);
		}
	}

	public static List<GPGAchievementMetadata> getAllAchievementMetadata()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return new List<GPGAchievementMetadata>();
		}
		string json = _plugin.Call<string>("getAllAchievementMetadata", new object[0]);
		return DTOBase.listFromJson<GPGAchievementMetadata>(json);
	}

	public static void showLeaderboard(string leaderboardId, GPGLeaderboardTimeScope timeScope = GPGLeaderboardTimeScope.AllTime)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("showLeaderboard", leaderboardId);
		}
	}

	public static void showLeaderboards()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("showLeaderboards");
		}
	}

	public static void submitScore(string leaderboardId, long score)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("submitScore", leaderboardId, score);
		}
	}

	public static void loadScoresForLeaderboard(string leaderboardId, GPGLeaderboardTimeScope timeScope, bool isSocial, bool personalWindow)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("loadScoresForLeaderboard", leaderboardId, (int)timeScope, isSocial, personalWindow);
		}
	}

	public static List<GPGLeaderboardMetadata> getAllLeaderboardMetadata()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return new List<GPGLeaderboardMetadata>();
		}
		string json = _plugin.Call<string>("getAllLeaderboardMetadata", new object[0]);
		return DTOBase.listFromJson<GPGLeaderboardMetadata>(json);
	}
}
