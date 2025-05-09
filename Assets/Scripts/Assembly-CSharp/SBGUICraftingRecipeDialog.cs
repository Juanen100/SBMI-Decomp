#define ASSERTS_ON
using System.Collections.Generic;
using UnityEngine;

public class SBGUICraftingRecipeDialog : SBGUIElement
{
	private const int MAX_REWARDS = 1;

	private const int REWARD_GAP_SIZE = 10;

	private static readonly Color rewardColor = new Color(0.384f, 0.133f, 0.09f, 0.5f);

	public GameObject craftingIngredientPrefab;

	private SBGUILabel nameLabel;

	private SBGUILabel cookTimeLabel;

	private SBGUIAtlasImage cookTimeIcon;

	private SBGUIAtlasImage topSecretTreatment;

	protected SBGUIElement rewardMarker;

	private SBGUIAtlasImage ingredientAreaImage;

	private float ingredientWidgetWidth;

	private Vector2 ingredientAreaDimensions;

	private Stack<SBGUICraftingIngredient> emptyIngredientPool = new Stack<SBGUICraftingIngredient>();

	private Stack<SBGUICraftingIngredient> activeIngredientPool = new Stack<SBGUICraftingIngredient>();

	public void Init()
	{
		nameLabel = (SBGUILabel)FindChild("name_label");
		cookTimeLabel = (SBGUILabel)FindChild("time_label");
		cookTimeIcon = (SBGUIAtlasImage)FindChild("timer_icon");
		topSecretTreatment = (SBGUIAtlasImage)FindChild("top_secret_treatment");
		topSecretTreatment.SetVisible(false);
		rewardMarker = FindChild("reward_marker");
		ingredientAreaImage = (SBGUIAtlasImage)FindChild("ingredient_area");
		ingredientAreaDimensions = new Vector2(ingredientAreaImage.Size.x, ingredientAreaImage.Size.y);
		YGAtlasSprite component = craftingIngredientPrefab.GetComponent<YGAtlasSprite>();
		ingredientWidgetWidth = component.size.x;
	}

	private void CreateCraftingIngredient(ResourceManager resMgr, SBGUIElement parent, int resourceId, int price, Vector3 offset)
	{
		SBGUICraftingIngredient sBGUICraftingIngredient;
		if (emptyIngredientPool.Count > 0)
		{
			sBGUICraftingIngredient = emptyIngredientPool.Pop();
			sBGUICraftingIngredient.MuteButtons(false);
			sBGUICraftingIngredient.Setup(resMgr, parent, resourceId, price, offset);
			sBGUICraftingIngredient.SetActive(true);
		}
		else
		{
			sBGUICraftingIngredient = SBGUICraftingIngredient.Create(resMgr, parent, resourceId, price, offset);
		}
		activeIngredientPool.Push(sBGUICraftingIngredient);
	}

