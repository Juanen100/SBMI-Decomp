using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGSLeaderboardsClient : MonoBehaviour
{
	private static AmazonJavaWrapper JavaObject;

	private static readonly string PROXY_CLASS_NAME;

	public static event Action<string, string> SubmitScoreFailedEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string> SubmitScoreSucceededEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string> RequestLeaderboardsFailedEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<List<AGSLeaderboard>> RequestLeaderboardsSucceededEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string, string> RequestLocalPlayerScoreFailedEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string, int, long> RequestLocalPlayerScoreSucceededEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	static AGSLeaderboardsClient()
	{
	}

	public static void SubmitScore(string leaderboardId, long score)
	{
	}

	public static void ShowLeaderboardsOverlay()
	{
	}

	public static void RequestLeaderboards()
	{
	}

	public static void RequestLocalPlayerScore(string leaderboardId, LeaderboardScope scope)
	{
	}

	public static void SubmitScoreFailed(string json)
	{
	}

	public static void SubmitScoreSucceeded(string json)
	{
	}

	public static void RequestLeaderboardsFailed(string json)
	{
	}

	public static void RequestLeaderboardsSucceeded(string json)
	{
	}

	public static void RequestLocalPlayerScoreFailed(string json)
	{
	}

	public static void RequestLocalPlayerScoreSucceeded(string json)
	{
	}

	private static string GetStringFromHashtable(Hashtable ht, string key)
	{
		return null;
	}
}
