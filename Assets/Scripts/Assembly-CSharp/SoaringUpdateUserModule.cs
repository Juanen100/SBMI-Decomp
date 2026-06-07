public class SoaringUpdateUserModule : SoaringModule
{
	public override string ModuleName()
	{
		return null;
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
	}

	public override bool VerifyCallStillValid(SCWebQueue.SCWebQueueState state, SoaringError error, object userData, object data)
	{
		return false;
	}

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
	}
}
