using System;

public class ProductionSlotShellUnavailable : ProductionSlotShell
{
	public ProductionSlotShellUnavailable(SBGUIProductionSlot core, int slotId)
		: base(null, 0)
	{
	}

	public override void UpdateInfo(BuildingEntity producer, int slot, Action<int> rushHandler, Game game, Action<int> watchADHandler, bool isAdAvailable)
	{
	}
}
