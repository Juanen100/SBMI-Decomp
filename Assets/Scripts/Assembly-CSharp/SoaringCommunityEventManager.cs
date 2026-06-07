using System;
using MTools;

public class SoaringCommunityEventManager
{
	private MDictionary m_pEvents;

	public static event Action<bool, SoaringError, SoaringDictionary, SoaringContext> SetValueFinished
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<bool, SoaringError, SoaringDictionary, SoaringContext> GetValueFinished
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event Action<bool, SoaringError, SoaringDictionary, SoaringContext> AquireGiftFinished
	{
		add
		{
		}
		remove
		{
		}
	}

	public SoaringCommunityEvent GetEvent(string sEventID)
	{
		return null;
	}

	public void _HandleSetValueFinished(bool bSuccess, SoaringError pError, SoaringDictionary pData, SoaringContext pContext)
	{
	}

	public void _HandleGetValueFinished(bool bSuccess, SoaringError pError, SoaringDictionary pData, SoaringContext pContext)
	{
	}

	public void _HandleAquireGiftFinished(bool bSuccess, SoaringError pError, SoaringDictionary pData, SoaringContext pContext)
	{
	}

	public void _HandleValidateUpsightRewardFinished(bool bSuccess, SoaringError pError, SoaringDictionary pData, SoaringContext pContext)
	{
	}

	private void AddEvent(string sEventID, SoaringDictionary pData)
	{
	}

	private void RemoveEvent(string sEventID)
	{
	}
}
