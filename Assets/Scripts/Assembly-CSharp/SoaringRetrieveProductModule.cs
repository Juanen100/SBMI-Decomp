public class SoaringRetrieveProductModule : SoaringModule
{
	public override string ModuleName()
	{
		return null;
	}

	public override int ModuleChannel()
	{
		return 0;
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
	}

	public static SoaringPurchasable[] LoadCachedProductData()
	{
		return null;
	}

	public override void HandleDelegateCallback(SoaringModuleData data)
	{
	}
}
