using System.Collections.Generic;
using UnityEngine;

public class SBGUIConfirmationDialog : SBGUIModalDialog
{
	private SBGUILabel messageLabel;

	private SBGUIAtlasImage messageLabelBoundary;

	private SBGUILabel titleLabel;

	private SBGUILabel acceptButtonLabel;

	private SBGUILabel cancelButtonLabel;

	private SBGUIButton acceptButton;

	private SBGUIButton cancelButton;

	private Vector3 originalAcceptButtonPosition;

	private Vector3 rewardCenter;

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	public void SetUp(string title, string message, string acceptButtonLabel, string cancelButtonLabel, Dictionary<string, int> resources, string prefix)
	{
	}

	public override void AddItem(string texture, int amount, string prefix)
	{
	}

	private new void CenterRewards()
	{
	}

	public float GetMainWindowZ()
	{
		return 0f;
	}
}
