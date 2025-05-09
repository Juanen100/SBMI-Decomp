public class SoaringSendMessageModule : SoaringModule
{
	public override string ModuleName()
	{
		return "sendMessage";
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(callData.soaringValue("authToken"), "authToken");
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", ModuleName(), null, soaringDictionary);
		soaringDictionary.clear();
		SoaringMessage soaringMessage = (SoaringMessage)callData.objectWithKey("messages");
		text = text + ",\n\"data\" : " + soaringMessage.ToJsonString() + "\n}";
		soaringDictionary.addValue(text, "data");
		context.addValue(soaringMessage, "message");
		PushCorePostDataToQueue(soaringDictionary, 0, context, true);
	}

	public override void HandleDelegateCallback(SoaringModuleData moduledata)
	{
		SoaringMessage message = null;
		if (moduledata.context != null)
		{
			message = (SoaringMessage)moduledata.context.objectWithKey("message");
		}
		Soaring.Delegate.OnSendMessage(moduledata.state, moduledata.error, message);
	}
}
