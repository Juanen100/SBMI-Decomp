public class SoaringRedeemRewardModule : SoaringModule
{
	public override string ModuleName()
	{
		return "tearVirtualGoodCoupons";
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(data.objectWithKey("authToken"), "authToken");
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", ModuleName(), null, soaringDictionary);
		text += ",\n";
		callData.removeObjectWithKey("authToken");
		callData.removeObjectWithKey("gameId");
		text = text + SCQueueTools.CreateJsonMessage("data", null, null, callData) + "\n}";
		soaringDictionary.clear();
		soaringDictionary.addValue(text, "data");
		PushCorePostDataToQueue(soaringDictionary, 1, context, true);
	}

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
		SoaringInternal.Delegate.OnRedeemUserReward(moduleData.state, moduleData.error, moduleData.data);
	}
}
