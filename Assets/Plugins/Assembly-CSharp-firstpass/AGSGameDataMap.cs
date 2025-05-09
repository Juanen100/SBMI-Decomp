using System.Collections.Generic;
using UnityEngine;

public class AGSGameDataMap : AGSSyncable
{
	public AGSGameDataMap(AmazonJavaWrapper javaObject)
		: base(javaObject)
	{
	}

	public AGSGameDataMap(AndroidJavaObject javaObject)
		: base(javaObject)
	{
	}

	public AGSSyncableNumber GetHighestNumber(string name)
	{
		return GetAGSSyncable<AGSSyncableNumber>(SyncableMethod.getHighestNumber, name);
	}

	public HashSet<string> GetHighestNumberKeys()
	{
		return GetHashSet(HashSetMethod.getHighestNumberKeys);
	}

	public AGSSyncableNumber GetLowestNumber(string name)
	{
		return GetAGSSyncable<AGSSyncableNumber>(SyncableMethod.getLowestNumber, name);
	}

	public HashSet<string> GetLowestNumberKeys()
	{
		return GetHashSet(HashSetMethod.getLowestNumberKeys);
	}

	public AGSSyncableNumber GetLatestNumber(string name)
	{
		return GetAGSSyncable<AGSSyncableNumber>(SyncableMethod.getLatestNumber, name);
	}

	public HashSet<string> GetLatestNumberKeys()
	{
		return GetHashSet(HashSetMethod.getLatestNumberKeys);
	}

	public AGSSyncableNumberList GetHighNumberList(string name)
	{
		return GetAGSSyncable<AGSSyncableNumberList>(SyncableMethod.getHighNumberList, name);
	}

	public HashSet<string> GetHighNumberListKeys()
	{
		return GetHashSet(HashSetMethod.getHighNumberListKeys);
	}

	public AGSSyncableNumberList GetLowNumberList(string name)
	{
		return GetAGSSyncable<AGSSyncableNumberList>(SyncableMethod.getLowNumberList, name);
	}

	public HashSet<string> GetLowNumberListKeys()
	{
		return GetHashSet(HashSetMethod.getLowNumberListKeys);
	}

	public AGSSyncableNumberList GetLatestNumberList(string name)
	{
		return GetAGSSyncable<AGSSyncableNumberList>(SyncableMethod.getLatestNumberList, name);
	}

	public HashSet<string> GetLatestNumberListKeys()
	{
		return GetHashSet(HashSetMethod.getLatestNumberListKeys);
	}

	public AGSSyncableAccumulatingNumber GetAccumulatingNumber(string name)
	{
		return GetAGSSyncable<AGSSyncableAccumulatingNumber>(SyncableMethod.getAccumulatingNumber, name);
	}

	public HashSet<string> GetAccumulatingNumberKeys()
	{
		return GetHashSet(HashSetMethod.getAccumulatingNumberKeys);
	}

	public AGSSyncableString GetLatestString(string name)
	{
		return GetAGSSyncable<AGSSyncableString>(SyncableMethod.getLatestString, name);
	}

	public HashSet<string> GetLatestStringKeys()
	{
		return GetHashSet(HashSetMethod.getLatestStringKeys);
	}

	public AGSSyncableStringList GetLatestStringList(string name)
	{
		return GetAGSSyncable<AGSSyncableStringList>(SyncableMethod.getLatestStringList, name);
	}

	public HashSet<string> GetLatestStringListKeys()
	{
		return GetHashSet(HashSetMethod.getLatestStringListKeys);
	}

	public AGSSyncableStringSet GetStringSet(string name)
	{
		return GetAGSSyncable<AGSSyncableStringSet>(SyncableMethod.getStringSet, name);
	}

	public HashSet<string> GetStringSetKeys()
	{
		return GetHashSet(HashSetMethod.getStringSetKeys);
	}

	public AGSGameDataMap GetMap(string name)
	{
		return GetAGSSyncable<AGSGameDataMap>(SyncableMethod.getMap);
	}

	public HashSet<string> GetMapKeys()
	{
		return GetHashSet(HashSetMethod.getMapKeys);
	}
}
