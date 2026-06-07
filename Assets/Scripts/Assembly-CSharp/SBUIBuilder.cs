using System;
using System.Collections.Generic;
using UnityEngine;
using Yarg;

public static class SBUIBuilder
{
	public class ScreenContext
	{
		public int layers;

		public ScreenContext next;

		public override string ToString()
		{
			return null;
		}
	}

	public delegate SBGUIScreen MakeScreen();

	private class InteractionStripButtonInfo
	{
		public const string PREFAB_NAME = "ButtonStandinBig";

		public string textureToUse;

		public IControlBinding control;

		public InteractionStripButtonInfo(string textureToUse, IControlBinding control)
		{
		}
	}

	public const float kErrorMessageScale = 0.85f;

	public const float kErrorTitleScale = 0.45f;

	public const float ENABLED_ALPHA = 1f;

	public const float DISABLED_ALPHA = 0.25f;

	public const int MAX_EDIT_MODE_BARS = 14;

	private static GUIMainView mainView;

	private static Dictionary<string, SBGUIScreen> sCache;

	private static ScreenContext topContext;

	private static string game_revision;

	private static Vector3 invWidgetStartPos;

	private static Transform specialBarParent;

	private static Vector3 specialBarStartPosition;

	private static TFPool<SBGUITimebar> sTimebarPool;

	private static TFPool<SBGUINamebar> sNamebarPool;

	private static Dictionary<int, string> ResourcePrefixes;

	private const string TRACKING_RESOURCE_AMOUNTS = "TrackingResourceAmounts";

	private const string TRACKING_RESOURCE_PERCENTAGES = "TrackingResourcePercentages";

	private static readonly Dictionary<Type, string> controlToTextureMap;

	private static Dictionary<string, SBGUIButton> sInteractionStripButtons;

	private static SBGUIElement sInteractionStrip;

	private static GUIMainView MainView
	{
		get
		{
			return null;
		}
	}

	public static ScreenContext PushNewScreenContext()
	{
		return null;
	}

	public static void ReleaseScreenContexts(ScreenContext start, ScreenContext end)
	{
	}

	private static SBGUIScreen OptionalCacheScreen(string key, MakeScreen make, out bool instantiated)
	{
		instantiated = default(bool);
		return null;
	}

	private static SBGUIScreen CacheScreen(string key, MakeScreen make, out bool instantiated)
	{
		instantiated = default(bool);
		return null;
	}

	private static void PushScreen(SBGUIScreen screen)
	{
	}

	public static SBGUIScreen PeekTopScreen()
	{
		return null;
	}

	public static SBGUIScreen ReleaseTopScreen()
	{
		return null;
	}

	public static SBGUIScreen ReleaseScreen(int depth)
	{
		return null;
	}

	public static void ReleaseScreens(int depth, int layers)
	{
	}

	public static void ClearScreenCache()
	{
	}

	public static SBGUIAcceptUI MakeAndPushAcceptUI(Session session, Action<SBGUIEvent, Session> guiEventHandler, Action acceptButtonClickHandler)
	{
		return null;
	}

	public static SBGUIScreen MakeAndPushScratchLayer(Session session)
	{
		return null;
	}

	public static SBGUIScreen MakeAndPushEmptyUI(Session session, Action<SBGUIEvent, Session> guiEventHandler)
	{
		return null;
	}

	public static Action<SBGUIEvent, Session> UpdateGuiEventHandler(Session session, Action<SBGUIEvent, Session> guiEventHandler)
	{
		return null;
	}

	private static void AddTrackingForResource(SBGUIScreen screen, string labelKey, int resourceId)
	{
	}

	private static void AddTrackingForResources(SBGUIScreen screen, Session session)
	{
	}

	private static void UpdateTrackingForSpecialResource(SBGUIScreen screen, Session session)
	{
	}

	private static void UpdateQuestCounter(SBGUIScreen screen, Session session)
	{
	}

	public static SBGUIScreen MakeAndPushStartingProgress(Session session, Action privacyHandler, Action<SBGUIEvent, Session> guiEventHandler, bool makeLoadingBar, bool bPatchy)
	{
		return null;
	}

	public static SBGUIStandardScreen MakeAndPushStandardUI(Session session, bool allowHiding, Action<SBGUIEvent, Session> guiEventHandler, Action shopClickHandler, Action inventoryClickHandler, Action optionsHandler, Action editClickHandler, Action pavingClickHandler, Action<int, YGEvent> startDragOutHandler, Action<YGEvent> dragThroughHandler, Action openIAPTabHandlerSoft, Action openIAPTabHandlerHard, Action communityEventClickHandler, Action patchyClickHandler, bool editing = false)
	{
		return null;
	}

