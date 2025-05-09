using System;
using UnityEngine;

public class SoaringVersionSoaringModule : SoaringModule
{
	public override string ModuleName()
	{
		return "retrieveVersions";
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
				HandleDelegateCallback(soaringModuleData.Set(true, null, parsed_data, (SoaringContext)userData));
				break;
			case SCWebQueue.SCWebQueueState.Failed:
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
		string source = null;
		string commit = null;
		long newVersion = -1L;
		SoaringArray soaringArray = null;
		SoaringArray diffs = null;
		if (moduleData.state && moduleData.error == null && moduleData.data != null)
		{
			SoaringDictionary data = moduleData.data;
			newVersion = data.soaringValue("version");
			commit = data.soaringValue("commit");
			source = data.soaringValue("source");
			soaringArray = (SoaringArray)data.objectWithKey("contents");
			if (SoaringInternal.Campaign != null && soaringArray != null)
			{
				diffs = (SoaringArray)data.objectWithKey(SoaringInternal.Campaign.Group, true);
			}
			SoaringDictionary soaringDictionary = (SoaringDictionary)data.objectWithKey(SoaringPlatform.PrimaryPlatformName, true);
			SoaringArray subContentCategories = SoaringInternal.instance.Versions.SubContentCategories;
			if (subContentCategories != null)
			{
				for (int i = 0; i < subContentCategories.count(); i++)
				{
					string key = subContentCategories.soaringValue(i);
					SoaringArray soaringArray2 = (SoaringArray)data.objectWithKey(key);
					if (soaringArray2 != null)
					{
						int num = soaringArray2.count();
						for (int j = 0; j < num; j++)
						{
							soaringArray.addObject(soaringArray2.objectAtIndex(i));
						}
					}
				}
				if (soaringDictionary != null)
				{
					for (int k = 0; k < subContentCategories.count(); k++)
					{
						string key2 = subContentCategories.soaringValue(k);
						SoaringArray soaringArray3 = (SoaringArray)soaringDictionary.objectWithKey(key2);
						if (soaringArray3 != null)
						{
							int num2 = soaringArray3.count();
							for (int l = 0; l < num2; l++)
							{
								soaringArray.addObject(soaringArray3.objectAtIndex(k));
							}
						}
					}
				}
			}
		}
		SoaringInternal.instance.Versions.AddFileVersions(soaringArray, diffs, newVersion, source, commit);
	}
}
