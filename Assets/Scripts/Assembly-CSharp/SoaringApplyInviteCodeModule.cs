using System;
using UnityEngine;

public class SoaringApplyInviteCodeModule : SoaringModule
{
	public override string ModuleName()
	{
		return "applyInvitationCode";
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
		PushCorePostDataToQueue(soaringDictionary, 2, context, true);
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
			}
			SoaringDebug.Log((string)data);
			switch (state)
			{
			case SCWebQueue.SCWebQueueState.Finished:
			{
				SoaringDictionary soaringDictionary = (SoaringDictionary)parsed_data.objectWithKey("data");
				SoaringDictionary soaringDictionary2 = (SoaringDictionary)soaringDictionary.objectWithKey("profile");
				string userId = soaringDictionary2.soaringValue("userId");
				HandleDelegateCallback(soaringModuleData.Set(true, null, soaringDictionary, (SoaringContext)userData));
				SoaringInternal.instance.RequestFriendship(null, null, userId, (SoaringContext)userData);
				break;
			}
			case SCWebQueue.SCWebQueueState.Failed:
				HandleDelegateCallback(soaringModuleData.Set(false, null, null, (SoaringContext)userData));
				break;
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("SoaringModule:" + ModuleName() + ": Error: " + ex.Message, LogType.Warning);
			state = SCWebQueue.SCWebQueueState.Failed;
			HandleDelegateCallback(soaringModuleData.Set(false, ex.Message, null, (SoaringContext)userData));
		}
		ReturnModuledata(soaringModuleData);
		return state == SCWebQueue.SCWebQueueState.Finished;
	}

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
		SoaringInternal.Delegate.OnApplyInviteCode(moduleData.state, moduleData.error, moduleData.data, moduleData.context);
	}
}
