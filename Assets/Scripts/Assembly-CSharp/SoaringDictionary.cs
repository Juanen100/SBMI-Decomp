public class SoaringDictionary : SoaringObjectBase
{
	private string[] mKeys;

	private int mCapacity;

	private int mSize;

	private SoaringArray mValues;

	public SoaringObjectBase Item
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public SoaringDictionary()
		: base(default(IsType))
	{
	}

	public SoaringDictionary(int capacity)
		: base(default(IsType))
	{
	}

	public SoaringDictionary(string json_data)
		: base(default(IsType))
	{
	}

	public SoaringDictionary(byte[] json_data)
		: base(default(IsType))
	{
	}

	~SoaringDictionary()
	{
	}

	public int count()
	{
		return 0;
	}

	public string[] allKeys()
	{
		return null;
	}

	public SoaringObjectBase[] allValues()
	{
		return null;
	}

	public SoaringDictionary makeCopy()
	{
		return null;
	}

	public void CopyExisting(SoaringDictionary dictionary)
	{
	}

	private void add_key(string key)
	{
	}

	public void addValue(SoaringValue val, string key)
	{
	}

	public void addValue(SoaringObjectBase val, string key)
	{
	}

	public void setValue(SoaringValue val, string key)
	{
	}

	public void setValue(SoaringObjectBase val, string key)
	{
	}

	public void addValue_unsafe(SoaringObjectBase val, string key)
	{
	}

	public SoaringObjectBase objectWithKey(string key)
	{
		return null;
	}

	public SoaringObjectBase objectWithKey(string key, bool ignoreCase)
	{
		return null;
	}

	public SoaringValue soaringValue(string key)
	{
		return null;
	}

	public SoaringObjectBase objectWithType(IsType type)
	{
		return null;
	}

	public void removeObjectWithKey(string key)
	{
	}

	public int indexOfObjectWithKey(string key)
	{
		return 0;
	}

	public SoaringObjectBase objectAtIndex(int index)
	{
		return null;
	}

	public void removeObjectAtIndex(int index)
	{
	}

	public void clear()
	{
	}

	public bool containsKey(string key)
	{
		return false;
	}

	public override string ToJsonString()
	{
		return null;
	}

	private void ReadJson(string json)
	{
	}

	private void ReadJson(byte[] json)
	{
	}
}
