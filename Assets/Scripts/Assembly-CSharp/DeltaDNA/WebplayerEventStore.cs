using System;
using System.Collections.Generic;

namespace DeltaDNA
{
	public class WebplayerEventStore : IEventStore, IDisposable
	{
		private Queue<string> inEvents = new Queue<string>();

		private Queue<string> outEvents = new Queue<string>();

		private bool disposed;

		public bool Push(string obj)
		{
			inEvents.Enqueue(obj);
			return true;
		}

		public bool Swap()
		{
			if (outEvents.Count == 0)
			{
				Queue<string> queue = outEvents;
				outEvents = inEvents;
				inEvents = queue;
				return true;
			}
			return false;
		}

		public List<string> Read()
		{
			List<string> list = new List<string>();
			foreach (string outEvent in outEvents)
			{
				list.Add(outEvent);
			}
			return list;
		}

		public void Clear()
		{
			outEvents.Clear();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~WebplayerEventStore()
		{
			Dispose(false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
				}
				disposed = true;
			}
		}
	}
}
