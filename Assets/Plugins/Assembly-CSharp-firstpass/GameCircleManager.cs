using UnityEngine;

public class GameCircleManager : MonoBehaviour
{
	public static GameCircleManager instance;

	public static GameCircleManager getInstance()
	{
		return null;
	}

	private void Awake()
	{
	}

	public void serviceReady(string empty)
	{
	}

	public void serviceNotReady(string param)
	{
	}

	public void playerAliasReceived(string json)
	{
	}

	public void playerAliasFailed(string json)
	{
	}

	public void submitScoreFailed(string json)
	{
	}

	public void submitScoreSucceeded(string json)
	{
	}

	public void requestLeaderboardsFailed(string json)
	{
	}

	public void requestLeaderboardsSucceeded(string json)
	{
	}

	public void requestLocalPlayerScoreFailed(string json)
	{
	}

	public void requestLocalPlayerScoreSucceeded(string json)
	{
	}

	public void updateAchievementSucceeded(string json)
	{
	}

	public void updateAchievementFailed(string json)
	{
	}

	public void requestAchievementsSucceeded(string json)
	{
	}

	public void requestAchievementsFailed(string json)
	{
	}

	public void onNewCloudData(string empty)
	{
	}

	public void OnApplicationFocus(bool focusStatus)
	{
	}
}
