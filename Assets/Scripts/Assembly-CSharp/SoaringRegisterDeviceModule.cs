using System;
using UnityEngine;

public class SoaringRegisterDeviceModule : SoaringModule
{
	public override string ModuleName()
	{
		return "registerDevice";
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
		PushCorePostDataToQueue(soaringDictionary, 2, context, false);
	}

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
		if (moduleData.context != null && moduleData.state)
		{
			try
			{
				string value = moduleData.context.soaringValue("trToken");
				if (!string.IsNullOrEmpty(value))
				{
					PlayerPrefs.SetString("trToken", value);
				}
			}
			catch (Exception ex)
			{
				SoaringDebug.Log("RegisterDevice" + ex.Message);
			}
		}
		SoaringInternal.Delegate.OnDeviceRegistered(moduleData.state, moduleData.error, moduleData.context);
	}
}
