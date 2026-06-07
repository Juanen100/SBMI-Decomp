namespace MTools
{
	public class MDictionary
	{
		private MArray mKeys;

		private MArray mValues;

		public MDictionary()
		{
		}

		public MDictionary(MArray keys, MArray values)
		{
		}

		public MDictionary(string[] keys, MArray values)
		{
		}

		public MDictionary(int capacity)
		{
		}

		public static MDictionary Create(params string[] values)
		{
			return null;
		}

		~MDictionary()
		{
		}

		public int count()
		{
			return 0;
		}

		public MArray allKeys()
		{
			return null;
		}

		public MArray allValues()
		{
			return null;
		}

		public void addValue(object val, string key)
		{
		}

		public void setValue(object val, string key)
		{
		}

		public void addValue_unsafe(object val, string key)
		{
		}

		public object objectWithKey(string key)
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

		public object objectAtIndex(int index)
		{
			return null;
		}

		public void clear()
		{
		}

		public bool containsKey(string key)
		{
			return false;
		}
	}
}
