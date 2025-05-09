public class SoaringUpdateUserModule : SoaringModule
{
	public override string ModuleName()
	{
		return "updateUserProfile";
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		string text = "{\n\"action\" : {\n\"name\":\"" + ModuleName() + "\",\n";
		text = string.Concat(text, "\"authToken\":\"", callData.soaringValue("authToken"), "\"\n},");
		text += "\"data\" : ";
		callData.removeObjectWithKey("authToken");
		callData.removeObjectWithKey("gameId");
		SoaringArray soaringArray = (SoaringArray)callData.objectWithKey("emails");
		if (soaringArray != null)
		{
			context.addValue(soaringArray, "emails");
		}
		text += callData.ToJsonString();
		text += "\n}";
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringDictionary.addValue(text, "data");
		context.addValue(Soaring.Player.AuthToken, "authToken");
		PushCorePostDataToQueue(soaringDictionary, 1, context, false);
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
		if (moduleData.state && moduleData.data != null)
		{
			if (moduleData.context != null)
			{
				SoaringObjectBase soaringObjectBase = moduleData.context.objectWithKey("emails");
				if (soaringObjectBase != null)
				{
					moduleData.data.addValue(soaringObjectBase, "emails");
				}
			}
			if (VerifyCallStillValid(SCWebQueue.SCWebQueueState.Finished, moduleData.error, moduleData.context, null))
			{
				SoaringInternal.instance.UpdatePlayerData(moduleData.data, false);
			}
		}
		SoaringInternal.Delegate.OnUpdatingUserProfile(moduleData.state, moduleData.error, moduleData.data, moduleData.context);
	}
}
