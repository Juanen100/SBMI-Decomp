public class SoaringLookupUserModule : SoaringModule
{
	public override bool ShouldEncryptCall()
	{
		return false;
	}

	public override string ModuleName()
	{
		return "lookupUser";
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = (SoaringDictionary)callData.objectWithKey("custom");
		if (soaringDictionary != null)
		{
			callData.removeObjectWithKey("custom");
		}
		callData.addValue(data.objectWithKey("gameId"), "gameId");
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", ModuleName(), null, callData);
		if (soaringDictionary != null)
		{
			text = text + ",\n\"data\":" + soaringDictionary.ToJsonString();
		}
		text += "\n}";
		SoaringDictionary soaringDictionary2 = new SoaringDictionary(2);
		soaringDictionary2.addValue(text, "data");
		PushCorePostDataToQueue(soaringDictionary2, 1, context, true);
	}

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
		bool success = moduleData.error == null;
		Soaring.Delegate.OnLookupUser(success, moduleData.error, moduleData.context);
	}
}
