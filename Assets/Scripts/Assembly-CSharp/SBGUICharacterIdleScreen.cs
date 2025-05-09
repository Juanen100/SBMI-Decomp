using System;
using System.Collections.Generic;
using UnityEngine;

public class SBGUICharacterIdleScreen : SBGUIScrollableDialog
{
	public const int STEP_GAP = 20;

	private const float CHECKBOX_GAP = 0.7f;

	private const float TEXT_GAP = 40f;

	private Vector3 checkBoxLocPos = new Vector3(-0.05f, 0.35f, 0f);

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
		SetTransformParent(element);
	}

	public void CreateScrollRegionUI(List<TaskData> pTaskDatas)
	{
		scrollSize = Vector2.zero;
		if (region.Marker.transform.GetChildCount() > 0)
		{
			SBGUIElement[] componentsInChildren = region.Marker.GetComponentsInChildren<SBGUIElement>();
			SBGUIElement[] array = componentsInChildren;
			foreach (SBGUIElement sBGUIElement in array)
			{
				if (!(sBGUIElement == region.Marker))
				{
					UnityEngine.Object.Destroy(sBGUIElement.gameObject);
				}
			}
		}
		int num = ((!m_nCostumeDID.HasValue) ? (-1) : m_nCostumeDID.Value);
		if ((num != -1 && session.TheGame.costumeManager.IsCostumeUnlocked(num)) || costumeCount == 0)
		{
			m_pDialogueBubble.SetActive(false);
			m_pCostumeUnlockLabel.SetActive(false);
			m_pTasksTitleLabel.SetText(Language.Get("!!TASK_UI_STUFF_DO"));
		}
		m_pTaskDatas = pTaskDatas;
		object obj = session.CheckAsyncRequest("purchasedCostume");
		if (obj != null)
		{
			SetArrowList((int)obj);
			return;
		}
		List<int> tasksCompleting = session.TheGame.questManager.GetTasksCompleting();
		List<TaskData> list = new List<TaskData>();
		List<TaskData> list2 = new List<TaskData>();
		List<TaskData> list3 = new List<TaskData>();
		int count = pTaskDatas.Count;
		for (int j = 0; j < count; j++)
		{
			TaskData taskData = pTaskDatas[j];
			TaskManager.TaskBlockedStatus taskBlockedStatus = session.TheGame.taskManager.GetTaskBlockedStatus(session.TheGame, taskData, num);
			if (taskBlockedStatus.m_eTaskBlockedType != TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone)
			{
				if (!taskData.m_bHiddenUntilUnlocked && (taskBlockedStatus.m_eTaskBlockedType & TaskManager.TaskBlockedStatus._eTaskBlockedType.eSourceCostume) == TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone && (taskBlockedStatus.m_eTaskBlockedType & TaskManager.TaskBlockedStatus._eTaskBlockedType.eMicroEvent) == TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone && (taskBlockedStatus.m_eTaskBlockedType & TaskManager.TaskBlockedStatus._eTaskBlockedType.eRepeatable) == TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone && (taskBlockedStatus.m_eTaskBlockedType & TaskManager.TaskBlockedStatus._eTaskBlockedType.eQuestUnlock) == TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone && (taskBlockedStatus.m_eTaskBlockedType & TaskManager.TaskBlockedStatus._eTaskBlockedType.eActiveQuest) == TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone)
				{
					list3.Add(taskData);
				}
			}
			else if (tasksCompleting.Contains(taskData.m_nDID))
			{
				list.Add(taskData);
			}
			else
			{
				list2.Add(taskData);
			}
		}
		list.Sort();
		list2.Sort();
		list3.Sort();
		count = list.Count;
		for (int num2 = count - 1; num2 >= 0; num2--)
		{
			list2.Insert(0, list[num2]);
		}
		int num3 = 0;
		int count2 = list2.Count;
		int count3 = list3.Count;
		count = count2 + count3;
		for (int k = 0; k < count; k++)
		{
			TaskData pTaskData;
			if (k < count2)
			{
				pTaskData = list2[k];
			}
			else
			{
				pTaskData = list3[k - count2];
			}
			string prefabName = "Prefabs/GUI/Widgets/TaskWidget";
			SBGUITaskWidget sBGUITaskWidget = (SBGUITaskWidget)SBGUI.InstantiatePrefab(prefabName);
			sBGUITaskWidget.SessionActionId = "task_widget_" + pTaskData.m_sName;
			SBGUIImage sBGUIImage = (SBGUIImage)sBGUITaskWidget.FindChild("window");
			sBGUITaskWidget.SetParent(stepsMarker);
			sBGUITaskWidget.tform.localPosition = Vector3.zero;
			sBGUITaskWidget.tform.localPosition = new Vector3(0f, (0f - (sBGUIImage.Size.y + 20f) * 0.01f) * (float)num3, 0f);
			scrollSize += (sBGUIImage.Size + new Vector2(20f, 20f)) * 0.01f;
			float y = sBGUITaskWidget.transform.localPosition.y;
			sBGUITaskWidget.SetParent(region.Marker);
			Vector3 localPosition = new Vector3(0f, y, 0f);
			sBGUITaskWidget.transform.localPosition = localPosition;
			Action pDoTaskAction = delegate
			{
				m_pDoTaskAction(pTaskData.m_nDID);
			};
			sBGUITaskWidget.SetData(session, pDoTaskAction, pTaskData, num, costumeCount);
			num3++;
		}
		Rect rect = new Rect(0f, 0f, scrollSize.x, scrollSize.y);
		region.ResetScroll(rect);
		region.ResetToMinScroll();
		if (region.scrollBar.IsActive())
		{
			region.scrollBar.Reset();
		}
	}

	public void SetupDialogInfo(Simulated pSimulated, Action pFeedWishAction, Action pRushWishAction, Action<int> pDoTaskAction)
	{
		stepsMarker = FindChild("steps_marker");
		window = (SBGUIAtlasImage)FindChild("window");
		m_pCharacterWishWidget = (SBGUICharacterWishWidget)FindChild("wish_widget");
		m_pCharacterWishWidget.SetData(session, pSimulated, pFeedWishAction, pRushWishAction);
		m_pDoTaskAction = pDoTaskAction;
		m_pArrowList = (SBGUIArrowList)FindChild("character_portrait_parent");
		m_pCharacterNameLabel = (SBGUILabel)FindChild("character_name_label");
		m_pCostumeUnlockLabel = (SBGUILabel)FindChild("costume_unlock_label");
		m_pTasksTitleLabel = (SBGUILabel)FindChild("tasks_title_label");
		m_pDialogueBubble = (SBGUIAtlasImage)FindChild("Dialogue_bubble");
		ResidentEntity entity = pSimulated.GetEntity<ResidentEntity>();
		List<CostumeManager.Costume> costumesForUnit = session.TheGame.costumeManager.GetCostumesForUnit(pSimulated.entity.DefinitionId, true, false);
		int nNumCostumes = costumesForUnit.Count;
		costumeCount = nNumCostumes;
		checkBoxes = new List<SBGUIAtlasImage>();
		ticks = new List<SBGUIAtlasImage>();
		popupTexts = new List<SBGUILabel>();
		Action<int> pSelectedItemChanged = delegate(int nCostumeDID)
		{
			ClearList();
			if (nNumCostumes > 0 && !session.TheGame.costumeManager.IsCostumeUnlocked(nCostumeDID))
			{
				SetupDialogBubble(nCostumeDID);
			}
			m_nCostumeDID = nCostumeDID;
			CreateScrollRegionUI(m_pTaskDatas);
		};
		Action<int> pItemClick = delegate(int nCostumeDID)
		{
			m_nCostumeDID = nCostumeDID;
			m_pArrowList.SetSelectedID(nCostumeDID);
		};
		if (nNumCostumes <= 0)
		{
			m_pArrowList.SetData(session, new List<SBGUIArrowList.ListItemData>
			{
				new SBGUIArrowList.ListItemData(0, entity.DialogPortrait)
			}, 0, null, pSelectedItemChanged, pItemClick);
		}
		else
		{
			List<SBGUIArrowList.ListItemData> list = new List<SBGUIArrowList.ListItemData>(nNumCostumes);
			for (int num = 0; num < nNumCostumes; num++)
			{
				list.Add(new SBGUIArrowList.ListItemData(costumesForUnit[num].m_nDID, costumesForUnit[num].m_sPortrait, !session.TheGame.costumeManager.IsCostumeUnlocked(costumesForUnit[num].m_nDID)));
			}
			m_pArrowList.SetData(session, list, (!entity.CostumeDID.HasValue) ? entity.DefaultCostumeDID.Value : entity.CostumeDID.Value, null, pSelectedItemChanged, pItemClick);
		}
		m_nCostumeDID = entity.CostumeDID;
		m_pCharacterNameLabel.SetText(Language.Get(entity.Name));
	}

	private void SetupDialogBubble(int costumeDID)
	{
		CostumeManager.Costume costume = session.TheGame.costumeManager.GetCostume(costumeDID);
		m_pDialogueBubble.SetActive(!session.TheGame.costumeManager.IsCostumeUnlocked(costumeDID));
		m_pCostumeUnlockLabel.SetActive(true);
		m_pTasksTitleLabel.SetText(Language.Get("!!PREFAB_LOCKED"));
		CreateCheckBox(costume.m_nCriteriaCount);
		float num = GenerateTextandTick(costume);
		m_pDialogueBubble.Size = new Vector2(m_pDialogueBubble.Size.x, num + 80f);
	}

	private void PositionObject(SBGUIElement obj, Vector3 loc)
	{
		obj.SetParent(m_pCostumeUnlockLabel);
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localPosition = loc;
	}

	public void ClearList()
	{
		for (int i = 0; i < popupTexts.Count; i++)
		{
			UnityEngine.Object.Destroy(popupTexts[i].gameObject);
		}
		popupTexts.Clear();
		for (int j = 0; j < ticks.Count; j++)
		{
			UnityEngine.Object.Destroy(ticks[j].gameObject);
		}
		ticks.Clear();
		for (int k = 0; k < checkBoxes.Count; k++)
		{
			UnityEngine.Object.Destroy(checkBoxes[k].gameObject);
		}
		checkBoxes.Clear();
	}

	private float GenerateTextandTick(CostumeManager.Costume costume)
	{
		string prefabName = "Prefabs/GUI/Screens/PopUpText";
		string prefabName2 = "Prefabs/GUI/Screens/CheckBoxTick";
		int num = 0;
		float num2 = 0f;
		if (costume.m_nUnlockLevel > 0)
		{
			SBGUILabel sBGUILabel = (SBGUILabel)SBGUI.InstantiatePrefab(prefabName);
			sBGUILabel.SetText("Reach Level " + costume.m_nUnlockLevel);
			PositionObject(sBGUILabel, new Vector3(0.2f, checkBoxLocPos.y - (float)num * 0.7f, 0f));
			popupTexts.Add(sBGUILabel);
			num2 += (float)sBGUILabel.Height + 40f;
			if (session.TheGame.resourceManager.PlayerLevelAmount >= costume.m_nUnlockLevel)
			{
				SBGUIAtlasImage sBGUIAtlasImage = (SBGUIAtlasImage)SBGUI.InstantiatePrefab(prefabName2);
				PositionObject(sBGUIAtlasImage, new Vector3(checkBoxLocPos.x, checkBoxLocPos.y - (float)num * 0.7f, -0.01f));
				ticks.Add(sBGUIAtlasImage);
			}
			num++;
		}
		if (costume.m_nUnlockAssetDid > 0)
		{
			SBGUILabel sBGUILabel2 = (SBGUILabel)SBGUI.InstantiatePrefab(prefabName);
			Blueprint blueprint = EntityManager.GetBlueprint(EntityType.BUILDING, costume.m_nUnlockAssetDid, true);
			sBGUILabel2.SetText("Buy and Place " + Language.Get((string)blueprint.Invariable["name"]));
			PositionObject(sBGUILabel2, new Vector3(0.2f, checkBoxLocPos.y - (float)num * 0.7f, 0f));
			popupTexts.Add(sBGUILabel2);
			num2 += (float)sBGUILabel2.Height + 40f;
			if (session.TheGame.inventory.HasItem(costume.m_nUnlockAssetDid) || session.TheGame.simulation.FindSimulated(costume.m_nUnlockAssetDid) != null)
			{
				SBGUIAtlasImage sBGUIAtlasImage2 = (SBGUIAtlasImage)SBGUI.InstantiatePrefab(prefabName2);
				PositionObject(sBGUIAtlasImage2, new Vector3(checkBoxLocPos.x, checkBoxLocPos.y - (float)num * 0.7f, -0.01f));
				ticks.Add(sBGUIAtlasImage2);
			}
			num++;
		}
		if (costume.m_nUnlockQuest1 > 0)
		{
			SBGUILabel sBGUILabel3 = (SBGUILabel)SBGUI.InstantiatePrefab(prefabName);
			sBGUILabel3.SetText(Language.Get(costume.m_sUnlockQuest1Descript));
			PositionObject(sBGUILabel3, new Vector3(0.2f, checkBoxLocPos.y - (float)num * 0.7f, 0f));
			popupTexts.Add(sBGUILabel3);
			num2 += (float)sBGUILabel3.Height + 40f;
			if (session.TheGame.questManager.IsQuestCompleted((uint)costume.m_nUnlockQuest1))
			{
				SBGUIAtlasImage sBGUIAtlasImage3 = (SBGUIAtlasImage)SBGUI.InstantiatePrefab(prefabName2);
				PositionObject(sBGUIAtlasImage3, new Vector3(checkBoxLocPos.x, checkBoxLocPos.y - (float)num * 0.7f, -0.01f));
				ticks.Add(sBGUIAtlasImage3);
			}
			num++;
		}
		if (costume.m_nUnlockQuest2 > 0)
		{
			SBGUILabel sBGUILabel4 = (SBGUILabel)SBGUI.InstantiatePrefab(prefabName);
			sBGUILabel4.SetText(Language.Get(costume.m_sUnlockQuest2Descript));
			PositionObject(sBGUILabel4, new Vector3(0.2f, checkBoxLocPos.y - (float)num * 0.7f, 0f));
			popupTexts.Add(sBGUILabel4);
			num2 += (float)sBGUILabel4.Height + 40f;
			if (session.TheGame.questManager.IsQuestCompleted((uint)costume.m_nUnlockQuest2))
			{
				SBGUIAtlasImage sBGUIAtlasImage4 = (SBGUIAtlasImage)SBGUI.InstantiatePrefab(prefabName2);
				PositionObject(sBGUIAtlasImage4, new Vector3(checkBoxLocPos.x, checkBoxLocPos.y - (float)num * 0.7f, -0.01f));
				ticks.Add(sBGUIAtlasImage4);
			}
			num++;
		}
		return num2;
	}

	private void CreateCheckBox(int count)
	{
		for (int i = 0; i < count; i++)
		{
			string prefabName = "Prefabs/GUI/Screens/CheckBox";
			SBGUIAtlasImage sBGUIAtlasImage = (SBGUIAtlasImage)SBGUI.InstantiatePrefab(prefabName);
			PositionObject(sBGUIAtlasImage, new Vector3(checkBoxLocPos.x, checkBoxLocPos.y - (float)i * 0.7f, 0f));
			checkBoxes.Add(sBGUIAtlasImage);
		}
	}

	public Vector2 GetWishWidgetRushButtonPosition()
	{
		return m_pCharacterWishWidget.GetRushWishButtonPosition();
	}

	public void SetArrowList(int id)
	{
		m_pArrowList.SetSelectedID(id);
	}
}
