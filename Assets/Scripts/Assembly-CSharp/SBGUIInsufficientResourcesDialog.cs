using System.Collections.Generic;
using UnityEngine;

public class SBGUIInsufficientResourcesDialog : SBGUIModalDialog
{
	private SBGUILabel messageLabel;

	private SBGUILabel titleLabel;

	private SBGUILabel storeButtonLabel;

	private SBGUILabel buyWithLabel;

	private SBGUIElement costMarker;

	private SBGUIRewardWidget rmtCost;

	private Vector3 rewardCenter;

	protected override void Awake()
	{
		base.Awake();
		messageLabel = (SBGUILabel)FindChild("message_label");
		titleLabel = (SBGUILabel)FindChild("title_label");
		costMarker = FindChild("cost_marker");
		storeButtonLabel = (SBGUILabel)FindChild("shopping_label");
		buyWithLabel = (SBGUILabel)FindChild("cost_label");
	}

	private void Start()
	{
		rewardCenter = rewardMarker.tform.position;
	}

	public void SetUp(string title, string message, string storeLabel, Dictionary<string, int> resources, int? rmtCost, string rmtTexture, string prefix)
	{
		titleLabel.SetText(title);
		messageLabel.SetText(message);
		if (buyWithLabel != null)
		{
			buyWithLabel.SetText(Language.Get("!!PREFAB_BUY_WITH"));
		}
		if (storeButtonLabel != null)
		{
			storeButtonLabel.SetText(storeLabel);
		}
		if (resources == null)
		{
			return;
		}
		if (rmtCost.HasValue)
		{
			SBGUIRewardWidget.Create(rewardWidgetPrefab, costMarker, 0f, rmtTexture, rmtCost.Value, string.Empty);
		}
		foreach (KeyValuePair<string, int> resource in resources)
		{
			AddItem(resource.Key, resource.Value, prefix);
		}
	}

	public override void AddItem(string texture, int amount, string prefix)
	{
		base.AddItem(texture, amount, prefix);
		base.View.RefreshEvent += CenterRewards;
	}

	private new void CenterRewards()
	{
		Vector3 vector = rewardMarker.TotalBounds.center - rewardCenter;
		Vector3 localPosition = rewardMarker.tform.localPosition;
		localPosition.x -= vector.x;
		rewardMarker.tform.localPosition = localPosition;
	}

	public Vector2 GetHardSpendPosition()
	{
		Vector2 result = base.View.WorldToScreen(storeButtonLabel.transform.position);
		result.y = (float)Screen.height - result.y;
		return result;
	}
}
