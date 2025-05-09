using System.Collections.Generic;

public class PlayerInventory
{
	private List<SBInventoryItem> items = new List<SBInventoryItem>();

	public void AddItem(BuildingEntity entity, List<KeyValuePair<int, Identity>> associatedEntities)
	{
		string name = (string)entity.Invariable["name"];
		string filename = (string)entity.Invariable["portrait"];
		SBInventoryItem item = new SBInventoryItem(entity, associatedEntities, "stashed", name, string.Empty, filename, false);
		items.Add(item);
	}

	public void AddAssociatedEntities(Identity entityId, List<KeyValuePair<int, Identity>> associatedEntities)
	{
		foreach (SBInventoryItem item in items)
		{
			if (item.entity.Id.Equals(entityId))
			{
				if (item.associatedEntities == null)
				{
					item.associatedEntities = associatedEntities;
				}
				else
				{
					item.associatedEntities.AddRange(associatedEntities);
				}
				break;
			}
		}
	}

	public bool HasItem(int? did)
	{
		foreach (SBInventoryItem item in items)
		{
			if (item.entity.DefinitionId == did.GetValueOrDefault() && did.HasValue)
			{
				return true;
			}
		}
		return false;
	}

	public bool HasItem(Identity ID)
	{
		foreach (SBInventoryItem item in items)
		{
			if (item.entity.Id.Equals(ID))
			{
				return true;
			}
		}
		return false;
	}

	public int GetNumItems(int? did)
	{
		int num = 0;
		foreach (SBInventoryItem item in items)
		{
			if (item.entity.DefinitionId == did.GetValueOrDefault() && did.HasValue)
			{
				num++;
			}
		}
		return num;
	}

	public int GetNumItems(Identity ID)
	{
		int num = 0;
		foreach (SBInventoryItem item in items)
		{
			if (item.entity.Id.Equals(ID))
			{
				num++;
			}
		}
		return num;
	}

	public List<SBInventoryItem> GetItems()
	{
		return items;
	}

	public Entity RemoveEntity(Identity id, out List<KeyValuePair<int, Identity>> outAssociatedEntities)
	{
		Entity result = null;
		outAssociatedEntities = new List<KeyValuePair<int, Identity>>();
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].entity.Id.Equals(id))
			{
				result = items[i].entity;
				outAssociatedEntities = items[i].associatedEntities;
				items.RemoveAt(i);
				break;
			}
		}
		return result;
	}

	public int GetNumUniqueItems()
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		foreach (SBInventoryItem item in items)
		{
			if (!dictionary.ContainsKey(item.entity.DefinitionId))
			{
				dictionary.Add(item.entity.DefinitionId, 1);
			}
		}
		return dictionary.Count;
	}
}
