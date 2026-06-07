using System;
using System.Collections.Generic;
using DeltaDNA.Ads;

namespace DeltaDNA
{
	public class SmartAds : Singleton<SmartAds>
	{
		private ISmartAdsManager manager;

		private ConcurrentQueue<Action> actions;

		private EngageCache engageCache;

		internal event Action<string> OnRewardedAdOpenedWithDecisionPoint
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action OnDidRegisterForInterstitialAds
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action<string> OnDidFailToRegisterForInterstitialAds
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action OnDidRegisterForRewardedAds
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action<string> OnDidFailToRegisterForRewardedAds
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action OnInterstitialAdOpened
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action<string> OnInterstitialAdFailedToOpen
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action OnInterstitialAdClosed
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action OnRewardedAdLoaded
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action OnRewardedAdOpened
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action<string> OnRewardedAdFailedToOpen
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action<bool> OnRewardedAdClosed
		{
			add
			{
			}
			remove
			{
			}
		}

		internal SmartAds()
		{
		}

		internal SmartAds Config(EngageCache engageCache)
		{
			return null;
		}

		internal bool IsInterstitialAdAllowed(Engagement engagement, bool checkTime)
		{
			return false;
		}

		internal bool IsRewardedAdAllowed(Engagement engagement, bool checkTime)
		{
			return false;
		}

		internal long TimeUntilRewardedAdAllowed(Engagement engagement)
		{
			return 0L;
		}

		internal bool HasLoadedInterstitialAd()
		{
			return false;
		}

		internal bool HasLoadedRewardedAd()
		{
			return false;
		}

		internal void ShowInterstitialAd(Engagement engagement)
		{
		}

		internal void ShowRewardedAd(Engagement engagement)
		{
		}

		internal DateTime? GetLastShown(string decisionPoint)
		{
			return null;
		}

		internal long GetSessionCount(string decisionPoint)
		{
			return 0L;
		}

		internal long GetDailyCount(string decisionPoint)
		{
			return 0L;
		}

		internal void DidRegisterForInterstitialAds()
		{
		}

		internal void DidFailToRegisterForInterstitialAds(string reason)
		{
		}

		internal void DidOpenInterstitialAd()
		{
		}

		internal void DidFailToOpenInterstitialAd(string reason)
		{
		}

		internal void DidCloseInterstitialAd()
		{
		}

		internal void DidRegisterForRewardedAds()
		{
		}

		internal void DidFailToRegisterForRewardedAds(string reason)
		{
		}

		internal void DidLoadRewardedAd()
		{
		}

		internal void DidOpenRewardedAd(string decisionPoint)
		{
		}

		internal void DidFailToOpenRewardedAd(string reason)
		{
		}

		internal void DidCloseRewardedAd(string rewardJSON)
		{
		}

		internal void RecordEvent(string message)
		{
		}

		private void Update()
		{
		}

		private void OnApplicationPause(bool pauseStatus)
		{
		}

		public override void OnDestroy()
		{
		}

		private void CreateManager()
		{
		}

		internal void RegisterForAdsInternal(Dictionary<string, object> config)
		{
		}
	}
}
