using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace DeltaDNA
{
	internal sealed class EngageArchive
	{
		private Hashtable _table = new Hashtable();

		private object _lock = new object();

		private static readonly string FILENAME = "ENGAGEMENTS";

		private string _path;

		public string this[string decisionPoint]
		{
			get
			{
				return _table[decisionPoint] as string;
			}
			set
			{
				lock (_lock)
				{
					_table[decisionPoint] = value;
				}
			}
		}

		public EngageArchive(string path)
		{
			Load(path);
			_path = path;
		}

		public bool Contains(string decisionPoint)
		{
			Debug.Log("Does Engage contain " + decisionPoint);
			return _table.ContainsKey(decisionPoint);
		}

		private void Load(string path)
		{
			lock (_lock)
			{
				try
				{
					string text = Path.Combine(path, FILENAME);
					Debug.Log("Loading Engage from " + text);
					if (!File.Exists(text))
					{
						return;
					}
					using (FileStream fileStream = new FileStream(text, FileMode.Open, FileAccess.Read))
					{
						string key = null;
						string text2 = null;
						int num = 0;
						byte[] array = new byte[4];
						while (fileStream.Read(array, 0, array.Length) > 0)
						{
							int num2 = BitConverter.ToInt32(array, 0);
							byte[] array2 = new byte[num2];
							fileStream.Read(array2, 0, array2.Length);
							if (num % 2 == 0)
							{
								key = Encoding.UTF8.GetString(array2, 0, array2.Length);
							}
							else
							{
								text2 = Encoding.UTF8.GetString(array2, 0, array2.Length);
								_table.Add(key, text2);
							}
							num++;
						}
					}
				}
				catch (Exception ex)
				{
					Debug.LogWarning("Unable to load Engagement archive: " + ex.Message);
				}
			}
		}

		public void Save()
		{
			lock (_lock)
			{
				try
				{
					if (!Directory.Exists(_path))
					{
						Directory.CreateDirectory(_path);
					}
					List<byte> list = new List<byte>();
					foreach (DictionaryEntry item in _table)
					{
						byte[] bytes = Encoding.UTF8.GetBytes(item.Key as string);
						byte[] bytes2 = BitConverter.GetBytes(bytes.Length);
						byte[] bytes3 = Encoding.UTF8.GetBytes(item.Value as string);
						byte[] bytes4 = BitConverter.GetBytes(bytes3.Length);
						list.AddRange(bytes2);
						list.AddRange(bytes);
						list.AddRange(bytes4);
						list.AddRange(bytes3);
					}
					byte[] array = list.ToArray();
					string path = Path.Combine(_path, FILENAME);
					using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
					{
						fileStream.Write(array, 0, array.Length);
					}
				}
				catch (Exception ex)
				{
					Debug.LogWarning("Unable to save Engagement archive: " + ex.Message);
				}
			}
		}

		public void Clear()
		{
			lock (_lock)
			{
				_table.Clear();
			}
		}
	}
}
