using System;
using System.Collections.Generic;
using UnityEngine;

public class AGSSyncableStringSet : AGSSyncable
{
	public AGSSyncableStringSet(AmazonJavaWrapper javaObject)
		: base(javaObject)
	{
	}

	public AGSSyncableStringSet(AndroidJavaObject javaObject)
		: base(javaObject)
	{
	}

	public void Add(string val)
	{
		javaObject.Call("add", val);
	}

	public void Add(string val, Dictionary<string, string> metadata)
	{
		javaObject.Call("add", val, DictionaryToAndroidHashMap(metadata));
	}

	public AGSSyncableStringElement Get(string val)
	{
		return GetAGSSyncable<AGSSyncableStringElement>(SyncableMethod.getStringSet, val);
	}

	public bool Contains(string val)
	{
		return javaObject.Call<bool>("contains", new object[1] { val });
	}

	public bool IsSet()
	{
		return javaObject.Call<bool>("isSet", new object[0]);
	}

	public HashSet<AGSSyncableStringElement> GetValues()
	{
		AndroidJNI.PushLocalFrame(10);
		HashSet<AGSSyncableStringElement> hashSet = new HashSet<AGSSyncableStringElement>();
		AndroidJavaObject androidJavaObject = javaObject.Call<AndroidJavaObject>("getValues", new object[0]);
		if (androidJavaObject == null)
		{
			return hashSet;
		}
		AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("iterator", new object[0]);
		if (androidJavaObject2 == null)
		{
			return hashSet;
		}
		while (androidJavaObject2.Call<bool>("hasNext", new object[0]))
		{
			AndroidJavaObject androidJavaObject3 = androidJavaObject2.Call<AndroidJavaObject>("next", new object[0]);
			if (androidJavaObject3 != null)
			{
				hashSet.Add(new AGSSyncableStringElement(androidJavaObject3));
			}
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
		return hashSet;
	}
}
