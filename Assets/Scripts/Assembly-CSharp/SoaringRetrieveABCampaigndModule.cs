public class SoaringRetrieveABCampaigndModule : SoaringModule
{
	public override string ModuleName()
	{
		return "retrieveABCampaignData";
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(data.objectWithKey("authToken"), "authToken");
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", ModuleName(), null, soaringDictionary) + "\n}";
		soaringDictionary.clear();
		soaringDictionary.addValue(text, "data");
		PushCorePostDataToQueue(soaringDictionary, 1, context, false);
	}

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
		SoaringArray soaringArray = null;
		if (moduleData.state && moduleData.data != null)
		{
			soaringArray = (SoaringArray)moduleData.data.objectWithKey("campaigns");
			if (soaringArray != null)
			{
				bool flag = false;
				int num = soaringArray.count();
				SoaringArray soaringArray2 = new SoaringArray(num);
				for (int i = 0; i < num; i++)
				{
					SoaringCampaign soaringCampaign = new SoaringCampaign((SoaringDictionary)soaringArray.objectAtIndex(i));
					if (soaringCampaign.CampaignType == "content")
					{
						SoaringInternal.Campaign = soaringCampaign;
						flag = true;
					}
					soaringArray2.addObject(soaringCampaign);
				}
				soaringArray = soaringArray2;
				if (!flag && num != 0)
				{
					SoaringInternal.Campaign = (SoaringCampaign)soaringArray.objectAtIndex(0);
				}
			}
		}
		SoaringInternal.Delegate.OnRetrieveCampaign(moduleData.state, moduleData.error, soaringArray, moduleData.context);
	}
}
