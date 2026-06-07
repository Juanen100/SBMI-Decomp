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
			return null;
		}
	}

	public string ProductID
	{
		get
		{
			return null;
		}
	}

	public string Datetime
	{
		get
		{
			return null;
		}
	}

	public string UpdateDatetime
	{
		get
		{
			return null;
		}
	}

	public bool Gift
	{
		get
		{
			return false;
		}
	}

	public bool Valid
	{
		get
		{
			return false;
		}
	}

	public int Amount
	{
		get
		{
			return 0;
		}
	}

	public int ResourceType
	{
		get
		{
			return 0;
		}
	}

	public SoaringPurchase(SoaringDictionary data, SoaringPurchasable purchasable)
		: base(default(IsType))
	{
	}
}
