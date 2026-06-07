using UnityEngine;

public class GooglePlayDownloader
{
	private static AndroidJavaClass detectAndroidJNI;

	private static AndroidJavaClass Environment;

	private const string Environment_MEDIA_MOUNTED = "mounted";

	private static string obb_package;

	private static int obb_version;

	static GooglePlayDownloader()
	{
	}

	public static bool RunningOnAndroid()
	{
		return false;
	}

	public static string GetExpansionFilePath()
	{
		return null;
	}

	public static string GetMainOBBPath(string expansionFilePath)
	{
		return null;
	}

	public static string GetPatchOBBPath(string expansionFilePath)
	{
		return null;
	}

	public static void FetchOBB()
	{
	}

	private static void populateOBBData()
	{
	}
}
