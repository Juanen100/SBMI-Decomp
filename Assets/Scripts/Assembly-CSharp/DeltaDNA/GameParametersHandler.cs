using System;
using System.Collections.Generic;

namespace DeltaDNA
{
	public class GameParametersHandler : EventActionHandler
	{
		private readonly Action<Dictionary<string, object>> callback;

		public GameParametersHandler(Action<Dictionary<string, object>> callback)
		{
		}

		internal override bool Handle(EventTrigger trigger, ActionStore store)
		{
			return false;
		}

		internal override string Type()
		{
			return null;
		}
	}
}
