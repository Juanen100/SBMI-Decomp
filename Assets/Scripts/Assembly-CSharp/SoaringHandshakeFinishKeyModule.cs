internal class SoaringHandshakeFinishKeyModule : SoaringModule
{
	public override string ModuleName()
	{
		return null;
	}

	public override int ModuleChannel()
	{
		return 0;
	}

	public override bool ShouldEncryptCall()
	{
		return false;
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
	}

	public override void HandleDelegateCallback(SoaringModuleData data)
	{
	}
}
