using System.Collections.Generic;

public class BuildingUnlockManager
{
	private List<int> unlockedBuildings;

	public BuildingUnlockManager()
	{
		unlockedBuildings = new List<int>();
	}

	public void UnlockBuilding(int buildingDid)
	{
		if (!unlockedBuildings.Contains(buildingDid))
		{
			unlockedBuildings.Add(buildingDid);
		}
	}

	public bool CheckBuildingUnlock(int buildingdid)
	{
		if (unlockedBuildings.Contains(buildingdid))
		{
			return true;
		}
		return false;
	}
}
