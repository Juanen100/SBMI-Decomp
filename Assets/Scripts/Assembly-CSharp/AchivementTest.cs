using UnityEngine;

public class AchivementTest : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnGUI()
	{
		float num = 5f;
		float left = 5f;
		float width = ((Screen.width < 800 && Screen.height < 800) ? 160 : 320);
		float num2 = ((Screen.width < 800 && Screen.height < 800) ? 40 : 80);
		float num3 = num2 + 10f;
		if (GUI.Button(new Rect(left, num, width, num2), "Init"))
		{
			ServiceReadyEvent();
			AGSClient.Init(false, true, false);
		}
		if (GUI.Button(new Rect(left, num += num3, width, num2), "show achivement"))
		{
			AGSAchievementsClient.ShowAchievementsOverlay();
		}
		if (GUI.Button(new Rect(left, num += num3, width, num2), "update achiveent"))
		{
			AGSAchievementsClient.UpdateAchievementProgress("achievement1", 50f);
		}
		if (GUI.Button(new Rect(left, num += num3, width, num2), "get userId"))
		{
			AGSProfilesClient.PlayerAliasReceivedEvent += PlayerAliasReceived;
			AGSProfilesClient.PlayerAliasFailedEvent += PlayerAliasFailed;
			AGSProfilesClient.RequestLocalPlayerProfile();
		}
		if (GUI.Button(new Rect(left, num += num3, width, num2), "load"))
		{
			Application.LoadLevel("Scene0");
		}
	}

	private void ServiceReadyHandler()
	{
		Debug.LogError("ServiceReadyHandler");
		UnServiceReadyEvent();
		SubscribeToProfileEvents();
		AGSProfilesClient.RequestLocalPlayerProfile();
	}

	private void ServiceNotReadyHandler(string error)
	{
		Debug.LogError("ServiceNotReadyHandler");
		UnServiceReadyEvent();
	}

	private void ServiceReadyEvent()
	{
		AGSClient.ServiceReadyEvent += ServiceReadyHandler;
		AGSClient.ServiceNotReadyEvent += ServiceNotReadyHandler;
	}

	private void UnServiceReadyEvent()
	{
		AGSClient.ServiceReadyEvent -= ServiceReadyHandler;
		AGSClient.ServiceNotReadyEvent -= ServiceNotReadyHandler;
	}

	private void PlayerAliasReceived(AGSProfile profile)
	{
		Debug.LogError("PlayerAliasReceived");
		Debug.LogError("profile.playerId " + profile.playerId);
		UnsubscribeFromProfileEvents();
	}

	private void PlayerAliasFailed(string errorMessage)
	{
		Debug.LogError("PlayerAliasFailed " + errorMessage);
		UnsubscribeFromProfileEvents();
	}

	private void SubscribeToProfileEvents()
	{
		AGSProfilesClient.PlayerAliasReceivedEvent += PlayerAliasReceived;
		AGSProfilesClient.PlayerAliasFailedEvent += PlayerAliasFailed;
	}

	private void UnsubscribeFromProfileEvents()
	{
		AGSProfilesClient.PlayerAliasReceivedEvent -= PlayerAliasReceived;
		AGSProfilesClient.PlayerAliasFailedEvent -= PlayerAliasFailed;
	}
}
