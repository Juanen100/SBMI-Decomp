using System;
using System.Collections.Generic;
using UnityEngine;

public class AGSSyncableElement : AGSSyncable
{
	public AGSSyncableElement(AmazonJavaWrapper javaObject)
		: base(javaObject)
	{
	}

	public AGSSyncableElement(AndroidJavaObject javaObject)
		: base(javaObject)
	{
	}

	public long GetTimestamp()
	{
		return javaObject.Call<long>("getTimestamp", new object[0]);
	}

	public Dictionary<string, string> GetMetadata()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		AndroidJNI.PushLocalFrame(10);
		AndroidJavaObject androidJavaObject = javaObject.Call<AndroidJavaObject>("getMetadata", new object[0]);
		if (androidJavaObject == null)
		{
			AGSClient.LogGameCircleError("Whispersync element was unable to retrieve metadata java map");
			return dictionary;
		}
		AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("keySet", new object[0]);
		if (androidJavaObject2 == null)
		{
			AGSClient.LogGameCircleError("Whispersync element was unable to retrieve java keyset");
			return dictionary;
		}
		AndroidJavaObject androidJavaObject3 = androidJavaObject2.Call<AndroidJavaObject>("iterator", new object[0]);
		if (androidJavaObject3 == null)
		{
			AGSClient.LogGameCircleError("Whispersync element was unable to retrieve java iterator");
			return dictionary;
		}
		while (androidJavaObject3.Call<bool>("hasNext", new object[0]))
		{
			string text = androidJavaObject3.Call<string>("next", new object[0]);
			if (text != null)
			{
				string text2 = androidJavaObject.Call<string>("get", new object[1] { text });
				if (text2 != null)
				{
					dictionary.Add(text, text2);
				}
			}
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
		return dictionary;
	}
}
