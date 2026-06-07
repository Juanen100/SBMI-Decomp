using System.IO;

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
		}

		public MBinaryWriter()
		{
		}

		~MBinaryWriter()
		{
		}

		public override bool Open(string filename)
		{
			return false;
		}

		public override bool Open(string filename, bool deleteExisting)
		{
			return false;
		}

		public override bool Open(string filename, bool deleteExisting, bool createDirectory)
		{
			return false;
		}

		public override bool Open(string filename, bool deleteExisting, bool createDirectory, string backupExt)
		{
			return false;
		}

		public override bool IsOpen()
		{
			return false;
		}

		public override void Close()
		{
		}

		public string GetFilePath()
		{
			return null;
		}

		public override void Write(byte val)
		{
		}

		public override void Write(char val)
		{
		}

		public override void Write(ushort val)
		{
		}

		public override void Write(short val)
		{
		}

		public override void Write(uint val)
		{
		}

		public override void Write(int val)
		{
		}

		public override void Write(ulong val)
		{
		}

		public override void Write(long val)
		{
		}

		public override void Write(sbyte val)
		{
		}

		public override void Write(float val)
		{
		}

		public override void Write(double val)
		{
		}

		public override void Write(string val)
		{
		}

		public override void Write(char[] arry)
		{
		}

		public override void Write(char[] arry, int length)
		{
		}

		public void Write(byte[] arry)
		{
		}

		public override void Flush()
		{
		}

		public override void WriteCharArrayAsString(string str)
		{
		}

		public override void WriteRawString(string str)
		{
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
