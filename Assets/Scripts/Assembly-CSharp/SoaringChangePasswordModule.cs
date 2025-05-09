public class SoaringChangePasswordModule : SoaringModule
{
	public override string ModuleName()
	{
		return "changeUserPassword";
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringValue val = callData.soaringValue("newPassword");
		SoaringValue val2 = callData.soaringValue("oldPassword");
		callData.removeObjectWithKey("newPassword");
		callData.removeObjectWithKey("oldPassword");
		callData.removeObjectWithKey("gameId");
		context.addValue(val, "password");
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(val, "newPassword");
		soaringDictionary.addValue(val2, "oldPassword");
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", ModuleName(), null, callData);
		text = text + ",\n\"data\" : " + soaringDictionary.ToJsonString() + "\n}";
		soaringDictionary.clear();
		soaringDictionary.addValue(text, "data");
		PushCorePostDataToQueue(soaringDictionary, 1, context, true);
	}

	public override void HandleDelegateCallback(SoaringModuleData data)
	{
		if (data.context != null && data.state)
		{
			SoaringDictionary soaringDictionary = new SoaringDictionary();
			soaringDictionary.addValue(data.context.objectWithKey("password"), "password");
			SoaringInternal.instance.UpdatePlayerData(soaringDictionary, false);
		}
		SoaringInternal.Delegate.OnPasswordChanged(data.state, data.error, data.context);
	}
}
