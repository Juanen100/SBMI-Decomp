using UnityEngine;

public class SBGUILevelUpSlot : SBGUIElement
{
	public const int GAP_SIZE = 3;

	public static SBGUILevelUpSlot Create(Session session, SBGUIScreen screen, SBGUIElement parent, Vector3 offset, string iconTexture)
	{
		SBGUILevelUpSlot sBGUILevelUpSlot = (SBGUILevelUpSlot)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/LevelUpSlot");
		sBGUILevelUpSlot.Setup(session, screen, parent, offset, iconTexture);
		return sBGUILevelUpSlot;
	}

	public void Setup(Session session, SBGUIScreen screen, SBGUIElement parent, Vector3 offset, string iconTexture)
	{
		base.name = string.Format("Slot_{0}", iconTexture);
		SetParent(parent);
		base.transform.localPosition = offset;
		SBGUIAtlasImage sBGUIAtlasImage = (SBGUIAtlasImage)FindChild("icon");
		sBGUIAtlasImage.SetTextureFromAtlas(iconTexture, true, false, true);
	}
}
