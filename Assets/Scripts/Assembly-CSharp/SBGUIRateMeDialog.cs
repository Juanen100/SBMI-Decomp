using UnityEngine;

public class SBGUIRateMeDialog : SBGUIModalDialog
{
	private SBGUILabel messageLabel;

	private SBGUIButton acceptButton;

	private SBGUIButton cancelButton;

	private SBGUIButton laterButton;

	private SBGUILabel laterButtonLabel;

	private SBGUIAtlasImage icon;

	private Vector3 originalAcceptButtonPosition;

	private Vector3 originalCancelButtonPosition;

	private Vector3 rewardCenter;

	public int Stage;

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	public void UpdateDialog(int stage)
	{
	}

	public float GetMainWindowZ()
	{
		return 0f;
	}
}
