namespace MTools
{
	public class MStringHash
	{
		public delegate int MStringHashDelegate(string key);

		private MArray<int> mKeys;

		private MArray mValues;

		private MStringHashDelegate mKeyHash;

		public MStringHash(MStringHashDelegate del, int cap)
		{
			setHashArray(del, cap);
		}

		public MStringHash(MStringHashDelegate del)
		{
			setHashArray(del, 2);
		}

		public MStringHash(int cap)
		{
			setHashArray(null, cap);
		}

		public MStringHash()
		{
			setHashArray(null, 2);
		}

		private void setHashArray(MStringHashDelegate del, int cap)
		{
			if (cap <= 0)
			{
				cap = 2;
			}
			if (del == null)
			{
				del = _SoftHash;
			}
			mKeyHash = del;
			mValues = new MArray(cap);
			mKeys = new MArray<int>(cap);
		}

		public void addObjectWithKey(object obj, string key)
		{
			int num = mKeyHash(key);
			int num2 = indexOfObjectWithKey(num);
			if (num2 != -1)
			{
				mValues.setObjectAtIndex(obj, num2);
				return;
			}
			mValues.addObject(obj);
			mKeys.addObject(num);
		}

		public void setObjectWithKey(object obj, string key)
		{
			int num = mKeyHash(key);
			int num2 = indexOfObjectWithKey(num);
			if (num2 != -1)
			{
				mValues.setObjectAtIndex(obj, num2);
				return;
			}
			mValues.addObject(obj);
			mKeys.addObject(num);
		}

		public void removeObjectWithKey(string key)
		{
			int key2 = mKeyHash(key);
			int num = indexOfObjectWithKey(key2);
			if (num != -1)
			{
				mValues.removeObjectAtIndex(num);
				mKeys.removeObjectAtIndex(num);
			}
		}

		public void clear()
		{
			mValues.clear();
			mKeys.clear();
		}

		public object objectWithKey(string key)
		{
			int key2 = mKeyHash(key);
			int num = indexOfObjectWithKey(key2);
			if (num != -1)
			{
				return mValues.objectAtIndex(num);
			}
			return null;
		}

		private int _SoftHash(string key)
		{
			int num = 0;
			int length = key.Length;
			for (int i = 0; i < length; i++)
			{
				num += key[i];
			}
			return num + length;
		}

		public int indexOfObjectWithKey(string key)
		{
			int num = mKeyHash(key);
			int[] array = mKeys.array();
			int num2 = mKeys.count();
			for (int i = 0; i < num2; i++)
			{
				if (array[i] == num)
				{
					return i;
				}
			}
			return -1;
		}

		public int indexOfObjectWithKey(int key)
		{
			int[] array = mKeys.array();
			int num = mKeys.count();
			for (int i = 0; i < num; i++)
			{
				if (array[i] == key)
				{
					return i;
				}
			}
			return -1;
		}
	}
}
