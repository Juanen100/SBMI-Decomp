#define ASSERTS_ON
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbabilityTable : IEnumerable, IEnumerable<ProbabilityTable.Entry>, ResultGenerator
{
	public class Entry
	{
		public double rangeStart;

		public double rangeEnd;

		public string eventName;

		public double Range
		{
			get
			{
				return rangeEnd - rangeStart;
			}
		}

		public Entry(double rangeStart, double rangeEnd, string eventName)
		{
			this.rangeStart = rangeStart;
			this.rangeEnd = rangeEnd;
			this.eventName = eventName;
		}

		public override string ToString()
		{
			return "[Entry event(" + eventName + "), range:[" + rangeStart + "," + rangeEnd + ") ]";
		}
	}

	private Dictionary<string, Entry> entries;

	private double totalRange;

	public double TotalRange
	{
		get
		{
			return totalRange;
		}
	}

	public ProbabilityTable()
	{
		entries = new Dictionary<string, Entry>();
	}

	public ProbabilityTable(Dictionary<string, object> dict)
	{
		entries = new Dictionary<string, Entry>(dict.Count);
		foreach (KeyValuePair<string, object> item in dict)
		{
			string key = item.Key;
			double probability = Convert.ToDouble(item.Value);
			Add(key, probability);
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public void Add(string eventName, double probability)
	{
		if (entries.ContainsKey(eventName))
		{
			Entry entry = entries[eventName];
			entry.rangeEnd += probability;
			foreach (Entry value in entries.Values)
			{
				if (value.rangeStart >= entry.rangeStart)
				{
					value.rangeStart += probability;
					value.rangeEnd += probability;
				}
			}
		}
		else
		{
			double num = totalRange;
			double rangeEnd = num + probability;
			entries.Add(eventName, new Entry(num, rangeEnd, eventName));
		}
		totalRange += probability;
	}

	public void Normalize()
	{
		double num = 0.0;
		double num2 = totalRange;
		foreach (Entry value in entries.Values)
		{
			double range = value.Range;
			value.rangeStart = num;
			value.rangeEnd = num + range / num2;
			num = value.rangeEnd;
		}
		totalRange = 1.0;
	}

	public string GetResult()
	{
		float num = UnityEngine.Random.Range(0f, 1f);
		foreach (Entry value in entries.Values)
		{
			if ((double)num >= value.rangeStart && (double)num < value.rangeEnd)
			{
				return value.eventName;
			}
		}
		return null;
	}

	public string GetExpectedValue()
	{
		double num = 0.0;
		foreach (Entry value in entries.Values)
		{
			num += double.Parse(value.eventName) * value.Range;
		}
		return string.Empty + num;
	}

	public string GetLowestValue()
	{
		double num = -1.0;
		foreach (Entry value in entries.Values)
		{
			double num2 = double.Parse(value.eventName);
			if (num == -1.0 || num2 < num)
			{
				num = num2;
			}
		}
		if (num < 0.0)
		{
			num = 0.0;
		}
		return string.Empty + num;
	}

	public IEnumerator<Entry> GetEnumerator()
	{
		return entries.Values.GetEnumerator();
	}

	public ProbabilityTable Where(Func<string, bool> predicate, bool normalize)
	{
		ProbabilityTable probabilityTable = new ProbabilityTable();
		double num = 0.0;
		Dictionary<string, Entry> dictionary = entries;
		List<Entry> list = new List<Entry>();
		foreach (Entry value in dictionary.Values)
		{
			if (predicate(value.eventName))
			{
				list.Add(value);
				num += value.Range;
			}
		}
		double num2 = 0.0;
		foreach (Entry item in list)
		{
			double range = item.Range;
			double num3 = ((!normalize) ? range : (range / num));
			probabilityTable.Add(item.eventName, num3);
			num2 += num3;
		}
		return probabilityTable;
	}

	public double ProbabilityOfEvent(string eventName)
	{
		Entry entry = entries[eventName];
		if (entry == null)
		{
			return 0.0;
		}
		return entry.Range;
	}

	public void AssertLte1()
	{
		TFUtils.Assert(Mathf.Approximately((float)TotalRange, 1f) || TotalRange < 1.0, "Cumulative probability exceeding 1.0. Options after 1.0 are unreachable. Encountered " + TotalRange);
	}

	public override string ToString()
	{
		string text = "[ProbabilityTable (";
		foreach (Entry value in entries.Values)
		{
			string text2 = text;
			text = text2 + "\n p:" + value.Range + ", v:" + value.eventName;
		}
		return text + ")]";
	}
}