	public static SBGUIStandardScreen MakeAndPushPavingUI(Session session, Action<SBGUIEvent, Session> guiEventHandler, Action acceptHandler, Action editHandler, Action inventoryHandler)
	{
		return null;
	}

	public static SBGUIInsufficientResourcesDialog MakeAndPushInsufficientResourcesDialog(Session session, Dictionary<int, int> insufficientResourceIds, Dictionary<string, int> insufficientResourceTextures, int? rmtCost, string rmtTexture, string acceptLabel, Action okButtonHandler, Action cancelButtonHandler)
	{
		return null;
	}

	public static SBGUIRateMeDialog MakeAndAddRateMeDialog(Session session, bool unmutable = false)
	{
		return null;
	}

	public static SBGUIConfirmationDialog MakeAndPushConfirmationDialog(Session session, Action<SBGUIEvent, Session> guiEventHandler, string title, string message, string acceptLabel, string cancelLabel, Dictionary<string, int> resources, Action okButtonHandler, Action cancelButtonHandler, bool unmutable = false)
	{
		return null;
	}

	public static SBGUIFoundItemScreen MakeAndPushAcknowledgeDialog(Session session, Action<SBGUIEvent, Session> guiEventHandler, string title, string message, string texture, string acceptLabel, Action okButtonHandler)
	{
		return null;
	}

	public static SBGUIConfirmationDialog MakeAndPushExpansionDialog(Session session, Action<SBGUIEvent, Session> guiEventHandler, string title, string message, string acceptLabel, string cancelLabel, Dictionary<string, int> resources, Action okButtonHandler, Action cancelButtonHandler, bool unmutable = false)
	{
		return null;
	}

	public static SBGUIGetJellyDialog MakeAndPushGetJellyDialog(Session session, Action<SBGUIEvent, Session> guiEventHandler, string title, string message, string question, string acceptLabel, string cancelLabel, Dictionary<string, int> resources, Action okButtonHandler, Action cancelButtonHandler, bool unmutable = false)
	{
		return null;
	}

	public static SBGUIMicroConfirmDialog MakeAndPushJjMicroConfirmDialog(Session session, Action<SBGUIEvent, Session> overrideGuiEventHandler, string message, Cost.CostAtTime jjAmount, Action acceptHandler, Action cancelHandler, Vector2 screenPosition)
	{
		return null;
	}

	public static SBGUICharacterDialog MakeAndAddDialogSequence(SBGUIScreen parent, Session session, List<object> sequence, Action<int> dialogChangeHandler)
	{
		return null;
	}

	public static SBGUIQuestDialog MakeAndAddQuestStartDialog(SBGUIScreen parent, Session session, List<Reward> rewards, string title, string icon)
	{
		return null;
	}

	public static SBGUIAutoQuestStatusDialog MakeAndAddAutoQuestStartDialog(SBGUIScreen parent, SBGUIStandardScreen screen, Session session, List<Reward> rewards, QuestDefinition questDef, List<ConditionDescription> steps, Action allDoneButton, Action makeButton)
	{
		return null;
	}

	public static SBGUIChunkQuestDialog MakeAndAddQuestChunkStartDialog(SBGUIScreen parent, SBGUIStandardScreen screen, Session session, List<Reward> rewards, QuestDefinition questDef, List<ConditionDescription> steps, Action findButton)
	{
		return null;
	}

	public static SBGUIQuestDialog MakeAndPushQuestStatusDialog(SBGUIStandardScreen screen, Session session, QuestDefinition questDef, List<ConditionDescription> steps, Action okButton, Action findButton)
	{
		return null;
	}

	public static SBGUIAutoQuestStatusDialog MakeAndPushAutoQuestStatusDialog(SBGUIStandardScreen screen, Session session, QuestDefinition questDef, List<ConditionDescription> steps, Action okButton, Action allDoneButton, Action makeButton)
	{
		return null;
	}

	public static SBGUIChunkQuestDialog MakeAndPushChunkQuestStatusDialog(SBGUIStandardScreen screen, Session session, QuestDefinition questDef, List<ConditionDescription> steps, Action findButton, Action okButton)
	{
		return null;
	}

	public static SBGUIQuestDialog MakeAndAddQuestCompleteDialog(SBGUIScreen parent, Session session, List<Reward> rewards, string title, string icon)
	{
		return null;
	}

	public static SBGUIAutoQuestCompleteDialog MakeAndAddAutoQuestCompleteDialog(SBGUIScreen parent, SBGUIStandardScreen screen, Session session, List<Reward> rewards, QuestDefinition questDef)
	{
		return null;
	}

	public static SBGUIChunkQuestDialog MakeAndAddQuestChunkCompleteDialog(SBGUIScreen parent, SBGUIStandardScreen screen, Session session, List<Reward> rewards, QuestDefinition questDef, List<ConditionDescription> steps)
	{
		return null;
	}

