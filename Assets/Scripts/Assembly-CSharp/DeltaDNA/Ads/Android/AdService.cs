using System;
using System.Collections.Generic;

namespace DeltaDNA.Ads.Android
{
	internal class AdService : ISmartAdsManager
	{
		internal AdService(SmartAds ads, string sdkVersion)
		{
		}

		public void RegisterForAds(Dictionary<string, object> config, bool userConsent, bool ageRestricted)
		{
		}

		public bool IsInterstitialAdAllowed(Engagement engagement, bool checkTime)
		{
			return false;
		}

		public bool IsRewardedAdAllowed(Engagement engagement, bool checkTime)
		{
			return false;
		}

		public long TimeUntilRewardedAdAllowed(Engagement engagement)
		{
			return 0L;
		}

		public bool HasLoadedInterstitialAd()
		{
			return false;
		}

		public bool HasLoadedRewardedAd()
		{
			return false;
		}

		public void ShowInterstitialAd(Engagement engagement)
		{
		}

		public void ShowRewardedAd(Engagement engagement)
		{
		}

		public DateTime? GetLastShown(string decisionPoint)
		{
			return null;
		}

		public long GetSessionCount(string decisionPoint)
		{
			return 0L;
		}

		public long GetDailyCount(string decisionPoint)
		{
			return 0L;
		}

		public void OnPause()
		{
		}

		public void OnResume()
		{
		}

		public void OnDestroy()
		{
		}

		public void OnNewSession()
		{
		}
	}
}
