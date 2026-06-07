using System;
using System.Collections.Generic;
using Prime31;

public class GPGManager : AbstractManager
{
	public static event Action<string> authenticationSucceededEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string> authenticationFailedEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action userSignedOutEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string> reloadDataForKeyFailedEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string> reloadDataForKeySucceededEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action licenseCheckFailedEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string> profileImageLoadedAtPathEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string> loadCloudDataForKeyFailedEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<int, string> loadCloudDataForKeySucceededEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string> updateCloudDataForKeyFailedEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<int, string> updateCloudDataForKeySucceededEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string> clearCloudDataForKeyFailedEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string> clearCloudDataForKeySucceededEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string> deleteCloudDataForKeyFailedEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string> deleteCloudDataForKeySucceededEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string, string> unlockAchievementFailedEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string, bool> unlockAchievementSucceededEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string, string> incrementAchievementFailedEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string, bool> incrementAchievementSucceededEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string, string> revealAchievementFailedEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string> revealAchievementSucceededEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string, string> submitScoreFailedEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string, Dictionary<string, object>> submitScoreSucceededEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string, string> loadScoresFailedEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<List<GPGScore>> loadScoresSucceededEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	static GPGManager()
	{
	}

	private void fireEventWithIdentifierAndError(Action<string, string> theEvent, string json)
	{
	}

	private void fireEventWithIdentifierAndBool(Action<string, bool> theEvent, string param)
	{
	}

	public void userSignedOut(string empty)
	{
	}

	public void reloadDataForKeyFailed(string error)
	{
	}

	public void reloadDataForKeySucceeded(string param)
	{
	}

	public void licenseCheckFailed(string param)
	{
	}

	public void profileImageLoadedAtPath(string path)
	{
	}

	public void loadCloudDataForKeyFailed(string error)
	{
	}

	public void loadCloudDataForKeySucceeded(string json)
	{
	}

	public void updateCloudDataForKeyFailed(string error)
	{
	}

	public void updateCloudDataForKeySucceeded(string json)
	{
	}

	public void clearCloudDataForKeyFailed(string error)
	{
	}

	public void clearCloudDataForKeySucceeded(string param)
	{
	}

	public void deleteCloudDataForKeyFailed(string error)
	{
	}

	public void deleteCloudDataForKeySucceeded(string param)
	{
	}

	public void unlockAchievementFailed(string json)
	{
	}

	public void unlockAchievementSucceeded(string param)
	{
	}

	public void incrementAchievementFailed(string json)
	{
	}

	public void incrementAchievementSucceeded(string param)
	{
	}

	public void revealAchievementFailed(string json)
	{
	}

	public void revealAchievementSucceeded(string achievementId)
	{
	}

	public void submitScoreFailed(string json)
	{
	}

	public void submitScoreSucceeded(string json)
	{
	}

	public void loadScoresFailed(string json)
	{
	}

	public void loadScoresSucceeded(string json)
	{
	}

	public void authenticationSucceeded(string param)
	{
	}

	public void authenticationFailed(string error)
	{
	}
}
