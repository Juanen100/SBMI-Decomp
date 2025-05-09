using System;
using System.Collections.Generic;
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

	public void SetData(Session pSession, Action pDoTaskAction, TaskData pTaskData, int nCostumeDID, int costumeCount)
	{
		m_pDoTaskButton.ClearClickEvents();
		AttachActionToButton(m_pDoTaskButton, pDoTaskAction);
		m_pDoTaskButton.UpdateCollider();
		m_pDoTaskButton.SessionActionId = "task_do_button_" + pTaskData.m_sName;
		m_pTaskNameLabel.SetText(Language.Get(pTaskData.m_sName));
		m_pTaskDurationLabel.SetText(TFUtils.DurationToString((ulong)pTaskData.m_nDuration));
		m_pTaskXPRewardLabel.SetText(pTaskData.m_pReward.ResourceAmounts[ResourceManager.XP].ToString());
		foreach (KeyValuePair<int, int> resourceAmount in pTaskData.m_pReward.ResourceAmounts)
		{
			if (resourceAmount.Key != ResourceManager.XP)
			{
				m_pTaskSoftRewardLabel.SetText(pTaskData.m_pReward.ResourceAmounts[resourceAmount.Key].ToString());
				m_pCurrencyImage.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[resourceAmount.Key].GetResourceTexture());
				break;
			}
		}
		if (pTaskData.tasksHasBonus.Contains(pTaskData.m_nDID))
		{
			m_pTaskBonusRewardIcon.SetTextureFromAtlas(pTaskData.m_sPaytableRewardIcon);
			m_pTaskBonusRewardIcon.SetActive(true);
		}
		else
		{
			m_pTaskBonusRewardIcon.gameObject.SetActive(false);
		}
		TaskManager.TaskBlockedStatus taskBlockedStatus = pSession.TheGame.taskManager.GetTaskBlockedStatus(pSession.TheGame, pTaskData, nCostumeDID);
		bool flag = taskBlockedStatus.m_eTaskBlockedType == TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone;
		if (costumeCount > 0 && pSession.TheGame.costumeManager.IsCostumeUnlocked(nCostumeDID) && flag)
		{
			m_pLockedParent.SetActive(false);
			m_pUnlockedParent.SetActive(true);
		}
		else if (costumeCount == 0 && flag)
		{
			m_pLockedParent.SetActive(false);
			m_pUnlockedParent.SetActive(true);
		}
		else
		{
			m_pLockedParent.SetActive(true);
			m_pUnlockedParent.SetActive(false);
			m_pLockedLevelLabel.SetActive(false);
			m_pLockedLevelLabelSmall.SetActive(false);
			m_pLockedLevelImage.SetActive(false);
			m_pLockedLevelImageSmall.SetActive(false);
			bool flag2 = (taskBlockedStatus.m_eTaskBlockedType & TaskManager.TaskBlockedStatus._eTaskBlockedType.eLevel) != 0;
			if ((taskBlockedStatus.m_eTaskBlockedType & TaskManager.TaskBlockedStatus._eTaskBlockedType.eTarget) != TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone)
			{
				m_pLockedImage.SetActive(true);
				m_pLockedBackingImage.SetActive(true);
				if (flag2)
				{
					m_pLockedLevelLabelSmall.SetActive(true);
					m_pLockedLevelImageSmall.SetActive(true);
					m_pLockedLevelLabelSmall.SetText(pTaskData.m_nMinLevel.ToString());
				}
				Blueprint blueprint = EntityManager.GetBlueprint(EntityType.BUILDING, pTaskData.m_nTargetDID);
				if (blueprint.Invariable.ContainsKey("portrait"))
				{
					m_pLockedImage.SetSizeNoRebuild(m_pLockedIconSize);
					m_pLockedImage.SetTextureFromAtlas((string)blueprint.Invariable["portrait"], true, false, true);
				}
				else
				{
					m_pLockedImage.SetActive(false);
				}
			}
			else if (flag2)
			{
				m_pLockedImage.SetActive(false);
				m_pLockedBackingImage.SetActive(false);
				m_pLockedLevelLabel.SetActive(true);
				m_pLockedLevelImage.SetActive(true);
				m_pLockedLevelLabel.SetText(pTaskData.m_nMinLevel.ToString());
			}
		}
		nTaskDID = pTaskData.m_nDID;
	}

	protected override void Awake()
	{
		m_pCurrencyImage = (SBGUIAtlasImage)FindChild("soft_currency_icon");
		m_pLockedImage = (SBGUIAtlasImage)FindChild("locked_icon");
		m_pLockedBackingImage = (SBGUIAtlasImage)FindChild("locked_icon_backing");
		m_pLockedLevelImage = (SBGUIAtlasImage)FindChild("locked_level_icon");
		m_pLockedLevelImageSmall = (SBGUIAtlasImage)FindChild("locked_level_icon_small");
		m_pTaskNameLabel = (SBGUILabel)FindChild("task_name_label");
		m_pTaskDurationLabel = (SBGUILabel)FindChild("task_duration_label");
		m_pTaskXPRewardLabel = (SBGUILabel)FindChild("task_xp_reward_label");
		m_pTaskSoftRewardLabel = (SBGUILabel)FindChild("task_soft_reward_label");
		m_pLockedLevelLabel = (SBGUILabel)FindChild("locked_level_label");
		m_pLockedLevelLabelSmall = (SBGUILabel)FindChild("locked_level_label_small");
		m_pDoTaskButton = (SBGUIButton)FindChild("do_task_button");
		m_pTaskBonusRewardIcon = (SBGUIAtlasImage)FindChild("task_bonus_icon");
		m_pLockedIconSize = m_pLockedImage.Size;
		m_pLockedParent = m_pLockedImage.transform.parent.gameObject;
		m_pUnlockedParent = m_pDoTaskButton.transform.parent.gameObject;
		SBGUILabel sBGUILabel = (SBGUILabel)FindChild("reward_title_label");
		sBGUILabel.SetText(Language.Get("!!PREFAB_REWARD") + ":");
		base.Awake();
	}
}
