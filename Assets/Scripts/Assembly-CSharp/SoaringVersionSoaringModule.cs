public class SoaringVersionSoaringModule : SoaringModule
{
	public override string ModuleName()
	{
		return null;
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
	}

	protected override bool Web_Callback(SCWebQueue.SCWebQueueState state, SoaringError error, object userData, object data)
	{
		return false;
	}

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
	}
}