	public static SBGUIQuestDialog MakeAndAddBootyQuestCompleteDialog(SBGUIScreen parent, Session session, List<Reward> rewards, string title, string icon)
	{
		return null;
	}

	public static SBGUIQuestLineDialog MakeAndAddQuestLineStartDialog(SBGUIScreen parent, Session session, List<Reward> rewards, string dialogHeading, string dialogBody, string portrait, string rewardTexture, string rewardName)
	{
		return null;
	}

	public static SBGUIQuestLineDialog MakeAndAddQuestLineCompleteDialog(SBGUIScreen parent, Session session, List<Reward> rewards, string dialogHeading, string dialogBody, string portrait, string rewardTexture, string rewardName)
	{
		return null;
	}

	public static SBGUICharacterBusyScreen MakeAndPushUnitBusyUI(SBGUIStandardScreen screen, Session session, Simulated pSimulated, Task pTask, Action pFeedWishAction, Action pRushWishAction, Action pRushTaskAction, Action closeButton, Action onWatchWishAd, Action onWatchTaskAd)
	{
		return null;
	}

	public static SBGUICharacterIdleScreen MakeAndPushUnitIdleUI(SBGUIStandardScreen screen, Session session, Simulated pSimulated, List<TaskData> pTaskDatas, Action pFeedWishAction, Action pRushWishAction, Action<int> pDoTaskAction, Action closeButton, Action onWatchAd)
	{
		return null;
	}

	public static SBGUIProgressDialog MakeAndAddProgressDialog(SBGUIScreen parent, Session session, string title, string description, Cost rush_cost, Action onRush, Action onClose)
	{
		return null;
	}

	public static SBGUITimebar MakeAndAddTimebar(Session session, SBGUIScreen parent, uint ownerDid, string description, ulong completeTime, ulong totalTime, float duration, Cost rushCost, Action onRush, SBGUITimebar.HostPosition hPosition, Action onFinish, List<int> pTaskCharacterDIDs, Action<int> pTaskCharacterClicked, Action onWatchAd)
	{
		return null;
	}

	public static void ReleaseTimebar(SBGUITimebar timebar)
	{
	}

	public static void ReleaseTimebars()
	{
	}

	public static SBGUINamebar MakeAndAddNamebar(Session session, SBGUIScreen parent, string name, SBGUINamebar.HostPosition hPosition, Action onFinish, List<int> pTaskCharacterDIDs, Action<int> pTaskCharacterClicked)
	{
		return null;
	}

	public static void ReleaseNamebar(SBGUINamebar namebar)
	{
	}

	public static void ReleaseNamebars()
	{
	}

	public static SBGUIElement MakeAndAddInteractionStrip(Session session, uint ownerDid, SBGUIScreen parent, ICollection<IControlBinding> controls)
	{
		return null;
	}

	private static SBGUIElement MakeGenericInteractionStrip(Session session, uint ownerDid, List<InteractionStripButtonInfo> buttonInfos)
	{
		return null;
	}

	private static Action InteractionStripButtonHandlerClosure(Session session, Action<Session> action)
	{
		return null;
	}

	public static void UpdateAcceptPlacementButton(SBGUIButton button, Session session)
	{
	}

	public static void UpdateButton(SBGUIButton button, bool enabled)
	{
	}

	private static void SwapButtonTexture(SBGUIElement parent, string buttonName, string textureToUse)
	{
	}

	public static void MakeActivityIndicator(SBGUIScreen parent)
	{
	}

	public static SBGUIMarketplaceScreen MakeAndPushMarketplaceDialog(Session session, Action<SBGUIEvent, Session> guiEventHandler, Action closeClickHandler, Action<SBMarketOffer> purchaseClickHandler, EntityManager entityMgr, ResourceManager resourceMgr, SoundEffectManager sfxMgr, Catalog catalog)
	{
		return null;
	}

	public static SBGUICraftingScreen MakeAndPushCraftingUI(Session session, Action<SBGUIEvent, Session> guiEventHandler, Action closeClickHandler, Action<SBGUICraftingScreen, CraftingRecipe> craftRecipeHandler, Action<int> rushCraftHandler, Action<CraftingRecipe> setSelected, CraftingCookbook cookbook, CraftingRecipe highlightedRecipe, List<int> pTaskCharacterDIDs, Action<int> pTaskCharacterClicked, int effectiveSlotCount, int maxSlotCount, Action<int> watchADHandler)
	{
		return null;
	}

