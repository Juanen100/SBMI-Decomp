using System.Collections.Generic;
using UnityEngine;

public class GPGSEventListener : MonoBehaviour
{
	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void authenticationSucceededEvent(string param)
	{
	}

	private void authenticationFailedEvent(string error)
	{
	}

	private void licenseCheckFailedEvent()
	{
	}

	private void profileImageLoadedAtPathEvent(string path)
	{
	}

	private void userSignedOutEvent()
	{
	}

	private void reloadDataForKeyFailedEvent(string error)
	{
	}

	private void reloadDataForKeySucceededEvent(string param)
	{
	}

	private void loadCloudDataForKeyFailedEvent(string error)
	{
	}

	private void loadCloudDataForKeySucceededEvent(int key, string data)
	{
	}

	private void updateCloudDataForKeyFailedEvent(string error)
	{
	}

	private void updateCloudDataForKeySucceededEvent(int key, string data)
	{
	}

	private void clearCloudDataForKeyFailedEvent(string error)
	{
	}

	private void clearCloudDataForKeySucceededEvent(string param)
	{
	}

	private void deleteCloudDataForKeyFailedEvent(string error)
	{
	}

	private void deleteCloudDataForKeySucceededEvent(string param)
	{
	}

	private void unlockAchievementFailedEvent(string achievementId, string error)
	{
	}

	private void unlockAchievementSucceededEvent(string achievementId, bool newlyUnlocked)
	{
	}

	private void incrementAchievementFailedEvent(string achievementId, string error)
	{
	}

	private void incrementAchievementSucceededEvent(string achievementId, bool newlyUnlocked)
	{
	}

	private void revealAchievementFailedEvent(string achievementId, string error)
	{
	}

	private void revealAchievementSucceededEvent(string achievementId)
	{
	}

	private void submitScoreFailedEvent(string leaderboardId, string error)
	{
	}

	private void submitScoreSucceededEvent(string leaderboardId, Dictionary<string, object> scoreReport)
	{
	}

	private void loadScoresFailedEvent(string leaderboardId, string error)
	{
	}

	private void loadScoresSucceededEvent(List<GPGScore> scores)
	{
	}
}
