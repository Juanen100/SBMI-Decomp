using System.Collections.Generic;
using UnityEngine;

public class SBGUIGetJellyDialog : SBGUIModalDialog
{
	private SBGUILabel messageLabel;

	private SBGUILabel questionLabel;

	private SBGUILabel titleLabel;

	private SBGUILabel acceptButtonLabel;

	private SBGUILabel cancelButtonLabel;

	private SBGUIButton acceptButton;

	private SBGUIButton cancelButton;

	private Vector3 originalAcceptButtonPosition;

	protected override void Awake()
	{
		base.Awake();
		messageLabel = (SBGUILabel)FindChild("message_label");
		questionLabel = (SBGUILabel)FindChild("question_label");
		titleLabel = (SBGUILabel)FindChild("title_label");
		acceptButtonLabel = (SBGUILabel)FindChild("okay_label");
		cancelButtonLabel = (SBGUILabel)FindChild("cancel_label");
		acceptButton = (SBGUIButton)FindChild("okay_button");
		cancelButton = (SBGUIButton)FindChild("cancel_button");
		originalAcceptButtonPosition = acceptButton.transform.localPosition;
	}

	private void Start()
	{
	}

	public void SetUp(string title, string message, string question, string acceptButtonLabel, string cancelButtonLabel, Dictionary<string, int> resources)
	{
		titleLabel.SetText(title);
		messageLabel.SetText(message);
		questionLabel.SetText(question);
		this.acceptButtonLabel.SetText(acceptButtonLabel);
		this.cancelButtonLabel.SetText(cancelButtonLabel);
		cancelButton.SetActive(true);
		acceptButton.tform.localPosition = originalAcceptButtonPosition;
	}

	public float GetMainWindowZ()
	{
		return base.gameObject.transform.Find("window").localPosition.z;
	}
}
