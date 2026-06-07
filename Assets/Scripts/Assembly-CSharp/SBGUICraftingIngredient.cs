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

	private static readonly Color sufficientColor;

	private static readonly Color insufficientColor;

	public static SBGUICraftingIngredient Create(ResourceManager resMgr, SBGUIElement parent, int resourceId, int price, Vector3 offset)
	{
		return null;
	}

	public void Setup(ResourceManager resMgr, SBGUIElement parent, int resourceId, int price, Vector3 offset)
	{
	}

	public void Update()
	{
	}
}
