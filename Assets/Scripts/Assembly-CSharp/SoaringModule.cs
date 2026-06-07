public class SoaringModule : SoaringObjectBase
{
	public class SoaringModuleData : SoaringObjectBase
	{
		public bool state;

		public SoaringDictionary data;

		public SoaringContext context;

		public SoaringError error;

		public SoaringModuleData()
			: base(default(IsType))
		{
		}

		public void Reset()
		{
		}

		public SoaringModuleData Set(bool state, SoaringError error, SoaringDictionary data, SoaringContext context_data)
		{
			return null;
		}
	}

	private static SoaringArray sModuleDataArray;

	public bool encryptedCall;

	public SoaringModule()
		: base(default(IsType))
	{
	}

	protected SoaringModuleData CreateModuleData()
	{
		return null;
	}

	public virtual bool ShouldEncryptCall()
	{
		return false;
	}

	protected void ReturnModuledata(SoaringModuleData data)
	{
	}

	public virtual int ModuleChannel()
	{
		return 0;
	}

	public virtual string ModuleName()
	{
		return null;
	}

	public virtual void InitializeModule(SoaringDictionary data)
	{
	}

	public virtual void FinalizeModule(SoaringDictionary data)
	{
	}

	public virtual bool VerifyCallStillValid(SCWebQueue.SCWebQueueState state, SoaringError error, object userData, object data)
	{
		return false;
	}

	public virtual void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
	}

	protected void PushCorePostDataToQueue(SoaringDictionary data, int channel, SoaringContext context, bool updatePlayer)
	{
	}

	protected virtual void BuildEncryptedCall(SoaringDictionary call_data)
	{
	}

	protected virtual SoaringDictionary DecryptCall(SoaringDictionary encodedData)
	{
		return null;
	}

	protected void PushCallData(SoaringDictionary call_data, SoaringContext context)
	{
	}

	protected virtual bool Web_Callback(SCWebQueue.SCWebQueueState state, SoaringError error, object userData, object data)
	{
		return false;
	}

	protected virtual bool Web_Callback_NoPlayerUpdate(SCWebQueue.SCWebQueueState state, SoaringError error, object userData, object data)
	{
		return false;
	}

	protected virtual bool Web_Callback_Handler(SCWebQueue.SCWebQueueState state, SoaringError error, object userData, object data, bool updatePlayer)
	{
		return false;
	}

	protected void ExtractTimestamp(SoaringDictionary data)
	{
	}

	public virtual void HandleDelegateCallback(SoaringModuleData data)
	{
	}
}
