#define ASSERTS_ON
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DeltaDNA;
using MiniJSON;
using UnityEngine;

public class Game
{
	public class GameSoaringResponder : SoaringDelegate
	{
		public override void OnRequestingSessionData(bool success, SoaringError error, SoaringArray sessions, SoaringDictionary raw_data, SoaringContext context)
		{
			if (context.ContextResponder != null)
			{
				context.addValue(success, "status");
				if (error != null)
				{
					context.addValue(error, "error_message");
				}
				if (sessions != null)
				{
					context.addValue(sessions, "custom");
				}
				context.ContextResponder(context);
			}
			else
			{
				SoaringDebug.Log("Game: OnRequestingSessionData: No Return Context");
			}
		}

		public override void OnSavingSessionData(bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
		{
			if (context != null && context.ContextResponder != null)
			{
				context.addValue(success, "status");
				if (error != null)
				{
					context.addValue(error, "error_message");
				}
				if (data != null)
				{
					context.addValue(data, "custom");
				}
				context.ContextResponder(context);
			}
			SoaringDebug.Log("Game: OnSavingSessionData: " + success);
		}

		public override void OnComponentFinished(bool success, string module, SoaringError error, SoaringDictionary data, SoaringContext context)
		{
			if (context != null)
			{
				context.addValue(success, "status");
				context.addValue(module, "module");
				if (error != null)
				{
					context.addValue(error, "error_message");
				}
				if (data != null)
				{
					context.addValue(data, "custom");
				}
				context.ContextResponder(context);
			}
			SoaringDebug.Log("Game: OnComponentFinished: " + success);
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
			expansion = s;
			slotObject = o;
			utcNow = u;
			loader = l;
		}

		public void Load()
		{
			loader(expansion, slotObject, utcNow);
		}
	}

	public delegate void GamestateWriter(Dictionary<string, object> gameState);

	private delegate void ObjectLoaderFn(Dictionary<string, object> data, ulong utcNow);

	private delegate void LoadSlotObject(TerrainSlot expansion, TerrainSlotObject slotObject, ulong utcNow);

	public const string PLAYTIME = "playtime";

	public const string GAME_FILE = "game.json";

	private const float m_fLocalSaveTimeLength = 30f;

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

	public bool CanSave;

	public volatile bool needsReloadErrorDialog;

	public volatile bool needsNetworkDownErrorDialog;

	public volatile bool tutorialLocked;

	public Dictionary<string, object> gameState;

	private List<Action<Game>> sessionStateChangeObservers;

	private string gameFile;

	private volatile bool needsReload;

	private volatile bool loadFriendGame;

	private volatile bool pendingReload;

	private Action[] loadSimulationActions;

	private IEnumerator loadSimulationActionsEnumerator;

	private IEnumerator loadExpansionSlotObjectsEnumerator;

	private float m_fLocalSaveTimer;

	private bool m_bNeedsLocalSave;

	public Game(SBAnalytics analytics, Dictionary<string, object> gameState, Player p, StaticContentLoader contentLoader, PersistedActionBuffer actBuffer, PlayHavenController phController)
	{
		TFUtils.Assert(IsValidState(gameState), "Game.ctor received invalid gamestate. Try to catch where the invalid state was created and fix it!");
		playHavenController = phController;
		this.gameState = gameState;
		actionBuffer = actBuffer;
		gameFile = p.CacheFile("game.json");
		player = p;
		resourceManager = contentLoader.TheResourceManager;
		this.analytics = analytics;
		levelingManager = contentLoader.TheLevelingManager;
		SaveVersionInfo();
		LoadResources();
		LoadPlaytime(this.gameState, resourceManager);
		inventory = new PlayerInventory();
		rewardCap = new RewardCap();
		entities = contentLoader.TheEntityManager;
		dialogPackageManager = new DialogPackageManager(gameState);
		sessionActionManager = new SessionActionManager();
		craftManager = contentLoader.TheCraftingManager;
		vendingManager = contentLoader.TheVendingManager;
		paytableManager = contentLoader.ThePaytableManager;
		featureManager = contentLoader.TheFeatureManager;
		buildingUnlockManager = contentLoader.TheBuildingUnlockManager;
		treasureManager = contentLoader.TheTreasureManager;
		communityEventManager = contentLoader.TheCommunityEventManager;
		taskManager = contentLoader.TheTaskManager;
		microEventManager = contentLoader.TheMicroEventManager;
		costumeManager = contentLoader.TheCostumeManager;
		wishTableManager = contentLoader.TheWishTableManager;
		movieManager = contentLoader.TheMovieManager;
		dropManager = new ItemDropManager();
		catalog = contentLoader.TheCatalog;
		autoQuestDatabase = contentLoader.TheAutoQuestDatabase;
		questManager = contentLoader.TheQuestManager;
		questManager.SetDialogManager(dialogPackageManager);
		notificationManager = new NotificationManager();
		triggerRouter = new TriggerRouter(new List<ITriggerObserver> { questManager, notificationManager, sessionActionManager });
		sessionStateChangeObservers = new List<Action<Game>>();
		sessionStateChangeObservers.Add(sessionActionManager.RequestProcess);
		terrain = contentLoader.TheTerrain;
		border = contentLoader.TheBorder;
		resourceManager.UpdateProductGroups(craftManager);
		TFUtils.DebugTimeOffset = 0uL;
		TFUtils.FastForwardOffset = 0uL;
		TFUtils.AddTimeOffset = 0uL;
		if (gameState.ContainsKey("playtime"))
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["playtime"];
			object value = null;
			if (dictionary.TryGetValue("adjusted_debug_time", out value) && value != null)
			{
				ulong num = 0uL;
				Type type = value.GetType();
				if (type == typeof(ulong))
				{
					num = (ulong)value;
				}
				else if (type == typeof(double))
				{
					num = Convert.ToUInt64((double)value);
				}
				else if (type == typeof(uint))
				{
					num = Convert.ToUInt64((uint)value);
				}
				else if (type == typeof(long))
				{
					num = Convert.ToUInt64((long)value);
				}
				TFUtils.DebugTimeOffset = num;
				TFUtils.FastForwardOffset = num;
			}
			object value2 = null;
			if (dictionary.TryGetValue("debug_add_time", out value2) && value2 != null)
			{
				ulong addTimeOffset = 0uL;
				Type type2 = value2.GetType();
				if (type2 == typeof(ulong))
				{
					addTimeOffset = (ulong)value2;
				}
				else if (type2 == typeof(double))
				{
					addTimeOffset = Convert.ToUInt64((double)value2);
				}
				else if (type2 == typeof(uint))
				{
					addTimeOffset = Convert.ToUInt64((uint)value2);
				}
				else if (type2 == typeof(long))
				{
					addTimeOffset = Convert.ToUInt64((long)value2);
				}
				TFUtils.AddTimeOffset = addTimeOffset;
			}
		}
		LoadVariables();
		AutoQuestDatabase.SetPreviousAutoQuestDataFramGameState(gameState);
		m_fLocalSaveTimer = 0f;
		m_bNeedsLocalSave = false;
	}

