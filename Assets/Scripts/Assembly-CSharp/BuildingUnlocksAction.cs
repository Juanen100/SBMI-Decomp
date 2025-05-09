using System.Collections.Generic;

public class BuildingUnlocksAction : PersistedTriggerableAction
{
	public const string UNLOCK_BUILDING = "ub";

	public List<int> buildings;

	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	public BuildingUnlocksAction(List<int> buildings)
		: base("ub", Identity.Null())
	{
		this.buildings = buildings;
	}

	public new static BuildingUnlocksAction FromDict(Dictionary<string, object> data)
	{
		List<int> list = TFUtils.LoadList<int>(data, "building_unlocks");
		return new BuildingUnlocksAction(list);
	}

	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["building_unlocks"] = TFUtils.CloneAndCastList<int, object>(buildings);
		return dictionary;
	}

	public override void Apply(Game game, ulong utcNow)
	{
		foreach (int building in buildings)
		{
			game.buildingUnlockManager.UnlockBuilding(building);
		}
		base.Apply(game, utcNow);
	}

	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["farm"];
		if (!dictionary.ContainsKey("building_unlocks"))
		{
			dictionary["building_unlocks"] = new List<object>();
		}
		List<object> list = (List<object>)dictionary["building_unlocks"];
		foreach (int building in buildings)
		{
			if (!list.Contains(building))
			{
				list.Add(building);
			}
		}
		base.Confirm(gameState);
	}

	public virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}

	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return triggerable.BuildTrigger(GetType().ToString(), AddMoreDataToTrigger);
	}
}
