using System;
using System.Collections;
using System.Diagnostics;

namespace DeltaDNA
{
	public class RewardedAd : Ad
	{
		private bool waitingToLoad;

		public string RewardType
		{
			get
			{
				return null;
			}
		}

		public long RewardAmount
		{
			get
			{
				return 0L;
			}
		}

		public event Action<RewardedAd> OnRewardedAdLoaded
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action<RewardedAd> OnRewardedAdExpired
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action<RewardedAd> OnRewardedAdOpened
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action<RewardedAd, string> OnRewardedAdFailedToOpen
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action<RewardedAd, bool> OnRewardedAdClosed
		{
			add
			{
			}
			remove
			{
			}
		}

		private RewardedAd(Engagement engagement)
			: base(null)
		{
		}

		public static RewardedAd Create()
		{
			return null;
		}

		public static RewardedAd Create(Engagement engagement)
		{
			return null;
		}

		internal static RewardedAd CreateUnchecked(Engagement engagement)
		{
			return null;
		}

		public override bool IsReady()
		{
			return false;
		}

		public override void Show()
		{
		}

		private void NotifyOnLoaded()
		{
		}

		[DebuggerHidden]
		private IEnumerator NotifyOnLoadedDelayable(float waitFor)
		{
			return null;
		}

		private void NotifyOnOpened(string decisionPoint)
		{
		}

		private void OnRewaredAdOpenedHandler()
		{
		}

		private void OnRewardedAdFailedToOpenHandler(string reason)
		{
		}

		private void OnRewardedAdClosedHandler(bool reward)
		{
		}
	}
}