	public static bool GameExists(Player p)
	{
		return TFUtils.FileIsExists(p.CacheFile("game.json"));
	}

	public static bool GameCacheExists(string playerName)
	{
		return TFUtils.FileIsExists(Player.PlayerCacheFile(playerName, "game.json"));
	}

	public static string GamePath(Player p)
	{
		return p.CacheFile("game.json");
	}

	public static string GameCachePath(string playerName)
	{
		return Player.PlayerCacheFile(playerName, "game.json");
	}

	public static Game CreateNew(SBAnalytics analytics, Player p, StaticContentLoader contentLoader, out int performedMigration, PlayHavenController phController)
	{
		TFUtils.DebugLog("Creating new game");
		Dictionary<string, object> data = InitialGame.Generate(contentLoader.TheEntityManager, contentLoader.TheResourceManager);
		Game game = LoadFromDataDict(data, analytics, p, contentLoader, out performedMigration, phController);
		game.actionBuffer.Record(new InitializeAction());
		game.analytics.InitGameValues(game);
		TFUtils.DebugTimeOffset = 0uL;
		TFUtils.AddTimeOffset = 0uL;
		TFUtils.TriggerPurchaseWarning();
		return game;
	}

	public static Game LoadFromCache(Player p, SBAnalytics analytics, StaticContentLoader contentLoader, out int performedMigration, PlayHavenController phController)
	{
		string json = TFUtils.ReadAllText(p.CacheFile("game.json"));
		Dictionary<string, object> data = (Dictionary<string, object>)Json.Deserialize(json);
		return LoadFromDataDict(data, analytics, p, contentLoader, out performedMigration, phController);
	}

	public static SoaringContext CreateSoaringGameResponderContext(SoaringContextDelegate del)
	{
		SoaringContext soaringContext = new SoaringContext();
		soaringContext.Responder = new GameSoaringResponder();
		soaringContext.ContextResponder = del;
		return soaringContext;
	}

	public static void LoadFromNetwork(string userID, long timestamp, SoaringContext context, Session session)
	{
		if (context != null && context.Responder == null)
		{
			context.Responder = new SoaringDelegate();
		}
		session.WebFileServer.GetGameData(userID, timestamp, context);
	}

	public static Game LoadFromDataDict(Dictionary<string, object> data, SBAnalytics analytics, Player p, StaticContentLoader contentLoader, out int performedMigration, PlayHavenController phController)
	{
		if (!IsValidState(data))
		{
			string message = "Encountered invalid data! Creating a new local state.";
			TFUtils.DebugLog(message);
			throw new Exception(message);
		}
		List<Dictionary<string, object>> actionList = PersistedActionBuffer.LoadActionList(p);
		GamestateMigrator gamestateMigrator = new GamestateMigrator();
		gamestateMigrator.Migrate(data, actionList, contentLoader, p, out performedMigration);
		if (performedMigration == 3)
		{
			return null;
		}
		PersistedActionBuffer actBuffer = new PersistedActionBuffer(p, actionList);
		return new Game(analytics, data, p, contentLoader, actBuffer, phController);
	}

	public static bool IsValidState(Dictionary<string, object> data)
	{
		TFUtils.AssertKeyExists(data, "farm");
		return (!data.ContainsKey("error") || !((string)data["error"]).Equals("new_user")) && data.ContainsKey("farm");
	}

	public void DestroyCache()
	{
		lock (gameState)
		{
			TFUtils.DeleteFile(gameFile);
			actionBuffer.Flush();
		}
	}

	public void Clear()
	{
		sessionActionManager.ClearActions();
		if (simulation != null)
		{
			simulation.Clear();
		}
		SBGUI.GetInstance().ResetWhiteList();
		UnityGameResources.Reset();
	}

