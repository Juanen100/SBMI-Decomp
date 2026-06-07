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
	}

	private void Start()
	{
	}

	public void SetUp(string title, string message, string question, string acceptButtonLabel, string cancelButtonLabel, Dictionary<string, int> resources)
	{
	}

	public float GetMainWindowZ()
	{
		return 0f;
	}
}
