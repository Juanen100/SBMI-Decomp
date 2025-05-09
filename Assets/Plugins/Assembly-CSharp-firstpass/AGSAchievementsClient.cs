using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGSAchievementsClient : MonoBehaviour
{
	private static AmazonJavaWrapper JavaObject;

	private static readonly string PROXY_CLASS_NAME;

	public static event Action<string, string> UpdateAchievementFailedEvent;

	public static event Action<string> UpdateAchievementSucceededEvent;

	public static event Action<List<AGSAchievement>> RequestAchievementsSucceededEvent;

	public static event Action<string> RequestAchievementsFailedEvent;

	static AGSAchievementsClient()
	{
		PROXY_CLASS_NAME = "com.amazon.ags.api.unity.AchievementsClientProxyImpl";
		JavaObject = new AmazonJavaWrapper();
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(PROXY_CLASS_NAME))
		{
			if (androidJavaClass.GetRawClass() == IntPtr.Zero)
			{
				AGSClient.LogGameCircleWarning(string.Format("No java class {0} present, can't use AGSAchievementsClient", PROXY_CLASS_NAME));
			}
			else
			{
				JavaObject.setAndroidJavaObject(androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]));
			}
		}
	}

	public static void UpdateAchievementProgress(string achievementId, float progress)
	{
		JavaObject.Call("updateAchievementProgress", achievementId, progress);
	}

	public static void RequestAchievements()
	{
		JavaObject.Call("requestAchievements");
	}

	public static void ShowAchievementsOverlay()
	{
		JavaObject.Call("showAchievementsOverlay");
	}

	public static void RequestAchievementsSucceeded(string json)
	{
		if (AGSAchievementsClient.RequestAchievementsSucceededEvent == null)
		{
			return;
		}
		List<AGSAchievement> list = new List<AGSAchievement>();
		ArrayList arrayList = json.arrayListFromJson();
		foreach (Hashtable item in arrayList)
		{
			list.Add(AGSAchievement.fromHashtable(item));
		}
		AGSAchievementsClient.RequestAchievementsSucceededEvent(list);
	}

	public static void UpdateAchievementFailed(string json)
	{
		if (AGSAchievementsClient.UpdateAchievementFailedEvent != null)
		{
			Hashtable ht = json.hashtableFromJson();
			string stringFromHashtable = GetStringFromHashtable(ht, "achievementId");
			string stringFromHashtable2 = GetStringFromHashtable(ht, "error");
			AGSAchievementsClient.UpdateAchievementFailedEvent(stringFromHashtable, stringFromHashtable2);
		}
	}

	public static void UpdateAchievementSucceeded(string json)
	{
		if (AGSAchievementsClient.UpdateAchievementSucceededEvent != null)
		{
			Hashtable ht = json.hashtableFromJson();
			string stringFromHashtable = GetStringFromHashtable(ht, "achievementId");
			AGSAchievementsClient.UpdateAchievementSucceededEvent(stringFromHashtable);
		}
	}

	public static void RequestAchievementsFailed(string json)
	{
		if (AGSAchievementsClient.RequestAchievementsFailedEvent != null)
		{
			Hashtable ht = json.hashtableFromJson();
			string stringFromHashtable = GetStringFromHashtable(ht, "error");
			AGSAchievementsClient.RequestAchievementsFailedEvent(stringFromHashtable);
		}
	}

	private static string GetStringFromHashtable(Hashtable ht, string key)
	{
		if (ht == null)
		{
			return null;
		}
		if (key == null)
		{
			return null;
		}
		string result = null;
		if (ht.Contains(key))
		{
			result = ht[key].ToString();
		}
		return result;
	}
}
