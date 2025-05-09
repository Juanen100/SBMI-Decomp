public class SoaringDownloadModule : SoaringModule
{
	public override string ModuleName()
	{
		return "downloadFiles";
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(4);
		string text = callData.soaringValue("turl");
		if (!string.IsNullOrEmpty(text))
		{
			SoaringDebug.Log("SoaringDownloadModule: url: " + text);
			soaringDictionary.addValue(text, "turl");
		}
		SoaringDictionary soaringDictionary2 = (SoaringDictionary)callData.objectWithKey("tgets");
		if (soaringDictionary2 != null)
		{
			soaringDictionary.addValue(soaringDictionary2, "tgets");
		}
		soaringDictionary2 = (SoaringDictionary)callData.objectWithKey("tposts");
		if (soaringDictionary2 != null)
		{
			soaringDictionary.addValue(soaringDictionary2, "tposts");
		}
		object obj = callData.objectWithKey("tcallback");
		SoaringObjectBase val = callData.objectWithKey("tsave");
		context.addValue((SCWebQueue.SCDownloadCallbackObject)obj, "tcallback");
		context.addValue(callData.objectWithKey("tobject"), "tobject");
		context.addValue(val, "tsave");
		soaringDictionary.addValue(4, "tchannel");
		soaringDictionary.addValue(new SCWebQueue.SCWebCallbackObject(Web_Callback), "tcallback");
		soaringDictionary.addValue(val, "tsave");
		soaringDictionary.addValue(context, "tobject");
		PushCallData(soaringDictionary, context);
	}

	protected override bool Web_Callback(SCWebQueue.SCWebQueueState state, SoaringError error, object userData, object data)
	{
		if (state == SCWebQueue.SCWebQueueState.Updated)
		{
			SoaringInternal.Delegate.OnFileDownloadUpdate(SoaringState.Update, null, data, (SoaringContext)userData);
			return true;
		}
		if (error != null)
		{
			state = SCWebQueue.SCWebQueueState.Failed;
		}
		SoaringModuleData soaringModuleData = CreateModuleData();
		HandleDelegateCallback(soaringModuleData.Set(state == SCWebQueue.SCWebQueueState.Finished, error, null, (SoaringContext)userData));
		ReturnModuledata(soaringModuleData);
		return state != SCWebQueue.SCWebQueueState.Finished;
	}

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
		if (moduleData.context != null)
		{
			SoaringContext context = moduleData.context;
			SCWebQueue.SCDownloadCallbackObject sCDownloadCallbackObject = (SCWebQueue.SCDownloadCallbackObject)context.objectWithKey("tcallback");
			string id = context.soaringValue("tobject");
			string path = context.soaringValue("tsave");
			if (sCDownloadCallbackObject.callback != null)
			{
				sCDownloadCallbackObject.callback(id, moduleData.state, path);
			}
		}
		else
		{
			SoaringInternal.Delegate.OnFileDownloadUpdate((!moduleData.state) ? SoaringState.Fail : SoaringState.Success, moduleData.error, null, moduleData.context);
		}
	}
}
