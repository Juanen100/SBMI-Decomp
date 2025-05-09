using System.Collections.Generic;
using UnityEngine;

public static class RewardManager
{
	public class RewardDropResults
	{
		public Dictionary<string, object> buildingLabels;

		public List<Identity> dropIdentities;

		public RewardDropResults(Dictionary<string, object> buildingLabels, List<Identity> dropIdentities)
		{
			this.buildingLabels = buildingLabels;
			this.dropIdentities = dropIdentities;
		}
	}

	public static void ApplyToGameState(Reward reward, ulong collectionTime, Dictionary<string, object> gameState)
	{
		foreach (KeyValuePair<int, int> resourceAmount in reward.ResourceAmounts)
		{
			ResourceManager.AddAmountToGameState(resourceAmount.Key, resourceAmount.Value, gameState);
		}
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["buildings"];
		foreach (KeyValuePair<int, int> buildingAmount in reward.BuildingAmounts)
		{
			int key = buildingAmount.Key;
			int value = buildingAmount.Value;
			Blueprint blueprint = EntityManager.GetBlueprint("building", key);
			List<object> list2 = (List<object>)reward.BuildingLabels[key.ToString()];
			for (int i = 0; i < value; i++)
			{
				Dictionary<string, object> data = new Dictionary<string, object>();
				if (reward.BuildingPositions == null || !reward.BuildingPositions.ContainsKey(key))
				{
					data["did"] = key;
					data["extensions"] = (uint)(blueprint.PrimaryType | EntityType.CORE);
					data["label"] = (string)list2[i];
					data["x"] = null;
					data["y"] = null;
					data["flip"] = false;
					data["build_finish_time"] = collectionTime;
					ActivatableDecorator.Serialize(ref data, collectionTime);
					if (blueprint.Invariable.ContainsKey("time.production") && blueprint.Invariable["time.production"] != null)
					{
						data["rent_ready_time"] = collectionTime + (ulong)blueprint.Invariable["time.production"];
					}
					list.Add(data);
				}
			}
		}
		List<object> list3 = (List<object>)((Dictionary<string, object>)gameState["farm"])["recipes"];
		foreach (int recipeUnlock in reward.RecipeUnlocks)
		{
			list3.Add(recipeUnlock);
		}
		List<object> list4 = (List<object>)((Dictionary<string, object>)gameState["farm"])["costumes"];
		foreach (int costumeUnlock in reward.CostumeUnlocks)
		{
			list4.Add(costumeUnlock);
		}
		List<object> list5 = (List<object>)((Dictionary<string, object>)gameState["farm"])["movies"];
		foreach (int movieUnlock in reward.MovieUnlocks)
		{
			list5.Add(movieUnlock);
		}
	}

	public static RewardDropResults GenerateRewardDrops(Reward reward, Simulation simulation, Simulated simulated, ulong utcNow, bool bonusReward = false)
	{
		Vector3 dropPosition = new Vector3(simulated.ThoughtDisplayController.Position.x - simulated.ThoughtDisplayController.Width / 2f, simulated.ThoughtDisplayController.Position.y, simulated.ThoughtDisplayController.Position.z);
		return GenerateRewardDrops(reward, simulation, dropPosition, utcNow, bonusReward);
	}

