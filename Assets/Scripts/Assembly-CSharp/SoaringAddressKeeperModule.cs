public class SoaringAddressKeeperModule : SoaringModule
{
	public override string ModuleName()
	{
		return "retrieveGameLinks";
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary data2 = SCQueueTools.CreateMessage(ModuleName(), data.soaringValue("gameId"), callData);
		PushCorePostDataToQueue(data2, 0, context, false);
	}

	public override bool ShouldEncryptCall()
	{
		return false;
	}

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
		if (moduleData.data != null)
		{
			SoaringObjectBase soaringObjectBase = moduleData.data.objectWithKey("links");
			if (soaringObjectBase.Type == IsType.Array)
			{
				SoaringArray soaringArray = (SoaringArray)soaringObjectBase;
				int num = soaringArray.count();
				moduleData.data = new SoaringDictionary(num);
				for (int i = 0; i < num; i++)
				{
					SoaringDictionary soaringDictionary = (SoaringDictionary)soaringArray.objectAtIndex(i);
					string text = soaringDictionary.soaringValue("key");
					string text2 = soaringDictionary.soaringValue("url");
					if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
					{
						moduleData.data.addValue(text2, text);
					}
				}
			}
		}
		SoaringInternal.instance.AddressesKeeper.SetSoaringAddressData(moduleData.data);
	}
}
