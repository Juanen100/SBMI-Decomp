using System;
using System.Collections.Generic;
using UnityEngine;

public class SBGUICharacterIdleScreen : SBGUIScrollableDialog
{
	public const int STEP_GAP = 20;

	private const float CHECKBOX_GAP = 0.7f;

	private const float TEXT_GAP = 40f;

	private Vector3 checkBoxLocPos;

	private int costumeCount;

	private Vector2 scrollSize;

	private SBGUIAtlasImage window;

	private SBGUIElement stepsMarker;

	private SBGUILabel m_pCharacterNameLabel;

	private SBGUILabel m_pCostumeUnlockLabel;

	private SBGUILabel m_pTasksTitleLabel;

	private SBGUICharacterWishWidget m_pCharacterWishWidget;

	private Action<int> m_pDoTaskAction;

	private SBGUIArrowList m_pArrowList;

	private List<TaskData> m_pTaskDatas;

	private SBGUIAtlasImage m_pDialogueBubble;

	private List<SBGUIAtlasImage> checkBoxes;

	private List<SBGUIAtlasImage> ticks;

	private List<SBGUILabel> popupTexts;

	public int? m_nCostumeDID { get; private set; }

	public override void SetParent(SBGUIElement element)
	{
	}

	public void CreateScrollRegionUI(List<TaskData> pTaskDatas)
	{
	}

	public void SetupDialogInfo(Simulated pSimulated, Action pFeedWishAction, Action pRushWishAction, Action<int> pDoTaskAction, Action onWatchAd)
	{
	}

	private void SetupDialogBubble(int costumeDID)
	{
	}

	private void PositionObject(SBGUIElement obj, Vector3 loc)
	{
	}

	public void ClearList()
	{
	}

	private float GenerateTextandTick(CostumeManager.Costume costume)
	{
		return 0f;
	}

	private void CreateCheckBox(int count)
	{
	}

	public Vector2 GetWishWidgetRushButtonPosition()
	{
		return default(Vector2);
	}

	public void SetArrowList(int id)
	{
	}
}
