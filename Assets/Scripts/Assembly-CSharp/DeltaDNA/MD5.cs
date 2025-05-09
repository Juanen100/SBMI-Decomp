using System;
using System.IO;
using System.Text;

namespace DeltaDNA
{
	public class MD5 : IDisposable
	{
		private const byte S11 = 7;

		private const byte S12 = 12;

		private const byte S13 = 17;

		private const byte S14 = 22;

		private const byte S21 = 5;

		private const byte S22 = 9;

		private const byte S23 = 14;

		private const byte S24 = 20;

		private const byte S31 = 4;

		private const byte S32 = 11;

		private const byte S33 = 16;

		private const byte S34 = 23;

		private const byte S41 = 6;

		private const byte S42 = 10;

		private const byte S43 = 15;

		private const byte S44 = 21;

		private static byte[] PADDING;

		private uint[] state = new uint[4];

		private uint[] count = new uint[2];

		private byte[] buffer = new byte[64];

		protected byte[] HashValue;

		protected int State;

		protected int HashSizeValue = 128;

		public virtual bool CanReuseTransform
		{
			get
			{
				return true;
			}
		}

		public virtual bool CanTransformMultipleBlocks
		{
			get
			{
				return true;
			}
		}

		public virtual byte[] Hash
		{
			get
			{
				if (State != 0)
				{
					throw new InvalidOperationException();
				}
				return (byte[])HashValue.Clone();
			}
		}

		public virtual int HashSize
		{
			get
			{
				return HashSizeValue;
			}
		}

		public virtual int InputBlockSize
		{
			get
			{
				return 1;
			}
		}

		public virtual int OutputBlockSize
		{
			get
			{
				return 1;
			}
		}

		internal MD5()
		{
			Initialize();
		}

		static MD5()
		{
			byte[] array = new byte[64];
			array[0] = 128;
			PADDING = array;
		}

		public static MD5 Create(string hashName)
		{
			if (hashName == "MD5")
			{
				return new MD5();
			}
			throw new NotSupportedException();
		}

		public static string GetMd5String(string source)
		{
			MD5 mD = Create();
			UTF8Encoding uTF8Encoding = new UTF8Encoding();
			byte[] bytes = uTF8Encoding.GetBytes(source);
			byte[] array = mD.ComputeHash(bytes);
			StringBuilder stringBuilder = new StringBuilder();
			byte[] array2 = array;
			foreach (byte b in array2)
			{
				stringBuilder.Append(b.ToString("x2"));
			}
			return stringBuilder.ToString();
		}

		public static MD5 Create()
		{
			return new MD5();
		}

		private static uint F(uint x, uint y, uint z)
		{
			return (x & y) | (~x & z);
		}

		private static uint G(uint x, uint y, uint z)
		{
			return (x & z) | (y & ~z);
		}

		private static uint H(uint x, uint y, uint z)
		{
			return x ^ y ^ z;
		}

		private static uint I(uint x, uint y, uint z)
		{
			return y ^ (x | ~z);
		}

		private static uint ROTATE_LEFT(uint x, byte n)
		{
			return (x << (int)n) | (x >> 32 - n);
		}

		private static void FF(ref uint a, uint b, uint c, uint d, uint x, byte s, uint ac)
		{
			a += F(b, c, d) + x + ac;
			a = ROTATE_LEFT(a, s);
			a += b;
		}

		private static void GG(ref uint a, uint b, uint c, uint d, uint x, byte s, uint ac)
		{
			a += G(b, c, d) + x + ac;
			a = ROTATE_LEFT(a, s);
			a += b;
		}

		private static void HH(ref uint a, uint b, uint c, uint d, uint x, byte s, uint ac)
		{
			a += H(b, c, d) + x + ac;
			a = ROTATE_LEFT(a, s);
			a += b;
		}

		private static void II(ref uint a, uint b, uint c, uint d, uint x, byte s, uint ac)
		{
			a += I(b, c, d) + x + ac;
			a = ROTATE_LEFT(a, s);
			a += b;
		}

		public virtual void Initialize()
		{
			count[0] = (count[1] = 0u);
			state[0] = 1732584193u;
			state[1] = 4023233417u;
			state[2] = 2562383102u;
			state[3] = 271733878u;
		}

		protected virtual void HashCore(byte[] input, int offset, int count)
		{
			int num = (int)((this.count[0] >> 3) & 0x3F);
			if ((this.count[0] += (uint)(count << 3)) < (uint)(count << 3))
			{
				this.count[1]++;
			}
			this.count[1] += (uint)count >> 29;
			int num2 = 64 - num;
			int i;
			if (count >= num2)
			{
				Buffer.BlockCopy(input, offset, buffer, num, num2);
				Transform(buffer, 0);
				for (i = num2; i + 63 < count; i += 64)
				{
					Transform(input, offset + i);
				}
				num = 0;
			}
			else
			{
				i = 0;
			}
			Buffer.BlockCopy(input, offset + i, buffer, num, count - i);
		}

