using System.Collections;
using System.Collections.Generic;

public class OrderedSet<T> : IEnumerable, ICollection<T>, IEnumerable<T>
{
	private readonly IDictionary<T, LinkedListNode<T>> dictionary;

	private readonly LinkedList<T> linkedList;

	public int Count
	{
		get
		{
			return dictionary.Count;
		}
	}

	public virtual bool IsReadOnly
	{
		get
		{
			return dictionary.IsReadOnly;
		}
	}

	public OrderedSet()
		: this((IEqualityComparer<T>)EqualityComparer<T>.Default)
	{
	}

	public OrderedSet(IEqualityComparer<T> comparer)
	{
		dictionary = new Dictionary<T, LinkedListNode<T>>(comparer);
		linkedList = new LinkedList<T>();
	}

	void ICollection<T>.Add(T item)
	{
		Add(item);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public void Clear()
	{
		linkedList.Clear();
		dictionary.Clear();
	}

	public bool Remove(T item)
	{
		LinkedListNode<T> value;
		if (!dictionary.TryGetValue(item, out value))
		{
			return false;
		}
		dictionary.Remove(item);
		linkedList.Remove(value);
		return true;
	}

	public IEnumerator<T> GetEnumerator()
	{
		return linkedList.GetEnumerator();
	}

	public bool Contains(T item)
	{
		return dictionary.ContainsKey(item);
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
		linkedList.CopyTo(array, arrayIndex);
	}

	public bool Add(T item)
	{
		if (dictionary.ContainsKey(item))
		{
			return false;
		}
		LinkedListNode<T> value = linkedList.AddLast(item);
		dictionary.Add(item, value);
		return true;
	}

	public T Last()
	{
		return linkedList.Last.Value;
	}
}
