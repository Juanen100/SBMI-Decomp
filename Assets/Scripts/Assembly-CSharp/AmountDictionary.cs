using System.Collections.Generic;
using UnityEngine;

public static class AmountDictionary
{
	public static Dictionary<string, object> ToJSONDict(Dictionary<int, int> srcDict)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (KeyValuePair<int, int> item in srcDict)
		{
			dictionary[item.Key.ToString()] = item.Value;
		}
		return dictionary;
	}

	public static Dictionary<int, int> FromJSONDict(Dictionary<string, object> srcDict)
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		foreach (string key2 in srcDict.Keys)
		{
			int key = int.Parse(key2);
			int value = TFUtils.LoadInt(srcDict, key2);
			dictionary[key] = value;
		}
		return dictionary;
	}

	public static Dictionary<string, object> ToJSONDict(Dictionary<int, Vector2> srcDict)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (KeyValuePair<int, Vector2> item in srcDict)
		{
			dictionary[item.Key.ToString()] = item.Value;
		}
		return dictionary;
	}

	public static Dictionary<int, Vector2> FromJSONDictVector2(Dictionary<string, object> srcDict)
	{
		Dictionary<int, Vector2> dictionary = new Dictionary<int, Vector2>();
		foreach (string key2 in srcDict.Keys)
		{
			int key = int.Parse(key2);
			Vector2 v;
			TFUtils.LoadVector2(out v, (Dictionary<string, object>)srcDict[key2]);
			dictionary[key] = v;
		}
		return dictionary;
	}
}
