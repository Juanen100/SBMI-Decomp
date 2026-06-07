using System;

public class SoaringEncryption : SoaringObjectBase
{
	public class RC4
	{
		public const int RC4_Variable = -1;

		public const int RC4_40bit = 5;

		public const int RC4_128bit = 16;

		public const int RC4_256bit = 32;

		public const int RC4_512bit = 64;

		public static int[] mKey;

		public static int[] mBox;

		private static void AllocateBuffers(int itteration_length)
		{
		}

		public static byte[] Encrypt(byte[] pwd, byte[] data, int key_length = -1, int itterations = 256)
		{
			return null;
		}

		public static byte[] EncryptString(byte[] pwd, string data, int key_length = -1, int itterations = 256)
		{
			return null;
		}

		public static byte[] Decrypt(byte[] pwd, byte[] data, int bit_length = 32)
		{
			return null;
		}
	}

	private static byte[] EncryptionKey;

	private static string EncryptionSID;

	private int mEncryptionBits;

	private DateTime mKeyDateStamp;

	private DateTime mEncrytionKeyTime;

	private int mMaxTimeForKeys;

	public static string SID
	{
		get
		{
			return null;
		}
	}

	public SoaringEncryption(string cipher, string digest)
		: base(default(IsType))
	{
	}

	public bool HasExpired()
	{
		return false;
	}

	public void SetEncryptionKey(byte[] key)
	{
	}

	public void SetSID(string sid)
	{
	}

	public static bool IsEncryptionAvailable()
	{
		return false;
	}

	public byte[] Encrypt(byte[] data)
	{
		return null;
	}

	public byte[] Encrypt(string data)
	{
		return null;
	}

	public byte[] Decrypt(byte[] data)
	{
		return null;
	}

	public void StartUsingEncryption()
	{
	}
}
