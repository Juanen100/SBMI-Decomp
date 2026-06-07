using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Yarg;

public class SBGUIStandardScreen : SBGUIScreen
{
	public class Positioning
	{
		public GameObject gameObject;

		public Vector3 origin;

		public Vector3 target;

		public Positioning(GameObject gameObject, Vector3 origin, Vector3 target)
		{
		}
	}

	private class Interpolator
	{
		private int locks;

		public bool IsLocked
		{
			get
			{
				return false;
			}
		}

		public void Lock()
		{
		}

		public void Unlock()
		{
		}

		public void UpdateUIEasing(Dictionary<string, Positioning> elementPositionings, float interp, Func<float, float, float, float> easingMethod)
		{
		}
	}

	public const string ORIGINAL_DRAG_EVENT = "OriginalDragEvent";

	public const string LAST_TOUCHED_PRODUCT = "LastStandardHudTouchedProduct";

	public Vector2 FoodDeliverySize;

	public static bool userClosedWishList;

	public EventDispatcher<int> QuestStatusEvent;

	private const int QUEST_GAP = 12;

	private const float DISPLAY_UI_TIMEOUT = 30f;

	private Dictionary<string, Positioning> elementPositionings;

	private List<GameObject> nativeElements;

	public SBGUIElement questMarker;

	private SBGUIButton questsOrigin;

	public SBGUIElement questCountIcon;

	private SBGUIElement settingsHudIcon;

	private SBGUIElement settingsHudCountIcon;

	private SBGUILabel settingsHudCountLabel;

	private SBGUIElement editModeHudIcon;

	private SBGUIElement inventoryHudIcon;

	private SBGUIElement marketplaceHudIcon;

	private SBGUIElement communityEventHudIcon;

	private SBGUIElement patchyHudTitleIcon;

	private SBGUIElement patchyHudTitleLabel;

	private SBGUIElement patchyHudTitleBg;

	private SBGUIElement patchyHudIcon;

	private SBGUIElement happyfaceHud;

	private SBGUIElement jjBarHud;

	private SBGUIElement moneyBarHud;

	private SBGUIElement specialBarHud;

	private SBGUILabel questCountLabel;

	private int helpshiftNotificationCount;

	private SBGUIAtlasButton pathEditToggle;

	private SBGUIPulseImage softCurrencyBar;

	private SBGUIPulseImage softCurrencyIcon;

	private SBGUIPulseImage hardCurrencyBar;

	private SBGUIPulseImage hardCurrencyIcon;

	private SBGUIElement xpBar;

	private SBGUIPulseImage xpBarStar;

	private SBGUIPulseButton questScrollUp;

	private SBGUIPulseButton questScrollDown;

	private float? questScrollUpperBound;

	private float? questScrollLowerBound;

	private bool questMarkersDoneAnimating;

	public SBGUIInventoryHudWidget inventory;

	public ReadyEventDispatcher ReadyEvent;

	private Vector3 visiblePos;

	private Vector3 hiddenPos;

	private float uiDuration;

	private bool isUiOn;

	private static bool questsThinkTheyreOn;

	private static bool questsShown;

	private bool hidable;

	private Action postHideCallback;

	private bool didSetupTweeningParams;

	private Interpolator uiInterpolator;

	private SortedDictionary<uint, SBGUIButton> questButtons;

	private List<GoodWidgetTransfer> goodWidgetTransfers;

	private List<GoodWidgetTransfer> goodWidgetTransferCorpses;

	private float HideQuestsAnimDuration;

	private float questInterp;

	public SBGUILabel QuestCountLabel
	{
		get
		{
			return null;
		}
	}

	public int HelpshiftNotificationCount
	{
		set
		{
		}
	}

	public override bool UsedInSessionAction
	{
		set
		{
		}
	}

	public int GetQuestButtonCount
	{
		get
		{
			return 0;
		}
	}

