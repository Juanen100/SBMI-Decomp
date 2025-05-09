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
			mIsCircular = circular;
		}

		public int count()
		{
			return mCount;
		}

		public void PushFront(object data)
		{
			if (mStart == null)
			{
				if (mIsCircular)
				{
					mStart = new MListNode();
					mStart.data = data;
					mEnd = mStart;
					mStart.next = mStart;
					mStart.prev = mStart;
				}
				else
				{
					mStart = new MListNode();
					mStart.data = data;
					mEnd = mStart;
					mStart.next = null;
					mStart.prev = null;
				}
				mCount = 1;
				return;
			}
			if (mIsCircular)
			{
				MListNode mListNode = new MListNode();
				mListNode.data = data;
				mListNode.next = mStart;
				mListNode.prev = mEnd;
				mEnd.next = mListNode;
				mStart.prev = mListNode;
				mStart = mListNode;
			}
			else
			{
				MListNode mListNode2 = new MListNode();
				mListNode2.data = data;
				mStart.prev = mListNode2;
				mListNode2.next = mStart;
				mStart = mListNode2;
			}
			mCount++;
		}

		public void PushBack(object data)
		{
			if (mEnd == null)
			{
				if (mIsCircular)
				{
					mEnd = new MListNode();
					mEnd.data = data;
					mEnd = mStart;
					mEnd.next = mEnd;
					mEnd.prev = mEnd;
				}
				else
				{
					mEnd = new MListNode();
					mEnd.data = data;
					mEnd.next = null;
					mEnd.prev = null;
					mStart = mEnd;
				}
				mCount = 1;
				return;
			}
			if (mIsCircular)
			{
				MListNode mListNode = new MListNode();
				mListNode.data = data;
				mListNode.next = mStart;
				mListNode.prev = mEnd;
				mStart.prev = mListNode;
				mEnd.next = mListNode;
				mEnd = mListNode;
			}
			else
			{
				MListNode mListNode2 = new MListNode();
				mListNode2.data = data;
				mEnd.next = mListNode2;
				mListNode2.prev = mEnd;
				mEnd = mListNode2;
			}
			mCount++;
		}

		public void Insert(object data, int offset)
		{
			if (data == null)
			{
				return;
			}
			if (mStart == null)
			{
				PushFront(data);
				return;
			}
			if (!mIsCircular)
			{
				if (offset < 0)
				{
					PushFront(data);
					return;
				}
				if (offset > mCount)
				{
					PushBack(data);
					return;
				}
			}
			MListNode next = mStart;
			for (int i = 0; i < offset; i++)
			{
				next = next.next;
			}
			MListNode mListNode = new MListNode();
			mListNode.data = data;
			mListNode.prev = next;
			mListNode.next = next.next;
			next.next.prev = mListNode;
			next.next = mListNode;
			mCount++;
		}

		public object ObjectAtIndex(int idx)
		{
			if (idx < 0)
			{
				return GetFront();
			}
			if (idx >= mCount && !mIsCircular)
			{
				return GetBack();
			}
			MListNode next = mStart;
			for (int i = 0; i < idx; i++)
			{
				next = next.next;
			}
			return next.data;
		}

		public object GetFront()
		{
			if (mStart == null)
			{
				return null;
			}
			return mStart.data;
		}

		public object GetBack()
		{
			if (mEnd == null)
			{
				return null;
			}
			return mEnd.data;
		}

		public object PopFront()
		{
			object obj = null;
			if (mStart == null)
			{
				return null;
			}
			obj = mStart.data;
			if (mIsCircular)
			{
				MListNode next = mStart.next;
				next.prev = mEnd;
				mEnd.next = next;
				mStart.next = null;
				mStart.prev = null;
				mStart.data = null;
				mStart = next;
			}
			else
			{
				MListNode next2 = mStart.next;
				mStart.data = null;
				mStart.next = null;
				mStart.prev = null;
				mStart = next2;
				if (next2 != null)
				{
					next2.prev = null;
				}
				else
				{
					mEnd = (mStart = null);
				}
			}
			mCount--;
			return obj;
		}

		public object PopBack()
		{
			object obj = null;
			if (mStart == null)
			{
				return null;
			}
			obj = mEnd.data;
			if (mIsCircular)
			{
				MListNode prev = mEnd.prev;
				prev.next = mStart;
				mStart.prev = prev;
				mEnd.next = null;
				mEnd.prev = null;
				mEnd.data = null;
				mEnd = prev;
			}
			else
			{
				MListNode prev2 = mEnd.prev;
				mEnd.data = null;
				mEnd.next = null;
				mEnd.prev = null;
				mEnd = prev2;
				if (prev2 != null)
				{
					prev2.next = null;
				}
				else
				{
					mEnd = (mStart = null);
				}
			}
			mCount--;
			return obj;
		}
	}
}