	public static RewardDropResults GenerateRewardDrops(Reward reward, Simulation simulation, Vector3 dropPosition, ulong utcNow, bool bonusReward = false)
	{
		if (reward == null)
		{
			TFUtils.ErrorLog("RewardManager.GenerateRewardDrops - reward is null");
			return null;
		}
		int amount = simulation.resourceManager.Resources[ResourceManager.LEVEL].Amount;
		int num = 0;
		int num2 = 0;
		int amountOfCurrentRewardToDrop = 0;
		List<ItemDropCtor> list = new List<ItemDropCtor>();
		List<Identity> list2 = new List<Identity>();
		foreach (KeyValuePair<int, int> resourceAmount in reward.ResourceAmounts)
		{
			int key = resourceAmount.Key;
			int value = resourceAmount.Value;
			if (key == ResourceManager.SOFT_CURRENCY)
			{
				num2 = value;
			}
			else if (key == ResourceManager.XP)
			{
				num = value;
			}
			else if (key == ResourceManager.VALENTINES_CURRENCY || key == ResourceManager.CHRISTMAS_CURRENCY || key == ResourceManager.CHRISTMAS_CURRENCY_V2 || key == ResourceManager.HALLOWEEN_CURRENCY || key == ResourceManager.SPONGY_GAMES_CURRENCY || key == ResourceManager.SPECIAL_CURRENCY)
			{
				amountOfCurrentRewardToDrop = value;
			}
			simulation.analytics.LogResourceDrop(simulation.resourceManager.Resources[resourceAmount.Key].Name, value, amount);
		}
		foreach (KeyValuePair<int, int> resourceAmount2 in reward.ResourceAmounts)
		{
			int key2 = resourceAmount2.Key;
			int value2 = resourceAmount2.Value;
			if (key2 == ResourceManager.SOFT_CURRENCY && !bonusReward)
			{
				GenerateDividedRewardDrops(simulation, key2, list, utcNow, num2, num);
				continue;
			}
			if (key2 == ResourceManager.XP && !bonusReward)
			{
				GenerateDividedRewardDrops(simulation, key2, list, utcNow, num, num2);
				continue;
			}
			if ((key2 == ResourceManager.VALENTINES_CURRENCY || key2 == ResourceManager.CHRISTMAS_CURRENCY || key2 == ResourceManager.CHRISTMAS_CURRENCY_V2 || key2 == ResourceManager.HALLOWEEN_CURRENCY || key2 == ResourceManager.SPONGY_GAMES_CURRENCY || key2 == ResourceManager.SPECIAL_CURRENCY) && !bonusReward)
			{
				GenerateDividedRewardDrops(simulation, key2, list, utcNow, amountOfCurrentRewardToDrop, num);
				continue;
			}
			for (int i = 0; i < value2; i++)
			{
				string resourceTexture = simulation.resourceManager.Resources[key2].GetResourceTexture(1);
				IDisplayController displayController = simulation.rewardDropManager.CreateDrop(simulation.resourceManager.Resources[key2]);
				Vector2 screenCollectionDestination = ResourceDrop.GetScreenCollectionDestination(key2);
				ItemDropDefinition itemDropDefinition = new ItemDropDefinition(key2, displayController, screenCollectionDestination, simulation.resourceManager.Resources[key2].ForceTapToCollect);
				itemDropDefinition.DisplayController.Billboard(SBCamera.BillboardDefinition);
				list.Add(new ResourceDropCtor(itemDropDefinition, 1, utcNow));
				if (resourceTexture != null)
				{
					displayController.UpdateMaterialOrTexture(resourceTexture);
				}
			}
		}
		foreach (KeyValuePair<int, int> buildingAmount in reward.BuildingAmounts)
		{
			int key3 = buildingAmount.Key;
			int value3 = buildingAmount.Value;
			List<object> list3 = (List<object>)reward.BuildingLabels[key3.ToString()];
			for (int j = 0; j < value3; j++)
			{
				Blueprint blueprint = EntityManager.GetBlueprint("building", key3);
				IDisplayController displayController2 = (IDisplayController)blueprint.Invariable["display"];
				displayController2 = simulation.rewardDropManager.CreateDrop((int)displayController2.Width, (int)displayController2.Height, null, displayController2.MaterialName);
				Vector2 screenCollectionDestination2 = BuildingDrop.GetScreenCollectionDestination();
				ItemDropDefinition itemDropDefinition2 = new ItemDropDefinition(key3, displayController2, screenCollectionDestination2, false);
				itemDropDefinition2.DisplayController.Billboard(SBCamera.BillboardDefinition);
				list.Add(new BuildingDropCtor(itemDropDefinition2, new Identity((string)list3[j]), utcNow));
				simulation.analytics.LogBuildingDrop((string)blueprint.Invariable["name"], 1, amount);
			}
		}
		foreach (int recipeUnlock in reward.RecipeUnlocks)
		{
			IDisplayController displayController3 = simulation.rewardDropManager.CreateDrop("RecipeIcon.png");
			Vector2 screenCollectionDestination3 = RecipeDrop.GetScreenCollectionDestination();
			ItemDropDefinition itemDropDefinition3 = new ItemDropDefinition(recipeUnlock, displayController3, screenCollectionDestination3, true);
			itemDropDefinition3.DisplayController.Billboard(SBCamera.BillboardDefinition);
			list.Add(new RecipeDropCtor(itemDropDefinition3, utcNow));
			CraftingRecipe recipeById = simulation.game.craftManager.GetRecipeById(recipeUnlock);
			simulation.analytics.LogRecipeDrop(recipeById.recipeTag, amount);
		}
		foreach (int movieUnlock in reward.MovieUnlocks)
		{
			IDisplayController displayController4 = simulation.rewardDropManager.CreateDrop("MovieIcon.png");
			Vector2 screenCollectionDestination4 = MovieDrop.GetScreenCollectionDestination();
			ItemDropDefinition itemDropDefinition4 = new ItemDropDefinition(movieUnlock, displayController4, screenCollectionDestination4, true);
			itemDropDefinition4.DisplayController.Billboard(SBCamera.BillboardDefinition);
			list.Add(new MovieDropCtor(itemDropDefinition4, utcNow));
			simulation.analytics.LogMovieDrop(string.Format("movie_{0:0000}", movieUnlock), amount);
		}
		simulation.DropManager.AddDrops(dropPosition, list, list2, simulation);
		return new RewardDropResults(reward.BuildingLabels, list2);
	}

