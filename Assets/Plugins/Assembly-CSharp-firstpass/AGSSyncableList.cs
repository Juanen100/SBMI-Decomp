using System.Collections.Generic;
using UnityEngine;

public class AGSSyncableList : AGSSyncable
{
	public AGSSyncableList(AmazonJavaWrapper javaObject)
		: base(javaObject)
	{
	}

	public AGSSyncableList(AndroidJavaObject javaObject)
		: base(javaObject)
	{
	}

	public void SetMaxSize(int size)
	{
		javaObject.Call("setMaxSize", size);
	}

	public int GetMaxSize()
	{
		return javaObject.Call<int>("getMaxSize", new object[0]);
	}

	public bool IsSet()
	{
		return javaObject.Call<bool>("isSet", new object[0]);
	}

	public void Add(string val, Dictionary<string, string> metadata)
	{
		javaObject.Call("add", val, DictionaryToAndroidHashMap(metadata));
	}

	public void Add(string val)
	{
		javaObject.Call("add", val);
	}
}
