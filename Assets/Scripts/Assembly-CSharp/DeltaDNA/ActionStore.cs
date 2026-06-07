using System.Collections.Generic;

namespace DeltaDNA
{
	internal class ActionStore
	{
		private static object LOCK;

		private readonly string location;

		private byte[] salt;

		internal ActionStore(string location)
		{
		}

		internal virtual Dictionary<string, object> Get(EventTrigger trigger)
		{
			return null;
		}

		internal virtual void Put(EventTrigger trigger, Dictionary<string, object> action)
		{
		}

		internal virtual void Remove(EventTrigger trigger)
		{
		}

		internal virtual void Clear()
		{
		}

		private void InitialiseSalt()
		{
		}

		private static byte[] GeneratedSaltedHash(byte[] text, byte[] salt)
		{
			return null;
		}
	}
}