		protected virtual byte[] HashFinal()
		{
			byte[] array = new byte[16];
			byte[] array2 = new byte[8];
			Encode(array2, 0, count, 0, 8);
			int num = (int)((count[0] >> 3) & 0x3F);
			int num2 = ((num >= 56) ? (120 - num) : (56 - num));
			HashCore(PADDING, 0, num2);
			HashCore(array2, 0, 8);
			Encode(array, 0, state, 0, 16);
			count[0] = (count[1] = 0u);
			state[0] = 0u;
			state[1] = 0u;
			state[2] = 0u;
			state[3] = 0u;
			Initialize();
			return array;
		}

		private void Transform(byte[] block, int offset)
		{
			uint a = state[0];
			uint a2 = state[1];
			uint a3 = state[2];
			uint a4 = state[3];
			uint[] array = new uint[16];
			Decode(array, 0, block, offset, 64);
			FF(ref a, a2, a3, a4, array[0], 7, 3614090360u);
			FF(ref a4, a, a2, a3, array[1], 12, 3905402710u);
			FF(ref a3, a4, a, a2, array[2], 17, 606105819u);
			FF(ref a2, a3, a4, a, array[3], 22, 3250441966u);
			FF(ref a, a2, a3, a4, array[4], 7, 4118548399u);
			FF(ref a4, a, a2, a3, array[5], 12, 1200080426u);
			FF(ref a3, a4, a, a2, array[6], 17, 2821735955u);
			FF(ref a2, a3, a4, a, array[7], 22, 4249261313u);
			FF(ref a, a2, a3, a4, array[8], 7, 1770035416u);
			FF(ref a4, a, a2, a3, array[9], 12, 2336552879u);
			FF(ref a3, a4, a, a2, array[10], 17, 4294925233u);
			FF(ref a2, a3, a4, a, array[11], 22, 2304563134u);
			FF(ref a, a2, a3, a4, array[12], 7, 1804603682u);
			FF(ref a4, a, a2, a3, array[13], 12, 4254626195u);
			FF(ref a3, a4, a, a2, array[14], 17, 2792965006u);
			FF(ref a2, a3, a4, a, array[15], 22, 1236535329u);
			GG(ref a, a2, a3, a4, array[1], 5, 4129170786u);
			GG(ref a4, a, a2, a3, array[6], 9, 3225465664u);
			GG(ref a3, a4, a, a2, array[11], 14, 643717713u);
			GG(ref a2, a3, a4, a, array[0], 20, 3921069994u);
			GG(ref a, a2, a3, a4, array[5], 5, 3593408605u);
			GG(ref a4, a, a2, a3, array[10], 9, 38016083u);
			GG(ref a3, a4, a, a2, array[15], 14, 3634488961u);
			GG(ref a2, a3, a4, a, array[4], 20, 3889429448u);
			GG(ref a, a2, a3, a4, array[9], 5, 568446438u);
			GG(ref a4, a, a2, a3, array[14], 9, 3275163606u);
			GG(ref a3, a4, a, a2, array[3], 14, 4107603335u);
			GG(ref a2, a3, a4, a, array[8], 20, 1163531501u);
			GG(ref a, a2, a3, a4, array[13], 5, 2850285829u);
			GG(ref a4, a, a2, a3, array[2], 9, 4243563512u);
			GG(ref a3, a4, a, a2, array[7], 14, 1735328473u);
			GG(ref a2, a3, a4, a, array[12], 20, 2368359562u);
			HH(ref a, a2, a3, a4, array[5], 4, 4294588738u);
			HH(ref a4, a, a2, a3, array[8], 11, 2272392833u);
			HH(ref a3, a4, a, a2, array[11], 16, 1839030562u);
			HH(ref a2, a3, a4, a, array[14], 23, 4259657740u);
			HH(ref a, a2, a3, a4, array[1], 4, 2763975236u);
			HH(ref a4, a, a2, a3, array[4], 11, 1272893353u);
			HH(ref a3, a4, a, a2, array[7], 16, 4139469664u);
			HH(ref a2, a3, a4, a, array[10], 23, 3200236656u);
			HH(ref a, a2, a3, a4, array[13], 4, 681279174u);
			HH(ref a4, a, a2, a3, array[0], 11, 3936430074u);
			HH(ref a3, a4, a, a2, array[3], 16, 3572445317u);
			HH(ref a2, a3, a4, a, array[6], 23, 76029189u);
			HH(ref a, a2, a3, a4, array[9], 4, 3654602809u);
			HH(ref a4, a, a2, a3, array[12], 11, 3873151461u);
			HH(ref a3, a4, a, a2, array[15], 16, 530742520u);
			HH(ref a2, a3, a4, a, array[2], 23, 3299628645u);
			II(ref a, a2, a3, a4, array[0], 6, 4096336452u);
			II(ref a4, a, a2, a3, array[7], 10, 1126891415u);
			II(ref a3, a4, a, a2, array[14], 15, 2878612391u);
			II(ref a2, a3, a4, a, array[5], 21, 4237533241u);
			II(ref a, a2, a3, a4, array[12], 6, 1700485571u);
			II(ref a4, a, a2, a3, array[3], 10, 2399980690u);
			II(ref a3, a4, a, a2, array[10], 15, 4293915773u);
			II(ref a2, a3, a4, a, array[1], 21, 2240044497u);
			II(ref a, a2, a3, a4, array[8], 6, 1873313359u);
			II(ref a4, a, a2, a3, array[15], 10, 4264355552u);
			II(ref a3, a4, a, a2, array[6], 15, 2734768916u);
			II(ref a2, a3, a4, a, array[13], 21, 1309151649u);
			II(ref a, a2, a3, a4, array[4], 6, 4149444226u);
			II(ref a4, a, a2, a3, array[11], 10, 3174756917u);
			II(ref a3, a4, a, a2, array[2], 15, 718787259u);
			II(ref a2, a3, a4, a, array[9], 21, 3951481745u);
			state[0] += a;
			state[1] += a2;
			state[2] += a3;
			state[3] += a4;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 0u;
			}
		}

