public class SoaringVerifyServerRecieptModule : SoaringModule
{
	public override string ModuleName()
	{
		return "validateIapReceipt";
	}

	public override int ModuleChannel()
	{
		return 0;
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(data.objectWithKey("authToken"), "authToken");
		callData.removeObjectWithKey("gameId");
		callData.removeObjectWithKey("authToken");
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", ModuleName(), null, soaringDictionary);
		text += ",\n";
		text = text + SCQueueTools.CreateJsonMessage("data", null, null, callData) + "\n}";
		soaringDictionary.clear();
		soaringDictionary.addValue(text, "data");
		PushCorePostDataToQueue(soaringDictionary, ModuleChannel(), context, false);
	}

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
		bool success = false;
		if (moduleData.error == null && moduleData.data != null)
		{
			SoaringValue soaringValue = moduleData.data.soaringValue("valid");
			if ((bool)soaringValue)
			{
				success = true;
			}
		}
		Soaring.Delegate.OnRecieptValidated(success, moduleData.error, moduleData.context);
	}
}
