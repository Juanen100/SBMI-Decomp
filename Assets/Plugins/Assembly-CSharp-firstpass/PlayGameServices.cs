using System.Collections.Generic;
using UnityEngine;

public class PlayGameServices
{
	private static AndroidJavaObject _plugin;

	static PlayGameServices()
	{
	}

	public static void setAchievementToastSettings(GPGToastPlacement placement, int offset)
	{
	}

	public static void setWelcomeBackToastSettings(GPGToastPlacement placement, int offset)
	{
	}

	public static void enableDebugLog(bool shouldEnable)
	{
	}

	public static void setToastSettings(GPGToastPlacement placement)
	{
	}

	public static void init(string clientId, bool requestAppStateScope, bool fetchMetadataAfterAuthentication = true, bool pauseUnityWhileShowingFullScreenViews = true)
	{
	}

	public static void authenticate()
	{
	}

	public static void signOut()
	{
	}

	public static bool isSignedIn()
	{
		return false;
	}

	public static GPGPlayerInfo getLocalPlayerInfo()
	{
		return null;
	}

	public static void reloadAchievementAndLeaderboardData()
	{
	}

	public static void loadProfileImageForUri(string uri)
	{
	}

	public static void setStateData(string data, int key)
	{
	}

	public static string stateDataForKey(int key)
	{
		return null;
	}

	public static void loadCloudDataForKey(int key, bool useRemoteDataForConflictResolution = true)
	{
	}

	public static void deleteCloudDataForKey(int key, bool useRemoteDataForConflictResolution = true)
	{
	}

	public static void clearCloudDataForKey(int key, bool useRemoteDataForConflictResolution = true)
	{
	}

	public static void updateCloudDataForKey(int key, bool useRemoteDataForConflictResolution = true)
	{
	}

	public static void showAchievements()
	{
	}

	public static void revealAchievement(string achievementId)
	{
	}

	public static void unlockAchievement(string achievementId, bool showsCompletionNotification = true)
	{
	}

	public static void incrementAchievement(string achievementId, int numSteps)
	{
	}

	public static List<GPGAchievementMetadata> getAllAchievementMetadata()
	{
		return null;
	}

	public static void showLeaderboard(string leaderboardId, GPGLeaderboardTimeScope timeScope = GPGLeaderboardTimeScope.AllTime)
	{
	}

	public static void showLeaderboards()
	{
	}

	public static void submitScore(string leaderboardId, long score)
	{
	}

	public static void loadScoresForLeaderboard(string leaderboardId, GPGLeaderboardTimeScope timeScope, bool isSocial, bool personalWindow)
	{
	}

	public static List<GPGLeaderboardMetadata> getAllLeaderboardMetadata()
	{
		return null;
	}
}