		private static void Encode(byte[] output, int outputOffset, uint[] input, int inputOffset, int count)
		{
			int num = outputOffset + count;
			int num2 = inputOffset;
			for (int i = outputOffset; i < num; i += 4)
			{
				output[i] = (byte)(input[num2] & 0xFF);
				output[i + 1] = (byte)((input[num2] >> 8) & 0xFF);
				output[i + 2] = (byte)((input[num2] >> 16) & 0xFF);
				output[i + 3] = (byte)((input[num2] >> 24) & 0xFF);
				num2++;
			}
		}

		private static void Decode(uint[] output, int outputOffset, byte[] input, int inputOffset, int count)
		{
			int num = inputOffset + count;
			int num2 = outputOffset;
			for (int i = inputOffset; i < num; i += 4)
			{
				output[num2] = (uint)(input[i] | (input[i + 1] << 8) | (input[i + 2] << 16) | (input[i + 3] << 24));
				num2++;
			}
		}

		public void Clear()
		{
			Dispose(true);
		}

		public byte[] ComputeHash(byte[] buffer)
		{
			return ComputeHash(buffer, 0, buffer.Length);
		}

		public byte[] ComputeHash(byte[] buffer, int offset, int count)
		{
			Initialize();
			HashCore(buffer, offset, count);
			HashValue = HashFinal();
			return (byte[])HashValue.Clone();
		}

		public byte[] ComputeHash(Stream inputStream)
		{
			Initialize();
			byte[] input = new byte[4096];
			int num;
			while (0 < (num = inputStream.Read(input, 0, 4096)))
			{
				HashCore(input, 0, num);
			}
			HashValue = HashFinal();
			return (byte[])HashValue.Clone();
		}

		public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			if (inputBuffer == null)
			{
				throw new ArgumentNullException("inputBuffer");
			}
			if (inputOffset < 0)
			{
				throw new ArgumentOutOfRangeException("inputOffset");
			}
			if (inputCount < 0 || inputCount > inputBuffer.Length)
			{
				throw new ArgumentException("inputCount");
			}
			if (inputBuffer.Length - inputCount < inputOffset)
			{
				throw new ArgumentOutOfRangeException("inputOffset");
			}
			if (State == 0)
			{
				Initialize();
				State = 1;
			}
			HashCore(inputBuffer, inputOffset, inputCount);
			if (inputBuffer != outputBuffer || inputOffset != outputOffset)
			{
				Buffer.BlockCopy(inputBuffer, inputOffset, outputBuffer, outputOffset, inputCount);
			}
			return inputCount;
		}

		public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			if (inputBuffer == null)
			{
				throw new ArgumentNullException("inputBuffer");
			}
			if (inputOffset < 0)
			{
				throw new ArgumentOutOfRangeException("inputOffset");
			}
			if (inputCount < 0 || inputCount > inputBuffer.Length)
			{
				throw new ArgumentException("inputCount");
			}
			if (inputBuffer.Length - inputCount < inputOffset)
			{
				throw new ArgumentOutOfRangeException("inputOffset");
			}
			if (State == 0)
			{
				Initialize();
			}
			HashCore(inputBuffer, inputOffset, inputCount);
			HashValue = HashFinal();
			byte[] array = new byte[inputCount];
			Buffer.BlockCopy(inputBuffer, inputOffset, array, 0, inputCount);
			State = 0;
			return array;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposing)
			{
				Initialize();
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}
