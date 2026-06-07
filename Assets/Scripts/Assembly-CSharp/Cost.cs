using System.Collections.Generic;

public class Cost
{
	public delegate Cost CostAtTime(ulong time);

	private Dictionary<int, int> resourceAmounts;

	public Dictionary<int, int> ResourceAmounts
	{
		get
		{
			return null;
		}
	}

	public Cost()
	{
	}

	public Cost(Dictionary<int, int> resourceAmounts)
	{
	}

	public Cost(Cost other)
	{
	}

	public static Cost FromDict(Dictionary<string, object> dict)
	{
		return null;
	}

	public static Cost FromObject(object o)
	{
		return null;
	}

	public int GetOnlyCostKey()
	{
		return 0;
	}

	public Dictionary<string, object> ToDict()
	{
		return null;
	}

	public static Cost operator +(Cost c1, Cost c2)
	{
		return null;
	}

	public static Cost operator -(Cost c1, Cost c2)
	{
		return null;
	}

	public static Dictionary<string, int> DisplayDictionary(Dictionary<int, int> costDict, ResourceManager resMgr)
	{
		return null;
	}

	public static Dictionary<string, int> GetResourcesStillRequired(Dictionary<int, int> costDict, ResourceManager resourceManager)
	{
		return null;
	}

	public static Cost GetResourcesToPurchase(Dictionary<int, int> costDict, ResourceManager resourceManager)
	{
		return null;
	}

	public void Prorate(float percentLeft)
	{
	}

	public void Prorate(ulong endTime, ulong totalTime)
	{
	}

	public static Cost Prorate(Cost full, float percentLeft)
	{
		return null;
	}

	public static Cost Prorate(Cost full, ulong endTime, ulong totalTime)
	{
		return null;
	}

	public static Cost Prorate(Cost full, ulong startTime, ulong endTime, ulong currentTime)
	{
		return null;
	}
}
