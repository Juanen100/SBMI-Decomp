using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class AmazonGameCircleExampleLeaderboards : AmazonGameCircleExampleBase
{
	private Dictionary<string, string> leaderboardsSubmissionStatus;

	private Dictionary<string, string> leaderboardsSubmissionStatusMessage;

	private Dictionary<string, string> leaderboardsLocalScoreStatus;

	private Dictionary<string, string> leaderboardsLocalScoreStatusMessage;

	private Dictionary<string, bool> leaderboardsFoldout;

	private long leaderboardScoreValue;

	private string requestLeaderboardsStatus;

	private string requestLeaderboardsStatusMessage;

	private List<AGSLeaderboard> leaderboardList;

	private bool leaderboardsReady;

	private DateTime leaderboardsRequestTime;

	private LeaderboardScope leaderboardScoreScope;

	private AGSLeaderboard invalidLeaderboard;

	private readonly Regex addNewlineEverySecondCommaRegex;

	private const int betweenCommaRegexGroup = 2;

	private const string leaderboardsMenuTitle = "Leaderboards";

	private const string DisplayLeaderboardOverlayButtonLabel = "Leaderboards Overlay";

	private const string requestLeaderboardsButtonLabel = "Request Leaderboards";

	private const string requestingLeaderboardsLabel = "Requesting Leaderboards...";

	private const string requestLeaderboardsFailedLabel = "Request Leaderboards failed with error:";

	private const string requestLeaderboardsSucceededLabel = "Available Leaderboards";

	private const string noLeaderboardsAvailableLabel = "No Leaderboards Available";

	private const string leaderboardIDLabel = "Leaderboard \"{0}\"";

	private const string leaderboardRequestTimeLabel = "{0,5:N1} seconds";

	private const string leaderboardScoreDisplayLabel = "{0} score units";

	private const string submitLeaderboardButtonLabel = "Submit Score";

	private const string leaderboardFailed = "Leaderboard \"{0}\" failed with error:";

	private const string leaderboardSucceeded = "Score uploaded to \"{0}\" successfully.";

	private const string requestLeaderboardScoreButtonLabel = "Request Score";

	private const string leaderboardRankScoreLabel = "Rank {0} with score of {1,5:N1}";

	private const string leaderboardScoreFailed = "\"{0}\" score request failed with error:";

	private const float leaderboardMinValue = -10000f;

	private const float leaderboardMaxValue = 10000f;

	public override string MenuTitle()
	{
		return null;
	}

	public override void DrawMenu()
	{
	}

	private void DisplayLeaderboard(AGSLeaderboard leaderboard)
	{
	}

	private string AddNewlineEverySecondComma(string stringToChange)
	{
		return null;
	}

	private void RequestLeaderboards()
	{
	}

	private void SubmitScoreToLeaderboard(string leaderboardId, long scoreValue)
	{
	}

	private void RequestLocalPlayerScore(string leaderboardId)
	{
	}

	private void SubscribeToLeaderboardRequestEvents()
	{
	}

	private void UnsubscribeFromLeaderboardRequestEvents()
	{
	}

	private void SubscribeToScoreSubmissionEvents()
	{
	}

	private void UnsubscribeFromScoreSubmissionEvents()
	{
	}

	private void SubscribeToLocalPlayerScoreRequestEvents()
	{
	}

	private void UnsubscribeFromLocalPlayerScoreRequestEvents()
	{
	}

	private void RequestLeaderboardsFailed(string error)
	{
	}

	private void RequestLeaderboardsSucceeded(List<AGSLeaderboard> leaderboards)
	{
	}

	private void SubmitScoreFailed(string leaderboardId, string error)
	{
	}

	private void SubmitScoreSucceeded(string leaderboardId)
	{
	}

	private void RequestLocalPlayerScoreFailed(string leaderboardId, string error)
	{
	}

	private void RequestLocalPlayerScoreSucceeded(string leaderboardId, int rank, long score)
	{
	}
}
