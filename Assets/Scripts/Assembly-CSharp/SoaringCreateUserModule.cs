using System;
using UnityEngine;

public class SoaringCreateUserModule : SoaringModule
{
	public override string ModuleName()
	{
		return "registerUser";
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		context.addValue(callData.soaringValue("password"), "password");
		context.addValue(callData.soaringValue("tag"), "tag");
		SoaringObjectBase soaringObjectBase = callData.objectWithKey("tregister");
		if (soaringObjectBase != null)
		{
			context.addValue(soaringObjectBase, "tregister");
			callData.removeObjectWithKey("tregister");
		}
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
		PushCorePostDataToQueue(soaringDictionary2, 0, context, true);
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
			{
				parsed_data = (SoaringDictionary)parsed_data.objectWithKey("data");
				SoaringContext soaringContext = (SoaringContext)userData;
				string text = soaringContext.soaringValue("tag");
				string text2 = soaringContext.soaringValue("password");
				string text3 = parsed_data.soaringValue("newtag");
				if (!string.IsNullOrEmpty(text3))
				{
					text = text3;
				}
				if (!string.IsNullOrEmpty(text))
				{
					parsed_data.setValue(text, "tag");
				}
				if (!string.IsNullOrEmpty(text2))
				{
					parsed_data.setValue(text2, "password");
				}
				SoaringInternal.instance.UpdatePlayerData(parsed_data, true);
				HandleDelegateCallback(soaringModuleData.Set(true, null, parsed_data, soaringContext));
				break;
			}
			case SCWebQueue.SCWebQueueState.Failed:
				HandleDelegateCallback(soaringModuleData.Set(false, error, null, (SoaringContext)userData));
				break;
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("SoaringModule:" + ModuleName() + ": Error: " + ex.Message + " Stack: " + ex.StackTrace, LogType.Warning);
			state = SCWebQueue.SCWebQueueState.Failed;
			HandleDelegateCallback(soaringModuleData.Set(false, ex.Message, null, (SoaringContext)userData));
		}
		ReturnModuledata(soaringModuleData);
		return state == SCWebQueue.SCWebQueueState.Finished;
	}

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
		SoaringObjectBase soaringObjectBase = null;
		if (moduleData.data != null)
		{
			SoaringContext context = moduleData.context;
			if (context != null)
			{
				soaringObjectBase = context.objectWithKey("tregister");
			}
		}
		else
		{
			moduleData.state = false;
		}
		if (soaringObjectBase == null)
		{
			SoaringPlayer.ValidCredentials = moduleData.state;
			SoaringInternal.Delegate.OnRegisterUser(moduleData.state, moduleData.error, Soaring.Player, moduleData.context);
		}
		else
		{
			SoaringPlayer.ValidCredentials = true;
			SoaringInternal.instance.GenerateInviteCode();
			SoaringInternal.instance.HandleLogin(SoaringLoginType.Soaring, moduleData.state, moduleData.error, moduleData.data, moduleData.context);
		}
	}
}
