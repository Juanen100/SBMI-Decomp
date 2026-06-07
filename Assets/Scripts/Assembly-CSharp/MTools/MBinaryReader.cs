using System.IO;

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
				return null;
			}
		}

		public MBinaryReader(string filePath)
		{
		}

		public MBinaryReader(byte[] data)
		{
		}

		public MBinaryReader()
		{
		}

		~MBinaryReader()
		{
		}

		public override bool Open(string path)
		{
			return false;
		}

		public override bool Open(byte[] byteArray)
		{
			return false;
		}

		public override void Close()
		{
		}

		public override bool IsOpen()
		{
			return false;
		}

		public override byte ReadByte()
		{
			return 0;
		}

		public override sbyte ReadSByte()
		{
			return 0;
		}

		public override ushort ReadUShort()
		{
			return 0;
		}

		public override short ReadShort()
		{
			return 0;
		}

		public override uint ReadUInt()
		{
			return 0u;
		}

		public override int ReadInt()
		{
			return 0;
		}

		public override ulong ReadULong()
		{
			return 0uL;
		}

		public override long ReadLong()
		{
			return 0L;
		}

		public override float ReadFloat()
		{
			return 0f;
		}

		public override float ReadSingle()
		{
			return 0f;
		}

		public override double ReadDouble()
		{
			return 0.0;
		}

		public override string ReadString()
		{
			return null;
		}

		public override char[] ReadCharArray(int count)
		{
			return null;
		}

		public override int ReadBytes(int length, ref byte[] buffer)
		{
			return 0;
		}

		public override byte[] ReadAllBytes()
		{
			return null;
		}

		public override string ReadCharArrayAsString()
		{
			return null;
		}

		public override int FileLength()
		{
			return 0;
		}

		public override long FileLengthLong()
		{
			return 0L;
		}

		public override void Seek(int offset)
		{
		}

		public override int Pos()
		{
			return 0;
		}
	}
}
