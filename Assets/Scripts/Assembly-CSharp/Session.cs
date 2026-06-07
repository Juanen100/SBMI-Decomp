using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Yarg;

public class Session
{
	public delegate void GameloopAction();

	public class SoaringSessionRestartDelegate : SoaringDelegate
	{
		public override void OnRequestingSessionData(bool success, SoaringError error, SoaringArray sessions, SoaringDictionary raw_data, SoaringContext context)
		{
		}
	}

	public class FramerateWatcher
	{
		public float frequency;

		private float accum;

		private int frames;

		private float waitTime;

		private float prevWindowsFPS;

		public float Framerate
		{
			get
			{
				return 0f;
			}
		}

		public void OnUpdate()
		{
		}
	}

	private class SessionProperties
	{
		public class DraggedGood
		{
			public int productId;

			public Resource resource;

			public DraggedGood(int productId, Resource resource)
			{
			}
		}

		public SBGUIStandardScreen playingHud;

		public SBGUIStandardScreen ageGateHud;

		public bool transitionSilently;

		public SBGUIStandardScreen recipesHud;

		public SBGUICraftingScreen recipesWindow;

		public Dictionary<CraftingCookbook, CraftingRecipe> lastSelectedRecipe;

		public Simulated m_pTaskSimulated;

		public bool m_bAutoPanToSimulatedOnLeave;

		public SBGUIStandardScreen communityEventHud;

		public SBGUICommunityEventScreen communityEventScreen;

		public SBGUIStandardScreen dragFeedHud;

		public DraggedGood draggedGood;

		public Simulated candidateSimulated;

		public YGEvent carriedUiEvent;

		public int playDelayCounter;

		public SBGUIMicroConfirmDialog microConfirmDialog;

		public Action denialActions;

		public Action cleanUp;

		public HardSpendActions hardSpendActions;

		public Simulated overrideSimulatedToRush;

		public SBGUIStandardScreen helpHud;

		public string iapBundleName;

		public SBGUIInsufficientResourcesDialog insufficientDialog;

		public SBGUIStandardScreen inventoryHud;

		public SBGUIScreen editingHud;

		public bool waitToDecidePlacement;

		public Vector2 preMovePosition;

		public bool preMoveFlip;

		public bool preMovePositionSet;

		public bool isInteractionStripActive;

		public bool isDraggingBuilding;

		public bool isDraggingBuildingAndScreen;

		public bool firstEntered;

		public bool startedTouchOnEmptySpace;

		public SBGUIStandardScreen optionsHud;

		public bool cameFromMarketplace;

		public Simulated touchingSim;

		public Simulated queuedClickedSim;

		public Vector2 moveDragStart;

		public IDisplayController tappedDisplayController;

		public SBGUIStandardScreen shoppingHud;

		public string marketplaceSessionActionID;

		public string m_sLeaveType;

		public bool reducedBuffer;

		public int storeVisitSinceLastPurchase;

		public SBGUIStandardScreen dialogHud;

		public SBGUIStandardScreen unitBusyHud;

		public SBGUICharacterBusyScreen unitBusyWindow;

		public Task unitBusyTask;

		public SBGUIStandardScreen unitIdleHud;

		public SBGUICharacterIdleScreen unitIdleWindow;

		public SBGUIVendorScreen vendorScreen;

		public Reward reward;
	}

	[StructLayout((LayoutKind)0, Size = 16)]
	private struct StateChangeRequest
	{
		public string state;

		public bool changeContext;
	}

	public delegate void AsyncAction();

	private class BubbleSwipeParticleSystemRequestDelegate : ParticleSystemManager.Request.IDelegate
	{
		protected Session session;

		protected Vector3 viewportPosition;

		public Transform ParentTransform
		{
			get
			{
				return null;
			}
		}

		public Vector3 Position
		{
			get
			{
				return default(Vector3);
			}
		}

		public bool isVisible
		{
			get
			{
				return false;
			}
		}

		public BubbleSwipeParticleSystemRequestDelegate(Session s)
		{
		}
	}

	private class ConfettiSwipeParticleSystemRequestDelegate : ParticleSystemManager.Request.IDelegate
	{
		protected Session session;

		protected Vector3 viewportPosition;

		public Transform ParentTransform
		{
			get
			{
				return null;
			}
		}

		public Vector3 Position
		{
			get
			{
				return default(Vector3);
			}
		}

		public bool isVisible
		{
			get
			{
				return false;
			}
		}

		public ConfettiSwipeParticleSystemRequestDelegate(Session s)
		{
		}
	}

	private class BalloonSwipeParticleSystemRequestDelegate : ParticleSystemManager.Request.IDelegate
	{
		protected Session session;

		protected Vector3 viewportPosition;

		public Transform ParentTransform
		{
			get
			{
				return null;
			}
		}

		public Vector3 Position
		{
			get
			{
				return default(Vector3);
			}
		}

		public bool isVisible
		{
			get
			{
				return false;
			}
		}

		public BalloonSwipeParticleSystemRequestDelegate(Session s)
		{
		}
	}

	private class SeaflowerSwipeParticleSystemRequestDelegate : ParticleSystemManager.Request.IDelegate
	{
		protected Session session;

		protected Vector3 viewportPosition;

		public Transform ParentTransform
		{
			get
			{
				return null;
			}
		}

		public Vector3 Position
		{
			get
			{
				return default(Vector3);
			}
		}

		public bool isVisible
		{
			get
			{
				return false;
			}
		}

		public SeaflowerSwipeParticleSystemRequestDelegate(Session s)
		{
		}
	}

	private class FogEffectRequestDelegate : ParticleSystemManager.Request.IDelegate
	{
		protected Session session;

		protected Vector3 position;

		public Transform ParentTransform
		{
			get
			{
				return null;
			}
		}

		public Vector3 Position
		{
			get
			{
				return default(Vector3);
			}
			set
			{
			}
		}

		public bool isVisible
		{
			get
			{
				return false;
			}
		}

		public FogEffectRequestDelegate(Session s)
		{
		}
	}

	private class TapFXParticleSystemRequestDelegate : ParticleSystemManager.Request.IDelegate
	{
		protected Session session;

		protected Vector3 position;

		public Transform ParentTransform
		{
			get
			{
				return null;
			}
		}

		public Vector3 Position
		{
			get
			{
				return default(Vector3);
			}
			set
			{
			}
		}

		public bool isVisible
		{
			get
			{
				return false;
			}
		}

		public TapFXParticleSystemRequestDelegate(Session s)
		{
		}
	}

	public class InteractionStripMixin
	{
		private const string INTERACTION_STRIP = "InteractionStrip";

		private const string INTERACTION_CONTROLS = "InteractionControls";

		public const string ACCEPT_CALLBACK = "InteractionStrip_AcceptCallback";

		public const string REJECT_CALLBACK = "InteractionStrip_RejectCallback";

		public Vector3 StripPosition { get; set; }

		public void ActivateOnSelected(Session session)
		{
		}

		public void Deactivate(Session session)
		{
		}

		public void EnableRejectButton(Session session, bool enable)
		{
		}

		public void EnableButtons(Session session, bool enable)
		{
		}

