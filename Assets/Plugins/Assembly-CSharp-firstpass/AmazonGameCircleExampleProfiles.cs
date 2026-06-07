public class AmazonGameCircleExampleProfiles : AmazonGameCircleExampleBase
{
	private string playerProfileStatus;

	private string playerProfileStatusMessage;

	private AGSProfile playerProfile;

	private const string profileMenuTitle = "User Profile";

	private const string playerAliasReceivedLabel = "Retrieved local player data";

	private const string playerAliasFailedLabel = "Failed to retrieve local player data";

	private const string playerAliasRetrieveButtonLabel = "Retrieve local player data";

	private const string playerProfileLabel = "ID {0} : Alias {1}";

	private const string playerAliasRetrievingLabel = "Retrieving local player data...";

	private const string nullAsString = "null";

	public override string MenuTitle()
	{
		return null;
	}

	public override void DrawMenu()
	{
	}

	private void RequestLocalPlayerData()
	{
	}

	private void SubscribeToProfileEvents()
	{
	}

	private void UnsubscribeFromProfileEvents()
	{
	}

	private void PlayerAliasReceived(AGSProfile profile)
	{
	}

	private void PlayerAliasFailed(string errorMessage)
	{
	}
}
