using System;
using System.IO;

namespace MTools
{
	public class MCommon
	{
		public delegate void StandardDelegate();

		public delegate void StandardDelegate_Object(object o);

		public delegate void StandardDelegate_String(string s);

		public delegate void StandardDelegate_Key(string s, object o);

		public delegate bool StandardDelegate_Check(object o);

		private MCommon()
		{
		}

		public static string CreateFileHash(string filePath)
		{
			string text = string.Empty;
			if (!File.Exists(filePath))
			{
				return text;
			}
			MBinaryReader mBinaryReader = new MBinaryReader(filePath);
			if (mBinaryReader == null)
			{
				return text;
			}
			if (!mBinaryReader.IsOpen())
			{
				return text;
			}
			long num = mBinaryReader.FileLengthLong();
			long num2 = 0L;
			while (num > 0)
			{
				ulong num3 = 0uL;
				long num4 = 16777210L;
				if (num4 > num)
				{
					num4 = num;
				}
				for (int i = 0; i < num4; i++)
				{
					num3 += mBinaryReader.ReadByte();
					num2++;
				}
				num -= num4;
				byte[] bytes = BitConverter.GetBytes(num3);
				text += Convert.ToBase64String(bytes);
			}
			text = mBinaryReader.FileLengthLong() + ":" + text;
			mBinaryReader.Close();
			mBinaryReader = null;
			return text;
		}

		public static string CreateStringHash(string message)
		{
			string empty = string.Empty;
			if (string.IsNullOrEmpty(message))
			{
				return empty;
			}
			int length = message.Length;
			uint num = 0u;
			for (int i = 0; i < length; i++)
			{
				num += message[i];
			}
			empty = length + ":";
			byte[] bytes = BitConverter.GetBytes(num);
			return empty + Convert.ToBase64String(bytes);
		}

		public static bool ValidateFileHash(string filePath, string hash)
		{
			if (string.IsNullOrEmpty(hash))
			{
				return false;
			}
			string text = CreateFileHash(filePath);
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			int num = text.LastIndexOf(':');
			if (num >= 0)
			{
				text = text.Substring(num, text.Length - num);
			}
			num = hash.LastIndexOf(':');
			if (num >= 0)
			{
				hash = hash.Substring(num, hash.Length - num);
			}
			return text == hash;
		}

		public static bool ValidateStringHash(string message, string existing_hash)
		{
			if (string.IsNullOrEmpty(existing_hash))
			{
				return false;
			}
			string text = CreateStringHash(message);
			if (string.IsNullOrEmpty(message))
			{
				return false;
			}
			return text == existing_hash;
		}
	}
}
