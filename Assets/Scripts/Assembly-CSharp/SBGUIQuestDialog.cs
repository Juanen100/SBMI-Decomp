using System;
using System.Collections.Generic;
using UnityEngine;

public class SBGUIQuestDialog : SBGUIModalDialog
{
	public const int STEP_GAP = 4;

	private double residentPosX;

	private double residentPosY;

	private int? prefabIconSize;

	protected override void Awake()
	{
		rewardMarker = FindChild("reward_marker");
		base.Awake();
	}

	public override void SetParent(SBGUIElement element)
	{
		SetTransformParent(element);
	}

	public void SetupQuestDialogInfo(string name, string icon, List<ConditionDescription> steps, bool hasCount, List<QuestBookendInfo.ChunkConditions> chunks, Action findButtonHandler)
	{
		SBGUILabel sBGUILabel = (SBGUILabel)FindChild("description_label");
		SBGUIAtlasImage sBGUIAtlasImage = (SBGUIAtlasImage)FindChild("icon");
		SBGUILabel sBGUILabel2 = (SBGUILabel)FindChild("count");
		SBGUIButton sBGUIButton = (SBGUIButton)FindChild("find_button");
		SBGUIButton sBGUIButton2 = (SBGUIButton)FindChild("okay");
		int? num = prefabIconSize;
		if (!num.HasValue)
		{
			prefabIconSize = (int)sBGUIAtlasImage.Size.x;
		}
		sBGUILabel.SetText(name);
		sBGUIButton.gameObject.SetActive(false);
		sBGUIButton2.gameObject.SetActive(false);
		if (steps != null && hasCount)
		{
			if (sBGUILabel2 != null)
			{
				sBGUILabel2.SetActive(true);
				sBGUILabel2.SetText(steps[0].OccuranceCount + "/" + steps[0].OccurancesRequired);
			}
		}
		else if (sBGUILabel2 != null)
		{
			sBGUILabel2.SetActive(false);
		}
		if (sBGUIAtlasImage != null)
		{
			int? num2 = prefabIconSize;
			sBGUIAtlasImage.SetTextureFromAtlas(icon, true, false, true, false, false, num2.Value);
		}
		if (chunks.Count <= 0)
		{
			return;
		}
		Dictionary<string, object> dictionary = chunks[0].Condition.ToDict();
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		if (dictionary.ContainsKey("task_id"))
		{
			num3 = TFUtils.LoadInt(dictionary, "task_id");
		}
		if (dictionary.ContainsKey("costume_id"))
		{
			num4 = TFUtils.LoadInt(dictionary, "costume_id");
		}
		if (num3 != 0 || num4 != 0)
		{
			sBGUIButton.SetActive(true);
			sBGUIButton2.SetActive(false);
		}
		else
		{
			sBGUIButton.SetActive(false);
			sBGUIButton2.SetActive(true);
		}
		if (num3 == 0 && num4 == 0)
		{
			return;
		}
		TaskData taskData = null;
		Simulated costumeSourceDID = null;
		if (num3 != 0)
		{
			taskData = session.TheGame.taskManager.GetTaskData(num3);
		}
		if (num4 != 0)
		{
			costumeSourceDID = session.TheGame.simulation.FindSimulated(session.TheGame.costumeManager.GetCostume(num4).m_nUnitDID);
		}
		if (taskData != null)
		{
			TFUtils.ErrorLog("Task Data != null - line 130 SBGUIQuestDialog.cs");
			num5 = taskData.m_nSourceDID;
			Simulated pSimulated = session.TheGame.simulation.FindSimulated(num5);
			bool flag = true;
			if (pSimulated != null && pSimulated.HasEntity<ResidentEntity>())
			{
				TFUtils.ErrorLog("pSimulated != null - line 143 SBGUIQuestDialog.cs");
				ResidentEntity entity = pSimulated.GetEntity<ResidentEntity>();
				if (entity.m_pTask != null && (entity.m_pTask.m_bMovingToTarget || entity.m_pTask.m_bAtTarget))
				{
					residentPosX = entity.m_pTaskTargetPosition.x;
					residentPosY = entity.m_pTaskTargetPosition.y;
					TFUtils.ErrorLog("pResidentEntity.m_pTask != null - line 158 SBGUIQuestDialog.cs");
				}
				else
				{
					TFUtils.ErrorLog("pResidentEntity.m_pTask == null - line 162 SBGUIQuestDialog.cs");
					residentPosX = -1.0;
				}
			}
			if (!flag)
			{
				return;
			}
			Action action = delegate
			{
				session.TheSoundEffectManager.PlaySound("Accept");
				session.TheGame.selected = pSimulated;
				ResidentEntity entity3 = pSimulated.GetEntity<ResidentEntity>();
				if (residentPosX > 1000010.0 || residentPosX == 0.0 || pSimulated.PositionCenter.x > 1000010f || pSimulated.PositionCenter.x == 0f)
				{
					session.TheCamera.AutoPanToPosition(new Vector2(1000.9f, 667.5f), 0.75f);
					if (entity3.m_pTask != null && (entity3.m_pTask.m_bMovingToTarget || entity3.m_pTask.m_bAtTarget))
					{
						session.ChangeState("UnitBusy");
						TFUtils.ErrorLog("ChangeState BUSY - task != null, moving to or at target 224");
						pSimulated.InteractionState.SelectedTransition = new Session.UnitBusyTransition(pSimulated);
					}
					else
					{
						session.ChangeState("UnitIdle");
						TFUtils.ErrorLog("ChangeState IDLE - line 229 SBGUIQuestDialog.cs");
						pSimulated.InteractionState.SelectedTransition = new Session.UnitIdleTransition(pSimulated);
					}
				}
				else if (residentPosX > 0.0 && residentPosX < 1000010.0)
				{
					session.TheCamera.AutoPanToPosition(new Vector2((float)residentPosX, (float)residentPosY), 0.75f);
					if (entity3.m_pTask != null && (entity3.m_pTask.m_bMovingToTarget || entity3.m_pTask.m_bAtTarget))
					{
						session.ChangeState("UnitBusy");
						pSimulated.InteractionState.SelectedTransition = new Session.UnitBusyTransition(pSimulated);
					}
					else
					{
						session.ChangeState("UnitIdle");
						pSimulated.InteractionState.SelectedTransition = new Session.UnitIdleTransition(pSimulated);
					}
				}
				else if (residentPosX == -1.0)
				{
					session.TheCamera.AutoPanToPosition(pSimulated.PositionCenter, 0.75f);
					if (entity3.m_pTask != null && (entity3.m_pTask.m_bMovingToTarget || entity3.m_pTask.m_bAtTarget))
					{
						session.ChangeState("UnitBusy");
						TFUtils.ErrorLog("ChangeState BUSY - Task != null, mvoing to or at target");
					}
					else
					{
						session.ChangeState("UnitIdle");
						TFUtils.ErrorLog("ChangeState IDLE - resident position = -1 ");
					}
				}
			};
			AttachActionToButton(sBGUIButton, findButtonHandler);
			AttachActionToButton(sBGUIButton, action);
		}
		else
		{
			if (costumeSourceDID == null)
			{
				return;
			}
			TFUtils.ErrorLog("costumeSourceDID != null ");
			bool flag2 = true;
			if (costumeSourceDID != null && costumeSourceDID.HasEntity<ResidentEntity>())
			{
				ResidentEntity entity2 = costumeSourceDID.GetEntity<ResidentEntity>();
				if (entity2.CostumeDID.HasValue)
				{
					residentPosX = costumeSourceDID.Position.x;
					residentPosY = costumeSourceDID.Position.y;
				}
				else
				{
					residentPosX = -1.0;
				}
			}
			if (!flag2)
			{
				return;
			}
			Action action2 = delegate
			{
				session.TheSoundEffectManager.PlaySound("Accept");
				session.TheGame.selected = costumeSourceDID;
				ResidentEntity entity3 = costumeSourceDID.GetEntity<ResidentEntity>();
				if (residentPosX > 1000010.0 || residentPosX == 0.0 || costumeSourceDID.PositionCenter.x > 1000010f || costumeSourceDID.PositionCenter.x == 0f)
				{
					session.TheCamera.AutoPanToPosition(new Vector2(1000.9f, 667.5f), 0.75f);
					if (entity3.m_pTask != null && (entity3.m_pTask.m_bMovingToTarget || entity3.m_pTask.m_bAtTarget))
					{
						session.ChangeState("UnitBusy");
					}
					else
					{
						session.ChangeState("UnitIdle");
					}
				}
				else if (residentPosX > 0.0 && residentPosX < 1000010.0)
				{
					session.TheCamera.AutoPanToPosition(new Vector2((float)residentPosX, (float)residentPosY), 0.75f);
					if (entity3.m_pTask != null && (entity3.m_pTask.m_bMovingToTarget || entity3.m_pTask.m_bAtTarget))
					{
						session.ChangeState("UnitBusy");
					}
					else
					{
						session.ChangeState("UnitIdle");
					}
				}
				else if (residentPosX == -1.0)
				{
					session.TheCamera.AutoPanToPosition(costumeSourceDID.PositionCenter, 0.75f);
					if (entity3.m_pTask != null && (entity3.m_pTask.m_bMovingToTarget || entity3.m_pTask.m_bAtTarget))
					{
						session.ChangeState("UnitBusy");
					}
					else
					{
						session.ChangeState("UnitIdle");
					}
				}
			};
			AttachActionToButton(sBGUIButton, findButtonHandler);
			AttachActionToButton(sBGUIButton, action2);
		}
	}

