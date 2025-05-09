using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace DeltaDNA
{
	public class EventStore : IEventStore, IDisposable
	{
		private static readonly string PF_KEY_IN_FILE = "DDSDK_EVENT_IN_FILE";

		private static readonly string PF_KEY_OUT_FILE = "DDSDK_EVENT_OUT_FILE";

		private static readonly string FILE_A = "A";

		private static readonly string FILE_B = "B";

		private static readonly int FILE_BUFFER_SIZE = 4096;

		private static readonly long MAX_FILE_SIZE = 41943040L;

		private FileStream infs;

		private FileStream outfs;

		private bool initialised;

		private bool disposed;

		private bool debug;

		public EventStore(string path, bool debug = false)
		{
			this.debug = debug;
			try
			{
				InitialiseFileStreams(path, false);
				initialised = true;
			}
			catch (Exception ex)
			{
				Log("Problem initialising Event Store: " + ex.Message);
			}
		}

		public bool Push(string obj)
		{
			if (initialised && infs.Length < MAX_FILE_SIZE)
			{
				try
				{
					byte[] bytes = Encoding.UTF8.GetBytes(obj);
					byte[] bytes2 = BitConverter.GetBytes(bytes.Length);
					List<byte> list = new List<byte>();
					list.AddRange(bytes2);
					list.AddRange(bytes);
					byte[] array = list.ToArray();
					infs.Write(array, 0, array.Length);
					return true;
				}
				catch (Exception ex)
				{
					Log("Problem pushing event to Event Store: " + ex.Message);
				}
			}
			return false;
		}

		public bool Swap()
		{
			if (outfs.Length == 0L)
			{
				infs.Flush();
				FileStream fileStream = infs;
				infs = outfs;
				outfs = fileStream;
				infs.SetLength(0L);
				outfs.Seek(0L, SeekOrigin.Begin);
				PlayerPrefs.SetString(PF_KEY_IN_FILE, Path.GetFileName(infs.Name));
				PlayerPrefs.SetString(PF_KEY_OUT_FILE, Path.GetFileName(outfs.Name));
				return true;
			}
			return false;
		}

		public List<string> Read()
		{
			List<string> list = new List<string>();
			try
			{
				byte[] array = new byte[4];
				while (outfs.Read(array, 0, array.Length) > 0)
				{
					int num = BitConverter.ToInt32(array, 0);
					byte[] array2 = new byte[num];
					outfs.Read(array2, 0, array2.Length);
					string item = Encoding.UTF8.GetString(array2, 0, array2.Length);
					list.Add(item);
				}
				outfs.Seek(0L, SeekOrigin.Begin);
			}
			catch (Exception ex)
			{
				Log("Problem reading events from Event Store: " + ex.Message);
			}
			return list;
		}

		public void Clear()
		{
			infs.SetLength(0L);
			outfs.SetLength(0L);
		}

		private void InitialiseFileStreams(string path, bool reset)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			string path2 = PlayerPrefs.GetString(PF_KEY_IN_FILE, FILE_A);
			string path3 = PlayerPrefs.GetString(PF_KEY_OUT_FILE, FILE_B);
			path2 = Path.GetFileName(path2);
			path3 = Path.GetFileName(path3);
			string text = Path.Combine(path, path2);
			string text2 = Path.Combine(path, path3);
			FileMode mode = ((!reset) ? FileMode.OpenOrCreate : FileMode.Create);
			if (File.Exists(text) && File.Exists(text2) && !reset)
			{
				Log("Loaded existing Event Store in @ " + text + " out @ " + text2);
			}
			else
			{
				Log("Creating new Event Store in @ " + path);
			}
			infs = new FileStream(text, mode, FileAccess.ReadWrite, FileShare.None, FILE_BUFFER_SIZE);
			infs.Seek(0L, SeekOrigin.End);
			outfs = new FileStream(text2, mode, FileAccess.ReadWrite, FileShare.None, FILE_BUFFER_SIZE);
			PlayerPrefs.SetString(PF_KEY_IN_FILE, Path.GetFileName(infs.Name));
			PlayerPrefs.SetString(PF_KEY_OUT_FILE, Path.GetFileName(outfs.Name));
		}

		private void Log(string message)
		{
			if (debug)
			{
				Debug.Log("[DDSDK EventStore] " + message);
			}
		}

		~EventStore()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			Log("Disposing on EventStore...");
			try
			{
				if (!disposed && disposing)
				{
					Log("Disposing filestreams");
					infs.Dispose();
					outfs.Dispose();
				}
			}
			finally
			{
				disposed = true;
			}
		}
	}
}
