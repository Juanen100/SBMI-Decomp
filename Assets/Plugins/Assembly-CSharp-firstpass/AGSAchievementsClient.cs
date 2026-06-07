using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGSAchievementsClient : MonoBehaviour
{
	private static AmazonJavaWrapper JavaObject;

	private static readonly string PROXY_CLASS_NAME;

	public static event Action<string, string> UpdateAchievementFailedEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string> UpdateAchievementSucceededEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<List<AGSAchievement>> RequestAchievementsSucceededEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string> RequestAchievementsFailedEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	static AGSAchievementsClient()
	{
	}

	public static void UpdateAchievementProgress(string achievementId, float progress)
	{
	}

	public static void RequestAchievements()
	{
	}

	public static void ShowAchievementsOverlay()
	{
	}

	public static void RequestAchievementsSucceeded(string json)
	{
	}

	public static void UpdateAchievementFailed(string json)
	{
	}

	public static void UpdateAchievementSucceeded(string json)
	{
	}

	public static void RequestAchievementsFailed(string json)
	{
	}

	private static string GetStringFromHashtable(Hashtable ht, string key)
	{
		return null;
	}
}
