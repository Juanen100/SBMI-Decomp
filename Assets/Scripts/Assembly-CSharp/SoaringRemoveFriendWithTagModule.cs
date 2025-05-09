using System;
using UnityEngine;

public class SoaringRemoveFriendWithTagModule : SoaringModule
{
	private bool AddingFriend;

	private string AuthToken;

	private string UserID;

	private string UserTag;

	public override string ModuleName()
	{
		return "removeFriendshipWithTag";
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		if (data == null && callData == null)
		{
			AddingFriend = true;
		}
		else
		{
			AddingFriend = false;
		}
		if (!AddingFriend)
		{
			AuthToken = data.soaringValue("authToken");
			UserTag = callData.soaringValue("tag");
			soaringDictionary.addValue(AuthToken, "authToken");
			string text = "{\n" + SCQueueTools.CreateJsonMessage("action", "searchUsers", null, soaringDictionary);
			text += ",\n";
			soaringDictionary.clear();
			soaringDictionary.addValue("tag", "type");
			soaringDictionary.addValue(UserTag, "value");
			text = text + SCQueueTools.CreateJsonMessage("data", null, null, soaringDictionary) + "\n}";
			soaringDictionary.clear();
			soaringDictionary.addValue(text, "data");
		}
		else
		{
			soaringDictionary.addValue(AuthToken, "authToken");
			string text2 = "{\n" + SCQueueTools.CreateJsonMessage("action", "removeFriendship", null, soaringDictionary);
			text2 += ",\n";
			soaringDictionary.clear();
			soaringDictionary.addValue(UserID, "userId");
			text2 = text2 + SCQueueTools.CreateJsonMessage("data", null, null, soaringDictionary) + "\n}";
			soaringDictionary.clear();
			soaringDictionary.addValue(text2, "data");
		}
		PushCorePostDataToQueue(soaringDictionary, 0, context, true);
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
			}
			SoaringDebug.Log((string)data);
			switch (state)
			{
			case SCWebQueue.SCWebQueueState.Finished:
				if (!AddingFriend)
				{
					AddingFriend = true;
					parsed_data = (SoaringDictionary)parsed_data.objectWithKey("data");
					SoaringArray data2 = (SoaringArray)parsed_data.objectWithKey("users");
					SoaringArray<SoaringUser> soaringArray = SCQueueTools.ParseUsers(data2, true);
					if (soaringArray == null)
					{
						HandleDelegateCallback(soaringModuleData.Set(false, "No Users Found", null, (SoaringContext)userData));
						break;
					}
					if (soaringArray.count() == 0)
					{
						HandleDelegateCallback(soaringModuleData.Set(false, "No Users Found", null, (SoaringContext)userData));
						break;
					}
					SoaringUser soaringUser = soaringArray.objectAtIndex(0);
					UserID = soaringUser.UserID;
					CallModule(null, null, (SoaringContext)userData);
				}
				else
				{
					HandleDelegateCallback(soaringModuleData.Set(true, null, parsed_data, (SoaringContext)userData));
					AddingFriend = false;
					Soaring.UpdateFriendsListWithLastSettings();
				}
				break;
			case SCWebQueue.SCWebQueueState.Failed:
				HandleDelegateCallback(soaringModuleData.Set(false, error, null, (SoaringContext)userData));
				break;
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("SoaringModule:" + ModuleName() + ": Error: " + ex.Message, LogType.Warning);
			state = SCWebQueue.SCWebQueueState.Failed;
			HandleDelegateCallback(soaringModuleData.Set(false, ex.Message, null, (SoaringContext)userData));
			AddingFriend = false;
		}
		ReturnModuledata(soaringModuleData);
		return state == SCWebQueue.SCWebQueueState.Finished;
	}

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
		Soaring.Delegate.OnRemoveFriend(moduleData.state, moduleData.error, moduleData.data, moduleData.context);
	}
}
