using System.Collections.Generic;
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
			int num = prefixLabel.Width + label.Width;
			return Mathf.CeilToInt(base.sprite.size.x + 5f + (float)num);
		}
	}

	private static SBGUIRewardWidget Alloc()
	{
		SBGUIRewardWidget sBGUIRewardWidget = (SBGUIRewardWidget)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/RewardWidget");
		sBGUIRewardWidget.name = "RewardWidget_" + sNumAllocations++;
		sBGUIRewardWidget.gameObject.SetActiveRecursively(false);
		sBGUIRewardWidget.transform.parent = GUIMainView.GetInstance().gameObject.transform;
		return sBGUIRewardWidget;
	}

	public static void MakeRewardWidgetPool()
	{
		widgetsPool = TFPool<SBGUIRewardWidget>.CreatePool(20, Alloc);
	}

	protected override void Awake()
	{
		label = (SBGUILabel)FindChild("reward_label");
		prefixLabel = (SBGUILabel)FindChild("reward_prefix_label");
		base.Awake();
	}

	public void DetailedSetup(GameObject prefab, SBGUIElement parent, float xOffset, string texture, int amount, string prefix)
	{
		base.name = string.Format("Reward_{0}_{1}", xOffset, amount);
		SetParent(parent);
		base.tform.localPosition = new Vector3(xOffset, 0f, 0f);
		SetTextureFromAtlas(texture);
		SetPrefixText(prefix);
		SetText(amount.ToString());
	}

	public static SBGUIRewardWidget Create(GameObject prefab, SBGUIElement parent, float xOffset, string texture, int amount, string prefix)
	{
		GameObject gameObject = (GameObject)Object.Instantiate(prefab);
		SBGUIRewardWidget component = gameObject.GetComponent<SBGUIRewardWidget>();
		component.DetailedSetup(prefab, parent, xOffset, texture, amount, prefix);
		return component;
	}

	public void BriefSetup(SBGUIElement parent, float xOffset)
	{
		base.name = string.Format("Reward");
		SetParent(parent);
		base.tform.localPosition = new Vector3(xOffset, 0f, 0f);
	}

	public static SBGUIRewardWidget Create(SBGUIElement parent, float xOffset)
	{
		SBGUIRewardWidget sBGUIRewardWidget = (SBGUIRewardWidget)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/RewardWidget");
		sBGUIRewardWidget.BriefSetup(parent, xOffset);
		return sBGUIRewardWidget;
	}

	public void SetText(string text, bool dim = false)
	{
		label.SetText(text);
		if (dim)
		{
			label.SetAlpha(0.2f);
		}
	}

	public void SetPrefixText(string text, bool dim = false)
	{
		prefixLabel.SetText(text);
		if (dim)
		{
			prefixLabel.SetAlpha(0.2f);
		}
	}

	public void SetTextScale(float scale)
	{
		prefixLabel.textSprite.scale = new Vector2(scale, scale);
		label.textSprite.scale = new Vector2(scale, scale);
	}

	public void SetTextColor(Color color)
	{
		color.a = 0.5f;
		label.SetColor(color);
		prefixLabel.SetColor(color);
	}

	public void CreateTextStroke(Color color)
	{
		float num = 0.02f;
		List<SBGUILabel> list = new List<SBGUILabel>();
		List<Vector3> list2 = new List<Vector3>();
		list2.Add(new Vector3(num, 0f, 0.01f));
		list2.Add(new Vector3(0f - num, 0f, 0.01f));
		list2.Add(new Vector3(0f, num, 0.01f));
		list2.Add(new Vector3(0f, 0f - num, 0.01f));
		for (int i = 0; i < 4; i++)
		{
			SBGUILabel sBGUILabel = (SBGUILabel)Object.Instantiate(label, label.tform.position, label.tform.rotation);
			sBGUILabel.name = "reward_label_outline" + i;
			list.Add(sBGUILabel);
		}
		int num2 = 0;
		foreach (SBGUILabel item in list)
		{
			item.SetParent(label);
			item.SetColor(color);
			item.tform.localPosition = list2[num2];
			num2++;
		}
	}

	public static void SetupRewardWidget(ResourceManager resMgr, Reward reward, string prefix, int maxCount, SBGUIElement marker, float rewardGapSize, bool dim, Color textColor, bool useCache = false, float scale = 1f)
	{
		float markerXOffset = 0f;
		int rewardCount = 0;
		if (reward.ResourceAmounts != null)
		{
			foreach (KeyValuePair<int, int> resourceAmount in reward.ResourceAmounts)
			{
				string resourceTexture = resMgr.Resources[resourceAmount.Key].GetResourceTexture();
				AddRewardWidget(resourceTexture, resourceAmount.Value.ToString(), prefix, marker, ref markerXOffset, ref rewardCount, rewardGapSize, textColor, dim, useCache, scale);
				if (rewardCount >= maxCount)
				{
					return;
				}
			}
		}
		if (reward.BuildingAmounts == null)
		{
			return;
		}
		foreach (KeyValuePair<int, int> buildingAmount in reward.BuildingAmounts)
		{
			Blueprint blueprint = EntityManager.GetBlueprint("building", buildingAmount.Key);
			string texture = (string)blueprint.Invariable["portrait"];
			AddRewardWidget(texture, buildingAmount.Value.ToString(), prefix, marker, ref markerXOffset, ref rewardCount, rewardGapSize, textColor, dim, useCache, scale);
			if (rewardCount >= maxCount)
			{
				break;
			}
		}
	}

	private static void ResetWidget(SBGUIRewardWidget rewardWidget)
	{
		rewardWidget.muted = false;
		rewardWidget.SetParent(null);
		rewardWidget.SetActive(false);
	}

	public static void ClearWidgetPool()
	{
		widgetsPool.Clear(ResetWidget);
	}

	public static void ReleaseRewardWidget(SBGUIRewardWidget widget)
	{
		widget.muted = false;
		widgetsPool.Release(widget);
	}

	private static void AddRewardWidget(string texture, string text, string prefix, SBGUIElement marker, ref float markerXOffset, ref int rewardCount, float rewardGapSize, Color textColor, bool dim, bool useCache, float scale)
	{
		SBGUIRewardWidget sBGUIRewardWidget;
		if (useCache)
		{
			sBGUIRewardWidget = widgetsPool.Create(Alloc);
			sBGUIRewardWidget.gameObject.SetActiveRecursively(true);
			sBGUIRewardWidget.BriefSetup(marker, markerXOffset);
		}
		else
		{
			sBGUIRewardWidget = Create(marker, markerXOffset);
		}
		sBGUIRewardWidget.SetTextureFromAtlas(texture);
		sBGUIRewardWidget.SetTextScale(scale);
		sBGUIRewardWidget.SetTextColor(textColor);
		sBGUIRewardWidget.SetPrefixText(prefix, dim);
		sBGUIRewardWidget.SetText(text, dim);
		if (dim)
		{
			sBGUIRewardWidget.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 0.2f);
		}
		markerXOffset += ((float)sBGUIRewardWidget.Width + rewardGapSize) * 0.01f;
		rewardCount++;
	}
}
