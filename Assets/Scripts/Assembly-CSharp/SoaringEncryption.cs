using System;
using UnityEngine;

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
			if (mKey == null)
			{
				mKey = new int[itteration_length];
			}
			else if (mKey.Length != itteration_length)
			{
				mKey = new int[itteration_length];
			}
			if (mBox == null)
			{
				mBox = new int[itteration_length];
			}
			else if (mBox.Length != itteration_length)
			{
				mBox = new int[itteration_length];
			}
		}

		public static byte[] Encrypt(byte[] pwd, byte[] data, int key_length = -1, int itterations = 256)
		{
			if (pwd == null || data == null)
			{
				SoaringDebug.Log("Invalid Encrypt Password Or Key", LogType.Error);
				return null;
			}
			int num = itterations - 1;
			AllocateBuffers(itterations);
			int num2 = pwd.Length;
			if (num2 != -1)
			{
				if (num2 < key_length)
				{
					SoaringDebug.Log("Invalid Key Length: " + key_length + " : " + num2, LogType.Error);
					return null;
				}
				num2 = key_length;
			}
			byte[] array = new byte[data.Length];
			int i;
			for (i = 0; i < itterations; i++)
			{
				mKey[i] = pwd[i % num2];
				mBox[i] = i;
			}
			int num3 = (i = 0);
			for (; i < itterations; i++)
			{
				num3 = (num3 + mBox[i] + mKey[i]) & num;
				int num4 = mBox[i];
				mBox[i] = mBox[num3];
				mBox[num3] = num4;
			}
			int num5 = (num3 = (i = 0));
			for (; i < data.Length; i++)
			{
				num5++;
				num5 %= itterations;
				num3 += mBox[num5];
				num3 %= itterations;
				int num4 = mBox[num5];
				mBox[num5] = mBox[num3];
				mBox[num3] = num4;
				int num6 = mBox[(mBox[num5] + mBox[num3]) & num];
				array[i] = (byte)(data[i] ^ num6);
			}
			return array;
		}

		public static byte[] EncryptString(byte[] pwd, string data, int key_length = -1, int itterations = 256)
		{
			if (pwd == null || data == null)
			{
				SoaringDebug.Log("Invalid Encrypt Password Or Key", LogType.Error);
				return null;
			}
			int num = itterations - 1;
			AllocateBuffers(itterations);
			int num2 = pwd.Length;
			if (num2 != -1)
			{
				if (num2 < key_length)
				{
					SoaringDebug.Log("Invalid Key Length: " + key_length + " : " + num2, LogType.Error);
					return null;
				}
				num2 = key_length;
			}
			byte[] array = new byte[data.Length];
			int i;
			for (i = 0; i < itterations; i++)
			{
				mKey[i] = pwd[i % num2];
				mBox[i] = i;
			}
			int num3 = (i = 0);
			for (; i < itterations; i++)
			{
				num3 = (num3 + mBox[i] + mKey[i]) & num;
				int num4 = mBox[i];
				mBox[i] = mBox[num3];
				mBox[num3] = num4;
			}
			int num5 = (num3 = (i = 0));
			for (; i < data.Length; i++)
			{
				num5++;
				num5 %= itterations;
				num3 += mBox[num5];
				num3 %= itterations;
				int num4 = mBox[num5];
				mBox[num5] = mBox[num3];
				mBox[num3] = num4;
				int num6 = mBox[(mBox[num5] + mBox[num3]) & num];
				array[i] = (byte)(data[i] ^ num6);
			}
			return array;
		}

		public static byte[] Decrypt(byte[] pwd, byte[] data, int bit_length = 32)
		{
			return Encrypt(pwd, data, bit_length);
		}
	}

	private static byte[] EncryptionKey;

	private static string EncryptionSID;

	private int mEncryptionBits;

	private DateTime mKeyDateStamp = new DateTime(1970, 1, 1, 0, 0, 0);

	private DateTime mEncrytionKeyTime = new DateTime(1970, 1, 1, 0, 0, 0);

	private int mMaxTimeForKeys = 280;

	public static string SID
	{
		get
		{
			return EncryptionSID;
		}
	}

	public SoaringEncryption(string cipher, string digest)
		: base(IsType.Object)
	{
		try
		{
			string text = cipher.ToLower();
			if (text.Contains("rc4"))
			{
				mEncryptionBits = -1;
				text = text.Replace("rc4", string.Empty).Replace("-", string.Empty);
				if (!int.TryParse(text, out mEncryptionBits))
				{
					mEncryptionBits = -1;
				}
				if (mEncryptionBits != 0)
				{
					mEncryptionBits /= 8;
				}
			}
		}
		catch
		{
		}
	}

	public bool HasExpired()
	{
		return (DateTime.UtcNow - mKeyDateStamp).TotalSeconds > (double)mMaxTimeForKeys;
	}

	public void SetEncryptionKey(byte[] key)
	{
		EncryptionKey = key;
		if (EncryptionKey != null)
		{
			mEncrytionKeyTime = DateTime.UtcNow;
		}
		else
		{
			mEncrytionKeyTime = new DateTime(1970, 1, 1, 0, 0, 0);
		}
	}

	public void SetSID(string sid)
	{
		EncryptionSID = sid;
	}

	public static bool IsEncryptionAvailable()
	{
		if (!SoaringInternalProperties.SecureCommunication)
		{
			return false;
		}
		return EncryptionKey != null && EncryptionSID != null;
	}

	public byte[] Encrypt(byte[] data)
	{
		return RC4.Encrypt(EncryptionKey, data, mEncryptionBits);
	}

	public byte[] Encrypt(string data)
	{
		return RC4.EncryptString(EncryptionKey, data, mEncryptionBits);
	}

	public byte[] Decrypt(byte[] data)
	{
		return RC4.Decrypt(EncryptionKey, data, mEncryptionBits);
	}

	public void StartUsingEncryption()
	{
		mKeyDateStamp = mEncrytionKeyTime;
	}
}
