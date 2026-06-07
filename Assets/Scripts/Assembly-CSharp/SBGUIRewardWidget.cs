using UnityEngine;

public class SBGUIRewardWidget : SBGUIAtlasImage
{
	private SBGUILabel prefixLabel;

	private SBGUILabel label;

	private static int sNumAllocations;

	protected static TFPool<SBGUIRewardWidget> widgetsPool;

	public override int Width
	{
		get
		{
			return 0;
		}
	}

	private static SBGUIRewardWidget Alloc()
	{
		return null;
	}

	public static void MakeRewardWidgetPool()
	{
	}

	protected override void Awake()
	{
	}

	public void DetailedSetup(GameObject prefab, SBGUIElement parent, float xOffset, string texture, int amount, string prefix)
	{
	}

	public static SBGUIRewardWidget Create(GameObject prefab, SBGUIElement parent, float xOffset, string texture, int amount, string prefix)
	{
		return null;
	}

	public void BriefSetup(SBGUIElement parent, float xOffset)
	{
	}

	public static SBGUIRewardWidget Create(SBGUIElement parent, float xOffset)
	{
		return null;
	}

	public void SetText(string text, bool dim = false)
	{
	}

	public void SetPrefixText(string text, bool dim = false)
	{
	}

	public void SetTextScale(float scale)
	{
	}

	public void SetTextColor(Color color)
	{
	}

	public void CreateTextStroke(Color color)
	{
	}

	public static void SetupRewardWidget(ResourceManager resMgr, Reward reward, string prefix, int maxCount, SBGUIElement marker, float rewardGapSize, bool dim, Color textColor, bool useCache = false, float scale = 1f)
	{
	}

	private static void ResetWidget(SBGUIRewardWidget rewardWidget)
	{
	}

	public static void ClearWidgetPool()
	{
	}

	public static void ReleaseRewardWidget(SBGUIRewardWidget widget)
	{
	}

	private static void AddRewardWidget(string texture, string text, string prefix, SBGUIElement marker, ref float markerXOffset, ref int rewardCount, float rewardGapSize, Color textColor, bool dim, bool useCache, float scale)
	{
	}
}
