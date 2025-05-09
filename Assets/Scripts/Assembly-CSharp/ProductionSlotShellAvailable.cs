#define ASSERTS_ON
using System;

public class ProductionSlotShellAvailable : ProductionSlotShell
{
	public ProductionSlotShellAvailable(SBGUIProductionSlot core, int slotId)
		: base(core, slotId)
	{
		base.core.rushButton.SetVisible(true);
	}

	public override void UpdateInfo(BuildingEntity producer, int slot, Action<int> rushHandler, Game game)
	{
		CraftingInstance craftingInstance = null;
		if (producer != null)
		{
			craftingInstance = ((!producer.ShuntsCrafting) ? game.craftManager.GetCraftingInstance(producer.Id, slot) : game.craftManager.GetCraftingInstance(producer.Annexes[slot].Id, 0));
		}
		if (craftingInstance == null || craftingInstance.rushed)
		{
			core.label.Text = string.Empty;
			core.rushButton.SessionActionId = string.Empty;
			core.rushCostLabel.Text = string.Empty;
			core.rushButton.SetActive(false);
			core.icon.SetTextureFromAtlas("empty.png");
			core.background.SetVisible(false);
			core.ClearButtonActions(core.rushButton.name);
			activated = false;
			return;
		}
		if (activated)
		{
			CraftingRecipe recipeById = game.craftManager.GetRecipeById(craftingInstance.recipeId);
			float num = (float)Math.Max(0.0, (double)craftingInstance.ReadyTimeFromNow / (double)recipeById.craftTime);
			if (num < 0f)
			{
				core.label.Text = string.Empty;
				core.rushButton.SessionActionId = string.Empty;
				core.rushCostLabel.Text = string.Empty;
				core.rushButton.SetActive(false);
				core.icon.SetTextureFromAtlas("empty.png");
				core.background.SetVisible(false);
				core.ClearButtonActions(core.rushButton.name);
				activated = false;
			}
			Cost cost = Cost.Prorate(recipeById.rushCost, num);
			core.label.Text = TFUtils.DurationToString(craftingInstance.ReadyTimeFromNow);
			core.rushCostLabel.Text = cost.ResourceAmounts[ResourceManager.HARD_CURRENCY].ToString();
			return;
		}
		CraftingRecipe recipeById2 = game.craftManager.GetRecipeById(craftingInstance.recipeId);
		int num2 = recipeById2.rushCost.ResourceAmounts[ResourceManager.HARD_CURRENCY];
		string resourceTexture = game.resourceManager.Resources[recipeById2.productId].GetResourceTexture();
		TFUtils.Assert(resourceTexture != null, "This craft rewards thought icon is null! Need to know what to show!");
		core.label.Text = TFUtils.DurationToString(craftingInstance.ReadyTimeFromNow);
		core.icon.SetTextureFromAtlas(resourceTexture);
		core.background.SetTextureFromAtlas("ProductionSlotInUse.png");
		core.background.SetVisible(true);
		Action rushButtonHandler = null;
		rushButtonHandler = delegate
		{
			rushHandler(slot);
			core.rushButton.ClickEvent -= rushButtonHandler;
		};
		core.rushButton.SetActive(true);
		core.rushButton.SessionActionId = "ProductionRush_" + slot + "_" + producer.BlueprintName;
		core.ClearButtonActions(core.rushButton.name);
		core.AttachActionToButton(core.rushButton.name, rushButtonHandler);
		core.rushCostLabel.Text = num2.ToString();
		activated = true;
	}
}
