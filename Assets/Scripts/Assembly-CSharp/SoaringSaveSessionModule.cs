public class SoaringSaveSessionModule : SoaringModule
{
	private bool mIsPersistantSession;

	public override string ModuleName()
	{
		return "saveGameSession";
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(data.objectWithKey("authToken"), "authToken");
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", ModuleName(), null, soaringDictionary);
		text += ",\n";
		callData.removeObjectWithKey("authToken");
		callData.removeObjectWithKey("gameId");
		soaringDictionary.clear();
		text += "\"data\":";
		SoaringDictionary soaringDictionary2 = (SoaringDictionary)callData.objectWithKey("custom");
		if (soaringDictionary2 != null)
		{
			soaringDictionary.addValue(soaringDictionary2, "custom");
			string text2 = callData.soaringValue("sessionType");
			if (text2 == SoaringSession.GetSoaringSessionTypeString(SoaringSession.SessionType.PersistantOneWay))
			{
				mIsPersistantSession = true;
			}
			soaringDictionary.addValue(text2, "sessionType");
		}
		SoaringObjectBase soaringObjectBase = callData.objectWithKey("gameSessionId");
		if (soaringObjectBase != null)
		{
			soaringDictionary.addValue(soaringObjectBase, "gameSessionId");
		}
		else
		{
			soaringObjectBase = callData.objectWithKey("label");
			if (soaringObjectBase != null)
			{
				soaringDictionary.addValue(soaringObjectBase, "label");
			}
		}
		text += soaringDictionary.ToJsonString();
		text += "}\n";
		soaringDictionary.clear();
		soaringDictionary.addValue(text, "data");
		context.addValue(Soaring.Player.AuthToken, "authToken");
		PushDataToQueue(soaringDictionary, ModuleChannel(), context);
	}

	protected void PushDataToQueue(SoaringDictionary data, int channel, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(5);
		if (data != null)
		{
			soaringDictionary.addValue(data, "tposts");
		}
		soaringDictionary.addValue(channel, "tchannel");
		SCWebQueue.SCWebCallbackObject val = new SCWebQueue.SCWebCallbackObject(Web_Callback);
		soaringDictionary.addValue(val, "tcallback");
		if (context.containsKey("trIOff"))
		{
			soaringDictionary.addValue(true, "trIOff");
		}
		val = new SCWebQueue.SCWebCallbackObject(VerifyCallStillValid);
		soaringDictionary.addValue(val, "tvcallback");
		if (context != null)
		{
			soaringDictionary.addValue(context, "tobject");
		}
		PushCallData(soaringDictionary, context);
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
		if (moduleData.data == null)
		{
			moduleData.state = false;
		}
		bool flag = VerifyCallStillValid(SCWebQueue.SCWebQueueState.Finished, moduleData.error, moduleData.context, moduleData.data);
		if (moduleData.state && !flag)
		{
			moduleData.state = flag;
		}
		SoaringDictionary soaringDictionary = null;
		if (mIsPersistantSession && moduleData.state)
		{
			bool flag2 = false;
			soaringDictionary = new SoaringDictionary();
			SoaringDictionary soaringDictionary2 = Soaring.Player.CustomData;
			if (soaringDictionary2 == null)
			{
				soaringDictionary2 = new SoaringDictionary();
				flag2 = true;
			}
			SoaringDictionary soaringDictionary3 = (SoaringDictionary)soaringDictionary2.objectWithKey("public");
			if (soaringDictionary3 == null)
			{
				soaringDictionary3 = new SoaringDictionary();
				soaringDictionary2.addValue(soaringDictionary3, "public");
				flag2 = true;
			}
			string text = moduleData.data.soaringValue("gameSessionId");
			if (text != null)
			{
				soaringDictionary3.setValue(text, "gameSessionId");
			}
			if (flag2)
			{
				soaringDictionary = new SoaringDictionary();
				soaringDictionary.addValue(soaringDictionary2, "custom");
				Soaring.Player.SetUserData(soaringDictionary);
			}
		}
		SoaringInternal.Delegate.OnSavingSessionData(moduleData.state, moduleData.error, moduleData.data, moduleData.context);
		if (mIsPersistantSession && soaringDictionary != null && moduleData.state)
		{
			Soaring.UpdateUserProfile(null, Soaring.Player.CustomData, moduleData.context);
		}
		mIsPersistantSession = false;
	}
}
