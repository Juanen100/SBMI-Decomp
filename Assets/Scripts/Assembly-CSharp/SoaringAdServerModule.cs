using System;
using UnityEngine;

public class SoaringAdServerModule : SoaringModule
{
	public override string ModuleName()
	{
		return "adServer";
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		string text = callData.soaringValue("turl");
		SoaringDictionary soaringDictionary = new SoaringDictionary(4);
		if (string.IsNullOrEmpty(text))
		{
			SoaringDictionary soaringDictionary2 = SCQueueTools.CreateMessage(ModuleName(), data.soaringValue("gameId"), callData);
			if (soaringDictionary2 != null)
			{
				soaringDictionary.addValue(soaringDictionary2, "tposts");
			}
		}
		else
		{
			soaringDictionary.addValue(text, "turl");
			SoaringValue val = new SoaringValue(text);
			context.addValue(val, "turl");
		}
		soaringDictionary.addValue(0, "tchannel");
		soaringDictionary.addValue(new SCWebQueue.SCWebCallbackObject(Web_Callback), "tcallback");
		soaringDictionary.addValue(context, "tobject");
		PushCallData(soaringDictionary, context);
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
				if (soaringDictionary != null)
				{
					parsed_data = soaringDictionary;
				}
				HandleDelegateCallback(soaringModuleData.Set(true, null, parsed_data, (SoaringContext)userData));
				break;
			}
			case SCWebQueue.SCWebQueueState.Failed:
				SoaringDebug.Log("SoaringModule:" + ModuleName() + ": Error: Download Failed", LogType.Error);
				HandleDelegateCallback(soaringModuleData.Set(false, error, null, (SoaringContext)userData));
				break;
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("SoaringModule:" + ModuleName() + ": Error: " + ex.Message + "\n" + ex.StackTrace, LogType.Warning);
			state = SCWebQueue.SCWebQueueState.Failed;
			HandleDelegateCallback(soaringModuleData.Set(false, ex.Message, null, (SoaringContext)userData));
		}
		ReturnModuledata(soaringModuleData);
		return state == SCWebQueue.SCWebQueueState.Finished;
	}

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
		if (moduleData.data == null)
		{
			moduleData.state = false;
		}
		SoaringInternal.instance.AdServer.HandleAdRequestReturn(moduleData.data, moduleData.context);
	}
}
