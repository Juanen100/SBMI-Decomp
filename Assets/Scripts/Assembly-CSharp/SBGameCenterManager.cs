using System.Collections.Generic;
using UnityEngine;

public class SBGameCenterManager : MonoBehaviour
{
	/*public List<GameCenterAchievementMetadata> achievementMetadata = new List<GameCenterAchievementMetadata>();

	public List<GameCenterAchievement> achievements = new List<GameCenterAchievement>();*/

	private void Awake()
	{
		Object.DontDestroyOnLoad(this);
	}

	private void Start()
	{
		/*GameCenterBinding.showCompletionBannerForAchievements();
		GameCenterManager.loadPlayerDataFailedEvent += LoadPlayerDataFailed;
		GameCenterManager.playerAuthenticatedEvent += PlayerAuthenticated;
		GameCenterManager.playerFailedToAuthenticateEvent += PlayerFailedToAuthenticate;
		GameCenterManager.playerLoggedOutEvent += PlayerLoggedOut;
		GameCenterManager.profilePhotoLoadedEvent += ProfilePhotoLoaded;
		GameCenterManager.profilePhotoFailedEvent += ProfilePhotoFailed;
		GameCenterManager.reportAchievementFailedEvent += ReportAchievementFailed;
		GameCenterManager.reportAchievementFinishedEvent += ReportAchievementFinished;
		GameCenterManager.loadAchievementsFailedEvent += LoadAchievementsFailed;
		GameCenterManager.achievementsLoadedEvent += AchievementsLoaded;
		GameCenterManager.resetAchievementsFailedEvent += ResetAchievementsFailed;
		GameCenterManager.resetAchievementsFinishedEvent += ResetAchievementsFinished;
		GameCenterManager.retrieveAchievementMetadataFailedEvent += RetrieveAchievementMetadataFailed;
		GameCenterManager.achievementMetadataLoadedEvent += AchievementMetadataLoaded;*/
	}

	private void OnDisable()
	{
		/*GameCenterManager.loadPlayerDataFailedEvent -= LoadPlayerDataFailed;
		GameCenterManager.playerAuthenticatedEvent -= PlayerAuthenticated;
		GameCenterManager.playerLoggedOutEvent -= PlayerLoggedOut;
		GameCenterManager.profilePhotoLoadedEvent -= ProfilePhotoLoaded;
		GameCenterManager.profilePhotoFailedEvent -= ProfilePhotoFailed;
		GameCenterManager.reportAchievementFailedEvent -= ReportAchievementFailed;
		GameCenterManager.reportAchievementFinishedEvent -= ReportAchievementFinished;
		GameCenterManager.loadAchievementsFailedEvent -= LoadAchievementsFailed;
		GameCenterManager.achievementsLoadedEvent -= AchievementsLoaded;
		GameCenterManager.resetAchievementsFailedEvent -= ResetAchievementsFailed;
		GameCenterManager.resetAchievementsFinishedEvent -= ResetAchievementsFinished;
		GameCenterManager.retrieveAchievementMetadataFailedEvent -= RetrieveAchievementMetadataFailed;
		GameCenterManager.achievementMetadataLoadedEvent -= AchievementMetadataLoaded;
		GameCenterBinding.isPlayerAuthenticated();*/
	}

	private void ResetLocalPlayer()
	{
		/*achievementMetadata.Clear();
		achievements.Clear();*/
	}

	public void ReportAchievement(string achievementId, float percentComplete)
	{
		/*if (!GameCenterBinding.isPlayerAuthenticated())
		{
			return;
		}
		foreach (GameCenterAchievement achievement in achievements)
		{
			if (achievement.identifier == achievementId)
			{
				TFUtils.DebugLog("Achievement Already Earned");
				return;
			}
		}
		TFUtils.DebugLog("Achievement Earned - " + achievementId);
		GameCenterBinding.reportAchievement(achievementId, percentComplete);*/
	}

	public void ResetAchievements()
	{
		/*if (GameCenterBinding.isPlayerAuthenticated())
		{
			GameCenterBinding.resetAchievements();
			achievements.Clear();
		}*/
	}

	public void PlayerAuthenticated()
	{
		/*TFUtils.DebugLog("Player Authenticated - Attempting to load achievements and leaderboards");
		GameCenterBinding.loadProfilePhotoForLocalPlayer();
		GameCenterBinding.getAchievements();
		GameCenterBinding.retrieveAchievementMetadata();*/
	}

	private void PlayerFailedToAuthenticate(string error)
	{
		TFUtils.DebugLog("PlayerFailedToAuthenticate: " + error);
	}

	private void PlayerLoggedOut()
	{
		TFUtils.DebugLog("playerLoggedOut");
		ResetLocalPlayer();
	}

	private void LoadPlayerDataFailed(string error)
	{
		TFUtils.DebugLog("LoadPlayerDataFailed: " + error);
	}

	private void ProfilePhotoLoaded(string path)
	{
		TFUtils.DebugLog("ProfilePhotoLoaded: " + path);
	}

	private void ProfilePhotoFailed(string error)
	{
		TFUtils.DebugLog("ProfilePhotoFailed: " + error);
	}

	/*private void AchievementMetadataLoaded(List<GameCenterAchievementMetadata> achievementMetadata)
	{
		/*this.achievementMetadata = achievementMetadata;
		TFUtils.DebugLog("achievementMetadatLoaded");
		foreach (GameCenterAchievementMetadata achievementMetadatum in achievementMetadata)
		{
			TFUtils.DebugLog(achievementMetadatum);
		}
	}*/

	private void RetrieveAchievementMetadataFailed(string error)
	{
		TFUtils.DebugLog("RetrieveAchievementMetadataFailed: " + error);
	}

	private void ResetAchievementsFinished()
	{
		TFUtils.DebugLog("resetAchievmenetsFinished");
	}

	private void ResetAchievementsFailed(string error)
	{
		TFUtils.DebugLog("ResetAchievementsFailed: " + error);
	}

	/*private void AchievementsLoaded(List<GameCenterAchievement> achievements)
	{
		/*this.achievements = achievements;
		TFUtils.DebugLog("AchievementsLoaded");
		foreach (GameCenterAchievement achievement in achievements)
		{
			TFUtils.DebugLog(achievement);
		}
	}*/

	private void LoadAchievementsFailed(string error)
	{
		TFUtils.DebugLog("LoadAchievementsFailed: " + error);
	}

	private void ReportAchievementFinished(string identifier)
	{
		TFUtils.DebugLog("ReportAchievementFinished: " + identifier);
		//GameCenterBinding.getAchievements();
	}

	private void ReportAchievementFailed(string error)
	{
		TFUtils.DebugLog("ReportAchievementFailed: " + error);
	}
}
