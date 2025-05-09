namespace MTools
{
	public class MWriter
	{
		public virtual bool Open(string filename)
		{
			return false;
		}

		public virtual bool Open(string filename, bool deleteExisting)
		{
			return false;
		}

		public virtual bool Open(string filename, bool deleteExisting, bool createDirectory)
		{
			return false;
		}

		public virtual bool Open(string filename, bool deleteExisting, bool createDirectory, string backupExt)
		{
			return false;
		}

		public virtual bool IsOpen()
		{
			return false;
		}

		public virtual void Close()
		{
		}

		public virtual void Write(byte val)
		{
		}

		public virtual void Write(char val)
		{
		}

		public virtual void Write(ushort val)
		{
		}

		public virtual void Write(short val)
		{
		}

		public virtual void Write(uint val)
		{
		}

		public virtual void Write(int val)
		{
		}

		public virtual void Write(ulong val)
		{
		}

		public virtual void Write(long val)
		{
		}

		public virtual void Write(sbyte val)
		{
		}

		public virtual void Write(float val)
		{
		}

		public virtual void Write(double val)
		{
		}

		public virtual void Write(string val)
		{
		}

		public virtual void Write(char[] arry)
		{
		}

		public virtual void Write(char[] arry, int length)
		{
		}

		public virtual void Flush()
		{
		}

		public virtual void WriteCharArrayAsString(string str)
		{
		}

		public virtual void WriteRawString(string str)
		{
		}

		public virtual void Seek(int offset)
		{
		}

		public virtual int Pos()
		{
			return 0;
		}
	}
}
