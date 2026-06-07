using System;
using System.Collections.Generic;
using UnityEngine;

public class AGSSyncable : IDisposable
{
	public enum SyncableMethod
	{
		getHighestNumber = 0,
		getLowestNumber = 1,
		getLatestNumber = 2,
		getHighNumberList = 3,
		getLowNumberList = 4,
		getLatestNumberList = 5,
		getAccumulatingNumber = 6,
		getLatestString = 7,
		getLatestStringList = 8,
		getStringSet = 9,
		getMap = 10
	}

	public enum HashSetMethod
	{
		getHighestNumberKeys = 0,
		getLowestNumberKeys = 1,
		getLatestNumberKeys = 2,
		getHighNumberListKeys = 3,
		getLowNumberListKeys = 4,
		getLatestNumberListKeys = 5,
		getAccumulatingNumberKeys = 6,
		getLatestStringKeys = 7,
		getLatestStringListKeys = 8,
		getStringSetKeys = 9,
		getMapKeys = 10
	}

	protected AmazonJavaWrapper javaObject;

	public AGSSyncable(AmazonJavaWrapper jo)
	{
	}

	public AGSSyncable(AndroidJavaObject jo)
	{
	}

	public void Dispose()
	{
	}

	protected AmazonJavaWrapper DictionaryToAndroidHashMap(Dictionary<string, string> dictionary)
	{
		return null;
	}

	protected T GetAGSSyncable<T>(SyncableMethod method)
	{
		return default(T);
	}

	protected T GetAGSSyncable<T>(SyncableMethod method, string key)
	{
		return default(T);
	}

	protected HashSet<string> GetHashSet(HashSetMethod method)
	{
		return null;
	}
}
