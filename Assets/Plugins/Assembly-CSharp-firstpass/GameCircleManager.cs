using UnityEngine;

public class GameCircleManager : MonoBehaviour
{
	public static GameCircleManager instance;

	public static GameCircleManager getInstance()
	{
		if (instance == null)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = "GameCircleManager";
			instance = gameObject.AddComponent<GameCircleManager>();
		}
		return instance;
	}

	private void Awake()
	{
		base.gameObject.name = GetType().ToString();
		Object.DontDestroyOnLoad(this);
	}

	public void serviceReady(string empty)
	{
		AGSClient.Log("GameCircleManager - serviceReady");
		AGSClient.ServiceReady(empty);
	}

	public void serviceNotReady(string param)
	{
		AGSClient.Log("GameCircleManager - serviceNotReady");
		AGSClient.ServiceNotReady(param);
	}

	public void playerAliasReceived(string json)
	{
		AGSClient.Log("GameCircleManager - playerAliasReceived");
		AGSProfilesClient.PlayerAliasReceived(json);
	}

	public void playerAliasFailed(string json)
	{
		AGSClient.Log("GameCircleManager - playerAliasFailed");
		AGSProfilesClient.PlayerAliasFailed(json);
	}

	public void submitScoreFailed(string json)
	{
		AGSClient.Log("GameCircleManager - submitScoreFailed");
		AGSLeaderboardsClient.SubmitScoreFailed(json);
	}

	public void submitScoreSucceeded(string json)
	{
		AGSClient.Log("GameCircleManager - submitScoreSucceeded");
		AGSLeaderboardsClient.SubmitScoreSucceeded(json);
	}

	public void requestLeaderboardsFailed(string json)
	{
		AGSClient.Log("GameCircleManager - requestLeaderboardsFailed");
		AGSLeaderboardsClient.RequestLeaderboardsFailed(json);
	}

	public void requestLeaderboardsSucceeded(string json)
	{
		AGSClient.Log("GameCircleManager - requestLeaderboardsSucceeded");
		AGSLeaderboardsClient.RequestLeaderboardsSucceeded(json);
	}

	public void requestLocalPlayerScoreFailed(string json)
	{
		AGSClient.Log("GameCircleManager - requestLocalPlayerScoreFailed");
		AGSLeaderboardsClient.RequestLocalPlayerScoreFailed(json);
	}

	public void requestLocalPlayerScoreSucceeded(string json)
	{
		AGSClient.Log("GameCircleManager - requestLocalPlayerScoreSucceeded");
		AGSLeaderboardsClient.RequestLocalPlayerScoreSucceeded(json);
	}

	public void updateAchievementSucceeded(string json)
	{
		AGSClient.Log("GameCircleManager - updateAchievementSucceeded");
		AGSAchievementsClient.UpdateAchievementSucceeded(json);
	}

	public void updateAchievementFailed(string json)
	{
		AGSClient.Log("GameCircleManager - updateAchievementsFailed");
		AGSAchievementsClient.UpdateAchievementFailed(json);
	}

	public void requestAchievementsSucceeded(string json)
	{
		AGSClient.Log("GameCircleManager - requestAchievementsSucceeded");
		AGSAchievementsClient.RequestAchievementsSucceeded(json);
	}

	public void requestAchievementsFailed(string json)
	{
		AGSClient.Log("GameCircleManager -  requestAchievementsFailed");
		AGSAchievementsClient.RequestAchievementsFailed(json);
	}

	public void onNewCloudData(string empty)
	{
		AGSWhispersyncClient.OnNewCloudData();
	}

	public void OnApplicationFocus(bool focusStatus)
	{
		if (AGSClient.IsServiceReady() && !focusStatus)
		{
			AGSClient.release();
		}
	}
}
