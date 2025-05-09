public class SoaringPurchase : SoaringObjectBase
{
	private SoaringPurchasable mPurchasable;

	private string mProductID;

	private string mDatetime;

	private string mUpdateDatetime;

	private bool mGift;

	private bool mValid;

	private int mAmount;

	private int mResourceType;

	public SoaringPurchasable Purchasable
	{
		get
		{
			return mPurchasable;
		}
	}

	public string ProductID
	{
		get
		{
			return mProductID;
		}
	}

	public string Datetime
	{
		get
		{
			return mDatetime;
		}
	}

	public string UpdateDatetime
	{
		get
		{
			return mUpdateDatetime;
		}
	}

	public bool Gift
	{
		get
		{
			return mGift;
		}
	}

	public bool Valid
	{
		get
		{
			return mValid;
		}
	}

	public int Amount
	{
		get
		{
			return mAmount;
		}
	}

	public int ResourceType
	{
		get
		{
			return mResourceType;
		}
	}

	public SoaringPurchase(SoaringDictionary data, SoaringPurchasable purchasable)
		: base(IsType.Object)
	{
		mPurchasable = purchasable;
		mProductID = data.soaringValue("productId");
		mDatetime = data.soaringValue("datetime");
		mValid = data.soaringValue("valid");
		mGift = data.soaringValue("gift");
		mUpdateDatetime = data.soaringValue("updateDatetime");
		mAmount = data.soaringValue("amount");
		string s = data.soaringValue("resourceType");
		mResourceType = -1;
		int.TryParse(s, out mResourceType);
		if (mResourceType < 0)
		{
			SoaringDebug.Log("SoaringPurchase: failed to parse resourceType as integer.");
		}
	}
}
