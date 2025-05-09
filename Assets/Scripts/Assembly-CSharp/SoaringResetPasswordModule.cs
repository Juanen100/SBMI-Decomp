public class SoaringResetPasswordModule : SoaringModule
{
	public override string ModuleName()
	{
		return "resetUserPassword";
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", ModuleName(), null, null);
		text = text + ",\n\"data\" : " + callData.ToJsonString() + "\n}";
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringDictionary.addValue(text, "data");
		PushCorePostDataToQueue(soaringDictionary, 0, context, false);
	}

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
		SoaringInternal.Delegate.OnPasswordReset(moduleData.state, moduleData.error);
	}
}
