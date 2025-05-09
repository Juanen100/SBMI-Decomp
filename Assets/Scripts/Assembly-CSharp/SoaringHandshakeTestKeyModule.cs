internal class SoaringHandshakeTestKeyModule : SoaringModule
{
	public override string ModuleName()
	{
		return "handshake_pt2";
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

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
		if (moduleData.data == null)
		{
			moduleData.state = false;
		}
		bool flag = false;
		if (moduleData.state)
		{
			string text = moduleData.data.soaringValue("finish");
			if (!string.IsNullOrEmpty(text))
			{
				text = text.Replace("\n", "\\n");
				SoaringDictionary soaringDictionary = new SoaringDictionary(2);
				soaringDictionary.addValue(text, "finish");
				soaringDictionary.addValue(SoaringEncryption.SID, "sid");
				flag = SoaringInternal.instance.CallModule("handshake_pt3", soaringDictionary, moduleData.context);
			}
		}
		if (!flag)
		{
			SoaringInternal.instance.TriggerOfflineMode(true);
			if (!SoaringInternal.instance.IsInitialized())
			{
				SoaringInternal.instance.HandleFinalGameInitialization(false);
			}
			else
			{
				SoaringInternal.instance.HandleStashedCalls();
			}
		}
	}
}
