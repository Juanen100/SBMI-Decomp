public class SoaringRetrievePurchasesModule : SoaringModule
{
	private static int totalMoneySpent;

	public static int TotalMoneySpent
	{
		get
		{
			return 0;
		}
	}

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

	public override void HandleDelegateCallback(SoaringModuleData data)
	{
	}
}
