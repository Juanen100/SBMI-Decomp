using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DeltaDNA
{
	public sealed class EventAction
	{
		internal static readonly ReadOnlyCollection<EventTrigger> EMPTY_TRIGGERS;

		private readonly GameEvent evnt;

		private readonly ReadOnlyCollection<EventTrigger> triggers;

		private readonly Settings settings;

		private readonly ActionStore store;

		private readonly List<EventActionHandler> handlers;

		internal EventAction(GameEvent evnt, ReadOnlyCollection<EventTrigger> triggers, ActionStore store, Settings settings)
		{
		}

		public EventAction Add(EventActionHandler handler)
		{
			return null;
		}

		public void Run()
		{
		}

		internal static EventAction CreateEmpty(GameEvent evnt)
		{
			return null;
		}
	}
}
