public class SoaringNullValue : SoaringValue
{
	public SoaringNullValue()
		: base(0)
	{
	}

	public override string ToString()
	{
		return null;
	}

	public override string ToJsonString()
	{
		return null;
	}
}
