using System;
using System.Collections.Generic;
using UnityEngine;

public class SBGUIAutoQuestStatusDialog : SBGUIScrollableDialog
{
	public const int STEP_GAP = 0;

	private int? questIconSize;

	private float markerXOffset;

	private Vector2 scrollSize;

	private SBGUIPulseButton okayButton;

	private SBGUIPulseButton allDoneButton;

	private SBGUIAtlasImage window;

	private SBGUIElement stepsMarker;

	private int numChunksLeft = -1;

	public override void SetParent(SBGUIElement element)
	{
		SetTransformParent(element);
	}

	public void CreateScrollRegionUI(SBGUIStandardScreen screen, List<QuestBookendInfo.ChunkConditions> chunks, List<ConditionDescription> steps, Action makeButtonHandler, string forcedStepPrefabName = null)
	{
		scrollSize = Vector2.zero;
		if (region.Marker.transform.GetChildCount() > 0)
		{
			SBGUIElement[] componentsInChildren = region.Marker.GetComponentsInChildren<SBGUIElement>();
			YGAtlasSprite[] componentsInChildren2 = region.Marker.GetComponentsInChildren<YGAtlasSprite>();
			YGAtlasSprite[] array = componentsInChildren2;
			foreach (YGAtlasSprite yGAtlasSprite in array)
			{
				if (!string.IsNullOrEmpty(yGAtlasSprite.nonAtlasName))
				{
					base.View.Library.incrementTextureDuplicates(yGAtlasSprite.nonAtlasName);
				}
			}
			SBGUIElement[] array2 = componentsInChildren;
			foreach (SBGUIElement sBGUIElement in array2)
			{
				if (!(sBGUIElement == region.Marker))
				{
					UnityEngine.Object.Destroy(sBGUIElement.gameObject);
				}
			}
		}
		int count = chunks.Count;
		List<QuestBookendInfo.ChunkConditions> list = new List<QuestBookendInfo.ChunkConditions>(count);
		List<ConditionDescription> list2 = new List<ConditionDescription>(count);
		int k;
		for (k = 0; k < count; k++)
		{
			if (!steps[k].IsPassed && k != count - 1)
			{
				list.Insert(0, chunks[k]);
				list2.Insert(0, steps[k]);
			}
			else
			{
				list.Add(chunks[k]);
				list2.Add(steps[k]);
			}
		}
		k = 0;
		numChunksLeft = count - 1;
		foreach (QuestBookendInfo.ChunkConditions item in list)
		{
			if (k >= count - 1)
			{
				break;
			}
			Dictionary<string, object> dictionary = item.Condition.ToDict();
			int? num = null;
			int? num2 = null;
			CraftingRecipe craftingRecipe = null;
			Simulated simulated = null;
			string prefabName = "Prefabs/GUI/Widgets/AutoQuest_Step";
			Action action = null;
			if (dictionary.ContainsKey("resource_id"))
			{
				num = TFUtils.LoadInt(dictionary, "resource_id");
				craftingRecipe = session.TheGame.craftManager.GetRecipeByProductId(num.Value);
				simulated = session.TheGame.simulation.FindSimulated(craftingRecipe.buildingId);
			}
			if (dictionary.ContainsKey("simulated_id"))
			{
				num2 = TFUtils.LoadInt(dictionary, "simulated_id");
			}
			SBGUIElement sBGUIElement2 = SBGUI.InstantiatePrefab(prefabName);
			SBGUIImage sBGUIImage = (SBGUIImage)sBGUIElement2.FindChild("window");
			SBGUIAtlasImage sBGUIAtlasImage = (SBGUIAtlasImage)sBGUIElement2.FindChild("item_icon");
			SBGUILabel sBGUILabel = (SBGUILabel)sBGUIElement2.FindChild("item_name");
			SBGUIAtlasImage boundary = (SBGUIAtlasImage)sBGUIElement2.FindChild("item_name_boundary");
			SBGUILabel stepYouHave = (SBGUILabel)sBGUIElement2.FindChild("you_have_amount");
			SBGUILabel sBGUILabel2 = (SBGUILabel)sBGUIElement2.FindChild("you_need_amount");
			SBGUILabel stepYouHaveTitle = (SBGUILabel)sBGUIElement2.FindChild("you_have_title");
			SBGUILabel stepYouNeedTitle = (SBGUILabel)sBGUIElement2.FindChild("you_need_title");
			SBGUIButton sBGUIButton = (SBGUIButton)sBGUIElement2.FindChild("make_button");
			SBGUIButton collectButton = (SBGUIButton)sBGUIElement2.FindChild("collect_button");
			GameObject doneGO = sBGUIElement2.FindChild("done_icon").gameObject.transform.parent.gameObject;
			stepsMarker = FindChild("steps_marker");
			sBGUIElement2.SetParent(stepsMarker);
			sBGUIElement2.tform.localPosition = Vector3.zero;
			sBGUIElement2.tform.localPosition = new Vector3(0f, (0f - sBGUIImage.Size.y * 0.01f) * (float)k, 0f);
			Dictionary<string, object> pCondition = item.Condition.ToDict();
			int nYouNeed = 0;
			if (pCondition.ContainsKey("count"))
			{
				nYouNeed = TFUtils.LoadInt(pCondition, "count");
				sBGUILabel2.SetText(nYouNeed.ToString());
			}
			int num3 = 0;
			Resource resource = null;
			if (pCondition.ContainsKey("resource_id"))
			{
				int key = TFUtils.LoadInt(pCondition, "resource_id");
				resource = session.TheGame.resourceManager.Resources[key];
				sBGUIAtlasImage.SetTextureFromAtlas(resource.GetResourceTexture(), false);
				sBGUIAtlasImage.ScaleToMaxSize((int)sBGUIAtlasImage.Size.x);
				sBGUILabel.SetText(Language.Get(resource.Name));
				sBGUILabel.AdjustText(boundary);
				num3 = resource.Amount;
				stepYouHave.SetText(num3.ToString());
			}
			doneGO.SetActive(list2[k].IsPassed);
			if (list2[k].IsPassed)
			{
				numChunksLeft--;
				sBGUIButton.SetActive(false);
				collectButton.SetActive(false);
				stepYouHave.SetActive(false);
				stepYouHaveTitle.SetActive(false);
				stepYouNeedTitle.SetActive(false);
			}
			else if (num3 >= nYouNeed)
			{
				sBGUIButton.SetActive(false);
				collectButton.SetActive(true);
				stepYouHave.SetColor(Color.blue);
				Action action2 = delegate
				{
					session.TheSoundEffectManager.PlaySound("Accept");
					if (pCondition.ContainsKey("resource_id"))
					{
						int nDID = TFUtils.LoadInt(pCondition, "resource_id");
						session.TheGame.simulation.ModifyGameStateSimulated(session.TheGame.simulation.FindSimulated(1018), new AutoQuestCraftCollectAction(nDID, nYouNeed));
						stepYouHave.SetActive(false);
						stepYouHaveTitle.SetActive(false);
						stepYouNeedTitle.SetActive(false);
						collectButton.SetActive(false);
						doneGO.SetActive(true);
						numChunksLeft--;
						if (numChunksLeft <= 0)
						{
							okayButton.SetActive(false);
							allDoneButton.SetActive(true);
						}
					}
				};
				AttachActionToButton(collectButton, action2);
			}
			else
			{
				sBGUIButton.SetActive(false);
				collectButton.SetActive(false);
				stepYouHave.SetColor(Color.red);
				if (resource != null)
				{
					CraftingRecipe recipeByProductId = session.TheGame.craftManager.GetRecipeByProductId(resource.Did);
					if (recipeByProductId != null)
					{
						Simulated pSimulated = session.TheGame.simulation.FindSimulated(recipeByProductId.buildingId);
						if (pSimulated != null && pSimulated.HasEntity<BuildingEntity>())
						{
							bool flag = true;
							BuildingEntity entity = pSimulated.GetEntity<BuildingEntity>();
							if (entity.CanCraft && session.TheGame.craftManager.GetCookbookById(entity.CraftMenu) != null && entity.HasSlots)
							{
								sBGUIButton.SetActive(true);
								Action action3 = delegate
								{
									session.TheSoundEffectManager.PlaySound("Accept");
									session.TheGame.selected = pSimulated;
									session.ChangeState("BrowsingRecipes");
								};
								AttachActionToButton(sBGUIButton, makeButtonHandler);
								AttachActionToButton(sBGUIButton, action3);
								flag = false;
							}
							else if (entity.CanVend)
							{
								VendingDecorator entity2 = pSimulated.GetEntity<VendingDecorator>();
								if (entity2 != null)
								{
									VendorDefinition vendorDefinition = session.TheGame.vendingManager.GetVendorDefinition(entity2.VendorId);
									if (vendorDefinition != null)
									{
										sBGUIButton.SetActive(true);
										Action action4 = delegate
										{
											session.TheSoundEffectManager.PlaySound("Accept");
											session.TheGame.selected = pSimulated;
											session.ChangeState("vending");
										};
										AttachActionToButton(sBGUIButton, makeButtonHandler);
										AttachActionToButton(sBGUIButton, action4);
										flag = false;
									}
								}
							}
							if (flag)
							{
								sBGUIButton.SetActive(true);
								Action action5 = delegate
								{
									session.TheSoundEffectManager.PlaySound("Accept");
									session.TheGame.selected = pSimulated;
									session.TheCamera.AutoPanToPosition(pSimulated.PositionCenter, 0.75f);
									session.ChangeState("SelectedPlaying");
								};
								AttachActionToButton(sBGUIButton, makeButtonHandler);
								AttachActionToButton(sBGUIButton, action5);
							}
						}
					}
				}
			}
			scrollSize += (sBGUIImage.Size + new Vector2(0f, 0f)) * 0.01f;
			float y = sBGUIElement2.transform.localPosition.y;
			sBGUIElement2.SetParent(region.Marker);
			Vector3 localPosition = new Vector3(0f, y, 0f);
			sBGUIElement2.transform.localPosition = localPosition;
			sBGUIButton.UpdateCollider();
			collectButton.UpdateCollider();
			k++;
		}
		if (numChunksLeft <= 0)
		{
			okayButton.SetActive(false);
			allDoneButton.SetActive(true);
		}
		else
		{
			okayButton.SetActive(true);
			allDoneButton.SetActive(false);
		}
		Rect rect = new Rect(0f, 0f, scrollSize.x, scrollSize.y);
		region.ResetScroll(rect);
		region.ResetToMinScroll();
		if (region.scrollBar.IsActive())
		{
			region.scrollBar.Reset();
		}
	}

