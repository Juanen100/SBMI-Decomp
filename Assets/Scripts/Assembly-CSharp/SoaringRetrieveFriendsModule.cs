using System;
using UnityEngine;

public class SoaringRetrieveFriendsModule : SoaringModule
{
	private const string kFriendList = "_FriendList";

	public override string ModuleName()
	{
		return "retrieveFriendsList";
	}

	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(data.objectWithKey("authToken"), "authToken");
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", ModuleName(), null, soaringDictionary);
		text += ",\n";
		callData.removeObjectWithKey("authToken");
		callData.removeObjectWithKey("gameId");
		text = text + SCQueueTools.CreateJsonMessage("data", null, null, callData) + "\n}";
		soaringDictionary.clear();
		soaringDictionary.addValue(text, "data");
		SoaringArray soaringArray = (SoaringArray)context.objectWithKey("_FriendList");
		if (soaringArray == null)
		{
			context.addValue(new SoaringArray(), "_FriendList");
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
				parsed_data = (SoaringDictionary)parsed_data.objectWithKey("data");
				HandleDelegateCallback(soaringModuleData.Set(true, null, parsed_data, (SoaringContext)userData));
				break;
			case SCWebQueue.SCWebQueueState.Failed:
				HandleDelegateCallback(soaringModuleData.Set(false, error, null, (SoaringContext)userData));
				break;
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("SoaringModule:" + ModuleName() + ": Error: " + ex.Message + "\nStack: " + ex.StackTrace, LogType.Warning);
			state = SCWebQueue.SCWebQueueState.Failed;
			HandleDelegateCallback(soaringModuleData.Set(false, ex.Message, null, (SoaringContext)userData));
		}
		ReturnModuledata(soaringModuleData);
		return state == SCWebQueue.SCWebQueueState.Finished;
	}

	public override void HandleDelegateCallback(SoaringModuleData moduleData)
	{
		SoaringArray soaringArray = null;
		SoaringArray<SoaringUser> soaringArray2 = null;
		int num = 0;
		int num2 = 0;
		SoaringArray soaringArray3 = null;
		if (moduleData.data != null)
		{
			soaringArray = (SoaringArray)moduleData.data.objectWithKey("friends");
			num = moduleData.data.soaringValue("friendsCount");
			if (soaringArray != null)
			{
				soaringArray2 = SCQueueTools.ParseUsers(soaringArray, false);
			}
		}
		if (soaringArray2 != null)
		{
			num2 = soaringArray2.count();
		}
		else
		{
			soaringArray2 = new SoaringArray<SoaringUser>();
		}
		if (moduleData.context != null)
		{
			soaringArray3 = (SoaringArray)moduleData.context.objectWithKey("_FriendList");
		}
		if (soaringArray3 == null)
		{
			soaringArray3 = new SoaringArray();
		}
		for (int i = 0; i < num2; i++)
		{
			SoaringUser obj = soaringArray2[i];
			soaringArray3.addObject(obj);
		}
		if (soaringArray3.count() < num && num != 0)
		{
			Soaring.UpdateFriendsListWithLastSettings(soaringArray3.count(), num, moduleData.context);
			return;
		}
		int num3 = 0;
		int num4 = soaringArray3.count();
		for (int j = 0; j < num4; j++)
		{
			SoaringUser soaringUser = (SoaringUser)soaringArray3.objectAtIndex(j);
			if (soaringUser != null)
			{
				num3++;
			}
		}
		SoaringArray<SoaringUser> soaringArray4 = new SoaringArray<SoaringUser>(num3);
		for (int k = 0; k < num4; k++)
		{
			SoaringUser soaringUser2 = (SoaringUser)soaringArray3.objectAtIndex(k);
			if (soaringUser2 != null)
			{
				soaringArray4.addObject(soaringUser2);
			}
		}
		SoaringUser[] array = soaringArray4.array();
		if (array == null)
		{
			array = new SoaringUser[0];
		}
		SoaringInternal.instance.Player.SetFriendsData(soaringArray4);
		SoaringInternal.Delegate.OnUpdateFriendList(moduleData.state, moduleData.error, array, moduleData.context);
	}
}
