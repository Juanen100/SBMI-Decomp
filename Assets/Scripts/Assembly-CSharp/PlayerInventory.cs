using System.Collections.Generic;

public class PlayerInventory
{
	private List<SBInventoryItem> items;

	public void AddItem(BuildingEntity entity, List<KeyValuePair<int, Identity>> associatedEntities)
	{
	}

	public void AddAssociatedEntities(Identity entityId, List<KeyValuePair<int, Identity>> associatedEntities)
	{
	}

	public bool HasItem(int? did)
	{
		return false;
	}

	public bool HasItem(Identity ID)
	{
		return false;
	}

	public int GetNumItems(int? did)
	{
		return 0;
	}

	public int GetNumItems(Identity ID)
	{
		return 0;
	}

	public List<SBInventoryItem> GetItems()
	{
		return null;
	}

	public Entity RemoveEntity(Identity id, out List<KeyValuePair<int, Identity>> outAssociatedEntities)
	{
		outAssociatedEntities = null;
		return null;
	}

	public int GetNumUniqueItems()
	{
		return 0;
	}
}
