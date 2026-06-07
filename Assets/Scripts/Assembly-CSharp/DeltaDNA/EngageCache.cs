using System;
using System.Collections.Generic;

namespace DeltaDNA
{
	internal class EngageCache
	{
		private const string TIMES = "times";

		private static object LOCK;

		private readonly Settings settings;

		private readonly string location;

		private readonly IDictionary<string, string> cache;

		private readonly IDictionary<string, DateTime> times;

		internal EngageCache(Settings settings)
		{
		}

		internal void Put(string decisionPoint, string flavour, string data)
		{
		}

		internal string Get(string decisionPoint, string flavour)
		{
			return null;
		}

		internal void Save()
		{
		}

		internal void Clear()
		{
		}

		private void CreateDirectory()
		{
		}

		private static string Key(string decisionPoint, string flavour)
		{
			return null;
		}
	}
}
