public class SoaringEvent : SoaringObjectBase
{
	public enum SoaringEventActionType
	{
		Custom = 0,
		DisplayBanner = 1,
		HardCurrency = 2,
		SoftCurrency = 3,
		Item = 4
	}

	public class SoaringEventAction
	{
		public string Key;

		public string Value;

		public int Quantity;

		public bool Display;

		public bool AutoHandle;

		public int Priority;

		public SoaringDictionary Custom;

		public SoaringEventActionType Type;
	}

	public enum Equivelency
	{
		equal = 0,
		greaterThen = 1,
		greaterThenEquals = 2,
		lessThen = 3,
		lessThenEquals = 4
	}

	public class SoaringEventRequirements
	{
		public string Key;

		public string Value;

		public Equivelency Sign;

		public SoaringDictionary Custom;
	}

	public string Name;

	public SoaringEventAction[] Actions;

	public SoaringEventRequirements[] Requires;

	public bool AutoHandled;

	public SoaringEvent(SoaringDictionary ev)
		: base(default(IsType))
	{
	}

	public bool HasDisplayBannerEvent()
	{
		return false;
	}

	public bool HasDisplayBannerEvent(ref SoaringEventAction action)
	{
		return false;
	}
}
