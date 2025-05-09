using System;
using System.Text;
using UnityEngine;

public class SoaringCustomQueryModule : SoaringModule
{
	public virtual string CustomSoaringModuleName()
	{
		return null;
	}

	public override string ModuleName()
	{
		return CustomSoaringModuleName();
	}

	public virtual string QueryActionName()
	{
		return "customQuery";
	}

	public override int ModuleChannel()
	{
		return 2;
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(data.objectWithKey("authToken"), "authToken");
		callData.removeObjectWithKey("authToken");
		callData.removeObjectWithKey("gameId");
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", QueryActionName(), null, soaringDictionary) + ",\n";
		string text2 = text;
		text = text2 + "\"data\" : {\n\"queryService\" : \"" + CustomSoaringModuleName() + "\",\n\"queryParameters\" : " + callData.ToJsonString() + "\n}\n}";
		soaringDictionary.clear();
		soaringDictionary.addValue(text, "data");
		PostCallData(soaringDictionary, context);
	}

	protected void PostCallData(SoaringDictionary parameters, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(6);
		soaringDictionary.addValue(parameters, "tposts");
		soaringDictionary.addValue(ModuleChannel(), "tchannel");
		soaringDictionary.addValue(new SCWebQueue.SCWebCallbackObject(Web_Callback), "tcallback");
		soaringDictionary.addValue(context, "tobject");
		soaringDictionary.addValue(new SCWebQueue.SCWebCallbackObject(VerifyCallStillValid), "tvcallback");
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
				else
				{
					parsed_data = DecryptCall(parsed_data);
				}
			}
			if (!SoaringInternal.IsProductionMode || SoaringDebug.IsLoggingToConsole)
			{
				SoaringDebug.Log((string)data);
			}
			switch (state)
			{
			case SCWebQueue.SCWebQueueState.Finished:
				parsed_data = (SoaringDictionary)parsed_data.objectWithKey("data");
				ExtractTimestamp(parsed_data);
				SoaringInternal.instance.SetSoaringInternalData(parsed_data);
				HandleDelegateCallback(soaringModuleData.Set(true, null, parsed_data, (SoaringContext)userData));
				break;
			case SCWebQueue.SCWebQueueState.Failed:
				HandleDelegateCallback(soaringModuleData.Set(false, error, null, (SoaringContext)userData));
				break;
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("SoaringModule:" + ModuleName() + ": Error: " + ex.Message + " Stack:\n" + ex.StackTrace, LogType.Warning);
			state = SCWebQueue.SCWebQueueState.Failed;
			HandleDelegateCallback(soaringModuleData.Set(false, ex.Message, null, (SoaringContext)userData));
		}
		ReturnModuledata(soaringModuleData);
		return state == SCWebQueue.SCWebQueueState.Finished;
	}

	public override void HandleDelegateCallback(SoaringModuleData data)
	{
		if (data.data == null || data.error != null)
		{
			data.state = false;
			data.data = new SoaringDictionary();
		}
		Soaring.Delegate.OnComponentFinished(data.state, CustomSoaringModuleName(), data.error, data.data, data.context);
	}

	protected override void BuildEncryptedCall(SoaringDictionary call_data)
	{
		try
		{
			if (call_data == null)
			{
				return;
			}
			SoaringDictionary soaringDictionary = (SoaringDictionary)call_data.objectWithKey("tposts");
			if (soaringDictionary == null)
			{
				return;
			}
			string text = soaringDictionary.soaringValue("data");
			if (text != null)
			{
				byte[] array = SoaringInternal.Encryption.Encrypt(Encoding.ASCII.GetBytes(text));
				if (array != null)
				{
					text = Convert.ToBase64String(array);
					string text2 = "{\"action\":{\"name\":\"" + QueryActionName() + "UsingEncryption\",\"sid\":\"" + SoaringEncryption.SID + "\",\"value\":\"" + text + "\"}}";
					soaringDictionary.setValue(text2, "data");
				}
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log(ex.Message + "\n" + ex.StackTrace, LogType.Error);
		}
		catch
		{
			SoaringDebug.Log("BuildEncryptedCall: Unknown Exception Thrown", LogType.Error);
		}
	}
}
