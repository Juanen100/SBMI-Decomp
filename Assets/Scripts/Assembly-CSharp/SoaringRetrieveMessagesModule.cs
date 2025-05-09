public class SoaringRetrieveMessagesModule : SoaringModule
{
	public override string ModuleName()
	{
		return "retrieveUnreadMessages";
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary data2 = SCQueueTools.CreateMessage(ModuleName(), null, callData);
		PushCorePostDataToQueue(data2, 0, context, true);
	}

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
		SoaringArray soaringArray = null;
		if (moduleData.data == null)
		{
			moduleData.state = false;
		}
		if (moduleData.state)
		{
			SoaringArray soaringArray2 = (SoaringArray)moduleData.data.objectWithKey("messages");
			if (soaringArray2 != null)
			{
				soaringArray = new SoaringArray();
				int num = soaringArray2.count();
				for (int i = 0; i < num; i++)
				{
					SoaringDictionary soaringDictionary = (SoaringDictionary)soaringArray2.objectAtIndex(i);
					SoaringDictionary soaringDictionary2 = (SoaringDictionary)soaringDictionary.objectWithKey("header");
					if (soaringDictionary2 != null)
					{
						SoaringMessage soaringMessage = new SoaringMessage();
						soaringMessage.SetCategory(soaringDictionary2.soaringValue("category"));
						soaringMessage.SetMessageID(soaringDictionary2.soaringValue("messageId"));
						soaringMessage.SetSenderID(soaringDictionary2.soaringValue("fromUserId"));
						soaringMessage.SetMessageSendData(soaringDictionary2.soaringValue("sentDate"));
						soaringMessage.SetTextBody(soaringDictionary.soaringValue("body"));
						soaringArray.addObject(soaringMessage);
					}
				}
			}
		}
		SoaringInternal.Delegate.OnCheckMessages(moduleData.state, moduleData.error, soaringArray);
	}
}
