public class UpsightPurchase
{
	public string productIdentifier { get; private set; }

	public int quantity { get; private set; }

	public string billboardScope { get; private set; }

	public static UpsightPurchase purchaseFromJson(string json)
	{
		return null;
	}

	protected void populateFromJson(string json)
	{
	}

	public override string ToString()
	{
		return null;
	}
}
