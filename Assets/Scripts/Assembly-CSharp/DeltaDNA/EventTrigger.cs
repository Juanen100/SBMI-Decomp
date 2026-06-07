using System;
using System.Collections.Generic;

namespace DeltaDNA
{
	internal class EventTrigger : IComparable<EventTrigger>
	{
		private readonly DDNABase ddna;

		private readonly int index;

		private readonly string eventName;

		private readonly Dictionary<string, object> response;

		private readonly long priority;

		private readonly long limit;

		private readonly Dictionary<string, object>[] condition;

		private readonly long campaignId;

		private readonly long variantId;

		private readonly string campaignName;

		private readonly string variantName;

		private int runs;

		private static readonly Dictionary<string, Func<bool, bool, bool>> BOOLS;

		private static readonly Dictionary<string, Func<long, long, bool>> LONGS;

		private static readonly Dictionary<string, Func<double, double, bool>> DOUBLES;

		private static readonly Dictionary<string, Func<string, string, bool>> STRINGS;

		private static readonly Dictionary<string, Func<DateTime, DateTime, bool>> DATES;

		internal EventTrigger(DDNABase ddna, int index, Dictionary<string, object> json)
		{
		}

		internal string GetEventName()
		{
			return null;
		}

		internal virtual string GetAction()
		{
			return null;
		}

		internal virtual Dictionary<string, object> GetResponse()
		{
			return null;
		}

		internal virtual long GetCampaignId()
		{
			return 0L;
		}

		internal virtual bool Evaluate(GameEvent evnt)
		{
			return false;
		}

		public int CompareTo(EventTrigger other)
		{
			return 0;
		}
	}
}
