using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SBGUIChunkQuestDialog : SBGUIScrollableDialog
{
	public const int STEP_GAP = -10;

	private const float PROGRESSBAR_HEIGHT = 71f;

	private const float PROGRESSBAR_FILLRATE = 0.1f;

	private const int REWARD_GAP_SIZE = 10;

	public GameObject rewardWidgetPrefab;

	public ParticleSystem progressBarParticle;

	private List<SBGUIRewardWidget> rewards = new List<SBGUIRewardWidget>();

	private int? prefabIconSize;

	private int? questLineIconSize;

	private int? questIconSize;

	private float markerXOffset;

	private Vector2 scrollSize;

	private Vector2? prefabWindowSize;

	private Vector3? prefabOkayButtonPos;

	private SBGUIProgressMeter progressMeter;

	private SBGUIPulseButton okayButton;

	private SBGUIAtlasImage questlineRewardIcon;

	private SBGUIAtlasImage questRewardIcon;

	private SBGUIAtlasImage window;

	private SBGUIElement rewardItemBg;

	private SBGUIElement progressbar_group;

	private SBGUIElement stepsMarker;

	protected SBGUIElement rewardMarker;

	private double residentPosX;

	private double residentPosY;

	protected override void Awake()
	{
		if (rewardWidgetPrefab == null)
		{
			rewardWidgetPrefab = (GameObject)Resources.Load("Prefabs/GUI/Widgets/RewardWidget");
		}
		rewardMarker = FindChild("reward_marker");
		base.Awake();
	}

	public override void SetParent(SBGUIElement element)
	{
		SetTransformParent(element);
	}

	public void CreateScrollRegionUI(SBGUIStandardScreen screen, List<QuestBookendInfo.ChunkConditions> chunks, List<ConditionDescription> steps, Action findButtonHandler, string forcedStepPrefabName = null)
	{
		scrollSize = Vector2.zero;
		if (region.Marker.transform.GetChildCount() > 0)
		{
			TFUtils.ErrorLog("Child Count: " + region.Marker.transform.childCount);
			YGAtlasSprite[] componentsInChildren = region.Marker.GetComponentsInChildren<YGAtlasSprite>();
			YGAtlasSprite[] array = componentsInChildren;
			foreach (YGAtlasSprite yGAtlasSprite in array)
			{
				if (!string.IsNullOrEmpty(yGAtlasSprite.nonAtlasName))
				{
					base.View.Library.incrementTextureDuplicates(yGAtlasSprite.nonAtlasName);
				}
			}
			SBGUIElement[] componentsInChildren2 = region.Marker.GetComponentsInChildren<SBGUIElement>();
			SBGUIElement[] array2 = componentsInChildren2;
			foreach (SBGUIElement sBGUIElement in array2)
			{
				if (!(sBGUIElement == region.Marker))
				{
					UnityEngine.Object.Destroy(sBGUIElement.gameObject);
				}
			}
		}
		int num = 0;
		foreach (QuestBookendInfo.ChunkConditions chunk in chunks)
		{
			Dictionary<string, object> dictionary = chunk.Condition.ToDict();
			int? num2 = null;
			int? num3 = null;
			CraftingRecipe craftingRecipe = null;
			Simulated simBuilding = null;
			string text = null;
			Action action = null;
			int nDID = 0;
			int num4 = 0;
			if (dictionary.ContainsKey("resource_id"))
			{
				num2 = TFUtils.LoadInt(dictionary, "resource_id");
				craftingRecipe = session.TheGame.craftManager.GetRecipeByProductId(num2.Value);
				simBuilding = session.TheGame.simulation.FindSimulated(craftingRecipe.buildingId);
			}
			if (dictionary.ContainsKey("simulated_id"))
			{
				num3 = TFUtils.LoadInt(dictionary, "simulated_id");
			}
			if (dictionary.ContainsKey("task_id"))
			{
				nDID = TFUtils.LoadInt(dictionary, "task_id");
			}
			if (forcedStepPrefabName == null)
			{
				if (steps[num].IsPassed)
				{
					text = "Prefabs/GUI/Widgets/QuestCompleteChunk_Step";
				}
				else
				{
					text = "Prefabs/GUI/Widgets/QuestStatusChunk_Step";
					text = "Prefabs/GUI/Widgets/QuestStartChunk_Step";
				}
			}
			else
			{
				text = forcedStepPrefabName;
			}
			SBGUIElement sBGUIElement2 = SBGUI.InstantiatePrefab(text);
			SBGUIImage sBGUIImage = (SBGUIImage)sBGUIElement2.FindChild("window");
			SBGUILabel sBGUILabel = (SBGUILabel)sBGUIElement2.FindChild("description");
			SBGUIShadowedLabel sBGUIShadowedLabel = (SBGUIShadowedLabel)sBGUIElement2.FindChild("count");
			SBGUIButton sBGUIButton = (SBGUIButton)sBGUIElement2.FindChild("store_button");
			SBGUIButton sBGUIButton2 = (SBGUIButton)sBGUIElement2.FindChild("skip_button");
			SBGUIShadowedLabel sBGUIShadowedLabel2 = (SBGUIShadowedLabel)sBGUIElement2.FindChild("store_label");
			SBGUIAtlasImage sBGUIAtlasImage = (SBGUIAtlasImage)sBGUIElement2.FindChild("icon");
			SBGUIAtlasImage sBGUIAtlasImage2 = (SBGUIAtlasImage)sBGUIElement2.FindChild("description_boundary");
			SBGUIButton sBGUIButton3 = (SBGUIButton)sBGUIElement2.FindChild("find_button");
			stepsMarker = FindChild("steps_marker");
			prefabIconSize = (int)sBGUIAtlasImage.Size.x;
			sBGUIElement2.SetParent(stepsMarker);
			sBGUIElement2.tform.localPosition = Vector3.zero;
			sBGUIElement2.tform.localPosition = new Vector3(0f, (0f - (sBGUIImage.Size.y + -10f) * 0.01f) * (float)num, 0f);
			sBGUILabel.SetText(Language.Get(chunk.Name));
			sBGUIAtlasImage.SetTextureFromAtlas(chunk.Icon, true, false, true);
			if ((bool)sBGUIShadowedLabel)
			{
				if (chunk.Condition.hasCountField)
				{
					uint num5 = steps[num].OccuranceCount;
					uint occurancesRequired = steps[num].OccurancesRequired;
					if (num5 > occurancesRequired)
					{
						num5 = occurancesRequired;
					}
					sBGUIShadowedLabel.SetText(num5 + " / " + occurancesRequired);
				}
				else
				{
					sBGUIShadowedLabel.SetActive(false);
					sBGUILabel.transform.Translate(-0.45f, 0f, 0f, Space.Self);
					sBGUIAtlasImage2.transform.Translate(-0.45f, 0f, 0f, Space.Self);
					Vector2 size = sBGUIAtlasImage2.Size;
					size.x += 45f;
					sBGUIAtlasImage2.Size = size;
				}
			}
			if (!steps[num].IsPassed && steps[num].OccuranceCount < steps[num].OccurancesRequired && steps[num].OccuranceCount < steps[num].OccurancesRequired)
			{
				sBGUIButton3.SetActive(false);
				TaskData taskData = null;
				taskData = session.TheGame.taskManager.GetTaskData(nDID);
				if (taskData != null)
				{
					num4 = taskData.m_nSourceDID;
					Simulated pSimulated = session.TheGame.simulation.FindSimulated(num4);
					bool flag = true;
					if (pSimulated != null && pSimulated.HasEntity<ResidentEntity>())
					{
						sBGUIButton3.SetActive(true);
						ResidentEntity pResidentEntity = pSimulated.GetEntity<ResidentEntity>();
						if (pResidentEntity.m_pTask != null && (pResidentEntity.m_pTask.m_bMovingToTarget || pResidentEntity.m_pTask.m_bAtTarget))
						{
							residentPosX = pResidentEntity.m_pTaskTargetPosition.x;
							residentPosY = pResidentEntity.m_pTaskTargetPosition.y;
						}
						else
						{
							residentPosX = -1.0;
						}
						Action action2 = delegate
						{
							session.TheSoundEffectManager.PlaySound("Accept");
							session.TheGame.selected = pSimulated;
							if (pResidentEntity.m_pTask != null && (pResidentEntity.m_pTask.m_bMovingToTarget || pResidentEntity.m_pTask.m_bAtTarget))
							{
								session.ChangeState("UnitBusy");
							}
						};
						AttachActionToButton(sBGUIButton3, findButtonHandler);
						AttachActionToButton(sBGUIButton3, action2);
					}
					if (flag)
					{
						sBGUIButton3.SetActive(true);
						Action action3 = delegate
						{
							session.TheSoundEffectManager.PlaySound("Accept");
							session.TheGame.selected = pSimulated;
							ResidentEntity entity = pSimulated.GetEntity<ResidentEntity>();
							if (residentPosX > 1000010.0 || residentPosX == 0.0 || pSimulated.PositionCenter.x > 1000010f || pSimulated.PositionCenter.x == 0f)
							{
								session.TheCamera.AutoPanToPosition(new Vector2(1000.9f, 667.5f), 0.75f);
							}
							else if (residentPosX > 0.0 && residentPosX < 1000010.0)
							{
								session.TheCamera.AutoPanToPosition(new Vector2((float)residentPosX, (float)residentPosY), 0.75f);
							}
							else if (residentPosX == -1.0)
							{
								session.TheCamera.AutoPanToPosition(pSimulated.PositionCenter, 0.75f);
								session.ChangeState("UnitIdle");
							}
						};
						AttachActionToButton(sBGUIButton3, findButtonHandler);
						AttachActionToButton(sBGUIButton3, action3);
					}
				}
			}
			sBGUILabel.AdjustText(sBGUIAtlasImage2);
			if ((bool)sBGUIButton)
			{
				if (simBuilding != null)
				{
					action = delegate
					{
						session.TheGame.selected = simBuilding;
						simBuilding.InteractionState.SelectedTransition.Apply(session);
					};
					sBGUIShadowedLabel2.SetText(Language.Get("!!PREFAB_STORE"));
					sBGUIButton.ClickEvent += action;
				}
				else if (num3.HasValue)
				{
					action = delegate
					{
						SBGUIButton sBGUIButton4 = (SBGUIButton)screen.FindChild("marketplace");
						sBGUIButton4.MockClick();
					};
					sBGUIShadowedLabel2.SetText(Language.Get("!!PREFAB_STORE"));
					sBGUIButton.ClickEvent += action;
				}
				else
				{
					UnityEngine.Object.Destroy(sBGUIButton.gameObject);
					if ((bool)sBGUIButton2)
					{
						UnityEngine.Object.Destroy(sBGUIButton2.gameObject);
					}
				}
			}
			GUIMainView.GetInstance().Library.bShowingDialog = true;
			scrollSize += (sBGUIImage.Size + new Vector2(-10f, -10f)) * 0.01f;
			float y = sBGUIElement2.transform.localPosition.y;
			sBGUIElement2.SetParent(region.Marker);
			Vector3 localPosition = new Vector3(0f, y, 0f);
			sBGUIElement2.transform.localPosition = localPosition;
			if ((bool)sBGUIButton)
			{
				sBGUIButton.UpdateCollider();
			}
			if ((bool)sBGUIButton3)
			{
				sBGUIButton3.UpdateCollider();
			}
			num++;
		}
		Rect rect = new Rect(0f, 0f, scrollSize.x, scrollSize.y);
		region.ResetScroll(rect);
		region.ResetToMinScroll();
		if (region.scrollBar.IsActive())
		{
			region.scrollBar.Reset();
		}
	}

	public void SetupChunkDialogInfo(string dialogHeading, string dialogBody, string portrait, string name, bool isComplete, QuestDefinition pQuestDef)
	{
		SBGUIShadowedLabel sBGUIShadowedLabel = (SBGUIShadowedLabel)FindChild("dialog_heading");
		SBGUILabel sBGUILabel = (SBGUILabel)FindChild("dialog_body");
		SBGUIShadowedLabel sBGUIShadowedLabel2 = (SBGUIShadowedLabel)FindChild("description");
		SBGUIAtlasImage sBGUIAtlasImage = (SBGUIAtlasImage)FindChild("portrait");
		SBGUIAtlasImage sBGUIAtlasImage2 = (SBGUIAtlasImage)FindChild("portrait_shadow");
		SBGUIAtlasImage sBGUIAtlasImage3 = (SBGUIAtlasImage)FindChild("dialog_body_boundary");
		SBGUIShadowedLabel sBGUIShadowedLabel3 = (SBGUIShadowedLabel)FindChild("reward_label");
		questlineRewardIcon = (SBGUIAtlasImage)FindChild("questline_reward_item");
		questRewardIcon = (SBGUIAtlasImage)FindChild("reward_item");
		progressMeter = (SBGUIProgressMeter)FindChild("progress_meter");
		progressbar_group = FindChild("progressbar_group");
		rewardItemBg = FindChild("reward_item_bg");
		stepsMarker = FindChild("steps_marker");
		window = (SBGUIAtlasImage)FindChild("window");
		okayButton = (SBGUIPulseButton)FindChild("okay");
		int? num = questLineIconSize;
		if (!num.HasValue)
		{
			questLineIconSize = (int)questlineRewardIcon.Size.x;
		}
		int? num2 = questIconSize;
		if (!num2.HasValue)
		{
			questIconSize = (int)questRewardIcon.Size.x;
		}
		Vector2? vector = prefabWindowSize;
		if (!vector.HasValue)
		{
			prefabWindowSize = window.Size;
		}
		Vector3? vector2 = prefabOkayButtonPos;
		if (!vector2.HasValue)
		{
			prefabOkayButtonPos = okayButton.tform.localPosition;
		}
		rewardItemBg.SetActive(false);
		if (!rewardItemBg.IsActive() && isComplete)
		{
			rewardMarker.transform.Translate(new Vector3(0f, 0.5f, 0f));
		}
		if (dialogHeading != string.Empty)
		{
			sBGUIShadowedLabel.SetText(Language.Get(dialogHeading));
		}
		else
		{
			sBGUIShadowedLabel.SetActive(false);
			sBGUILabel.transform.localPosition = sBGUIAtlasImage3.transform.localPosition;
		}
		sBGUIShadowedLabel2.SetText(Language.Get(name));
		sBGUILabel.SetText(Language.Get(dialogBody));
		sBGUILabel.AdjustText(sBGUIAtlasImage3);
		sBGUIShadowedLabel3.SetText(Language.Get("!!PREFAB_REWARD") + ":");
		sBGUIAtlasImage.SetTextureFromAtlas(portrait, true);
		sBGUIAtlasImage2.GetComponent<Renderer>().material.SetColor("_Color", new Color(0f, 0f, 0f, 0.2f));
	}

	public void SetQuestLineInfo(QuestLineInfo questLine, float? start, float? progress, bool skipAnimation)
	{
		if (progress.HasValue)
		{
			if (!progressbar_group.IsActive())
			{
				progressbar_group.SetActive(true);
			}
			Vector2 size = window.Size;
			Vector2? vector = prefabWindowSize;
			if (size != vector.GetValueOrDefault() || !vector.HasValue)
			{
				SBGUIAtlasImage sBGUIAtlasImage = window;
				Vector2? vector2 = prefabWindowSize;
				sBGUIAtlasImage.Size = vector2.Value;
			}
			Vector3 localPosition = okayButton.tform.localPosition;
			Vector3? vector3 = prefabOkayButtonPos;
			if (localPosition != vector3.GetValueOrDefault() || !vector3.HasValue)
			{
				Transform obj = okayButton.tform;
				Vector3? vector4 = prefabOkayButtonPos;
				obj.localPosition = vector4.Value;
			}
			if (skipAnimation)
			{
				progressMeter.Progress = progress.Value;
			}
			else
			{
				float duration = (progress.Value - start.Value) / 0.1f;
				progressMeter.ForceAnimatedProgress(start.Value, progress.Value, duration);
				if (progressBarParticle != null)
				{
					progressBarParticle = (ParticleSystem)UnityEngine.Object.Instantiate(progressBarParticle);
					progressBarParticle.transform.parent = progressMeter.fill.transform;
					progressBarParticle.transform.localPosition = Vector3.zero;
					StartCoroutine(AnimateParticlePosition(duration));
				}
			}
			questlineRewardIcon.SetTextureFromAtlas(questLine.Icon);
			SBGUIAtlasImage sBGUIAtlasImage2 = questlineRewardIcon;
			int? num = questLineIconSize;
			sBGUIAtlasImage2.ScaleToMaxSize(num.Value);
		}
		else
		{
			if (progressbar_group.IsActive())
			{
				progressbar_group.SetActive(false);
			}
			Vector2 size2 = window.Size;
			Vector2? vector5 = prefabWindowSize;
			if (size2 == vector5.GetValueOrDefault() && vector5.HasValue)
			{
				Vector2? vector6 = prefabWindowSize;
				Vector2 value = vector6.Value;
				value.y -= 71f;
				window.Size = value;
			}
			Vector3 localPosition2 = okayButton.tform.localPosition;
			Vector3? vector7 = prefabOkayButtonPos;
			if (localPosition2 == vector7.GetValueOrDefault() && vector7.HasValue)
			{
				Vector3? vector8 = prefabOkayButtonPos;
				Vector3 value2 = vector8.Value;
				value2.y += 0.71f;
				okayButton.tform.localPosition = value2;
			}
		}
	}

	private IEnumerator AnimateParticlePosition(float duration)
	{
		progressBarParticle.Play();
		float elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			Vector3 position = progressBarParticle.transform.localPosition;
			position.x = progressMeter.meter.Size.x * progressMeter.Progress;
			position *= 0.01f;
			position.z = -0.3f;
			progressBarParticle.transform.localPosition = position;
			yield return null;
		}
		progressBarParticle.Stop();
		yield return null;
	}

	public virtual void AddItem(string texture, int amount, string prefix)
	{
		if (amount == 0)
		{
			TFUtils.WarningLog("rewarding 0 of :" + texture);
			return;
		}
		if (texture == null || string.IsNullOrEmpty(texture.Trim()))
		{
			TFUtils.WarningLog("resource has no texture");
			return;
		}
		SBGUIRewardWidget item = SBGUIRewardWidget.Create(rewardWidgetPrefab, rewardMarker, markerXOffset, texture, amount, prefix);
		rewards.Add(item);
		markerXOffset = 0f;
		float num = 1f;
		float y = 0f;
		if (rewards.Count > 3)
		{
			num = 0.5f;
			y = 0.1f;
		}
		foreach (SBGUIRewardWidget reward in rewards)
		{
			reward.transform.localScale = new Vector3(num, num, num);
			reward.transform.localPosition = new Vector3(markerXOffset, y, 0f);
			markerXOffset += (float)(reward.Width + 10) * num * 0.01f;
		}
	}

	private void ClearItems()
	{
		markerXOffset = 0f;
		foreach (SBGUIRewardWidget reward in rewards)
		{
			reward.gameObject.SetActiveRecursively(false);
			UnityEngine.Object.Destroy(reward.gameObject);
		}
		rewards.Clear();
	}

	private void InitializeRewardComponentAmounts(Reward reward, Dictionary<int, int> componentAmounts, Dictionary<int, int> outAmounts)
	{
		outAmounts.Clear();
		foreach (KeyValuePair<int, int> componentAmount in componentAmounts)
		{
			int key = componentAmount.Key;
			int value = componentAmount.Value;
			if (!outAmounts.ContainsKey(key))
			{
				outAmounts[key] = 0;
			}
			Dictionary<int, int> dictionary2;
			Dictionary<int, int> dictionary = (dictionary2 = outAmounts);
			int key3;
			int key2 = (key3 = key);
			key3 = dictionary2[key3];
			dictionary[key2] = key3 + value;
		}
	}

	public void SetRewardIcons(Session session, List<Reward> rewards, string prefix)
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
		ClearItems();
		foreach (Reward reward in rewards)
		{
			if (reward != null)
			{
				InitializeRewardComponentAmounts(reward, reward.ResourceAmounts, dictionary);
				InitializeRewardComponentAmounts(reward, reward.BuildingAmounts, dictionary2);
			}
		}
		foreach (KeyValuePair<int, int> item in dictionary)
		{
			int key = item.Key;
			int value = item.Value;
			AddItem(session.TheGame.resourceManager.Resources[key].GetResourceTexture(), value, prefix);
		}
		foreach (KeyValuePair<int, int> item2 in dictionary2)
		{
			int value2 = item2.Value;
			Blueprint blueprint = EntityManager.GetBlueprint("building", item2.Key);
			AddItem((string)blueprint.Invariable["portrait"], value2, prefix);
		}
	}

	public void CenterRewards()
	{
		Vector3 position = rewardMarker.tform.position;
		Vector3 vector = rewardMarker.TotalBounds.center - position;
		Vector3 localPosition = rewardMarker.tform.localPosition;
		localPosition.x -= vector.x;
		rewardMarker.tform.localPosition = localPosition;
	}
}
