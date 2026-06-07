namespace MTools
{
	public class MBufferWriter : MWriter
	{
		private MBuffer mCurrentBuffer;

		private MArray mBuffersList;

		private int mDefaultBufferSize;

		public int BuffersCount
		{
			get
			{
				return 0;
			}
		}

		public MBufferWriter(int bufferSize)
		{
		}

		public int GetBuffer(ref byte[] buffer, int idx)
		{
			return 0;
		}

		private MBuffer AllocateSpace(int space)
		{
			return null;
		}

		public override bool Open(string filename, bool deleteExisting, bool createDirectory)
		{
			return false;
		}

		public override bool Open(string filename, bool deleteExisting, bool createDirectory, string backupEXT)
		{
			return false;
		}

		public override bool IsOpen()
		{
			return false;
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

		public override void WriteCharArrayAsString(string str)
		{
		}

		public override void WriteRawString(string str)
		{
		}

		public override void Flush()
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
