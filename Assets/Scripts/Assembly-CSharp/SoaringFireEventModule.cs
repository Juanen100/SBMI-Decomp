public class SoaringFireEventModule : SoaringModule
{
	public override string ModuleName()
	{
		return "fireEvent";
	}

	public override int ModuleChannel()
	{
		return 3;
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
		if (moduleData.state && moduleData.data != null)
		{
			SoaringArray soaringArray = (SoaringArray)moduleData.data.objectWithKey("events");
			if (soaringArray != null)
			{
				SoaringInternal.instance.Events.LoadEvents(soaringArray);
			}
			SoaringDictionary soaringDictionary = (SoaringDictionary)moduleData.data.objectWithKey("ev");
			if (soaringDictionary != null)
			{
				SoaringArray soaringArray2 = new SoaringArray(1);
				soaringArray2.addObject(soaringDictionary);
				SoaringInternal.instance.Events.LoadEvents(soaringArray2);
			}
		}
	}
}