	public void SetupQuestDialogInfo(string name, string icon, List<ConditionDescription> steps, bool hasCount)
	{
		SBGUILabel sBGUILabel = (SBGUILabel)FindChild("description_label");
		SBGUIAtlasImage sBGUIAtlasImage = (SBGUIAtlasImage)FindChild("icon");
		SBGUILabel sBGUILabel2 = (SBGUILabel)FindChild("count");
		SBGUIButton sBGUIButton = (SBGUIButton)FindChild("find_button");
		sBGUIButton.gameObject.SetActive(false);
		int? num = prefabIconSize;
		if (!num.HasValue)
		{
			prefabIconSize = (int)sBGUIAtlasImage.Size.x;
		}
		sBGUILabel.SetText(name);
		if (steps != null && hasCount)
		{
			if (sBGUILabel2 != null)
			{
				sBGUILabel2.SetActive(true);
				sBGUILabel2.SetText(steps[0].OccuranceCount + "/" + steps[0].OccurancesRequired);
			}
		}
		else if (sBGUILabel2 != null)
		{
			sBGUILabel2.SetActive(false);
		}
		if (sBGUIAtlasImage != null)
		{
			int? num2 = prefabIconSize;
			sBGUIAtlasImage.SetTextureFromAtlas(icon, true, false, true, false, false, num2.Value);
		}
	}

	public void SetupQuestDialogInfo(string name, string icon)
	{
		SetupQuestDialogInfo(name, icon, null, false);
	}
}
