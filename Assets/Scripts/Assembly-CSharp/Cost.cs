#define ASSERTS_ON
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cost
{
	public delegate Cost CostAtTime(ulong time);

	private Dictionary<int, int> resourceAmounts;

	public Dictionary<int, int> ResourceAmounts
	{
		get
		{
			return resourceAmounts;
		}
	}

	public Cost()
	{
		resourceAmounts = new Dictionary<int, int>();
	}

	public Cost(Dictionary<int, int> resourceAmounts)
	{
		this.resourceAmounts = resourceAmounts;
	}

	public Cost(Cost other)
	{
		Dictionary<int, int> dictionary = TFUtils.CloneDictionary(other.ResourceAmounts);
		resourceAmounts = dictionary;
	}

	public static Cost FromDict(Dictionary<string, object> dict)
	{
		if (dict == null)
		{
			return null;
		}
		Dictionary<int, int> dictionary = AmountDictionary.FromJSONDict(dict);
		return new Cost(dictionary);
	}

	public static Cost FromObject(object o)
	{
		return (o != null) ? FromDict((Dictionary<string, object>)o) : null;
	}

	public int GetOnlyCostKey()
	{
		TFUtils.Assert(ResourceAmounts.Count == 1, "Cost expect to have only one entry, it has " + ResourceAmounts.Count);
		int result = -1;
		using (Dictionary<int, int>.KeyCollection.Enumerator enumerator = ResourceAmounts.Keys.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				int current = enumerator.Current;
				result = current;
			}
		}
		return result;
	}

	public Dictionary<string, object> ToDict()
	{
		return AmountDictionary.ToJSONDict(resourceAmounts);
	}

	public static Dictionary<string, int> DisplayDictionary(Dictionary<int, int> costDict, ResourceManager resMgr)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		foreach (KeyValuePair<int, int> item in costDict)
		{
			string resourceTexture = resMgr.Resources[item.Key].GetResourceTexture();
			dictionary[resourceTexture] = item.Value;
		}
		return dictionary;
	}

	public static Dictionary<string, int> GetResourcesStillRequired(Dictionary<int, int> costDict, ResourceManager resourceManager)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		foreach (KeyValuePair<int, int> item in costDict)
		{
			int num = resourceManager.Query(item.Key);
			int num2 = item.Value - num;
			if (num2 > 0)
			{
				string resourceTexture = resourceManager.Resources[item.Key].GetResourceTexture();
				dictionary[resourceTexture] = num2;
			}
		}
		return dictionary;
	}

	public static Cost GetResourcesToPurchase(Dictionary<int, int> costDict, ResourceManager resourceManager)
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		foreach (KeyValuePair<int, int> item in costDict)
		{
			int num = resourceManager.Query(item.Key);
			int num2 = item.Value - num;
			if (num2 > 0)
			{
				dictionary[item.Key] = num2;
			}
		}
		return new Cost(dictionary);
	}

	public void Prorate(float percentLeft)
	{
		int[] array = resourceAmounts.Keys.ToArray();
		foreach (int key in array)
		{
			resourceAmounts[key] = Resource.Prorate(resourceAmounts[key], percentLeft);
		}
	}

	public void Prorate(ulong endTime, ulong totalTime)
	{
		Prorate(Mathf.Max(0f, endTime - TFUtils.EpochTime()) / (float)totalTime);
	}

	public static Cost Prorate(Cost full, float percentLeft)
	{
		Cost cost = new Cost(full);
		cost.Prorate(percentLeft);
		return cost;
	}

	public static Cost Prorate(Cost full, ulong endTime, ulong totalTime)
	{
		Cost cost = new Cost(full);
		cost.Prorate(endTime, totalTime);
		return cost;
	}

	public static Cost Prorate(Cost full, ulong startTime, ulong endTime, ulong currentTime)
	{
		Cost cost = new Cost(full);
		float num = endTime - startTime;
		float num2 = endTime - currentTime;
		cost.Prorate(Mathf.Max(0f, num2 / num));
		return cost;
	}

	public static Cost operator +(Cost c1, Cost c2)
	{
		Cost cost = new Cost(c1);
		foreach (int key in c2.resourceAmounts.Keys)
		{
			int value;
			if (cost.resourceAmounts.TryGetValue(key, out value))
			{
				cost.resourceAmounts[key] = value + c2.resourceAmounts[key];
			}
			else
			{
				cost.resourceAmounts[key] = c2.resourceAmounts[key];
			}
		}
		return cost;
	}

	public static Cost operator -(Cost c1, Cost c2)
	{
		Cost cost = new Cost(c1);
		foreach (int key in c2.resourceAmounts.Keys)
		{
			int value;
			if (cost.resourceAmounts.TryGetValue(key, out value))
			{
				cost.resourceAmounts[key] = value - c2.resourceAmounts[key];
			}
			else
			{
				cost.resourceAmounts[key] = -c2.resourceAmounts[key];
			}
		}
		return cost;
	}
}
