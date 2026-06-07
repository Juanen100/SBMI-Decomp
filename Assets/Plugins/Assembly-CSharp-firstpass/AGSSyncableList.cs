using System.Collections.Generic;
using UnityEngine;

public class AGSSyncableList : AGSSyncable
{
	public AGSSyncableList(AmazonJavaWrapper javaObject)
		: base((AmazonJavaWrapper)null)
	{
	}

	public AGSSyncableList(AndroidJavaObject javaObject)
		: base((AmazonJavaWrapper)null)
	{
	}

	public void SetMaxSize(int size)
	{
	}

	public int GetMaxSize()
	{
		return 0;
	}

	public bool IsSet()
	{
		return false;
	}

	public void Add(string val, Dictionary<string, string> metadata)
	{
	}

	public void Add(string val)
	{
	}
}
