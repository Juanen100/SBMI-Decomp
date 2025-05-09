using System;
using System.Collections.Generic;
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

	private GameObject m_pTaskInProgressParent;

	private GameObject m_pTaskDoneParent;

	private Vector3 m_pCharacterPortraitSize;

	private Simulated m_pSimulated;

	private ResidentEntity m_pEntity;

	private Task m_pTask;

	private Action m_pRushTaskAction;

	private ulong m_ulTaskTimeLeft;

	public int taskRushCost;

	public void SetupDialogInfo(Simulated pSimulated, Task pTask, Action pFeedWishAction, Action pRushWishAction, Action pRushTaskAction)
	{
		m_pSimulated = pSimulated;
		m_pTask = pTask;
		m_pRushTaskAction = pRushTaskAction;
		m_pEntity = m_pSimulated.GetEntity<ResidentEntity>();
		m_pWishWidget.SetData(session, pSimulated, pFeedWishAction, pRushWishAction);
		TaskData pTaskData = pTask.m_pTaskData;
		m_pTaskNameLabel.SetText(Language.Get(pTaskData.m_sName));
		foreach (KeyValuePair<int, int> resourceAmount in pTaskData.m_pReward.ResourceAmounts)
		{
			if (resourceAmount.Key != ResourceManager.XP)
			{
				m_pTaskSoftRewardLabel.SetText(pTaskData.m_pReward.ResourceAmounts[resourceAmount.Key].ToString());
				m_pCurrencyImage.SetTextureFromAtlas(session.TheGame.resourceManager.Resources[resourceAmount.Key].GetResourceTexture());
				break;
			}
		}
		m_pTaskXPRewardLabel.SetText(pTaskData.m_pReward.ResourceAmounts[ResourceManager.XP].ToString());
		m_ulTaskTimeLeft = m_pTask.GetTimeLeft();
		m_pTaskInProgressParent.SetActive(true);
		if (m_pTask.m_bMovingToTarget)
		{
			m_pTaskProgressLabel.SetText("Waiting...");
			m_pTaskProgressMeter.Progress = 0f;
		}
		else
		{
			m_pTaskProgressLabel.SetText(TFUtils.DurationToString(m_ulTaskTimeLeft));
			m_pTaskProgressMeter.Progress = m_pTask.GetTimeLeftPercentage();
		}
		Cost cost = m_pTask.RushCostNow();
		m_pTaskRushCostLabel.SetText(cost.ResourceAmounts[cost.GetOnlyCostKey()].ToString());
		m_pRushTaskButton.ClearClickEvents();
		AttachActionToButton(m_pRushTaskButton, pRushTaskAction);
		ResidentEntity entity = pSimulated.GetEntity<ResidentEntity>();
		int num = ((!entity.CostumeDID.HasValue) ? (-1) : entity.CostumeDID.Value);
		if (num < 0)
		{
			num = ((!entity.DefaultCostumeDID.HasValue) ? (-1) : entity.DefaultCostumeDID.Value);
		}
		if (num >= 0)
		{
			CostumeManager.Costume costume = session.TheGame.costumeManager.GetCostume(num);
			m_pCharacterPortrait.SetSizeNoRebuild(m_pCharacterPortraitSize);
			m_pCharacterPortrait.SetTextureFromAtlas(costume.m_sPortrait, true, false, true);
		}
		else if (!string.IsNullOrEmpty(entity.DialogPortrait))
		{
			m_pCharacterPortrait.SetSizeNoRebuild(m_pCharacterPortraitSize);
			m_pCharacterPortrait.SetTextureFromAtlas(entity.DialogPortrait, true, false, true);
		}
		else
		{
			m_pCharacterPortrait.SetTextureFromAtlas("_blank_.png");
		}
		m_pCharacterNameLabel.SetText(Language.Get(entity.Name));
	}

	public Vector2 GetWishWidgetRushButtonPosition()
	{
		return m_pWishWidget.GetRushWishButtonPosition();
	}

	public Vector2 GetTaskRushButtonPosition()
	{
		return m_pRushTaskButton.GetScreenPosition();
	}

	protected override void Awake()
	{
		m_pWishWidget = (SBGUICharacterWishWidget)FindChild("wish_widget");
		m_pCharacterPortrait = (SBGUIAtlasImage)FindChild("character_portrait");
		m_pCurrencyImage = (SBGUIAtlasImage)FindChild("soft_currency_icon");
		m_pTaskNameLabel = (SBGUILabel)FindChild("task_name_label");
		m_pTaskSoftRewardLabel = (SBGUILabel)FindChild("task_soft_reward_label");
		m_pTaskXPRewardLabel = (SBGUILabel)FindChild("task_xp_reward_label");
		m_pTaskProgressLabel = (SBGUILabel)FindChild("task_progress_label");
		m_pTaskRushCostLabel = (SBGUILabel)FindChild("task_rush_cost_label");
		m_pCharacterNameLabel = (SBGUILabel)FindChild("character_name_label");
		m_pTaskProgressMeter = (SBGUIProgressMeter)FindChild("task_progress_meter");
		m_pRushTaskButton = (SBGUIButton)FindChild("rush_task_button");
		m_pTaskInProgressParent = m_pTaskProgressMeter.transform.parent.parent.gameObject;
		m_pCharacterPortraitSize = m_pCharacterPortrait.Size;
		SBGUILabel sBGUILabel = (SBGUILabel)FindChild("reward_title_label");
		sBGUILabel.SetText(Language.Get("!!PREFAB_REWARD") + ":");
		base.Awake();
	}

	private new void Update()
	{
		if (m_ulTaskTimeLeft == 0)
		{
			return;
		}
		m_ulTaskTimeLeft = m_pTask.GetTimeLeft();
		if (m_ulTaskTimeLeft == 0L)
		{
			m_pTaskInProgressParent.SetActive(false);
			return;
		}
		if (m_pTask.m_bMovingToTarget)
		{
			m_pTaskProgressLabel.SetText("Waiting...");
			m_pTaskProgressMeter.Progress = 0f;
		}
		else
		{
			m_pTaskProgressLabel.SetText(TFUtils.DurationToString(m_ulTaskTimeLeft));
			m_pTaskProgressMeter.Progress = m_pTask.GetTimeLeftPercentage();
		}
		Cost cost = m_pTask.RushCostNow();
		m_pTaskRushCostLabel.SetText(cost.ResourceAmounts[cost.GetOnlyCostKey()].ToString());
		int.TryParse(cost.ResourceAmounts[cost.GetOnlyCostKey()].ToString(), out taskRushCost);
	}
}
