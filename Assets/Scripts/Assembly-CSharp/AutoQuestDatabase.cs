#define ASSERTS_ON
using System.Collections.Generic;
using UnityEngine;

public class AutoQuestDatabase
{
	private const int _nNumSavedAutoQuests = 3;

	private Dictionary<string, List<int>> m_pCategoryItems;

	private Dictionary<int, AutoQuestData> m_pAutoQuests;

	private static List<int> m_pPreviousAutoQuests;

	private static Dictionary<int, List<int>> m_pPreviousAutoQuestCharacters;

	public AutoQuestDatabase()
	{
		LoadDatabase();
	}

	public void AddPreviousAutoQuests(int nAutoQuestDID, int nCharacterDID)
	{
		m_pPreviousAutoQuests.Add(nAutoQuestDID);
		while (m_pPreviousAutoQuests.Count > 3 || m_pPreviousAutoQuests.Count >= m_pAutoQuests.Count)
		{
			m_pPreviousAutoQuests.RemoveAt(0);
		}
		if (m_pPreviousAutoQuestCharacters.ContainsKey(nAutoQuestDID))
		{
			if (m_pPreviousAutoQuestCharacters[nAutoQuestDID].Contains(nCharacterDID))
			{
				m_pPreviousAutoQuestCharacters[nAutoQuestDID] = new List<int> { nCharacterDID };
			}
			else
			{
				m_pPreviousAutoQuestCharacters[nAutoQuestDID].Add(nCharacterDID);
			}
		}
		else
		{
			m_pPreviousAutoQuestCharacters.Add(nAutoQuestDID, new List<int> { nCharacterDID });
		}
	}

