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
		}

		public MStringHash(MStringHashDelegate del)
		{
		}

		public MStringHash(int cap)
		{
		}

		public MStringHash()
		{
		}

		private void setHashArray(MStringHashDelegate del, int cap)
		{
		}

		public void addObjectWithKey(object obj, string key)
		{
		}

		public void setObjectWithKey(object obj, string key)
		{
		}

		public void removeObjectWithKey(string key)
		{
		}

		public void clear()
		{
		}

		public object objectWithKey(string key)
		{
			return null;
		}

		private int _SoftHash(string key)
		{
			return 0;
		}

		public int indexOfObjectWithKey(string key)
		{
			return 0;
		}

		public int indexOfObjectWithKey(int key)
		{
			return 0;
		}
	}
}
