using System.Collections.Generic;

public class Trigger : ITrigger
{
	private static readonly Trigger nullTrigger;

	public Identity target;

	public Identity dropID;

	private string type;

	private ulong utcTimeStamp;

	private Dictionary<string, object> data;

	public string Type
	{
		get
		{
			return null;
		}
	}

	public Dictionary<string, object> Data
	{
		get
		{
			return null;
		}
	}

	public ulong TimeStamp
	{
		get
		{
			return 0uL;
		}
	}

	public static Trigger Null
	{
		get
		{
			return null;
		}
	}

	public Trigger(string type, Dictionary<string, object> data)
	{
	}

	public Trigger(string type, Dictionary<string, object> data, ulong utcTimeStamp, Identity target = null, Identity dropID = null)
	{
	}

	public Dictionary<string, object> ToDict()
	{
		return null;
	}

	public static ITrigger FromDict(Dictionary<string, object> dict)
	{
		return null;
	}

	public override string ToString()
	{
		return null;
	}
}
