public class SoaringRetrievePurchasesModule : SoaringModule
{
	private static int totalMoneySpent;

	public static int TotalMoneySpent
	{
		get
		{
			return totalMoneySpent;
		}
	}

	public override string ModuleName()
	{
		return "retrieveIapPurchases";
	}

	public override int ModuleChannel()
	{
		return 1;
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		string text = "{\n\"action\" : {\n\"name\":\"" + ModuleName() + "\",\n";
		text = string.Concat(text, "\"authToken\":\"", data.soaringValue("authToken"), "\"\n},");
		text += "\n\"data\" : ";
		callData.removeObjectWithKey("authToken");
		callData.removeObjectWithKey("gameId");
		text += callData.ToJsonString();
		text += "\n}";
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringDictionary.addValue(text, "data");
		PushCorePostDataToQueue(soaringDictionary, ModuleChannel(), context, false);
	}

	public override void HandleDelegateCallback(SoaringModuleData data)
	{
		totalMoneySpent = 0;
		SoaringPurchase[] array = null;
		SoaringDictionary purchasables = SoaringInternal.instance.Purchasables;
		if (data.data != null)
		{
			SoaringArray soaringArray = (SoaringArray)data.data.objectWithKey("purchases");
			if (soaringArray != null)
			{
				int num = soaringArray.count();
				array = new SoaringPurchase[num];
				for (int i = 0; i < num; i++)
				{
					SoaringDictionary soaringDictionary = (SoaringDictionary)soaringArray.objectAtIndex(i);
					string key = soaringDictionary.soaringValue("productId");
					SoaringPurchasable soaringPurchasable = (SoaringPurchasable)purchasables.objectWithKey(key);
					array[i] = new SoaringPurchase(soaringDictionary, soaringPurchasable);
					totalMoneySpent += soaringPurchasable.USDPrice;
				}
			}
		}
		Soaring.Delegate.OnRetrievePurchases(data.state, data.error, array, data.context);
	}
}
