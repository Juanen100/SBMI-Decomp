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
		: base(IsType.Object)
	{
		Name = ev.soaringValue("event_name");
		AutoHandled = ev.soaringValue("auto");
		SoaringArray soaringArray = (SoaringArray)ev.objectWithKey("actions");
		SoaringArray soaringArray2 = (SoaringArray)ev.objectWithKey("client_requires");
		if (soaringArray2 != null)
		{
			Requires = new SoaringEventRequirements[soaringArray2.count()];
			for (int i = 0; i < soaringArray2.count(); i++)
			{
				SoaringEventRequirements soaringEventRequirements = new SoaringEventRequirements();
				SoaringDictionary soaringDictionary = (SoaringDictionary)soaringArray2.objectAtIndex(i);
				soaringEventRequirements.Key = soaringDictionary.soaringValue("key");
				soaringEventRequirements.Value = soaringDictionary.soaringValue("value");
				soaringEventRequirements.Custom = (SoaringDictionary)soaringDictionary.objectWithKey("custom");
				switch ((string)soaringDictionary.soaringValue("test"))
				{
				case "equals":
					soaringEventRequirements.Sign = Equivelency.equal;
					break;
				case "greaterThenEquals":
					soaringEventRequirements.Sign = Equivelency.greaterThenEquals;
					break;
				case "lessThenEquals":
					soaringEventRequirements.Sign = Equivelency.lessThenEquals;
					break;
				case "greaterThen":
					soaringEventRequirements.Sign = Equivelency.greaterThen;
					break;
				case "lessThen":
					soaringEventRequirements.Sign = Equivelency.lessThen;
					break;
				}
				Requires[i] = soaringEventRequirements;
			}
		}
		int num = 0;
		if (soaringArray != null)
		{
			num = soaringArray.count();
		}
		Actions = new SoaringEventAction[num];
		for (int j = 0; j < num; j++)
		{
			SoaringEventAction soaringEventAction = new SoaringEventAction();
			SoaringDictionary soaringDictionary2 = (SoaringDictionary)soaringArray.objectAtIndex(j);
			soaringEventAction.Key = soaringDictionary2.soaringValue("key");
			soaringEventAction.Value = soaringDictionary2.soaringValue("value");
			soaringEventAction.Quantity = soaringDictionary2.soaringValue("quantity");
			SoaringObjectBase soaringObjectBase = soaringDictionary2.soaringValue("display");
			if (soaringObjectBase != null)
			{
				if (soaringObjectBase.Type == IsType.Dictionary)
				{
					SoaringDictionary soaringDictionary3 = (SoaringDictionary)soaringObjectBase;
					soaringEventAction.Display = soaringDictionary3.soaringValue("display");
					soaringEventAction.Priority = soaringDictionary3.soaringValue("priority");
					soaringEventAction.AutoHandle = soaringDictionary3.soaringValue("auto");
				}
				else
				{
					soaringEventAction.Display = (SoaringValue)soaringObjectBase;
				}
			}
			else
			{
				soaringEventAction.Display = false;
			}
			soaringEventAction.Custom = (SoaringDictionary)soaringDictionary2.objectWithKey("custom");
			soaringEventAction.Type = SoaringEventActionType.Custom;
			switch (soaringEventAction.Key.ToLower())
			{
			case "display_banner":
				soaringEventAction.Type = SoaringEventActionType.DisplayBanner;
				break;
			case "hard_currency":
				soaringEventAction.Type = SoaringEventActionType.HardCurrency;
				break;
			case "soft_currency":
				soaringEventAction.Type = SoaringEventActionType.SoftCurrency;
				break;
			case "item":
				soaringEventAction.Type = SoaringEventActionType.Item;
				break;
			}
			Actions[j] = soaringEventAction;
		}
	}

	public bool HasDisplayBannerEvent()
	{
		SoaringEventAction action = null;
		return HasDisplayBannerEvent(ref action);
	}

	public bool HasDisplayBannerEvent(ref SoaringEventAction action)
	{
		bool result = false;
		if (Actions == null)
		{
			return result;
		}
		int num = Actions.Length;
		for (int i = 0; i < num; i++)
		{
			if (Actions[i].Type == SoaringEventActionType.DisplayBanner && Actions[i].Display)
			{
				action = Actions[i];
				result = true;
				break;
			}
		}
		return result;
	}
}
