using Prime31;
using UnityEngine;

public class IABUIManager : MonoBehaviourGUI
{
	private void OnGUI()
	{
		beginColumn();
		if (GUILayout.Button("Initialize IAB"))
		{
			string publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwqo+nBgOz9uvW/EeHYhTpctG0OdutjmzVbHlQBJWYVgqaDFcliO76afXQ2qnf5EW7H7jlQcF42zizzs1O1vC7cmYN6mbTnWnmOEDHuSyG02tKIVU2pGWBM/VsnIqL4lpmNaM4JKJYNcZ7a9pnBhfiUAqR9pejVAqYqVc5dG091TPIwzE/MjchSac0EFXa49iMuiYfdYjzXye/981t23PDk7IS+weXcthqjSfjwtnxobxktnJ9eSUK7jaBpFPbRIFHXuE/+ZQSYRtMu48myp/CnJd/0A/srnmDetb2ai990A4hQgvP9/HySyw14CHhq87tRCRAzXso5Q09iaU8e538QIDAQAB";
			GoogleIAB.init(publicKey);
		}
		if (GUILayout.Button("Query Inventory"))
		{
			string[] skus = new string[2] { "com.mtvn.sbmi.jellybundle1", "com.mtvn.sbmi.jellybundle2" };
			GoogleIAB.queryInventory(skus);
		}
		if (GUILayout.Button("Are subscriptions supported?"))
		{
			Debug.Log("subscriptions supported: " + GoogleIAB.areSubscriptionsSupported());
		}
		if (GUILayout.Button("Purchase Test Product"))
		{
			GoogleIAB.purchaseProduct("com.mtvn.sbmi.jellybundle1");
		}
		if (GUILayout.Button("Purchase Test Product2"))
		{
			GoogleIAB.purchaseProduct("com.mtvn.sbmi.jellybundle2");
		}
		if (GUILayout.Button("Consume Test Purchase"))
		{
			GoogleIAB.consumeProduct("android.test.purchased");
		}
		if (GUILayout.Button("Test Unavailable Item"))
		{
			GoogleIAB.purchaseProduct("android.test.item_unavailable");
		}
		endColumn(true);
		if (GUILayout.Button("Purchase Real Product"))
		{
			GoogleIAB.purchaseProduct("com.prime31.testproduct", "payload that gets stored and returned");
		}
		if (GUILayout.Button("Purchase Real Subscription"))
		{
			GoogleIAB.purchaseProduct("com.prime31.testsubscription", "subscription payload");
		}
		if (GUILayout.Button("Consume Real Purchase"))
		{
			GoogleIAB.consumeProduct("com.prime31.testproduct");
		}
		if (GUILayout.Button("Enable High Details Logs"))
		{
			GoogleIAB.enableLogging(true);
		}
		if (GUILayout.Button("Consume Multiple Purchases"))
		{
			string[] skus2 = new string[2] { "com.prime31.testproduct", "android.test.purchased" };
			GoogleIAB.consumeProducts(skus2);
		}
		if (GUILayout.Button("loading spongeBob"))
		{
			Application.LoadLevel("Scene0");
		}
		endColumn();
	}
}
