public class SoaringArray : SoaringObjectBase
{
	private SoaringObjectBase[] mArray;

	private int mCapacity;

	private int mSize;

	public SoaringArray()
		: base(IsType.Array)
	{
		mCapacity = 2;
		mArray = new SoaringObjectBase[mCapacity];
	}

	public SoaringArray(int cap)
		: base(IsType.Array)
	{
		if (cap < 1)
		{
			cap = 1;
		}
		mCapacity = cap;
		mArray = new SoaringObjectBase[mCapacity];
	}

	~SoaringArray()
	{
		int i = 0;
		for (; i < mCapacity; i++)
		{
			mArray[i] = null;
		}
		mArray = null;
		mCapacity = 0;
		mSize = 0;
	}

	public int count()
	{
		return mSize;
	}

	public int capacity()
	{
		return mCapacity;
	}

	public void addObject(SoaringValue obj)
	{
		addObject((SoaringObjectBase)obj);
	}

	public void addObject(SoaringObjectBase obj)
	{
		if (obj == null)
		{
			return;
		}
		if (mCapacity == mSize)
		{
			mCapacity <<= 1;
			SoaringObjectBase[] array = new SoaringObjectBase[mCapacity];
			for (int i = 0; i < mSize; i++)
			{
				array[i] = mArray[i];
			}
			mArray = array;
			array = null;
		}
		mArray[mSize] = obj;
		mSize++;
	}

	public void fastClear()
	{
		mSize = 0;
	}

	public void clear()
	{
		for (int i = 0; i < mSize; i++)
		{
			mArray[i] = null;
		}
		mSize = 0;
	}

	public void fullClear()
	{
		for (int i = 0; i < mCapacity; i++)
		{
			mArray[i] = null;
		}
		mSize = 0;
	}

	public void reset()
	{
		mCapacity = 2;
		mSize = 0;
		mArray = new SoaringObjectBase[mCapacity];
	}

	public void removeObjectAtIndex(int idx)
	{
		mArray[idx] = mArray[mSize - 1];
		mSize--;
		mArray[mSize] = null;
	}

	public void removeObject(SoaringObjectBase obj)
	{
		int num = indexOfObject(obj);
		if (num != -1)
		{
			removeObjectAtIndex(num);
		}
	}

	public int indexOfObject(SoaringObjectBase obj)
	{
		int result = -1;
		for (int i = 0; i < mSize; i++)
		{
			if (mArray[i] == obj)
			{
				result = i;
				break;
			}
		}
		return result;
	}

	public SoaringObjectBase[] array()
	{
		return mArray;
	}

	public void setObjectAtIndex(SoaringObjectBase obj, int index)
	{
		mArray[index] = obj;
	}

	public SoaringObjectBase objectAtIndex(int index)
	{
		return mArray[index];
	}

	public SoaringValue soaringValue(int atIndex)
	{
		return (SoaringValue)mArray[atIndex];
	}

	public bool containsObject(SoaringObjectBase obj)
	{
		return indexOfObject(obj) != -1;
	}

	public void swapObjects(int swap, int with)
	{
		SoaringObjectBase soaringObjectBase = mArray[swap];
		mArray[swap] = mArray[with];
		mArray[with] = soaringObjectBase;
	}

	public override string ToJsonString()
	{
		string text = "[\n";
		for (int i = 0; i < mSize; i++)
		{
			if (i != 0)
			{
				text += ",\n";
			}
			text += mArray[i].ToJsonString();
		}
		return text + "\n]";
	}
}
public class SoaringArray<T> : SoaringObjectBase where T : SoaringObjectBase
{
	private T[] mArray;

	private int mCapacity;

	private int mSize;

	public T this[int index]
	{
		get
		{
			return mArray[index];
		}
		set
		{
			mArray[index] = value;
		}
	}

	public SoaringArray()
		: base(IsType.Array)
	{
		mCapacity = 2;
		mArray = new T[mCapacity];
	}

	public SoaringArray(int cap)
		: base(IsType.Array)
	{
		if (cap < 1)
		{
			cap = 1;
		}
		mCapacity = cap;
		mArray = new T[mCapacity];
	}

	~SoaringArray()
	{
		int i = 0;
		for (; i < mCapacity; i++)
		{
			mArray[i] = (T)null;
		}
		mArray = null;
		mCapacity = 0;
		mSize = 0;
	}

	public int count()
	{
		return mSize;
	}

	public int capacity()
	{
		return mCapacity;
	}

	public void addObject(SoaringValue obj)
	{
		addObject((SoaringObjectBase)obj);
	}

	public void addObject(SoaringObjectBase obj)
	{
		if (obj == null)
		{
			return;
		}
		if (mCapacity == mSize)
		{
			mCapacity <<= 1;
			T[] array = new T[mCapacity];
			for (int i = 0; i < mSize; i++)
			{
				array[i] = mArray[i];
			}
			mArray = array;
			array = null;
		}
		mArray[mSize] = (T)obj;
		mSize++;
	}

	public void fastClear()
	{
		mSize = 0;
	}

	public void clear()
	{
		for (int i = 0; i < mSize; i++)
		{
			mArray[i] = (T)null;
		}
		mSize = 0;
	}

	public void fullClear()
	{
		for (int i = 0; i < mCapacity; i++)
		{
			mArray[i] = (T)null;
		}
		mSize = 0;
	}

	public void reset()
	{
		mCapacity = 2;
		mSize = 0;
		mArray = new T[mCapacity];
	}

	public void removeObjectAtIndex(int idx)
	{
		mArray[idx] = mArray[mSize - 1];
		mSize--;
		mArray[mSize] = (T)null;
	}

	public void removeObject(SoaringObjectBase obj)
	{
		int num = indexOfObject(obj);
		if (num != -1)
		{
			removeObjectAtIndex(num);
		}
	}

	public int indexOfObject(SoaringObjectBase obj)
	{
		int result = -1;
		for (int i = 0; i < mSize; i++)
		{
			if (mArray[i] == obj)
			{
				result = i;
				break;
			}
		}
		return result;
	}

	public T[] array()
	{
		return mArray;
	}

	public void setObjectAtIndex(T obj, int index)
	{
		mArray[index] = obj;
	}

	public T objectAtIndex(int index)
	{
		return mArray[index];
	}

	public bool containsObject(T obj)
	{
		return indexOfObject(obj) != -1;
	}

	public void swapObjects(int swap, int with)
	{
		T val = mArray[swap];
		mArray[swap] = mArray[with];
		mArray[with] = val;
	}

	public override string ToJsonString()
	{
		string text = "[\n";
		for (int i = 0; i < mSize; i++)
		{
			if (i != 0)
			{
				text += ",\n";
			}
			text += mArray[i].ToJsonString();
		}
		return text + "\n]";
	}
}
