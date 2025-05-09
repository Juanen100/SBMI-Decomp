using System;
using System.Collections.Generic;
using Prime31;
using UnityEngine;

public class GPGManager : AbstractManager
{
	public static event Action<string> authenticationSucceededEvent;

	public static event Action<string> authenticationFailedEvent;

	public static event Action userSignedOutEvent;

	public static event Action<string> reloadDataForKeyFailedEvent;

	public static event Action<string> reloadDataForKeySucceededEvent;

	public static event Action licenseCheckFailedEvent;

	public static event Action<string> profileImageLoadedAtPathEvent;

	public static event Action<string> loadCloudDataForKeyFailedEvent;

	public static event Action<int, string> loadCloudDataForKeySucceededEvent;

	public static event Action<string> updateCloudDataForKeyFailedEvent;

	public static event Action<int, string> updateCloudDataForKeySucceededEvent;

	public static event Action<string> clearCloudDataForKeyFailedEvent;

	public static event Action<string> clearCloudDataForKeySucceededEvent;

	public static event Action<string> deleteCloudDataForKeyFailedEvent;

	public static event Action<string> deleteCloudDataForKeySucceededEvent;

	public static event Action<string, string> unlockAchievementFailedEvent;

	public static event Action<string, bool> unlockAchievementSucceededEvent;

	public static event Action<string, string> incrementAchievementFailedEvent;

	public static event Action<string, bool> incrementAchievementSucceededEvent;

	public static event Action<string, string> revealAchievementFailedEvent;

	public static event Action<string> revealAchievementSucceededEvent;

	public static event Action<string, string> submitScoreFailedEvent;

	public static event Action<string, Dictionary<string, object>> submitScoreSucceededEvent;

	public static event Action<string, string> loadScoresFailedEvent;

	public static event Action<List<GPGScore>> loadScoresSucceededEvent;

	static GPGManager()
	{
		AbstractManager.initialize(typeof(GPGManager));
	}

	private void fireEventWithIdentifierAndError(Action<string, string> theEvent, string json)
	{
		if (theEvent != null)
		{
			Dictionary<string, object> dictionary = json.dictionaryFromJson();
			if (dictionary != null && dictionary.ContainsKey("identifier") && dictionary.ContainsKey("error"))
			{
				theEvent(dictionary["identifier"].ToString(), dictionary["error"].ToString());
			}
			else
			{
				Debug.LogError("json could not be deserialized to an identifier and an error: " + json);
			}
		}
	}

	private void fireEventWithIdentifierAndBool(Action<string, bool> theEvent, string param)
	{
		if (theEvent != null)
		{
			string[] array = param.Split(',');
			if (array.Length == 2)
			{
				theEvent(array[0], array[1] == "1");
			}
			else
			{
				Debug.LogError("param could not be deserialized to an identifier and an error: " + param);
			}
		}
	}

	public void userSignedOut(string empty)
	{
		GPGManager.userSignedOutEvent.fire();
	}

	public void reloadDataForKeyFailed(string error)
	{
		GPGManager.reloadDataForKeyFailedEvent.fire(error);
	}

	public void reloadDataForKeySucceeded(string param)
	{
		GPGManager.reloadDataForKeySucceededEvent.fire(param);
	}

	public void licenseCheckFailed(string param)
	{
		GPGManager.licenseCheckFailedEvent.fire();
	}

	public void profileImageLoadedAtPath(string path)
	{
		GPGManager.profileImageLoadedAtPathEvent.fire(path);
	}

	public void loadCloudDataForKeyFailed(string error)
	{
		GPGManager.loadCloudDataForKeyFailedEvent.fire(error);
	}

	public void loadCloudDataForKeySucceeded(string json)
	{
		Dictionary<string, object> dictionary = json.dictionaryFromJson();
		GPGManager.loadCloudDataForKeySucceededEvent.fire(int.Parse(dictionary["key"].ToString()), dictionary["data"].ToString());
	}

	public void updateCloudDataForKeyFailed(string error)
	{
		GPGManager.updateCloudDataForKeyFailedEvent.fire(error);
	}

	public void updateCloudDataForKeySucceeded(string json)
	{
		Dictionary<string, object> dictionary = json.dictionaryFromJson();
		GPGManager.updateCloudDataForKeySucceededEvent.fire(int.Parse(dictionary["key"].ToString()), dictionary["data"].ToString());
	}

	public void clearCloudDataForKeyFailed(string error)
	{
		GPGManager.clearCloudDataForKeyFailedEvent.fire(error);
	}

	public void clearCloudDataForKeySucceeded(string param)
	{
		GPGManager.clearCloudDataForKeySucceededEvent.fire(param);
	}

	public void deleteCloudDataForKeyFailed(string error)
	{
		GPGManager.deleteCloudDataForKeyFailedEvent.fire(error);
	}

	public void deleteCloudDataForKeySucceeded(string param)
	{
		GPGManager.deleteCloudDataForKeySucceededEvent.fire(param);
	}

	public void unlockAchievementFailed(string json)
	{
		fireEventWithIdentifierAndError(GPGManager.unlockAchievementFailedEvent, json);
	}

	public void unlockAchievementSucceeded(string param)
	{
		fireEventWithIdentifierAndBool(GPGManager.unlockAchievementSucceededEvent, param);
	}

	public void incrementAchievementFailed(string json)
	{
		fireEventWithIdentifierAndError(GPGManager.incrementAchievementFailedEvent, json);
	}

	public void incrementAchievementSucceeded(string param)
	{
		string[] array = param.Split(',');
		if (array.Length == 2)
		{
			GPGManager.incrementAchievementSucceededEvent.fire(array[0], array[1] == "1");
		}
	}

	public void revealAchievementFailed(string json)
	{
		fireEventWithIdentifierAndError(GPGManager.revealAchievementFailedEvent, json);
	}

	public void revealAchievementSucceeded(string achievementId)
	{
		GPGManager.revealAchievementSucceededEvent.fire(achievementId);
	}

	public void submitScoreFailed(string json)
	{
		fireEventWithIdentifierAndError(GPGManager.submitScoreFailedEvent, json);
	}

	public void submitScoreSucceeded(string json)
	{
		if (GPGManager.submitScoreSucceededEvent != null)
		{
			Dictionary<string, object> dictionary = json.dictionaryFromJson();
			string arg = "Unknown";
			if (dictionary.ContainsKey("leaderboardId"))
			{
				arg = dictionary["leaderboardId"].ToString();
			}
			GPGManager.submitScoreSucceededEvent(arg, dictionary);
		}
	}

	public void loadScoresFailed(string json)
	{
		fireEventWithIdentifierAndError(GPGManager.loadScoresFailedEvent, json);
	}

	public void loadScoresSucceeded(string json)
	{
		GPGManager.loadScoresSucceededEvent.fire(DTOBase.listFromJson<GPGScore>(json));
	}

	public void authenticationSucceeded(string param)
	{
		GPGManager.authenticationSucceededEvent.fire(param);
	}

	public void authenticationFailed(string error)
	{
		GPGManager.authenticationFailedEvent.fire(error);
	}
}
