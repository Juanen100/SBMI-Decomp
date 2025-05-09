public class SoaringObjectBase
{
	public enum IsType
	{
		Int = 0,
		Float = 1,
		String = 2,
		Array = 3,
		Dictionary = 4,
		Object = 5,
		Module = 6,
		Management = 7,
		Boolean = 8,
		Null = 9
	}

	protected IsType mType;

	public IsType Type
	{
		get
		{
			return mType;
		}
	}

	public SoaringObjectBase(IsType t)
	{
		mType = t;
	}

	public virtual string ToJsonString()
	{
		return ToString();
	}
}