		public void OnUpdate(Session session)
		{
		}

		public void SetAcceptHandler(Session session, Action<Session> handler)
		{
		}

		public void SetRejectHandler(Session session, Action<Session> handler)
		{
		}

		public bool FindTutorialPointer(Session session)
		{
			return false;
		}

		private void MoveSubUiWithSelected(Session session)
		{
		}
	}

	public class NamebarMixin
	{
		public const int YOFFSET = 20;

		public const int HEIGHT = 100;

		private SBGUINamebar m_pNamebarGUI;

		private string m_sGameObjectID;

		private const string _sNAMEBAR = "Namebar";

		public bool IsActive
		{
			get
			{
				return false;
			}
		}

		public bool ActivateOnSelected(Session pSession, Simulated pSimulated, float fYOffset = 20f)
		{
			return false;
		}

		public void Deactivate(Session pSession)
		{
		}

		public void Extend()
		{
		}
	}

	public class NamebarGroup
	{
		public const string TASK_SRC_UNIT = "TaskSrcUnit";

		private NamebarMixin m_pTaskAtBuildingNamebar;

		private NamebarMixin m_pNamebar;

		public bool IsActive
		{
			get
			{
				return false;
			}
		}

		public void ActivateOnSelected(Session pSession)
		{
		}

		public void Deactivate(Session pSession)
		{
		}

		public void Extend()
		{
		}
	}

	public abstract class Prioritizer
	{
		protected Simulated best;

		protected Camera camera;

		public Simulated Best
		{
			get
			{
				return null;
			}
		}

		public Prioritizer(Camera camera)
		{
		}

		public void SelectBest(Simulated simulated)
		{
		}

		public float distanceToCamera(Simulated simulated, Camera camera)
		{
			return 0f;
		}

		protected int CompareByDistanceToCamera(Simulated a, Simulated b)
		{
			return 0;
		}

		protected abstract int Compare(Simulated a, Simulated b);
	}

	public class SelectionPrioritizer : Prioritizer
	{
		public SelectionPrioritizer(Camera camera)
			: base(null)
		{
		}

		protected override int Compare(Simulated a, Simulated b)
		{
			return 0;
		}
	}

	public class TemptationPrioritizer : Prioritizer
	{
		public TemptationPrioritizer(Camera camera)
			: base(null)
		{
		}

		protected override int Compare(Simulated a, Simulated b)
		{
			return 0;
		}
	}

	public class AgeGate : State
	{
		private string inputString;

		private SBGUIScreen ageGate;

		private SBGUILabel invalidAnswer;

		private SBGUILabel inputLabel;

		private SBGUIButton inputBox;

		private SBGUIButton submit;

		private SBGUIButton cancel;

		private TouchScreenKeyboard keyboard;

		private SBGUIButton closeButton;

		private bool agegateShown;

		private List<string> modelIdentifiers;

		public void OnEnter(Session session)
		{
		}

		public bool SubmitCheck(Session session)
		{
			return false;
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session = null)
		{
		}
	}

	public class Authorizing : State
	{
		private bool errorScreenShown;

		public void OnEnter(Session session)
		{
		}

		public void HandshakeResponder(SoaringContext c)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		public void AddAdditionalCredentials()
		{
		}
	}

	public class BrowsingRecipes : State
	{
		private const string BROWSING_UI_HANDLER = "browsing_ui";

		private const string KEEP_INVENTORY_OPEN = "KeepInventoryOpen";

		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}

		public void CheckRecipeForJelly(Session session, SBGUICraftingScreen screen, CraftingRecipe recipe)
		{
		}

		public void CraftRecipe(Session session, SBGUICraftingScreen screen, CraftingRecipe recipe)
		{
		}

		private void WatchADCraftProduction(Session session, int slotId)
		{
		}

