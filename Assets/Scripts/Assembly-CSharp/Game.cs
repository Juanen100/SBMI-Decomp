using System;
using System.Collections;
using System.Collections.Generic;

public class Game
{
	public class GameSoaringResponder : SoaringDelegate
	{
		public override void OnRequestingSessionData(bool success, SoaringError error, SoaringArray sessions, SoaringDictionary raw_data, SoaringContext context)
		{
		}

		public override void OnSavingSessionData(bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
		{
		}

		public override void OnComponentFinished(bool success, string module, SoaringError error, SoaringDictionary data, SoaringContext context)
		{
		}
	}

	private class LoadSlotObjectData
	{
		public TerrainSlot expansion;

		public TerrainSlotObject slotObject;

		public ulong utcNow;

		public LoadSlotObject loader;

		public LoadSlotObjectData(TerrainSlot s, TerrainSlotObject o, ulong u, LoadSlotObject l)
		{
		}

		public void Load()
		{
		}
	}

	public delegate void GamestateWriter(Dictionary<string, object> gameState);

	private delegate void ObjectLoaderFn(Dictionary<string, object> data, ulong utcNow);

	private delegate void LoadSlotObject(TerrainSlot expansion, TerrainSlotObject slotObject, ulong utcNow);

	public const string PLAYTIME = "playtime";

	public EntityManager entities;

	public ItemDropManager dropManager;

	public CraftingManager craftManager;

	public VendingManager vendingManager;

	public TreasureManager treasureManager;

	public PaytableManager paytableManager;

	public FeatureManager featureManager;

	public BuildingUnlockManager buildingUnlockManager;

	public MovieManager movieManager;

	public CommunityEventManager communityEventManager;

	public TaskManager taskManager;

	public MicroEventManager microEventManager;

	public CostumeManager costumeManager;

	public WishTableManager wishTableManager;

	public Terrain terrain;

	public Border border;

	public Simulation simulation;

	public Simulated selected;

	public PersistedActionBuffer actionBuffer;

	public Player player;

	public ResourceManager resourceManager;

	public LevelingManager levelingManager;

	public ResourceCalculatorManager resourceCalculatorManager;

	public PlayerInventory inventory;

	public SessionActionManager sessionActionManager;

	public TriggerRouter triggerRouter;

	public DialogPackageManager dialogPackageManager;

	public QuestManager questManager;

	public AutoQuestDatabase autoQuestDatabase;

	public NotificationManager notificationManager;

	public Catalog catalog;

	public RewardCap rewardCap;

	public RmtStore store;

	public SBAnalytics analytics;

	public PlaytimeRegistrar playtimeRegistrar;

	public PlayHavenController playHavenController;

	public SBUpsightADManager upsightADManager;

	public bool CanSave;

	public bool needsReloadErrorDialog;

	public bool needsNetworkDownErrorDialog;

	public bool tutorialLocked;

	public const string GAME_FILE = "game.json";

	public Dictionary<string, object> gameState;

	private List<Action<Game>> sessionStateChangeObservers;

	private string gameFile;

	private bool needsReload;

	private bool loadFriendGame;

	private bool pendingReload;

	private Action[] loadSimulationActions;

	private IEnumerator loadSimulationActionsEnumerator;

	private IEnumerator loadExpansionSlotObjectsEnumerator;

	private float m_fLocalSaveTimer;

	private const float m_fLocalSaveTimeLength = 30f;

	private bool m_bNeedsLocalSave;

	public Game(SBAnalytics analytics, Dictionary<string, object> gameState, Player p, StaticContentLoader contentLoader, PersistedActionBuffer actBuffer, PlayHavenController phController)
	{
	}

	public static bool GameExists(Player p)
	{
		return false;
	}

	public static bool GameCacheExists(string playerName)
	{
		return false;
	}

	public static string GamePath(Player p)
	{
		return null;
	}

