public class SoaringAddFriendModule : SoaringModule
{
	public override string ModuleName()
	{
		return "requestFriendship";
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(data.objectWithKey("authToken"), "authToken");
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", ModuleName(), null, soaringDictionary);
		text += ",\n";
		callData.removeObjectWithKey("authToken");
		callData.removeObjectWithKey("gameId");
		SoaringObjectBase soaringObjectBase = callData.objectWithKey("userId");
		if (soaringObjectBase != null && soaringObjectBase.Type == IsType.Array)
		{
			SoaringArray soaringArray = (SoaringArray)soaringObjectBase;
			callData.setValue(soaringArray.objectAtIndex(0), "userId");
			soaringArray.removeObjectAtIndex(0);
			if (soaringArray.count() > 0)
			{
				if (context == null)
				{
					context = new SoaringContext();
				}
				context.setValue(soaringArray, "userIds");
			}
		}
		text = text + SCQueueTools.CreateJsonMessage("data", null, null, callData) + "\n}";
		soaringDictionary.clear();
		soaringDictionary.addValue(text, "data");
		PushCorePostDataToQueue(soaringDictionary, 2, context, true);
	}

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
		if (moduleData.state && moduleData.context != null)
		{
			SoaringArray soaringArray = (SoaringArray)moduleData.context.objectWithKey("userIds");
			if (soaringArray != null && soaringArray.count() > 0)
			{
				SoaringInternal.instance.RequestFriendships(soaringArray, moduleData.context);
				return;
			}
		}
		SoaringInternal.Delegate.OnRequestFriend(moduleData.state, moduleData.error, moduleData.data, moduleData.context);
		Soaring.UpdateFriendsListWithLastSettings();
	}
}
