using System.Collections.Generic;
using UnityEngine;

public class AGSSyncableString : AGSSyncableStringElement
{
	public AGSSyncableString(AmazonJavaWrapper javaObject)
		: base((AmazonJavaWrapper)null)
	{
	}

	public AGSSyncableString(AndroidJavaObject javaObject)
		: base((AmazonJavaWrapper)null)
	{
	}

	public void Set(string val)
	{
	}

	public void Set(string val, Dictionary<string, string> metadata)
	{
	}

	public bool IsSet()
	{
		return false;
	}
}
