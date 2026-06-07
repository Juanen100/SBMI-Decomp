public class SoaringSaveSessionModule : SoaringModule
{
	private bool mIsPersistantSession;

	public override string ModuleName()
	{
		return null;
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
	}

	protected void PushDataToQueue(SoaringDictionary data, int channel, SoaringContext context)
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
