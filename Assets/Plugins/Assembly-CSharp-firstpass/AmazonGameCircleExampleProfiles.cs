using UnityEngine;

public class AmazonGameCircleExampleProfiles : AmazonGameCircleExampleBase
{
	private const string profileMenuTitle = "User Profile";

	private const string playerAliasReceivedLabel = "Retrieved local player data";

	private const string playerAliasFailedLabel = "Failed to retrieve local player data";

	private const string playerAliasRetrieveButtonLabel = "Retrieve local player data";

	private const string playerProfileLabel = "ID {0} : Alias {1}";

	private const string playerAliasRetrievingLabel = "Retrieving local player data...";

	private const string nullAsString = "null";

	private string playerProfileStatus;

	private string playerProfileStatusMessage;

	private AGSProfile playerProfile;

	public override string MenuTitle()
	{
		return "User Profile";
	}

	public override void DrawMenu()
	{
		if (string.IsNullOrEmpty(playerProfileStatus))
		{
			if (GUILayout.Button("Retrieve local player data"))
			{
				RequestLocalPlayerData();
			}
			return;
		}
		AmazonGameCircleExampleGUIHelpers.CenteredLabel(playerProfileStatus);
		if (!string.IsNullOrEmpty(playerProfileStatusMessage))
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel(playerProfileStatusMessage);
		}
		if (playerProfile != null)
		{
			string arg = (string.IsNullOrEmpty(playerProfile.playerId) ? "null" : playerProfile.playerId);
			string arg2 = (string.IsNullOrEmpty(playerProfile.alias) ? "null" : playerProfile.alias);
			AmazonGameCircleExampleGUIHelpers.CenteredLabel(string.Format("ID {0} : Alias {1}", arg, arg2));
		}
	}

	private void RequestLocalPlayerData()
	{
		SubscribeToProfileEvents();
		AGSProfilesClient.RequestLocalPlayerProfile();
		playerProfileStatus = "Retrieving local player data...";
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

	private void PlayerAliasReceived(AGSProfile profile)
	{
		playerProfileStatus = "Retrieved local player data";
		playerProfileStatusMessage = null;
		playerProfile = profile;
		UnsubscribeFromProfileEvents();
	}

	private void PlayerAliasFailed(string errorMessage)
	{
		playerProfileStatus = "Failed to retrieve local player data";
		playerProfileStatusMessage = errorMessage;
		UnsubscribeFromProfileEvents();
	}
}
