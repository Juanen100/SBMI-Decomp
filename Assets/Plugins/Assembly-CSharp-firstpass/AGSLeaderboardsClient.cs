using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGSLeaderboardsClient : MonoBehaviour
{
	private static AmazonJavaWrapper JavaObject;

	private static readonly string PROXY_CLASS_NAME;

	public static event Action<string, string> SubmitScoreFailedEvent;

	public static event Action<string> SubmitScoreSucceededEvent;

	public static event Action<string> RequestLeaderboardsFailedEvent;

	public static event Action<List<AGSLeaderboard>> RequestLeaderboardsSucceededEvent;

	public static event Action<string, string> RequestLocalPlayerScoreFailedEvent;

	public static event Action<string, int, long> RequestLocalPlayerScoreSucceededEvent;

	static AGSLeaderboardsClient()
	{
		PROXY_CLASS_NAME = "com.amazon.ags.api.unity.LeaderboardsClientProxyImpl";
		JavaObject = new AmazonJavaWrapper();
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(PROXY_CLASS_NAME))
		{
			if (androidJavaClass.GetRawClass() == IntPtr.Zero)
			{
				AGSClient.LogGameCircleWarning("No java class " + PROXY_CLASS_NAME + " present, can't use AGSLeaderboardsClient");
			}
			else
			{
				JavaObject.setAndroidJavaObject(androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]));
			}
		}
	}

	public static void SubmitScore(string leaderboardId, long score)
	{
		JavaObject.Call("submitScore", leaderboardId, score);
	}

	public static void ShowLeaderboardsOverlay()
	{
		JavaObject.Call("showLeaderboardsOverlay");
	}

	public static void RequestLeaderboards()
	{
		JavaObject.Call("requestLeaderboards");
	}

	public static void RequestLocalPlayerScore(string leaderboardId, LeaderboardScope scope)
	{
		JavaObject.Call("requestLocalPlayerScore", leaderboardId, (int)scope);
	}

	public static void SubmitScoreFailed(string json)
	{
		if (AGSLeaderboardsClient.SubmitScoreFailedEvent != null)
		{
			Hashtable ht = json.hashtableFromJson();
			string stringFromHashtable = GetStringFromHashtable(ht, "leaderboardId");
			string stringFromHashtable2 = GetStringFromHashtable(ht, "error");
			AGSLeaderboardsClient.SubmitScoreFailedEvent(stringFromHashtable, stringFromHashtable2);
		}
	}

	public static void SubmitScoreSucceeded(string json)
	{
		if (AGSLeaderboardsClient.SubmitScoreSucceededEvent != null)
		{
			Hashtable ht = json.hashtableFromJson();
			string stringFromHashtable = GetStringFromHashtable(ht, "leaderboardId");
			AGSLeaderboardsClient.SubmitScoreSucceededEvent(stringFromHashtable);
		}
	}

	public static void RequestLeaderboardsFailed(string json)
	{
		if (AGSLeaderboardsClient.RequestLeaderboardsFailedEvent != null)
		{
			Hashtable ht = json.hashtableFromJson();
			string stringFromHashtable = GetStringFromHashtable(ht, "error");
			AGSLeaderboardsClient.RequestLeaderboardsFailedEvent(stringFromHashtable);
		}
	}

	public static void RequestLeaderboardsSucceeded(string json)
	{
		if (AGSLeaderboardsClient.RequestLeaderboardsSucceededEvent == null)
		{
			return;
		}
		List<AGSLeaderboard> list = new List<AGSLeaderboard>();
		ArrayList arrayList = json.arrayListFromJson();
		foreach (Hashtable item in arrayList)
		{
			list.Add(AGSLeaderboard.fromHashtable(item));
		}
		AGSLeaderboardsClient.RequestLeaderboardsSucceededEvent(list);
	}

	public static void RequestLocalPlayerScoreFailed(string json)
	{
		if (AGSLeaderboardsClient.RequestLocalPlayerScoreFailedEvent != null)
		{
			Hashtable ht = json.hashtableFromJson();
			string stringFromHashtable = GetStringFromHashtable(ht, "leaderboardId");
			string stringFromHashtable2 = GetStringFromHashtable(ht, "error");
			AGSLeaderboardsClient.RequestLocalPlayerScoreFailedEvent(stringFromHashtable, stringFromHashtable2);
		}
	}

	public static void RequestLocalPlayerScoreSucceeded(string json)
	{
		if (AGSLeaderboardsClient.RequestLocalPlayerScoreSucceededEvent == null)
		{
			return;
		}
		Hashtable hashtable = json.hashtableFromJson();
		int arg = 0;
		long arg2 = 0L;
		string arg3 = null;
		try
		{
			if (hashtable.Contains("leaderboardId"))
			{
				arg3 = hashtable["leaderboardId"].ToString();
			}
			if (hashtable.Contains("rank"))
			{
				arg = int.Parse(hashtable["rank"].ToString());
			}
			if (hashtable.Contains("score"))
			{
				arg2 = long.Parse(hashtable["score"].ToString());
			}
		}
		catch (FormatException ex)
		{
			AGSClient.Log("unable to parse score " + ex.Message);
		}
		AGSLeaderboardsClient.RequestLocalPlayerScoreSucceededEvent(arg3, arg, arg2);
	}

	private static string GetStringFromHashtable(Hashtable ht, string key)
	{
		string result = null;
		if (ht.Contains(key))
		{
			result = ht[key].ToString();
		}
		return result;
	}
}
