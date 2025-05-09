using UnityEngine;

public class SBGUIInventorySlot : SBGUIScrollListElement
{
	public const int GAP_SIZE = 6;

	private const int MAX_SLOT_ICON_SIZE = 150;

	private const int MAX_REWARDS = 2;

	private const int REWARD_GAP_SIZE = 10;

	public bool needsToBeDeleted;

	private SBGUIAtlasImage iconImage;

	private SBGUILabel titleLabel;

	private SBGUILabel descriptionLabel;

	private SBGUIElement productionInfo;

	private SBGUILabel productionTimeLabel;

	private SBGUIElement rewardMarker;

	private SBGUILabel buttonLabel;

	private SBGUIElement ownedInfo;

	protected SBGUILabel numberOwnedLabel;

	private static int slotCount;

	public static string CalculateSlotName(SBInventoryItem invItem)
	{
		if (invItem.entity != null)
		{
			return string.Format("InventorySlot_{0}", invItem.entity.DefinitionId);
		}
		return string.Format("InventorySlot_{0}", invItem.displayName);
	}

	public static SBGUIInventorySlot MakeInventorySlot()
	{
		SBGUIInventorySlot sBGUIInventorySlot = (SBGUIInventorySlot)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/InventorySlot");
		sBGUIInventorySlot.name = "InventorySlot_" + slotCount++;
		sBGUIInventorySlot.gameObject.transform.parent = GUIMainView.GetInstance().gameObject.transform;
		return sBGUIInventorySlot;
	}

	public void Setup(Session session, SBGUIElement anchor, SBInventoryItem invItem, EventDispatcher<SBInventoryItem> itemClickedEvent, Vector3 offset)
	{
		SetParent(anchor);
		base.tform.localPosition = offset;
		SetActive(true);
		base.name = CalculateSlotName(invItem);
		iconImage = (SBGUIAtlasImage)FindChild("icon");
		iconImage.SetSizeNoRebuild(new Vector2(150f, 150f));
		iconImage.SetTextureFromAtlas(invItem.iconFilename, true, false, true);
		titleLabel = (SBGUILabel)FindChild("name_label");
		titleLabel.SetText(Language.Get(invItem.displayName));
		descriptionLabel = (SBGUILabel)FindChild("description_label");
		productionInfo = FindChild("makes_info");
		productionTimeLabel = (SBGUILabel)FindChild("makes_per_hour_label");
		rewardMarker = FindChild("makes_icon");
		ownedInfo = FindChild("owned_info");
		numberOwnedLabel = (SBGUILabel)FindChild("owned_num_label");
		numberOwnedLabel.SetActive(false);
		buttonLabel = (SBGUILabel)FindChild("place_label");
		ClearButtonActions("button");
		AttachActionToButton("button", delegate
		{
			itemClickedEvent.FireEvent(invItem);
		});
		string text = null;
		switch (invItem.itemType)
		{
		case "movie":
			RemoveProductionInfo();
			text = invItem.description;
			if (text != null)
			{
				descriptionLabel.SetText(Language.Get(text));
			}
			else
			{
				descriptionLabel.SetText(string.Empty);
			}
			RemoveOwnedInfo();
			buttonLabel.SetText(Language.Get("!!PREFAB_PLAY_MOVIE"));
			break;
		default:
		{
			if (invItem.entity.Invariable.ContainsKey("product") && invItem.entity.Invariable["product"] != null)
			{
				RemoveDescriptionInfo();
				RewardDefinition rewardDefinition = (RewardDefinition)invItem.entity.Invariable["product"];
				SBGUIRewardWidget.SetupRewardWidget(session.TheGame.resourceManager, rewardDefinition.Summary, string.Empty, 2, rewardMarker, 10f, false, Color.white);
				ulong duration = (ulong)invItem.entity.Invariable["time.production"];
				productionTimeLabel.SetText(TFUtils.DurationToString(duration));
			}
			else
			{
				RemoveProductionInfo();
				text = session.TheGame.catalog.GetDescription(invItem.entity.DefinitionId);
				if (text != null)
				{
					descriptionLabel.SetText(Language.Get(text));
				}
				else
				{
					descriptionLabel.SetText(string.Empty);
				}
			}
			int level = session.TheGame.resourceManager.Query(ResourceManager.LEVEL);
			Blueprint blueprint = EntityManager.GetBlueprint(invItem.entity.AllTypes, invItem.entity.DefinitionId);
			if (blueprint.GetInstanceLimitByLevel(level).HasValue)
			{
				numberOwnedLabel.SetText(string.Format("{0}/{1}", session.TheGame.entities.GetEntityCount(invItem.entity.AllTypes, invItem.entity.DefinitionId), blueprint.GetInstanceLimitByLevel(level)));
			}
			else
			{
				numberOwnedLabel.SetText(session.TheGame.entities.GetEntityCount(blueprint.PrimaryType, invItem.entity.DefinitionId).ToString());
			}
			break;
		}
		}
		SessionActionId = CalculateSlotName(invItem);
		base.View.RefreshEvent += base.ReregisterColliders;
	}

	private void RemoveProductionInfo()
	{
		productionInfo.SetActive(false);
	}

	private void RemoveOwnedInfo()
	{
		ownedInfo.SetActive(false);
	}

	private void RemoveDescriptionInfo()
	{
		descriptionLabel.SetActive(false);
	}

	public override void Deactivate()
	{
		ClearButtonActions("button");
		SBGUIRewardWidget[] componentsInChildren = base.gameObject.GetComponentsInChildren<SBGUIRewardWidget>(true);
		SBGUIRewardWidget[] array = componentsInChildren;
		foreach (SBGUIRewardWidget sBGUIRewardWidget in array)
		{
			sBGUIRewardWidget.SetParent(null);
			sBGUIRewardWidget.gameObject.SetActiveRecursively(false);
			SBGUIRewardWidget.ReleaseRewardWidget(sBGUIRewardWidget);
		}
		base.Deactivate();
	}
}
