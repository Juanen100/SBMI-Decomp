public class SoaringCommunityEventDelegate : SoaringDelegate
{
	public override void OnComponentFinished(bool bSuccess, string sModule, SoaringError pError, SoaringDictionary pData, SoaringContext pContext)
	{
		switch (sModule)
		{
		case "setEventValue":
			Soaring.CommunityEventManager._HandleSetValueFinished(bSuccess, pError, pData, pContext);
			break;
		case "getEventValue":
			Soaring.CommunityEventManager._HandleGetValueFinished(bSuccess, pError, pData, pContext);
			break;
		case "acquireEventGift":
			Soaring.CommunityEventManager._HandleAquireGiftFinished(bSuccess, pError, pData, pContext);
			break;
		}
	}
}
