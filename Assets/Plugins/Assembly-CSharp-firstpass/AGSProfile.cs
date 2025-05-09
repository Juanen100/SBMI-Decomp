using System.Collections;

public class AGSProfile
{
	public readonly string alias;

	public readonly string playerId;

	private AGSProfile()
	{
		alias = null;
		playerId = null;
		AGSClient.LogGameCircleError("AGSProfile was instantiated without valid playerId and alias information.");
	}

	private AGSProfile(string alias, string playerId)
	{
		this.alias = alias;
		this.playerId = playerId;
	}

	public static AGSProfile fromHashtable(Hashtable profileDataAsHashtable)
	{
		if (profileDataAsHashtable == null)
		{
			return null;
		}
		return new AGSProfile(getStringValue(profileDataAsHashtable, "alias"), getStringValue(profileDataAsHashtable, "playerId"));
	}

	private static string getStringValue(Hashtable hashtable, string key)
	{
		if (hashtable == null)
		{
			return null;
		}
		if (hashtable.Contains(key))
		{
			return hashtable[key].ToString();
		}
		return null;
	}

	public override string ToString()
	{
		return string.Format("alias: {0}, playerId: {1}", alias, playerId);
	}
}
