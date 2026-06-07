public class SoaringLookupUserModule : SoaringModule
{
	public override bool ShouldEncryptCall()
	{
		return false;
	}

	public override string ModuleName()
	{
		return null;
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
	}

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
	}
}
