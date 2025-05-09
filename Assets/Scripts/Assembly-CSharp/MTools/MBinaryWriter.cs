using System;
using System.IO;
using UnityEngine;

namespace MTools
{
	public class MBinaryWriter : MWriter
	{
		private string FilePath;

		private bool mIsOpen;

		private FileStream stream;

		private System.IO.BinaryWriter writer;

		public MBinaryWriter(string filename)
		{
			Open(filename, false);
		}

		public MBinaryWriter()
		{
		}

		~MBinaryWriter()
		{
			Close();
			stream = null;
			writer = null;
		}

		public override bool Open(string filename)
		{
			return Open(filename, false, false);
		}

		public override bool Open(string filename, bool deleteExisting)
		{
			return Open(filename, deleteExisting, false);
		}

		public override bool Open(string filename, bool deleteExisting, bool createDirectory)
		{
			return Open(filename, deleteExisting, createDirectory, null);
		}

		public override bool Open(string filename, bool deleteExisting, bool createDirectory, string backupExt)
		{
			if (!mIsOpen)
			{
				try
				{
					if (!string.IsNullOrEmpty(backupExt))
					{
						string text = filename + "." + backupExt;
						if (File.Exists(text))
						{
							File.Delete(text);
						}
						if (File.Exists(filename))
						{
							File.Move(filename, text);
						}
					}
					if (deleteExisting && File.Exists(filename))
					{
						File.Delete(filename);
					}
					if (createDirectory)
					{
						string directoryName = Path.GetDirectoryName(filename);
						if (!Directory.Exists(directoryName))
						{
							Directory.CreateDirectory(directoryName);
						}
					}
					if (File.Exists(filename))
					{
						stream = File.OpenWrite(filename);
					}
					else
					{
						stream = File.Create(filename);
					}
					writer = new System.IO.BinaryWriter(stream);
					mIsOpen = true;
				}
				catch (Exception ex)
				{
					Debug.Log(ex.Message + "\n" + ex.StackTrace);
					mIsOpen = false;
				}
			}
			return mIsOpen;
		}

		public override bool IsOpen()
		{
			return mIsOpen;
		}

		public override void Close()
		{
			if (!mIsOpen)
			{
				return;
			}
			try
			{
				if (writer != null)
				{
					writer.Close();
				}
				if (stream != null)
				{
					stream.Close();
				}
				writer = null;
				stream = null;
			}
			catch (Exception ex)
			{
				Debug.Log(ex.Message);
			}
		}

		public string GetFilePath()
		{
			return FilePath;
		}

		public override void Write(byte val)
		{
			writer.Write(val);
		}

		public override void Write(char val)
		{
			writer.Write(val);
		}

		public override void Write(ushort val)
		{
			writer.Write(val);
		}

		public override void Write(short val)
		{
			writer.Write(val);
		}

		public override void Write(uint val)
		{
			writer.Write(val);
		}

		public override void Write(int val)
		{
			writer.Write(val);
		}

		public override void Write(ulong val)
		{
			writer.Write(val);
		}

		public override void Write(long val)
		{
			writer.Write(val);
		}

		public override void Write(sbyte val)
		{
			writer.Write(val);
		}

		public override void Write(float val)
		{
			writer.Write(val);
		}

		public override void Write(double val)
		{
			writer.Write(val);
		}

		public override void Write(string val)
		{
			WriteCharArrayAsString(val);
		}

		public override void Write(char[] arry)
		{
			writer.Write(arry);
		}

		public override void Write(char[] arry, int length)
		{
			writer.Write(arry, 0, length);
		}

		public void Write(byte[] arry)
		{
			writer.Write(arry);
		}

		public override void Flush()
		{
			writer.Flush();
		}

		public override void WriteCharArrayAsString(string str)
		{
			ushort num = 0;
			if (str != null)
			{
				num = (ushort)str.Length;
			}
			writer.Write(num);
			for (int i = 0; i < num; i++)
			{
				writer.Write((byte)str[i]);
			}
		}

		public override void WriteRawString(string str)
		{
			if (str != null)
			{
				int length = str.Length;
				for (int i = 0; i < length; i++)
				{
					writer.Write((byte)str[i]);
				}
			}
		}

		public override void Seek(int offset)
		{
			writer.BaseStream.Seek(offset, SeekOrigin.Begin);
		}

		public override int Pos()
		{
			return (int)writer.BaseStream.Position;
		}
	}
}
