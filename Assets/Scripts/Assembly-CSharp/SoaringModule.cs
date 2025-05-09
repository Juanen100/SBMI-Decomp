using System;
using System.Text;
using UnityEngine;

public class SoaringModule : SoaringObjectBase
{
	public class SoaringModuleData : SoaringObjectBase
	{
		public bool state;

		public SoaringDictionary data;

		public SoaringContext context;

		public SoaringError error;

		public SoaringModuleData()
			: base(IsType.Object)
		{
		}

		public void Reset()
		{
			state = false;
			data = null;
			context = null;
			error = null;
		}

		public SoaringModuleData Set(bool state, SoaringError error, SoaringDictionary data, SoaringContext context_data)
		{
			this.state = state;
			this.error = error;
			this.data = data;
			context = context_data;
			return this;
		}
	}

	private static SoaringArray sModuleDataArray;

	public bool encryptedCall = SoaringInternalProperties.SecureCommunication;

	public SoaringModule()
		: base(IsType.Module)
	{
	}

	protected SoaringModuleData CreateModuleData()
	{
		if (sModuleDataArray == null)
		{
			sModuleDataArray = new SoaringArray();
		}
		SoaringModuleData soaringModuleData = null;
		if (sModuleDataArray.count() == 0)
		{
			soaringModuleData = new SoaringModuleData();
		}
		else
		{
			soaringModuleData = (SoaringModuleData)sModuleDataArray.objectAtIndex(0);
			sModuleDataArray.removeObjectAtIndex(0);
			soaringModuleData.Reset();
		}
		return soaringModuleData;
	}

	public virtual bool ShouldEncryptCall()
	{
		return encryptedCall;
	}

	protected void ReturnModuledata(SoaringModuleData data)
	{
		if (data != null && sModuleDataArray != null)
		{
			sModuleDataArray.addObject(data);
		}
	}

	public virtual int ModuleChannel()
	{
		return 0;
	}

	public virtual string ModuleName()
	{
		return null;
	}

	public virtual void InitializeModule(SoaringDictionary data)
	{
	}

	public virtual void FinalizeModule(SoaringDictionary data)
	{
	}

	public virtual bool VerifyCallStillValid(SCWebQueue.SCWebQueueState state, SoaringError error, object userData, object data)
	{
		return true;
	}

