#define ASSERTS_ON
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
			return EntityType.BUILDING;
		}
	}

	public List<Entity> Annexes
	{
		get
		{
			return (List<Entity>)Variable["annexes"];
		}
	}

	public List<int> ResidentDids
	{
		get
		{
			return (List<int>)Invariable["residents"];
		}
	}

	public int? PetDid
	{
		get
		{
			return (int?)Invariable["pet"];
		}
	}

	public Vector2 PointOfInterestOffset
	{
		get
		{
			return (Vector2)Invariable["point_of_interest"];
		}
	}

	public bool HasResident
	{
		get
		{
			return !Invariable.ContainsKey("residents") || Invariable["residents"] != null;
		}
	}

	public bool CanCraft
	{
		get
		{
			return Invariable.ContainsKey("crafting_menu") && Invariable["crafting_menu"] != null;
		}
	}

	public int CraftMenu
	{
		get
		{
			return (int)Invariable["crafting_menu"];
		}
	}

	public bool ShuntsCrafting
	{
		get
		{
			return (bool)Invariable["shunts_crafting"];
		}
	}

	public bool HasSlots
	{
		get
		{
			return Variable.ContainsKey("crafting_slots") && Slots != -1;
		}
	}

	public int Slots
	{
		get
		{
			TFUtils.Assert(Variable.ContainsKey("crafting_slots"), "Trying to lookup production slots on this entity, but none were assigned. Is there an appropriate production slots file linked to this entity? EntityDid=" + DefinitionId);
			return (int)Variable["crafting_slots"];
		}
		set
		{
			Variable["crafting_slots"] = value;
		}
	}

	public Reward CraftRewards
	{
		get
		{
			return (!Variable.ContainsKey("craft_rewards")) ? null : ((Reward)Variable["craft_rewards"]);
		}
		set
		{
			Variable["craft_rewards"] = value;
		}
	}

	public int TaskSourceFeedDID
	{
		get
		{
			if (Variable.ContainsKey("task_source_feed_did"))
			{
				return (int)Variable["task_source_feed_did"];
			}
			return 0;
		}
		set
		{
			Variable["task_source_feed_did"] = value;
		}
	}

	public bool CanVend
	{
		get
		{
			return Invariable.ContainsKey("vendor_id");
		}
	}

	public string OverrideRewardTexture
	{
		get
		{
			if (Invariable.ContainsKey("crafted_icon"))
			{
				return (string)Invariable["crafted_icon"];
			}
			return null;
		}
	}

	public bool Stashable
	{
		get
		{
			return (bool)Invariable["stashable"];
		}
	}

	public bool Flippable
	{
		get
		{
			return (bool)Invariable["flippable"];
		}
	}

	public int BusyAnnexCount
	{
		get
		{
			return (int)Variable["busy_annex_count"];
		}
		set
		{
			Variable["busy_annex_count"] = value;
		}
	}

	public BuildingEntity(Entity toDecorate)
		: base(toDecorate)
	{
		new StructureDecorator(this);
		new ErectableDecorator(this);
		new ActivatableDecorator(this);
		if (Invariable["product"] != null)
		{
			new PeriodicProductionDecorator(this);
		}
		if (Invariable.ContainsKey("vendor_id"))
		{
			new VendingDecorator(this);
		}
		Variable["annexes"] = new List<Entity>();
	}

	public void RegisterAnnex(Entity annex)
	{
		List<Entity> list = (List<Entity>)Variable["annexes"];
		list.Add(annex);
	}

	public void CraftingComplete(Reward reward)
	{
		Reward craftRewards = CraftRewards;
		if (craftRewards != null)
		{
			CraftRewards = craftRewards + reward;
		}
		else
		{
			CraftRewards = reward;
		}
	}

	public void ClearCraftingRewards()
	{
		if (Variable.ContainsKey("craft_rewards"))
		{
			Variable.Remove("craft_rewards");
		}
		else if (Variable.ContainsKey("craft.rewards"))
		{
			Variable.Remove("craft.rewards");
		}
	}

	public void AddCraftingSlot()
	{
		Variable["crafting_slots"] = Slots + 1;
	}
}
