using System;

namespace MTools
{
	public class MBufferReader : MReader
	{
		private MArray mBufferList;

		private int mCurrentBuffer;

		public MBufferReader()
		{
			mBufferList = new MArray();
		}

		public override bool Open(string path)
		{
			mCurrentBuffer = 0;
			return IsOpen();
		}

		public override bool Open(byte[] byteArray)
		{
			mCurrentBuffer = 0;
			return IsOpen();
		}

		public void SetBuffers(MBuffer[] buffersArray)
		{
			mBufferList.clear();
			if (buffersArray != null)
			{
				for (int i = 0; i < buffersArray.Length; i++)
				{
					buffersArray[i].useLength = buffersArray[i].bufferSize;
					mBufferList.addObject(buffersArray[i]);
				}
			}
		}

		public MBuffer CheckActiveBuffer(int writeSpace)
		{
			if (writeSpace < 0 || mCurrentBuffer >= mBufferList.count())
			{
				return null;
			}
			MBuffer mBuffer = (MBuffer)mBufferList.objectAtIndex(mCurrentBuffer);
			if (mBuffer.readPos + writeSpace > mBuffer.useLength)
			{
				mCurrentBuffer++;
				mBuffer = (MBuffer)mBufferList.objectAtIndex(mCurrentBuffer);
			}
			return mBuffer;
		}

		public override void Close()
		{
			mBufferList.clear();
			mCurrentBuffer = 0;
		}

		public override bool IsOpen()
		{
			return mBufferList.count() != 0;
		}

		public override byte ReadByte()
		{
			MBuffer mBuffer = CheckActiveBuffer(1);
			byte result = mBuffer.buffer[mBuffer.readPos];
			mBuffer.IncrimentReadBuffer(1);
			return result;
		}

		public override sbyte ReadSByte()
		{
			MBuffer mBuffer = CheckActiveBuffer(1);
			sbyte result = (sbyte)mBuffer.buffer[mBuffer.readPos];
			mBuffer.IncrimentReadBuffer(1);
			return result;
		}

		public override ushort ReadUShort()
		{
			MBuffer mBuffer = CheckActiveBuffer(2);
			ushort result = (ushort)(mBuffer.buffer[mBuffer.readPos] | (mBuffer.buffer[mBuffer.readPos + 1] << 8));
			mBuffer.IncrimentReadBuffer(2);
			return result;
		}

		public override short ReadShort()
		{
			MBuffer mBuffer = CheckActiveBuffer(2);
			short result = (short)(mBuffer.buffer[mBuffer.readPos] | (mBuffer.buffer[mBuffer.readPos + 1] << 8));
			mBuffer.IncrimentReadBuffer(2);
			return result;
		}

		public override uint ReadUInt()
		{
			MBuffer mBuffer = CheckActiveBuffer(4);
			uint result = (uint)(mBuffer.buffer[mBuffer.readPos] | (mBuffer.buffer[mBuffer.readPos + 1] << 8) | (mBuffer.buffer[mBuffer.readPos + 2] << 16) | (mBuffer.buffer[mBuffer.readPos + 3] << 24));
			mBuffer.IncrimentReadBuffer(4);
			return result;
		}

		public override int ReadInt()
		{
			MBuffer mBuffer = CheckActiveBuffer(4);
			int result = mBuffer.buffer[mBuffer.readPos] | (mBuffer.buffer[mBuffer.readPos + 1] << 8) | (mBuffer.buffer[mBuffer.readPos + 2] << 16) | (mBuffer.buffer[mBuffer.readPos + 3] << 24);
			mBuffer.IncrimentReadBuffer(4);
			return result;
		}

		public override ulong ReadULong()
		{
			MBuffer mBuffer = CheckActiveBuffer(8);
			ulong result = (ulong)(mBuffer.buffer[mBuffer.readPos] | (mBuffer.buffer[mBuffer.readPos + 1] << 8) | (mBuffer.buffer[mBuffer.readPos + 2] << 16) | (mBuffer.buffer[mBuffer.readPos + 3] << 24) | mBuffer.buffer[mBuffer.readPos + 4] | (mBuffer.buffer[mBuffer.readPos + 5] << 8) | (mBuffer.buffer[mBuffer.readPos + 6] << 16) | (mBuffer.buffer[mBuffer.readPos + 7] << 24));
			mBuffer.IncrimentReadBuffer(8);
			return result;
		}

