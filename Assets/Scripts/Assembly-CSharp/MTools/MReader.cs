namespace MTools
{
	public class MReader
	{
		public virtual bool Open(string path)
		{
			return false;
		}

		public virtual bool Open(byte[] byteArray)
		{
			return false;
		}

		public virtual void Close()
		{
		}

		public virtual bool IsOpen()
		{
			return false;
		}

		public virtual byte ReadByte()
		{
			return 0;
		}

		public virtual sbyte ReadSByte()
		{
			return 0;
		}

		public virtual ushort ReadUShort()
		{
			return 0;
		}

		public virtual short ReadShort()
		{
			return 0;
		}

		public virtual uint ReadUInt()
		{
			return 0u;
		}

		public virtual int ReadInt()
		{
			return 0;
		}

		public virtual ulong ReadULong()
		{
			return 0uL;
		}

		public virtual long ReadLong()
		{
			return 0L;
		}

		public virtual float ReadFloat()
		{
			return 0f;
		}

		public virtual float ReadSingle()
		{
			return 0f;
		}

		public virtual double ReadDouble()
		{
			return 0.0;
		}

		public virtual string ReadString()
		{
			return null;
		}

		public virtual char[] ReadCharArray(int count)
		{
			return null;
		}

		public virtual int ReadBytes(int length, ref byte[] buffer)
		{
			return 0;
		}

		public virtual byte[] ReadAllBytes()
		{
			return null;
		}

		public virtual string ReadCharArrayAsString()
		{
			return null;
		}

		public virtual int FileLength()
		{
			return 0;
		}

		public virtual long FileLengthLong()
		{
			return 0L;
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
