using System;

public class ProductionSlotShellLocked : ProductionSlotShell
{
	public ProductionSlotShellLocked(SBGUIProductionSlot core, Cost purchaseCost, int slotId, Game game)
		: base(core, slotId)
	{
		base.core.background.SetTextureFromAtlas("StoreLock.png");
		base.core.background.SetVisible(true);
		if (game.featureManager.CheckFeature("allow_production_slot_purchase"))
		{
			base.core.rushButton.SetVisible(true);
			base.core.rushButton.SetActive(true);
			base.core.rushCostLabel.Text = purchaseCost.ResourceAmounts[ResourceManager.HARD_CURRENCY].ToString();
		}
		activated = false;
	}

	public override void UpdateInfo(BuildingEntity producer, int slot, Action<int> rushHandler, Game game)
	{
		if (!activated)
		{
			Action rushButtonHandler = null;
			rushButtonHandler = delegate
			{
				rushHandler(slot);
				core.rushButton.ClickEvent -= rushButtonHandler;
			};
			core.ClearButtonActions(core.rushButton.name);
			if (game.featureManager.CheckFeature("allow_production_slot_purchase"))
			{
				core.AttachActionToButton(core.rushButton.name, rushButtonHandler);
				core.rushButton.SetActive(true);
			}
			core.rushButton.SessionActionId = "ProductionRush_" + slot + "_" + producer.BlueprintName;
			activated = true;
		}
	}
}