		private void CraftProductionRush(Session session, int slotId)
		{
		}
	}

	public class CheckPatching : State
	{
		private bool _doneChecking;

		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}

		public void PatchingEventListener(string patchingEvent)
		{
		}
	}

	public class Clearing : State
	{
		public void Purchase(Session session, DebrisEntity debrisEntity)
		{
		}

		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}
	}

	public class CommunityEventSession : State
	{
		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}
	}

	public class Credits : State
	{
		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}
	}

	public class SessionDebug : State
	{
		private const string DEBUG_SCREEN = "DEBUG_SCREEN";

		private int dailyBonusDay;

		public void OnEnter(Session session)
		{
		}

		private void DisplayDailyBonus(SoaringContext context)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}
	}

	public class DragFeeding : State
	{
		private SBGUIPulseImage icon;

		private static readonly Vector2 FINGER_OFFSET;

		private static readonly Quaternion ICON_ANGLE;

		private const string DRAGFEEDING_UI_HANDLER = "dragfeeding_ui_handler";

		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}

		public static void SwitchTo(Session session, int productId, YGEvent triggeringEvent)
		{
		}

		public static Action<int, YGEvent> SwitchToFn(Session session)
		{
			return null;
		}

		private void SetIconToEventPosition(YGEvent evt)
		{
		}

		private void Tempt(Simulated simulated, Session session)
		{
		}

		private void CancelTempt(Session session)
		{
		}

		private bool TryFeedTempted(Session session)
		{
			return false;
		}
	}

	public class Editing : Playing
	{
		public const string FROM_EDIT = "FromEdit";

		public override void OnEnter(Session session)
		{
		}

		public override void OnLeave(Session session)
		{
		}

		public override void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		public override void OnUpdate(Session session)
		{
		}
	}

	public class ErrorDialog : State
	{
		private const string ERROR_DIALOG = "ERROR_DIALOG";

		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}
	}

	public class Expanding : State
	{
		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}

		private void ShowDialog(Session session)
		{
		}

		public void PurchaseExpansion(Session session)
		{
		}
	}

	public class NewExpansion : State
	{
		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}

		public void PurchaseExpansion(Session session)
		{
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		private void ShowDialog(Session session)
		{
		}
	}

	public class FacebookLogin : State
	{
		private Session currentSession;

		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}

		private void InitCallback()
		{
		}

		private void FBLogin()
		{
		}

		private void FBLoginCallback(bool success)
		{
		}

		private void OnHideUnity(bool isGameShown)
		{
		}

		private void ChangeToResolveSessionStateOnStartup()
		{
		}

		public void AddAdditionalCredentials(string facebookId)
		{
		}
	}

	public class FacebookLogout : State
	{
		private Session currentSession;

		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}

		private void InitCallback()
		{
		}

		private void FBLogout()
		{
		}

		private void OnHideUnity(bool isGameShown)
		{
		}
	}

	public class FBAgeGate : State
	{
		private string inputString;

		private SBGUIScreen ageGate;

		private SBGUILabel invalidAnswer;

		private SBGUILabel inputLabel;

		private SBGUIButton inputBox;

		private SBGUIButton submit;

		private SBGUIButton cancel;

		private TouchScreenKeyboard keyboard;

		private SBGUIButton closeButton;

		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}

		public bool SubmitCheck(Session session)
		{
			return false;
		}

		private void DisplayError()
		{
		}
	}

	public class GameStarting : State
	{
		private enum GameStartingState
		{
			STATE_FIRST = -1,
			STATE_PATCHING_CONTENT = 0,
			STATE_ASSEMBLE_GAME_STATE = 1,
			STATE_LOAD_ENTITY_BLUEPRINTS = 2,
			STATE_CREATE_GAME = 3,
			STATE_LOAD_ASSETS = 4,
			STATE_FETCH_PRODUCT_INFO = 5,
			STATE_AWAIT_PRODUCT_INFO = 6,
			STATE_FETCH_PURCHASE_INFO = 7,
			STATE_AWAIT_PURCHASE_INFO = 8,
			STATE_START_STORE = 9,
			STATE_LOAD_ASSETS_TERRAIN = 10,
			STATE_LOAD_ASSETS_SIMULATION = 11,
			STATE_PRECACHE_GUI = 12,
			STATE_LOAD_ASSETS_TIME_DEPENDENTS = 13,
			STATE_LOAD_ASSETS_SEND_COMMANDS = 14,
			STATE_CREATE_TERRAIN_MESHES = 15,
			STATE_LOAD_ASSETS_ACTIVATE_QUESTS = 16,
			STATE_PROCESS_PENDING = 17,
			STATE_UNLOAD_UNUSED_ASSETS = 18,
			STATE_SETUP_SIMULATION = 19,
			STATE_LOAD_SOARING_COMMUNITY_EVENTS = 20,
			STATE_ANALYTICS_BOOKKEPING = 21,
			STATE_LAST = 22,
			STATE_ERROR = 23
		}

		private delegate void ProcessStartingProgressState(Session session);

		public enum SplashScreenState
		{
			Loading = 0,
			Patchy = 1,
			None = 2
		}

		private SoaringContext LOAD_GAME_CONTEXT;

		private SaveGameScreen saveGameScreen;

		private float elapsedProductInfoTime;

		private float elapsedPurchaseInfoTime;

		private int currentState;

		private ProcessStartingProgressState[] processes;

		private float errorMessageScale;

		private float errorTitleScale;

		private int currentAdvance;

		private SBGUIButton policyButton;

		private SBGUILabel policy_Label;

		private int precacheGUIState;

		private int loadTimeDependentsState;

		private StaticContentLoader contentLoader;

		private int performedMigration;

		private SBGUIElement loadingSpinner;

		private static int _CommunityEventIndex;

		private static CommunityEvent[] _CommunityEvents;

		private static Session _CommunityEventSession;

		private AssetServices.AssetServicesMonitor mUnloadAssetMonitor;

		private static bool didOpenUpdateDialog;

		private const string STARTING_PROGRESS = "starting_progress";

		private const string POLICY_BUTTON = "policy_button";

		private void OnGameCreated(Session session)
		{
		}

		private void DeferDialogs(Session session)
		{
		}

		public void OnLoadGameDelegate(SoaringContext context)
		{
		}

		private void LoadEntityBlueprints(Session session)
		{
		}

		private void CallLoadFromNetwork(Session session, bool isRetryAttempt = false)
		{
		}

		public static void ResetShowSplashScreen(SplashScreenState state)
		{
		}

		public static void UnloadSaveGameAtlas()
		{
		}

		public static SBGUIScreen CreateLoadingScreen(Session session, bool makeLoadingBar = false, string starting_progress = "starting_progress", bool changeInitState = true)
		{
			return null;
		}

		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public static void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}

		private void DisplayDailyBonus(SoaringContext context)
		{
		}

		private void AdvanceState(Session session)
		{
		}

		public void CRITICAL_ERROR_ALL_GAMES_CORRUPTED(Session session, Exception e)
		{
		}

		public string WithErrorID(string message, int errorID)
		{
			return null;
		}

		private bool CheckServerGameWithSession(Session session, bool canSave)
		{
			return false;
		}

		private void CreateGame(Session session)
		{
		}

		private void LoadSoaringCommunityEvents(Session session)
		{
		}

		private void HandleSetValueFinished(bool bSuccess, SoaringError pError, SoaringDictionary pData, SoaringContext pContext)
		{
		}

		private bool dataIsChange(string level_server, string money_server, string jelly_server, string patty_server, string timeStamp_server, string level_local, string money_local, string jelly_local, string patty_local, string timeStamp_local)
		{
			return false;
		}

		private void LoadAssets(Session session)
		{
		}

		private void CreateTerrainMeshes(Session session)
		{
		}

		private void FetchProductInfo(Session session)
		{
		}

		private void AwaitProductInfo(Session session)
		{
		}

		private void ProcessTriggers(Session session)
		{
		}

		private void HandleUnusedAssets(Session session)
		{
		}

		private void SetupSimulation(Session session)
		{
		}

		private void AnalyticsBookkeeping(Session session)
		{
		}

		private void PatchContent(Session session)
		{
		}

		public void PatchingEventListener(string patchingEvent, Session session)
		{
		}

		private void AssembleGameState(Session session)
		{
		}

		private void FetchPurchaseInfo(Session session)
		{
		}

		private void AwaitPurchaseInfo(Session session)
		{
		}

		private void StartStore(Session session)
		{
		}

		private void LoadLocalAssetsTerrain(Session session)
		{
		}

		private void LoadLocalAssetsCreateSimulation(Session session)
		{
		}

		private void PrecacheGUI(Session session)
		{
		}

		private void LoadLocalAssetsLoadTimeDependents(Session session)
		{
		}

		private void LoadLocalAssetsSendPendingCommands(Session session)
		{
		}

		private void LoadLocalAssetsActivateQuests(Session session)
		{
		}

		private void CreateErrorDialog(Session session, string title, string message, string okButtonLabel, Action okHandler, float messageScale, float titleScale)
		{
		}

		private void CreateErrorDialog(Session session, string title, string message, string okButtonLabel, Action okHandler, string cancelButtonLabel, Action cancelHandler, float messageScale, float titleScale)
		{
		}
	}

	public class GameStopping : State
	{
		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}
	}

	public class GetJelly : State
	{
		private const string GET_JELLY = "GetJelly";

		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}
	}

	public class HardSpendConfirm : State
	{
		private const string HARD_SPEND_CONFIRM_HANDLER = "hard_spend_confirm_ui";

		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}
	}

	public class HardSpendPassthrough : State
	{
		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}

		public static void ClearSpendProperties(Session session)
		{
		}
	}

	public class HardSpendActions
	{
		public Cost.CostAtTime cost;

		public string subjectText;

		public int subjectDID;

		public Action execute;

		public Action complete;

		public Action cancel;

		public Action<bool, Cost> logSpend;

		public Vector2 screenPosition;

		public HardSpendActions(Action execute, Cost.CostAtTime cost, string subjectText, int subjectDID, Action complete, Action<bool, Cost> logSpend, Vector2 screenPosition)
		{
		}

		public HardSpendActions(Action execute, Cost.CostAtTime cost, string subjectText, int subjectDID, Action complete, Action cancel, Action<bool, Cost> logSpend, Vector2 screenPosition)
		{
		}
	}

	public class Help : State
	{
		private Session _session;

		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}
	}

	public class InAppPurchasing : State
	{
		private bool receivedProduct;

		private bool receivedError;

		private bool canceledTransaction;

		private float elapsedTime;

		private string errorTitle;

		private string errorMessage;

		public void OnUpdate(Session session)
		{
		}

		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnPurchaseUpdate(object sender, RmtStore.StoreEventArgs args)
		{
		}

		public void OnPurchaseResponse(object sender, RmtStore.StoreEventArgs args)
		{
		}

		public static void OnPurchaseDefered(object sender, RmtStore.StoreEventArgs args)
		{
		}

		public void OnPurchaseError(object sender, RmtStore.StoreEventArgs args)
		{
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}
	}

	public class InsufficientDialog : State
	{
		public void OnEnter(Session session)
		{
		}

		public void Setup(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}

		private void PrepForStoreUI(Session session, string tabToOpen)
		{
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}
	}

	public class Inventory : State
	{
		public const string FROM_INVENTORY = "FromInventory";

		public const string ASSOCIATED_ENTITIES = "AssociatedEntities";

		private bool didFinishInitialization;

		private const string INVENTORY_UI_HANDLER = "inventory_ui";

		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		public void InventoryLoadingFinished()
		{
		}

		public void OnUpdate(Session session)
		{
		}
	}

	public abstract class MoveBuilding : State
	{
		public const string OVERRIDE_DRAG = "override_drag";

		public const string PANNING_EVENT = "panning_event";

		public const string SILENT_ENTER = "silent_enter";

		protected const string BLOCKING_SIMULATEDS = "blocking_sims";

		protected InteractionStripMixin interactionStrip;

		protected bool? savedFlippedState;

		private const string MOVEDRAGGING_UI_HANDLER = "movedragging_ui";

		public virtual void OnEnter(Session session)
		{
		}

		public virtual void OnLeave(Session session)
		{
		}

		protected void DecideForSelectedBuilding(Session session)
		{
		}

		protected abstract void HandleSBGUIEvent(SBGUIEvent evt, Session session);

		public void OnUpdate(Session session)
		{
		}

		protected void SnapSelectedToInputPosition(Session session, Vector2 position, bool snapObject, bool updatePaths = false)
		{
		}

		protected bool IsValidLocationForSelected(Session session)
		{
			return false;
		}

		protected virtual void AcceptPlacement(Session session)
		{
		}

		protected void DenyPlacement(Session session)
		{
		}

		protected void ResetMoveDecorationsOnSelected(Session session)
		{
		}

		protected Simulated getWorker(Simulated buildingSim, Simulation simulation)
		{
			return null;
		}

		protected void ResetPlacement(Session session)
		{
		}

		private bool CheckFlag(Session session, string flagKey)
		{
			return false;
		}

		protected bool WasFromInventory(Session session)
		{
			return false;
		}

		protected bool WasFromEdit(Session session)
		{
			return false;
		}

		protected void ColorSelectedByOccupation(Session session)
		{
		}

		protected void MarkBlockers(Session session, bool persist = true)
		{
		}

		protected void UnmarkBlockers(Session session, bool persist = false)
		{
		}

		protected void CleanupMovementVisuals(Session session)
		{
		}

		protected void AdornMovementVisuals(Session session)
		{
		}

		protected void UpdateMovementBookkeeping(Session session)
		{
		}
	}

	public class MoveBuildingInEdit : MoveBuilding
	{
		private bool m_bTouchBegan;

		private bool userCameraActive;

		private Simulated clickedSim;

		public override void OnEnter(Session session)
		{
		}

		public void Update()
		{
		}

		public override void OnLeave(Session session)
		{
		}

		public void LoadInteractionStrip(Session session)
		{
		}

		protected override void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		protected override void AcceptPlacement(Session session)
		{
		}

		public void DeactivateInteractionStrip(Session session)
		{
		}
	}

	public class MoveBuildingInPlacement : MoveBuilding
	{
		public const string CURRENT_UI_EVENT = "currentUiEvt";

		private bool isTutorialPointerOnStrip;

		private bool m_bTouchBegan;

		private bool userCameraActive;

		public override void OnEnter(Session session)
		{
		}

		protected override void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		protected override void AcceptPlacement(Session session)
		{
		}
	}

	public class MoveBuildingPanningInEdit : State
	{
		private InteractionStripMixin interactionStrip;

		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}
	}

	public class MoveBuildingPanningInPlacement : State
	{
		private const string MOVEBUILDING_UI_HANDLER = "movebuildingpanninginplacement_ui";

		private InteractionStripMixin interactionStrip;

		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}
	}

	public class Movie : State
	{
		private string movie;

		private string nextSession;

		public string TheMovie
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public string NextSessionState
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}
	}

	public class Options : State
	{
		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}
	}

	public class Paving : State
	{
		private List<PaveAction.PaveElement> workingList;

		private Cost segmentCost;

		private Cost totalCost;

		private int placed;

		private int removed;

		private int cannotPay;

		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}
	}

	public class Placing : State
	{
		public const string FRESHLY_PURCHASED = "FreshlyPurchased";

		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}

		private bool PlayerCanAfford(Session session, Simulated simulated)
		{
			return false;
		}
	}

	public class Playing : State
	{
		public static string INVENTORY_ENTITY;

		public virtual void OnEnter(Session session)
		{
		}

		public virtual void OnLeave(Session session)
		{
		}

		public virtual void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		public virtual void OnUpdate(Session session)
		{
		}

		public void DisappearingResourceAmount(Vector2 screenPosition, int amount)
		{
		}

		public void SimulatedClick(Simulated clickedSim, Session session)
		{
		}

		protected virtual void CleanupTouchingSim(Session session)
		{
		}

		protected virtual void CleanupTouchingBubble(Session session)
		{
		}
	}

	public class ResolveUser : State
	{
		private SoaringContext LOAD_GAME_RESOLVE_CONTEXT;

		private SoaringContext MIGRATION_CONTEXT;

		private Dictionary<string, object> serverSave;

		private Dictionary<string, object> localSave;

		private SoaringPlayerResolver.SoaringPlayerData platform_account;

		private SoaringPlayerResolver.SoaringPlayerData last_account;

		private SoaringPlayerResolver.SoaringPlayerData device_account;

		private SaveGameScrollScreen saveGameScrollScreen;

		private SaveGameScreen saveGameScreen;

		private SaveGameScreen1 saveGameScreen1;

		private Dictionary<SoaringPlayerResolver.SoaringPlayerData, Dictionary<string, object>> deviceGameSaves;

		private string SocialNetworkMediaName;

		public void OnEnter(Session session)
		{
		}

		public void OnLoadRemoteGame(SoaringContext context)
		{
		}

		public void OnUpdate(Session session)
		{
		}

		private void RemoteGameReturned(Session session, SoaringContext context)
		{
		}

		private void LoadAndPresentAllPossibleGamestates(Session session, SoaringDictionary sessionSaveData)
		{
		}

		private void PresentGameOptions(Session session)
		{
		}

		private void alert(Session session)
		{
		}

		private void SelectSaveGame(Session session, Dictionary<string, object> selectedGame)
		{
		}

		private Dictionary<SoaringPlayerResolver.SoaringPlayerData, Dictionary<string, object>> gatherLocalDeviceSaves()
		{
			return null;
		}

		public void OnLeave(Session session)
		{
		}

		private void MigratePlayer(Session session, SoaringPlayerResolver.SoaringPlayerData sourceAccount, SoaringPlayerResolver.SoaringPlayerData targetAccount)
		{
		}

		public void OnMigrationComplete(SoaringContext context)
		{
		}

		public void ProcessMigrationResults(Session session, SoaringContext context)
		{
		}

		private void OnSaveComplete(SoaringContext context)
		{
		}

		private void UserResolutionComplete(Session session)
		{
		}
	}

	public class SelectedPlaying : Playing
	{
		public static string TASK_CHARACTER_SELECT;

		protected TimebarGroup timebarGroup;

		protected NamebarGroup m_pNamebarGroup;

		protected InteractionStripMixin interactionStrip;

		public override void OnEnter(Session session)
		{
		}

		public override void OnLeave(Session session)
		{
		}

		public override void OnUpdate(Session session)
		{
		}

		public override void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		private void DeactivateTimeBarAndInteractionStrip(Session session)
		{
		}
	}

	public class SellBuildingConfirmation : State
	{
		public void OnEnter(Session session)
		{
		}

		public void Setup(Session session)
		{
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}

		private void SellSimulated(Session session, Simulated toSell)
		{
		}
	}

	public class Shopping : State
	{
		private const string SHOPPING_UI_HANDLER = "shopping_ui";

		private static SBMarketOffer hackLastOffer;

		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public static void FireFinishShoppingEvent(Session session)
		{
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}
	}

	public class ShowingDialog : State
	{
		private void AdvanceToNextDialog(SBGUIScreen screen, Session session, SBGUIScreen dialog)
		{
		}

		private SBGUIScreen CreateCharacterDialog(CharacterDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			return null;
		}

		private SBGUIScreen CreateQuestDialog(QuestDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			return null;
		}

		private SBGUIScreen CreateLevelUpDialog(LevelUpDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			return null;
		}

		private SBGUIScreen CreateFoundItemDialog(FoundItemDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			return null;
		}

		private SBGUIScreen CreateFoundMovieDialog(FoundMovieDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			return null;
		}

		private SBGUIScreen CreateExplanationDialog(ExplanationDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			return null;
		}

		private SBGUIScreen CreateMoveInDialog(MoveInDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			return null;
		}

		private SBGUIScreen CreateTreasureDialog(TreasureDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			return null;
		}

		private SBGUIScreen CreateSpongyGamesDialog(SpongyGamesDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			return null;
		}

		private SBGUIScreen CreateRateMeDialog(RateMeDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			return null;
		}

		private SBGUIScreen CreateDailyBonusDialog(DailyBonusDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			return null;
		}

		private void CreateOrAdvanceDialog(DialogInputData inputData, SBGUIScreen screen, Session session)
		{
		}

		private SBGUIScreen CreateDialog(DialogInputData inputData, SBGUIScreen screen, Session session)
		{
			return null;
		}

		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		private QuestDefinition RetrieveQuestDefinition(Session session, uint questDid)
		{
			return null;
		}

		private List<ConditionDescription> RetrieveQuestConditionDescriptions(Session session, uint questDid)
		{
			return null;
		}
	}

	public class StashBuildingConfirmation : State
	{
		public void OnEnter(Session session)
		{
		}

		public void Setup(Session session)
		{
		}

		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}
	}

	public interface State
	{
		void OnEnter(Session session);

		void OnLeave(Session session);

		void OnUpdate(Session session);
	}

	public class Stopping : State
	{
		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}
	}

	public class Sync : State
	{
		private float mResyncStartTime;

		private bool mWasResync;

		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}

		private void Reauthenticate(Session session)
		{
		}

		private void ReloadFromDisk(Session session)
		{
		}

		private void ReloadToFriendsSession(Session session)
		{
		}

		private void ReloadFromNetwork(Session session)
		{
		}

		private void CleanUp(Session session)
		{
		}
	}

	public class UnitBusy : State
	{
		private const string UNIT_BUSY_UI_HANDLER = "unit_busy_ui";

		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}
	}

	public class UnitIdle : State
	{
		private const string UNIT_IDLE_UI_HANDLER = "unit_idle_ui";

		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}
	}

	public class Vending : State
	{
		private const string VENDING_UI_HANDLER = "vending_ui";

		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}

		private void CheckInstanceForJelly(Session session, VendingInstance instance)
		{
		}

		private void VendorRestockRush(Session session)
		{
		}

		private void Purchase(Session session, VendingInstance instance)
		{
		}

		private void Restock(Session session, Simulated simulated, bool refresh)
		{
		}
	}

	public class VisitGameStarting : State
	{
		private enum VisitStartingState
		{
			STATE_FIRST = -1,
			STATE_ASSEMBLE_GAME_STATE = 0,
			STATE_RETRIEVE_GAME_SAVE = 1,
			STATE_LOAD_ENTITY_BLUEPRINTS = 2,
			STATE_CREATE_GAME = 3,
			STATE_LOAD_ASSETS = 4,
			STATE_LOAD_ASSETS_TERRAIN = 5,
			STATE_LOAD_ASSETS_SIMULATION = 6,
			STATE_PRECACHE_GUI = 7,
			STATE_LOAD_ASSETS_TIME_DEPENDENTS = 8,
			STATE_LOAD_ASSETS_SEND_COMMANDS = 9,
			STATE_CREATE_TERRAIN_MESHES = 10,
			STATE_LOAD_ASSETS_ACTIVATE_QUESTS = 11,
			STATE_PROCESS_PENDING = 12,
			STATE_UNLOAD_UNUSED_ASSETS = 13,
			STATE_SETUP_SIMULATION = 14,
			STATE_LAST = 15,
			STATE_ERROR = 16
		}

		private delegate void ProcessStartingProgressState(Session session);

		private class VisitFriendSoaringDelegate : SoaringDelegate
		{
			public override void OnComponentFinished(bool success, string module, SoaringError error, SoaringDictionary data, SoaringContext context)
			{
			}

			public override void OnRequestingSessionData(bool success, SoaringError error, SoaringArray sessions, SoaringDictionary raw_data, SoaringContext context)
			{
			}
		}

		public const uint VISIT_FRIEND_QUEST_ID = 2400u;

		public const uint VISIT_FRIEND_DIALOG_ID = 2401u;

		private SoaringDictionary FRIEND_SAVE_GAME;

		private int currentState;

		private ProcessStartingProgressState[] processes;

		private int currentAdvance;

		private int precacheGUIState;

		private int loadTimeDependentsState;

		private StaticContentLoader contentLoader;

		public bool blockUpdates;

		private SBGUIElement loadingSpinner;

		public bool attempLoadPatchTown;

		private AssetServices.AssetServicesMonitor mUnloadAssetMonitor;

		public const string VISIT_STARTING_PROGRESS = "visit_starting_progress";

		private const string POLICY_BUTTON = "policy_button";

		private void OnGameCreated(Session session)
		{
		}

		private void DeferDialogs(Session session)
		{
		}

		public void OnLoadGameDelegate(SoaringContext context)
		{
		}

		private void LoadEntityBlueprints(Session session)
		{
		}

		private void CallLoadFromNetwork(Session session, bool isRetryAttempt = false)
		{
		}

		public void OnEnter(Session session)
		{
		}

		public void OnLeave(Session session)
		{
		}

		public static void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		public void OnUpdate(Session session)
		{
		}

		private void AdvanceState(Session session)
		{
		}

		private void RequestGameState(Session session)
		{
		}

		private void SaveFriendGameTimeStamp()
		{
		}

		private bool CheckFriendGameTimestamp()
		{
			return false;
		}

		private void GameRetrieved(SoaringContext context)
		{
		}

		private void CreateGame(Session session)
		{
		}

		public void DisplayFailedToLoadDialog(Session session)
		{
		}

		private void LoadAssets(Session session)
		{
		}

		private void CreateTerrainMeshes(Session session)
		{
		}

		private void AwaitProductInfo(Session session)
		{
		}

		private void ProcessTriggers(Session session)
		{
		}

		private void HandleUnusedAssets(Session session)
		{
		}

		private void SetupSimulation(Session session)
		{
		}

		private void AssembleGameState(Session session)
		{
		}

		private void LoadLocalAssetsTerrain(Session session)
		{
		}

		private void LoadLocalAssetsCreateSimulation(Session session)
		{
		}

		private void PrecacheGUI(Session session)
		{
		}

		private void LoadLocalAssetsLoadTimeDependents(Session session)
		{
		}

		private void LoadLocalAssetsSendPendingCommands(Session session)
		{
		}

		private void LoadLocalAssetsActivateQuests(Session session)
		{
		}

		private void CreateErrorDialog(Session session, string title, string message, string okButtonLabel, Action okHandler, float messageScale, float titleScale)
		{
		}
	}

	public class TimebarMixin
	{
		public const int YOFFSET = 20;

		public const int HEIGHT = 100;

		private SBGUITimebar timebarGUI;

		private string gameObjectID;

		private const string TIMEBAR = "Timebar";

		public bool IsActive
		{
			get
			{
				return false;
			}
		}

		public bool ActivateOnSelected(Session session, Simulated simulated, float yOffset = 20f)
		{
			return false;
		}

		public void DoRush(Session session, Simulated simulated, Action goBackToPlaying, Action goBackToPlayingCancel)
		{
		}

		public void Deactivate(Session session)
		{
		}

		public void Extend()
		{
		}

		private Vector2 GetRushButtonScreenPosition()
		{
			return default(Vector2);
		}
	}

	public class TimebarGroup
	{
		public const string TASK_SRC_UNIT = "TaskSrcUnit";

		private TimebarMixin taskAtBuildingTimebar;

		private TimebarMixin timebar;

		public bool IsActive
		{
			get
			{
				return false;
			}
		}

		public void ActivateOnSelected(Session session)
		{
		}

		public void Deactivate(Session session)
		{
		}

		public void Extend()
		{
		}
	}

	public class AcceptPlacementControl : BaseControlBinding
	{
		public AcceptPlacementControl()
		{
		}

		public AcceptPlacementControl(Action callback)
		{
		}

		public override void DynamicUpdate(Session session)
		{
		}

		private void OnClick(Session session)
		{
		}
	}

	public class BrowseRecipesControl : BaseControlBinding
	{
		public BrowseRecipesControl(Simulated toBrowse)
		{
		}

		private void OnClick(Session session, Simulated toBrowse)
		{
		}
	}

	public class ClearDebrisControl : BaseControlBinding
	{
		public ClearDebrisControl(Simulated toClear)
		{
		}

		private void OnClick(Session session, Simulated toClear)
		{
		}

		public override void DynamicUpdate(Session session)
		{
		}
	}

	public static class PushForPlacementHelper
	{
		public static void PushPlacementConfirmation(Session session, Simulated subject)
		{
		}
	}

	public class RejectControl : BaseControlBinding
	{
		public RejectControl()
		{
		}

		public RejectControl(Action callback)
		{
		}

		private void OnClick(Session session)
		{
		}
	}

	public class RotateControl : BaseControlBinding
	{
		private bool isEnabled;

		public RotateControl(Simulated toRotate, bool isEnabled, Simulation simulation = null)
		{
		}

		private void OnClick(Session session, Simulated toRotate, Simulation simulation)
		{
		}

		public override void DynamicUpdate(Session session)
		{
		}
	}

	public class RushControl : BaseControlBinding
	{
		public RushControl(Simulated toRush)
		{
		}

		private void OnClick(Session session, Simulated toSell)
		{
		}
	}

	public class SellControl : BaseControlBinding
	{
		private bool isEnabled;

		public SellControl(Simulated toSell, bool isEnabled, string sellError)
		{
		}

		private void OnClick(Session session, Simulated toSell, string sellError)
		{
		}

		public override void DynamicUpdate(Session session)
		{
		}
	}

	public class StashControl : BaseControlBinding
	{
		private bool isEnabled;

		public StashControl(Simulated toStash, bool isEnabled, string stashError)
		{
		}

		private void OnClick(Session session, Simulated toStash, string stashError)
		{
		}

		public override void DynamicUpdate(Session session)
		{
		}
	}

	public class SelectedStateTransition : BaseTransitionBinding
	{
		private string targetState;

		public SelectedStateTransition(Simulated targetSim, string state)
		{
		}

		private void OnClick(Session session, Simulated targetSim)
		{
		}
	}

	public class BrowseRecipesTransition : SelectedStateTransition
	{
		public BrowseRecipesTransition(Simulated targetSim)
			: base(null, null)
		{
		}
	}

	public class VendingTransition : SelectedStateTransition
	{
		public VendingTransition(Simulated targetSim)
			: base(null, null)
		{
		}
	}

	public class UnitIdleTransition : SelectedStateTransition
	{
		public UnitIdleTransition(Simulated targetSim)
			: base(null, null)
		{
		}
	}

	public class UnitBusyTransition : SelectedStateTransition
	{
		public UnitBusyTransition(Simulated targetSim)
			: base(null, null)
		{
		}
	}

	public class ShowTreasureRewardTransition : BaseTransitionBinding
	{
		public ShowTreasureRewardTransition(Simulated toShow)
		{
		}

		private void OnClick(Session session, Simulated toShow)
		{
		}
	}

	private static Dictionary<string, State> states;

	private SBGamePersister gameSaver;

	private SBTransactionMonitor transactionMonitor;

	private CallbackQueue callbackQueue;

	public PlayHavenController PlayHavenController;

	public SBAnalytics analytics;

	public SBContentPatcher contentPatcher;

	public bool notifyOnDisconnect;

	public bool gameInitialized;

	private bool _reinitializeSession;

	private bool _resyncConnection;

	public bool gameIsReloading;

	public static bool PatchyTownGame;

	public bool WasInFriendsGame;

	public bool musicStateBeforePT;

	public bool sfxStateBeforePT;

	public bool haveReloaded;

	public PushNotificationManager pushNotificationManager;

	public GameObject statisticsTracker;

	public SBStatisticsTracker tracker;

	private static ulong logDumpShake;

	private bool lastOnlineState;

	private bool isShowingOfflineDialog;

	public bool canChangeState;

	private bool checkForPatching;

	private bool justCheckForUpdates;

	public SoaringArray soaringEvents;

	public DateTime lastResetTime;

	private ulong? m_ulPauseTimestamp;

	private AndroidJavaObject androidActivity;

	public FramerateWatcher framerateWatcher;

	private const string CHECK_PATCHING = "CheckPatching";

	private const string GAME_STARTING = "GameStarting";

	private const string GAME_STOPPING = "GameStopping";

	private const string AUTHORIZING = "Authorizing";

	public const string PLAYING = "Playing";

	public const string SELECTED_PLAYING = "SelectedPlaying";

	private const string EDITING = "Editing";

	private const string MOVE_IN_EDIT = "MoveBuildingInEdit";

	private const string MOVE_IN_PLACEMENT = "MoveBuildingInPlacement";

	private const string MOVE_PANNING_IN_EDIT = "MoveBuildingPanningInEdit";

	private const string MOVE_PANNING_IN_PLACEMENT = "MoveBuildingPanningInPlacement";

	private const string PLACING = "Placing";

	private const string PAVING = "Paving";

	private const string DRAG_FEEDING = "DragFeeding";

	private const string SHOPPING = "Shopping";

	private const string INVENTORY = "Inventory";

	private const string COMMUNITY_EVENT = "CommunityEvent";

	public const string BROWSING_RECIPES = "BrowsingRecipes";

	private const string SYNC = "Sync";

	private const string STOPPING = "Stopping";

	private const string IN_APP_PURCHASING = "InAppPurchasing";

	private const string SHOWING_DIALOG = "ShowingDialog";

	private const string HARD_SPEND_CONFIRM = "HardSpendConfirm";

	private const string HARD_SPEND_PASSTHROUGH = "HardSpendPassthrough";

	private const string INSUFFICIENT_DIALOG = "InsufficientDialog";

	private const string EXPANSION = "Expansion";

	private const string EXPANDING = "Expanding";

	private const string CLEARING = "Clearing";

	private const string OPTIONS = "Options";

	private const string MOVIE = "Movie";

	private const string DEBUG = "Debug";

	private const string ERROR_DIALOG = "ErrorDialog";

	private const string GET_JELLY = "GetJelly";

	private const string CREDITS = "Credits";

	private const string MOVIE_START_TIME = "MovieStartTime";

	private const string SELL_BUILDING_CONFIRMATION = "SellBuildingConfirmation";

	private const string STASH_BUILDING_CONFIRMATION = "StashBuildingConfirmation";

	public const string VENDING = "vending";

	public const string UNIT_IDLE = "UnitIdle";

	public const string UNIT_BUSY = "UnitBusy";

	public const string AGE_GATE = "AgeGate";

	public const string HELP = "Help";

	public const string FBAGEGATE = "FBAgeGate";

	public const string FACEBOOKLOGIN = "FacebookLogin";

	public const string FACEBOOKLOGOUT = "FacebookLogout";

	private State state;

	private bool saveGame;

	private Player player;

	private Game game;

	private SBCamera camera;

	private SBWebFileServer webFileServer;

	private SBAuth auth;

	private static DebugManager debugManager;

	private float lastUpdateTime;

	private int currentVersion;

	private List<GameloopAction> actions;

	public MusicManager musicManager;

	private SoundEffectManager soundEffectManager;

	private SBUIBuilder.ScreenContext simulationContext;

	private SBGUIScreen simulationScratchScreen;

	private SBUIBuilder.ScreenContext currentGuiContext;

	private List<StateChangeRequest> queuedStateChanges;

	private Dictionary<string, TFServer.JsonResponseHandler> externalRequests;

	private SessionProperties properties;

	private Dictionary<string, object> asyncRequests;

	private Dictionary<string, TFWebClient> asyncFileRequests;

	private BubbleSwipeParticleSystemRequestDelegate bubbleSwipeParticleSystemRequestDelegate;

	private ConfettiSwipeParticleSystemRequestDelegate confettiSwipeParticleSystemRequestDelegate;

	private BalloonSwipeParticleSystemRequestDelegate balloonSwipeParticleSystemRequestDelegate;

	private SeaflowerSwipeParticleSystemRequestDelegate seaFlowerSwipeParticleSystemRequestDelegate;

	private FogEffectRequestDelegate fogEffectRequestDelegate;

	private TapFXParticleSystemRequestDelegate tapFXParticleSystemRequestDelegate;

	protected const string ERROR_MESSAGE_TITLE = "error_message_title";

	protected const string ERROR_MESSAGE = "error_message";

	protected const string ERROR_MESSAGE_OK_LABEL = "error_message_ok_label";

	protected const string ERROR_MESSAGE_OK_ACTION = "error_message_ok_action";

	protected const string ERROR_MESSAGE_SCALE = "error_message_scale";

	protected const string JELLY_MESSAGE_TITLE = "jelly_message_title";

	protected const string JELLY_MESSAGE = "jelly_message";

	protected const string JELLY_QUESTION = "jelly_question";

	protected const string JELLY_MESSAGE_OK_LABEL = "jelly_message_ok_label";

	protected const string JELLY_MESSAGE_CANCEL_LABEL = "jelly_message_cancel_label";

	protected const string JELLY_MESSAGE_OK_ACTION = "jelly_message_ok_action";

	protected const string JELLY_MESSAGE_CANCEL_ACTION = "jelly_message_cancel_action";

	protected const string INSUFFICIENT_ITEM_DID = "insufficient_item_did";

	protected const string INSUFFICIENT_ITEMNAME = "insufficient_itemname";

	protected const string INSUFFICIENT_RESOURCES = "insufficient_resources";

	protected const string INSUFFICIENT_CANCEL = "insufficient_cancel";

	protected const string INSUFFICIENT_ACCEPT = "insufficient_accept";

	private const string USER_LOGIN = "userLogin";

	private const string EXPANDING_UI_HANDLER = "expanding_ui";

	protected const string EXPANSION_SCREEN = "expansion";

	private const string EXPANSION_UI_HANDLER = "expansion_ui";

	private static readonly object expandLock;

	public static bool didRegisterNotifications;

	private const string STARTING_PROGRESS = "starting_progress";

	private const int TERRAIN_DEPTH = 5;

	private static bool loggingTimedDependents;

	public const string TARGET_STORE_TAB = "target_store_tab";

	public const string TARGET_STORE_DID = "target_store_did";

	public const string CURRENT_UI_EVENT = "CurrentGuiEventInfo";

	protected const string IN_STATE_MOVE_IN_EDIT = "in_state_move_in_edit";

	private const string DIALOGS_TO_SHOW = "dialogs_to_show";

	private const string PLAYING_UI_HANDLER = "playing_ui";

	public const string STANDARD_SCREEN = "standard_screen";

	public const string LEVELUP_SCREEN = "levelup_screen";

	public const string CLEAR_PURCHASE_ON_MOVEMENT = "clear_purchase_on_movement";

	private static bool draggingCamera;

	private static bool fogEnabled;

	private const string RESOLVE_USER = "resolve_user";

	public const string TRANSACTION_OFFER = "transaction_offer";

	public const string STORE_OPEN_TYPE = "store_open_type";

	public bool marketpalceActive;

	public const bool DEBUG_LOG = true;

	private const string VISIT_FRIEND_STARTING = "visit_friend";

	protected const string TO_SELL = "to_sell";

	protected const string SELL_ERROR = "sell_error";

	protected const string TO_STASH = "to_stash";

	protected const string STASH_ERROR = "stash_error";

	public SBTransactionMonitor TransactionMonitor
	{
		get
		{
			return null;
		}
	}

	public bool reinitializeSession
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public bool resyncConnection
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public bool InFriendsGame
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public bool IsAgeAppropriate
	{
		get
		{
			return false;
		}
	}

	public SBWebFileServer WebFileServer
	{
		get
		{
			return null;
		}
	}

	public SBAnalytics Analytics
	{
		get
		{
			return null;
		}
	}

	public static DebugManager TheDebugManager
	{
		get
		{
			return null;
		}
	}

	public SBAuth Auth
	{
		get
		{
			return null;
		}
	}

	public Game TheGame
	{
		get
		{
			return null;
		}
	}

	public CallbackQueue CallbackQueue
	{
		get
		{
			return null;
		}
	}

	public SBCamera TheCamera
	{
		get
		{
			return null;
		}
	}

	public Player ThePlayer
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public SoundEffectManager TheSoundEffectManager
	{
		get
		{
			return null;
		}
	}

	public State TheState
	{
		get
		{
			return null;
		}
	}

	public SBGUIScreen SimulationSBGUIScreen
	{
		get
		{
			return null;
		}
	}

	public string NextState { get; set; }

	static Session()
	{
	}

	public Session(int currentVersion)
	{
	}

	public bool IsOnline()
	{
		return false;
	}

	public void ClearUserState()
	{
	}

	public void SaveGame()
	{
	}

	public void GameInitialized(bool initialized)
	{
	}

	public void OnPause(bool paused)
	{
	}

	public void PurchasePremiumProduct(string productIdentifier)
	{
	}

	public void StopGameSaveTimer()
	{
	}

	public void OnApplicationQuit()
	{
	}

	public void OnApplicationFocus(bool bFocus)
	{
	}

	public void HandleReset(bool forceReset)
	{
	}

	public void OnTestContextResponder(SoaringContext context)
	{
	}

	public void CheckForPatching(bool checkForUpdates)
	{
	}

	private void OnPatchingEvent(string eventStr)
	{
	}

	public bool IsPatchingInProgress()
	{
		return false;
	}

	public void ChangeState(string state, bool newContext = true)
	{
	}

	private void SetState(StateChangeRequest request)
	{
	}

	public void ProcessStateChanges()
	{
	}

	public void CheckForPatchingUpdate()
	{
	}

	public void OnUpdate()
	{
	}

	public void GiveSoaringReward(SoaringEvent.SoaringEventAction reward)
	{
	}

	private void PopulateRewardDict(string prefix, Dictionary<string, object> dict, string rewardName, int quantity)
	{
	}

	public void SetupPlayer(SoaringContext context)
	{
	}

	public void AddAction(GameloopAction action)
	{
	}

	public int GetLocalVersion()
	{
		return 0;
	}

	public void DropGame()
	{
	}

	public bool PlayerIsLoggedIn()
	{
		return false;
	}

	public void onExternalMessage(string msg)
	{
	}

	public void RegisterExternalCallback(string requestId, TFServer.JsonResponseHandler callback)
	{
	}

	public void unregisterExternalCallback(string requestId, TFServer.JsonResponseHandler callback)
	{
	}

	public AndroidJavaObject getAndroidActivity()
	{
		return null;
	}

	protected void CheckInventorySoftLock()
	{
	}

	public void AddAsyncResponse(string key, object val)
	{
	}

	public void AddAsyncResponse(string key, object val, bool warnIfDuplicate)
	{
	}

	public object CheckAsyncRequest(string key)
	{
		return null;
	}

	public TFServer.JsonResponseHandler AsyncResponder(string key)
	{
		return null;
	}

	public void AddAsyncFileResponse(string key, TFWebClient val)
	{
	}

	public TFWebClient CheckAsyncFileRequest(string key)
	{
		return null;
	}

	public TFWebClient.GetCallbackHandler AsyncFileResponder(string key)
	{
		return null;
	}

	public void ClearAsyncRequests()
	{
	}

	public void PlayBubbleScreenSwipeEffect()
	{
	}

	public void PlayConfettiScreenSwipeEffect()
	{
	}

	public void PlaySeaflowerAndBubbleScreenSwipeEffect()
	{
	}

	public void PlayTapParticleEffect(Vector3 position)
	{
	}

	public void PlayFogParticleEffects()
	{
	}

	private void InitScreenSwipeEffects()
	{
	}

	public void ErrorMessageHandler(Session session, string title, string message, string okButtonLabel, Action okAction, float messageScale = 1f)
	{
	}

	public void GetJellyHandler(Session session, string title, string message, string question, string okButtonLabel, string cancelButtonLabel, Action okAction, Action cancelAction)
	{
	}

	public void InsufficientResourcesHandler(Session session, string itemName, int itemDID, Action okAction, Action cancelAction, Cost insufficientCost)
	{
	}

	public static Simulated FindBestSimulatedUnderPoint(Prioritizer prioritizer, Simulation simulation, SBCamera camera, Vector2 screenPos, out Ray rayCast)
	{
		rayCast = default(Ray);
		return null;
	}

	public static Simulated FindBestSimulatedUnderPoint(Prioritizer prioritizer, Predicate<Simulated> filterOutMatching, Simulation simulation, SBCamera camera, Vector2 screenPos, out Ray rayCast)
	{
		rayCast = default(Ray);
		return null;
	}

	public static List<Simulated> FindSimulatedsUnderPoint(Predicate<Simulated> filterOutMatching, Simulation simulation, SBCamera camera, Vector2 screenPos, out Ray rayCast)
	{
		rayCast = default(Ray);
		return null;
	}

	public static Simulated FindAlreadySelected(Predicate<Simulated> filterOutMatching, Simulation simulation, SBCamera camera, Vector2 screenPos, out Ray rayCast, Simulated selected)
	{
		rayCast = default(Ray);
		return null;
	}

	private static void ChangeToResolveSessionStateOnStartup(Session session)
	{
	}

	private static void RegisterForLocalNotifications()
	{
	}

	protected void PlayMovie(string movie, string nextSession)
	{
	}

	public void PlayMovieFromInventory(string movie)
	{
	}

	public void PlayMovieFromPlaying(string movie)
	{
	}

	public void PlayMovieFromShowingDialog(string movie)
	{
	}

	public static void TryGrabSimulated(Session session, SBGUIEvent evt)
	{
	}

	public static bool TryGrabSimulated(Session session, List<Simulated> candidateSimulateds, SBGUIEvent evt)
	{
		return false;
	}

	public static bool TryGrabSimulated(Session session, Simulated candidateSimulated, SBGUIEvent evt)
	{
		return false;
	}
}
