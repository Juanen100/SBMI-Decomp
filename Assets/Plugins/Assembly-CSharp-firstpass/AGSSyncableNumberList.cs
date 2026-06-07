using System.Collections.Generic;
using UnityEngine;

public class AGSSyncableNumberList : AGSSyncableList
{
	public AGSSyncableNumberList(AmazonJavaWrapper javaObject)
		: base((AmazonJavaWrapper)null)
	{
	}

	public AGSSyncableNumberList(AndroidJavaObject javaObject)
		: base((AmazonJavaWrapper)null)
	{
	}

	public void Add(long val)
	{
	}

	public void Add(double val)
	{
	}

	public void Add(int val)
	{
	}

	public void Add(long val, Dictionary<string, string> metadata)
	{
	}

	public void Add(double val, Dictionary<string, string> metadata)
	{
	}

	public void Add(int val, Dictionary<string, string> metadata)
	{
	}

	public AGSSyncableNumberElement[] GetValues()
	{
		return null;
	}
}
