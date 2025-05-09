using System;
using UnityEngine;

internal static class SCQueueTools
{
	public static SoaringError CheckAndHandleError(string data, ref SoaringDictionary parsed_data)
	{
		if (string.IsNullOrEmpty(data))
		{
			return null;
		}
		parsed_data = ParseMessage(data);
		if (parsed_data == null)
		{
			return new SoaringError(data, -1);
		}
		string text = parsed_data.soaringValue("error_message");
		if (text == null)
		{
			text = string.Empty;
		}
		SoaringObjectBase soaringObjectBase = parsed_data.soaringValue("error_code");
		if (soaringObjectBase == null && string.IsNullOrEmpty(text))
		{
			return null;
		}
		int code = -1;
		if (soaringObjectBase != null)
		{
			code = (SoaringValue)soaringObjectBase;
		}
		return new SoaringError(text, code);
	}

	public static SoaringDictionary CreateMessage(string action, string gameID, SoaringDictionary parameters)
	{
		SoaringDictionary soaringDictionary = null;
		if (parameters == null && string.IsNullOrEmpty(action) && string.IsNullOrEmpty(gameID))
		{
			return null;
		}
		if (parameters == null)
		{
			parameters = new SoaringDictionary(2);
		}
		if (action != null)
		{
			parameters.addValue(action, "name");
		}
		if (gameID != null)
		{
			parameters.addValue(gameID, "gameId");
		}
		string text = "{\n\"action\" : " + parameters.ToJsonString() + "\n}";
		soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(text, "data");
		return soaringDictionary;
	}

	public static SoaringDictionary CreateJsonDictionary(string header, string action, string gameID, SoaringDictionary parameters)
	{
		string message = null;
		return CreateJsonDictionary(header, action, gameID, parameters, ref message);
	}

	public static SoaringDictionary CreateJsonDictionary(string header, string action, string gameID, SoaringDictionary parameters, ref string message)
	{
		message = CreateJsonMessage(header, action, gameID, parameters);
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(message, "data");
		return soaringDictionary;
	}

	public static string CreateJsonMessage(string header, string action, string gameID, SoaringDictionary parameters)
	{
		if (string.IsNullOrEmpty(header))
		{
			return null;
		}
		if (parameters == null)
		{
			parameters = new SoaringDictionary(2);
		}
		string text = "\"" + header + "\" : ";
		if (action != null)
		{
			parameters.addValue(action, "name");
		}
		if (gameID != null)
		{
			parameters.addValue(gameID, "gameId");
		}
		return text + parameters.ToJsonString();
	}

	public static SoaringDictionary ParseMessage(string message)
	{
		SoaringDictionary soaringDictionary = null;
		try
		{
			soaringDictionary = new SoaringDictionary(message);
			if (soaringDictionary.count() == 0)
			{
				soaringDictionary = null;
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("SCQueueTools: " + ex.Message, LogType.Warning);
			soaringDictionary = null;
		}
		return soaringDictionary;
	}

	public static SoaringArray<SoaringUser> ParseUsers(SoaringArray data, bool mixedUsers)
	{
		if (data == null)
		{
			return new SoaringArray<SoaringUser>(0);
		}
		SoaringArray<SoaringUser> soaringArray = new SoaringArray<SoaringUser>(data.count());
		int num = data.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDictionary soaringDictionary = (SoaringDictionary)data.objectAtIndex(i);
			if (mixedUsers)
			{
				string text = soaringDictionary.soaringValue("isFriend");
				bool flag = false;
				if (!string.IsNullOrEmpty(text))
				{
					text = text.ToLower();
					if (text == "true")
					{
						flag = true;
					}
				}
				SoaringUser soaringUser = null;
				soaringUser = ((!flag) ? new SoaringUser() : new SoaringFriend());
				soaringUser.SetUserData(soaringDictionary);
				soaringArray.addObject(soaringUser);
			}
			else
			{
				SoaringUser soaringUser2 = new SoaringFriend();
				soaringUser2.SetUserData(soaringDictionary);
				soaringArray.addObject(soaringUser2);
			}
		}
		return soaringArray;
	}
}
