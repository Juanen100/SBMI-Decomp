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
			return null;
		}
	}

	public string Alias
	{
		get
		{
			return null;
		}
	}

	public string PriceTier
	{
		get
		{
			return null;
		}
	}

	public string Screenshot
	{
		get
		{
			return null;
		}
	}

	public string DisplayName
	{
		get
		{
			return null;
		}
	}

	public int Amount
	{
		get
		{
			return 0;
		}
	}

	public string Description
	{
		get
		{
			return null;
		}
	}

	public int ResourceType
	{
		get
		{
			return 0;
		}
	}

	public int USDPrice
	{
		get
		{
			return 0;
		}
	}

	public string Texture
	{
		get
		{
			return null;
		}
	}

	public SoaringPurchasable(SoaringDictionary data)
		: base(default(IsType))
	{
	}
}
