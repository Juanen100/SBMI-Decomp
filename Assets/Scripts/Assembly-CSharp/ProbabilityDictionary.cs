using System.Collections.Generic;

public static class ProbabilityDictionary
{
	public static Dictionary<int, ResultGenerator> FromJSONDict(Dictionary<string, object> srcDict)
	{
		Dictionary<int, ResultGenerator> dictionary = new Dictionary<int, ResultGenerator>();
		foreach (string key2 in srcDict.Keys)
		{
			int key = int.Parse(key2);
			ResultGenerator value = ((!(srcDict[key2] is Dictionary<string, object>)) ? ((!(srcDict[key2] is List<object>)) ? ((ResultGenerator)new ConstantGenerator(srcDict[key2].ToString())) : ((ResultGenerator)new UniformGenerator((List<object>)srcDict[key2]))) : new ProbabilityTable((Dictionary<string, object>)srcDict[key2]));
			dictionary[key] = value;
		}
		return dictionary;
	}

	public static Dictionary<int, int> GenerateAmounts(Dictionary<int, ResultGenerator> srcDict)
	{
		if (srcDict == null)
		{
			return null;
		}
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		foreach (int key in srcDict.Keys)
		{
			string result = srcDict[key].GetResult();
			if (result != null)
			{
				int value = int.Parse(result);
				dictionary[key] = value;
			}
		}
		return dictionary;
	}

	public static Dictionary<int, float> CalculateExpectedValues(Dictionary<int, ResultGenerator> srcDict)
	{
		if (srcDict == null)
		{
			return null;
		}
		Dictionary<int, float> dictionary = new Dictionary<int, float>();
		foreach (int key in srcDict.Keys)
		{
			string expectedValue = srcDict[key].GetExpectedValue();
			if (expectedValue != null)
			{
				float value = float.Parse(expectedValue);
				dictionary[key] = value;
			}
		}
		return dictionary;
	}
}
