using System;
using UnityEngine;

public class AGSWhispersyncClient : MonoBehaviour
{
	private static AmazonJavaWrapper javaObject;

	private static readonly string PROXY_CLASS_NAME;

	public static event Action OnNewCloudDataEvent;

	static AGSWhispersyncClient()
	{
		PROXY_CLASS_NAME = "com.amazon.ags.api.unity.WhispersyncClientProxyImpl";
		javaObject = new AmazonJavaWrapper();
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(PROXY_CLASS_NAME))
		{
			if (androidJavaClass.GetRawClass() == IntPtr.Zero)
			{
				AGSClient.LogGameCircleWarning("No java class " + PROXY_CLASS_NAME + " present, can't use AGSWhispersyncClient");
			}
			else
			{
				javaObject.setAndroidJavaObject(androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]));
			}
		}
	}

	public static AGSGameDataMap GetGameData()
	{
		AndroidJavaObject androidJavaObject = javaObject.Call<AndroidJavaObject>("getGameData", new object[0]);
		if (androidJavaObject != null)
		{
			return new AGSGameDataMap(new AmazonJavaWrapper(androidJavaObject));
		}
		return null;
	}

	public static void Synchronize()
	{
		javaObject.Call("synchronize");
	}

	public static void Flush()
	{
		javaObject.Call("flush");
	}

	public static void OnNewCloudData()
	{
		if (AGSWhispersyncClient.OnNewCloudDataEvent != null)
		{
			AGSWhispersyncClient.OnNewCloudDataEvent();
		}
	}
}
