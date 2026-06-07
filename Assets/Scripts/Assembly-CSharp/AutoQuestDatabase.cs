using System.Collections.Generic;

public class AutoQuestDatabase
{
	private Dictionary<string, List<int>> m_pCategoryItems;

	private Dictionary<int, AutoQuestData> m_pAutoQuests;

	private static List<int> m_pPreviousAutoQuests;

	private static Dictionary<int, List<int>> m_pPreviousAutoQuestCharacters;

	private const int _nNumSavedAutoQuests = 3;

	public void AddPreviousAutoQuests(int nAutoQuestDID, int nCharacterDID)
	{
	}

	public static void SetPreviousAutoQuestDataFramGameState(Dictionary<string, object> pGameState)
	{
	}

	public static void WritePreviousAutoQuestDataToGameState(Dictionary<string, object> pGameState)
	{
	}

	public AutoQuestData.DialogData GetDialogData(int nAutoQuestDID, int nCharacterDID)
	{
		return null;
	}

	public AutoQuest GenerateNextAutoQuest(Game pGame)
	{
		return null;
	}

	public bool IsQuestValid(Game pGame, QuestDefinition pQuestDef)
	{
		return false;
	}

	private void LoadDatabase()
	{
	}

	private AutoQuest GenerateAutoQuest(Game pGame, int nDID)
	{
		return null;
	}

	private int GetXpForCraftingRecipe(Game pGame, CraftingRecipe pCraftingRecipe)
	{
		return 0;
	}

	private float GetGoldForCraftingRecipe(Game pGame, CraftingRecipe pCraftingRecipe)
	{
		return 0f;
	}

	private float GetGoldForCost(Game pGame, CraftingManager pCraftingManager, Cost pCost)
	{
		return 0f;
	}

	private bool IsAutoQuestAvailable(Game pGame, int nDID)
	{
		return false;
	}

	private void LoadCategories()
	{
	}

	private void LoadAutoQuests()
	{
	}
}
