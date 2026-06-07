public class UpsightReward
{
	public string productIdentifier { get; private set; }

	public int quantity { get; private set; }

	public string signatureData { get; private set; }

	public string billboardScope { get; private set; }

	public static UpsightReward rewardFromJson(string json)
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
