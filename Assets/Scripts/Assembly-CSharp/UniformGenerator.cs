using System.Collections.Generic;
using UnityEngine;

public class UniformGenerator : ResultGenerator
{
	private List<string> options;

	public UniformGenerator(List<object> list)
	{
		options = new List<string>();
		foreach (object item in list)
		{
			options.Add(item.ToString());
		}
	}

	public string GetResult()
	{
		if (options.Count == 0)
		{
			return null;
		}
		return options[Random.Range(0, options.Count - 1)];
	}

	public string GetExpectedValue()
	{
		if (options.Count == 0)
		{
			return null;
		}
		float num = 0f;
		foreach (string option in options)
		{
			num += (float)int.Parse(option);
		}
		return string.Empty + num / (float)options.Count;
	}

	public string GetLowestValue()
	{
		if (options.Count == 0)
		{
			return "0";
		}
		float num = -1f;
		float num2 = 0f;
		foreach (string option in options)
		{
			num2 = int.Parse(option);
			if (num == -1f || num2 < num)
			{
				num = num2;
			}
		}
		if (num < 0f)
		{
			num = 0f;
		}
		return string.Empty + num;
	}
}
