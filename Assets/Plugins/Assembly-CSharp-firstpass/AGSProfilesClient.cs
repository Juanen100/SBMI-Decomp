using System;
using System.Collections;
using UnityEngine;

public class AGSProfilesClient : MonoBehaviour
{
	private static AmazonJavaWrapper JavaObject;

	private static readonly string PROXY_CLASS_NAME;

	public static event Action<AGSProfile> PlayerAliasReceivedEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string> PlayerAliasFailedEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	static AGSProfilesClient()
	{
	}

	public static void RequestLocalPlayerProfile()
	{
	}

	public static void PlayerAliasReceived(string json)
	{
	}

	public static void PlayerAliasFailed(string json)
	{
	}

	private static string GetStringFromHashtable(Hashtable ht, string key)
	{
		return null;
	}
}
