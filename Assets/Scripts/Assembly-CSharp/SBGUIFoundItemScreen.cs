public class SBGUIFoundItemScreen : SBGUIModalDialog
{
	protected override void Awake()
	{
		rewardMarker = FindChild("reward_marker");
		base.Awake();
	}

	public override void SetParent(SBGUIElement element)
	{
		SetTransformParent(element);
	}

	public void Setup(string title, string message, string texture, bool useExtraButton = false, string extraButtonText = "")
	{
		SBGUILabel sBGUILabel = (SBGUILabel)FindChild("title");
		SBGUILabel sBGUILabel2 = (SBGUILabel)FindChild("message_label");
		SBGUIAtlasImage sBGUIAtlasImage = (SBGUIAtlasImage)FindChild("icon");
		SBGUIButton sBGUIButton = (SBGUIButton)FindChild("extra_button");
		if (useExtraButton)
		{
			sBGUIButton.SetActive(true);
			SBGUILabel sBGUILabel3 = (SBGUILabel)sBGUIButton.FindChild("extra_label");
			sBGUILabel3.SetText(extraButtonText);
		}
		else
		{
			sBGUIButton.SetActive(false);
		}
		sBGUILabel.SetText(title);
		sBGUILabel2.SetText(message);
		sBGUIAtlasImage.SetTextureFromAtlas(texture, true, false, true);
	}
}
