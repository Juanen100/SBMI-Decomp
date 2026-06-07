using System;
using System.Collections.Generic;
using Prime31;

public class GoogleIABManager : AbstractManager
{
	public static event Action billingSupportedEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string> billingNotSupportedEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<List<GooglePurchase>, List<GoogleSkuInfo>> queryInventorySucceededEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string> queryInventoryFailedEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string, string> purchaseCompleteAwaitingVerificationEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<GooglePurchase> purchaseSucceededEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string> purchaseFailedEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<GooglePurchase> consumePurchaseSucceededEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<string> consumePurchaseFailedEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	static GoogleIABManager()
	{
	}

	public void billingSupported(string empty)
	{
	}

	public void billingNotSupported(string error)
	{
	}

	public void queryInventorySucceeded(string json)
	{
	}

	public void queryInventoryFailed(string error)
	{
	}

	public void purchaseCompleteAwaitingVerification(string json)
	{
	}

	public void purchaseSucceeded(string json)
	{
	}

	public void purchaseFailed(string error)
	{
	}

	public void consumePurchaseSucceeded(string json)
	{
	}

	public void consumePurchaseFailed(string error)
	{
	}
}
