using System.Collections.Generic;
using UnityEngine;

public class SBGUICraftingRecipeDialog : SBGUIElement
{
	private const int MAX_REWARDS = 1;

	private const int REWARD_GAP_SIZE = 10;

	private static readonly Color rewardColor;

	public GameObject craftingIngredientPrefab;

	private SBGUILabel nameLabel;

	private SBGUILabel cookTimeLabel;

	private SBGUIAtlasImage cookTimeIcon;

	private SBGUIAtlasImage topSecretTreatment;

	protected SBGUIElement rewardMarker;

	private SBGUIAtlasImage ingredientAreaImage;

	private float ingredientWidgetWidth;

	private Vector2 ingredientAreaDimensions;

	private Stack<SBGUICraftingIngredient> emptyIngredientPool;

	private Stack<SBGUICraftingIngredient> activeIngredientPool;

	public void Init()
	{
	}

	private void CreateCraftingIngredient(ResourceManager resMgr, SBGUIElement parent, int resourceId, int price, Vector3 offset)
	{
	}

	public void Setup(CraftingRecipe recipe, ResourceManager resourceManager)
	{
	}

	private void ResetIngredientArea()
	{
	}

	public void Deactivate()
	{
	}

	public void Deselect()
	{
	}
}
