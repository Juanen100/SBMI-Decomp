using System;

public class ProductionSlotShellUnavailable : ProductionSlotShell
{
	public ProductionSlotShellUnavailable(SBGUIProductionSlot core, int slotId)
		: base(core, slotId)
	{
		base.core.rushButton.SetVisible(false);
		base.core.background.SetTextureFromAtlas("StoreLock.png");
		base.core.background.SetVisible(true);
		activated = false;
	}

	public override void UpdateInfo(BuildingEntity producer, int slot, Action<int> rushHandler, Game game)
	{
	}
}
