public class SBGUIClearingScreen : SBGUIModalDialog
{
	private SBGUILabel messageLabel;

	private SBGUILabel titleLabel;

	private SBGUILabel costLabel;

	protected override void Awake()
	{
		base.Awake();
		messageLabel = (SBGUILabel)FindChild("message_label");
		titleLabel = (SBGUILabel)FindChild("title_label");
		costLabel = (SBGUILabel)FindChild("cost_label");
	}

	public void SetUp(string title, string message, string cost)
	{
		titleLabel.SetText(title);
		messageLabel.SetText(message);
		costLabel.SetText(cost);
	}
}