		public override long ReadLong()
		{
			MBuffer mBuffer = CheckActiveBuffer(8);
			long result = mBuffer.buffer[mBuffer.readPos] | (mBuffer.buffer[mBuffer.readPos + 1] << 8) | (mBuffer.buffer[mBuffer.readPos + 2] << 16) | (mBuffer.buffer[mBuffer.readPos + 3] << 24) | mBuffer.buffer[mBuffer.readPos + 4] | (mBuffer.buffer[mBuffer.readPos + 5] << 8) | (mBuffer.buffer[mBuffer.readPos + 6] << 16) | (mBuffer.buffer[mBuffer.readPos + 7] << 24);
			mBuffer.IncrimentReadBuffer(8);
			return result;
		}

		public override float ReadFloat()
		{
			MBuffer mBuffer = CheckActiveBuffer(4);
			float result = BitConverter.ToSingle(mBuffer.buffer, mBuffer.readPos);
			mBuffer.IncrimentReadBuffer(4);
			return result;
		}

		public override float ReadSingle()
		{
			return ReadFloat();
		}

		public override double ReadDouble()
		{
			MBuffer mBuffer = CheckActiveBuffer(8);
			double result = BitConverter.ToDouble(mBuffer.buffer, mBuffer.readPos);
			mBuffer.IncrimentReadBuffer(8);
			return result;
		}

		public override string ReadString()
		{
			return ReadCharArrayAsString();
		}

		public override char[] ReadCharArray(int count)
		{
			return null;
		}

		public override int ReadBytes(int length, ref byte[] bytes)
		{
			if (length <= 0)
			{
				return 0;
			}
			MBuffer mBuffer = CheckActiveBuffer(length);
			bytes = new byte[length];
			for (int i = 0; i < length; i++)
			{
				bytes[i] = mBuffer.buffer[mBuffer.readPos + i];
			}
			mBuffer.IncrimentReadBuffer(length);
			return length;
		}

		public override byte[] ReadAllBytes()
		{
			return null;
		}

		public override string ReadCharArrayAsString()
		{
			MBuffer mBuffer = CheckActiveBuffer(4);
			int num = mBuffer.buffer[mBuffer.readPos] | (mBuffer.buffer[mBuffer.readPos + 1] << 8) | (mBuffer.buffer[mBuffer.readPos + 2] << 16) | (mBuffer.buffer[mBuffer.readPos + 3] << 24);
			mBuffer.IncrimentReadBuffer(4);
			char[] array = new char[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = (char)mBuffer.buffer[mBuffer.readPos + i];
			}
			mBuffer.IncrimentReadBuffer(num);
			return new string(array);
		}

		public override int FileLength()
		{
			int num = 0;
			for (int i = 0; i < mBufferList.count(); i++)
			{
				MBuffer mBuffer = (MBuffer)mBufferList.objectAtIndex(i);
				num += mBuffer.useLength;
			}
			return num;
		}

		public override long FileLengthLong()
		{
			long num = 0L;
			for (int i = 0; i < mBufferList.count(); i++)
			{
				MBuffer mBuffer = (MBuffer)mBufferList.objectAtIndex(i);
				num += mBuffer.useLength;
			}
			return num;
		}

		public override void Seek(int offset)
		{
			MBuffer mBuffer = null;
			int num = 0;
			for (int i = 0; i < mBufferList.count(); i++)
			{
				mBuffer = (MBuffer)mBufferList.objectAtIndex(i);
				if (offset < mBuffer.useLength + num && offset > num)
				{
					mCurrentBuffer = i;
					int readPos = mBuffer.useLength + num - offset;
					mBuffer.readPos = readPos;
				}
				else
				{
					mBuffer.readPos = mBuffer.useLength;
				}
			}
			for (int j = mCurrentBuffer + 1; j < mBufferList.count(); j++)
			{
				mBuffer = (MBuffer)mBufferList.objectAtIndex(j);
				mBuffer.readPos = 0;
			}
		}

		public override int Pos()
		{
			MBuffer mBuffer = null;
			int num = 0;
			for (int i = 0; i < mCurrentBuffer; i++)
			{
				mBuffer = (MBuffer)mBufferList.objectAtIndex(i);
				num += mBuffer.useLength;
			}
			mBuffer = (MBuffer)mBufferList.objectAtIndex(mCurrentBuffer);
			return num + mBuffer.readPos;
		}
	}
}
