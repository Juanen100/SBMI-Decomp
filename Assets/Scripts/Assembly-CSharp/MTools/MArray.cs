using UnityEngine;

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
			reset();
		}

		public MArray(MArray arr)
		{
			if (arr == null)
			{
				reset();
				return;
			}
			mCapacity = arr.mCapacity;
			mArray = new object[mCapacity];
			mSize = arr.mSize;
			for (int i = 0; i < mSize; i++)
			{
				mArray = arr.mArray;
			}
		}

		public MArray(int cap)
		{
			if (cap < 1)
			{
				cap = 1;
			}
			mCapacity = cap;
			mArray = new object[mCapacity];
		}

		public static void SaveArrayReport()
		{
		}

		public static void ClearArrayReport()
		{
		}

		~MArray()
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

		public void addObject(object obj)
		{
			if (obj == null)
			{
				return;
			}
			if (mCapacity == mSize)
			{
				mCapacity <<= 1;
				object[] array = new object[mCapacity];
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
			mArray = new object[mCapacity];
		}

		public void removeObjectAtIndex(int idx)
		{
			mArray[idx] = mArray[mSize - 1];
			mSize--;
			mArray[mSize] = null;
		}

		public void removeObject(object obj)
		{
			int num = indexOfObject(obj);
			if (num != -1)
			{
				removeObjectAtIndex(num);
			}
		}

		public int indexOfObject(object obj)
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

		public int indexOfEquivelentObject(object obj)
		{
			int result = -1;
			for (int i = 0; i < mSize; i++)
			{
				if (obj.Equals(mArray[i]))
				{
					result = i;
					break;
				}
			}
			return result;
		}

		public object[] array()
		{
			return mArray;
		}

		public void setObjectAtIndex(object obj, int index)
		{
			mArray[index] = obj;
		}

		public object objectAtIndex(int index)
		{
			return mArray[index];
		}

		public bool containsObject(object obj)
		{
			return indexOfObject(obj) != -1;
		}

		public void swapObjects(int swap, int with)
		{
			object obj = mArray[swap];
			mArray[swap] = mArray[with];
			mArray[with] = obj;
		}

		public void randomize()
		{
			for (int num = mSize - 1; num > 0; num--)
			{
				swapObjects(num, Random.Range(0, num));
			}
		}
	}
	public class MArray<T>
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

		public MArray()
		{
			reset();
		}

		public MArray(int cap)
		{
			if (cap < 1)
			{
				cap = 1;
			}
			mCapacity = cap;
			mArray = new T[mCapacity];
		}

		~MArray()
		{
			int i = 0;
			for (; i < mCapacity; i++)
			{
				mArray[i] = default(T);
			}
			mArray = null;
		}

		public int count()
		{
			return mSize;
		}

		public int capacity()
		{
			return mCapacity;
		}

		public void addObject(T obj)
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
			mArray[mSize] = obj;
			mSize++;
		}

		public void clear()
		{
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
			mArray[mSize] = default(T);
		}

		public void setObjectAtIndex(T obj, int index)
		{
			mArray[index] = obj;
		}

		public T objectAtIndex(int index)
		{
			return mArray[index];
		}

		public void swapObjects(int swap, int with)
		{
			T val = mArray[swap];
			mArray[swap] = mArray[with];
			mArray[with] = val;
		}

		public T[] array()
		{
			return mArray;
		}

		public T[] resizedArray()
		{
			T[] array = new T[mSize];
			for (int i = 0; i < mSize; i++)
			{
				array[i] = mArray[i];
			}
			return array;
		}

		public void randomize()
		{
			for (int num = mSize - 1; num > 0; num--)
			{
				swapObjects(num, Random.Range(0, num));
			}
		}
	}
}
