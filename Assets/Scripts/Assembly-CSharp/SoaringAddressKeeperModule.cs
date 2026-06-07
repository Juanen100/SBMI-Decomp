public class SoaringAddressKeeperModule : SoaringModule
{
	public override string ModuleName()
	{
		return null;
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
	}

	public override bool ShouldEncryptCall()
	{
		return false;
	}

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
	}
}
