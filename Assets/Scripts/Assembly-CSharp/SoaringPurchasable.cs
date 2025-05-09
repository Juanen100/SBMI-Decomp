public class SoaringPurchasable : SoaringObjectBase
{
	private string mProductID;

	private string mAlias;

	private string mPriceTier;

	private string mScreenshot;

	private string mDisplayName;

	private int mAmount;

	private string mDescription;

	private int mResourceType;

	private int mUSDPrice;

	private string mTexture;

	public string ProductID
	{
		get
		{
			return mProductID;
		}
	}

	public string Alias
	{
		get
		{
			return mAlias;
		}
	}

	public string PriceTier
	{
		get
		{
			return mPriceTier;
		}
	}

	public string Screenshot
	{
		get
		{
			return mScreenshot;
		}
	}

	public string DisplayName
	{
		get
		{
			return mDisplayName;
		}
	}

	public int Amount
	{
		get
		{
			return mAmount;
		}
	}

	public string Description
	{
		get
		{
			return mDescription;
		}
	}

	public int ResourceType
	{
		get
		{
			return mResourceType;
		}
	}

	public int USDPrice
	{
		get
		{
			return mUSDPrice;
		}
	}

	public string Texture
	{
		get
		{
			return mTexture;
		}
	}

	public SoaringPurchasable(SoaringDictionary data)
		: base(IsType.Object)
	{
		mProductID = data.soaringValue("productId");
		mAlias = data.soaringValue("alias");
		mPriceTier = data.soaringValue("priceTier");
		mScreenshot = data.soaringValue("screenshotName");
		mDisplayName = data.soaringValue("displayName");
		mAmount = data.soaringValue("amount");
		mDescription = data.soaringValue("description");
		mTexture = data.soaringValue("screenshotName");
		mUSDPrice = data.soaringValue("usdPrice");
		string s = data.soaringValue("resourceType");
		mResourceType = -1;
		int.TryParse(s, out mResourceType);
		if (mResourceType < 0)
		{
			SoaringDebug.Log("SoaringPurchasable: failed to parse resourceType as integer.");
		}
	}
}
