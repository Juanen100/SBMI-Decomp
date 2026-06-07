public class SoaringArray : SoaringObjectBase
{
	private SoaringObjectBase[] mArray;

	private int mCapacity;

	private int mSize;

	public SoaringArray()
		: base(default(IsType))
	{
	}

	public SoaringArray(int cap)
		: base(default(IsType))
	{
	}

	~SoaringArray()
	{
	}

	public int count()
	{
		return 0;
	}

	public int capacity()
	{
		return 0;
	}

	public void addObject(SoaringValue obj)
	{
	}

	public void addObject(SoaringObjectBase obj)
	{
	}

	public void fastClear()
	{
	}

	public void clear()
	{
	}

	public void fullClear()
	{
	}

	public void reset()
	{
	}

	public void removeObjectAtIndex(int idx)
	{
	}

	public void removeObject(SoaringObjectBase obj)
	{
	}

	public int indexOfObject(SoaringObjectBase obj)
	{
		return 0;
	}

	public SoaringObjectBase[] array()
	{
		return null;
	}

	public void setObjectAtIndex(SoaringObjectBase obj, int index)
	{
	}

	public SoaringObjectBase objectAtIndex(int index)
	{
		return null;
	}

	public SoaringValue soaringValue(int atIndex)
	{
		return null;
	}

	public bool containsObject(SoaringObjectBase obj)
	{
		return false;
	}

	public void swapObjects(int swap, int with)
	{
	}

	public override string ToJsonString()
	{
		return null;
	}
}
public class SoaringArray<T> : SoaringObjectBase where T : SoaringObjectBase
{
	private T[] mArray;

	private int mCapacity;

	private int mSize;

	public T Item
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public SoaringArray()
		: base(default(IsType))
	{
	}

	public SoaringArray(int cap)
		: base(default(IsType))
	{
	}

	~SoaringArray()
	{
	}

	public int count()
	{
		return 0;
	}

	public int capacity()
	{
		return 0;
	}

	public void addObject(SoaringValue obj)
	{
	}

	public void addObject(SoaringObjectBase obj)
	{
	}

	public void fastClear()
	{
	}

	public void clear()
	{
	}

	public void fullClear()
	{
	}

	public void reset()
	{
	}

	public void removeObjectAtIndex(int idx)
	{
	}

	public void removeObject(SoaringObjectBase obj)
	{
	}

	public int indexOfObject(SoaringObjectBase obj)
	{
		return 0;
	}

	public T[] array()
	{
		return null;
	}

	public void setObjectAtIndex(T obj, int index)
	{
	}

	public T objectAtIndex(int index)
	{
		return null;
	}

	public bool containsObject(T obj)
	{
		return false;
	}

	public void swapObjects(int swap, int with)
	{
	}

	public override string ToJsonString()
	{
		return null;
	}
}
