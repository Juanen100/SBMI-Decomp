using System.Collections.Generic;
using UnityEngine;

public class AGSSyncableElement : AGSSyncable
{
	public AGSSyncableElement(AmazonJavaWrapper javaObject)
		: base((AmazonJavaWrapper)null)
	{
	}

	public AGSSyncableElement(AndroidJavaObject javaObject)
		: base((AmazonJavaWrapper)null)
	{
	}

	public long GetTimestamp()
	{
		return 0L;
	}

	public Dictionary<string, string> GetMetadata()
	{
		return null;
	}
}
