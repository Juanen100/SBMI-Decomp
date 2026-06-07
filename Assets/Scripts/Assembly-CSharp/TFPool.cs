using System.Collections.Generic;

public class TFPool<T>
{
	private Stack<T> inactiveList;

	private HashSet<T> activeSet;

	public int SizeOfPool
	{
		get
		{
			return 0;
		}
	}

	public HashSet<T> ActiveSet
	{
		get
		{
			return null;
		}
	}

	public static TFPool<T> CreatePool(int size, Alloc<T> allocDelegate)
	{
		return null;
	}

	public int AllocateToPool(Alloc<T> allocDelegate)
	{
		return 0;
	}

	public T Create(Alloc<T> allocDelegate = null)
	{
		return default(T);
	}

	public bool Release(T item)
	{
		return false;
	}

	public void Clear(Deactivate<T> deactivateDelegate = null)
	{
	}

	public void Purge(Deactivate<T> deactivateDelegate = null)
	{
	}
}
