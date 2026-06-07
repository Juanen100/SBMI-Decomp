public class SoaringContext : SoaringDictionary
{
	private const string kDefaultContextName = "_def";

	private string mContextName;

	private SoaringDelegate mMainResponder;

	private SoaringContextDelegate mContextResponder;

	public string Name
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public SoaringDelegate Responder
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public SoaringContextDelegate ContextResponder
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public static implicit operator SoaringContext(SoaringDelegate b)
	{
		return null;
	}

	public static implicit operator SoaringContext(string b)
	{
		return null;
	}

	public static implicit operator string(SoaringContext b)
	{
		return null;
	}
}
