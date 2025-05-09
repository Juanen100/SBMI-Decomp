public class SBGUIAcceptUI : SBGUIScreen
{
	public override void Deactivate()
	{
		ClearButtonActions("accept");
		base.Deactivate();
	}
}
