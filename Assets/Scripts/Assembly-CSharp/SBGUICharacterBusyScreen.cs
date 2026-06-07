using System;
using UnityEngine;

public class SBGUICharacterBusyScreen : SBGUIScreen
{
	private SBGUICharacterWishWidget m_pWishWidget;

	private SBGUIAtlasImage m_pCharacterPortrait;

	private SBGUIAtlasImage m_pCurrencyImage;

	private SBGUILabel m_pTaskNameLabel;

	private SBGUILabel m_pTaskSoftRewardLabel;

	private SBGUILabel m_pTaskXPRewardLabel;

	private SBGUILabel m_pTaskProgressLabel;

	private SBGUILabel m_pTaskRushCostLabel;

	private SBGUILabel m_pCharacterNameLabel;

	private SBGUIProgressMeter m_pTaskProgressMeter;

	private SBGUIButton m_pRushTaskButton;

	private SBGUIButton m_pWatchTaskAdButton;

	private GameObject m_pTaskInProgressParent;

	private GameObject m_pTaskDoneParent;

	private Vector3 m_pCharacterPortraitSize;

	private Simulated m_pSimulated;

	private ResidentEntity m_pEntity;

	private Task m_pTask;

	private Action m_pRushTaskAction;

	private Action m_pWatchTaskAdAction;

	private ulong m_ulTaskTimeLeft;

	public int taskRushCost;

	public void SetupDialogInfo(Simulated pSimulated, Task pTask, Action pFeedWishAction, Action pRushWishAction, Action pRushTaskAction, Action onWatchWishAd, Action onWatchTaskAd)
	{
	}

	public Vector2 GetWishWidgetRushButtonPosition()
	{
		return default(Vector2);
	}

	public Vector2 GetTaskRushButtonPosition()
	{
		return default(Vector2);
	}

	protected override void Awake()
	{
	}

	private new void Update()
	{
	}
}
