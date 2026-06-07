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
		return null;
	}

	public static SBGUIInventorySlot MakeInventorySlot()
	{
		return null;
	}

	public void Setup(Session session, SBGUIElement anchor, SBInventoryItem invItem, EventDispatcher<SBInventoryItem> itemClickedEvent, Vector3 offset)
	{
	}

	private void RemoveProductionInfo()
	{
	}

	private void RemoveOwnedInfo()
	{
	}

	private void RemoveDescriptionInfo()
	{
	}

	public override void Deactivate()
	{
	}
}
