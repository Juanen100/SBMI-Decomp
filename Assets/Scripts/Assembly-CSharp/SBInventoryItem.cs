using System;
using System.Collections.Generic;

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
	}

	public override string ToString()
	{
		return null;
	}

	public int CompareTo(SBInventoryItem rhs)
	{
		return 0;
	}
}
