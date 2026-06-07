namespace MTools
{
	public class MList
	{
		public class MListNode
		{
			public object data;

			public MListNode next;

			public MListNode prev;
		}

		private MListNode mStart;

		private MListNode mEnd;

		private int mCount;

		private bool mIsCircular;

		public MList()
		{
		}

		public MList(bool circular)
		{
		}

		public int count()
		{
			return 0;
		}

		public void PushFront(object data)
		{
		}

		public void PushBack(object data)
		{
		}

		public void Insert(object data, int offset)
		{
		}

		public object ObjectAtIndex(int idx)
		{
			return null;
		}

		public object GetFront()
		{
			return null;
		}

		public object GetBack()
		{
			return null;
		}

		public object PopFront()
		{
			return null;
		}

		public object PopBack()
		{
			return null;
		}
	}
}
