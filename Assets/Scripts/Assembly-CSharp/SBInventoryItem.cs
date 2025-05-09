using System;
using System.Collections.Generic;
using MiniJSON;

public class SBInventoryItem : IComparable<SBInventoryItem>
{
	public Entity entity;

	public List<KeyValuePair<int, Identity>> associatedEntities;

	public string itemType;

	public string displayName;

	public string iconFilename;

	public bool discardable;

	public string description;

	public string movieFileName;

	public SBInventoryItem(Entity entity, List<KeyValuePair<int, Identity>> associatedEntities, string type, string name, string description, string filename, bool isDiscardable, string movieFileName = null)
	{
		this.entity = entity;
		this.associatedEntities = associatedEntities;
		displayName = name;
		iconFilename = filename;
		this.description = description;
		discardable = isDiscardable;
		itemType = type;
		this.movieFileName = movieFileName;
	}

	public override string ToString()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["entity.did"] = entity.DefinitionId;
		dictionary["itemType"] = itemType;
		dictionary["displayName"] = displayName;
		dictionary["iconFilename"] = iconFilename;
		dictionary["description"] = description;
		dictionary["discardable"] = discardable;
		dictionary["movieFileName"] = movieFileName;
		return Json.Serialize(dictionary);
	}

	public int CompareTo(SBInventoryItem rhs)
	{
		string text = Language.Get(displayName);
		string strB = Language.Get(rhs.displayName);
		return text.CompareTo(strB);
	}
}
