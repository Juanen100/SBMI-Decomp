using System;
using System.IO;
using UnityEngine;

namespace MTools
{
	public class MBinaryReader : MReader
	{
		private string FilePath;

		private bool isFileOpen;

		private Stream stream;

		private System.IO.BinaryReader reader;

		public Stream Stream
		{
			get
			{
				return stream;
			}
		}

		public MBinaryReader(string filePath)
		{
			Open(filePath);
		}

		public MBinaryReader(byte[] data)
		{
			Open(data);
		}

		public MBinaryReader()
		{
		}

		~MBinaryReader()
		{
			Close();
			stream = null;
			reader = null;
		}

		public override bool Open(string path)
		{
			if (isFileOpen)
			{
				return false;
			}
			if (!File.Exists(path))
			{
				return false;
			}
			try
			{
				FilePath = path;
				stream = File.OpenRead(FilePath);
				reader = new System.IO.BinaryReader(stream);
				isFileOpen = true;
			}
			catch
			{
				isFileOpen = false;
			}
			return isFileOpen;
		}

		public override bool Open(byte[] byteArray)
		{
			if (isFileOpen)
			{
				return false;
			}
			if (byteArray == null)
			{
				return false;
			}
			if (byteArray.Length == 0)
			{
				return false;
			}
			try
			{
				FilePath = null;
				stream = new MemoryStream(byteArray);
				reader = new System.IO.BinaryReader(stream);
				isFileOpen = true;
			}
			catch
			{
				stream = null;
				reader = null;
				isFileOpen = false;
			}
			return isFileOpen;
		}

		public override void Close()
		{
			if (!isFileOpen)
			{
				return;
			}
			try
			{
				if (reader != null)
				{
					reader.Close();
				}
				if (stream != null)
				{
					stream.Close();
				}
				stream = null;
				reader = null;
				isFileOpen = false;
			}
			catch (Exception ex)
			{
				Debug.Log(ex.Message);
			}
		}

		public override bool IsOpen()
		{
			return isFileOpen;
		}

		public override byte ReadByte()
		{
			return reader.ReadByte();
		}

		public override sbyte ReadSByte()
		{
			return reader.ReadSByte();
		}

		public override ushort ReadUShort()
		{
			return reader.ReadUInt16();
		}

		public override short ReadShort()
		{
			return reader.ReadInt16();
		}

		public override uint ReadUInt()
		{
			return reader.ReadUInt32();
		}

		public override int ReadInt()
		{
			return reader.ReadInt32();
		}

		public override ulong ReadULong()
		{
			return reader.ReadUInt64();
		}

		public override long ReadLong()
		{
			return reader.ReadInt64();
		}

		public override float ReadFloat()
		{
			return reader.ReadSingle();
		}

		public override float ReadSingle()
		{
			return reader.ReadSingle();
		}

		public override double ReadDouble()
		{
			return reader.ReadDouble();
		}

		public override string ReadString()
		{
			return ReadCharArrayAsString();
		}

		public override char[] ReadCharArray(int count)
		{
			return reader.ReadChars(count);
		}

		public override int ReadBytes(int length, ref byte[] buffer)
		{
			long num = 0L;
			if (buffer == null)
			{
				buffer = new byte[length];
			}
			num = reader.BaseStream.Length - reader.BaseStream.Position;
			if (num - length > 0)
			{
				num = length;
			}
			for (int i = 0; i < num; i++)
			{
				buffer[i] = reader.ReadByte();
			}
			return (int)num;
		}

		public override byte[] ReadAllBytes()
		{
			if (!isFileOpen)
			{
				return null;
			}
			reader.BaseStream.Position = 0L;
			return reader.ReadBytes((int)reader.BaseStream.Length);
		}

		public override string ReadCharArrayAsString()
		{
			string text = string.Empty;
			ushort num = reader.ReadUInt16();
			if (num == 0)
			{
				return text;
			}
			for (int i = 0; i < num; i++)
			{
				text += (char)reader.ReadByte();
			}
			return text;
		}

		public override int FileLength()
		{
			return (int)reader.BaseStream.Length;
		}

		public override long FileLengthLong()
		{
			return reader.BaseStream.Length;
		}

		public override void Seek(int offset)
		{
			reader.BaseStream.Seek(offset, SeekOrigin.Begin);
		}

		public override int Pos()
		{
			return (int)reader.BaseStream.Position;
		}
	}
}
