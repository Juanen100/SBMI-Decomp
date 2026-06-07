using System.Collections.Generic;

namespace DeltaDNA
{
	public class GameEvent<T> where T : GameEvent<T>
	{
		internal readonly Params parameters;

		public string Name { get; private set; }

		public GameEvent(string name)
		{
		}

		public T AddParam(string key, object value)
		{
			return null;
		}

		public Dictionary<string, object> AsDictionary()
		{
			return null;
		}
	}
	public class GameEvent : GameEvent<GameEvent>
	{
		public GameEvent(string name)
			: base((string)null)
		{
		}
	}
}
