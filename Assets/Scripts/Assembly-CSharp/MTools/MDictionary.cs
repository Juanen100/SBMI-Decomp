namespace MTools
{
	public class MDictionary
	{
		private MArray mKeys;

		private MArray mValues;

		public MDictionary()
		{
			mKeys = new MArray(2);
			mValues = new MArray(2);
		}

		public MDictionary(MArray keys, MArray values)
		{
			mKeys = keys;
			if (mKeys == null)
			{
				mKeys = new MArray(2);
			}
			mValues = values;
			if (mValues == null)
			{
				mValues = new MArray(2);
			}
		}

		public MDictionary(string[] keys, MArray values)
		{
			MArray mArray = new MArray(keys.Length);
			int num = keys.Length;
			for (int i = 0; i < num; i++)
			{
				mArray.addObject(keys[i]);
			}
			mKeys = mArray;
			if (mKeys == null)
			{
				mKeys = new MArray(2);
			}
			mValues = values;
			if (mValues == null)
			{
				mValues = new MArray(2);
			}
		}

		public MDictionary(int capacity)
		{
			mKeys = new MArray(capacity);
			mValues = new MArray(capacity);
		}

		public static MDictionary Create(params string[] values)
		{
			MDictionary mDictionary = new MDictionary();
			if (values == null)
			{
				return mDictionary;
			}
			int num = values.Length;
			if ((num & 1) == 1)
			{
				return mDictionary;
			}
			num <<= 1;
			for (int i = 0; i < num; i++)
			{
				string val = values[i];
				string key = values[i];
				mDictionary.addValue_unsafe(val, key);
			}
			return mDictionary;
		}

		~MDictionary()
		{
			mKeys.clear();
			mValues.clear();
			mKeys = null;
			mValues = null;
		}

		public int count()
		{
			return mKeys.count();
		}

		public MArray allKeys()
		{
			return mKeys;
		}

		public MArray allValues()
		{
			return mValues;
		}

		public void addValue(object val, string key)
		{
			if (val != null && key != null)
			{
				int num = indexOfObjectWithKey(key);
				if (num == -1)
				{
					mKeys.addObject(key);
					mValues.addObject(val);
				}
			}
		}

		public void setValue(object val, string key)
		{
			if (val != null && key != null)
			{
				int num = indexOfObjectWithKey(key);
				if (num != -1)
				{
					mValues.setObjectAtIndex(val, num);
					return;
				}
				mKeys.addObject(key);
				mValues.addObject(val);
			}
		}

		public void addValue_unsafe(object val, string key)
		{
			if (val != null && key != null)
			{
				mKeys.addObject(key);
				mValues.addObject(val);
			}
		}

		public object objectWithKey(string key)
		{
			object result = null;
			int num = mKeys.count();
			for (int i = 0; i < num; i++)
			{
				string text = (string)mKeys.objectAtIndex(i);
				if (text == key)
				{
					result = mValues.objectAtIndex(i);
					break;
				}
			}
			return result;
		}

		public void removeObjectWithKey(string key)
		{
			int num = mKeys.count();
			for (int i = 0; i < num; i++)
			{
				string text = (string)mKeys.objectAtIndex(i);
				if (text == key)
				{
					mValues.removeObjectAtIndex(i);
					mKeys.removeObjectAtIndex(i);
					break;
				}
			}
		}

		public int indexOfObjectWithKey(string key)
		{
			int result = -1;
			int num = mKeys.count();
			for (int i = 0; i < num; i++)
			{
				string text = (string)mKeys.objectAtIndex(i);
				if (text == key)
				{
					result = i;
					break;
				}
			}
			return result;
		}

		public object objectAtIndex(int index)
		{
			if (index < 0 || index >= count())
			{
				return null;
			}
			return mValues.objectAtIndex(index);
		}

		public void clear()
		{
			mValues.clear();
			mKeys.clear();
		}

		public bool containsKey(string key)
		{
			int num = mKeys.count();
			for (int i = 0; i < num; i++)
			{
				string text = (string)mKeys.objectAtIndex(i);
				if (text == key)
				{
					return true;
				}
			}
			return false;
		}
	}
}