	protected override void Awake()
	{
	}

	protected override void OnDisable()
	{
	}

	public void Initialize(Session session)
	{
	}

	public void SetInventoryWidgetDraggingCallbacks(Action<int, YGEvent> startDragoutCallback, Action<YGEvent> dragThroughHandler)
	{
	}

	public void RefreshFromCache()
	{
	}

	public void DisableInactiveElements()
	{
	}

	public override void Update()
	{
	}

	private void UpdateGoodDeliveries()
	{
	}

	private void ResetUIVisibleDuration()
	{
	}

	private void ChildInventoryGotEvent(YGEvent evt, int? productId, Session session)
	{
	}

	public bool ShowInventoryWidget()
	{
		return false;
	}

	public void CloseInventoryWidget()
	{
	}

	public void TryPulseResourceError(int resourceId)
	{
	}

	public void DeliverGood(GoodToSimulatedDeliveryRequest goodDelivery)
	{
	}

	public void ReturnGood(GoodReturnRequest goodReturn)
	{
	}

	public void ToggleQuestTracker(Session session, bool bForce = false, bool bIsButton = false)
	{
	}

	public bool EnableQuestTracker(bool enable, Session session, bool bForce = false)
	{
		return false;
	}

	public void EnableUI(bool enable)
	{
	}

	public void EnableFullHiding(bool enabled)
	{
	}

	public void SetPatchyHudIconVisible()
	{
	}

	public void SetVisibleNonEssentialElements(bool visible)
	{
	}

	public void SetVisibleNonEssentialElements(bool visible, bool alsoHideGrubWidget)
	{
	}

	public void HideAllElements()
	{
	}

	public void HideCurrencies()
	{
	}

	public void ShowCurrencies()
	{
	}

	public void HideElementsForEditMode(bool editMode)
	{
	}

	public void SoftCurrencyBarAnimatedPulse()
	{
	}

	public void HardCurrencyBarAnimatedPulse()
	{
	}

	public void XPBarStarAnimatedPulse()
	{
	}

	[DebuggerHidden]
	private IEnumerator HideQuestsCoroutine()
	{
		return null;
	}

	private void HideQuestsCoroutineFinish()
	{
	}

	[DebuggerHidden]
	private IEnumerator ShowQuestsCoroutine()
	{
		return null;
	}

	private void ShowQuestsCoroutineFinish()
	{
	}

	private void InterpolateQuestButtons(float interp, float delay, Func<float, float, float, float> easeFn)
	{
	}

	public override void Close()
	{
	}

	public override void Deactivate()
	{
	}

	protected override void OnEnable()
	{
	}

	public void CalculateTweeningParams()
	{
	}

	[DebuggerHidden]
	private IEnumerator HideUICoroutine()
	{
		return null;
	}

	[DebuggerHidden]
	private IEnumerator ShowUICoroutine()
	{
		return null;
	}

	private SBGUIButton AddQuestTracker(uint did, string texture, Action clickHandler)
	{
		return null;
	}

	private void RemoveQuestTracker(uint did)
	{
	}

	public void RemoveQuestTrackers()
	{
	}

	public void RefreshQuestTrackers(Session session)
	{
	}

	public void HideZeroQuestCount()
	{
	}

	public void ShowHelpshiftNotification()
	{
	}

	private Action HandleClick(Session session, int did)
	{
		return null;
	}

	public void TryFireQuestStatusEvent(Session session, int did)
	{
	}

	private void DeactivateQuestScrollButtons()
	{
	}

	private void DeactivatePatchyUI()
	{
	}

	private void DeactivateNonPatchyUI()
	{
	}

	public bool IsQuestTrackerVisible()
	{
		return false;
	}

	private void ExamineQuest(int questDid)
	{
	}

	private bool ShouldCommunityShow()
	{
		return false;
	}

	private void ResetCommunityImage(bool reset)
	{
	}
}
