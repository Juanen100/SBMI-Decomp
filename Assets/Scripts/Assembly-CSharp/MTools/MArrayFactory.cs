namespace MTools
{
	public static class MArrayFactory
	{
		private const int sMaxArraySize = 32;

		private static MArray[] sMArrayList;

		private static void _Setup()
		{
			sMArrayList = new MArray[32];
			for (int i = 0; i < 32; i++)
			{
				sMArrayList[i] = new MArray(2);
			}
		}

		public static MArray MArray(int capacity)
		{
			if (sMArrayList == null)
			{
				_Setup();
			}
			if (capacity >= 32 || capacity < 0)
			{
				return new MArray();
			}
			MArray mArray = sMArrayList[capacity];
			int num = mArray.count();
			if (num <= 0)
			{
				return new MArray(capacity);
			}
			MArray result = (MArray)mArray.objectAtIndex(num - 1);
			mArray.removeObjectAtIndex(num - 1);
			return result;
		}

		public static void ReturnMArray(MArray arr)
		{
			if (arr != null && sMArrayList != null)
			{
				int num = arr.capacity();
				if (num < 32 && num >= 0)
				{
					arr.clear();
					sMArrayList[num].addObject(arr);
				}
			}
		}
	}
}
