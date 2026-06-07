using System;
using UnityEngine;

public class SBGUITaskWidget : SBGUIElement
{
	private SBGUIAtlasImage m_pCurrencyImage;

	private SBGUIAtlasImage m_pLockedImage;

	private SBGUIAtlasImage m_pLockedBackingImage;

	private SBGUIAtlasImage m_pLockedLevelImage;

	private SBGUIAtlasImage m_pLockedLevelImageSmall;

	private SBGUILabel m_pTaskNameLabel;

	private SBGUILabel m_pTaskDurationLabel;

	private SBGUILabel m_pTaskXPRewardLabel;

	private SBGUILabel m_pTaskSoftRewardLabel;

	private SBGUILabel m_pLockedLevelLabel;

	private SBGUILabel m_pLockedLevelLabelSmall;

	private SBGUIButton m_pDoTaskButton;

	private Vector2 m_pLockedIconSize;

	private GameObject m_pLockedParent;

	private GameObject m_pUnlockedParent;

	private SBGUIAtlasImage m_pTaskBonusRewardIcon;

	private int nTaskDID;

	public void SetData(Session pSession, Action pDoTaskAction, TaskData pTaskData, int nCostumeDID, int costumeCount, Simulated pCostumeSimulated)
	{
	}

	protected override void Awake()
	{
	}
}
