public class SoaringContext : SoaringDictionary
{
	private const string kDefaultContextName = "_def";

	private string mContextName = "_def";

	private SoaringDelegate mMainResponder;

	private SoaringContextDelegate mContextResponder;

	public string Name
	{
		get
		{
			return mContextName;
		}
		set
		{
			mContextName = value;
		}
	}

	public SoaringDelegate Responder
	{
		get
		{
			return mMainResponder;
		}
		set
		{
			mMainResponder = value;
		}
	}

	public SoaringContextDelegate ContextResponder
	{
		get
		{
			return mContextResponder;
		}
		set
		{
			mContextResponder = value;
		}
	}

	public static implicit operator SoaringContext(SoaringDelegate b)
	{
		SoaringContext soaringContext = new SoaringContext();
		if (b != null)
		{
			soaringContext.Responder = b;
		}
		return soaringContext;
	}

	public static implicit operator SoaringContext(string b)
	{
		SoaringContext soaringContext = new SoaringContext();
		if (b != null)
		{
			soaringContext.mContextName = b;
		}
		return soaringContext;
	}

	public static implicit operator string(SoaringContext b)
	{
		string result = null;
		if (b != null)
		{
			result = b.mContextName;
		}
		return result;
	}
}
