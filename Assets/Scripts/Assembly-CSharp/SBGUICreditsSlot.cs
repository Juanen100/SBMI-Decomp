using UnityEngine;

public class SBGUICreditsSlot : SBGUIScrollListElement
{
	public static SBGUICreditsSlot MakeCreditsSlot()
	{
		SBGUICreditsSlot sBGUICreditsSlot = (SBGUICreditsSlot)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/CreditsSlot");
		sBGUICreditsSlot.name = "CreditsSlot";
		sBGUICreditsSlot.gameObject.SetActiveRecursively(false);
		sBGUICreditsSlot.gameObject.transform.parent = GUIMainView.GetInstance().gameObject.transform;
		return sBGUICreditsSlot;
	}

	public static SBGUICreditsSlot MakeCreditsSlot1()
	{
		SBGUICreditsSlot sBGUICreditsSlot = (SBGUICreditsSlot)SBGUI.InstantiatePrefab("Prefabs/CreditsSlot1");
		sBGUICreditsSlot.name = "CreditsSlot1";
		sBGUICreditsSlot.gameObject.SetActiveRecursively(false);
		sBGUICreditsSlot.gameObject.transform.parent = GUIMainView.GetInstance().gameObject.transform;
		return sBGUICreditsSlot;
	}

	public static SBGUICreditsSlot MakeCreditsSlot2()
	{
		SBGUICreditsSlot sBGUICreditsSlot = (SBGUICreditsSlot)SBGUI.InstantiatePrefab("Prefabs/CreditsSlot2");
		sBGUICreditsSlot.name = "CreditsSlot2";
		sBGUICreditsSlot.gameObject.SetActiveRecursively(false);
		sBGUICreditsSlot.gameObject.transform.parent = GUIMainView.GetInstance().gameObject.transform;
		return sBGUICreditsSlot;
	}

	public void Setup(Session session, SBGUIScreen screen, SBGUIElement parent, Vector3 offset)
	{
		SetParent(parent);
		base.transform.localPosition = offset;
	}
}
