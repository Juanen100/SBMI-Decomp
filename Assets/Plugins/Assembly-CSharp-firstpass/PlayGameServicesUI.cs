using System.Collections.Generic;
using Prime31;
using UnityEngine;

public class PlayGameServicesUI : MonoBehaviourGUI
{
	private void Start()
	{
		PlayGameServices.enableDebugLog(true);
		PlayGameServices.init("160040154367.apps.googleusercontent.com", true);
	}

	private void OnGUI()
	{
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		beginColumn();
		GUILayout.Label("Authentication and Settings");
		if (GUILayout.Button("Set Toasts on Bottom"))
		{
			PlayGameServices.setAchievementToastSettings(GPGToastPlacement.Bottom, 50);
		}
		if (GUILayout.Button("Authenticate"))
		{
			PlayGameServices.authenticate();
		}
		if (GUILayout.Button("Sign Out"))
		{
			PlayGameServices.signOut();
		}
		if (GUILayout.Button("Is Signed In"))
		{
			Debug.Log("is signed in? " + PlayGameServices.isSignedIn());
		}
		if (GUILayout.Button("Get Player Info"))
		{
			GPGPlayerInfo localPlayerInfo = PlayGameServices.getLocalPlayerInfo();
			Debug.Log(localPlayerInfo);
			if (Application.platform == RuntimePlatform.Android && localPlayerInfo.avatarUrl != null)
			{
				PlayGameServices.loadProfileImageForUri(localPlayerInfo.avatarUrl);
			}
		}
		GUILayout.Label("Achievements");
		if (GUILayout.Button("Show Achievements"))
		{
			PlayGameServices.showAchievements();
		}
		if (GUILayout.Button("Increment Achievement"))
		{
			PlayGameServices.incrementAchievement("CgkI_-mLmdQEEAIQAQ", 2);
		}
		if (GUILayout.Button("Unlock Achievment"))
		{
			PlayGameServices.unlockAchievement("CgkI_-mLmdQEEAIQAw");
		}
		endColumn(true);
		if (toggleButtonState("Show Cloud Save Buttons"))
		{
			secondColumnButtions();
		}
		else
		{
			cloudSaveButtons();
		}
		toggleButton("Show Cloud Save Buttons", "Toggle Buttons");
		endColumn(false);
	}

	private void secondColumnButtions()
	{
		GUILayout.Label("Leaderboards");
		if (GUILayout.Button("Show Leaderboard"))
		{
			PlayGameServices.showLeaderboard("CgkI_-mLmdQEEAIQBQ");
		}
		if (GUILayout.Button("Show All Leaderboards"))
		{
			PlayGameServices.showLeaderboards();
		}
		if (GUILayout.Button("Submit Score"))
		{
			PlayGameServices.submitScore("CgkI_-mLmdQEEAIQBQ", 567L);
		}
		if (GUILayout.Button("Load Raw Score Data"))
		{
			PlayGameServices.loadScoresForLeaderboard("CgkI_-mLmdQEEAIQBQ", GPGLeaderboardTimeScope.AllTime, false, false);
		}
		if (GUILayout.Button("Get Leaderboard Metadata"))
		{
			List<GPGLeaderboardMetadata> allLeaderboardMetadata = PlayGameServices.getAllLeaderboardMetadata();
			Utils.logObject(allLeaderboardMetadata);
		}
		if (GUILayout.Button("Get Achievement Metadata"))
		{
			List<GPGAchievementMetadata> allAchievementMetadata = PlayGameServices.getAllAchievementMetadata();
			Utils.logObject(allAchievementMetadata);
		}
		if (GUILayout.Button("Reload All Metadata"))
		{
			PlayGameServices.reloadAchievementAndLeaderboardData();
		}
		if (GUILayout.Button("loading spongeBob"))
		{
			Application.LoadLevel("Scene0");
		}
	}

	private void cloudSaveButtons()
	{
		GUILayout.Label("Cloud Data");
		if (GUILayout.Button("Load Cloud Data"))
		{
			PlayGameServices.loadCloudDataForKey(0);
		}
		if (GUILayout.Button("Set Cloud Data"))
		{
			PlayGameServices.setStateData("I'm some data. I could be JSON or XML.", 0);
		}
		if (GUILayout.Button("Update Cloud Data"))
		{
			PlayGameServices.updateCloudDataForKey(0);
		}
		if (GUILayout.Button("Get Cloud Data"))
		{
			string message = PlayGameServices.stateDataForKey(0);
			Debug.Log(message);
		}
		if (GUILayout.Button("Delete Cloud Data"))
		{
			PlayGameServices.deleteCloudDataForKey(0);
		}
		if (GUILayout.Button("Clear Cloud Data"))
		{
			PlayGameServices.clearCloudDataForKey(0);
		}
	}
}
