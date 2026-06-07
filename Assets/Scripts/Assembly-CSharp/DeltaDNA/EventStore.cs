using System;
using System.Collections.Generic;
using System.IO;

namespace DeltaDNA
{
	public class EventStore : IDisposable
	{
		private static readonly string PF_KEY_IN_FILE;

		private static readonly string PF_KEY_OUT_FILE;

		private static readonly string FILE_A;

		private static readonly string FILE_B;

		private static readonly long MAX_FILE_SIZE_BYTES;

		private bool _initialised;

		private bool _disposed;

		private Stream _infs;

		private Stream _outfs;

		private static object _lock;

		public bool IsInitialised
		{
			get
			{
				return false;
			}
		}

		public EventStore(string dir)
		{
		}

		public bool Push(string obj)
		{
			return false;
		}

		public bool Swap()
		{
			return false;
		}

		public List<string> Read()
		{
			return null;
		}

		public void ClearOut()
		{
		}

		public void ClearAll()
		{
		}

		public void FlushBuffers()
		{
		}

		~EventStore()
		{
		}

		public void Dispose()
		{
		}

		protected virtual void Dispose(bool disposing)
		{
		}

		private bool InitialiseFileStreams(string dir)
		{
			return false;
		}

		public static bool PushEvent(string obj, Stream stream)
		{
			return false;
		}

		public static void ReadEvents(Stream stream, IList<string> events)
		{
		}

		public static void SwapStreams(ref Stream sin, ref Stream sout)
		{
		}

		public static void ClearStream(Stream stream)
		{
		}
	}
}
