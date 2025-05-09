internal class SoaringHandshakeFinishKeyModule : SoaringModule
{
	public override string ModuleName()
	{
		return "handshake_pt3";
	}

	public override int ModuleChannel()
	{
		return 0;
	}

	public override bool ShouldEncryptCall()
	{
		return false;
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", "handshake", data.soaringValue("gameId"), null) + ",\n";
		text = text + "\"data\" : " + callData.ToJsonString() + "\n}";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(text, "data");
		PushCorePostDataToQueue(soaringDictionary, 0, context, false);
	}

	public override void HandleDelegateCallback(SoaringModuleData data)
	{
		if (data.state)
		{
			SoaringInternal.Encryption = (SoaringEncryption)data.context.objectWithKey("encryption");
			if (SoaringInternal.Encryption != null)
			{
				SoaringInternal.Encryption.StartUsingEncryption();
			}
			else
			{
				SoaringInternal.instance.TriggerOfflineMode(true);
			}
		}
		else
		{
			SoaringInternal.instance.TriggerOfflineMode(true);
		}
		if (!SoaringInternal.instance.IsInitialized())
		{
			SoaringInternal.instance.HandleFinalGameInitialization(data.state);
		}
		else
		{
			SoaringInternal.instance.HandleStashedCalls();
		}
		if (data.context != null && data.context.ContextResponder != null)
		{
			data.context.ContextResponder(data.context);
		}
	}
}
