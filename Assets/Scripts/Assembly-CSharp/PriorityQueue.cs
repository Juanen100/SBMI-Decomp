using System;
using System.Collections.Generic;

public class PriorityQueue<T> where T : IComparable<T>
{
	private List<T> values;

	public int Count
	{
		get
		{
			return values.Count;
		}
	}

	public PriorityQueue()
	{
		values = new List<T>();
	}

	public bool Empty()
	{
		return 0 == values.Count;
	}

	public void Push(T value)
	{
		values.Add(value);
		Sort();
	}

	public T Pop()
	{
		int index = values.Count - 1;
		T result = values[index];
		values.RemoveAt(index);
		return result;
	}

	public T Find(Predicate<T> predicate)
	{
		return values.Find(predicate);
	}

	public void Sort()
	{
		values.Sort();
	}

	public void Clear()
	{
		values.Clear();
	}
}