	private void LoadVariables()
	{
		if (gameState.ContainsKey("variables"))
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["variables"];
			if (dictionary.ContainsKey("auto_quest_completion_time"))
			{
				questManager.m_uQuestCompletionTimestamp = TFUtils.LoadUlong(dictionary, "auto_quest_completion_time");
			}
			if (dictionary.ContainsKey("current_auto_quest_completion_count"))
			{
				questManager.m_autoQuestCount = TFUtils.LoadInt(dictionary, "current_auto_quest_completion_count");
			}
			if (dictionary.ContainsKey("first_auto_quest_completion_timestamp"))
			{
				questManager.m_uAutoQuestStartTime = TFUtils.LoadUlong(dictionary, "first_auto_quest_completion_timestamp");
			}
		}
	}

	public void LoadSimulation(ulong utcNow)
	{
		loadSimulationActions = new Action[26]
		{
			delegate
			{
				LoadMicroEvents(utcNow);
			},
			delegate
			{
				LoadTasks(utcNow);
			},
			delegate
			{
				LoadTaskCompletions();
			},
			delegate
			{
				LoadCostumes();
			},
			delegate
			{
				LoadCraftings(utcNow);
			},
			delegate
			{
				LoadVending(utcNow);
			},
			delegate
			{
				LoadBuildings(utcNow);
			},
			delegate
			{
				LoadDebris(utcNow);
			},
			delegate
			{
				LoadLandmarks(utcNow);
			},
			delegate
			{
				LoadTerrain();
			},
			delegate
			{
				LoadTreasures(utcNow);
			},
			delegate
			{
				LoadUnits(utcNow);
			},
			delegate
			{
				LoadWanderers(utcNow);
			},
			delegate
			{
				LoadLastRandomQuestId();
			},
			delegate
			{
				LoadLastAutoQuestId();
			},
			delegate
			{
				LoadQuestDefinitions(utcNow);
			},
			delegate
			{
				LoadQuests(utcNow);
			},
			delegate
			{
				LoadRecipes();
			},
			delegate
			{
				LoadFeatureUnlocks();
			},
			delegate
			{
				LoadBuildingUnlocks();
			},
			delegate
			{
				LoadTreasureState();
			},
			delegate
			{
				LoadRewardCaps();
			},
			delegate
			{
				LoadMovies();
			},
			delegate
			{
				LoadDropPickups();
			},
			delegate
			{
				taskManager.RemoveUnsafeActiveTasks(this);
			},
			delegate
			{
				PatchReferences();
			}
		};
		loadSimulationActionsEnumerator = loadSimulationActions.GetEnumerator();
	}

	public bool IterateLoadSimulation()
	{
		if (loadSimulationActionsEnumerator.MoveNext())
		{
			Action action = (Action)loadSimulationActionsEnumerator.Current;
			if (action != null)
			{
				action();
			}
			else
			{
				TFUtils.WarningLog("missing loaded action!");
			}
			return false;
		}
		return true;
	}

	public string LoadActions(ulong utcNow, bool applyAction, bool forceSave)
	{
		string text = null;
		lock (gameState)
		{
			List<PersistedActionBuffer.PersistedAction> allUnackedActions = actionBuffer.GetAllUnackedActions();
			TFUtils.DebugLog("Applying " + allUnackedActions.Count + " actions to gamestate.");
			if (allUnackedActions.Count > 0 || forceSave)
			{
				LoadActionsFromList(allUnackedActions, utcNow, applyAction);
				actionBuffer.Flush();
			}
			return SaveLocally(0uL, true, true);
		}
	}

	public void ClearActionBuffer()
	{
		lock (gameState)
		{
			actionBuffer.Flush();
		}
	}

	public void SaveToServer(Session session, ulong utcNow, bool applyActions, bool forceSave)
	{
		string text = null;
		if (CanSave)
		{
			try
			{
				text = LoadActions(utcNow, applyActions, forceSave);
			}
			catch (Exception ex)
			{
				TFUtils.ErrorLog("Needs reload error dialog");
				SoaringDebug.Log("Needs reload error dialog: " + ex.Message, LogType.Error);
				TFUtils.ErrorLog(ex);
				needsReloadErrorDialog = true;
				RequestReload();
				text = null;
			}
			if (text != null)
			{
				TFUtils.DebugLog("Saving gamedata to server");
				SoaringContext context = CreateSoaringGameResponderContext(OnSaveGameData);
				session.WebFileServer.SaveGameData(text, context);
			}
			else
			{
				TFUtils.DebugLog("Gamedata has not changed. Not saving to server");
			}
		}
	}

	public void OnSaveGameData(SoaringContext context)
	{
		if (context == null)
		{
			return;
		}
		bool flag = context.soaringValue("status");
		SoaringError soaringError = (SoaringError)context.objectWithKey("error_message");
		SoaringDictionary soaringDictionary = (SoaringDictionary)context.objectWithKey("custom");
		if (soaringError != null || !flag || !Soaring.IsOnline)
		{
			if (soaringDictionary == null)
			{
				needsNetworkDownErrorDialog = false;
			}
			SoaringDebug.Log(soaringError, LogType.Error);
		}
		else if (soaringDictionary != null)
		{
			SBWebFileServer.LastSuccessfulSave = DateTime.UtcNow;
			long num = soaringDictionary.soaringValue("datetime");
			if (Player.ValidTimeStamp(num))
			{
				player.SetStagedTimestamp(num);
				player.SaveStagedTimestamp();
			}
		}
	}

	public void AddTimeToSimulation(ulong nSeconds)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["playtime"];
		object value = null;
		if (dictionary.TryGetValue("debug_add_time", out value))
		{
			dictionary["debug_add_time"] = Convert.ToUInt64(value) + nSeconds;
		}
		else
		{
			dictionary.Add("debug_add_time", nSeconds);
		}
		gameState["playtime"] = dictionary;
		TFUtils.AddTimeOffset = (ulong)dictionary["debug_add_time"];
		SaveLocally(0uL, true);
	}

	public void FastForwardSimulationBegun()
	{
		TFUtils.UtcNow = new DateTime(1970, 1, 1);
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["playtime"];
		if (dictionary.ContainsKey("fast_forward_start_time"))
		{
			dictionary["fast_forward_start_time"] = DateTime.UtcNow;
		}
		else
		{
			dictionary.Add("fast_forward_start_time", DateTime.UtcNow);
		}
		gameState["playtime"] = dictionary;
		SaveLocally(0uL, true);
	}

	public void FastForwardSimulationFinished()
	{
		TFUtils.isFastForwarding = false;
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["playtime"];
		DateTime utcNow = TFUtils.UtcNow;
		DateTime value = utcNow;
		if (dictionary.ContainsKey("fast_forward_start_time"))
		{
			value = (DateTime)dictionary["fast_forward_start_time"];
		}
		object value2 = null;
		if (dictionary.TryGetValue("adjusted_debug_time", out value2))
		{
			dictionary["adjusted_debug_time"] = Convert.ToUInt64(value2) + Convert.ToUInt64(utcNow.Subtract(value).TotalSeconds);
		}
		else
		{
			dictionary.Add("adjusted_debug_time", Convert.ToUInt64(utcNow.Subtract(value).TotalSeconds));
		}
		gameState["playtime"] = dictionary;
		TFUtils.FastForwardOffset = (ulong)dictionary["adjusted_debug_time"];
		SaveLocally(0uL, true);
	}

	private void LoadDebrisSlotObject(TerrainSlot expansion, TerrainSlotObject debrisObject, ulong utcNow)
	{
		DebrisEntity decorator = entities.Create(EntityType.DEBRIS, debrisObject.did, debrisObject.id, true).GetDecorator<DebrisEntity>();
		PurchasableDecorator decorator2 = decorator.GetDecorator<PurchasableDecorator>();
		decorator2.Purchased = false;
		decorator.ExpansionId = expansion.Id;
		Simulated.Debris.Load(decorator, simulation, debrisObject.position.ToVector2(), utcNow);
	}

	private void LoadLandmarkSlotObject(TerrainSlot expansion, TerrainSlotObject landmarkObject, ulong utcNow)
	{
		LandmarkEntity decorator = entities.Create(EntityType.LANDMARK, landmarkObject.did, landmarkObject.id, true).GetDecorator<LandmarkEntity>();
		decorator.GetDecorator<PurchasableDecorator>().Purchased = false;
		Simulated.Landmark.Load(decorator, simulation, landmarkObject.position.ToVector2(), utcNow);
	}

	public void LoadExpansions(ulong utcNow)
	{
		List<LoadSlotObjectData> list = new List<LoadSlotObjectData>();
		foreach (TerrainSlot item in terrain.UnpurchasedExpansionSlots())
		{
			foreach (TerrainSlotObject debri in item.debris)
			{
				list.Add(new LoadSlotObjectData(item, debri, utcNow, LoadDebrisSlotObject));
			}
			foreach (TerrainSlotObject landmark in item.landmarks)
			{
				list.Add(new LoadSlotObjectData(item, landmark, utcNow, LoadLandmarkSlotObject));
			}
		}
		loadExpansionSlotObjectsEnumerator = list.GetEnumerator();
	}

	public bool IterateLoadExpansions()
	{
		float num = 10f;
		DateTime utcNow = DateTime.UtcNow;
		while (loadExpansionSlotObjectsEnumerator.MoveNext())
		{
			LoadSlotObjectData loadSlotObjectData = (LoadSlotObjectData)loadExpansionSlotObjectsEnumerator.Current;
			if (loadSlotObjectData != null)
			{
				loadSlotObjectData.Load();
			}
			else
			{
				TFUtils.WarningLog("missing slot object loading expansions!");
			}
			if ((DateTime.UtcNow - utcNow).TotalMilliseconds > (double)num)
			{
				return false;
			}
		}
		return true;
	}

	public void LocalSaveCheck(float fDeltaTime)
	{
		m_fLocalSaveTimer += fDeltaTime;
		if (m_bNeedsLocalSave && m_fLocalSaveTimer >= 30f)
		{
			SaveLocally(0uL, false, false, true);
		}
	}

	public string SaveLocally(ulong timestamp, bool skipSave = false, bool skipWrite = false, bool useStaged = false)
	{
		lock (gameState)
		{
			if (CanSave)
			{
				TFUtils.DebugLog("Game Saving: " + timestamp + ", " + skipSave + ", " + skipSave + ", " + useStaged);
				string text = Json.Serialize(gameState);
				if (skipWrite)
				{
					return text;
				}
				string directoryName = Path.GetDirectoryName(gameFile);
				Debug.Log(directoryName);
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				File.WriteAllText(gameFile, text);
				m_fLocalSaveTimer = 0f;
				m_bNeedsLocalSave = false;
				if (skipSave)
				{
					return text;
				}
				if (useStaged)
				{
					player.SaveStagedTimestamp();
					return text;
				}
				if (Player.ValidTimeStamp((long)timestamp))
				{
					player.SaveTimestamp((long)timestamp);
				}
				return text;
			}
			TFUtils.DebugLog("Game Is Locked Cannot Save Locally");
		}
		return null;
	}

	public string LastAction()
	{
		return (string)((Dictionary<string, object>)gameState["farm"])["last_action"];
	}

	public void LockedGameStateChange(GamestateWriter writer)
	{
		lock (gameState)
		{
			writer(gameState);
			SaveLocally(0uL, true);
		}
	}

	public void RequestLoadFriendPark(string park)
	{
		loadFriendGame = true;
		SetPendingReload(true);
	}

	public bool ReloadToFriendPark()
	{
		return loadFriendGame;
	}

	public void ClearLoadFriendPark()
	{
		loadFriendGame = false;
	}

	public void RequestReload()
	{
		needsReload = true;
		SetPendingReload(true);
	}

	public bool RequiresReload()
	{
		return needsReload;
	}

	public void ClearReloadRequest()
	{
		needsReload = false;
	}

	public void SetPendingReload(bool rr)
	{
		pendingReload = rr;
	}

	public bool PendingReload()
	{
		return pendingReload;
	}

	public void NULL_ModifyStateSimulated(Simulated simulated, PersistedSimulatedAction action)
	{
	}

	public void ModifyGameStateSimulated(Simulated simulated, PersistedSimulatedAction action)
	{
		ModifyGameStateHelper(action, new Dictionary<string, object> { { "simulated", simulated } });
	}

	public void NULL_ModifyGameState(PersistedTriggerableAction action)
	{
	}

	public void ModifyGameState(PersistedTriggerableAction action)
	{
		ModifyGameStateHelper(action, new Dictionary<string, object>());
	}

	public void LoadLastRandomQuestId()
	{
		uint lastRandomQuestId = 400000u;
		if (((Dictionary<string, object>)gameState["farm"]).ContainsKey("random_quest_id"))
		{
			lastRandomQuestId = uint.Parse(((Dictionary<string, object>)gameState["farm"])["random_quest_id"].ToString());
		}
		QuestDefinition.LastRandomQuestId = lastRandomQuestId;
	}

	public void LoadLastAutoQuestId()
	{
		uint lastAutoQuestId = 500001u;
		if (((Dictionary<string, object>)gameState["farm"]).ContainsKey("auto_quest_id"))
		{
			lastAutoQuestId = uint.Parse(((Dictionary<string, object>)gameState["farm"])["auto_quest_id"].ToString());
		}
		QuestDefinition.LastAutoQuestId = lastAutoQuestId;
	}

	public int GetResidentPopulation()
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["units"];
		return list.Count;
	}

	private void ModifyGameStateHelper(PersistedTriggerableAction action, Dictionary<string, object> data)
	{
		action.Process(this);
		simulation.RecordAction(action);
		if (!SBSettings.UseActionFile)
		{
			action.Confirm(gameState);
			m_bNeedsLocalSave = true;
		}
		playtimeRegistrar.Process(action, resourceManager.Query(ResourceManager.LEVEL), analytics);
		if (!Session.TheDebugManager.debugPlaceObjects)
		{
			ITrigger trigger = action.CreateTrigger(data);
			triggerRouter.RouteTrigger(trigger, this);
		}
	}

	public void Record(PersistedTriggerableAction action)
	{
		if (action != null)
		{
			if (SBSettings.UseActionFile)
			{
				actionBuffer.Record(action);
				return;
			}
			action.Confirm(gameState);
			m_bNeedsLocalSave = true;
		}
	}

	public void ApplyReward(Reward reward, ulong buildingCompleteTime, bool bDoAnalytics = true)
	{
		List<int> clearedLands = reward.ClearedLands;
		int num = 0;
		if (clearedLands != null)
		{
			num = clearedLands.Count;
		}
		for (int i = 0; i < num; i++)
		{
			simulation.game.terrain.AddAndClearExpansionSlot(this, clearedLands[i]);
		}
		foreach (KeyValuePair<int, int> resourceAmount in reward.ResourceAmounts)
		{
			simulation.resourceManager.Add(resourceAmount.Key, resourceAmount.Value, this);
		}
		Dictionary<int, Vector2> buildingPositions = reward.BuildingPositions;
		foreach (KeyValuePair<int, int> buildingAmount in reward.BuildingAmounts)
		{
			int key = buildingAmount.Key;
			int value = buildingAmount.Value;
			List<object> list = (List<object>)reward.BuildingLabels[key.ToString()];
			for (int j = 0; j < value; j++)
			{
				Identity id = new Identity((string)list[j]);
				if (buildingPositions.ContainsKey(key))
				{
					Entity entity = entities.Create(EntityType.BUILDING, key, id, true);
					BuildingEntity decorator = entity.GetDecorator<BuildingEntity>();
					decorator.Slots = craftManager.GetInitialSlots(decorator.DefinitionId);
					ErectableDecorator decorator2 = decorator.GetDecorator<ErectableDecorator>();
					decorator2.ErectionCompleteTime = TFUtils.EpochTime() + decorator2.ErectionTime;
					Simulated simulated = Simulated.Building.Load(decorator, simulation, buildingPositions[key], false, TFUtils.EpochTime());
					NewBuildingAction action = new NewBuildingAction(id, entity.BlueprintName, key, EntityType.BUILDING, false, decorator2.ErectionCompleteTime.Value, buildingPositions[key], false, new Cost());
					ModifyGameStateSimulated(simulated, action);
					communityEventManager.GetSession().TheCamera.AutoPanToPosition(simulated.PositionCenter, 0.75f);
				}
				else
				{
					BuildingEntity decorator3 = entities.Create(EntityType.BUILDING, key, id, true).GetDecorator<BuildingEntity>();
					decorator3.GetDecorator<ErectableDecorator>().ErectionCompleteTime = buildingCompleteTime;
					decorator3.GetDecorator<ActivatableDecorator>().Activated = buildingCompleteTime;
					inventory.AddItem(decorator3, null);
					if (bDoAnalytics)
					{
						analytics.LogBuildingAddToInventory(decorator3.BlueprintName, resourceManager.Resources[ResourceManager.LEVEL].Amount);
					}
				}
			}
		}
		foreach (int recipeUnlock in reward.RecipeUnlocks)
		{
			simulation.craftManager.UnlockRecipe(recipeUnlock, this);
		}
		foreach (int movieUnlock in reward.MovieUnlocks)
		{
			simulation.movieManager.UnlockMovie(movieUnlock);
		}
		foreach (int costumeUnlock in reward.CostumeUnlocks)
		{
			costumeManager.UnlockCostume(costumeUnlock);
		}
		foreach (int buildingUnlock in reward.BuildingUnlocks)
		{
			buildingUnlockManager.UnlockBuilding(buildingUnlock);
		}
		if (reward.RandomLand)
		{
			simulation.game.terrain.AddRandomAvailableSlot(simulation.game);
		}
	}

	public void OnChangeSessionState()
	{
		foreach (Action<Game> sessionStateChangeObserver in sessionStateChangeObservers)
		{
			sessionStateChangeObserver(this);
		}
	}

	public SBMIAnalytics.CommonData GetAnalyticsCommonData()
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["farm"];
		List<object> list = (List<object>)dictionary["buildings"];
		List<object> list2 = (List<object>)dictionary["units"];
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 1;
		List<Simulated> simulatedRaw = simulation.GetSimulatedRaw();
		int count = simulatedRaw.Count;
		for (int i = 0; i < count; i++)
		{
			Simulated simulated = simulatedRaw[i];
			if (simulated.entity.Type == EntityType.BUILDING)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				if (entity != null && entity.ResidentDids.Count > 0)
				{
					num3++;
				}
			}
			else if (simulated.entity.Type == EntityType.RESIDENT)
			{
				num4++;
			}
		}
		num2 = num4;
		num = num3;
		bool bIsEligibleForSpongyGames = false;
		CommunityEvent activeEvent = communityEventManager.GetActiveEvent();
		if (activeEvent != null && (activeEvent.m_sID == CommunityEventManager._sSpongyGamesEventID || activeEvent.m_sID == CommunityEventManager._sSpongyGamesLastDayEventID))
		{
			if (activeEvent.m_nQuestPrereqID >= 0)
			{
				if (questManager.IsQuestCompleted((uint)activeEvent.m_nQuestPrereqID))
				{
					bIsEligibleForSpongyGames = true;
				}
			}
			else
			{
				bIsEligibleForSpongyGames = true;
			}
		}
		int nSpongyCurrency = 0;
		if (resourceManager.Resources.ContainsKey(ResourceManager.SPONGY_GAMES_CURRENCY))
		{
			nSpongyCurrency = resourceManager.Resources[ResourceManager.SPONGY_GAMES_CURRENCY].Amount;
		}
		SBMIAnalytics.CommonData result = default(SBMIAnalytics.CommonData);
		result.ulDateTime = SoaringAnalytics.AnalyticTime();
		result.ulFirstPlayTime = TFUtils.LoadUlong((Dictionary<string, object>)list[0], "build_finish_time");
		result.nPlayerLevel = resourceManager.PlayerLevelAmount;
		result.nSoftCurrency = resourceManager.Resources[ResourceManager.SOFT_CURRENCY].Amount;
		result.nHardCurreny = resourceManager.Resources[ResourceManager.HARD_CURRENCY].Amount;
		result.nCharacters = num2;
		result.nHouses = num;
		result.nLandExpansions = terrain.purchasedSlots.Count - 1;
		result.nSpongyCurrency = nSpongyCurrency;
		result.bIsEligibleForSpongyGames = bIsEligibleForSpongyGames;
		result.sPlayerID = Soaring.Player.UserID;
		result.sPlatform = SoaringPlatform.PrimaryPlatformName;
		result.sDeviceName = SystemInfo.deviceModel;
		result.sBinaryVersion = SoaringInternal.GameVersion.ToString();
		result.sCampaignData = SBAuth.campaigns;
		result.sOSVersion = SystemInfo.operatingSystem;
		result.sManifest = SBSettings.MANIFEST_FILE;
		result.sGUID = SoaringAnalytics.GenerateGUID();
		result.sDeviceGUID = SoaringAnalytics.DeviceGUID;
		result.ulSequence = SoaringAnalytics.DeviceSequenceID;
		return result;
	}

	public SBMIAnalytics.PlayerObject GetAnalyticsPlayerObject()
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["farm"];
		List<object> list = (List<object>)dictionary["buildings"];
		List<object> list2 = (List<object>)dictionary["units"];
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 1;
		List<Simulated> simulatedRaw = simulation.GetSimulatedRaw();
		int count = simulatedRaw.Count;
		for (int i = 0; i < count; i++)
		{
			Simulated simulated = simulatedRaw[i];
			if (simulated.entity.Type == EntityType.BUILDING)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				if (entity != null && entity.ResidentDids.Count > 0)
				{
					num3++;
				}
			}
			else if (simulated.entity.Type == EntityType.RESIDENT)
			{
				num4++;
			}
		}
		num2 = num4;
		num = num3;
		CommunityEvent activeEvent = communityEventManager.GetActiveEvent();
		string sLiveEventName = ((activeEvent != null) ? activeEvent.m_sName : null);
		int num5 = ((activeEvent != null) ? activeEvent.m_nValueID : (-1));
		int nSpecialCurrencyAmount = ((num5 >= 0) ? resourceManager.Resources[num5].Amount : (-1));
		return new SBMIAnalytics.PlayerObject("player", Soaring.Player.UserID, sLiveEventName, TFUtils.LoadUlong((Dictionary<string, object>)list[0], "build_finish_time"), resourceManager.PlayerLevelAmount, num2, num, terrain.purchasedSlots.Count - 1, resourceManager.Resources[ResourceManager.SOFT_CURRENCY].Amount, resourceManager.Resources[ResourceManager.HARD_CURRENCY].Amount, num5, nSpecialCurrencyAmount, SBAuth.campaigns);
	}

	public ulong FirstPlayTime()
	{
		ulong result = 0uL;
		try
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["farm"];
			List<object> list = (List<object>)dictionary["buildings"];
			result = TFUtils.LoadUlong((Dictionary<string, object>)list[0], "build_finish_time");
		}
		catch
		{
		}
		return result;
	}

	public SBMIAnalytics.MetaObject GetAnalyticsMetaObject(string sEventName, int nOverrideTrackingVersion = -1)
	{
		return new SBMIAnalytics.MetaObject("meta", sEventName, SystemInfo.deviceModel, SoaringInternal.GameVersion.ToString(), SystemInfo.operatingSystem.ToString(), SBSettings.MANIFEST_FILE, SoaringPlatform.PrimaryPlatformName, SoaringAnalytics.GenerateGUID(), SoaringAnalytics.DeviceGUID, (nOverrideTrackingVersion < 0) ? SBMIAnalytics._nTRACKING_VERSION : nOverrideTrackingVersion, SoaringAnalytics.DeviceSequenceID, SoaringAnalytics.AnalyticTime());
	}

	public static SBMIDeltaDNA.DeviceObject GetDeltaDNADeviceObject()
	{
		return new SBMIDeltaDNA.DeviceObject("device_data", ClientInfo.DeviceName, ClientInfo.DeviceType, ClientInfo.DeviceModel, ClientInfo.OperatingSystem, ClientInfo.OperatingSystemVersion, ClientInfo.Manufacturer, ClientInfo.TimezoneOffset, ClientInfo.LanguageCode);
	}

	public SBMIDeltaDNA.PlayerObject GetDeltaDNAPlayerObject()
	{
		return new SBMIDeltaDNA.PlayerObject("player", resourceManager.PlayerLevelAmount, resourceManager.Resources[ResourceManager.XP].Amount, resourceManager.Resources[ResourceManager.HARD_CURRENCY].Amount, resourceManager.Resources[ResourceManager.SOFT_CURRENCY].Amount);
	}

	private void LoadTerrain()
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["pavement"];
		if (list.Count != 0)
		{
			foreach (Dictionary<string, object> item in list)
			{
				GridPosition gpos = new GridPosition(TFUtils.LoadInt(item, "row"), TFUtils.LoadInt(item, "col"));
				terrain.ChangePath(gpos);
			}
		}
		List<object> list2 = (List<object>)((Dictionary<string, object>)gameState["farm"])["expansions"];
		if (list2.Count != 0)
		{
			terrain.purchasedSlots = new HashSet<int>(list2.ConvertAll((object x) => Convert.ToInt32(x)));
			foreach (int purchasedSlot in terrain.purchasedSlots)
			{
				terrain.AddExpansionSlot(purchasedSlot);
			}
		}
		if (featureManager.CheckFeature("purchase_expansions"))
		{
			terrain.UpdateRealtySigns(entities.DisplayControllerManager, SBCamera.BillboardDefinition, this);
		}
		border.UpdateTerrainBorderStrip(terrain);
	}

	private void LoadRecipes()
	{
		TFUtils.AssertKeyExists((Dictionary<string, object>)gameState["farm"], "recipes");
		List<int> list = TFUtils.LoadList<int>((Dictionary<string, object>)gameState["farm"], "recipes");
		if (list.Count == 0)
		{
			return;
		}
		foreach (int item in list)
		{
			craftManager.UnlockRecipe(item, this);
		}
	}

	private void LoadFeatureUnlocks()
	{
		if (!((Dictionary<string, object>)gameState["farm"]).ContainsKey("features"))
		{
			return;
		}
		List<string> list = TFUtils.LoadList<string>((Dictionary<string, object>)gameState["farm"], "features");
		if (list.Count == 0)
		{
			return;
		}
		foreach (string item in list)
		{
			featureManager.UnlockFeature(item);
		}
	}

	private void LoadBuildingUnlocks()
	{
		if (!((Dictionary<string, object>)gameState["farm"]).ContainsKey("building_unlocks"))
		{
			return;
		}
		List<int> list = TFUtils.LoadList<int>((Dictionary<string, object>)gameState["farm"], "building_unlocks");
		if (list.Count == 0)
		{
			return;
		}
		foreach (int item in list)
		{
			buildingUnlockManager.UnlockBuilding(item);
		}
	}

	private void LoadTreasureState()
	{
		if (((Dictionary<string, object>)gameState["farm"]).ContainsKey("treasure_state"))
		{
			Dictionary<string, object> dict = TFUtils.LoadDict((Dictionary<string, object>)gameState["farm"], "treasure_state");
			treasureManager.InitializeTreasureTimers(dict);
		}
	}

	private void LoadRewardCaps()
	{
		if (((Dictionary<string, object>)gameState["farm"]).ContainsKey("caps"))
		{
			Dictionary<string, object> dictionary = TFUtils.LoadDict((Dictionary<string, object>)gameState["farm"], "caps");
			ulong expiration = TFUtils.LoadUlong(dictionary, "expiration");
			int recipes = TFUtils.LoadInt(dictionary, "recipe_count");
			int jelly = TFUtils.LoadInt(dictionary, "jelly_count");
			rewardCap.Reset(jelly, recipes, expiration);
		}
	}

	private void LoadMovies()
	{
		TFUtils.AssertKeyExists((Dictionary<string, object>)gameState["farm"], "movies");
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["movies"];
		if (list.Count == 0)
		{
			return;
		}
		HashSet<int> hashSet = new HashSet<int>(list.ConvertAll((object x) => Convert.ToInt32(x)));
		foreach (int item in hashSet)
		{
			movieManager.UnlockMovie(item);
		}
	}

	private void LoadDropPickups()
	{
		TFUtils.AssertKeyExists((Dictionary<string, object>)gameState["farm"], "drop_pickups");
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["drop_pickups"];
		if (list.Count == 0)
		{
			return;
		}
		foreach (Dictionary<string, object> item in list)
		{
			dropManager.AddPickupTrigger(item);
		}
	}

	private void SaveVersionInfo()
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["farm"];
		dictionary["game_version"] = SBSettings.BundleVersion;
		dictionary["bundle_id"] = SBSettings.BundleIdentifier;
		dictionary["manifest_url"] = SBSettings.MANIFEST_URL;
	}

	private void LoadResources()
	{
		TFUtils.AssertKeyExists((Dictionary<string, object>)gameState["farm"], "resources");
		List<object> resources = (List<object>)((Dictionary<string, object>)gameState["farm"])["resources"];
		resourceManager.LoadResources(resources);
		resourceCalculatorManager = new ResourceCalculatorManager(levelingManager);
		resourceManager.UpdateLevelExpToMilestone(levelingManager);
	}

	private void LoadPlaytime(Dictionary<string, object> gameState, ResourceManager resourceMgr)
	{
		TFUtils.AssertKeyExists(gameState, "playtime");
		Dictionary<string, object> data = TFUtils.LoadDict(gameState, "playtime");
		playtimeRegistrar = PlaytimeRegistrar.FromDict(data);
	}

	private void LoadActionsFromList(List<PersistedActionBuffer.PersistedAction> actions, ulong utcNow, bool applyAction)
	{
		foreach (PersistedActionBuffer.PersistedAction action in actions)
		{
			TFUtils.DebugLog("Applying action: " + action.type);
			if (applyAction)
			{
				action.Apply(this, utcNow);
			}
			action.Confirm(gameState);
		}
	}

	private void LoadUnits(ulong utcNow)
	{
		LoadObjects("units", LoadUnit, utcNow);
	}

	private void LoadWanderers(ulong utcNow)
	{
		LoadObjects("wanderers", LoadWanderer, utcNow);
	}

	private void LoadBuildings(ulong utcNow)
	{
		LoadObjects("buildings", LoadBuilding, utcNow);
	}

	private void LoadDebris(ulong utcNow)
	{
		LoadObjects("debris", LoadDebris, utcNow);
	}

	private void LoadTreasures(ulong utcNow)
	{
		LoadObjects("treasure", LoadTreasure, utcNow);
	}

	private void LoadLandmarks(ulong utcNow)
	{
		LoadObjects("landmarks", LoadLandmark, utcNow);
	}

	private void LoadTasks(ulong utcNow)
	{
		LoadObjects("tasks_v2", LoadTask, utcNow);
	}

	private void LoadTaskCompletions()
	{
		TFUtils.AssertKeyExists(gameState, "farm");
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["farm"];
		if (dictionary.ContainsKey("task_completion_counts"))
		{
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)dictionary["task_completion_counts"];
			int result = 0;
			{
				foreach (KeyValuePair<string, object> item in dictionary2)
				{
					if (int.TryParse(item.Key, out result))
					{
						taskManager.SetTaskCompletionCount(result, TFUtils.LoadInt(dictionary2, item.Key));
					}
				}
				return;
			}
		}
		dictionary.Add("task_completion_counts", new Dictionary<string, object>());
	}

	private void LoadMicroEvents(ulong utcNow)
	{
		LoadObjects("micro_events", LoadMicroEvent, utcNow);
	}

	private void LoadQuests(ulong utcNow)
	{
		LoadObjects("quests", LoadQuest, utcNow);
	}

	private void LoadQuestDefinitions(ulong utcNow)
	{
		LoadObjects("generated_quest_definition", LoadQuestDefinition, utcNow);
	}

	private void LoadCraftings(ulong utcNow)
	{
		LoadObjects("crafts", LoadCrafting, utcNow);
	}

	private void LoadVending(ulong utcNow)
	{
		LoadObjects("vending", LoadVendor, utcNow);
	}

	private void LoadObjects(string key, ObjectLoaderFn objectLoader, ulong utcNow)
	{
		TFUtils.AssertKeyExists(gameState, "farm");
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["farm"];
		if (!dictionary.ContainsKey(key))
		{
			dictionary[key] = new List<object>();
			return;
		}
		foreach (Dictionary<string, object> item in dictionary[key] as List<object>)
		{
			objectLoader(item, utcNow);
		}
	}

	private void LoadUnit(Dictionary<string, object> dict, ulong utcNow)
	{
		int num = TFUtils.LoadInt(dict, "did");
		Identity identity = new Identity((string)dict["label"]);
		Identity identity2 = new Identity((string)dict["residence"]);
		bool flag = (bool)dict["active"];
		ResidentEntity decorator = entities.Create(EntityType.RESIDENT, num, identity, true).GetDecorator<ResidentEntity>();
		decorator.Residence = identity2;
		decorator.HungryAt = TFUtils.LoadUlong(dict, "feed_ready_time", utcNow);
		if (decorator.HungryAt - utcNow > 1209600)
		{
			decorator.HungryAt = utcNow;
		}
		decorator.HungerResourceId = TFUtils.TryLoadInt(dict, "wish_product_id");
		decorator.PreviousResourceId = TFUtils.TryLoadInt(dict, "prev_wish_product_id");
		decorator.CostumeDID = TFUtils.TryLoadInt(dict, "costume_did");
		if (!decorator.CostumeDID.HasValue && decorator.DefaultCostumeDID.HasValue)
		{
			decorator.CostumeDID = decorator.DefaultCostumeDID;
		}
		ulong? num2 = TFUtils.TryLoadUlong(dict, "fullness_length");
		if (!num2.HasValue)
		{
			decorator.FullnessLength = 90uL;
		}
		else
		{
			decorator.FullnessLength = num2.Value;
		}
		decorator.WishExpiresAt = TFUtils.TryLoadUlong(dict, "wish_expires_at");
		if (decorator.HungerResourceId.HasValue && !decorator.WishExpiresAt.HasValue)
		{
			TFUtils.WarningLog("Did not find required field wish_expires_at using default of now instead.");
			decorator.WishExpiresAt = utcNow;
		}
		decorator.MatchBonus = Reward.FromDict(TFUtils.TryLoadDict(dict, "match_bonus"));
		if (flag && !decorator.Disabled)
		{
			Simulated.Resident.Load(decorator, identity2, decorator.WishExpiresAt, decorator.HungerResourceId, decorator.PreviousResourceId, decorator.HungryAt, decorator.FullnessLength, decorator.MatchBonus, simulation, utcNow);
			return;
		}
		inventory.AddAssociatedEntities(identity2, new List<KeyValuePair<int, Identity>>
		{
			new KeyValuePair<int, Identity>(num, identity)
		});
	}

	private void LoadWanderer(Dictionary<string, object> dict, ulong utcNow)
	{
		int did = TFUtils.LoadInt(dict, "did");
		Identity id = new Identity((string)dict["label"]);
		ResidentEntity decorator = entities.Create(EntityType.WANDERER, did, id, true).GetDecorator<ResidentEntity>();
		decorator.HideExpiresAt = TFUtils.TryLoadUlong(dict, "hide_expires_at");
		decorator.DisableFlee = TFUtils.TryLoadBool(dict, "disable_flee");
		decorator.Wanderer = true;
		if (!decorator.DisableIfWillFlee.Value || decorator.DisableFlee.Value)
		{
			Simulated.Wanderer.Load(decorator, decorator.HideExpiresAt, decorator.DisableFlee, simulation, utcNow);
		}
	}

	private bool BuildingIsInventory(Dictionary<string, object> dict)
	{
		return dict["x"] == null || dict["y"] == null;
	}

	private void LoadBuilding(Dictionary<string, object> dict, ulong utcNow)
	{
		int did = TFUtils.LoadInt(dict, "did");
		EntityType primaryType = (EntityType)TFUtils.LoadUint(dict, "extensions");
		Entity entity = entities.Create(EntityTypeNamingHelper.GetBlueprintName(primaryType, did), new Identity((string)dict["label"]), true);
		BuildingEntity decorator = entity.GetDecorator<BuildingEntity>();
		decorator.Deserialize(dict);
		object value = null;
		dict.TryGetValue("rent_ready_time", out value);
		if (value != null && decorator.HasDecorator<PeriodicProductionDecorator>())
		{
			decorator.GetDecorator<PeriodicProductionDecorator>().ProductReadyTime = TFUtils.LoadUlong(dict, "rent_ready_time");
		}
		if (dict.ContainsKey("craft_rewards"))
		{
			decorator.CraftRewards = Reward.FromObject(dict["craft_rewards"]);
		}
		else if (dict.ContainsKey("craft.rewards"))
		{
			decorator.CraftRewards = Reward.FromObject(dict["craft.rewards"]);
		}
		if (dict.ContainsKey("crafting_slots"))
		{
			decorator.Slots = TFUtils.LoadInt(dict, "crafting_slots");
		}
		else if (craftManager.HasInitialSlots(decorator.DefinitionId))
		{
			decorator.Slots = craftManager.GetInitialSlots(decorator.DefinitionId);
		}
		if (decorator.CanVend)
		{
			if (dict.ContainsKey("general_restock"))
			{
				decorator.GetDecorator<VendingDecorator>().RestockTime = TFUtils.LoadUlong(dict, "general_restock");
			}
			if (dict.ContainsKey("special_restock"))
			{
				decorator.GetDecorator<VendingDecorator>().SpecialRestockTime = TFUtils.LoadUlong(dict, "special_restock");
			}
		}
		if (BuildingIsInventory(dict))
		{
			inventory.AddItem(decorator, null);
			return;
		}
		Vector2 position = new Vector2(TFUtils.LoadInt(dict, "x"), TFUtils.LoadInt(dict, "y"));
		bool? flag = TFUtils.LoadNullableBool(dict, "flip");
		Simulated simulated = Simulated.Building.Load(decorator, simulation, position, flag.HasValue && flag.Value, utcNow);
		if ((entity.AllTypes & EntityType.ANNEX) != EntityType.INVALID)
		{
			Simulated.Annex.Extend(simulated, simulation);
		}
	}

	private void LoadLandmark(Dictionary<string, object> dict, ulong utcNow)
	{
		LandmarkEntity decorator = entities.Create(EntityType.LANDMARK, TFUtils.LoadInt(dict, "did"), new Identity(TFUtils.LoadString(dict, "label")), true).GetDecorator<LandmarkEntity>();
		decorator.Deserialize(dict);
		Simulated.Landmark.Load(position: new Vector2(TFUtils.LoadInt(dict, "x"), TFUtils.LoadInt(dict, "y")), landmarkEntity: decorator, simulation: simulation, utcNow: utcNow);
	}

	private void LoadDebris(Dictionary<string, object> dict, ulong utcNow)
	{
		DebrisEntity decorator = entities.Create(EntityType.DEBRIS, TFUtils.LoadInt(dict, "did"), new Identity((string)dict["label"]), true).GetDecorator<DebrisEntity>();
		if (dict.ContainsKey("clear_complete_time"))
		{
			PurchasableDecorator decorator2 = decorator.GetDecorator<PurchasableDecorator>();
			decorator2.Purchased = true;
			ClearableDecorator decorator3 = decorator.GetDecorator<ClearableDecorator>();
			decorator3.ClearCompleteTime = TFUtils.LoadNullableUlong(dict, "clear_complete_time");
		}
		Simulated.Debris.Load(position: new Vector2(TFUtils.LoadInt(dict, "x"), TFUtils.LoadInt(dict, "y")), debrisEntity: decorator, simulation: simulation, utcNow: utcNow);
	}

	private void LoadTreasure(Dictionary<string, object> dict, ulong utcNow)
	{
		if (Session.PatchyTownGame)
		{
			return;
		}
		TreasureEntity decorator = entities.Create(EntityType.TREASURE, TFUtils.LoadInt(dict, "did"), new Identity((string)dict["label"]), true).GetDecorator<TreasureEntity>();
		if (dict.ContainsKey("clear_complete_time"))
		{
			decorator.ClearCompleteTime = TFUtils.LoadNullableUlong(dict, "clear_complete_time");
		}
		string text = TFUtils.TryLoadNullableString(dict, "treasure_spawner_name");
		if (text == null)
		{
			text = "time_to_spawn";
		}
		if (decorator == null)
		{
			Debug.LogError("Game: Null Or Invalid Tressure Entity: " + text);
			return;
		}
		decorator.TreasureTiming = treasureManager.FindTreasureSpawner(text);
		if (decorator.TreasureTiming == null)
		{
			Debug.LogError("Game: Null Or Invalid Tressure Timing: " + text);
			return;
		}
		Vector2 position = decorator.TreasureTiming.GenerateLocation();
		Simulated.Treasure.Load(decorator, simulation, position, utcNow);
	}

	private void LoadTask(Dictionary<string, object> pDict, ulong utcNow)
	{
		taskManager.AddActiveTask(this, new Task(this, pDict), true);
	}

	private void LoadMicroEvent(Dictionary<string, object> pDict, ulong utcNow)
	{
		microEventManager.AddMicroEvent(this, new MicroEvent(this, pDict), true);
	}

	private void LoadCostumes()
	{
		if (!((Dictionary<string, object>)gameState["farm"]).ContainsKey("costumes"))
		{
			((Dictionary<string, object>)gameState["farm"]).Add("costumes", new List<object>());
		}
		List<int> list = TFUtils.LoadList<int>((Dictionary<string, object>)gameState["farm"], "costumes");
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			costumeManager.UnlockCostume(list[i]);
		}
	}

	private void LoadQuest(Dictionary<string, object> dict, ulong utcNow)
	{
		Quest quest = Quest.FromDict(dict);
		if (quest != null)
		{
			questManager.RegisterQuest(this, quest);
		}
	}

	private void LoadQuestDefinition(Dictionary<string, object> pDict, ulong nUtcNow)
	{
		QuestDefinition questDefinition = QuestDefinition.FromDict(pDict);
		Quest quest = questManager.AddQuestDefinition(questDefinition);
		if (quest != null)
		{
			if (quest.Did == QuestDefinition.LastRandomQuestId)
			{
				QuestDefinition.RecreateRandomQuestStartInputData(this, questDefinition.Did);
				QuestDefinition.RecreateRandomQuestCompleteInputData(this, questDefinition.Did);
			}
			else if (quest.Did == QuestDefinition.LastAutoQuestId)
			{
				QuestDefinition.RecreateAutoQuestIntroInputData(this, questDefinition.Did);
				QuestDefinition.RecreateAutoQuestOutroInputData(this, questDefinition.Did);
			}
		}
	}

	private void LoadCrafting(Dictionary<string, object> dict, ulong utcNow)
	{
		CraftingInstance craftingInstance = new CraftingInstance(dict);
		craftManager.AddCraftingInstance(craftingInstance);
		if (craftingInstance.ReadyTimeUtc > utcNow)
		{
			simulation.Router.Send(CraftedCommand.Create(craftingInstance.buildingLabel, craftingInstance.buildingLabel, craftingInstance.slotId), craftingInstance.ReadyTimeUtc - utcNow);
		}
		else
		{
			simulation.Router.Send(CraftedCommand.Create(craftingInstance.buildingLabel, craftingInstance.buildingLabel, craftingInstance.slotId));
		}
	}

	private void LoadVendor(Dictionary<string, object> dict, ulong utcNow)
	{
		Identity target = new Identity((string)dict["label"]);
		Dictionary<string, object> generalInstances = ((!dict.ContainsKey("general_instances")) ? new Dictionary<string, object>() : TFUtils.LoadDict(dict, "general_instances"));
		Dictionary<string, object> specialInstances = ((!dict.ContainsKey("special_instances")) ? new Dictionary<string, object>() : TFUtils.LoadDict(dict, "special_instances"));
		vendingManager.LoadVendorInstances(target, generalInstances, specialInstances);
	}

	private void PatchReferences()
	{
		foreach (Entity entity in entities.GetEntities())
		{
			entity.PatchReferences(this);
		}
	}
}