	public static string GameCachePath(string playerName)
	{
		return null;
	}

	public static Game CreateNew(SBAnalytics analytics, Player p, StaticContentLoader contentLoader, out int performedMigration, PlayHavenController phController)
	{
		performedMigration = default(int);
		return null;
	}

	public static Game LoadFromCache(Player p, SBAnalytics analytics, StaticContentLoader contentLoader, out int performedMigration, PlayHavenController phController)
	{
		performedMigration = default(int);
		return null;
	}

	public static SoaringContext CreateSoaringGameResponderContext(SoaringContextDelegate del)
	{
		return null;
	}

	public static void LoadFromNetwork(string userID, long timestamp, SoaringContext context, Session session)
	{
	}

	public static Game LoadFromDataDict(Dictionary<string, object> data, SBAnalytics analytics, Player p, StaticContentLoader contentLoader, out int performedMigration, PlayHavenController phController)
	{
		performedMigration = default(int);
		return null;
	}

	public static bool IsValidState(Dictionary<string, object> data)
	{
		return false;
	}

	public void DestroyCache()
	{
	}

	public void Clear()
	{
	}

	private void LoadVariables()
	{
	}

	public void LoadSimulation(ulong utcNow)
	{
	}

	public bool IterateLoadSimulation()
	{
		return false;
	}

	public string LoadActions(ulong utcNow, bool applyAction, bool forceSave)
	{
		return null;
	}

	public void ClearActionBuffer()
	{
	}

	public void SaveToServer(Session session, ulong utcNow, bool applyActions, bool forceSave)
	{
	}

	public void OnSaveGameData(SoaringContext context)
	{
	}

	public void AddTimeToSimulation(ulong nSeconds)
	{
	}

	public void FastForwardSimulationBegun()
	{
	}

	public void FastForwardSimulationFinished()
	{
	}

	private void LoadDebrisSlotObject(TerrainSlot expansion, TerrainSlotObject debrisObject, ulong utcNow)
	{
	}

	private void LoadLandmarkSlotObject(TerrainSlot expansion, TerrainSlotObject landmarkObject, ulong utcNow)
	{
	}

	public void LoadExpansions(ulong utcNow)
	{
	}

	public bool IterateLoadExpansions()
	{
		return false;
	}

	public void LocalSaveCheck(float fDeltaTime)
	{
	}

	public string SaveLocally(ulong timestamp, bool skipSave = false, bool skipWrite = false, bool useStaged = false)
	{
		return null;
	}

	public string LastAction()
	{
		return null;
	}

	public void LockedGameStateChange(GamestateWriter writer)
	{
	}

	public void RequestLoadFriendPark(string park)
	{
	}

	public bool ReloadToFriendPark()
	{
		return false;
	}

	public void ClearLoadFriendPark()
	{
	}

	public void RequestReload()
	{
	}

	public bool RequiresReload()
	{
		return false;
	}

	public void ClearReloadRequest()
	{
	}

	public void SetPendingReload(bool rr)
	{
	}

	public bool PendingReload()
	{
		return false;
	}

	public void NULL_ModifyStateSimulated(Simulated simulated, PersistedSimulatedAction action)
	{
	}

	public void ModifyGameStateSimulated(Simulated simulated, PersistedSimulatedAction action)
	{
	}

	public void NULL_ModifyGameState(PersistedTriggerableAction action)
	{
	}

	public void ModifyGameState(PersistedTriggerableAction action)
	{
	}

	public void LoadLastRandomQuestId()
	{
	}

	public void LoadLastAutoQuestId()
	{
	}

	public int GetResidentPopulation()
	{
		return 0;
	}

	private void ModifyGameStateHelper(PersistedTriggerableAction action, Dictionary<string, object> data)
	{
	}

	public void Record(PersistedTriggerableAction action)
	{
	}

	public void ApplyReward(Reward reward, ulong buildingCompleteTime, bool bDoAnalytics = true)
	{
	}

