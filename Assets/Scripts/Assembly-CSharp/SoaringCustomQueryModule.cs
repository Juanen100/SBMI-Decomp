public class SoaringCustomQueryModule : SoaringModule
{
	public virtual string CustomSoaringModuleName()
	{
		return null;
	}

	public override string ModuleName()
	{
		return null;
	}

	public virtual string QueryActionName()
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

	protected void PostCallData(SoaringDictionary parameters, SoaringContext context)
	{
	}

	protected override bool Web_Callback(SCWebQueue.SCWebQueueState state, SoaringError error, object userData, object data)
	{
		return false;
	}

	public override void HandleDelegateCallback(SoaringModuleData data)
	{
	}

	protected override void BuildEncryptedCall(SoaringDictionary call_data)
	{
	}
}
