public class SoaringCoupon : SoaringObjectBase
{
	private string mCoupon;

	private string mReciept;

	public SoaringCoupon(string id, string reciept)
		: base(IsType.Object)
	{
		mCoupon = id;
		mReciept = reciept;
	}

	public override string ToString()
	{
		return mCoupon;
	}

	public override string ToJsonString()
	{
		return "{\n\"coupon\" : \"" + mCoupon + "\",\n\"receipt\" : \"" + mReciept + "\"\n}";
	}
}