	public void OnChangeSessionState()
	{
	}

	public SBMIAnalytics.CommonData GetAnalyticsCommonData()
	{
		return default(SBMIAnalytics.CommonData);
	}

	public SBMIAnalytics.PlayerObject GetAnalyticsPlayerObject()
	{
		return null;
	}

	public ulong FirstPlayTime()
	{
		return 0uL;
	}

	public SBMIAnalytics.MetaObject GetAnalyticsMetaObject(string sEventName, int nOverrideTrackingVersion = -1)
	{
		return null;
	}

	public static SBMIDeltaDNA.DeviceObject GetDeltaDNADeviceObject()
	{
		return null;
	}

	public SBMIDeltaDNA.PlayerObject GetDeltaDNAPlayerObject()
	{
		return null;
	}

	private void LoadTerrain()
	{
	}

	private void LoadRecipes()
	{
	}

	private void LoadFeatureUnlocks()
	{
	}

	private void LoadBuildingUnlocks()
	{
	}

	private void LoadTreasureState()
	{
	}

	private void LoadRewardCaps()
	{
	}

	private void LoadMovies()
	{
	}

	private void LoadDropPickups()
	{
	}

	private void SaveVersionInfo()
	{
	}

	private void LoadResources()
	{
	}

	private void LoadPlaytime(Dictionary<string, object> gameState, ResourceManager resourceMgr)
	{
	}

	private void LoadActionsFromList(List<PersistedActionBuffer.PersistedAction> actions, ulong utcNow, bool applyAction)
	{
	}

	private void LoadUnits(ulong utcNow)
	{
	}

	private void LoadWanderers(ulong utcNow)
	{
	}

	private void LoadBuildings(ulong utcNow)
	{
	}

	private void LoadDebris(ulong utcNow)
	{
	}

	private void LoadTreasures(ulong utcNow)
	{
	}

	private void LoadLandmarks(ulong utcNow)
	{
	}

	private void LoadTasks(ulong utcNow)
	{
	}

	private void LoadTaskCompletions()
	{
	}

	private void LoadMicroEvents(ulong utcNow)
	{
	}

	private void LoadQuests(ulong utcNow)
	{
	}

	private void LoadQuestDefinitions(ulong utcNow)
	{
	}

	private void LoadCraftings(ulong utcNow)
	{
	}

	private void LoadVending(ulong utcNow)
	{
	}

	private void LoadObjects(string key, ObjectLoaderFn objectLoader, ulong utcNow)
	{
	}

	private void LoadUnit(Dictionary<string, object> dict, ulong utcNow)
	{
	}

	private void LoadWanderer(Dictionary<string, object> dict, ulong utcNow)
	{
	}

	private bool BuildingIsInventory(Dictionary<string, object> dict)
	{
		return false;
	}

	private void LoadBuilding(Dictionary<string, object> dict, ulong utcNow)
	{
	}

	private void LoadLandmark(Dictionary<string, object> dict, ulong utcNow)
	{
	}

	private void LoadDebris(Dictionary<string, object> dict, ulong utcNow)
	{
	}

	private void LoadTreasure(Dictionary<string, object> dict, ulong utcNow)
	{
	}

	private void LoadTask(Dictionary<string, object> pDict, ulong utcNow)
	{
	}

	private void LoadMicroEvent(Dictionary<string, object> pDict, ulong utcNow)
	{
	}

	private void LoadCostumes()
	{
	}

	private void LoadQuest(Dictionary<string, object> dict, ulong utcNow)
	{
	}

	private void LoadQuestDefinition(Dictionary<string, object> pDict, ulong nUtcNow)
	{
	}

	private void LoadCrafting(Dictionary<string, object> dict, ulong utcNow)
	{
	}

	private void LoadVendor(Dictionary<string, object> dict, ulong utcNow)
	{
	}

	private void PatchReferences()
	{
	}
}
