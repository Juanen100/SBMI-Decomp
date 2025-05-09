using System.Collections.Generic;
using UnityEngine;

public class AGSSyncableNumber : AGSSyncableNumberElement
{
	public AGSSyncableNumber(AmazonJavaWrapper javaObject)
		: base(javaObject)
	{
	}

	public AGSSyncableNumber(AndroidJavaObject javaObject)
		: base(javaObject)
	{
	}

	public void Set(long val)
	{
		javaObject.Call("set", val);
	}

	public void Set(double val)
	{
		javaObject.Call("set", val);
	}

	public void Set(int val)
	{
		javaObject.Call("set", val);
	}

	public void Set(string val)
	{
		javaObject.Call("set", val);
	}

	public void Set(long val, Dictionary<string, string> metadata)
	{
		javaObject.Call("set", val, DictionaryToAndroidHashMap(metadata));
	}

	public void Set(double val, Dictionary<string, string> metadata)
	{
		javaObject.Call("set", val, DictionaryToAndroidHashMap(metadata));
	}

	public void Set(int val, Dictionary<string, string> metadata)
	{
		javaObject.Call("set", val, DictionaryToAndroidHashMap(metadata));
	}

	public void Set(string val, Dictionary<string, string> metadata)
	{
		javaObject.Call("set", val, DictionaryToAndroidHashMap(metadata));
	}

	public bool IsSet()
	{
		return javaObject.Call<bool>("isSet", new object[0]);
	}
}
