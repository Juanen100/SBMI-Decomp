using System;
using System.Collections.Generic;
using MiniJSON;

public class SBMarketCategory : SBTabCategory
{
	private string name;

	private string type;

	private string texture;

	private string deltaDNAName;

	private int[] dids;

	private string label;

	private int microEventDID;

	private bool microEventOnly;

	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
		}
	}

	public string DeltaDNAName
	{
		get
		{
			return Catalog.ConvertTypeToDeltaDNAType(Name);
		}
	}

	public string Type
	{
		get
		{
			return type;
		}
		set
		{
			type = value;
		}
	}

	public string Texture
	{
		get
		{
			return texture;
		}
		set
		{
			texture = value;
		}
	}

	public int MicroEventDID
	{
		get
		{
			return microEventDID;
		}
		set
		{
			microEventDID = value;
		}
	}

	public bool MicroEventOnly
	{
		get
		{
			return microEventOnly;
		}
		set
		{
			microEventOnly = value;
		}
	}

	public string Label
	{
		get
		{
			return label;
		}
		set
		{
			label = value;
		}
	}

	public int[] Dids
	{
		get
		{
			return dids;
		}
		set
		{
			dids = value;
		}
	}

	public SBMarketCategory(Dictionary<string, object> cat)
	{
		name = (string)cat["name"];
		if (cat.ContainsKey("display.material"))
		{
			texture = (string)cat["display.material"];
		}
		if (cat.ContainsKey("type"))
		{
			type = (string)cat["type"];
		}
		label = (string)cat["label"];
		List<int> list = ((List<object>)cat["dids"]).ConvertAll((object x) => Convert.ToInt32(x));
		if (cat.ContainsKey("micro_event_did"))
		{
			microEventDID = TFUtils.LoadInt(cat, "micro_event_did");
		}
		else
		{
			microEventDID = -1;
		}
		if (cat.ContainsKey("event_only"))
		{
			microEventOnly = TFUtils.LoadBool(cat, "event_only");
		}
		else
		{
			microEventOnly = false;
		}
		dids = list.ToArray();
	}

	public override string ToString()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["name"] = name;
		dictionary["type"] = type;
		dictionary["dids"] = dids;
		dictionary["texture"] = texture;
		dictionary["label"] = label;
		return Json.Serialize(dictionary);
	}
}
