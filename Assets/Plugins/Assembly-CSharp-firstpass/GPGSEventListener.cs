using System.Collections.Generic;
using Prime31;
using UnityEngine;

public class GPGSEventListener : MonoBehaviour
{
	private void OnEnable()
	{
		GPGManager.authenticationSucceededEvent += authenticationSucceededEvent;
		GPGManager.authenticationFailedEvent += authenticationFailedEvent;
		GPGManager.licenseCheckFailedEvent += licenseCheckFailedEvent;
		GPGManager.profileImageLoadedAtPathEvent += profileImageLoadedAtPathEvent;
		GPGManager.userSignedOutEvent += userSignedOutEvent;
		GPGManager.reloadDataForKeyFailedEvent += reloadDataForKeyFailedEvent;
		GPGManager.reloadDataForKeySucceededEvent += reloadDataForKeySucceededEvent;
		GPGManager.loadCloudDataForKeyFailedEvent += loadCloudDataForKeyFailedEvent;
		GPGManager.loadCloudDataForKeySucceededEvent += loadCloudDataForKeySucceededEvent;
		GPGManager.updateCloudDataForKeyFailedEvent += updateCloudDataForKeyFailedEvent;
		GPGManager.updateCloudDataForKeySucceededEvent += updateCloudDataForKeySucceededEvent;
		GPGManager.clearCloudDataForKeyFailedEvent += clearCloudDataForKeyFailedEvent;
		GPGManager.clearCloudDataForKeySucceededEvent += clearCloudDataForKeySucceededEvent;
		GPGManager.deleteCloudDataForKeyFailedEvent += deleteCloudDataForKeyFailedEvent;
		GPGManager.deleteCloudDataForKeySucceededEvent += deleteCloudDataForKeySucceededEvent;
		GPGManager.unlockAchievementFailedEvent += unlockAchievementFailedEvent;
		GPGManager.unlockAchievementSucceededEvent += unlockAchievementSucceededEvent;
		GPGManager.incrementAchievementFailedEvent += incrementAchievementFailedEvent;
		GPGManager.incrementAchievementSucceededEvent += incrementAchievementSucceededEvent;
		GPGManager.revealAchievementFailedEvent += revealAchievementFailedEvent;
		GPGManager.revealAchievementSucceededEvent += revealAchievementSucceededEvent;
		GPGManager.submitScoreFailedEvent += submitScoreFailedEvent;
		GPGManager.submitScoreSucceededEvent += submitScoreSucceededEvent;
		GPGManager.loadScoresFailedEvent += loadScoresFailedEvent;
		GPGManager.loadScoresSucceededEvent += loadScoresSucceededEvent;
	}

	private void OnDisable()
	{
		GPGManager.authenticationSucceededEvent -= authenticationSucceededEvent;
		GPGManager.authenticationFailedEvent -= authenticationFailedEvent;
		GPGManager.licenseCheckFailedEvent -= licenseCheckFailedEvent;
		GPGManager.profileImageLoadedAtPathEvent -= profileImageLoadedAtPathEvent;
		GPGManager.userSignedOutEvent -= userSignedOutEvent;
		GPGManager.reloadDataForKeyFailedEvent -= reloadDataForKeyFailedEvent;
		GPGManager.reloadDataForKeySucceededEvent -= reloadDataForKeySucceededEvent;
		GPGManager.loadCloudDataForKeyFailedEvent -= loadCloudDataForKeyFailedEvent;
		GPGManager.loadCloudDataForKeySucceededEvent -= loadCloudDataForKeySucceededEvent;
		GPGManager.updateCloudDataForKeyFailedEvent -= updateCloudDataForKeyFailedEvent;
		GPGManager.updateCloudDataForKeySucceededEvent -= updateCloudDataForKeySucceededEvent;
		GPGManager.clearCloudDataForKeyFailedEvent -= clearCloudDataForKeyFailedEvent;
		GPGManager.clearCloudDataForKeySucceededEvent -= clearCloudDataForKeySucceededEvent;
		GPGManager.deleteCloudDataForKeyFailedEvent -= deleteCloudDataForKeyFailedEvent;
		GPGManager.deleteCloudDataForKeySucceededEvent -= deleteCloudDataForKeySucceededEvent;
		GPGManager.unlockAchievementFailedEvent -= unlockAchievementFailedEvent;
		GPGManager.unlockAchievementSucceededEvent -= unlockAchievementSucceededEvent;
		GPGManager.incrementAchievementFailedEvent -= incrementAchievementFailedEvent;
		GPGManager.incrementAchievementSucceededEvent -= incrementAchievementSucceededEvent;
		GPGManager.revealAchievementFailedEvent -= revealAchievementFailedEvent;
		GPGManager.revealAchievementSucceededEvent -= revealAchievementSucceededEvent;
		GPGManager.submitScoreFailedEvent -= submitScoreFailedEvent;
		GPGManager.submitScoreSucceededEvent -= submitScoreSucceededEvent;
		GPGManager.loadScoresFailedEvent -= loadScoresFailedEvent;
		GPGManager.loadScoresSucceededEvent -= loadScoresSucceededEvent;
	}

