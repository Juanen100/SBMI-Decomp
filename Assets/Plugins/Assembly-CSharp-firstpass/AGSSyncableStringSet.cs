using System.Collections.Generic;
using UnityEngine;

public class AGSSyncableStringSet : AGSSyncable
{
	public AGSSyncableStringSet(AmazonJavaWrapper javaObject)
		: base((AmazonJavaWrapper)null)
	{
	}

	public AGSSyncableStringSet(AndroidJavaObject javaObject)
		: base((AmazonJavaWrapper)null)
	{
	}

	public void Add(string val)
	{
	}

	public void Add(string val, Dictionary<string, string> metadata)
	{
	}

	public AGSSyncableStringElement Get(string val)
	{
		return null;
	}

	public bool Contains(string val)
	{
		return false;
	}

	public bool IsSet()
	{
		return false;
	}

	public HashSet<AGSSyncableStringElement> GetValues()
	{
		return null;
	}
}
