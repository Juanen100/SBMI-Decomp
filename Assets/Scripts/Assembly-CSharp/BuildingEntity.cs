using System.Collections.Generic;
using UnityEngine;

public class BuildingEntity : EntityDecorator
{
	public const string TYPE = "building";

	public const string ANNEXES = "annexes";

	public const string TASKBOOK_ID = "taskbook_id";

	public const string SHUNTS_CRAFTING = "shunts_crafting";

	public const string CRAFTING_SLOTS = "crafting_slots";

	public const string RESIDENTS = "residents";

	public override EntityType Type
	{
		get
		{
			return default(EntityType);
		}
	}

	public List<Entity> Annexes
	{
		get
		{
			return null;
		}
	}

	public List<int> ResidentDids
	{
		get
		{
			return null;
		}
	}

	public int? PetDid
	{
		get
		{
			return null;
		}
	}

	public Vector2 PointOfInterestOffset
	{
		get
		{
			return default(Vector2);
		}
	}

	public bool HasResident
	{
		get
		{
			return false;
		}
	}

	public bool CanCraft
	{
		get
		{
			return false;
		}
	}

	public int CraftMenu
	{
		get
		{
			return 0;
		}
	}

	public bool ShuntsCrafting
	{
		get
		{
			return false;
		}
	}

	public bool HasSlots
	{
		get
		{
			return false;
		}
	}

	public int Slots
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public Reward CraftRewards
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public int TaskSourceFeedDID
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public bool CanVend
	{
		get
		{
			return false;
		}
	}

	public string OverrideRewardTexture
	{
		get
		{
			return null;
		}
	}

	public bool Stashable
	{
		get
		{
			return false;
		}
	}

	public bool Flippable
	{
		get
		{
			return false;
		}
	}

	public int BusyAnnexCount
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public BuildingEntity(Entity toDecorate)
		: base(null)
	{
	}

	public void RegisterAnnex(Entity annex)
	{
	}

	public void CraftingComplete(Reward reward)
	{
	}

	public void ClearCraftingRewards()
	{
	}

	public void AddCraftingSlot()
	{
	}
}
