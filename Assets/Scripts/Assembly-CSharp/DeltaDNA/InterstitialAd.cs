using System;

namespace DeltaDNA
{
	public class InterstitialAd : Ad
	{
		public event Action<InterstitialAd> OnInterstitialAdOpened
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action<InterstitialAd, string> OnInterstitialAdFailedToOpen
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action<InterstitialAd> OnInterstitialAdClosed
		{
			add
			{
			}
			remove
			{
			}
		}

		private InterstitialAd(Engagement engagement)
			: base(null)
		{
		}

		public static InterstitialAd Create()
		{
			return null;
		}

		public static InterstitialAd Create(Engagement engagement)
		{
			return null;
		}

		internal static InterstitialAd CreateUnchecked(Engagement engagement)
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

		private void OnInterstitialAdOpenedHandler()
		{
		}

		private void OnInterstitialAdFailedToOpenHandler(string reason)
		{
		}

		private void OnInterstitialAdClosedHandler()
		{
		}
	}
}