	private void authenticationSucceededEvent(string param)
	{
		Debug.Log("authenticationSucceededEvent: " + param);
		Debug.Log("Player Authenticated - Attempting to load achievements and leaderboards");
		/*GameCenterBinding.loadProfilePhotoForLocalPlayer();
		GameCenterBinding.getAchievements();
		GameCenterBinding.retrieveAchievementMetadata();*/
	}

	private void authenticationFailedEvent(string error)
	{
		Debug.Log("authenticationFailedEvent: " + error);
	}

	private void licenseCheckFailedEvent()
	{
		Debug.Log("licenseCheckFailedEvent");
	}

	private void profileImageLoadedAtPathEvent(string path)
	{
		Debug.Log("profileImageLoadedAtPathEvent: " + path);
	}

	private void userSignedOutEvent()
	{
		Debug.Log("userSignedOutEvent");
	}

	private void reloadDataForKeyFailedEvent(string error)
	{
		Debug.Log("reloadDataForKeyFailedEvent: " + error);
	}

	private void reloadDataForKeySucceededEvent(string param)
	{
		Debug.Log("reloadDataForKeySucceededEvent: " + param);
	}

	private void loadCloudDataForKeyFailedEvent(string error)
	{
		Debug.Log("loadCloudDataForKeyFailedEvent: " + error);
	}

	private void loadCloudDataForKeySucceededEvent(int key, string data)
	{
		Debug.Log("loadCloudDataForKeySucceededEvent:" + data);
	}

	private void updateCloudDataForKeyFailedEvent(string error)
	{
		Debug.Log("updateCloudDataForKeyFailedEvent: " + error);
	}

	private void updateCloudDataForKeySucceededEvent(int key, string data)
	{
		Debug.Log("updateCloudDataForKeySucceededEvent: " + data);
	}

	private void clearCloudDataForKeyFailedEvent(string error)
	{
		Debug.Log("clearCloudDataForKeyFailedEvent: " + error);
	}

	private void clearCloudDataForKeySucceededEvent(string param)
	{
		Debug.Log("clearCloudDataForKeySucceededEvent: " + param);
	}

	private void deleteCloudDataForKeyFailedEvent(string error)
	{
		Debug.Log("deleteCloudDataForKeyFailedEvent: " + error);
	}

	private void deleteCloudDataForKeySucceededEvent(string param)
	{
		Debug.Log("deleteCloudDataForKeySucceededEvent: " + param);
	}

	private void unlockAchievementFailedEvent(string achievementId, string error)
	{
		Debug.Log("unlockAchievementFailedEvent. achievementId: " + achievementId + ", error: " + error);
	}

	private void unlockAchievementSucceededEvent(string achievementId, bool newlyUnlocked)
	{
		Debug.Log("unlockAchievementSucceededEvent. achievementId: " + achievementId + ", newlyUnlocked: " + newlyUnlocked);
	}

	private void incrementAchievementFailedEvent(string achievementId, string error)
	{
		Debug.Log("incrementAchievementFailedEvent. achievementId: " + achievementId + ", error: " + error);
	}

	private void incrementAchievementSucceededEvent(string achievementId, bool newlyUnlocked)
	{
		Debug.Log("incrementAchievementSucceededEvent. achievementId: " + achievementId + ", newlyUnlocked: " + newlyUnlocked);
	}

	private void revealAchievementFailedEvent(string achievementId, string error)
	{
		Debug.Log("revealAchievementFailedEvent. achievementId: " + achievementId + ", error: " + error);
	}

	private void revealAchievementSucceededEvent(string achievementId)
	{
		Debug.Log("revealAchievementSucceededEvent: " + achievementId);
		//GameCenterBinding.getAchievements();
	}

	private void submitScoreFailedEvent(string leaderboardId, string error)
	{
		Debug.Log("submitScoreFailedEvent. leaderboardId: " + leaderboardId + ", error: " + error);
	}

	private void submitScoreSucceededEvent(string leaderboardId, Dictionary<string, object> scoreReport)
	{
		Debug.Log("submitScoreSucceededEvent");
		Utils.logObject(scoreReport);
	}

	private void loadScoresFailedEvent(string leaderboardId, string error)
	{
		Debug.Log("loadScoresFailedEvent. leaderboardId: " + leaderboardId + ", error: " + error);
	}

	private void loadScoresSucceededEvent(List<GPGScore> scores)
	{
		Debug.Log("loadScoresSucceededEvent");
		Utils.logObject(scores);
	}
}
