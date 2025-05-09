using System;

namespace MTools
{
	public class MBuffer
	{
		public byte[] buffer;

		public int writePos;

		public int readPos;

		public int useLength;

		public int bufferSize;

		public MBuffer(int size)
		{
			bufferSize = size;
			if (bufferSize > 32768 || bufferSize < 4)
			{
				bufferSize = 32768;
			}
			while (buffer == null && bufferSize > 4)
			{
				try
				{
					byte[] array = new byte[bufferSize];
					buffer = array;
				}
				catch (Exception)
				{
					bufferSize <<= 1;
				}
			}
		}

		public MBuffer(byte[] readbuffer)
		{
			buffer = readbuffer;
			bufferSize = buffer.Length;
			useLength = 0;
			writePos = 0;
			readPos = 0;
		}

		public void IncrimentWriteBuffer(int size)
		{
			writePos += size;
			useLength += size;
		}

		public void IncrimentReadBuffer(int size)
		{
			readPos += size;
		}
	}
}
