using System;
using System.Collections;
using System.Collections.Generic;

public class ProbabilityTable : ResultGenerator, IEnumerable<ProbabilityTable.Entry>, IEnumerable
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
				return 0.0;
			}
		}

		public Entry(double rangeStart, double rangeEnd, string eventName)
		{
		}

		public override string ToString()
		{
			return null;
		}
	}

	private Dictionary<string, Entry> entries;

	private double totalRange;

	public double TotalRange
	{
		get
		{
			return 0.0;
		}
	}

	public ProbabilityTable()
	{
	}

	public ProbabilityTable(Dictionary<string, object> dict)
	{
	}

	public void Add(string eventName, double probability)
	{
	}

	public void Normalize()
	{
	}

	public string GetResult()
	{
		return null;
	}

	public string GetExpectedValue()
	{
		return null;
	}

	public string GetLowestValue()
	{
		return null;
	}

	public IEnumerator<Entry> GetEnumerator()
	{
		return null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return null;
	}

	public ProbabilityTable Where(Func<string, bool> predicate, bool normalize)
	{
		return null;
	}

	public double ProbabilityOfEvent(string eventName)
	{
		return 0.0;
	}

	public void AssertLte1()
	{
	}

	public override string ToString()
	{
		return null;
	}
}
