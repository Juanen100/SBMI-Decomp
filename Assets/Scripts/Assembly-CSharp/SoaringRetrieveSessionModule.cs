public class SoaringRetrieveSessionModule : SoaringModule
{
	public override string ModuleName()
	{
		return "retrieveGameSession";
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(data.objectWithKey("authToken"), "authToken");
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", ModuleName(), null, soaringDictionary);
		text += ",\n";
		callData.removeObjectWithKey("authToken");
		callData.removeObjectWithKey("gameId");
		text += "\"data\":{";
		bool flag = false;
		SoaringDictionary soaringDictionary2 = (SoaringDictionary)callData.objectWithKey("query");
		if (soaringDictionary2 != null)
		{
			text = text + "\n\"query\":" + soaringDictionary2.ToJsonString();
			flag = true;
		}
		soaringDictionary2 = (SoaringDictionary)callData.objectWithKey("sort");
		if (soaringDictionary2 != null)
		{
			text = text + ((!flag) ? "\n" : ",\n") + "\"sort\":" + soaringDictionary2.ToJsonString();
			flag = true;
		}
		SoaringArray soaringArray = (SoaringArray)callData.objectWithKey("identifiers");
		if (soaringArray != null)
		{
			text = text + ((!flag) ? "\n" : ",\n") + "\"identifiers\":" + soaringArray.ToJsonString();
			flag = true;
		}
		string text2 = callData.soaringValue("queryType");
		if (text2 != null)
		{
			string text3 = text;
			text = text3 + ((!flag) ? "\n" : ",\n") + "\"queryType\":\"" + text2 + "\"";
			flag = true;
		}
		text2 = callData.soaringValue("sessionType");
		if (!string.IsNullOrEmpty(text2))
		{
			string text3 = text;
			text = text3 + ((!flag) ? "\n" : ",\n") + "\"sessionType\":\"" + text2 + "\"";
			flag = true;
		}
		text += "\n}\n}\n";
		soaringDictionary.clear();
		soaringDictionary.addValue(text, "data");
		PushCorePostDataToQueue(soaringDictionary, 1, context, false);
	}

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
		SoaringArray soaringArray = null;
		if (moduleData.data != null)
		{
			SoaringArray soaringArray2 = (SoaringArray)moduleData.data.objectWithKey("sessions");
			if (soaringArray2 != null)
			{
				int num = soaringArray2.count();
				soaringArray = new SoaringArray(num);
				for (int i = 0; i < num; i++)
				{
					SoaringDictionary soaringDictionary = (SoaringDictionary)soaringArray2.objectAtIndex(i);
					SoaringObjectBase soaringObjectBase = soaringDictionary.objectWithKey("custom");
					if (soaringObjectBase != null)
					{
						soaringArray.addObject(soaringObjectBase);
					}
				}
			}
			SoaringObjectBase soaringObjectBase2 = moduleData.data.objectWithKey("custom");
			if (soaringObjectBase2 != null)
			{
				if (soaringObjectBase2.Type == IsType.Array)
				{
					soaringArray = (SoaringArray)soaringObjectBase2;
				}
				else
				{
					if (soaringArray != null)
					{
						soaringArray = new SoaringArray(1);
					}
					soaringArray.addObject(soaringObjectBase2);
				}
			}
		}
		SoaringInternal.Delegate.OnRequestingSessionData(moduleData.state, moduleData.error, soaringArray, moduleData.data, moduleData.context);
	}
}
