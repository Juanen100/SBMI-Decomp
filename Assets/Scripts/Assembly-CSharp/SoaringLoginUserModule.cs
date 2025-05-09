using System;
using UnityEngine;

public class SoaringLoginUserModule : SoaringModule
{
	public string userPassword;

	public string userTag;

	public override string ModuleName()
	{
		return "loginUser";
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		userPassword = callData.soaringValue("password");
		userTag = callData.soaringValue("tag");
		SoaringDictionary soaringDictionary = (SoaringDictionary)callData.objectWithKey("custom");
		if (soaringDictionary != null)
		{
			callData.removeObjectWithKey("custom");
		}
		callData.addValue(data.objectWithKey("gameId"), "gameId");
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", ModuleName(), null, callData);
		if (soaringDictionary != null)
		{
			text = text + ",\n\"data\":" + soaringDictionary.ToJsonString();
		}
		text += "\n}";
		SoaringDictionary soaringDictionary2 = new SoaringDictionary(2);
		soaringDictionary2.addValue(text, "data");
		PushCorePostDataToQueue(soaringDictionary2, 1, context, true);
	}

	protected override bool Web_Callback(SCWebQueue.SCWebQueueState state, SoaringError error, object userData, object data)
	{
		if (state == SCWebQueue.SCWebQueueState.Updated)
		{
			return true;
		}
		SoaringModuleData soaringModuleData = CreateModuleData();
		try
		{
			SoaringDictionary parsed_data = null;
			if (error == null)
			{
				error = SCQueueTools.CheckAndHandleError((string)data, ref parsed_data);
				if (error != null || parsed_data == null)
				{
					state = SCWebQueue.SCWebQueueState.Failed;
				}
				else
				{
					parsed_data = DecryptCall(parsed_data);
				}
			}
			SoaringDebug.Log((string)data);
			switch (state)
			{
			case SCWebQueue.SCWebQueueState.Finished:
				parsed_data = (SoaringDictionary)parsed_data.objectWithKey("data");
				parsed_data.setValue(userTag, "tag");
				parsed_data.setValue(userPassword, "password");
				SoaringInternal.instance.UpdatePlayerData(parsed_data, true);
				SoaringInternal.instance.SetSoaringInternalData(parsed_data);
				HandleDelegateCallback(soaringModuleData.Set(true, null, parsed_data, (SoaringContext)userData));
				Soaring.UpdateFriendsListWithLastSettings();
				break;
			case SCWebQueue.SCWebQueueState.Failed:
				HandleDelegateCallback(soaringModuleData.Set(false, error, null, (SoaringContext)userData));
				break;
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("SoaringModule:" + ModuleName() + ": Error: " + ex.Message + "\nStack: " + ex.StackTrace, LogType.Warning);
			state = SCWebQueue.SCWebQueueState.Failed;
			HandleDelegateCallback(soaringModuleData.Set(false, ex.Message, null, (SoaringContext)userData));
		}
		ReturnModuledata(soaringModuleData);
		userPassword = null;
		userTag = null;
		return state == SCWebQueue.SCWebQueueState.Finished;
	}

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
		if (moduleData.state)
		{
			SoaringPlayer.ValidCredentials = true;
			SoaringInternal.instance.Player.Save();
		}
		SoaringInternal.Delegate.OnAuthorize(moduleData.state, moduleData.error, Soaring.Player, moduleData.context);
		if (moduleData.state)
		{
			SoaringInternal.instance.RetrieveUserProfile(null, moduleData.context);
		}
	}
}
