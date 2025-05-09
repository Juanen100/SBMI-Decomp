using System;

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
				return mBuffersList.count();
			}
		}

		public MBufferWriter(int bufferSize)
		{
			mDefaultBufferSize = bufferSize;
			mBuffersList = new MArray(4);
			mBuffersList.addObject(new MBuffer(mDefaultBufferSize));
			mCurrentBuffer = (MBuffer)mBuffersList.objectAtIndex(0);
		}

		public int GetBuffer(ref byte[] buffer, int idx)
		{
			buffer = null;
			if (idx < 0 || idx > BuffersCount)
			{
				return 0;
			}
			MBuffer mBuffer = (MBuffer)mBuffersList.objectAtIndex(idx);
			buffer = mBuffer.buffer;
			return mBuffer.useLength;
		}

		private MBuffer AllocateSpace(int space)
		{
			if (mCurrentBuffer.writePos + space >= mCurrentBuffer.bufferSize)
			{
				mCurrentBuffer = new MBuffer(mDefaultBufferSize);
				mBuffersList.addObject(mCurrentBuffer);
			}
			return mCurrentBuffer;
		}

		public override bool Open(string filename, bool deleteExisting, bool createDirectory)
		{
			return true;
		}

		public override bool Open(string filename, bool deleteExisting, bool createDirectory, string backupEXT)
		{
			return true;
		}

		public override bool IsOpen()
		{
			return true;
		}

		public override void Write(byte val)
		{
			MBuffer mBuffer = AllocateSpace(1);
			mBuffer.buffer[mBuffer.writePos] = val;
			mBuffer.IncrimentWriteBuffer(1);
		}

		public override void Write(char val)
		{
			MBuffer mBuffer = AllocateSpace(2);
			mBuffer.buffer[mBuffer.writePos] = (byte)val;
			mBuffer.IncrimentWriteBuffer(1);
		}

		public override void Write(ushort val)
		{
			MBuffer mBuffer = AllocateSpace(2);
			mBuffer.buffer[mBuffer.writePos] = (byte)(0xFF & val);
			mBuffer.buffer[mBuffer.writePos + 1] = (byte)(0xFF & (val >> 8));
			mBuffer.IncrimentWriteBuffer(2);
		}

		public override void Write(short val)
		{
			MBuffer mBuffer = AllocateSpace(2);
			mBuffer.buffer[mBuffer.writePos] = (byte)(0xFF & val);
			mBuffer.buffer[mBuffer.writePos + 1] = (byte)(0xFF & (val >> 8));
			mBuffer.IncrimentWriteBuffer(2);
		}

		public override void Write(uint val)
		{
			MBuffer mBuffer = AllocateSpace(4);
			mBuffer.buffer[mBuffer.writePos] = (byte)(0xFF & val);
			mBuffer.buffer[mBuffer.writePos + 1] = (byte)(0xFF & (val >> 8));
			mBuffer.buffer[mBuffer.writePos + 2] = (byte)(0xFF & (val >> 16));
			mBuffer.buffer[mBuffer.writePos + 3] = (byte)(0xFF & (val >> 24));
			mBuffer.IncrimentWriteBuffer(4);
		}

		public override void Write(int val)
		{
			MBuffer mBuffer = AllocateSpace(4);
			mBuffer.buffer[mBuffer.writePos] = (byte)(0xFF & val);
			mBuffer.buffer[mBuffer.writePos + 1] = (byte)(0xFF & (val >> 8));
			mBuffer.buffer[mBuffer.writePos + 2] = (byte)(0xFF & (val >> 16));
			mBuffer.buffer[mBuffer.writePos + 3] = (byte)(0xFF & (val >> 24));
			mBuffer.IncrimentWriteBuffer(4);
		}

		public override void Write(ulong val)
		{
			MBuffer mBuffer = AllocateSpace(8);
			mBuffer.buffer[mBuffer.writePos] = (byte)(0xFF & val);
			mBuffer.buffer[mBuffer.writePos + 1] = (byte)(0xFF & (val >> 8));
			mBuffer.buffer[mBuffer.writePos + 2] = (byte)(0xFF & (val >> 16));
			mBuffer.buffer[mBuffer.writePos + 3] = (byte)(0xFF & (val >> 24));
			mBuffer.buffer[mBuffer.writePos + 4] = (byte)(0xFF & (val >> 32));
			mBuffer.buffer[mBuffer.writePos + 5] = (byte)(0xFF & (val >> 40));
			mBuffer.buffer[mBuffer.writePos + 6] = (byte)(0xFF & (val >> 48));
			mBuffer.buffer[mBuffer.writePos + 7] = (byte)(0xFF & (val >> 56));
			mBuffer.IncrimentWriteBuffer(8);
		}

		public override void Write(long val)
		{
			MBuffer mBuffer = AllocateSpace(8);
			mBuffer.buffer[mBuffer.writePos] = (byte)(0xFF & val);
			mBuffer.buffer[mBuffer.writePos + 1] = (byte)(0xFF & (val >> 8));
			mBuffer.buffer[mBuffer.writePos + 2] = (byte)(0xFF & (val >> 16));
			mBuffer.buffer[mBuffer.writePos + 3] = (byte)(0xFF & (val >> 24));
			mBuffer.buffer[mBuffer.writePos + 4] = (byte)(0xFF & (val >> 32));
			mBuffer.buffer[mBuffer.writePos + 5] = (byte)(0xFF & (val >> 40));
			mBuffer.buffer[mBuffer.writePos + 6] = (byte)(0xFF & (val >> 48));
			mBuffer.buffer[mBuffer.writePos + 7] = (byte)(0xFF & (val >> 56));
			mBuffer.IncrimentWriteBuffer(8);
		}

		public override void Write(sbyte val)
		{
			MBuffer mBuffer = AllocateSpace(1);
			mBuffer.buffer[mBuffer.writePos] = (byte)val;
			mBuffer.IncrimentWriteBuffer(1);
		}

		public override void Write(float val)
		{
			MBuffer mBuffer = AllocateSpace(4);
			byte[] bytes = BitConverter.GetBytes(val);
			mBuffer.buffer[mBuffer.writePos] = bytes[0];
			mBuffer.buffer[mBuffer.writePos + 1] = bytes[1];
			mBuffer.buffer[mBuffer.writePos + 2] = bytes[2];
			mBuffer.buffer[mBuffer.writePos + 3] = bytes[3];
			mBuffer.IncrimentWriteBuffer(4);
		}

		public override void Write(double val)
		{
			MBuffer mBuffer = AllocateSpace(8);
			byte[] bytes = BitConverter.GetBytes(val);
			mBuffer.buffer[mBuffer.writePos] = bytes[0];
			mBuffer.buffer[mBuffer.writePos + 1] = bytes[1];
			mBuffer.buffer[mBuffer.writePos + 2] = bytes[2];
			mBuffer.buffer[mBuffer.writePos + 3] = bytes[3];
			mBuffer.buffer[mBuffer.writePos + 4] = bytes[4];
			mBuffer.buffer[mBuffer.writePos + 5] = bytes[5];
			mBuffer.buffer[mBuffer.writePos + 6] = bytes[6];
			mBuffer.buffer[mBuffer.writePos + 7] = bytes[7];
			mBuffer.IncrimentWriteBuffer(8);
		}

		public override void Write(string val)
		{
			WriteCharArrayAsString(val);
		}

		public override void Write(char[] arry)
		{
			int length = 0;
			if (arry != null)
			{
				length = arry.Length;
			}
			Write(arry, length);
		}

		public override void Write(char[] arry, int length)
		{
			MBuffer mBuffer = AllocateSpace(length + 4);
			mBuffer.buffer[mBuffer.writePos] = (byte)(0xFF & length);
			mBuffer.buffer[mBuffer.writePos + 1] = (byte)(0xFF & (length >> 8));
			mBuffer.buffer[mBuffer.writePos + 2] = (byte)(0xFF & (length >> 16));
			mBuffer.buffer[mBuffer.writePos + 3] = (byte)(0xFF & (length >> 24));
			mBuffer.IncrimentWriteBuffer(4);
			for (int i = 0; i < length; i++)
			{
				mBuffer.buffer[mBuffer.writePos + i] = (byte)arry[i];
			}
			mBuffer.IncrimentWriteBuffer(length);
		}

		public override void WriteCharArrayAsString(string str)
		{
			int num = 0;
			if (str != null)
			{
				num = str.Length;
			}
			MBuffer mBuffer = AllocateSpace(num + 4);
			mBuffer.buffer[mBuffer.writePos] = (byte)(0xFF & num);
			mBuffer.buffer[mBuffer.writePos + 1] = (byte)(0xFF & (num >> 8));
			mBuffer.buffer[mBuffer.writePos + 2] = (byte)(0xFF & (num >> 16));
			mBuffer.buffer[mBuffer.writePos + 3] = (byte)(0xFF & (num >> 24));
			mBuffer.IncrimentWriteBuffer(4);
			for (int i = 0; i < num; i++)
			{
				mBuffer.buffer[mBuffer.writePos + i] = (byte)str[i];
			}
			mBuffer.IncrimentWriteBuffer(num);
		}

		public override void WriteRawString(string str)
		{
			int num = 0;
			if (str != null)
			{
				num = str.Length;
			}
			MBuffer mBuffer = AllocateSpace(num);
			for (int i = 0; i < num; i++)
			{
				mBuffer.buffer[mBuffer.writePos + i] = (byte)str[i];
			}
			mBuffer.IncrimentWriteBuffer(num);
		}

		public override void Flush()
		{
		}

		public override void Seek(int offset)
		{
			MBuffer mBuffer = null;
			int num = 0;
			for (int i = 0; i < mBuffersList.count(); i++)
			{
				mBuffer = (MBuffer)mBuffersList.objectAtIndex(i);
				if (offset < mBuffer.useLength + num && offset > num)
				{
					mCurrentBuffer = mBuffer;
					int writePos = mBuffer.useLength + num - offset;
					mBuffer.writePos = writePos;
				}
				else if (offset < mBuffer.useLength)
				{
					mBuffer.writePos = mBuffer.useLength;
				}
				else
				{
					mBuffer.writePos = 0;
				}
			}
		}

		public override int Pos()
		{
			MBuffer mBuffer = null;
			int num = 0;
			for (int i = 0; i < mBuffersList.count(); i++)
			{
				mBuffer = (MBuffer)mBuffersList.objectAtIndex(i);
				num += mBuffer.writePos;
			}
			return num;
		}
	}
}
