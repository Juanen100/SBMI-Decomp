public class SoaringObject : SoaringObjectBase
{
	private object mObject;

	public object Object
	{
		get
		{
			return mObject;
		}
	}

	public SoaringObject(object obj)
		: base(IsType.Object)
	{
		mObject = obj;
	}

	public override string ToString()
	{
		string text = null;
		if (mObject != null)
		{
			return mObject.ToString();
		}
		return string.Empty;
	}

	public override string ToJsonString()
	{
		return "\"" + ToString() + "\"";
	}
}
