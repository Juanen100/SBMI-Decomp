using System;

internal class SoaringDelegateArray : SoaringDelegate
{
	private SoaringArray<SoaringDelegate> mDelegateArray;

	public SoaringDelegateArray()
	{
		mDelegateArray = new SoaringArray<SoaringDelegate>(2);
	}

	public SoaringArray<SoaringDelegate> Modules()
	{
		SoaringArray<SoaringDelegate> soaringArray = new SoaringArray<SoaringDelegate>(mDelegateArray.count());
		for (int i = 0; i < mDelegateArray.count(); i++)
		{
			soaringArray.addObject(mDelegateArray.objectAtIndex(i));
		}
		return soaringArray;
	}

	public void RegisterDelegate(SoaringDelegate del)
	{
		if (del == null)
		{
			return;
		}
		Type type = del.GetType();
		int num = -1;
		int num2 = mDelegateArray.count();
		for (int i = 0; i < num2; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate == null)
			{
				num = i;
			}
			else if (del == soaringDelegate || type == soaringDelegate.GetType())
			{
				return;
			}
		}
		if (num == -1)
		{
			mDelegateArray.addObject(del);
		}
		else
		{
			mDelegateArray.setObjectAtIndex(del, num);
		}
	}

	public void UnregisterDelegate(SoaringDelegate del)
	{
		if (del == null)
		{
			return;
		}
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null && del == soaringDelegate)
			{
				mDelegateArray.removeObjectAtIndex(i);
				break;
			}
		}
	}

	public void UnregisterDelegate(Type type)
	{
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null && type == soaringDelegate.GetType())
			{
				mDelegateArray.removeObjectAtIndex(i);
				break;
			}
		}
	}

	public bool UseMainResponder(SoaringContext context)
	{
		if (context == null)
		{
			return false;
		}
		if (context.Responder == null)
		{
			return false;
		}
		return true;
	}

	public override void InternetStateChange(bool state)
	{
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.InternetStateChange(state);
			}
		}
	}

	public override void OnInitializing(bool success, SoaringError error, SoaringDictionary data)
	{
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnInitializing(success, error, data);
			}
		}
	}

	public override void OnAuthorize(bool success, SoaringError error, SoaringPlayer player, SoaringContext context)
	{
		if (UseMainResponder(context))
		{
			context.Responder.OnAuthorize(success, error, player, context);
			return;
		}
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnAuthorize(success, error, player, context);
			}
		}
	}

	public override void OnLookupUser(bool success, SoaringError error, SoaringContext context)
	{
		if (UseMainResponder(context))
		{
			context.Responder.OnLookupUser(success, error, context);
			return;
		}
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnLookupUser(success, error, context);
			}
		}
	}

	public override void OnGenerateUserName(bool success, SoaringError error, string nextTag, SoaringContext context)
	{
		if (UseMainResponder(context))
		{
			context.Responder.OnGenerateUserName(success, error, nextTag, context);
			return;
		}
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnGenerateUserName(success, error, nextTag, context);
			}
		}
	}

	public override void OnRegisterUser(bool success, SoaringError error, SoaringPlayer player, SoaringContext context)
	{
		if (UseMainResponder(context))
		{
			context.Responder.OnRegisterUser(success, error, player, context);
			return;
		}
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnRegisterUser(success, error, player, context);
			}
		}
	}

	public override void OnRetrieveUserProfile(bool succes, SoaringError error, SoaringUser user, SoaringContext context)
	{
		if (UseMainResponder(context))
		{
			context.Responder.OnRetrieveUserProfile(succes, error, user, context);
			return;
		}
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnRetrieveUserProfile(succes, error, user, context);
			}
		}
	}

	public override void OnUpdatingUserProfile(bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
		if (UseMainResponder(context))
		{
			context.Responder.OnUpdatingUserProfile(success, error, data, context);
			return;
		}
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnUpdatingUserProfile(success, error, data, context);
			}
		}
	}

	public override void OnSavingSessionData(bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
		if (UseMainResponder(context))
		{
			context.Responder.OnSavingSessionData(success, error, data, context);
			return;
		}
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnSavingSessionData(success, error, data, context);
			}
		}
	}

	public override void OnRequestingSessionData(bool success, SoaringError error, SoaringArray session_data, SoaringDictionary raw_data, SoaringContext context)
	{
		if (UseMainResponder(context))
		{
			context.Responder.OnRequestingSessionData(success, error, session_data, raw_data, context);
			return;
		}
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnRequestingSessionData(success, error, session_data, raw_data, context);
			}
		}
	}

	public override void OnRetrieveInvitationCode(bool success, SoaringError error, string invite_code)
	{
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnRetrieveInvitationCode(success, error, invite_code);
			}
		}
	}

	public override void OnFindUser(bool success, SoaringError error, SoaringUser[] users, SoaringContext context)
	{
		if (UseMainResponder(context))
		{
			context.Responder.OnFindUser(success, error, users, context);
			return;
		}
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnFindUser(success, error, users, context);
			}
		}
	}

	public override void OnRequestFriend(bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
		if (UseMainResponder(context))
		{
			context.Responder.OnRequestFriend(success, error, data, context);
			return;
		}
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnRequestFriend(success, error, data, context);
			}
		}
	}

	public override void OnRemoveFriend(bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
		if (UseMainResponder(context))
		{
			context.Responder.OnRemoveFriend(success, error, data, context);
			return;
		}
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnRemoveFriend(success, error, data, context);
			}
		}
	}

	public override void OnApplyInviteCode(bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
		if (UseMainResponder(context))
		{
			context.Responder.OnApplyInviteCode(success, error, data, context);
			return;
		}
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnApplyInviteCode(success, error, data, context);
			}
		}
	}

	public override void OnUpdateFriendList(bool success, SoaringError error, SoaringUser[] users, SoaringContext context)
	{
		if (UseMainResponder(context))
		{
			context.Responder.OnUpdateFriendList(success, error, users, context);
			return;
		}
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnUpdateFriendList(success, error, users, context);
			}
		}
	}

	public override void OnComponentFinished(bool success, string module, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
		if (UseMainResponder(context))
		{
			context.Responder.OnComponentFinished(success, module, error, data, context);
			return;
		}
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnComponentFinished(success, module, error, data, context);
			}
		}
	}

	public override void OnCheckUserRewards(bool success, SoaringError error, SoaringArray rewards)
	{
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnCheckUserRewards(success, error, rewards);
			}
		}
	}

	public override void OnRedeemUserReward(bool success, SoaringError error, SoaringDictionary data)
	{
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnRedeemUserReward(success, error, data);
			}
		}
	}

	public override void OnServerTimeUpdated(bool success, SoaringError error, long timestamp, SoaringContext context)
	{
		if (UseMainResponder(context))
		{
			context.Responder.OnServerTimeUpdated(success, error, timestamp, context);
			return;
		}
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnServerTimeUpdated(success, error, timestamp, context);
			}
		}
	}

	public override void OnCheckMessages(bool success, SoaringError error, SoaringArray messages)
	{
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnCheckMessages(success, error, messages);
			}
		}
	}

	public override void OnSendMessage(bool success, SoaringError error, SoaringMessage message)
	{
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnSendMessage(success, error, message);
			}
		}
	}

	public override void OnMessageStateChanged(bool success, SoaringError error, SoaringDictionary data)
	{
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnMessageStateChanged(success, error, data);
			}
		}
	}

	public override void OnFileDownloadUpdate(SoaringState state, SoaringError error, object data, SoaringContext context)
	{
		if (UseMainResponder(context))
		{
			context.Responder.OnFileDownloadUpdate(state, error, data, context);
			return;
		}
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnFileDownloadUpdate(state, error, data, context);
			}
		}
	}

	public override void OnFileVersionsUpdated(SoaringState state, SoaringError error, object data)
	{
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnFileVersionsUpdated(state, error, data);
			}
		}
	}

	public override void OnBlockGameSession(bool forceBlock, float version, float minvVer, float maxVer, string message)
	{
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnBlockGameSession(forceBlock, version, minvVer, maxVer, message);
			}
		}
	}

	public override void OnAdServed(bool success, SoaringAdData adData, SoaringAdServerState state, SoaringContext context)
	{
		if (UseMainResponder(context))
		{
			context.Responder.OnAdServed(success, adData, state, context);
			return;
		}
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnAdServed(success, adData, state, context);
			}
		}
	}

	public override void OnPasswordReset(bool success, SoaringError error)
	{
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnPasswordReset(success, error);
			}
		}
	}

	public override void OnPasswordResetConfirmed(bool success, SoaringError error)
	{
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnPasswordResetConfirmed(success, error);
			}
		}
	}

	public override void OnPasswordChanged(bool success, SoaringError error, SoaringContext context)
	{
		if (UseMainResponder(context))
		{
			context.Responder.OnPasswordChanged(success, error, context);
			return;
		}
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnPasswordChanged(success, error, context);
			}
		}
	}

	public override void OnDeviceRegistered(bool success, SoaringError error, SoaringContext context)
	{
		if (UseMainResponder(context))
		{
			context.Responder.OnDeviceRegistered(success, error, context);
			return;
		}
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnDeviceRegistered(success, error, context);
			}
		}
	}

	public override void OnRecieptValidated(bool success, SoaringError error, SoaringContext context)
	{
		if (UseMainResponder(context))
		{
			context.Responder.OnRecieptValidated(success, error, context);
			return;
		}
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnRecieptValidated(success, error, context);
			}
		}
	}

	public override void OnSaveStat(bool success, bool anonymous, SoaringError error, SoaringContext context)
	{
		if (UseMainResponder(context))
		{
			context.Responder.OnSaveStat(success, anonymous, error, context);
			return;
		}
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnSaveStat(success, anonymous, error, context);
			}
		}
	}

	public override void OnPlayerConflict(SoaringPlayerResolver player, SoaringPlayerResolver.SoaringPlayerData platform_player, SoaringPlayerResolver.SoaringPlayerData last_player, SoaringPlayerResolver.SoaringPlayerData device_player, SoaringContext context)
	{
		if (UseMainResponder(context))
		{
			context.Responder.OnPlayerConflict(player, platform_player, last_player, device_player, context);
			return;
		}
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnPlayerConflict(player, platform_player, last_player, device_player, context);
			}
		}
	}

	public override void OnRetrievePurchases(bool success, SoaringError error, SoaringPurchase[] purchases, SoaringContext context)
	{
		if (UseMainResponder(context))
		{
			context.Responder.OnRetrievePurchases(success, error, purchases, context);
			return;
		}
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnRetrievePurchases(success, error, purchases, context);
			}
		}
	}

	public override void OnRetrieveProducts(bool success, SoaringError error, SoaringPurchasable[] purchasables, SoaringContext context)
	{
		if (UseMainResponder(context))
		{
			context.Responder.OnRetrieveProducts(success, error, purchasables, context);
			return;
		}
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnRetrieveProducts(success, error, purchasables, context);
			}
		}
	}

	public override void OnRetrieveCampaign(bool success, SoaringError error, SoaringArray campaigns, SoaringContext context)
	{
		if (UseMainResponder(context))
		{
			context.Responder.OnRetrieveCampaign(success, error, campaigns, context);
			return;
		}
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnRetrieveCampaign(success, error, campaigns, context);
			}
		}
	}

	public override void OnRecievedEvent(SoaringEvents manager, SoaringEvent soaringEv)
	{
		if (soaringEv == null)
		{
			return;
		}
		int num = mDelegateArray.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDelegate soaringDelegate = mDelegateArray[i];
			if (soaringDelegate != null)
			{
				soaringDelegate.OnRecievedEvent(manager, soaringEv);
			}
		}
	}
}
