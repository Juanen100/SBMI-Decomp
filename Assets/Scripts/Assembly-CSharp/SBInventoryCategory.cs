using System.Collections.Generic;
using MiniJSON;

public class SBInventoryCategory : SBTabCategory
{
	private string name;

	private string label;

	private string type;

	private string texture;

	private int numItems;

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
			return -1;
		}
		set
		{
		}
	}

	public bool MicroEventOnly
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public int NumItems
	{
		get
		{
			return numItems;
		}
		set
		{
			numItems = value;
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

	public SBInventoryCategory(Dictionary<string, object> cat)
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
	}

	public override string ToString()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["name"] = name;
		dictionary["type"] = type;
		dictionary["texture"] = texture;
		dictionary["label"] = label;
		return Json.Serialize(dictionary);
	}
}
