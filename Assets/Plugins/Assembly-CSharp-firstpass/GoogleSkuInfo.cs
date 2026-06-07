using System.Collections.Generic;

public class GoogleSkuInfo
{
	public string title { get; private set; }

	public string price { get; private set; }

	public string type { get; private set; }

	public string description { get; private set; }

	public string productId { get; private set; }

	public GoogleSkuInfo(Dictionary<string, object> dict)
	{
	}

	public static List<GoogleSkuInfo> fromList(List<object> items)
	{
		return null;
	}

	public override string ToString()
	{
		return null;
	}
}
