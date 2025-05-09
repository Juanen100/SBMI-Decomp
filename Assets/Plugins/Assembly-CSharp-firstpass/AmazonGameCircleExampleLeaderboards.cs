using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class AmazonGameCircleExampleLeaderboards : AmazonGameCircleExampleBase
{
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

	private Dictionary<string, string> leaderboardsSubmissionStatus = new Dictionary<string, string>();

	private Dictionary<string, string> leaderboardsSubmissionStatusMessage = new Dictionary<string, string>();

	private Dictionary<string, string> leaderboardsLocalScoreStatus = new Dictionary<string, string>();

	private Dictionary<string, string> leaderboardsLocalScoreStatusMessage = new Dictionary<string, string>();

	private Dictionary<string, bool> leaderboardsFoldout = new Dictionary<string, bool>();

	private long leaderboardScoreValue;

	private string requestLeaderboardsStatus;

	private string requestLeaderboardsStatusMessage;

	private List<AGSLeaderboard> leaderboardList;

	private bool leaderboardsReady;

	private DateTime leaderboardsRequestTime;

	private LeaderboardScope leaderboardScoreScope;

	private AGSLeaderboard invalidLeaderboard;

	private readonly Regex addNewlineEverySecondCommaRegex = new Regex("(,([^,]+),)");

	public AmazonGameCircleExampleLeaderboards()
	{
		invalidLeaderboard = new AGSLeaderboard();
		invalidLeaderboard.id = "Invalid Leaderboard ID";
	}

	public override string MenuTitle()
	{
		return "Leaderboards";
	}

	public override void DrawMenu()
	{
		if (GUILayout.Button("Leaderboards Overlay"))
		{
			AGSLeaderboardsClient.ShowLeaderboardsOverlay();
		}
		if (string.IsNullOrEmpty(requestLeaderboardsStatus))
		{
			if (GUILayout.Button("Request Leaderboards"))
			{
				RequestLeaderboards();
			}
			return;
		}
		AmazonGameCircleExampleGUIHelpers.CenteredLabel(requestLeaderboardsStatus);
		if (!string.IsNullOrEmpty(requestLeaderboardsStatusMessage))
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel(requestLeaderboardsStatusMessage);
		}
		if (!leaderboardsReady)
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel(string.Format("{0,5:N1} seconds", (DateTime.Now - leaderboardsRequestTime).TotalSeconds));
			return;
		}
		if (leaderboardList != null && leaderboardList.Count > 0)
		{
			foreach (AGSLeaderboard leaderboard in leaderboardList)
			{
				DisplayLeaderboard(leaderboard);
			}
		}
		else
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel("No Leaderboards Available");
		}
		if (invalidLeaderboard != null)
		{
			DisplayLeaderboard(invalidLeaderboard);
		}
	}

	private void DisplayLeaderboard(AGSLeaderboard leaderboard)
	{
		GUILayout.BeginVertical(GUI.skin.box);
		if (!leaderboardsFoldout.ContainsKey(leaderboard.id))
		{
			leaderboardsFoldout.Add(leaderboard.id, false);
		}
		leaderboardsFoldout[leaderboard.id] = AmazonGameCircleExampleGUIHelpers.FoldoutWithLabel(leaderboardsFoldout[leaderboard.id], string.Format("Leaderboard \"{0}\"", leaderboard.id));
		if (leaderboardsFoldout[leaderboard.id])
		{
			AmazonGameCircleExampleGUIHelpers.AnchoredLabel(AddNewlineEverySecondComma(leaderboard.ToString()), TextAnchor.UpperCenter);
			leaderboardScoreValue = (long)AmazonGameCircleExampleGUIHelpers.DisplayCenteredSlider(leaderboardScoreValue, -10000f, 10000f, "{0} score units");
			if (leaderboardsSubmissionStatus.ContainsKey(leaderboard.id) && !string.IsNullOrEmpty(leaderboardsSubmissionStatus[leaderboard.id]))
			{
				AmazonGameCircleExampleGUIHelpers.CenteredLabel(leaderboardsSubmissionStatus[leaderboard.id]);
				if (leaderboardsSubmissionStatusMessage.ContainsKey(leaderboard.id) && !string.IsNullOrEmpty(leaderboardsSubmissionStatusMessage[leaderboard.id]))
				{
					AmazonGameCircleExampleGUIHelpers.CenteredLabel(leaderboardsSubmissionStatusMessage[leaderboard.id]);
				}
			}
			if (GUILayout.Button("Submit Score"))
			{
				SubmitScoreToLeaderboard(leaderboard.id, leaderboardScoreValue);
			}
			if (leaderboardsLocalScoreStatus.ContainsKey(leaderboard.id) && !string.IsNullOrEmpty(leaderboardsLocalScoreStatus[leaderboard.id]))
			{
				AmazonGameCircleExampleGUIHelpers.AnchoredLabel(leaderboardsLocalScoreStatus[leaderboard.id], TextAnchor.UpperCenter);
				if (leaderboardsLocalScoreStatusMessage.ContainsKey(leaderboard.id) && !string.IsNullOrEmpty(leaderboardsLocalScoreStatusMessage[leaderboard.id]))
				{
					AmazonGameCircleExampleGUIHelpers.AnchoredLabel(leaderboardsLocalScoreStatusMessage[leaderboard.id], TextAnchor.UpperCenter);
				}
			}
			if (GUILayout.Button("Request Score"))
			{
				RequestLocalPlayerScore(leaderboard.id);
			}
		}
		GUILayout.EndVertical();
	}

	private string AddNewlineEverySecondComma(string stringToChange)
	{
		return addNewlineEverySecondCommaRegex.Replace(stringToChange, (Match regexMatchEvaluator) => "," + regexMatchEvaluator.Groups[2].Value + ",\n");
	}

	private void RequestLeaderboards()
	{
		leaderboardsRequestTime = DateTime.Now;
		SubscribeToLeaderboardRequestEvents();
		AGSLeaderboardsClient.RequestLeaderboards();
		requestLeaderboardsStatus = "Requesting Leaderboards...";
	}

	private void SubmitScoreToLeaderboard(string leaderboardId, long scoreValue)
	{
		SubscribeToScoreSubmissionEvents();
		AGSLeaderboardsClient.SubmitScore(leaderboardId, scoreValue);
	}

	private void RequestLocalPlayerScore(string leaderboardId)
	{
		SubscribeToLocalPlayerScoreRequestEvents();
		AGSLeaderboardsClient.RequestLocalPlayerScore(leaderboardId, leaderboardScoreScope);
	}

	private void SubscribeToLeaderboardRequestEvents()
	{
		AGSLeaderboardsClient.RequestLeaderboardsFailedEvent += RequestLeaderboardsFailed;
		AGSLeaderboardsClient.RequestLeaderboardsSucceededEvent += RequestLeaderboardsSucceeded;
	}

	private void UnsubscribeFromLeaderboardRequestEvents()
	{
		AGSLeaderboardsClient.RequestLeaderboardsFailedEvent -= RequestLeaderboardsFailed;
		AGSLeaderboardsClient.RequestLeaderboardsSucceededEvent -= RequestLeaderboardsSucceeded;
	}

	private void SubscribeToScoreSubmissionEvents()
	{
		AGSLeaderboardsClient.SubmitScoreFailedEvent += SubmitScoreFailed;
		AGSLeaderboardsClient.SubmitScoreSucceededEvent += SubmitScoreSucceeded;
	}

	private void UnsubscribeFromScoreSubmissionEvents()
	{
		AGSLeaderboardsClient.SubmitScoreFailedEvent -= SubmitScoreFailed;
		AGSLeaderboardsClient.SubmitScoreSucceededEvent -= SubmitScoreSucceeded;
	}

	private void SubscribeToLocalPlayerScoreRequestEvents()
	{
		AGSLeaderboardsClient.RequestLocalPlayerScoreFailedEvent += RequestLocalPlayerScoreFailed;
		AGSLeaderboardsClient.RequestLocalPlayerScoreSucceededEvent += RequestLocalPlayerScoreSucceeded;
	}

	private void UnsubscribeFromLocalPlayerScoreRequestEvents()
	{
		AGSLeaderboardsClient.RequestLocalPlayerScoreFailedEvent -= RequestLocalPlayerScoreFailed;
		AGSLeaderboardsClient.RequestLocalPlayerScoreSucceededEvent -= RequestLocalPlayerScoreSucceeded;
	}

	private void RequestLeaderboardsFailed(string error)
	{
		requestLeaderboardsStatus = "Request Leaderboards failed with error:";
		requestLeaderboardsStatusMessage = error;
		UnsubscribeFromLeaderboardRequestEvents();
	}

	private void RequestLeaderboardsSucceeded(List<AGSLeaderboard> leaderboards)
	{
		requestLeaderboardsStatus = "Available Leaderboards";
		leaderboardList = leaderboards;
		leaderboardsReady = true;
		UnsubscribeFromLeaderboardRequestEvents();
	}

	private void SubmitScoreFailed(string leaderboardId, string error)
	{
		if (!leaderboardsSubmissionStatus.ContainsKey(leaderboardId))
		{
			leaderboardsSubmissionStatus.Add(leaderboardId, null);
		}
		if (!leaderboardsSubmissionStatusMessage.ContainsKey(leaderboardId))
		{
			leaderboardsSubmissionStatusMessage.Add(leaderboardId, null);
		}
		leaderboardsSubmissionStatus[leaderboardId] = string.Format("Leaderboard \"{0}\" failed with error:", leaderboardId);
		leaderboardsSubmissionStatusMessage[leaderboardId] = error;
		UnsubscribeFromScoreSubmissionEvents();
	}

	private void SubmitScoreSucceeded(string leaderboardId)
	{
		if (!leaderboardsSubmissionStatus.ContainsKey(leaderboardId))
		{
			leaderboardsSubmissionStatus.Add(leaderboardId, null);
		}
		leaderboardsSubmissionStatus[leaderboardId] = string.Format("Score uploaded to \"{0}\" successfully.", leaderboardId);
		UnsubscribeFromScoreSubmissionEvents();
	}

	private void RequestLocalPlayerScoreFailed(string leaderboardId, string error)
	{
		if (!leaderboardsLocalScoreStatus.ContainsKey(leaderboardId))
		{
			leaderboardsLocalScoreStatus.Add(leaderboardId, null);
		}
		if (!leaderboardsLocalScoreStatusMessage.ContainsKey(leaderboardId))
		{
			leaderboardsLocalScoreStatusMessage.Add(leaderboardId, null);
		}
		leaderboardsLocalScoreStatus[leaderboardId] = string.Format("\"{0}\" score request failed with error:", leaderboardId);
		leaderboardsLocalScoreStatusMessage[leaderboardId] = error;
		UnsubscribeFromLocalPlayerScoreRequestEvents();
	}

	private void RequestLocalPlayerScoreSucceeded(string leaderboardId, int rank, long score)
	{
		if (!leaderboardsLocalScoreStatus.ContainsKey(leaderboardId))
		{
			leaderboardsLocalScoreStatus.Add(leaderboardId, null);
		}
		leaderboardsLocalScoreStatus[leaderboardId] = string.Format("Rank {0} with score of {1,5:N1}", rank, score);
		UnsubscribeFromLocalPlayerScoreRequestEvents();
	}
}
