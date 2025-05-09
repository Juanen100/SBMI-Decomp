public class SoaringRetrieveUserProfileModule : SoaringModule
{
	public override string ModuleName()
	{
		return "retrieveUserProfile";
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		string text = callData.soaringValue("userId");
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(data.objectWithKey("authToken"), "authToken");
		string text2 = "{\n" + SCQueueTools.CreateJsonMessage("action", ModuleName(), null, soaringDictionary);
		text2 += ",\n";
		callData.removeObjectWithKey("authToken");
		callData.removeObjectWithKey("gameId");
		text2 = text2 + SCQueueTools.CreateJsonMessage("data", null, null, callData) + "\n}";
		soaringDictionary.clear();
		soaringDictionary.addValue(text2, "data");
		if (!string.IsNullOrEmpty(text))
		{
			context.addValue(text, "userId");
		}
		context.addValue(Soaring.Player.AuthToken, "authToken");
		PushCorePostDataToQueue(soaringDictionary, 0, context, false);
	}

	public override bool VerifyCallStillValid(SCWebQueue.SCWebQueueState state, SoaringError error, object userData, object data)
	{
		if (userData == null)
		{
			return true;
		}
		SoaringContext soaringContext = (SoaringContext)userData;
		string text = soaringContext.soaringValue("authToken");
		if (string.IsNullOrEmpty(text))
		{
			return true;
		}
		return Soaring.Player.AuthToken == text;
	}

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
		SoaringUser soaringUser = null;
		if (moduleData.data != null)
		{
			soaringUser = new SoaringUser();
			soaringUser.SetUserData(moduleData.data);
		}
		if (VerifyCallStillValid(SCWebQueue.SCWebQueueState.Finished, moduleData.error, moduleData.context, null) && moduleData.context.objectWithKey("userId") == null)
		{
			SoaringInternal.instance.UpdatePlayerData(moduleData.data);
		}
		SoaringInternal.Delegate.OnRetrieveUserProfile(moduleData.state, moduleData.error, soaringUser, moduleData.context);
	}
}