	private static void GenerateDividedRewardDrops(Simulation simulation, int resourceDid, List<ItemDropCtor> rewardDrops, ulong utcNow, int amountOfCurrentRewardToDrop, int amountOfNextRewardToDrop)
	{
		int num = (TFPerfUtils.IsNonParticleDevice() ? 1 : Random.Range(1, 6));
		int num2 = 0;
		int num3 = 0;
		if (amountOfCurrentRewardToDrop > num)
		{
			num2 = amountOfCurrentRewardToDrop / num;
			num3 = amountOfCurrentRewardToDrop % num;
			for (int i = 0; i < num; i++)
			{
				string text = null;
				text = ((i != num - 1) ? simulation.resourceManager.Resources[resourceDid].GetResourceTexture(num2) : simulation.resourceManager.Resources[resourceDid].GetResourceTexture(num2 + num3));
				IDisplayController displayController = simulation.rewardDropManager.CreateDrop(simulation.resourceManager.Resources[resourceDid]);
				Vector2 screenCollectionDestination = ResourceDrop.GetScreenCollectionDestination(resourceDid);
				ItemDropDefinition itemDropDefinition = new ItemDropDefinition(resourceDid, displayController, screenCollectionDestination, simulation.resourceManager.Resources[resourceDid].ForceTapToCollect);
				itemDropDefinition.DisplayController.Billboard(SBCamera.BillboardDefinition);
				if (text != null)
				{
					displayController.UpdateMaterialOrTexture(text);
				}
				if (i == num - 1)
				{
					rewardDrops.Add(new ResourceDropCtor(itemDropDefinition, num2 + num3, utcNow));
				}
				else
				{
					rewardDrops.Add(new ResourceDropCtor(itemDropDefinition, num2, utcNow));
				}
			}
			return;
		}
		for (int j = 0; j < amountOfCurrentRewardToDrop; j++)
		{
			string resourceTexture = simulation.resourceManager.Resources[resourceDid].GetResourceTexture(1);
			IDisplayController displayController2 = simulation.rewardDropManager.CreateDrop(simulation.resourceManager.Resources[resourceDid]);
			Vector2 screenCollectionDestination2 = ResourceDrop.GetScreenCollectionDestination(resourceDid);
			ItemDropDefinition itemDropDefinition2 = new ItemDropDefinition(resourceDid, displayController2, screenCollectionDestination2, simulation.resourceManager.Resources[resourceDid].ForceTapToCollect);
			itemDropDefinition2.DisplayController.Billboard(SBCamera.BillboardDefinition);
			rewardDrops.Add(new ResourceDropCtor(itemDropDefinition2, 1, utcNow));
			if (resourceTexture != null)
			{
				displayController2.UpdateMaterialOrTexture(resourceTexture);
			}
		}
	}

	public static bool ReleaseDisplayController(Simulation simulation, IDisplayController dc)
	{
		return simulation.rewardDropManager.ReleaseDrop(dc);
	}
}
