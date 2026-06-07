using System;
using System.Collections;

public class AGSAchievement
{
	public string title;

	public string id;

	public int pointValue;

	public bool isHidden;

	public bool isUnlocked;

	public float progress;

	public int position;

	public string decription;

	public DateTime dateUnlocked;

	public static AGSAchievement fromHashtable(Hashtable ht)
	{
		return null;
	}

	private static DateTime getTimefromEpochTime(double javaTimeStamp)
	{
		return default(DateTime);
	}

	private static string getStringValue(Hashtable ht, string key)
	{
		return null;
	}

	public override string ToString()
	{
		return null;
	}
}
