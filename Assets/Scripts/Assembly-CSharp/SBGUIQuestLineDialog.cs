public class SBGUIQuestLineDialog : SBGUIModalDialog
{
	private SBGUIElement rewardWindow;

	private int? prefabIconSize;

	protected override void Awake()
	{
	}

	public override void SetParent(SBGUIElement element)
	{
	}

	public void SetupQuestLineDialogInfo(string dialogHeading, string dialogBody, string portrait, string rewardTexture, string rewardName)
	{
	}

	public void ToggleRewardWindow(bool enabled)
	{
	}
}
