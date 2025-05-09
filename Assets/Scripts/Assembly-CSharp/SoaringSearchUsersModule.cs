public class SoaringSearchUsersModule : SoaringModule
{
	public override string ModuleName()
	{
		return "searchUsers";
	}

	public override int ModuleChannel()
	{
		return 2;
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
		PushCorePostDataToQueue(soaringDictionary, ModuleChannel(), context, false);
	}

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
		SoaringArray<SoaringUser> soaringArray = null;
		SoaringUser[] users = null;
		if (moduleData.state)
		{
			if (moduleData.data != null)
			{
				SoaringArray data = (SoaringArray)moduleData.data.objectWithKey("users");
				soaringArray = SCQueueTools.ParseUsers(data, true);
			}
			if (soaringArray == null)
			{
				moduleData.state = false;
			}
			else if (soaringArray.count() == 0)
			{
				moduleData.state = false;
			}
			else
			{
				users = soaringArray.array();
			}
		}
		SoaringInternal.Delegate.OnFindUser(moduleData.state, moduleData.error, users, moduleData.context);
	}
}