	public void SetupDialogInfo(string sDialogHeading, string sDialogBody, string sPortrait, List<Reward> pRewards, List<ConditionDescription> steps, QuestDefinition pQuestDef)
	{
		SBGUILabel sBGUILabel = (SBGUILabel)FindChild("dialog_heading");
		SBGUILabel sBGUILabel2 = (SBGUILabel)FindChild("dialog_body");
		SBGUIAtlasImage sBGUIAtlasImage = (SBGUIAtlasImage)FindChild("portrait");
		SBGUIAtlasImage sBGUIAtlasImage2 = (SBGUIAtlasImage)FindChild("portrait_shadow");
		SBGUIAtlasImage boundary = (SBGUIAtlasImage)FindChild("dialog_body_boundary");
		SBGUIShadowedLabel sBGUIShadowedLabel = (SBGUIShadowedLabel)FindChild("reward_label");
		SBGUILabel sBGUILabel3 = (SBGUILabel)FindChild("reward_gold_label");
		SBGUILabel sBGUILabel4 = (SBGUILabel)FindChild("reward_xp_label");
		stepsMarker = FindChild("steps_marker");
		window = (SBGUIAtlasImage)FindChild("window");
		okayButton = (SBGUIPulseButton)FindChild("okay");
		allDoneButton = (SBGUIPulseButton)FindChild("done");
		int num = 0;
		int num2 = 0;
		int count = pRewards.Count;
		for (int i = 0; i < count; i++)
		{
			Reward reward = pRewards[i];
			if (reward.ResourceAmounts.ContainsKey(ResourceManager.SOFT_CURRENCY))
			{
				num += reward.ResourceAmounts[ResourceManager.SOFT_CURRENCY];
			}
			if (reward.ResourceAmounts.ContainsKey(ResourceManager.XP))
			{
				num2 += reward.ResourceAmounts[ResourceManager.XP];
			}
		}
		sBGUILabel3.SetText(num.ToString());
		sBGUILabel4.SetText(num2.ToString());
		numChunksLeft = steps.Count - 1;
		for (int j = 0; j < steps.Count - 1; j++)
		{
			if (steps[j].IsPassed)
			{
				numChunksLeft--;
			}
		}
		if (numChunksLeft <= 0)
		{
			okayButton.SetActive(false);
			allDoneButton.SetActive(true);
		}
		else
		{
			okayButton.SetActive(true);
			allDoneButton.SetActive(false);
		}
		string text = Language.Get(sDialogBody);
		if (text.Contains("{0}") && pQuestDef.AutoQuestCharacterID >= 0)
		{
			Simulated simulated = session.TheGame.simulation.FindSimulated(pQuestDef.AutoQuestCharacterID);
			if (simulated != null && simulated.HasEntity<ResidentEntity>())
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				text = string.Format(text, Language.Get(entity.Name));
			}
		}
		sBGUILabel.SetText(Language.Get(sDialogHeading));
		sBGUILabel2.SetText(text);
		sBGUILabel2.AdjustText(boundary);
		sBGUIShadowedLabel.SetText(Language.Get("!!PREFAB_REWARD") + ":");
		sBGUIAtlasImage.SetTextureFromAtlas(sPortrait, true);
		sBGUIAtlasImage2.GetComponent<Renderer>().material.SetColor("_Color", new Color(0f, 0f, 0f, 0.2f));
	}
}
