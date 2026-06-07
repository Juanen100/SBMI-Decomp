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
	}

	private void Start()
	{
	}

	public void SetUp(string title, string message, string storeLabel, Dictionary<string, int> resources, int? rmtCost, string rmtTexture, string prefix)
	{
	}

	public override void AddItem(string texture, int amount, string prefix)
	{
	}

	private new void CenterRewards()
	{
	}

	public Vector2 GetHardSpendPosition()
	{
		return default(Vector2);
	}
}