	public virtual void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary data2 = SCQueueTools.CreateMessage(ModuleName(), data.soaringValue("gameId"), callData);
		PushCorePostDataToQueue(data2, ModuleChannel(), context, true);
	}

	protected void PushCorePostDataToQueue(SoaringDictionary data, int channel, SoaringContext context, bool updatePlayer)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(4);
		if (data != null)
		{
			soaringDictionary.addValue(data, "tposts");
		}
		soaringDictionary.addValue(channel, "tchannel");
		SCWebQueue.SCWebCallbackObject sCWebCallbackObject = null;
		sCWebCallbackObject = ((!updatePlayer) ? new SCWebQueue.SCWebCallbackObject(Web_Callback_NoPlayerUpdate) : new SCWebQueue.SCWebCallbackObject(Web_Callback));
		soaringDictionary.addValue(sCWebCallbackObject, "tcallback");
		sCWebCallbackObject = new SCWebQueue.SCWebCallbackObject(VerifyCallStillValid);
		soaringDictionary.addValue(sCWebCallbackObject, "tvcallback");
		if (context != null)
		{
			soaringDictionary.addValue(context, "tobject");
		}
		PushCallData(soaringDictionary, context);
	}

	protected virtual void BuildEncryptedCall(SoaringDictionary call_data)
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
				byte[] array = SoaringInternal.Encryption.Encrypt(text);
				if (array != null)
				{
					text = Convert.ToBase64String(array);
					string text2 = "{\"action\":{\"name\":\"" + ModuleName() + "UsingEncryption\",\"sid\":\"" + SoaringEncryption.SID + "\",\"value\":\"" + text + "\"}}";
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

	protected virtual SoaringDictionary DecryptCall(SoaringDictionary encodedData)
	{
		if (!ShouldEncryptCall() || !SoaringInternalProperties.SecureCommunication || encodedData == null)
		{
			return encodedData;
		}
		try
		{
			SoaringDictionary soaringDictionary = (SoaringDictionary)encodedData.objectWithKey("data");
			if (soaringDictionary != null)
			{
				string text = soaringDictionary.soaringValue("value");
				if (text != null)
				{
					byte[] array = SoaringInternal.Encryption.Decrypt(Convert.FromBase64String(text));
					if (array != null)
					{
						string text2 = Encoding.ASCII.GetString(array);
						encodedData = new SoaringDictionary(text2);
						if ((!SoaringInternal.IsProductionMode || SoaringDebug.IsLoggingToConsole) && encodedData != null)
						{
							SoaringDebug.Log("Decrypted: " + text2);
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log(ex.Message + "\n" + ex.StackTrace, LogType.Error);
			encodedData = null;
		}
		return encodedData;
	}

	protected void PushCallData(SoaringDictionary call_data, SoaringContext context)
	{
		if (ShouldEncryptCall())
		{
			if (!SoaringEncryption.IsEncryptionAvailable())
			{
				Web_Callback(SCWebQueue.SCWebQueueState.Failed, "{ \"error_message\" : \"Failed to generate connection request\"}", context, "{ \"error_message\" : \"Failed to generate connection request\"}");
				return;
			}
			BuildEncryptedCall(call_data);
		}
		SoaringInternal instance = SoaringInternal.instance;
		if (!instance.PushCall(call_data))
		{
			Web_Callback(SCWebQueue.SCWebQueueState.Failed, "{ \"error_message\" : \"Failed to generate connection request\"}", context, "{ \"error_message\" : \"Failed to generate connection request\"}");
		}
	}

	protected virtual bool Web_Callback(SCWebQueue.SCWebQueueState state, SoaringError error, object userData, object data)
	{
		return Web_Callback_Handler(state, error, userData, data, true);
	}

	protected virtual bool Web_Callback_NoPlayerUpdate(SCWebQueue.SCWebQueueState state, SoaringError error, object userData, object data)
	{
		return Web_Callback_Handler(state, error, userData, data, false);
	}

	protected virtual bool Web_Callback_Handler(SCWebQueue.SCWebQueueState state, SoaringError error, object userData, object data, bool updatePlayer)
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
				if (updatePlayer)
				{
					SoaringInternal.instance.UpdatePlayerData(parsed_data);
				}
				SoaringInternal.instance.SetSoaringInternalData(parsed_data);
				ExtractTimestamp(parsed_data);
				HandleDelegateCallback(soaringModuleData.Set(true, null, parsed_data, (SoaringContext)userData));
				break;
			case SCWebQueue.SCWebQueueState.Failed:
				HandleDelegateCallback(soaringModuleData.Set(false, error, null, (SoaringContext)userData));
				break;
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("SoaringModule:" + ModuleName() + ": Error: " + ex.Message + ": Stack: " + ex.StackTrace, LogType.Warning);
			state = SCWebQueue.SCWebQueueState.Failed;
			HandleDelegateCallback(soaringModuleData.Set(false, ex.Message, null, (SoaringContext)userData));
		}
		ReturnModuledata(soaringModuleData);
		return state == SCWebQueue.SCWebQueueState.Finished;
	}

	protected void ExtractTimestamp(SoaringDictionary data)
	{
		if (data == null)
		{
			return;
		}
		SoaringDictionary soaringDictionary = (SoaringDictionary)data.objectWithKey("serverTime");
		if (soaringDictionary == null)
		{
			return;
		}
		SoaringValue soaringValue = soaringDictionary.soaringValue("timestamp");
		if (soaringValue != null)
		{
			long num = soaringValue;
			if (num > 0)
			{
				SoaringTime.UpdateServerTime(num);
			}
		}
	}

	public virtual void HandleDelegateCallback(SoaringModuleData data)
	{
		SoaringInternal.Delegate.OnComponentFinished(data.state, ModuleName(), data.error, data.data, data.context);
	}
}
