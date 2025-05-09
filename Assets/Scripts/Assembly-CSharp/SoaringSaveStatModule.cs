public class SoaringSaveStatModule : SoaringModule
{
	public override string ModuleName()
	{
		return "saveStat";
	}

	public override int ModuleChannel()
	{
		return 3;
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		string text = "{\n\"action\" : {\n\"name\":\"" + ModuleName() + "\",\n";
		text = string.Concat(text, "\"authToken\":\"", data.soaringValue("authToken"), "\"\n},\n\"data\" : ");
		callData.removeObjectWithKey("authToken");
		callData.removeObjectWithKey("gameId");
		text += callData.ToJsonString();
		text += "\n}";
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringDictionary.addValue(text, "data");
		PushCorePostDataToQueue(soaringDictionary, ModuleChannel(), context, false);
	}

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
		Soaring.Delegate.OnSaveStat(moduleData.state, false, moduleData.error, moduleData.context);
	}
}
