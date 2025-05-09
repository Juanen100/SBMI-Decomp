public class SoaringNullValue : SoaringValue
{
	public SoaringNullValue()
		: base(0)
	{
		mType = IsType.Null;
	}

	public override string ToString()
	{
		return "null";
	}

	public override string ToJsonString()
	{
		return ToString();
	}
}
