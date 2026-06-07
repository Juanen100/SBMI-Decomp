using System;
using System.Collections.Generic;
using UnityEngine;

public class UpsightManager : MonoBehaviour
{
	public const string GameObjectName = "UpsightManager";

	private static bool initialized;

	private bool _destroyed;

	public static event Action sessionDidStartEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action sessionDidResumeEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action userSessionDidStartEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action userSessionDidResumeEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<List<string>> managedVariablesDidSynchronizeEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string, UpsightContentAttributes> onBillboardAppearEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string> onBillboardDismissEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<UpsightReward> billboardDidReceiveRewardEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<UpsightPurchase> billboardDidReceivePurchaseEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<UpsightData> billboardDidReceiveDataEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string, Dictionary<string, string>> onContentAvailableEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string, Dictionary<string, string>> onContentNotAvailableEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string> onPartnerInitializedEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static void init()
	{
	}

	private void Awake()
	{
	}

	private void sessionDidStart()
	{
	}

	private void sessionDidResume()
	{
	}

	private void userSessionDidStart()
	{
	}

	private void userSessionDidResume()
	{
	}

	private void managedVariablesDidSynchronize(string json)
	{
	}

	private void onBillboardAppear(string json)
	{
	}

	private void onBillboardDismiss(string scope)
	{
	}

	private void billboardDidReceiveReward(string json)
	{
	}

	private void billboardDidReceivePurchase(string json)
	{
	}

	private void billboardDidReceiveData(string json)
	{
	}

	private void onContentAvailable(string json)
	{
	}

	private void onContentNotAvailable(string json)
	{
	}

	private void onPartnerInitialized(string partnerName)
	{
	}

	private bool parseContentJson(string json, out string scope, out Dictionary<string, string> data)
	{
		scope = null;
		data = null;
		return false;
	}

	private void OnApplicationPause(bool paused)
	{
	}
}
