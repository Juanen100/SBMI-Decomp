public class SBGUIMoveInDialog : SBGUIModalDialog
{
	private const int ICON_SIZE = 128;

	private SBGUILabel characterMessage;

	private SBGUILabel buildingMessage;

	private SBGUIAtlasImage portrait;

	public void Setup(string characterName, string buildingName, string portraitTexture)
	{
	}

	public override void SetParent(SBGUIElement element)
	{
	}
}
