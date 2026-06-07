using System;
using System.Collections.Generic;

namespace DeltaDNA
{
	public class EngageFactory
	{
		private readonly DDNABase ddna;

		private readonly SmartAds smartads;

		internal EngageFactory(DDNABase ddna, SmartAds smartads)
		{
		}

		public void RequestGameParameters(string decisionPoint, Action<Dictionary<string, object>> callback)
		{
		}

		public void RequestGameParameters(string decisionPoint, Params parameters, Action<Dictionary<string, object>> callback)
		{
		}

		public void RequestImageMessage(string decisionPoint, Action<ImageMessage> callback)
		{
		}

		public void RequestImageMessage(string decisionPoint, Params parameters, Action<ImageMessage> callback)
		{
		}

		public void RequestInterstitialAd(string decisionPoint, Action<InterstitialAd> callback)
		{
		}

		public void RequestInterstitialAd(string decisionPoint, Params parameters, Action<InterstitialAd> callback)
		{
		}

		public void RequestRewardedAd(string decisionPoint, Action<RewardedAd> callback)
		{
		}

		public void RequestRewardedAd(string decisionPoint, Params parameters, Action<RewardedAd> callback)
		{
		}

		protected static Engagement BuildEngagement(string decisionPoint, Params parameters)
		{
			return null;
		}
	}
}
