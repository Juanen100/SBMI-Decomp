using System;
using System.Collections.Generic;

namespace DeltaDNA.Ads
{
	internal interface ISmartAdsManager
	{
		void RegisterForAds(Dictionary<string, object> config, bool userConsent, bool ageRestricted);

		bool IsInterstitialAdAllowed(Engagement engagement, bool checkTime);

		bool IsRewardedAdAllowed(Engagement engagement, bool checkTime);

		long TimeUntilRewardedAdAllowed(Engagement engagement);

		bool HasLoadedInterstitialAd();

		bool HasLoadedRewardedAd();

		void ShowInterstitialAd(Engagement engagement);

		void ShowRewardedAd(Engagement engagement);

		DateTime? GetLastShown(string decisionPoint);

		long GetSessionCount(string decisionPoint);

		long GetDailyCount(string decisionPoint);

		void OnPause();

		void OnResume();

		void OnDestroy();

		void OnNewSession();
	}
}
