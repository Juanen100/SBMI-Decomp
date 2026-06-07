using UnityEngine;

public class GoogleIAB
{
	private static AndroidJavaObject _plugin;

	static GoogleIAB()
	{
	}

	public static void enableLogging(bool shouldEnable)
	{
	}

	public static void setAutoVerifySignatures(bool shouldVerify)
	{
	}

	public static void init(string publicKey)
	{
	}

	public static void unbindService()
	{
	}

	public static bool areSubscriptionsSupported()
	{
		return false;
	}

	public static void queryInventory(string[] skus)
	{
	}

	public static void purchaseProduct(string sku)
	{
	}

	public static void purchaseProduct(string sku, string developerPayload)
	{
	}

	public static void consumeProduct(string sku)
	{
	}

	public static void consumeProducts(string[] skus)
	{
	}
}
