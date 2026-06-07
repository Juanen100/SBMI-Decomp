using System.Collections;
using System.Collections.Generic;

public class OrderedSet<T> : ICollection<T>, IEnumerable, IEnumerable<T>
{
	private readonly IDictionary<T, LinkedListNode<T>> dictionary;

	private readonly LinkedList<T> linkedList;

	public int Count
	{
		get
		{
			return 0;
		}
	}

	public virtual bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	public OrderedSet()
	{
	}

	public OrderedSet(IEqualityComparer<T> comparer)
	{
	}

	void ICollection<T>.Add(T item)
	{
	}

	public void Clear()
	{
	}

	public bool Remove(T item)
	{
		return false;
	}

	public IEnumerator<T> GetEnumerator()
	{
		return null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return null;
	}

	public bool Contains(T item)
	{
		return false;
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
	}

	public bool Add(T item)
	{
		return false;
	}

	public T Last()
	{
		return default(T);
	}
}
