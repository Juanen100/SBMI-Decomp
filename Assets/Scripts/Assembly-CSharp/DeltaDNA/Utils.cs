using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace DeltaDNA
{
	public static class Utils
	{
		public static Dictionary<K, V> HashtableToDictionary<K, V>(Hashtable table)
		{
			return null;
		}

		public static Dictionary<K, V> HashtableToDictionary<K, V>(Dictionary<K, V> dictionary)
		{
			return null;
		}

		public static byte[] ComputeMD5Hash(byte[] buffer)
		{
			return null;
		}

		public static bool IsDirectoryWritable(string path)
		{
			return false;
		}

		public static bool FileExists(string path)
		{
			return false;
		}

		public static bool DirectoryExists(string path)
		{
			return false;
		}

		public static void CreateDirectory(string path)
		{
		}

		public static Stream CreateStream(string path)
		{
			return null;
		}

		public static Stream OpenStream(string path)
		{
			return null;
		}

		public static string FixURL(string url)
		{
			return null;
		}

		public static T GetOrDefault<T, K>(this IDictionary<K, object> dict, K key, T def)
		{
			return default(T);
		}
	}
}
