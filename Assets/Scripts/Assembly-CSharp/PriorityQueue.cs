using System;
using System.Collections.Generic;

public class PriorityQueue<T> where T : IComparable<T>
{
	private List<T> values;

	public int Count
	{
		get
		{
			return 0;
		}
	}

	public bool Empty()
	{
		return false;
	}

	public void Push(T value)
	{
	}

	public T Pop()
	{
		return default(T);
	}

	public T Find(Predicate<T> predicate)
	{
		return default(T);
	}

	public void Sort()
	{
	}

	public void Clear()
	{
	}
}
