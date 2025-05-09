public class SoaringDictionary : SoaringObjectBase
{
	private string[] mKeys;

	private int mCapacity;

	private int mSize;

	private SoaringArray mValues;

	public SoaringObjectBase this[string i]
	{
		get
		{
			return objectWithKey(i);
		}
		set
		{
			addValue(value, i);
		}
	}

	public SoaringDictionary()
		: base(IsType.Dictionary)
	{
		mCapacity = 2;
		mKeys = new string[mCapacity];
		mValues = new SoaringArray(mCapacity);
	}

	public SoaringDictionary(int capacity)
		: base(IsType.Dictionary)
	{
		mCapacity = capacity;
		mKeys = new string[capacity];
		mValues = new SoaringArray(capacity);
	}

	public SoaringDictionary(string json_data)
		: base(IsType.Dictionary)
	{
		mCapacity = 2;
		mKeys = new string[mCapacity];
		mValues = new SoaringArray(mCapacity);
		ReadJson(json_data);
		if (mKeys == null || mValues == null)
		{
			mCapacity = 2;
			mKeys = new string[mCapacity];
			mValues = new SoaringArray(mCapacity);
		}
	}

	public SoaringDictionary(byte[] json_data)
		: base(IsType.Dictionary)
	{
		mCapacity = 2;
		mKeys = new string[mCapacity];
		mValues = new SoaringArray(mCapacity);
		ReadJson(json_data);
		if (mKeys == null || mValues == null)
		{
			mCapacity = 2;
			mKeys = new string[mCapacity];
			mValues = new SoaringArray(mCapacity);
		}
	}

	~SoaringDictionary()
	{
		if (mValues != null)
		{
			mValues.clear();
		}
		mCapacity = 0;
		mSize = 0;
		mKeys = null;
		mValues = null;
	}

	public int count()
	{
		return mSize;
	}

	public string[] allKeys()
	{
		return mKeys;
	}

	public SoaringObjectBase[] allValues()
	{
		return mValues.array();
	}

	public SoaringDictionary makeCopy()
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(mCapacity);
		for (int i = 0; i < mSize; i++)
		{
			soaringDictionary.addValue_unsafe(mValues.objectAtIndex(i), mKeys[i]);
		}
		return soaringDictionary;
	}

	public void CopyExisting(SoaringDictionary dictionary)
	{
		clear();
		int num = dictionary.count();
		for (int i = 0; i < num; i++)
		{
			addValue_unsafe(dictionary.objectAtIndex(i), dictionary.allKeys()[i]);
		}
	}

	private void add_key(string key)
	{
		if (key == null)
		{
			return;
		}
		if (mSize == mCapacity)
		{
			mCapacity <<= 1;
			string[] array = new string[mCapacity];
			for (int i = 0; i < mSize; i++)
			{
				array[i] = mKeys[i];
			}
			mKeys = array;
			array = null;
		}
		mKeys[mSize] = key;
		mSize++;
	}

	public void addValue(SoaringValue val, string key)
	{
		addValue((SoaringObjectBase)val, key);
	}

	public void addValue(SoaringObjectBase val, string key)
	{
		if (val != null && key != null)
		{
			int num = indexOfObjectWithKey(key);
			if (num == -1)
			{
				add_key(key);
				mValues.addObject(val);
			}
		}
	}

	public void setValue(SoaringValue val, string key)
	{
		setValue((SoaringObjectBase)val, key);
	}

	public void setValue(SoaringObjectBase val, string key)
	{
		if (val != null && key != null)
		{
			int num = indexOfObjectWithKey(key);
			if (num != -1)
			{
				mValues.setObjectAtIndex(val, num);
				return;
			}
			add_key(key);
			mValues.addObject(val);
		}
	}

	public void addValue_unsafe(SoaringObjectBase val, string key)
	{
		if (val != null && key != null)
		{
			add_key(key);
			mValues.addObject(val);
		}
	}

	public SoaringObjectBase objectWithKey(string key)
	{
		SoaringObjectBase result = null;
		for (int i = 0; i < mSize; i++)
		{
			if (mKeys[i] == key)
			{
				result = mValues.objectAtIndex(i);
				break;
			}
		}
		return result;
	}

	public SoaringObjectBase objectWithKey(string key, bool ignoreCase)
	{
		SoaringObjectBase result = null;
		for (int i = 0; i < mSize; i++)
		{
			if (string.Compare(mKeys[i], key, ignoreCase) == 0)
			{
				result = mValues.objectAtIndex(i);
				break;
			}
		}
		return result;
	}

	public SoaringValue soaringValue(string key)
	{
		SoaringValue result = null;
		for (int i = 0; i < mSize; i++)
		{
			if (mKeys[i] == key)
			{
				result = (SoaringValue)mValues.objectAtIndex(i);
				break;
			}
		}
		return result;
	}

	public SoaringObjectBase objectWithType(IsType type)
	{
		SoaringObjectBase result = null;
		for (int i = 0; i < mSize; i++)
		{
			SoaringObjectBase soaringObjectBase = mValues.objectAtIndex(i);
			if (soaringObjectBase.Type == type)
			{
				result = mValues.objectAtIndex(i);
				break;
			}
		}
		return result;
	}

	public void removeObjectWithKey(string key)
	{
		for (int i = 0; i < mSize; i++)
		{
			if (mKeys[i] == key)
			{
				mValues.removeObjectAtIndex(i);
				mKeys[i] = mKeys[mSize - 1];
				mSize--;
				mKeys[mSize] = null;
				break;
			}
		}
	}

	public int indexOfObjectWithKey(string key)
	{
		int result = -1;
		for (int i = 0; i < mSize; i++)
		{
			if (mKeys[i] == key)
			{
				result = i;
				break;
			}
		}
		return result;
	}

	public SoaringObjectBase objectAtIndex(int index)
	{
		if (index < 0 || index >= count())
		{
			return null;
		}
		return mValues.objectAtIndex(index);
	}

	public void removeObjectAtIndex(int index)
	{
		if (index < count())
		{
			mValues.removeObjectAtIndex(index);
			mKeys[index] = mKeys[mSize - 1];
			mSize--;
			mKeys[mSize] = null;
		}
	}

	public void clear()
	{
		if (mKeys != null)
		{
			for (int i = 0; i < mSize; i++)
			{
				mKeys[i] = null;
			}
		}
		mSize = 0;
		if (mValues != null)
		{
			mValues.clear();
		}
	}

	public bool containsKey(string key)
	{
		for (int i = 0; i < mSize; i++)
		{
			if (mKeys[i] == key)
			{
				return true;
			}
		}
		return false;
	}

	public override string ToJsonString()
	{
		string text = "{\n";
		SoaringObjectBase[] array = allValues();
		for (int i = 0; i < mSize; i++)
		{
			if (i != 0)
			{
				text += ",\n";
			}
			text = text + "\"" + mKeys[i] + "\" : ";
			text += array[i].ToJsonString();
		}
		return text + "\n}";
	}

	private void ReadJson(string json)
	{
		if (json != null)
		{
			clear();
			SoaringJSON.jsonDecode(json, this);
		}
	}

	private void ReadJson(byte[] json)
	{
		if (json != null)
		{
			clear();
			SoaringJSON.jsonDecode(json, this);
		}
	}
}
