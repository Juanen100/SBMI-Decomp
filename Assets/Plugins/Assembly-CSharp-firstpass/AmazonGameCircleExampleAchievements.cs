using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class AmazonGameCircleExampleAchievements : AmazonGameCircleExampleBase
{
	private const int betweenCommaRegexGroup = 2;

	private const string achievementsMenuTitle = "Achievements";

	private const string displayAchievementOverlayButtonLabel = "Achievements Overlay";

	private const string achievementProgressLabel = "Achievement \"{0}\"";

	private const string submitAchievementButtonLabel = "Submit Achievement Progress";

	private const string achievementFailedLabel = "Achievement \"{0}\" failed with error:";

	private const string achievementSucceededLabel = "Achievement \"{0}\" uploaded successfully.";

	private const string achievementPercent = "{0}%";

	private const string requestAchievementsButtonLabel = "Request Achievements";

	private const string requestingAchievementsLabel = "Requesting Achievements...";

	private const string requestAchievementsFailedLabel = "Request Achievements failed with error:";

	private const string requestAchievementsSucceededLabel = "Available Achievements";

	private const string noAchievementsAvailableLabel = "No Achievements Available";

	private const string achievementRequestTimeLabel = "{0,5:N1} seconds";

	private const string submittingInformationString = "Submitting Achievement...";

	private const string updateAchievementsReturnedMissingAchievementId = "AmazonGameCircleExampleAchievements received GameCircle plugin callback with invalid achievement ID.";

	private const string noErrorMessageReceived = "MISSING ERROR STRING";

	private const float achievementMinValue = -200f;

	private const float achievementMaxValue = 200f;

	private Dictionary<string, string> achievementsSubmissionStatus = new Dictionary<string, string>();

	private Dictionary<string, string> achievementsSubmissionStatusMessage = new Dictionary<string, string>();

	private Dictionary<string, bool> achievementsFoldout = new Dictionary<string, bool>();

	private string requestAchievementsStatus;

	private string requestAchievementsStatusMessage;

	private List<AGSAchievement> achievementList;

	private bool achievementsReady;

	private DateTime achievementsRequestTime;

	private AGSAchievement invalidAchievement;

	private readonly Regex addNewlineEveryThirdCommaRegex = new Regex("(,([^,]+,[^,]+),)");

	public AmazonGameCircleExampleAchievements()
	{
		invalidAchievement = new AGSAchievement();
		invalidAchievement.title = "Invalid Achievement Title";
		invalidAchievement.id = "Invalid Achievement ID";
		invalidAchievement.progress = 0f;
	}

	public override string MenuTitle()
	{
		return "Achievements";
	}

	public override void DrawMenu()
	{
		if (GUILayout.Button("Achievements Overlay"))
		{
			AGSAchievementsClient.ShowAchievementsOverlay();
		}
		if (string.IsNullOrEmpty(requestAchievementsStatus))
		{
			if (GUILayout.Button("Request Achievements"))
			{
				RequestAchievements();
			}
			return;
		}
		AmazonGameCircleExampleGUIHelpers.CenteredLabel(requestAchievementsStatus);
		if (!string.IsNullOrEmpty(requestAchievementsStatusMessage))
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel(requestAchievementsStatusMessage);
		}
		if (!achievementsReady)
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel(string.Format("{0,5:N1} seconds", (DateTime.Now - achievementsRequestTime).TotalSeconds));
			return;
		}
		if (achievementList != null && achievementList.Count > 0)
		{
			foreach (AGSAchievement achievement in achievementList)
			{
				DisplayAchievement(achievement);
			}
		}
		else
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel("No Achievements Available");
		}
		if (invalidAchievement != null)
		{
			DisplayAchievement(invalidAchievement);
		}
	}

	private void DisplayAchievement(AGSAchievement achievement)
	{
		GUILayout.BeginVertical(GUI.skin.box);
		if (!achievementsFoldout.ContainsKey(achievement.id))
		{
			achievementsFoldout.Add(achievement.id, false);
		}
		achievementsFoldout[achievement.id] = AmazonGameCircleExampleGUIHelpers.FoldoutWithLabel(achievementsFoldout[achievement.id], string.Format("Achievement \"{0}\"", achievement.id));
		if (achievementsFoldout[achievement.id])
		{
			AmazonGameCircleExampleGUIHelpers.AnchoredLabel(AddNewlineEveryThirdComma(achievement.ToString()), TextAnchor.UpperCenter);
			if (!achievementsSubmissionStatus.ContainsKey(achievement.id) || string.IsNullOrEmpty(achievementsSubmissionStatus[achievement.id]))
			{
				achievement.progress = AmazonGameCircleExampleGUIHelpers.DisplayCenteredSlider(achievement.progress, -200f, 200f, "{0}%");
				if (GUILayout.Button("Submit Achievement Progress"))
				{
					SubmitAchievement(achievement.id, achievement.progress);
				}
			}
			else
			{
				AmazonGameCircleExampleGUIHelpers.CenteredLabel(achievementsSubmissionStatus[achievement.id]);
				if (achievementsSubmissionStatusMessage.ContainsKey(achievement.id) && !string.IsNullOrEmpty(achievementsSubmissionStatusMessage[achievement.id]))
				{
					AmazonGameCircleExampleGUIHelpers.CenteredLabel(achievementsSubmissionStatusMessage[achievement.id]);
				}
			}
		}
		GUILayout.EndVertical();
	}

	private string AddNewlineEveryThirdComma(string stringToChange)
	{
		return addNewlineEveryThirdCommaRegex.Replace(stringToChange, (Match regexMatchEvaluator) => "," + regexMatchEvaluator.Groups[2].Value + ",\n");
	}

	private void RequestAchievements()
	{
		achievementsRequestTime = DateTime.Now;
		SubscribeToAchievementRequestEvents();
		AGSAchievementsClient.RequestAchievements();
		requestAchievementsStatus = "Requesting Achievements...";
	}

	private void SubmitAchievement(string achievementId, float progress)
	{
		SubscribeToSubmitAchievementEvents();
		AGSAchievementsClient.UpdateAchievementProgress(achievementId, progress);
		if (!achievementsSubmissionStatus.ContainsKey(achievementId))
		{
			achievementsSubmissionStatus.Add(achievementId, null);
		}
		achievementsSubmissionStatus[achievementId] = string.Format("Submitting Achievement...");
	}

	private void SubscribeToAchievementRequestEvents()
	{
		AGSAchievementsClient.RequestAchievementsFailedEvent += RequestAchievementsFailed;
		AGSAchievementsClient.RequestAchievementsSucceededEvent += RequestAchievementsSucceeded;
	}

	private void UnsubscribeFromAchievementRequestEvents()
	{
		AGSAchievementsClient.RequestAchievementsFailedEvent -= RequestAchievementsFailed;
		AGSAchievementsClient.RequestAchievementsSucceededEvent -= RequestAchievementsSucceeded;
	}

	private void SubscribeToSubmitAchievementEvents()
	{
		AGSAchievementsClient.UpdateAchievementFailedEvent += UpdateAchievementsFailed;
		AGSAchievementsClient.UpdateAchievementSucceededEvent += UpdateAchievementsSucceeded;
	}

	private void UnsubscribeFromSubmitAchievementEvents()
	{
		AGSAchievementsClient.UpdateAchievementFailedEvent -= UpdateAchievementsFailed;
		AGSAchievementsClient.UpdateAchievementSucceededEvent -= UpdateAchievementsSucceeded;
	}

	private void RequestAchievementsFailed(string error)
	{
		requestAchievementsStatus = "Request Achievements failed with error:";
		requestAchievementsStatusMessage = error;
		UnsubscribeFromAchievementRequestEvents();
	}

	private void RequestAchievementsSucceeded(List<AGSAchievement> achievements)
	{
		requestAchievementsStatus = "Available Achievements";
		achievementList = achievements;
		achievementsReady = true;
		UnsubscribeFromAchievementRequestEvents();
	}

	private void UpdateAchievementsFailed(string achievementId, string error)
	{
		if (string.IsNullOrEmpty(achievementId))
		{
			Debug.LogError("AmazonGameCircleExampleAchievements received GameCircle plugin callback with invalid achievement ID.");
			return;
		}
		if (string.IsNullOrEmpty(error))
		{
			error = "MISSING ERROR STRING";
		}
		if (!achievementsSubmissionStatus.ContainsKey(achievementId))
		{
			achievementsSubmissionStatus.Add(achievementId, null);
		}
		if (!achievementsSubmissionStatusMessage.ContainsKey(achievementId))
		{
			achievementsSubmissionStatusMessage.Add(achievementId, null);
		}
		achievementsSubmissionStatus[achievementId] = string.Format("Achievement \"{0}\" failed with error:", achievementId);
		achievementsSubmissionStatusMessage[achievementId] = error;
	}

	private void UpdateAchievementsSucceeded(string achievementId)
	{
		if (!achievementsSubmissionStatus.ContainsKey(achievementId))
		{
			achievementsSubmissionStatus.Add(achievementId, null);
		}
		achievementsSubmissionStatus[achievementId] = string.Format("Achievement \"{0}\" uploaded successfully.", achievementId);
	}
}
