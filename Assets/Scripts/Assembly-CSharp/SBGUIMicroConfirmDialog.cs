public class SBGUIMicroConfirmDialog : SBGUIScreen
{
	private SBGUIShadowedLabel hardAmountLabel;

	protected override void Awake()
	{
		base.Awake();
		hardAmountLabel = FindChild("amount").GetComponent<SBGUIShadowedLabel>();
	}

	public void SetHardAmount(int amount)
	{
		hardAmountLabel.Text = amount + "?";
	}
}
