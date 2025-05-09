using System.Collections;
using System.Collections.Generic;

namespace DeltaDNA
{
	internal static class Utils
	{
		public static Dictionary<K, V> HashtableToDictionary<K, V>(Hashtable table)
		{
			Dictionary<K, V> dictionary = new Dictionary<K, V>();
			foreach (DictionaryEntry item in table)
			{
				dictionary.Add((K)item.Key, (V)item.Value);
			}
			return dictionary;
		}

		public static Dictionary<K, V> HashtableToDictionary<K, V>(Dictionary<K, V> dictionary)
		{
			return dictionary;
		}
	}
}
