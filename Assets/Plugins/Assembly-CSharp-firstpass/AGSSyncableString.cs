using System.Collections.Generic;
using UnityEngine;

public class AGSSyncableString : AGSSyncableStringElement
{
	public AGSSyncableString(AmazonJavaWrapper javaObject)
		: base(javaObject)
	{
	}

	public AGSSyncableString(AndroidJavaObject javaObject)
		: base(javaObject)
	{
	}

	public void Set(string val)
	{
		javaObject.Call("set", val);
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
