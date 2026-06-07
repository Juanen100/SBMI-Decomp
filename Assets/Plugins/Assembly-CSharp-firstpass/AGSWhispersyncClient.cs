using System;
using UnityEngine;

public class AGSWhispersyncClient : MonoBehaviour
{
	private static AmazonJavaWrapper javaObject;

	private static readonly string PROXY_CLASS_NAME;

	public static event Action OnNewCloudDataEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	static AGSWhispersyncClient()
	{
	}

	public static AGSGameDataMap GetGameData()
	{
		return null;
	}

	public static void Synchronize()
	{
	}

	public static void Flush()
	{
	}

	public static void OnNewCloudData()
	{
	}
}