	public static SBGUIVendorScreen MakeAndPushVendorUI(Session session, Action<SBGUIEvent, Session> guiEventHandler, Action backHandler, Action<VendingInstance> vendorInstanceHandler, Action rushHandler, VendorDefinition vendorDef, Dictionary<int, VendingInstance> vendingInstances, VendingInstance specialVendingInstance, VendingDecorator vendingEntity, List<int> pTaskCharacterDIDs, Action<int> pTaskCharacterClicked)
	{
		return null;
	}

	private static void CraftingScreenGraphicalSetup(SBGUICraftingScreen crafting, CraftingCookbook cookbook)
	{
	}

	public static SBGUICreditsScreen MakeAndPushCreditsUI(Session session, Action closeClickHandler)
	{
		return null;
	}

	public static SBGUIDebugScreen MakeAndParentDebugUI(Session session, SBGUIScreen parent, Action closeClickHandler, Action toggleFramerateCounter, Action toggleFreeEditMode, Action saveFreeEditProgress, Action toggleHitBoxes, Action toggleFootprints, Action toggleExpansionBorders, Action addMoney, Action addJJ, Action addSpecialCurrency, Action addFoods, Action toggleRMT, Action deleteServerGame, Action resetEventItems, Action toggleFreeCameraMode, Action completeAllQuests, Action levelUp, Action logDump, Action unlockDecos, Action addHourSimulation, Action incrementDailyBonus, Action fastFoward, Action addOneLevel, Action reset_device_id)
	{
		return null;
	}

	public static SBGUIClearingScreen MakeAndPushClearingUI(string cost, Action okButtonHandler, Action cancelButtonHandler)
	{
		return null;
	}

	public static SBGUIInventoryScreen MakeAndPushInventoryDialog(Session session, EntityManager entityMgr, SoundEffectManager sfxMgr, Action closeClickHandler, Action<SBInventoryItem> buildingClickHandler, Action<SBInventoryItem> inventoryClickHandler)
	{
		return null;
	}

	public static SBGUICommunityEventScreen MakeAndPushCommunityEventDialog(Session session, Action closeClickHandler, Action<CommunityEvent, SoaringCommunityEvent, SoaringCommunityEvent.Reward> purchaseHandler)
	{
		return null;
	}

	public static SBGUILevelUpScreen MakeAndAddLevelUpDialog(SBGUIScreen parent, Session session, LevelUpDialogInputData inputData)
	{
		return null;
	}

	public static SBGUIFoundItemScreen MakeAndAddFoundItemScreen(Session session, SBGUIScreen parent)
	{
		return null;
	}

	public static SBGUIExplanationDialog MakeAndAddExplanationDialog(SBGUIScreen parent)
	{
		return null;
	}

	public static SBGUIMoveInDialog MakeAndAddMoveInDialog(SBGUIScreen parent)
	{
		return null;
	}

	public static SBGUIDailyBonusDialog MakeAndAddDailyBonusDialog(SBGUIScreen parent)
	{
		return null;
	}

	public static SBGUISpongyGamesDialog MakeAndAddSpongyGamesDialog(SBGUIScreen parent)
	{
		return null;
	}

	public static SBGUIScreen MakeAndPushAgeGateDialog(Action backHandler, Action submitHandler, Action cancelHandler, Action inputHandler, bool showBackground = false)
	{
		return null;
	}

	public static SBGUIScreen MakeAndPushFBAgeGateDialog(Action backHandler, Action submitHandler, Action cancelHandler, Action inputHandler, bool showBackground = false)
	{
		return null;
	}

	public static SBGUIScreen MakeAndPushHelpDialog(Action faqHandler, Action contactHandler, Action backHandler)
	{
		return null;
	}

	public static SBGUIScreen MakeAndPushOptionsDialog(Action backHandler, Action moreNickHandler, Action toggleSFXHandler, Action toggleMusicHandler, Action achievementsHandler, Action creditsHandler, Action privacyHandler, Action eulaHandler, Action debugHandler, Action parentsHandler, Action facebookHandler)
	{
		return null;
	}

	private static string GetResourcePrefix(int resourceId)
	{
		return null;
	}

	private static void UpdateStandardUI(SBGUIScreen screen, Session session)
	{
	}

	private static SBGUIElement CreateInteractionStripCache()
	{
		return null;
	}

	public static void ReleaseInteractionStrip(SBGUIElement strip)
	{
	}

	public static void CreateErrorDialog(Session session, string title, string message, string okButtonLabel, Action okHandler, float messageScale, float titleScale)
	{
	}

	public static void CreateErrorDialog(Session session, string title, string message, string okButtonLabel, Action okHandler, string cancelButtonLabel, Action cancelHandler, float messageScale, float titleScale)
	{
	}

	public static SBGUIFacebookConnectDialog MakeAndPushFacebookConnectDialog(Session session, Action ConnectButton, Action CloseButton, bool showBackground = true)
	{
		return null;
	}
}
