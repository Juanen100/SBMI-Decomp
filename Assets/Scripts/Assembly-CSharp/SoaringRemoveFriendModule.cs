public class SoaringRemoveFriendModule : SoaringModule
{
	public override string ModuleName()
	{
		return "removeFriendship";
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(callData.objectWithKey("authToken"), "authToken");
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", "removeFriendship", null, soaringDictionary);
		text += ",\n";
		soaringDictionary.clear();
		soaringDictionary.addValue(callData.objectWithKey("userId"), "userId");
		text = text + SCQueueTools.CreateJsonMessage("data", null, null, soaringDictionary) + "\n}";
		soaringDictionary.clear();
		soaringDictionary.addValue(text, "data");
		PushCorePostDataToQueue(soaringDictionary, 0, context, true);
	}

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
		Soaring.Delegate.OnRemoveFriend(moduleData.state, moduleData.error, moduleData.data, moduleData.context);
	}
}
