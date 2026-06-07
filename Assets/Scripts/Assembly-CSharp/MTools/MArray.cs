namespace MTools
{
	public class MArray
	{
		public const int MArray_Allocated = 0;

		private object[] mArray;

		private int mCapacity;

		private int mSize;

		public MArray()
		{
		}

		public MArray(MArray arr)
		{
		}

		public MArray(int cap)
		{
		}

		public static void SaveArrayReport()
		{
		}

		public static void ClearArrayReport()
		{
		}

		~MArray()
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

		public void addObject(object obj)
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

		public void removeObject(object obj)
		{
		}

		public int indexOfObject(object obj)
		{
			return 0;
		}

		public int indexOfEquivelentObject(object obj)
		{
			return 0;
		}

		public object[] array()
		{
			return null;
		}

		public void setObjectAtIndex(object obj, int index)
		{
		}

		public object objectAtIndex(int index)
		{
			return null;
		}

		public bool containsObject(object obj)
		{
			return false;
		}

		public void swapObjects(int swap, int with)
		{
		}

		public void randomize()
		{
		}
	}
	public class MArray<T>
	{
		private T[] mArray;

		private int mCapacity;

		private int mSize;

		public T Item
		{
			get
			{
				return default(T);
			}
			set
			{
			}
		}

		public MArray()
		{
		}

		public MArray(int cap)
		{
		}

		~MArray()
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

		public void addObject(T obj)
		{
		}

		public void clear()
		{
		}

		public void reset()
		{
		}

		public void removeObjectAtIndex(int idx)
		{
		}

		public void setObjectAtIndex(T obj, int index)
		{
		}

		public T objectAtIndex(int index)
		{
			return default(T);
		}

		public void swapObjects(int swap, int with)
		{
		}

		public T[] array()
		{
			return null;
		}

		public T[] resizedArray()
		{
			return null;
		}

		public void randomize()
		{
		}
	}
}