	public void Setup(CraftingRecipe recipe, ResourceManager resourceManager)
	{
		TFUtils.Assert(craftingIngredientPrefab != null, "The Crafting Ingredient Prefab has not been set in the CraftingRecipeDialog prefab.");
		nameLabel.SetText(Language.Get(recipe.recipeName));
		cookTimeLabel.SetText(TFUtils.DurationToString(recipe.craftTime));
		cookTimeIcon.SetActive(true);
		ResetIngredientArea();
		int count = recipe.cost.ResourceAmounts.Count;
		float num = 0f;
		float num2 = 0f;
		int num3 = 0;
		int num4 = 0;
		float num5 = 0.55f;
		if (resourceManager.Resources[recipe.productId].Reward != null)
		{
			SBGUIRewardWidget.SetupRewardWidget(resourceManager, resourceManager.Resources[recipe.productId].Reward.Summary, Language.Get("!!PREFAB_EARNS"), 1, rewardMarker, 10f, false, rewardColor, false, 0.85f);
		}
		switch (count)
		{
		case 1:
			num = (ingredientAreaDimensions.x - (float)cookTimeIcon.Width / 2f) / 2f * 0.01f;
			num2 = (0f - ingredientAreaDimensions.y) / 2f * 0.01f;
			if (recipe.productId == 90)
			{
				topSecretTreatment.SetVisible(true);
				num2 -= 0.5f;
			}
			foreach (KeyValuePair<int, int> resourceAmount in recipe.cost.ResourceAmounts)
			{
				CreateCraftingIngredient(resourceManager, ingredientAreaImage, resourceAmount.Key, resourceAmount.Value, new Vector3(num, num2, -0.1f));
			}
			break;
		case 2:
			num = num5;
			num2 = (0f - ingredientAreaDimensions.y) / 2f * 0.01f;
			foreach (KeyValuePair<int, int> resourceAmount2 in recipe.cost.ResourceAmounts)
			{
				CreateCraftingIngredient(resourceManager, ingredientAreaImage, resourceAmount2.Key, resourceAmount2.Value, new Vector3(num, num2, -0.1f));
				num += (ingredientWidgetWidth + 2f) * 0.01f;
			}
			break;
		case 3:
		case 4:
			num3 = 1;
			num4 = 0;
			num = num5;
			foreach (KeyValuePair<int, int> resourceAmount3 in recipe.cost.ResourceAmounts)
			{
				num2 = (0f - (ingredientAreaDimensions.y - (float)cookTimeIcon.Height)) / 2f * 0.01f * (float)num3;
				CreateCraftingIngredient(resourceManager, ingredientAreaImage, resourceAmount3.Key, resourceAmount3.Value, new Vector3(num, num2, -0.1f));
				num += (ingredientWidgetWidth + 2f) * 0.01f;
				num4++;
				if (num4 % 2 == 0)
				{
					num3++;
					num = num5;
				}
			}
			break;
		case 5:
		case 6:
			num3 = 1;
			num = num5;
			num4 = 0;
			foreach (KeyValuePair<int, int> resourceAmount4 in recipe.cost.ResourceAmounts)
			{
				num2 = (0f - (ingredientAreaDimensions.y - (float)cookTimeIcon.Height / 2f)) / 3f * 0.01f * (float)num3;
				CreateCraftingIngredient(resourceManager, ingredientAreaImage, resourceAmount4.Key, resourceAmount4.Value, new Vector3(num, num2, -0.1f));
				num += (ingredientWidgetWidth + 2f) * 0.01f;
				num4++;
				if (num4 % 2 == 0)
				{
					num3++;
					num = num5;
				}
			}
			break;
		default:
			TFUtils.Assert(false, "SBGUICraftingRecipeDialog doesn't support recipes with over 6 ingredients.");
			break;
		}
		SetActive(true);
	}

	private void ResetIngredientArea()
	{
		topSecretTreatment.SetVisible(false);
		SBGUIRewardWidget[] componentsInChildren = base.gameObject.GetComponentsInChildren<SBGUIRewardWidget>(true);
		SBGUIRewardWidget[] array = componentsInChildren;
		foreach (SBGUIRewardWidget sBGUIRewardWidget in array)
		{
			sBGUIRewardWidget.SetParent(null);
			sBGUIRewardWidget.gameObject.SetActiveRecursively(false);
			SBGUIRewardWidget.ReleaseRewardWidget(sBGUIRewardWidget);
		}
		while (activeIngredientPool.Count > 0)
		{
			SBGUICraftingIngredient sBGUICraftingIngredient = activeIngredientPool.Pop();
			sBGUICraftingIngredient.SetActive(false);
			sBGUICraftingIngredient.SetParent(null);
			emptyIngredientPool.Push(sBGUICraftingIngredient);
		}
	}

	public void Deactivate()
	{
		ResetIngredientArea();
		SetActive(false);
	}

	public void Deselect()
	{
		ResetIngredientArea();
		nameLabel.SetText(string.Empty);
		cookTimeLabel.SetText(string.Empty);
		cookTimeIcon.SetActive(false);
	}
}
