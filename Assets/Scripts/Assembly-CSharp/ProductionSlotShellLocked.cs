using System;

public class ProductionSlotShellLocked : ProductionSlotShell
{
	public ProductionSlotShellLocked(SBGUIProductionSlot core, Cost purchaseCost, int slotId, Game game)
		: base(null, 0)
	{
	}

	public override void UpdateInfo(BuildingEntity producer, int slot, Action<int> rushHandler, Game game, Action<int> watchADHandler, bool isAdAvailable)
	{
	}
}
