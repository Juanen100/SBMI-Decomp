using System;
using System.Collections.Generic;

public class CdfDictionary<T>
{
	public delegate T ParseT(object data);

	private Dictionary<string, T> values;

	private ProbabilityTable randomIndexer;

	public int Count
	{
		get
		{
			return 0;
		}
	}

	public List<T> ValuesClone
	{
		get
		{
			return null;
		}
	}

	public static CdfDictionary<T> FromList(List<object> data, ParseT parser)
	{
		return null;
	}

	public void Add(T val, double probability)
	{
	}

	public CdfDictionary<T> Clone()
	{
		return null;
	}

	public CdfDictionary<T> Where(Func<T, bool> predicate, bool normalize)
	{
		return null;
	}

	public CdfDictionary<T> Join(CdfDictionary<T> that)
	{
		return null;
	}

	public void Normalize()
	{
	}

	public T Spin()
	{
		return default(T);
	}

	public T Spin(T defaultValue)
	{
		return default(T);
	}

	public void Validate(bool ensureFullRange, string message)
	{
	}

	public override string ToString()
	{
		return null;
	}
}
