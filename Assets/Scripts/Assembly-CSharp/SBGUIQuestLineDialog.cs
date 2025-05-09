using UnityEngine;

public class SBGUIQuestLineDialog : SBGUIModalDialog
{
	private SBGUIElement rewardWindow;

	private int? prefabIconSize;

	protected override void Awake()
	{
		rewardWindow = FindChild("reward_window");
		base.Awake();
	}

	public override void SetParent(SBGUIElement element)
	{
		SetTransformParent(element);
	}

	public void SetupQuestLineDialogInfo(string dialogHeading, string dialogBody, string portrait, string rewardTexture, string rewardName)
	{
		SBGUIShadowedLabel sBGUIShadowedLabel = (SBGUIShadowedLabel)FindChild("dialog_heading");
		SBGUIShadowedLabel sBGUIShadowedLabel2 = (SBGUIShadowedLabel)FindChild("banner_label");
		SBGUILabel sBGUILabel = (SBGUILabel)FindChild("dialog_body");
		SBGUIAtlasImage sBGUIAtlasImage = (SBGUIAtlasImage)FindChild("portrait");
		SBGUIAtlasImage sBGUIAtlasImage2 = (SBGUIAtlasImage)FindChild("portrait_shadow");
		SBGUIAtlasImage boundary = (SBGUIAtlasImage)FindChild("dialog_body_boundary");
		SBGUIAtlasImage sBGUIAtlasImage3 = (SBGUIAtlasImage)FindChild("reward_icon");
		int? num = prefabIconSize;
		if (!num.HasValue)
		{
			prefabIconSize = (int)sBGUIAtlasImage3.Size.x;
		}
		sBGUIShadowedLabel.SetText(Language.Get(dialogHeading));
		sBGUILabel.SetText(Language.Get(dialogBody));
		sBGUILabel.AdjustText(boundary);
		if (rewardName != string.Empty)
		{
			string text = string.Format(Language.Get("!!EARNED_A_THING_DIALOG"), rewardName);
			text = sBGUIShadowedLabel2.textSprite.StripScalarDataFromString(text, false);
			sBGUIShadowedLabel2.SetText(text);
		}
		else
		{
			string text2 = Language.Get("!!YOU_WILL_EARN_A_THING_DIALOG");
			text2 = sBGUIShadowedLabel2.textSprite.StripScalarDataFromString(text2, false);
			sBGUIShadowedLabel2.SetText(text2);
		}
		sBGUIAtlasImage.SetTextureFromAtlas(portrait, true, false, true);
		sBGUIAtlasImage2.GetComponent<Renderer>().material.SetColor("_Color", new Color(0f, 0f, 0f, 0.2f));
		sBGUIAtlasImage3.SetTextureFromAtlas(rewardTexture, true, false, true);
	}

	public void ToggleRewardWindow(bool enabled)
	{
		rewardWindow.SetActive(enabled);
	}
}
