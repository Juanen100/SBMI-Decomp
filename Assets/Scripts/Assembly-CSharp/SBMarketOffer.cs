using System.Collections.Generic;

public class SBMarketOffer
{
	public string type;

	public int identity;

	public bool itemLocked;

	public string itemName;

	public string description;

	public string innerOffer;

	public string material;

	public string texture;

	public string buttonTexture;

	public Dictionary<int, int> cost;

	public Dictionary<int, int> data;

	public int width;

	public int height;

	public string resultType;

	public int microEventDID;

	public bool microEventOnly;

	public bool isSaleItem;

	public bool isNewItem;

	public bool isLimitedItem;

	public float salePercent;

	public SBMarketOffer(Dictionary<string, object> offer)
	{
	}
}
