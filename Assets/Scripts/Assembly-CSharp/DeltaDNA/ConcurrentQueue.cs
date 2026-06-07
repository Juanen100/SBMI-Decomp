using System.Collections.Generic;

namespace DeltaDNA
{
	internal class ConcurrentQueue<T>
	{
		private readonly object queueLock;

		private Queue<T> queue;

		public int Count
		{
			get
			{
				return 0;
			}
		}

		public T Peek()
		{
			return default(T);
		}

		public void Enqueue(T obj)
		{
		}

		public T Dequeue()
		{
			return default(T);
		}

		public void Clear()
		{
		}
	}
}
