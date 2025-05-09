#define ASSERTS_ON
using System.Collections.Generic;
using UnityEngine;

public class TFPool<T>
{
	private Stack<T> inactiveList = new Stack<T>();

	private HashSet<T> activeSet = new HashSet<T>();

	public int SizeOfPool
	{
		get
		{
			return inactiveList.Count;
		}
	}

	public HashSet<T> ActiveSet
	{
		get
		{
			return activeSet;
		}
	}

	public static TFPool<T> CreatePool(int size, Alloc<T> allocDelegate)
	{
		TFPool<T> tFPool = new TFPool<T>();
		for (int i = 0; i < size; i++)
		{
			tFPool.AllocateToPool(allocDelegate);
		}
		return tFPool;
	}

	public int AllocateToPool(Alloc<T> allocDelegate)
	{
		TFUtils.Assert(allocDelegate != null, "TFPool.AllocateToPool requires valid allocDelegate");
		T t = allocDelegate();
		inactiveList.Push(t);
		return SizeOfPool;
	}

	public T Create(Alloc<T> allocDelegate = null)
	{
		T val;
		if (inactiveList.Count > 0)
		{
			val = inactiveList.Pop();
		}
		else
		{
			if (allocDelegate == null)
			{
				throw new UnityException("TFPool.Create(): Pool is empty and no alloc is provided.");
			}
			val = allocDelegate();
		}
		activeSet.Add(val);
		return val;
	}

	public bool Release(T item)
	{
		if (activeSet.Remove(item))
		{
			inactiveList.Push(item);
			return true;
		}
		return false;
	}

	public void Clear(Deactivate<T> deactivateDelegate = null)
	{
		Deactivate<T> deactivate = ((deactivateDelegate == null) ? ((Deactivate<T>)delegate
		{
		}) : deactivateDelegate);
		foreach (T item in activeSet)
		{
			deactivate(item);
			inactiveList.Push(item);
		}
		activeSet.Clear();
	}

	public void Purge(Deactivate<T> deactivateDelegate = null)
	{
		Clear();
		Deactivate<T> deactivate = ((deactivateDelegate == null) ? ((Deactivate<T>)delegate
		{
		}) : deactivateDelegate);
		while (inactiveList.Count > 0)
		{
			deactivate(inactiveList.Pop());
		}
	}
}
