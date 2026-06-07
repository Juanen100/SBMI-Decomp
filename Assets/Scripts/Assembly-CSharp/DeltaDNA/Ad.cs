using System;
using System.Collections.Generic;

namespace DeltaDNA
{
	public abstract class Ad
	{
		protected Engagement engagement;

		public string DecisionPoint
		{
			get
			{
				return null;
			}
		}

		public Engagement Engagement
		{
			get
			{
				return null;
			}
		}

		public Dictionary<string, object> EngageParams
		{
			get
			{
				return null;
			}
		}

		public DateTime? LastShown
		{
			get
			{
				return null;
			}
		}

		public long AdShowWaitSecs
		{
			get
			{
				return 0L;
			}
		}

		public long SessionCount
		{
			get
			{
				return 0L;
			}
		}

		public long SessionLimit
		{
			get
			{
				return 0L;
			}
		}

		public long DailyCount
		{
			get
			{
				return 0L;
			}
		}

		public long DailyLimit
		{
			get
			{
				return 0L;
			}
		}

		protected Ad(Engagement engagement)
		{
		}

		public abstract bool IsReady();

		public abstract void Show();
	}
}
