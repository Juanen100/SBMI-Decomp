using UnityEngine;

public class SBGUICraftingIngredient : SBGUIElement
{
	public const int GAP_SIZE = 2;

	public const int TEXT_SPACING = 2;

	public Vector3 startingUIPosition;

	private SBGUIAtlasImage resourceIcon;

	private SBGUILabel resourceCost;

	private SBGUILabel resourceOwned;

	private int cost;

	private int resourceId;

	private ResourceManager resourceManager;

	private bool update;

	private static readonly Color sufficientColor = new Color(0.384f, 0.133f, 0.09f, 0.5f);

	private static readonly Color insufficientColor = new Color(1f, 0.486f, 0.412f, 0.5f);

	public static SBGUICraftingIngredient Create(ResourceManager resMgr, SBGUIElement parent, int resourceId, int price, Vector3 offset)
	{
		SBGUICraftingIngredient sBGUICraftingIngredient = (SBGUICraftingIngredient)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/CraftingIngredient");
		sBGUICraftingIngredient.resourceIcon = (SBGUIAtlasImage)sBGUICraftingIngredient.FindChild("icon");
		sBGUICraftingIngredient.resourceCost = (SBGUILabel)sBGUICraftingIngredient.FindChild("cost_label");
		sBGUICraftingIngredient.resourceOwned = (SBGUILabel)sBGUICraftingIngredient.FindChild("owned_label");
		sBGUICraftingIngredient.startingUIPosition = sBGUICraftingIngredient.resourceIcon.tform.localPosition;
		sBGUICraftingIngredient.Setup(resMgr, parent, resourceId, price, offset);
		return sBGUICraftingIngredient;
	}

	public void Setup(ResourceManager resMgr, SBGUIElement parent, int resourceId, int price, Vector3 offset)
	{
		SetParent(parent);
		base.transform.localPosition = offset;
		cost = price;
		int num = resMgr.Query(resourceId);
		resourceManager = resMgr;
		this.resourceId = resourceId;
		resourceIcon.SetTextureFromAtlas(resMgr.Resources[resourceId].GetResourceTexture());
		if (resourceId == ResourceManager.SOFT_CURRENCY || resourceId == ResourceManager.HARD_CURRENCY)
		{
			update = false;
			resourceOwned.SetText(Language.Get("!!PREFAB_COSTS"));
			resourceOwned.SetColor(new Color(0.384f, 0.133f, 0.09f, 0.5f));
			resourceCost.SetText(cost.ToString());
			resourceOwned.tform.localPosition = startingUIPosition;
			Vector3 localPosition = resourceOwned.tform.localPosition;
			localPosition.x += (float)(resourceOwned.Width + 4) * 0.01f;
			resourceCost.tform.localPosition = localPosition;
			localPosition = resourceCost.tform.localPosition;
			localPosition.x += (float)(resourceCost.Width + 2) * 0.01f;
			resourceIcon.tform.localPosition = localPosition;
			return;
		}
		update = true;
		if (num > cost)
		{
			num = cost;
		}
		resourceOwned.SetText(num.ToString());
		resourceCost.SetText("/" + cost);
		resourceIcon.tform.localPosition = startingUIPosition;
		Vector3 localPosition2 = resourceIcon.tform.localPosition;
		localPosition2.x += (float)(resourceIcon.Width + 2) * 0.01f;
		resourceOwned.tform.localPosition = localPosition2;
		localPosition2 = resourceOwned.tform.localPosition;
		localPosition2.x += (float)(resourceOwned.Width + 2) * 0.01f;
		resourceCost.tform.localPosition = localPosition2;
		if (num < cost)
		{
			resourceOwned.SetColor(insufficientColor);
		}
		else
		{
			resourceOwned.SetColor(sufficientColor);
		}
	}

	public void Update()
	{
		if (this != null && update && resourceOwned != null && resourceManager != null)
		{
			int num = resourceManager.Query(resourceId);
			if (num > cost)
			{
				num = cost;
			}
			Vector3 localPosition = resourceOwned.tform.localPosition;
			localPosition.x += (float)(resourceOwned.Width + 2) * 0.01f;
			resourceCost.tform.localPosition = localPosition;
			resourceOwned.SetText(num.ToString());
			if (num < cost)
			{
				resourceOwned.SetColor(insufficientColor);
			}
			else
			{
				resourceOwned.SetColor(sufficientColor);
			}
		}
	}
}
