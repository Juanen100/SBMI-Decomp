public class SBGUIMoveInDialog : SBGUIModalDialog
{
	private const int ICON_SIZE = 128;

	private SBGUILabel characterMessage;

	private SBGUILabel buildingMessage;

	private SBGUIAtlasImage portrait;

	public void Setup(string characterName, string buildingName, string portraitTexture)
	{
		characterMessage = (SBGUILabel)FindChild("character_message_label");
		characterMessage.SetText(string.Format(Language.Get("!!MOVE_IN_CHARACTER_MESSAGE"), characterName));
		buildingMessage = (SBGUILabel)FindChild("building_message_label");
		buildingMessage.SetText(string.Format(Language.Get("!!MOVE_IN_BUILDING_MESSAGE"), buildingName));
		portrait = (SBGUIAtlasImage)FindChild("icon");
		portrait.SetTextureFromAtlas(portraitTexture);
		portrait.ScaleToMaxSize(128);
	}

	public override void SetParent(SBGUIElement element)
	{
		SetTransformParent(element);
	}
}
