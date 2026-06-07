using System;
using System.Collections.Generic;

public class SBUpsightADManager
{
	private static SBUpsightADManager _instance;

	private string currentContentScope;

	private bool adAvailable;

	private UpsightReward pendingReward;

	private Session session;

	private ulong timeLastViewed;

	private Dictionary<string, object> callData;

	private Action<string, int> watchADDelegate;

	private static string[] scopes;

	public static SBUpsightADManager Instance
	{
		get
		{
			return null;
		}
	}

	public SBUpsightADManager(Session session)
	{
	}

	public bool IsAdAvailable(string scope, string calledFrom = null)
	{
		return false;
	}

	public void ShowUpsightAd(string scope, Dictionary<string, object> callData, Action<string, int> callback)
	{
	}

	private void AdRewardCallback(UpsightReward reward)
	{
	}

	public void SignatureValidated(bool isValid)
	{
	}

	private void ValidateReward(UpsightReward reward)
	{
	}
}
