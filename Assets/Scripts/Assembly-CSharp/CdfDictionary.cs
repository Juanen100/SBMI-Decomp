#define ASSERTS_ON
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CdfDictionary<T>
{
	public delegate T ParseT(object data);

	private Dictionary<string, T> values = new Dictionary<string, T>();

	private ProbabilityTable randomIndexer = new ProbabilityTable();

	public int Count
	{
		get
		{
			return values.Count;
		}
	}

	public List<T> ValuesClone
	{
		get
		{
			return new List<T>(values.Values);
		}
	}

	public static CdfDictionary<T> FromList(List<object> data, ParseT parser)
	{
		CdfDictionary<T> cdfDictionary = new CdfDictionary<T>();
		foreach (object datum in data)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)datum;
			if (SBSettings.AssertDataValidity)
			{
				TFUtils.AssertKeyExists(dictionary, "p");
				TFUtils.AssertKeyExists(dictionary, "value");
			}
			double probability = TFUtils.LoadDouble(dictionary, "p");
			T val = parser(dictionary["value"]);
			cdfDictionary.Add(val, probability);
		}
		return cdfDictionary;
	}

	public void Add(T val, double probability)
	{
		string text = val.GetHashCode().ToString();
		values[text] = val;
		randomIndexer.Add(text, probability);
	}

	public CdfDictionary<T> Clone()
	{
		CdfDictionary<T> cdfDictionary = new CdfDictionary<T>();
		cdfDictionary.values = new Dictionary<string, T>(values);
		cdfDictionary.randomIndexer = randomIndexer;
		return cdfDictionary;
	}

	public CdfDictionary<T> Where(Func<T, bool> predicate, bool normalize)
	{
		List<T> matchingValues = values.Values.Where(predicate).ToList();
		CdfDictionary<T> rv = new CdfDictionary<T>();
		rv.values = values.Where((KeyValuePair<string, T> kvp) => matchingValues.Contains(kvp.Value)).ToDictionary((KeyValuePair<string, T> kvp) => kvp.Key, (KeyValuePair<string, T> kvp) => kvp.Value);
		rv.randomIndexer = randomIndexer.Where((string i) => rv.values.Keys.Contains(i), normalize);
		return rv;
	}

	public CdfDictionary<T> Join(CdfDictionary<T> that)
	{
		CdfDictionary<T> rv = new CdfDictionary<T>();
		Action<CdfDictionary<T>> action = delegate(CdfDictionary<T> cdf)
		{
			foreach (ProbabilityTable.Entry item in cdf.randomIndexer)
			{
				rv.Add(cdf.values[item.eventName], item.Range);
			}
		};
		action(this);
		action(that);
		return rv;
	}

	public void Normalize()
	{
		randomIndexer.Normalize();
	}

	public T Spin()
	{
		return Spin(default(T));
	}

	public T Spin(T defaultValue)
	{
		randomIndexer.AssertLte1();
		string result = randomIndexer.GetResult();
		if (result == null)
		{
			return defaultValue;
		}
		return values[result];
	}

	public void Validate(bool ensureFullRange, string message)
	{
		randomIndexer.AssertLte1();
		TFUtils.Assert(!ensureFullRange || Mathf.Approximately((float)randomIndexer.TotalRange, 1f), message + "This CDF Dictionary is required to specify the full range of possibilities (from 0 to 1). Instead it specifies 0-" + randomIndexer.TotalRange);
	}

	public override string ToString()
	{
		string text = string.Empty;
		foreach (string key in values.Keys)
		{
			string text2 = text;
			text = string.Concat(text2, " { p: ", randomIndexer.ProbabilityOfEvent(key), ", v: ", values[key], " }\n");
		}
		return "CDF:[totalRange=" + randomIndexer.TotalRange + "\n" + text + " ]";
	}
}