	public static void SetPreviousAutoQuestDataFramGameState(Dictionary<string, object> pGameState)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)pGameState["farm"];
		if (!dictionary.ContainsKey("previous_auto_quests"))
		{
			m_pPreviousAutoQuests = new List<int>();
			m_pPreviousAutoQuestCharacters = new Dictionary<int, List<int>>();
			return;
		}
		Dictionary<string, object> dictionary2 = TFUtils.TryLoadDict(dictionary, "previous_auto_quests");
		if (dictionary2.ContainsKey("auto_quest_dids"))
		{
			m_pPreviousAutoQuests = TFUtils.TryLoadList<int>(dictionary2, "auto_quest_dids");
		}
		else
		{
			m_pPreviousAutoQuests = new List<int>();
		}
		m_pPreviousAutoQuestCharacters = new Dictionary<int, List<int>>();
		if (dictionary2.ContainsKey("characters_per_quest"))
		{
			List<Dictionary<string, object>> list = TFUtils.TryLoadList<Dictionary<string, object>>(dictionary2, "characters_per_quest");
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				Dictionary<string, object> dictionary3 = list[i];
				m_pPreviousAutoQuestCharacters.Add(TFUtils.LoadInt(dictionary3, "auto_quest_did"), TFUtils.TryLoadList<int>(dictionary3, "character_dids"));
			}
		}
	}

	public static void WritePreviousAutoQuestDataToGameState(Dictionary<string, object> pGameState)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)pGameState["farm"];
		Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
		List<object> list = new List<object>();
		if (m_pPreviousAutoQuests == null)
		{
			m_pPreviousAutoQuests = new List<int>();
		}
		if (m_pPreviousAutoQuestCharacters == null)
		{
			m_pPreviousAutoQuestCharacters = new Dictionary<int, List<int>>();
		}
		int count = m_pPreviousAutoQuests.Count;
		for (int i = 0; i < count; i++)
		{
			list.Add(m_pPreviousAutoQuests[i]);
		}
		dictionary2.Add("auto_quest_dids", list);
		List<object> list2 = new List<object>();
		foreach (KeyValuePair<int, List<int>> pPreviousAutoQuestCharacter in m_pPreviousAutoQuestCharacters)
		{
			Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
			dictionary3.Add("auto_quest_did", pPreviousAutoQuestCharacter.Key);
			int count2 = pPreviousAutoQuestCharacter.Value.Count;
			List<object> list3 = new List<object>();
			for (int j = 0; j < count2; j++)
			{
				list3.Add(pPreviousAutoQuestCharacter.Value[j]);
			}
			dictionary3.Add("character_dids", list3);
			list2.Add(dictionary3);
		}
		dictionary2.Add("characters_per_quest", list2);
		if (dictionary.ContainsKey("previous_auto_quests"))
		{
			dictionary["previous_auto_quests"] = dictionary2;
		}
		else
		{
			dictionary.Add("previous_auto_quests", dictionary2);
		}
	}

	public AutoQuestData.DialogData GetDialogData(int nAutoQuestDID, int nCharacterDID)
	{
		if (!m_pAutoQuests.ContainsKey(nAutoQuestDID))
		{
			return null;
		}
		return m_pAutoQuests[nAutoQuestDID].GetDialogData(nCharacterDID);
	}

	public AutoQuest GenerateNextAutoQuest(Game pGame)
	{
		if (m_pAutoQuests == null)
		{
			return null;
		}
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, AutoQuestData> pAutoQuest in m_pAutoQuests)
		{
			if (IsAutoQuestAvailable(pGame, pAutoQuest.Key))
			{
				list.Add(pAutoQuest.Key);
			}
		}
		int count = list.Count;
		if (count <= 0)
		{
			return null;
		}
		return GenerateAutoQuest(pGame, list[Random.Range(0, count)]);
	}

	public bool IsQuestValid(Game pGame, QuestDefinition pQuestDef)
	{
		CraftingManager craftManager = pGame.craftManager;
		List<int> list = new List<int>();
		List<QuestBookendInfo.ChunkConditions> chunks = pQuestDef.End.Chunks;
		int num = chunks.Count - 1;
		for (int i = 0; i < num; i++)
		{
			QuestBookendInfo.ChunkConditions chunkConditions = chunks[i];
			Dictionary<string, object> dictionary = chunkConditions.Condition.ToDict();
			if (dictionary.ContainsKey("resource_id"))
			{
				list.Add(TFUtils.LoadInt(dictionary, "resource_id"));
			}
		}
		int count = list.Count;
		for (int j = 0; j < count; j++)
		{
			int productId = list[j];
			CraftingRecipe recipeByProductId = craftManager.GetRecipeByProductId(productId);
			if (recipeByProductId == null)
			{
				return false;
			}
			if (!craftManager.IsRecipeUnlocked(recipeByProductId.identity))
			{
				return false;
			}
			if (recipeByProductId.buildingId >= 0)
			{
				Simulated simulated = pGame.simulation.FindSimulated(recipeByProductId.buildingId);
				if (simulated == null)
				{
					return false;
				}
			}
		}
		return true;
	}

	private void LoadDatabase()
	{
		m_pCategoryItems = new Dictionary<string, List<int>>();
		m_pAutoQuests = new Dictionary<int, AutoQuestData>();
		m_pPreviousAutoQuests = new List<int>();
		m_pPreviousAutoQuestCharacters = new Dictionary<int, List<int>>();
		LoadCategories();
		LoadAutoQuests();
	}

	private AutoQuest GenerateAutoQuest(Game pGame, int nDID)
	{
		if (nDID == 2 || nDID == 4)
		{
			int num = 5;
		}
		AutoQuestData autoQuestData = m_pAutoQuests[nDID];
		CraftingManager craftManager = pGame.craftManager;
		ResourceManager resourceManager = pGame.resourceManager;
		int num2 = Random.Range(autoQuestData.m_nMinItems, autoQuestData.m_nMaxItems + 1);
		ulong num3 = 0uL;
		ulong num4 = (ulong)pGame.levelingManager.AutoQuestLength(pGame.resourceManager.Query(ResourceManager.LEVEL));
		float num5 = 0f;
		int num6 = 0;
		int num7 = 0;
		string[] itemCategories = autoQuestData.GetItemCategories();
		bool[] pickOneCategories = autoQuestData.GetPickOneCategories();
		int num8 = itemCategories.Length;
		int[] characters = autoQuestData.GetCharacters();
		int num9 = characters.Length;
		List<int> value = null;
		List<int> list = new List<int>();
		int num10 = 0;
		m_pPreviousAutoQuestCharacters.TryGetValue(nDID, out value);
		for (int i = 0; i < num9; i++)
		{
			if ((value == null || !value.Contains(characters[i])) && pGame.simulation.FindSimulated(characters[i]) != null)
			{
				list.Add(characters[i]);
			}
		}
		if (list.Count <= 0)
		{
			for (int j = 0; j < num9; j++)
			{
				if (pGame.simulation.FindSimulated(characters[j]) != null)
				{
					list.Add(characters[j]);
				}
			}
			if (list.Count < 0)
			{
				TFUtils.ErrorLog("AutoQuestDatabase | GenerateAutoQuest: No available character for quest id " + nDID + ".");
				return null;
			}
			num10 = list[Random.Range(0, list.Count)];
		}
		else
		{
			num10 = list[Random.Range(0, list.Count)];
		}
		Dictionary<int, int>[] array = new Dictionary<int, int>[num8];
		List<int>[] array2 = new List<int>[num8];
		List<int>[] array3 = new List<int>[num8];
		int count;
		for (int k = 0; k < num8; k++)
		{
			array[k] = new Dictionary<int, int>();
			array2[k] = new List<int>();
			array3[k] = new List<int>();
			string key = itemCategories[k];
			if (!m_pCategoryItems.ContainsKey(key))
			{
				continue;
			}
			List<int> list2 = m_pCategoryItems[key];
			count = list2.Count;
			for (int l = 0; l < count; l++)
			{
				if (!craftManager.IsRecipeUnlocked(list2[l]))
				{
					continue;
				}
				CraftingRecipe recipeById = craftManager.GetRecipeById(list2[l]);
				if (recipeById != null)
				{
					if (recipeById.buildingId >= 0)
					{
						Simulated simulated = pGame.simulation.FindSimulated(recipeById.buildingId);
						if (simulated == null)
						{
							continue;
						}
					}
					if (!resourceManager.Resources.ContainsKey(recipeById.productId))
					{
						TFUtils.ErrorLog("Could not find product did: " + recipeById.productId + " for recipe did: " + recipeById.identity + " in resources. ");
					}
					else if (resourceManager.Resources[recipeById.productId].Amount > 0)
					{
						array[k].Add(recipeById.productId, resourceManager.Resources[recipeById.productId].Amount);
					}
					else
					{
						array2[k].Add(recipeById.productId);
					}
				}
				else
				{
					TFUtils.ErrorLog("AutoQuestDatabase | failed to find crafting recipe with did: " + list2[l]);
				}
			}
		}
		switch (autoQuestData.m_eDistribution)
		{
		case AutoQuestData.eDistributionType.eRandom:
			if (num8 <= 0)
			{
				break;
			}
			count = 0;
			while (num2 > 0 && num3 < num4)
			{
				if (count >= num8)
				{
					count = 0;
				}
				if (pickOneCategories[count] && array3[count].Count > 0)
				{
					array3[count].Add(array3[count][0]);
					CraftingRecipe recipeById = craftManager.GetRecipeByProductId(array3[count][0]);
					Reward reward = recipeById.rewardDefinition.GenerateReward(pGame.simulation, true, false);
					num3 += recipeById.craftTime;
					num6 += GetXpForCraftingRecipe(pGame, recipeById);
					num5 += GetGoldForCraftingRecipe(pGame, recipeById);
					num2--;
					count++;
					continue;
				}
				int num11 = -1;
				if ((num7 > 0 && array2[count].Count > 0) || array[count].Count <= 0)
				{
					int num12 = ((!pickOneCategories[count]) ? (array2[count].Count + 1) : array2[count].Count);
					num12 = Random.Range(0, num12);
					if (num12 != array2[count].Count)
					{
						num11 = array2[count][num12];
						num7--;
					}
				}
				else
				{
					int num12 = ((!pickOneCategories[count]) ? (array[count].Count + 1) : array[count].Count);
					num12 = Random.Range(0, num12);
					if (num12 != array[count].Count)
					{
						int num13 = 0;
						foreach (KeyValuePair<int, int> item in array[count])
						{
							if (num13 == num12)
							{
								num11 = item.Key;
								array[count][item.Key] = array[count][item.Key] - 1;
								if (array[count][item.Key] <= 0)
								{
									array[count].Remove(item.Key);
									array2[count].Add(item.Key);
								}
								num7++;
								break;
							}
							num13++;
						}
					}
				}
				if (num11 >= 0)
				{
					array3[count].Add(num11);
					CraftingRecipe recipeById = craftManager.GetRecipeByProductId(num11);
					Reward reward = recipeById.rewardDefinition.GenerateReward(pGame.simulation, true, false);
					num3 += recipeById.craftTime;
					num6 += GetXpForCraftingRecipe(pGame, recipeById);
					num5 += GetGoldForCraftingRecipe(pGame, recipeById);
					num2--;
				}
				count++;
			}
			break;
		case AutoQuestData.eDistributionType.eEqual:
		{
			if (num8 <= 0)
			{
				break;
			}
			num2 = Mathf.RoundToInt((float)num2 / (float)num8);
			for (int m = 0; m < num2; m++)
			{
				if (num3 >= num4)
				{
					break;
				}
				for (count = 0; count < num8; count++)
				{
					if (num3 >= num4)
					{
						break;
					}
					if (pickOneCategories[count] && array3[count].Count > 0)
					{
						array3[count].Add(array3[count][0]);
						CraftingRecipe recipeById = craftManager.GetRecipeByProductId(array3[count][0]);
						Reward reward = recipeById.rewardDefinition.GenerateReward(pGame.simulation, true, false);
						num3 += recipeById.craftTime;
						num6 += GetXpForCraftingRecipe(pGame, recipeById);
						num5 += GetGoldForCraftingRecipe(pGame, recipeById);
						continue;
					}
					int num11 = -1;
					if ((num7 > 0 && array2[count].Count > 0) || array[count].Count <= 0)
					{
						int num12 = Random.Range(0, array2[count].Count);
						if (num12 != array2[count].Count)
						{
							num11 = array2[count][num12];
							num7--;
						}
					}
					else
					{
						int num12 = Random.Range(0, array[count].Count);
						if (num12 != array[count].Count)
						{
							int num13 = 0;
							foreach (KeyValuePair<int, int> item2 in array[count])
							{
								if (num13 == num12)
								{
									num11 = item2.Key;
									array[count][item2.Key] = array[count][item2.Key] - 1;
									if (array[count][item2.Key] <= 0)
									{
										array[count].Remove(item2.Key);
										array2[count].Add(item2.Key);
									}
									num7++;
									break;
								}
								num13++;
							}
						}
					}
					if (num11 >= 0)
					{
						array3[count].Add(num11);
						CraftingRecipe recipeById = craftManager.GetRecipeByProductId(num11);
						Reward reward = recipeById.rewardDefinition.GenerateReward(pGame.simulation, true, false);
						num3 += recipeById.craftTime;
						num6 += GetXpForCraftingRecipe(pGame, recipeById);
						num5 += GetGoldForCraftingRecipe(pGame, recipeById);
					}
				}
			}
			break;
		}
		}
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		count = array3.Length;
		for (int n = 0; n < count; n++)
		{
			int num13 = array3[n].Count;
			for (int num14 = 0; num14 < num13; num14++)
			{
				if (dictionary.ContainsKey(array3[n][num14]))
				{
					Dictionary<int, int> dictionary3;
					Dictionary<int, int> dictionary2 = (dictionary3 = dictionary);
					int key3;
					int key2 = (key3 = array3[n][num14]);
					key3 = dictionary3[key3];
					dictionary2[key2] = key3 + 1;
				}
				else
				{
					dictionary.Add(array3[n][num14], 1);
				}
			}
		}
		return new AutoQuest(nDID, num10, dictionary, Mathf.RoundToInt(num5 * autoQuestData.m_fGoldMultiplier), Mathf.RoundToInt((float)num6 * autoQuestData.m_fExpMultiplier), autoQuestData.m_sName, autoQuestData.m_sDescription);
	}

	private int GetXpForCraftingRecipe(Game pGame, CraftingRecipe pCraftingRecipe)
	{
		int num = 0;
		Resource resource = pGame.resourceManager.Resources[pCraftingRecipe.productId];
		if (resource.Reward != null)
		{
			num = resource.Reward.LowestResourceValue(ResourceManager.XP);
		}
		Cost cost = pCraftingRecipe.cost;
		if (cost != null)
		{
			foreach (KeyValuePair<int, int> resourceAmount in cost.ResourceAmounts)
			{
				if (resourceAmount.Key != ResourceManager.SOFT_CURRENCY && resourceAmount.Key != ResourceManager.HARD_CURRENCY)
				{
					CraftingRecipe recipeByProductId = pGame.craftManager.GetRecipeByProductId(resourceAmount.Key);
					if (recipeByProductId != null && recipeByProductId.cost != null)
					{
						num += GetXpForCraftingRecipe(pGame, recipeByProductId) * resourceAmount.Value;
					}
				}
			}
		}
		return num;
	}

	private float GetGoldForCraftingRecipe(Game pGame, CraftingRecipe pCraftingRecipe)
	{
		int num = pCraftingRecipe.rewardDefinition.LowestResourceValue(pCraftingRecipe.productId);
		float num2 = GetGoldForCost(pGame, pGame.craftManager, pCraftingRecipe.cost) / (float)num;
		if (pCraftingRecipe.cost.ResourceAmounts.Count <= 0 && pCraftingRecipe.buildingId >= 0)
		{
			int num3 = Mathf.RoundToInt(1f / pGame.resourceManager.Resources[pCraftingRecipe.productId].HardCurrencyConversion);
			num2 += (float)num3 * pGame.resourceManager.Resources[ResourceManager.SOFT_CURRENCY].HardCurrencyConversion;
		}
		Resource resource = pGame.resourceManager.Resources[pCraftingRecipe.productId];
		if (resource.Reward != null)
		{
			int num4 = resource.Reward.LowestResourceValue(ResourceManager.HARD_CURRENCY);
			if (num4 > 0)
			{
				num2 += (float)num4 * pGame.resourceManager.Resources[ResourceManager.SOFT_CURRENCY].HardCurrencyConversion;
			}
			else
			{
				num4 = resource.Reward.LowestResourceValue(ResourceManager.SOFT_CURRENCY);
				num2 += (float)num4;
			}
		}
		return num2;
	}

	private float GetGoldForCost(Game pGame, CraftingManager pCraftingManager, Cost pCost)
	{
		float num = 0f;
		foreach (KeyValuePair<int, int> resourceAmount in pCost.ResourceAmounts)
		{
			if (resourceAmount.Key == ResourceManager.SOFT_CURRENCY)
			{
				num += (float)resourceAmount.Value;
				continue;
			}
			if (resourceAmount.Key == ResourceManager.HARD_CURRENCY)
			{
				num += (float)resourceAmount.Value * pGame.resourceManager.Resources[ResourceManager.SOFT_CURRENCY].HardCurrencyConversion;
				continue;
			}
			CraftingRecipe recipeByProductId = pCraftingManager.GetRecipeByProductId(resourceAmount.Key);
			if (recipeByProductId != null && recipeByProductId.cost != null)
			{
				num += GetGoldForCraftingRecipe(pGame, recipeByProductId) * (float)resourceAmount.Value;
			}
		}
		return num;
	}

	private bool IsAutoQuestAvailable(Game pGame, int nDID)
	{
		if (pGame == null || m_pAutoQuests == null || !m_pAutoQuests.ContainsKey(nDID))
		{
			return false;
		}
		CraftingManager craftManager = pGame.craftManager;
		Simulation simulation = pGame.simulation;
		AutoQuestData autoQuestData = m_pAutoQuests[nDID];
		string[] itemCategories = autoQuestData.GetItemCategories();
		int count = m_pPreviousAutoQuests.Count;
		for (int i = 0; i < count; i++)
		{
			if (m_pPreviousAutoQuests[i] == nDID)
			{
				return false;
			}
		}
		int[] characters = autoQuestData.GetCharacters();
		count = characters.Length;
		bool flag = false;
		for (int j = 0; j < count; j++)
		{
			if (simulation.FindSimulated(characters[j]) != null)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return false;
		}
		count = itemCategories.Length;
		for (int k = 0; k < count; k++)
		{
			string key = itemCategories[k];
			if (!m_pCategoryItems.ContainsKey(key))
			{
				return false;
			}
			List<int> list = m_pCategoryItems[key];
			int count2 = list.Count;
			flag = false;
			for (int l = 0; l < count2; l++)
			{
				if (!craftManager.IsRecipeUnlocked(list[l]))
				{
					continue;
				}
				CraftingRecipe recipeById = craftManager.GetRecipeById(list[l]);
				if (recipeById.buildingId >= 0)
				{
					Simulated simulated = simulation.FindSimulated(recipeById.buildingId);
					if (simulated == null)
					{
						continue;
					}
				}
				flag = true;
				break;
			}
			if (!flag)
			{
				return false;
			}
		}
		return true;
	}

	private void LoadCategories()
	{
		string text = "AutoQuestCategories";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null)
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		int num2 = -1;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, "id").ToString());
			if (num2 < 0)
			{
				num2 = instance.GetIntCell(sheetIndex, rowIndex, "items");
			}
			List<int> list = new List<int>();
			for (int j = 1; j <= num2; j++)
			{
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, "item did " + j);
				if (intCell >= 0)
				{
					list.Add(intCell);
				}
			}
			string stringCell = instance.GetStringCell(sheetIndex, rowIndex, "category name");
			if (m_pCategoryItems.ContainsKey(stringCell))
			{
				TFUtils.Assert(false, "Trying to add duplicate Auto Quest Category: " + stringCell + ".");
			}
			else
			{
				m_pCategoryItems.Add(stringCell, list);
			}
		}
	}

	private void LoadAutoQuests()
	{
		string text = "AutoQuest";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null)
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		Dictionary<int, Dictionary<string, object>> dictionary = new Dictionary<int, Dictionary<string, object>>();
		string text2 = "n/a";
		int num2 = -1;
		int num3 = -1;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, "id").ToString());
			if (num2 < 0)
			{
				num2 = instance.GetIntCell(sheetIndex, rowIndex, "item categories");
				num3 = instance.GetIntCell(sheetIndex, rowIndex, "characters");
			}
			int intCell = instance.GetIntCell(sheetIndex, rowIndex, "did");
			if (m_pAutoQuests.ContainsKey(intCell))
			{
				TFUtils.Assert(false, "Trying to add duplicate Auto Quest: " + intCell + ".");
				continue;
			}
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			dictionary2.Add("did", intCell);
			dictionary2.Add("min_items", instance.GetIntCell(sheetIndex, rowIndex, "min total items"));
			dictionary2.Add("max_items", instance.GetIntCell(sheetIndex, rowIndex, "max total items"));
			dictionary2.Add("exp_multiplier", instance.GetFloatCell(sheetIndex, rowIndex, "xp multiplier"));
			dictionary2.Add("gold_multiplier", instance.GetFloatCell(sheetIndex, rowIndex, "gold multiplier"));
			dictionary2.Add("distribution", instance.GetStringCell(sheetIndex, rowIndex, "distribution"));
			dictionary2.Add("item_categories", new List<string>());
			dictionary2.Add("pick_one_categories", new List<bool>());
			for (int j = 1; j <= num2; j++)
			{
				string stringCell = instance.GetStringCell(sheetIndex, rowIndex, "item category " + j);
				if (string.IsNullOrEmpty(stringCell) || stringCell == text2)
				{
					break;
				}
				((List<string>)dictionary2["item_categories"]).Add(stringCell);
				((List<bool>)dictionary2["pick_one_categories"]).Add(instance.GetIntCell(sheetIndex, rowIndex, "pick one category " + j) == 1);
			}
			dictionary2.Add("characters", new List<int>());
			for (int k = 1; k <= num3; k++)
			{
				intCell = instance.GetIntCell(sheetIndex, rowIndex, "character " + k);
				if (intCell < 0)
				{
					break;
				}
				((List<int>)dictionary2["characters"]).Add(intCell);
			}
			dictionary.Add((int)dictionary2["did"], dictionary2);
		}
		text = "AutoQuestDialog";
		sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		int num4 = -1;
		for (int l = 0; l < num; l++)
		{
			string rowName = l.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, "id").ToString());
			if (num4 < 0)
			{
				num4 = instance.GetIntCell(sheetIndex, rowIndex, "num dialogs");
			}
			int intCell = instance.GetIntCell(sheetIndex, rowIndex, "did");
			if (!dictionary.ContainsKey(intCell))
			{
				TFUtils.ErrorLog("AutoQuestDialog: No auto quest found for did: " + intCell);
				continue;
			}
			Dictionary<string, object> dictionary2 = dictionary[intCell];
			dictionary2.Add("name", instance.GetStringCell(sheetIndex, rowIndex, "autoQuest title"));
			dictionary2.Add("description", instance.GetStringCell(sheetIndex, rowIndex, "autoQuest description"));
			Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
			for (int m = 1; m <= num4; m++)
			{
				string stringCell = instance.GetStringCell(sheetIndex, rowIndex, "intro dialog " + m);
				if (!string.IsNullOrEmpty(stringCell) && stringCell != text2)
				{
					dictionary3.Add(m.ToString(), stringCell);
				}
			}
			dictionary2.Add("intro_dialog_data", dictionary3);
			dictionary3 = new Dictionary<string, object>();
			for (int n = 1; n <= num4; n++)
			{
				string stringCell = instance.GetStringCell(sheetIndex, rowIndex, "outro dialog " + n);
				if (!string.IsNullOrEmpty(stringCell) && stringCell != text2)
				{
					dictionary3.Add(n.ToString(), stringCell);
				}
			}
			dictionary2.Add("outro_dialog_data", dictionary3);
		}
		foreach (KeyValuePair<int, Dictionary<string, object>> item in dictionary)
		{
			AutoQuestData autoQuestData = new AutoQuestData(item.Value);
			m_pAutoQuests.Add(autoQuestData.m_nDID, autoQuestData);
		}
	}
}
