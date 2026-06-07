using UnityEngine;

namespace DeltaDNA.Ads.Android
{
	internal class AdServiceListener : AndroidJavaProxy
	{
		private SmartAds ads;

		internal AdServiceListener(SmartAds ads)
			: base((string)null)
		{
		}

		private void onRegisteredForInterstitialAds()
		{
		}

		private void onFailedToRegisterForInterstitialAds(string reason)
		{
		}

		private void onRegisteredForRewardedAds()
		{
		}

		private void onFailedToRegisterForRewardedAds(string reason)
		{
		}

		private void onInterstitialAdOpened()
		{
		}

		private void onInterstitialAdFailedToOpen(string reason)
		{
		}

		private void onInterstitialAdClosed()
		{
		}

		private void onRewardedAdLoaded()
		{
		}

		private void onRewardedAdOpened(string decisionPoint)
		{
		}

		private void onRewardedAdFailedToOpen(string reason)
		{
		}

		private void onRewardedAdClosed(bool completed)
		{
		}

		private void onRecordEvent(string eventName, string eventParamsJson)
		{
		}
	}
}
