#define ASSERTS_ON
using System;
using System.Collections;
using System.Collections.Generic;
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
			this.gameObject = gameObject;
			this.origin = origin;
			this.target = target;
		}
	}

	private class Interpolator
	{
		private int locks;

		public bool IsLocked
		{
			get
			{
				return locks > 0;
			}
		}

		public void Lock()
		{
			locks++;
		}

		public void Unlock()
		{
			locks--;
		}

		public void UpdateUIEasing(Dictionary<string, Positioning> elementPositionings, float interp, Func<float, float, float, float> easingMethod)
		{
			foreach (string key in elementPositionings.Keys)
			{
				TFUtils.Assert(elementPositionings[key].gameObject != null, "Got a null GameObject for element=" + key);
				elementPositionings[key].gameObject.transform.position = Easing.Vector3Easing(elementPositionings[key].origin, elementPositionings[key].target, interp, easingMethod);
			}
		}
	}

	public const string ORIGINAL_DRAG_EVENT = "OriginalDragEvent";

	public const string LAST_TOUCHED_PRODUCT = "LastStandardHudTouchedProduct";

	private const int QUEST_GAP = 12;

	private const float DISPLAY_UI_TIMEOUT = 30f;

	public Vector2 FoodDeliverySize = Vector2.one;

	public static bool userClosedWishList;

	public EventDispatcher<int> QuestStatusEvent = new EventDispatcher<int>();

	private Dictionary<string, Positioning> elementPositionings = new Dictionary<string, Positioning>();

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

	private bool questMarkersDoneAnimating = true;

	public SBGUIInventoryHudWidget inventory;

	public ReadyEventDispatcher ReadyEvent = new ReadyEventDispatcher();

	private Vector3 visiblePos;

	private Vector3 hiddenPos;

	private float uiDuration = 30f;

	private bool isUiOn = true;

	private static bool questsThinkTheyreOn = true;

	private static bool questsShown;

	private bool hidable = true;

	private Action postHideCallback;

	private bool didSetupTweeningParams;

	private Interpolator uiInterpolator = new Interpolator();

	private SortedDictionary<uint, SBGUIButton> questButtons = new SortedDictionary<uint, SBGUIButton>();

	private List<GoodWidgetTransfer> goodWidgetTransfers = new List<GoodWidgetTransfer>();

	private List<GoodWidgetTransfer> goodWidgetTransferCorpses = new List<GoodWidgetTransfer>();

	private float HideQuestsAnimDuration = 0.2f;

	private float questInterp;

	public SBGUILabel QuestCountLabel
	{
		get
		{
			return questCountLabel;
		}
	}

	public int HelpshiftNotificationCount
	{
		set
		{
			helpshiftNotificationCount = value;
		}
	}

	public override bool UsedInSessionAction
	{
		set
		{
			base.UsedInSessionAction = value;
			EnableFullHiding(!UsedInSessionAction);
		}
	}

	public int GetQuestButtonCount
	{
		get
		{
			return questButtons.Count;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		TFUtils.Assert(FoodDeliverySize != Vector2.one, "You must change the default size for FoodDeliverySize on standard screen!");
		int childCount = base.gameObject.transform.childCount;
		nativeElements = new List<GameObject>(childCount);
		for (int i = 0; i < childCount; i++)
		{
			GameObject item = base.gameObject.transform.GetChild(i).gameObject;
			nativeElements.Add(item);
		}
		softCurrencyBar = (SBGUIPulseImage)FindChild("money_bar");
		softCurrencyBar.InitializePulser(softCurrencyBar.Size, 1.5f, 0.2f);
		softCurrencyIcon = (SBGUIPulseImage)FindChild("coin_icon");
		softCurrencyIcon.InitializePulser(softCurrencyIcon.Size, 2f, 0.2f);
		hardCurrencyBar = (SBGUIPulseImage)FindChild("jj_bar");
		hardCurrencyBar.InitializePulser(hardCurrencyBar.Size, 1.5f, 0.2f);
		hardCurrencyIcon = (SBGUIPulseImage)FindChild("jj_icon");
		hardCurrencyIcon.InitializePulser(hardCurrencyIcon.Size, 2f, 0.2f);
		xpBar = FindChild("happyface");
		xpBarStar = (SBGUIPulseImage)FindChild("levelup_bar_star");
		xpBarStar.InitializePulser(xpBarStar.Size, 2f, 0.2f);
		questMarker = FindChild("quest_marker");
		questsOrigin = (SBGUIButton)FindChild("quest");
		questCountIcon = FindChild("red_button");
		inventory = (SBGUIInventoryHudWidget)FindChild("inventory_widget");
		settingsHudIcon = FindChild("settings");
		settingsHudCountIcon = FindChild("red_counter");
		settingsHudCountLabel = (SBGUILabel)FindChild("red_counter_label");
		editModeHudIcon = FindChild("edit");
		inventoryHudIcon = FindChild("inventory");
		marketplaceHudIcon = FindChild("marketplace");
		communityEventHudIcon = FindChild("community_event");
		patchyHudTitleIcon = FindChild("patchy_title_icon");
		patchyHudTitleBg = FindChild("patchy_title_bg");
		patchyHudTitleLabel = FindChild("patchy_title_label");
		patchyHudIcon = FindChild("patchy");
		happyfaceHud = FindChild("happyface");
		jjBarHud = FindChild("jj_bar");
		moneyBarHud = FindChild("money_bar");
		specialBarHud = FindChild("special_bar");
		pathEditToggle = FindChild("editpath_toggle").gameObject.GetComponent<SBGUIAtlasButton>();
		questScrollUp = (SBGUIPulseButton)FindChild("quest_scroll_up");
		questScrollDown = (SBGUIPulseButton)FindChild("quest_scroll_down");
		questCountLabel = (SBGUILabel)FindChild("quest_count");
		TFUtils.Assert(questMarker != null, "Couldn't find the quest marker!");
		TFUtils.Assert(questsOrigin != null, "Couldn't find the quest HUD icon!");
		TFUtils.Assert(inventory != null, "Couldn't find the inventory HUD widget!");
		TFUtils.Assert(settingsHudIcon != null, "Couldn't find the settings HUD button!");
		TFUtils.Assert(editModeHudIcon != null, "Couldn't find the edit mode HUD button!");
		TFUtils.Assert(inventoryHudIcon != null, "Couldn't find the inventory HUD button!");
		TFUtils.Assert(marketplaceHudIcon != null, "Couldn't find the marketplace HUD button!");
		TFUtils.Assert(communityEventHudIcon != null, "Couldn't find the community event HUD button!");
		TFUtils.Assert(patchyHudTitleIcon != null, "Couldn't find the patchy Hud title icon!");
		TFUtils.Assert(patchyHudTitleBg != null, "Couldn't find the patchy HUD title background!");
		TFUtils.Assert(patchyHudTitleLabel != null, "Couldn't find the patchy HUD title label!");
		TFUtils.Assert(patchyHudIcon != null, "Couldn't find the patchy HUD button!");
		TFUtils.Assert(questScrollUp != null, "Couldn't find the Quest Scroll up button!");
		TFUtils.Assert(questScrollDown != null, "Couldn't find the Quest Scroll down button!");
		TFUtils.Assert(pathEditToggle != null, "Couldn't find the edit/path toggle HUD button!");
		QuestStatusEvent.AddListener(ExamineQuest);
	}

	protected override void OnDisable()
	{
		if (!questMarkersDoneAnimating)
		{
			if (questsShown)
			{
				HideQuestsCoroutineFinish();
			}
			else
			{
				ShowQuestsCoroutineFinish();
			}
		}
		base.OnDisable();
	}

	public void Initialize(Session session)
	{
		TFUtils.Assert(inventory != null, "Inventory cannot be null!");
		TFUtils.Assert(session.TheGame != null, "Game cannot be null!");
		Action<YGEvent, int?> interactCallback = delegate(YGEvent evt, int? productId)
		{
			ChildInventoryGotEvent(evt, productId, session);
			session.AddAsyncResponse("ResetSimulationDrag", true, false);
		};
		inventory.Setup(session.TheGame, session.TheGame.craftManager, session.TheGame.vendingManager, session.TheGame.resourceManager, session.TheSoundEffectManager, interactCallback, marketplaceHudIcon.GetScreenPosition().y - 60f * GUIView.ResolutionScaleFactor());
		inventory.CloseRows();
		DeactivateQuestScrollButtons();
		ToggleQuestTracker(session, true);
		ReadyEvent.FireEvent();
		if (ShouldCommunityShow())
		{
			CommunityEvent activeEvent = session.TheGame.communityEventManager.GetActiveEvent();
			SBGUIAtlasImage component = communityEventHudIcon.GetComponent<SBGUIAtlasImage>();
			if (component != null)
			{
				component.SetTextureFromAtlas(activeEvent.m_sEventButtonTexture);
			}
			ResetCommunityImage(true);
		}
		else
		{
			SBGUIAtlasImage component2 = communityEventHudIcon.GetComponent<SBGUIAtlasImage>();
			if (component2 != null)
			{
				component2.SetTextureFromAtlas("_blank_.png");
			}
			ResetCommunityImage(false);
		}
	}

	public void SetInventoryWidgetDraggingCallbacks(Action<int, YGEvent> startDragoutCallback, Action<YGEvent> dragThroughHandler)
	{
		inventory.StartDragOutCallback = startDragoutCallback;
		inventory.DragThroughCallback = dragThroughHandler;
	}

	public void RefreshFromCache()
	{
		ResetCommunityImage(ShouldCommunityShow());
		DeactivateQuestScrollButtons();
		DeactivatePatchyUI();
	}

	public void DisableInactiveElements()
	{
		if (!ShouldCommunityShow())
		{
			ResetCommunityImage(false);
		}
	}

	public override void Update()
	{
		if (session == null || session.TheGame == null)
		{
			return;
		}
		base.Update();
		if (!didSetupTweeningParams)
		{
			CalculateTweeningParams();
		}
		uiDuration -= Time.deltaTime;
		if (hidable && uiDuration < 0f && isUiOn)
		{
			StartCoroutine(HideUICoroutine());
		}
		inventory.OnUpdate(session.TheGame.resourceManager);
		UpdateGoodDeliveries();
		if (((int?)session.CheckAsyncRequest(ResourceDrop.MakeResourceKey(ResourceManager.XP))).HasValue)
		{
			XPBarStarAnimatedPulse();
		}
		if (((int?)session.CheckAsyncRequest(ResourceDrop.MakeResourceKey(ResourceManager.SOFT_CURRENCY))).HasValue)
		{
			SoftCurrencyBarAnimatedPulse();
		}
		if (((int?)session.CheckAsyncRequest(ResourceDrop.MakeResourceKey(ResourceManager.HARD_CURRENCY))).HasValue)
		{
			HardCurrencyBarAnimatedPulse();
		}
		if (!questMarkersDoneAnimating || !IsQuestTrackerVisible())
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		float? num = questScrollUpperBound;
		if (!num.HasValue)
		{
			questScrollUpperBound = questScrollUp.GetScreenPosition().y + (float)questScrollUp.Height * 0.01f;
		}
		float? num2 = questScrollLowerBound;
		if (!num2.HasValue)
		{
			questScrollLowerBound = questScrollDown.GetScreenPosition().y + (float)questScrollDown.Height * 0.01f;
		}
		foreach (SBGUIQuestTrackerSlot value2 in questButtons.Values)
		{
			float? num3 = questScrollUpperBound;
			float value = num3.Value;
			float? num4 = questScrollLowerBound;
			switch (value2.OnUpdate(value, num4.Value))
			{
			case SBGUIQuestTrackerSlot.QuestTrackerState.AboveBounds:
				flag = true;
				break;
			case SBGUIQuestTrackerSlot.QuestTrackerState.BelowBounds:
				flag2 = true;
				break;
			}
		}
		if (flag && !questScrollUp.IsActive())
		{
			questScrollUp.SetActive(true);
		}
		else if (!flag && questScrollUp.IsActive())
		{
			questScrollUp.SetActive(false);
		}
		if (flag2 && !questScrollDown.IsActive())
		{
			questScrollDown.SetActive(true);
		}
		else if (!flag2 && questScrollDown.IsActive())
		{
			questScrollDown.SetActive(false);
		}
		if (!session.InFriendsGame)
		{
			if (inventory.IsActive() && session.marketpalceActive)
			{
				inventory.SetActive(false);
			}
			else if (!inventory.IsActive() && !session.marketpalceActive)
			{
				inventory.SetActive(true);
			}
		}
	}

	private void UpdateGoodDeliveries()
	{
		foreach (GoodWidgetTransfer goodWidgetTransfer in goodWidgetTransfers)
		{
			Vector2 targetScreenPosition = goodWidgetTransfer.GetTargetScreenPosition(session, inventory.GetScreenPosition());
			Vector2 screenPosition = goodWidgetTransfer.icon.GetScreenPosition();
			Vector2 vector = targetScreenPosition - screenPosition;
			float num = vector.SqrMagnitude();
			vector.Normalize();
			vector *= goodWidgetTransfer.speed;
			goodWidgetTransfer.icon.SetScreenPosition(screenPosition + vector);
			float num2 = goodWidgetTransfer.speed * goodWidgetTransfer.speed * 2f;
			if (num <= num2)
			{
				goodWidgetTransferCorpses.Add(goodWidgetTransfer);
			}
			inventory.IncrementDeductionsForTick(goodWidgetTransfer.goodId);
			ResetUIVisibleDuration();
		}
		foreach (GoodWidgetTransfer goodWidgetTransferCorpse in goodWidgetTransferCorpses)
		{
			goodWidgetTransfers.Remove(goodWidgetTransferCorpse);
			goodWidgetTransferCorpse.icon.Destroy();
		}
		goodWidgetTransferCorpses.Clear();
	}

	private void ResetUIVisibleDuration()
	{
		uiDuration = 30f;
	}

	private void ChildInventoryGotEvent(YGEvent evt, int? productId, Session session)
	{
		if (evt != null)
		{
			if (evt.type == YGEvent.TYPE.TOUCH_BEGIN)
			{
				session.AddAsyncResponse("OriginalDragEvent", evt, false);
				session.TheCamera.SetEnableUserInput(false);
			}
			else if (evt.type == YGEvent.TYPE.TOUCH_MOVE)
			{
				session.TheCamera.SetEnableUserInput(false);
			}
			else
			{
				session.CheckAsyncRequest("OriginalDragEvent");
				session.TheCamera.SetEnableUserInput(true);
			}
		}
		if (productId.HasValue)
		{
			session.AddAsyncResponse("LastStandardHudTouchedProduct", productId.Value, false);
		}
		ResetUIVisibleDuration();
	}

	public bool ShowInventoryWidget()
	{
		inventory.SetActive(true);
		return inventory.ActivateAllTabs();
	}

	public void CloseInventoryWidget()
	{
		inventory.CloseRows();
	}

	public void TryPulseResourceError(int resourceId)
	{
		inventory.TryPulseResourceError(resourceId);
	}

	public void DeliverGood(GoodToSimulatedDeliveryRequest goodDelivery)
	{
		SBGUIPulseImage sBGUIPulseImage = SBGUIPulseImage.Create(this, goodDelivery.materialName, FoodDeliverySize, 2f, 0.5f, null);
		sBGUIPulseImage.name = "HudDeliveryIcon_" + sBGUIPulseImage.name;
		sBGUIPulseImage.SetScreenPosition(goodDelivery.GetOriginalScreenPosition(session, inventory.GetScreenPosition()));
		sBGUIPulseImage.Pulser.PulseOneShot();
		goodDelivery.icon = sBGUIPulseImage;
		goodWidgetTransfers.Add(goodDelivery);
	}

	public void ReturnGood(GoodReturnRequest goodReturn)
	{
		SBGUIPulseImage sBGUIPulseImage = SBGUIPulseImage.Create(this, goodReturn.materialName, FoodDeliverySize, 2f, 0.2f, null);
		sBGUIPulseImage.name = "HudReturnIcon_" + sBGUIPulseImage.name;
		sBGUIPulseImage.SetScreenPosition(goodReturn.GetOriginalScreenPosition(session, inventory.GetScreenPosition()));
		sBGUIPulseImage.Pulser.PulseStartLoop();
		goodReturn.icon = sBGUIPulseImage;
		goodWidgetTransfers.Add(goodReturn);
	}

	public void ToggleQuestTracker(Session session, bool bForce = false, bool bIsButton = false)
	{
		if (bForce)
		{
			questsThinkTheyreOn = bForce;
			EnableQuestTracker(questsThinkTheyreOn, session, bForce);
			ResetUIVisibleDuration();
			return;
		}
		if (questsThinkTheyreOn)
		{
			session.TheSoundEffectManager.PlaySound("CloseQuestList");
			if (bIsButton)
			{
				AnalyticsWrapper.LogUIInteraction(session.TheGame, "ui_hide_quests", "button", "tap");
			}
		}
		else
		{
			session.TheSoundEffectManager.PlaySound("OpenQuestList");
			if (bIsButton)
			{
				AnalyticsWrapper.LogUIInteraction(session.TheGame, "ui_display_quests", "button", "tap");
			}
		}
		questsThinkTheyreOn = !questsThinkTheyreOn;
		EnableQuestTracker(questsThinkTheyreOn, session);
		ResetUIVisibleDuration();
	}

	public bool EnableQuestTracker(bool enable, Session session, bool bForce = false)
	{
		if (enable)
		{
			if ((!questsShown || bForce) && IsActive())
			{
				StartCoroutine(ShowQuestsCoroutine());
				return true;
			}
		}
		else if ((questsShown || bForce) && IsActive())
		{
			StartCoroutine(HideQuestsCoroutine());
			return true;
		}
		return false;
	}

	public void EnableUI(bool enable)
	{
		if (enable)
		{
			ResetUIVisibleDuration();
			if (!isUiOn)
			{
				base.gameObject.SetActive(true);
				StartCoroutine(ShowUICoroutine());
			}
		}
		else if (isUiOn)
		{
			StartCoroutine(HideUICoroutine());
		}
	}

	public void EnableFullHiding(bool enabled)
	{
		if (hidable != enabled)
		{
			hidable = enabled;
			if (!hidable && !isUiOn)
			{
				base.gameObject.active = true;
				StartCoroutine(ShowUICoroutine());
			}
			else if (hidable)
			{
				ResetUIVisibleDuration();
			}
		}
	}

	public void SetPatchyHudIconVisible()
	{
		if (session == null || session.TheGame == null)
		{
			return;
		}
		QuestManager questManager = session.TheGame.questManager;
		if (questManager == null)
		{
			return;
		}
		if (patchyHudIcon == null)
		{
			TFUtils.ErrorLog("Patchy Button Does Not Yet Exist");
			return;
		}
		try
		{
			if ((string.Compare(session.TheState.ToString(), "Session+Playing") == 0 || string.Compare(session.TheState.ToString(), "Session+DragFeeding") == 0) && !SBSettings.DisableFriendPark)
			{
				if (questManager.IsQuestActive(2400u))
				{
					patchyHudIcon.SetActive(true);
				}
				else
				{
					patchyHudIcon.SetActive(questManager.IsQuestCompleted(2400u));
				}
			}
			else
			{
				patchyHudIcon.SetActive(false);
			}
		}
		catch (Exception ex)
		{
			TFUtils.ErrorLog("SetPatchyHudIconVisible: " + ex.Message + "\n" + ex.StackTrace);
		}
	}

	public void SetVisibleNonEssentialElements(bool visible)
	{
		SetVisibleNonEssentialElements(visible, false);
	}

	public void SetVisibleNonEssentialElements(bool visible, bool alsoHideGrubWidget)
	{
		settingsHudIcon.SetActive(visible);
		ShowHelpshiftNotification();
		editModeHudIcon.SetActive(visible);
		pathEditToggle.SetActive(visible);
		inventoryHudIcon.SetActive(visible);
		marketplaceHudIcon.SetActive(visible);
		questsOrigin.SetActive(visible);
		questScrollDown.SetActive(visible);
		questScrollUp.SetActive(visible);
		if (session.InFriendsGame)
		{
			patchyHudTitleIcon.SetActive(visible);
			patchyHudTitleBg.SetActive(visible);
			patchyHudTitleLabel.SetActive(visible);
		}
		else
		{
			patchyHudTitleIcon.SetActive(false);
			patchyHudTitleBg.SetActive(false);
			patchyHudTitleLabel.SetActive(false);
		}
		patchyHudIcon.SetActive(visible && !SBSettings.DisableFriendPark);
		if (session.TheGame.communityEventManager.GetActiveEvent() != null)
		{
			ResetCommunityImage(ShouldCommunityShow() && visible);
		}
		else
		{
			ResetCommunityImage(false);
		}
		if (!visible)
		{
			if (alsoHideGrubWidget)
			{
				inventory.SetActive(false);
			}
			EnableQuestTracker(false, session);
		}
		else
		{
			inventory.SetActive(true);
			if (questsThinkTheyreOn)
			{
				EnableQuestTracker(true, session);
			}
		}
	}

	public void HideAllElements()
	{
		SetVisibleNonEssentialElements(false, true);
		HideCurrencies();
	}

	public void HideCurrencies()
	{
		jjBarHud.SetActive(false);
		moneyBarHud.SetActive(false);
		specialBarHud.SetActive(false);
		xpBar.SetActive(false);
	}

	public void ShowCurrencies()
	{
		jjBarHud.SetActive(true);
		moneyBarHud.SetActive(true);
		specialBarHud.SetActive(true);
		xpBar.SetActive(true);
	}

	public void HideElementsForEditMode(bool editMode)
	{
		if (editMode)
		{
			EnableQuestTracker(false, session);
			xpBar.SetActive(false);
			inventory.SetActive(false);
			questsOrigin.transform.localPosition = new Vector3(questsOrigin.transform.localPosition.x, questsOrigin.transform.localPosition.y, 100f);
			questsOrigin.MuteButtons(true);
			questsOrigin.EnableButtons(false);
			return;
		}
		if (session.TheState.GetType().Equals(typeof(Session.Playing)) && questsThinkTheyreOn)
		{
			EnableQuestTracker(true, session);
		}
		xpBar.SetActive(true);
		inventory.SetActive(true);
		questsOrigin.transform.localPosition = new Vector3(questsOrigin.transform.localPosition.x, questsOrigin.transform.localPosition.y, 1f);
		questsOrigin.MuteButtons(false);
		questsOrigin.EnableButtons(true);
	}

	public void SoftCurrencyBarAnimatedPulse()
	{
		softCurrencyBar.Pulser.PulseOneShot();
		softCurrencyIcon.Pulser.PulseOneShot();
	}

	public void HardCurrencyBarAnimatedPulse()
	{
		hardCurrencyBar.Pulser.PulseOneShot();
		hardCurrencyIcon.Pulser.PulseOneShot();
	}

	public void XPBarStarAnimatedPulse()
	{
		xpBarStar.Pulser.PulseOneShot();
	}

	private IEnumerator HideQuestsCoroutine()
	{
		questsShown = false;
		float questInterp = 0f;
		questMarkersDoneAnimating = false;
		DeactivateQuestScrollButtons();
		DeactivatePatchyUI();
		questMarker.EnableButtons(false);
		questScrollDown.EnableButtons(false);
		questScrollUp.EnableButtons(false);
		questsOrigin.EnableButtons(false);
		while (questInterp <= 1f)
		{
			questInterp += Time.deltaTime / HideQuestsAnimDuration;
			InterpolateQuestButtons(1f - questInterp, 0f, Easing.EaseInCirc);
			yield return null;
		}
		HideQuestsCoroutineFinish();
	}

	private void HideQuestsCoroutineFinish()
	{
		InterpolateQuestButtons(0f, 0f, Easing.Linear);
		foreach (Transform item in base.transform.Find("upperleft/quest_marker"))
		{
			item.GetComponent<Renderer>().enabled = false;
			item.Find("questbackground").GetComponent<Renderer>().enabled = false;
		}
		questMarker.EnableButtons(true);
		questScrollDown.EnableButtons(true);
		questScrollUp.EnableButtons(true);
		questsOrigin.EnableButtons(true);
		questMarkersDoneAnimating = true;
		DeactivateQuestScrollButtons();
	}

	private IEnumerator ShowQuestsCoroutine()
	{
		questsShown = true;
		bool buttonsOn = false;
		foreach (Transform child in base.transform.Find("upperleft/quest_marker"))
		{
			child.GetComponent<Renderer>().enabled = true;
			child.Find("questbackground").GetComponent<Renderer>().enabled = true;
		}
		questMarkersDoneAnimating = false;
		questMarker.EnableButtons(buttonsOn);
		float offset = (questInterp = (float)(questButtons.Keys.Count - 1) * 0.05f * -1f);
		while (questInterp <= 1f)
		{
			questInterp += Time.deltaTime / 0.8f;
			InterpolateQuestButtons(questInterp + offset, 0.05f, Easing.EaseOutElastic);
			ResetUIVisibleDuration();
			if (!buttonsOn && questInterp > 0.66f)
			{
				buttonsOn = true;
				questMarker.EnableButtons(buttonsOn);
			}
			yield return null;
		}
		ShowQuestsCoroutineFinish();
	}

	private void ShowQuestsCoroutineFinish()
	{
		InterpolateQuestButtons(1f, 0f, Easing.Linear);
		questMarkersDoneAnimating = true;
		questMarker.EnableButtons(true);
	}

	private void InterpolateQuestButtons(float interp, float delay, Func<float, float, float, float> easeFn)
	{
		Vector3 vector = new Vector3(0f, -0.7f, 0f);
		Vector3 zero = Vector3.zero;
		float num = 0f;
		Vector3 start = questsOrigin.WorldPosition + new Vector3(0f, 0f, 0.2f);
		foreach (SBGUIButton value in questButtons.Values)
		{
			value.WorldPosition = Easing.Vector3Easing(start, questMarker.WorldPosition + zero, Mathf.Clamp01(interp + num), easeFn);
			zero += vector;
			num += delay;
		}
	}

	public override void Close()
	{
		postHideCallback = base.Close;
		hidable = true;
		if (isUiOn)
		{
			inventory.CloseRows();
			SetActive(false);
			isUiOn = false;
		}
		base.Close();
	}

	public override void Deactivate()
	{
		hidable = true;
		if (isUiOn)
		{
			if (base.gameObject.activeSelf)
			{
				StartCoroutine(HideUICoroutine());
			}
		}
		else
		{
			foreach (Transform item in base.transform)
			{
				if (item.name != "upperleft")
				{
					item.gameObject.SetActive(false);
				}
			}
		}
		foreach (Transform item2 in base.transform.Find("upperleft/quest_marker"))
		{
			item2.GetComponent<Renderer>().enabled = false;
			item2.Find("questbackground").GetComponent<Renderer>().enabled = false;
		}
	}

	protected override void OnEnable()
	{
		EnableUI(true);
		base.OnEnable();
	}

	public void CalculateTweeningParams()
	{
		TFUtils.Assert(!didSetupTweeningParams, "Should only setup the tweening params once.");
		Vector3 zero = Vector3.zero;
		foreach (GameObject nativeElement in nativeElements)
		{
			zero += nativeElement.transform.position;
		}
		zero /= (float)nativeElements.Count;
		elementPositionings.Clear();
		foreach (GameObject nativeElement2 in nativeElements)
		{
			Vector3 position = nativeElement2.transform.position;
			Vector3 target = nativeElement2.transform.position - zero;
			target.Normalize();
			target *= 2f;
			target += position;
			elementPositionings.Add(nativeElement2.name, new Positioning(nativeElement2, position, target));
		}
		didSetupTweeningParams = true;
	}

	private IEnumerator HideUICoroutine()
	{
		if (!isUiOn)
		{
			yield break;
		}
		while (uiInterpolator.IsLocked)
		{
			yield return null;
		}
		if (!questMarkersDoneAnimating)
		{
			questInterp = 1f;
			yield return null;
		}
		uiInterpolator.Lock();
		if (isUiOn)
		{
			isUiOn = false;
			inventory.CloseRows();
			DeactivatePatchyUI();
			float interp = 0f;
			while (interp <= 1f)
			{
				interp += Time.deltaTime / 0.3f;
				uiInterpolator.UpdateUIEasing(elementPositionings, interp, Easing.EaseInBack);
				yield return null;
			}
			foreach (Transform child in base.transform)
			{
				if (child.name != "upperleft")
				{
					child.gameObject.SetActive(false);
				}
			}
			foreach (Transform child2 in base.transform.Find("upperleft/quest_marker"))
			{
				child2.GetComponent<Renderer>().enabled = false;
				child2.Find("questbackground").GetComponent<Renderer>().enabled = false;
			}
			isUiOn = false;
			if (postHideCallback != null)
			{
				postHideCallback();
			}
		}
		uiInterpolator.Unlock();
	}

	private IEnumerator ShowUICoroutine()
	{
		if (isUiOn)
		{
			yield break;
		}
		while (uiInterpolator.IsLocked)
		{
			yield return null;
		}
		if (!questMarkersDoneAnimating)
		{
			questInterp = 1f;
			yield return null;
		}
		uiInterpolator.Lock();
		if (!isUiOn)
		{
			SetActive(true);
			HideZeroQuestCount();
			ShowHelpshiftNotification();
			foreach (Transform child in base.transform.Find("upperleft/quest_marker"))
			{
				child.GetComponent<Renderer>().enabled = true;
				child.Find("questbackground").GetComponent<Renderer>().enabled = true;
			}
			SBGUIAtlasImage editModeFishPerson = (SBGUIAtlasImage)FindChild("edit_mode_fish_person");
			if (editModeFishPerson.IsActive())
			{
				editModeFishPerson.SetTextureFromAtlas("EditMode_FishPerson", true, false, true);
			}
			ResetCommunityImage(ShouldCommunityShow());
			EnableButtons(false);
			isUiOn = true;
			DeactivateQuestScrollButtons();
			if (!session.InFriendsGame)
			{
				DeactivatePatchyUI();
			}
			else
			{
				DeactivateNonPatchyUI();
			}
			if (!userClosedWishList)
			{
				inventory.ActivateAllTabs();
			}
			else
			{
				inventory.CloseRows();
			}
			float interp = 1f;
			while (interp > 0f)
			{
				interp -= Time.deltaTime / 0.2f;
				interp = Mathf.Max(0f, interp);
				uiInterpolator.UpdateUIEasing(elementPositionings, interp, Easing.EaseInBack);
				yield return null;
			}
			uiInterpolator.UpdateUIEasing(elementPositionings, 0f, Easing.Linear);
			if (!userClosedWishList)
			{
				inventory.ActivateAllTabs();
			}
			else
			{
				inventory.CloseRows();
			}
			EnableButtons(true);
			ResetCommunityImage(ShouldCommunityShow());
			isUiOn = true;
		}
		ReregisterColliders();
		uiInterpolator.Unlock();
	}

	private SBGUIButton AddQuestTracker(uint did, string texture, Action clickHandler)
	{
		if (session.TheGame.questManager.IsQuestActivated(40000u))
		{
			return null;
		}
		TFUtils.Assert(questMarker != null, "Must find reference for questMarker first!");
		if (!string.Equals(texture, "n/a"))
		{
			SBGUIQuestTrackerSlot sBGUIQuestTrackerSlot = (SBGUIQuestTrackerSlot)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/QuestIcon");
			sBGUIQuestTrackerSlot.SetParent(questMarker);
			sBGUIQuestTrackerSlot.SessionActionId = QuestDefinition.GenerateSessionActionId(did);
			int scalePixel = (int)sBGUIQuestTrackerSlot.Size.x;
			sBGUIQuestTrackerSlot.SetTextureFromAtlas(texture, true, false, true, scalePixel);
			AttachAnalyticsToButton("questIcon", sBGUIQuestTrackerSlot);
			sBGUIQuestTrackerSlot.ClickEvent += clickHandler;
			if (questButtons.ContainsKey(did))
			{
				RemoveQuestTracker(did);
			}
			questButtons.Add(did, sBGUIQuestTrackerSlot);
			ToggleQuestTracker(session, true);
			return sBGUIQuestTrackerSlot;
		}
		return null;
	}

	private void RemoveQuestTracker(uint did)
	{
		SBGUIButton value;
		if (questButtons.TryGetValue(did, out value))
		{
			value.SetParent(null);
			UnityEngine.Object.Destroy(value.gameObject);
			questButtons.Remove(did);
			if (questsThinkTheyreOn)
			{
				EnableQuestTracker(true, session, true);
			}
		}
	}

	public void RemoveQuestTrackers()
	{
		List<uint> list = new List<uint>();
		list.AddRange(questButtons.Keys);
		foreach (uint item in list)
		{
			RemoveQuestTracker(item);
		}
	}

	public void RefreshQuestTrackers(Session session)
	{
		List<uint> list = new List<uint>();
		List<uint> list2 = new List<uint>();
		list2.AddRange(questButtons.Keys);
		foreach (uint item in list2)
		{
			if (session.TheGame.questManager.IsQuestActive(item))
			{
				list.Add(item);
			}
			else
			{
				RemoveQuestTracker(item);
			}
		}
		uint questDid;
		foreach (uint item2 in session.TheGame.questManager.ActiveQuestDidsNotInPostponed)
		{
			questDid = item2;
			if (!list.Exists((uint did) => did == questDid))
			{
				Action clickHandler = HandleClick(session, (int)questDid);
				QuestDefinition questDefinition = session.TheGame.questManager.GetQuestDefinition(questDid);
				if (!questDefinition.Icon.Equals(string.Empty))
				{
					AddQuestTracker(questDefinition.Did, questDefinition.Icon, clickHandler);
				}
			}
		}
		if (CommunityEventManager._pEventStatusDialogData == null)
		{
			return;
		}
		CommunityEvent activeEvent = session.TheGame.communityEventManager.GetActiveEvent();
		if (activeEvent == null)
		{
			return;
		}
		Action clickHandler2 = delegate
		{
			if (CommunityEventManager._pEventStatusDialogData != null)
			{
				List<DialogInputData> inputs = new List<DialogInputData>
				{
					new SpongyGamesDialogInputData(600001u, CommunityEventManager._pEventStatusDialogData)
				};
				if (session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, inputs, 600001u))
				{
					session.TheGame.communityEventManager.DialogNeeded();
				}
			}
			else
			{
				RemoveQuestTracker(600001u);
			}
		};
		AddQuestTracker(600001u, activeEvent.m_sQuestIcon, clickHandler2);
	}

	public void HideZeroQuestCount()
	{
		if (questButtons.Count == 0)
		{
			questCountIcon.SetActive(false);
		}
	}

	public void ShowHelpshiftNotification()
	{
		if (settingsHudCountIcon != null && settingsHudIcon.IsActive())
		{
			if (helpshiftNotificationCount > 0)
			{
				settingsHudCountIcon.SetActive(true);
			}
			else
			{
				settingsHudCountIcon.SetActive(false);
			}
			settingsHudCountLabel.SetText(helpshiftNotificationCount.ToString());
		}
	}

	private Action HandleClick(Session session, int did)
	{
		return delegate
		{
			if (session.TheState.GetType().Equals(typeof(Session.Playing)))
			{
				QuestStatusEvent.FireEvent(did);
				TFUtils.ErrorLog("QuestStatusEvent.FireEvent: " + did + " (HandleClick)");
			}
		};
	}

	public void TryFireQuestStatusEvent(Session session, int did)
	{
		if (session.TheState.GetType().Equals(typeof(Session.Playing)))
		{
			QuestStatusEvent.FireEvent(did);
		}
	}

	private void DeactivateQuestScrollButtons()
	{
		if (questScrollUp.IsActive())
		{
			questScrollUp.SetActive(false);
		}
		if (questScrollDown.IsActive())
		{
			questScrollDown.SetActive(false);
		}
	}

	private void DeactivatePatchyUI()
	{
		if (patchyHudTitleBg.IsActive())
		{
			patchyHudTitleBg.SetActive(false);
		}
		if (patchyHudTitleIcon.IsActive())
		{
			patchyHudTitleIcon.SetActive(false);
		}
		if (patchyHudTitleLabel.IsActive())
		{
			patchyHudTitleLabel.SetActive(false);
		}
		SetPatchyHudIconVisible();
	}

	private void DeactivateNonPatchyUI()
	{
		jjBarHud.SetActive(false);
		moneyBarHud.SetActive(false);
		specialBarHud.SetActive(false);
		ResetCommunityImage(false);
		inventoryHudIcon.SetActive(false);
		editModeHudIcon.SetActive(false);
		questMarker.SetActive(false);
		happyfaceHud.SetActive(false);
		marketplaceHudIcon.SetActive(false);
		inventory.SetActive(false);
		questsOrigin.SetActive(false);
	}

	public bool IsQuestTrackerVisible()
	{
		return questsShown;
	}

	private void ExamineQuest(int questDid)
	{
		Quest quest = session.TheGame.questManager.GetQuest((uint)questDid);
		if (quest == null || SBGUI.GetInstance().ContainsGUIScreen<SBGUIAutoQuestStatusDialog>() || SBGUI.GetInstance().ContainsGUIScreen<SBGUIChunkQuestDialog>() || SBGUI.GetInstance().ContainsGUIScreen<SBGUIQuestDialog>())
		{
			return;
		}
		QuestDefinition questDefinition = session.TheGame.questManager.GetQuestDefinition(quest.Did);
		List<ConditionDescription> list = new List<ConditionDescription>();
		foreach (ConditionState endCondition in quest.EndConditions)
		{
			list.AddRange(endCondition.Describe(session.TheGame));
		}
		EnableUI(false);
		session.TheCamera.SetEnableUserInput(false);
		session.AddAsyncResponse("ignore_request_rush_sim", true);
		Action<SBGUIEvent, Session> oldGuiEventHandler = SBUIBuilder.UpdateGuiEventHandler(session, null);
		Action action = delegate
		{
			session.TheSoundEffectManager.PlaySound("Accept");
			SBUIBuilder.ReleaseTopScreen();
			EnableUI(true);
			session.TheCamera.SetEnableUserInput(true);
			session.CheckAsyncRequest("ignore_request_rush_sim");
			SBUIBuilder.UpdateGuiEventHandler(session, oldGuiEventHandler);
		};
		Action findButton = delegate
		{
			session.TheSoundEffectManager.PlaySound("Accept");
			EnableUI(true);
			session.TheCamera.SetEnableUserInput(true);
			session.CheckAsyncRequest("ignore_request_rush_sim");
			SBUIBuilder.UpdateGuiEventHandler(session, oldGuiEventHandler);
		};
		Action allDoneButton = delegate
		{
			session.TheSoundEffectManager.PlaySound("Accept");
			SBUIBuilder.ReleaseTopScreen();
			EnableUI(true);
			session.TheCamera.SetEnableUserInput(true);
			session.CheckAsyncRequest("ignore_request_rush_sim");
			SBUIBuilder.UpdateGuiEventHandler(session, oldGuiEventHandler);
			session.TheGame.simulation.ModifyGameState(new AutoQuestAllDoneAction(QuestDefinition.LastAutoQuestId));
		};
		if (questDefinition.Chunk)
		{
			if (questDefinition.Did == QuestDefinition.LastAutoQuestId)
			{
				SBUIBuilder.MakeAndPushAutoQuestStatusDialog(this, session, questDefinition, list, action, allDoneButton, action);
			}
			else
			{
				SBUIBuilder.MakeAndPushChunkQuestStatusDialog(this, session, questDefinition, list, action, action);
			}
		}
		else
		{
			SBUIBuilder.MakeAndPushQuestStatusDialog(this, session, questDefinition, list, action, findButton);
		}
		session.TheSoundEffectManager.PlaySound("quest_bubbles");
	}

	private bool ShouldCommunityShow()
	{
		if (session == null)
		{
			return false;
		}
		if (session.TheGame == null)
		{
			return false;
		}
		if (session.TheGame.communityEventManager == null)
		{
			return false;
		}
		CommunityEvent activeEvent = session.TheGame.communityEventManager.GetActiveEvent();
		if (activeEvent != null && activeEvent.m_nQuestPrereqID >= 0 && !session.TheGame.questManager.IsQuestCompleted((uint)activeEvent.m_nQuestPrereqID))
		{
			return false;
		}
		return activeEvent != null && !activeEvent.m_bHideUI && !(session.TheState is Session.CommunityEventSession) && !(session.TheState is Session.Shopping);
	}

	private void ResetCommunityImage(bool reset)
	{
		if (reset)
		{
			CommunityEvent activeEvent = session.TheGame.communityEventManager.GetActiveEvent();
			SBGUIAtlasImage component = communityEventHudIcon.GetComponent<SBGUIAtlasImage>();
			if (component != null)
			{
				component.SetTextureFromAtlas(activeEvent.m_sEventButtonTexture);
				communityEventHudIcon.SetActive(true);
			}
		}
		else
		{
			SBGUIAtlasImage component2 = communityEventHudIcon.GetComponent<SBGUIAtlasImage>();
			if (component2 != null)
			{
				component2.SetTextureFromAtlas("_blank_.png");
			}
			communityEventHudIcon.SetActive(false);
		}
	}
}
