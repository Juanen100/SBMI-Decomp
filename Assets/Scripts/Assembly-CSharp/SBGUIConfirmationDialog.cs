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
		base.Awake();
		messageLabel = (SBGUILabel)FindChild("message_label");
		messageLabelBoundary = (SBGUIAtlasImage)FindChild("message_label_boundary");
		titleLabel = (SBGUILabel)FindChild("title_label");
		acceptButtonLabel = (SBGUILabel)FindChild("okay_label");
		cancelButtonLabel = (SBGUILabel)FindChild("cancel_label");
		acceptButton = (SBGUIButton)FindChild("okay_button");
		cancelButton = (SBGUIButton)FindChild("cancel_button");
		originalAcceptButtonPosition = acceptButton.transform.localPosition;
	}

	private void Start()
	{
		rewardCenter = rewardMarker.tform.position;
	}

	public void SetUp(string title, string message, string acceptButtonLabel, string cancelButtonLabel, Dictionary<string, int> resources, string prefix)
	{
		titleLabel.SetText(title);
		messageLabel.SetText(message);
		messageLabel.AdjustText(messageLabelBoundary);
		this.acceptButtonLabel.SetText(acceptButtonLabel);
		if (cancelButtonLabel == null)
		{
			cancelButton.SetActive(false);
			acceptButton.tform.localPosition = new Vector3(0f, originalAcceptButtonPosition.y, originalAcceptButtonPosition.z);
		}
		else
		{
			this.cancelButtonLabel.SetText(cancelButtonLabel);
			cancelButton.SetActive(true);
			acceptButton.tform.localPosition = originalAcceptButtonPosition;
		}
		if (resources == null)
		{
			return;
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

	public float GetMainWindowZ()
	{
		return base.gameObject.transform.Find("window").localPosition.z;
	}
}
