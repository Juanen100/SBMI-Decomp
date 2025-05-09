public class SoaringMarkMessageReadModule : SoaringModule
{
	public override string ModuleName()
	{
		return "markMessagesAsRead";
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(data.objectWithKey("authToken"), "authToken");
		callData.removeObjectWithKey("gameId");
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", ModuleName(), null, soaringDictionary);
		text += ",\n";
		SoaringArray soaringArray = (SoaringArray)callData.objectWithKey("messages");
		if (soaringArray != null)
		{
			callData.removeObjectWithKey("messages");
			callData.addValue(soaringArray, "messageIds");
		}
		callData.removeObjectWithKey("authToken");
		text = text + SCQueueTools.CreateJsonMessage("data", null, null, callData) + "\n}";
		soaringDictionary.clear();
		soaringDictionary.addValue(text, "data");
		PushCorePostDataToQueue(soaringDictionary, 0, context, true);
	}

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
		Soaring.Delegate.OnMessageStateChanged(moduleData.state, moduleData.error, moduleData.data);
	}
}
