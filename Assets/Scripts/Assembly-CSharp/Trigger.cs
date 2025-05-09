#define ASSERTS_ON
using System.Collections.Generic;

public class Trigger : ITrigger
{
	private static readonly Trigger nullTrigger = new Trigger("nulltrigger", new Dictionary<string, object>(), 0uL);

	public Identity target;

	public Identity dropID;

	private string type;

	private ulong utcTimeStamp;

	private Dictionary<string, object> data;

	public string Type
	{
		get
		{
			return type;
		}
	}

	public Dictionary<string, object> Data
	{
		get
		{
			return data;
		}
	}

	public ulong TimeStamp
	{
		get
		{
			return utcTimeStamp;
		}
	}

	public static Trigger Null
	{
		get
		{
			return nullTrigger;
		}
	}

	public Trigger(string type, Dictionary<string, object> data)
		: this(type, data, 0uL)
	{
	}

	public Trigger(string type, Dictionary<string, object> data, ulong utcTimeStamp, Identity target = null, Identity dropID = null)
	{
		TFUtils.Assert(type != null && type != string.Empty, "Must specify a type for triggers.");
		this.type = type;
		this.data = data;
		this.utcTimeStamp = utcTimeStamp;
		this.target = target;
		this.dropID = dropID;
	}

	public Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["type"] = type;
		dictionary["utcTimeStamp"] = utcTimeStamp;
		dictionary["data"] = data;
		if (target != null)
		{
			dictionary["target"] = target.Describe();
		}
		if (dropID != null)
		{
			dictionary["dropID"] = dropID.Describe();
		}
		return dictionary;
	}

	public static ITrigger FromDict(Dictionary<string, object> dict)
	{
		string text = (string)dict["type"];
		ulong num = TFUtils.LoadUlong(dict, "utcTimeStamp");
		Dictionary<string, object> dictionary = TFUtils.LoadDict(dict, "data");
		Identity identity = ((!dict.ContainsKey("target")) ? null : new Identity((string)dict["target"]));
		Identity identity2 = ((!dict.ContainsKey("dropID")) ? null : new Identity((string)dict["dropID"]));
		return new Trigger(text, dictionary, num, identity, identity2);
	}

	public override string ToString()
	{
		return "Trigger:(type=" + type + ", data=" + TFUtils.DebugDictToString(data) + ")";
	}
}
