#define ASSERTS_ON
using System;
using System.Collections.Generic;
using UnityEngine;

public class Simulation
{
	public class Indexer<Key, Value>
	{
		private Dictionary<Key, Value> values;

		public Value this[Key key]
		{
			get
			{
				return values[key];
			}
			set
			{
				values[key] = value;
			}
		}

		public Indexer(Dictionary<Key, Value> values)
		{
			this.values = values;
		}
	}

	public class WaypointIndexer
	{
		private Dictionary<string, Waypoint> dictionary;

		private List<Waypoint> list;

		public Waypoint this[string key]
		{
			get
			{
				return dictionary[key];
			}
			set
			{
				dictionary[key] = value;
				list.Remove(value);
				list.Add(value);
			}
		}

		public WaypointIndexer(Dictionary<string, Waypoint> dictionary, List<Waypoint> list)
		{
			this.dictionary = dictionary;
			this.list = list;
		}
	}

	public class Placement
	{
		public enum RESULT
		{
			VALID = 0,
			INVALID = 1,
			CONDITIONAL = 2
		}
	}

	public delegate void RecordBufferAction(PersistedActionBuffer.PersistedAction action);

	public delegate void ModifyGameStateSimulatedFunction(Simulated simulated, PersistedSimulatedAction action);

	public delegate void ModifyGameStateFunction(PersistedTriggerableAction action);

	private const float TIME_STEP = 0.1f;

	private const string WORKER = "worker_0";

	public RecordBufferAction RecordAction;

	public ModifyGameStateSimulatedFunction ModifyGameStateSimulated;

	public ModifyGameStateFunction ModifyGameState;

	public Game game;

	public TriggerRouter triggerRouter;

	public ResourceManager resourceManager;

	public SBAnalytics analytics;

	public SoundEffectManager soundEffectManager;

	public ResourceCalculatorManager resourceCalculatorManager;

	public CraftingManager craftManager;

	public FeatureManager featureManager;

	public MovieManager movieManager;

	public ParticleSystemManager particleSystemManager;

	public EnclosureManager enclosureManager;

	public RewardDropManager rewardDropManager;

	public Catalog catalog;

	public SBGUIScreen scratchScreen;

	public RewardCap rewardCap;

	private Identity.Equality identityComperer = new Identity.Equality();

	private Dictionary<string, Waypoint> waypointDictionary;

	private List<Waypoint> waypointList;

	private WaypointIndexer waypointIndexer;

	private EntityManager entityManager;

	private List<Simulated> simulateds;

	private List<Simulated> simulatedsCopy;

	private ItemDropManager dropManager;

	private Dictionary<int, int> whitelistedDefinitions;

	private Dictionary<Identity, int> whitelistedIdentities;

	private Dictionary<int, int> whitelistedExpansions;

	private Dictionary<Identity, Simulated> workerSpawners;

	private Camera camera;

	private Terrain terrain;

	private Scene scene;

	private CommandRouter router;

	private float timeAccum;

	private float timeLast;

	private double timeSimulation;

	private float interpolant;

	public SplineInterpolator bounceInterpolator;

	public SplineInterpolator bounceStartInterpolator;

	public SplineInterpolator bounceEndInterpolator;

	public double Time
	{
		get
		{
			return timeSimulation;
		}
	}

	public float TimeStep
	{
		get
		{
			return 0.1f;
		}
	}

	public float Interpolant
	{
		get
		{
			return interpolant;
		}
	}

	public CommandRouter Router
	{
		get
		{
			return router;
		}
	}

	public Terrain Terrain
	{
		get
		{
			return terrain;
		}
	}

	public Scene Scene
	{
		get
		{
			return scene;
		}
	}

	public WaypointIndexer Waypoint
	{
		get
		{
			return waypointIndexer;
		}
	}

	public EntityManager EntityManager
	{
		get
		{
			return entityManager;
		}
	}

	public ItemDropManager DropManager
	{
		get
		{
			return dropManager;
		}
	}

	public Camera TheCamera
	{
		get
		{
			return camera;
		}
	}

	public bool Whitelisted
	{
		get
		{
			return whitelistedIdentities.Count > 0 || whitelistedDefinitions.Count > 0;
		}
	}

	public Simulation(ModifyGameStateFunction modifyGameState, ModifyGameStateSimulatedFunction modifyGameStateSimulated, Action<Simulated> rushSimulated, RecordBufferAction recordAction, Game game, EntityManager entityManager, TriggerRouter triggerRouter, ResourceManager resourceManager, ItemDropManager dropManager, SoundEffectManager soundEffectManager, ResourceCalculatorManager resourceCalculatorManager, CraftingManager craftManager, MovieManager movieManager, FeatureManager featureManager, Catalog catalog, RewardCap rewardCap, Camera camera, Terrain terrain, int depth, SBAnalytics analytics, SBGUIScreen scratchScreen, EnclosureManager enclosureManager)
	{
		this.game = game;
		this.analytics = analytics;
		this.soundEffectManager = soundEffectManager;
		this.triggerRouter = triggerRouter;
		this.resourceManager = resourceManager;
		this.resourceCalculatorManager = resourceCalculatorManager;
		this.craftManager = craftManager;
		this.movieManager = movieManager;
		this.dropManager = dropManager;
		this.featureManager = featureManager;
		this.catalog = catalog;
		ModifyGameState = modifyGameState;
		ModifyGameStateSimulated = modifyGameStateSimulated;
		RecordAction = recordAction;
		this.rewardCap = rewardCap;
		workerSpawners = new Dictionary<Identity, Simulated>();
		waypointDictionary = new Dictionary<string, Waypoint>();
		waypointList = new List<Waypoint>();
		waypointIndexer = new WaypointIndexer(waypointDictionary, waypointList);
		this.entityManager = entityManager;
		this.terrain = terrain;
		this.camera = camera;
		simulateds = new List<Simulated>();
		simulatedsCopy = new List<Simulated>();
		scene = new Scene(this.terrain, depth);
		router = new CommandRouter();
		particleSystemManager = new ParticleSystemManager();
		this.enclosureManager = enclosureManager;
		rewardDropManager = new RewardDropManager();
		this.scratchScreen = scratchScreen;
		bounceInterpolator = new SplineInterpolator();
		bounceInterpolator.LoadData("bounce.json");
		bounceStartInterpolator = new SplineInterpolator();
		bounceStartInterpolator.LoadData("bounce_start.json");
		bounceEndInterpolator = new SplineInterpolator();
		bounceEndInterpolator.LoadData("bounce_end.json");
		whitelistedIdentities = new Dictionary<Identity, int>();
		whitelistedDefinitions = new Dictionary<int, int>();
		whitelistedExpansions = new Dictionary<int, int>();
	}

	public Simulated CreateSimulated(Entity entity, Simulated.StateAction initialState, Vector2 position)
	{
		Simulated simulated = new Simulated(this, entity, position);
		AddSimulated(simulated);
		simulated.DisplayState("default");
		simulated.EnterInitialState(initialState, this);
		return simulated;
	}

	public Simulated CreateSimulated(string blueprint, Vector2 position)
	{
		return SetSimulated(new Simulated(this, entityManager.Create(blueprint, true), position));
	}

	public Simulated CreateSimulated(EntityType types, int did, Vector2 position)
	{
		return SetSimulated(new Simulated(this, entityManager.Create(types, did, true), position));
	}

	private Simulated SetSimulated(Simulated simulated)
	{
		AddSimulated(simulated);
		simulated.EnterInitialState((Simulated.StateAction)simulated.Invariable["action"], this);
		return simulated;
	}

	public void AddSimulated(Simulated simulated)
	{
		if (simulated.WorkerSpawner)
		{
			AddWorkerSpawner(simulated);
		}
		simulateds.Add(simulated);
		if (simulated.IsWaypoint)
		{
			AddWaypoint(simulated);
		}
		scene.Add(simulated);
		router.Register(simulated);
	}

	public void RemoveSimulated(Simulated simulated)
	{
		if (waypointDictionary.ContainsKey(simulated.Id.Describe()))
		{
			RemoveWaypoint(simulated);
		}
		simulateds.Remove(simulated);
		router.Unregister(simulated);
		scene.Remove(simulated);
		simulated.Destroy(this);
	}

	public void SendPendingCommands()
	{
		foreach (Simulated simulated in simulateds)
		{
			simulated.SendPendingCommands(this);
		}
	}

	public Simulated FindSimulated(Identity id)
	{
		if (id.Equals(Identity.Null()))
		{
			return null;
		}
		int count = simulateds.Count;
		for (int i = 0; i < count; i++)
		{
			if (identityComperer.Equals(simulateds[i].Id, id))
			{
				return simulateds[i];
			}
		}
		return null;
	}

	public Simulated FindSimulated(int? did)
	{
		if (!did.HasValue)
		{
			return null;
		}
		foreach (Simulated simulated in simulateds)
		{
			if (simulated.entity.DefinitionId == did.Value)
			{
				return simulated;
			}
		}
		return null;
	}

	public Simulated FindSimulated(int? did, EntityType type)
	{
		if (!did.HasValue)
		{
			return null;
		}
		foreach (Simulated simulated in simulateds)
		{
			if (simulated.entity.DefinitionId == did.Value && simulated.entity.Type == type)
			{
				return simulated;
			}
		}
		return null;
	}

	public List<Simulated> FindAllSimulateds(int did, EntityType? type = null)
	{
		if (type.HasValue)
		{
			return simulateds.FindAll((Simulated sim) => sim.entity.DefinitionId == did && sim.entity.Type == type.GetValueOrDefault() && type.HasValue);
		}
		return simulateds.FindAll((Simulated sim) => sim.entity.DefinitionId == did);
	}

	public IEnumerable<Simulated> GetSimulateds()
	{
		return simulateds;
	}

	public List<Simulated> GetSimulatedRaw()
	{
		return simulateds;
	}

	public Simulated SpawnWorker(Simulated simulated)
	{
		Simulated simulated2 = CreateSimulated("worker_0", Vector2.zero);
		Simulated closestWorkerSpawner = GetClosestWorkerSpawner(simulated.Position);
		if (closestWorkerSpawner != null)
		{
			simulated2.Warp(closestWorkerSpawner.PointOfInterest);
		}
		else
		{
			simulated2.Warp(simulated.PointOfInterest);
			Router.Send(SpawnCommand.Create(Identity.Null(), simulated.Id, "worker_0"));
		}
		simulated2.Visible = true;
		return simulated2;
	}

	public Waypoint GetRandomWaypoint()
	{
		int count = waypointList.Count;
		if (count == 0)
		{
			return null;
		}
		List<Waypoint> list = new List<Waypoint>();
		for (int i = 0; i < count; i++)
		{
			if (terrain.CheckIsPurchasedArea(waypointList[i].Position))
			{
				list.Add(waypointList[i]);
			}
		}
		count = list.Count;
		if (count == 0)
		{
			return null;
		}
		return list[UnityEngine.Random.Range(0, count)];
	}

	public void Clear()
	{
		foreach (Simulated simulated in simulateds)
		{
			simulated.Destroy(this);
		}
		SwarmManager.Instance.Cleanup();
		terrain.Destroy();
	}

	public Vector2 ScreenPositionFromWorldPosition(Vector3 worldPosition)
	{
		Vector3 vector = camera.WorldToScreenPoint(worldPosition);
		return new Vector2(vector.x, SBGUI.GetScreenHeight() - vector.y);
	}

	public void OnUpdate(Session session)
	{
		float time = UnityEngine.Time.time;
		interpolant = timeAccum / 0.1f;
		Animate();
		float num = time - timeLast;
		timeAccum += num;
		for (timeLast = time; timeAccum >= 0.1f; timeAccum -= 0.1f)
		{
			Simulate(session);
			particleSystemManager.OnUpdate();
			enclosureManager.OnUpdate(this);
			SwarmManager.Instance.OnUpdate(this, 0.1f);
			timeSimulation += 0.10000000149011612;
		}
		if (!SBSettings.UseActionFile)
		{
			game.LocalSaveCheck(num);
		}
	}

	public void OnUpdateVisitParkState(Session session)
	{
		float time = UnityEngine.Time.time;
		timeLast = time - 0.1f;
		interpolant = timeAccum / 0.1f;
		Animate();
		float num = time - timeLast;
		timeAccum += num;
		for (timeLast = time; timeAccum >= 0.1f; timeAccum -= 0.1f)
		{
			Simulate(session);
			particleSystemManager.OnUpdate();
			enclosureManager.OnUpdate(this);
			SwarmManager.Instance.OnUpdate(this, 0.1f);
			timeSimulation += 0.10000000149011612;
		}
	}

	public TerrainPathing CreatePathing(Vector2 start, Vector2 goal)
	{
		return new TerrainPathing(terrain, start, goal);
	}

	public void ResetAllAffectedPaths(AlignedBox box)
	{
		GridPosition gridPosition = terrain.ComputeGridPosition(new Vector2(box.xmin, box.ymin));
		GridPosition gridPosition2 = terrain.ComputeGridPosition(new Vector2(box.xmax, box.ymax));
		gridPosition.MakeValid(terrain.GridHeight - 1, terrain.GridWidth - 1);
		gridPosition2.MakeValid(terrain.GridHeight - 1, terrain.GridWidth - 1);
		foreach (Simulated simulated in simulateds)
		{
			HandleIfShouldRecalculatePath(simulated, gridPosition, gridPosition2);
		}
	}

	public void HandleIfShouldRecalculatePath(Simulated simulated, GridPosition min, GridPosition max)
	{
		object value = null;
		Path<GridPosition> path = null;
		simulated.Variable.TryGetValue("pathing", out value);
		if (value == null)
		{
			if (simulated.Variable.TryGetValue("path", out value))
			{
				path = (Path<GridPosition>)value;
			}
			if (path == null || path.Done())
			{
				return;
			}
			bool flag = true;
			bool flag2 = false;
			foreach (GridPosition item in path)
			{
				if (flag)
				{
					if (item == path.Current)
					{
						flag = false;
					}
				}
				else if (item.Within(min, max))
				{
					flag2 = true;
					break;
				}
			}
			if (!flag2)
			{
				return;
			}
		}
		GridPosition gridPosition = null;
		if (simulated.Variable.TryGetValue("pathGoal", out value))
		{
			gridPosition = value as GridPosition;
		}
		if (gridPosition != null)
		{
			Vector2 goal = Terrain.ComputeWorldPosition(gridPosition);
			simulated.Variable["pathing"] = CreatePathing(simulated.Position, goal);
			simulated.ClearPathInfo();
		}
	}

	public Placement.RESULT PlacementQuery(Simulated selected, ref List<Simulated> collisions, bool debrisOnly = false)
	{
		return PlacementQuery(selected.Box, ref collisions, selected.Id, debrisOnly);
	}

	public Placement.RESULT PlacementQuery(AlignedBox box, ref List<Simulated> collisions, Identity id = null, bool debrisOnly = false)
	{
		if (terrain.FootprintGuide != null && (terrain.FootprintGuide.xmin != box.xmin || terrain.FootprintGuide.ymin != box.ymin))
		{
			TFUtils.WarningLog("Terrain Debugging: PlacementQuery: Invalid Footprint Area");
			return Placement.RESULT.INVALID;
		}
		if (!terrain.CheckIsPurchasedArea(box))
		{
			TFUtils.WarningLog("Terrain Debugging: PlacementQuery: Area Is Not Purchased");
			return Placement.RESULT.INVALID;
		}
		scene.FindPlacementBlockers(box, ref collisions);
		if (debrisOnly)
		{
			foreach (Simulated collision in collisions)
			{
				if (collision.entity is DebrisEntity)
				{
					return Placement.RESULT.INVALID;
				}
			}
		}
		else
		{
			Simulated ourself = null;
			if (id != null)
			{
				ourself = FindSimulated(id);
			}
			if (ourself != null && ourself.HasEntity<StructureDecorator>() && ourself.GetEntity<StructureDecorator>().ShareableSpace)
			{
				collisions = collisions.FindAll((Simulated x) => !x.HasEntity<StructureDecorator>() || !x.GetEntity<StructureDecorator>().ShareableSpace);
			}
			else if (ourself != null && ourself.HasEntity<StructureDecorator>() && ourself.GetEntity<StructureDecorator>().ShareableSpaceSnap)
			{
				collisions = collisions.FindAll((Simulated x) => !x.HasEntity<StructureDecorator>() || !x.GetEntity<StructureDecorator>().ShareableSpaceSnap || x.Flip == ourself.Flip);
			}
			if (collisions.Count > 1 || (collisions.Count == 1 && collisions[0].entity.Id != id))
			{
				return Placement.RESULT.INVALID;
			}
		}
		return Placement.RESULT.VALID;
	}

	public Placement.RESULT PlacementQuery(AlignedBox box, Identity id = null, bool debrisOnly = false)
	{
		List<Simulated> collisions = new List<Simulated>();
		return PlacementQuery(box, ref collisions, id, debrisOnly);
	}

	public Placement.RESULT PlacementQuery(Simulated selected, bool debrisOnly = false)
	{
		List<Simulated> collisions = new List<Simulated>();
		return PlacementQuery(selected.Box, ref collisions, selected.Id, debrisOnly);
	}

	private void AddWorkerSpawner(Simulated workerSpawner)
	{
		if (!workerSpawners.ContainsKey(workerSpawner.Id))
		{
			workerSpawners.Add(workerSpawner.Id, workerSpawner);
		}
	}

	public void TryWorkerSpawnerCleanup(Identity id)
	{
		if (workerSpawners.ContainsKey(id))
		{
			workerSpawners.Remove(id);
		}
	}

	public Simulated GetClosestWorkerSpawner(Vector2 location)
	{
		Simulated result = null;
		float? num = null;
		foreach (Identity key in workerSpawners.Keys)
		{
			float num2 = Vector2.Distance(location, workerSpawners[key].PointOfInterest);
			if (!num.HasValue || num2 < num.Value)
			{
				result = workerSpawners[key];
				num = num2;
			}
		}
		return result;
	}

	private void Animate()
	{
		Simulated.SimulatedFlags simulatedFlags = Simulated.SimulatedFlags.MOBILE | Simulated.SimulatedFlags.FORCE_ANIMATE_ACTION | Simulated.SimulatedFlags.FORCE_ANIMATE_FOOTPRINT | Simulated.SimulatedFlags.FORCE_ANIMATE_BOUNCE | Simulated.SimulatedFlags.FORCE_ANIMATE_BOUNCE_START | Simulated.SimulatedFlags.FORCE_ANIMATE_BOUNCE_END;
		foreach (Simulated simulated in simulateds)
		{
			if ((simulated.simFlags & Simulated.SimulatedFlags.FIRST_ANIMATE) == 0)
			{
				if ((simulated.simFlags & simulatedFlags) != 0)
				{
					simulated.Animate(this);
				}
				IDisplayController displayController = simulated.DisplayController;
				DisplayControllerFlags flags = displayController.Flags;
				if ((flags & (DisplayControllerFlags.SWITCHED_STATE | DisplayControllerFlags.NEED_UPDATE)) != 0)
				{
					displayController.OnUpdate(camera, particleSystemManager);
					displayController.Flags = flags & ~DisplayControllerFlags.SWITCHED_STATE;
				}
				IDisplayController thoughtDisplayController = simulated.ThoughtDisplayController;
				if (thoughtDisplayController != null)
				{
					DisplayControllerFlags flags2 = thoughtDisplayController.Flags;
					if ((flags2 & DisplayControllerFlags.VISIBLE_AND_VALID_STATE) != 0)
					{
						simulated.AnimateOtherControllers(this);
					}
				}
				continue;
			}
			simulated.FirstAnimate(this);
			simulated.Animate(this);
			IDisplayController displayController2 = simulated.DisplayController;
			displayController2.OnUpdate(camera, particleSystemManager);
			IDisplayController thoughtDisplayController2 = simulated.ThoughtDisplayController;
			if (thoughtDisplayController2 != null)
			{
				DisplayControllerFlags flags3 = thoughtDisplayController2.Flags;
				if ((flags3 & DisplayControllerFlags.VISIBLE_AND_VALID_STATE) != 0)
				{
					simulated.AnimateOtherControllers(this);
				}
			}
		}
		if (!Session.TheDebugManager.showHitBoxes && !Session.TheDebugManager.showFootprints && !Session.TheDebugManager.showExpansionBorders)
		{
			return;
		}
		foreach (Simulated simulated2 in simulateds)
		{
			simulated2.AnimateDebugHitBox(this);
		}
	}

	private void Simulate(Session session)
	{
		router.Route();
		simulatedsCopy.Clear();
		simulatedsCopy.AddRange(simulateds);
		foreach (Simulated item in simulatedsCopy)
		{
			if (item.Simulate(this, session))
			{
				item.SetFootprint(this, false);
				RemoveSimulated(item);
			}
		}
		scene.OnUpdate(simulateds);
	}

	public void UpdateControls()
	{
		int count = simulateds.Count;
		for (int i = 0; i < count; i++)
		{
			simulateds[i].UpdateControls(this);
		}
	}

	public void UpdateDebugHitBoxes()
	{
		if (!SBSettings.DebugDisplayControllers)
		{
			return;
		}
		bool showHitBoxes = Session.TheDebugManager.showHitBoxes;
		foreach (Simulated simulated in simulateds)
		{
			simulated.DebugHitBoxesVisible = showHitBoxes;
		}
	}

	public void UpdateDebugFootprints()
	{
		if (!SBSettings.DebugDisplayControllers)
		{
			return;
		}
		bool showFootprints = Session.TheDebugManager.showFootprints;
		foreach (Simulated simulated in simulateds)
		{
			simulated.DebugFootprintsVisible = showFootprints;
		}
	}

	public void UpdateDebugExpansionBorders()
	{
		if (SBSettings.DebugDisplayControllers)
		{
			if (Session.TheDebugManager.showExpansionBorders)
			{
				terrain.OutlineAllExpansionSlots();
			}
			else
			{
				terrain.HideAllExpansionSlots();
			}
		}
	}

	private void AddWaypoint(Simulated sim)
	{
		string key = sim.Id.Describe();
		if (!waypointDictionary.ContainsKey(key))
		{
			Waypoint waypoint = new Waypoint(sim);
			waypointDictionary.Add(key, waypoint);
			waypointList.Add(waypoint);
		}
		Waypoint[sim.Id.Describe()] = new Waypoint(sim);
	}

	private void RemoveWaypoint(Simulated sim)
	{
		Waypoint item = waypointDictionary[sim.Id.Describe()];
		waypointList.Remove(item);
		waypointDictionary.Remove(sim.Id.Describe());
	}

	public void ClearPendingTimebarsInSimulateds()
	{
		foreach (Simulated simulated in simulateds)
		{
			simulated.RemoveSimulateOnceAction("show_timebar");
		}
	}

	public void ClearPendingNamebarsInSimulateds()
	{
		foreach (Simulated simulated in simulateds)
		{
			simulated.RemoveSimulateOnceAction("show_namebar");
		}
	}

	public void WhitelistSimulated(Identity id)
	{
		if (whitelistedIdentities.ContainsKey(id))
		{
			Dictionary<Identity, int> dictionary2;
			Dictionary<Identity, int> dictionary = (dictionary2 = whitelistedIdentities);
			Identity key2;
			Identity key = (key2 = id);
			int num = dictionary2[key2];
			dictionary[key] = num + 1;
		}
		else
		{
			whitelistedIdentities[id] = 1;
		}
	}

	public void WhitelistSimulated(int definitionId)
	{
		if (whitelistedDefinitions.ContainsKey(definitionId))
		{
			Dictionary<int, int> dictionary2;
			Dictionary<int, int> dictionary = (dictionary2 = whitelistedDefinitions);
			int key2;
			int key = (key2 = definitionId);
			key2 = dictionary2[key2];
			dictionary[key] = key2 + 1;
		}
		else
		{
			whitelistedDefinitions[definitionId] = 1;
		}
	}

	public void UnWhitelistSimulated(Identity id)
	{
		TFUtils.Assert(whitelistedIdentities.ContainsKey(id), "Trying to unrestrict a simulated by ID that is not tracked: " + id.Describe());
		Dictionary<Identity, int> dictionary2;
		Dictionary<Identity, int> dictionary = (dictionary2 = whitelistedIdentities);
		Identity key2;
		Identity key = (key2 = id);
		int num = dictionary2[key2];
		dictionary[key] = num - 1;
		if (whitelistedIdentities[id] <= 0)
		{
			whitelistedIdentities.Remove(id);
		}
	}

	public void UnWhitelistSimulated(int definitionId)
	{
		TFUtils.Assert(whitelistedDefinitions.ContainsKey(definitionId), "Trying to unrestrict a simulated by defId that is not tracked: " + definitionId);
		Dictionary<int, int> dictionary2;
		Dictionary<int, int> dictionary = (dictionary2 = whitelistedDefinitions);
		int key2;
		int key = (key2 = definitionId);
		key2 = dictionary2[key2];
		dictionary[key] = key2 - 1;
		if (whitelistedDefinitions[definitionId] <= 0)
		{
			whitelistedDefinitions.Remove(definitionId);
		}
	}

	public void WhitelistExpansion(int definitionId)
	{
		if (whitelistedExpansions.ContainsKey(definitionId))
		{
			Dictionary<int, int> dictionary2;
			Dictionary<int, int> dictionary = (dictionary2 = whitelistedExpansions);
			int key2;
			int key = (key2 = definitionId);
			key2 = dictionary2[key2];
			dictionary[key] = key2 + 1;
		}
		else
		{
			whitelistedExpansions[definitionId] = 1;
		}
	}

	public void UnWhitelistExpansion(int definitionId)
	{
		TFUtils.Assert(whitelistedExpansions.ContainsKey(definitionId), "Trying to unrestrict an expansion that is not tracked: " + definitionId);
		Dictionary<int, int> dictionary2;
		Dictionary<int, int> dictionary = (dictionary2 = whitelistedExpansions);
		int key2;
		int key = (key2 = definitionId);
		key2 = dictionary2[key2];
		dictionary[key] = key2 - 1;
		if (whitelistedExpansions[definitionId] <= 0)
		{
			whitelistedExpansions.Remove(definitionId);
		}
	}

	public void WhitelistSimulateds(ref List<Simulated> result)
	{
		if (whitelistedIdentities.Count == 0 && whitelistedDefinitions.Count == 0)
		{
			return;
		}
		List<Simulated> list = new List<Simulated>();
		foreach (Simulated item in result)
		{
			if (whitelistedIdentities.Count > 0 && whitelistedIdentities.ContainsKey(item.Id))
			{
				list.Add(item);
			}
			else if (whitelistedDefinitions.Count > 0 && whitelistedDefinitions.ContainsKey(item.entity.DefinitionId))
			{
				list.Add(item);
			}
		}
		result = list;
	}

	public bool CheckWhitelist(Simulated simulated)
	{
		return whitelistedIdentities.ContainsKey(simulated.entity.Id) || whitelistedDefinitions.ContainsKey(simulated.entity.DefinitionId);
	}

	private string PrintWhitelistedDefs()
	{
		string text = string.Empty;
		foreach (int key in whitelistedDefinitions.Keys)
		{
			string text2 = text;
			text = text2 + "{" + key + ":" + whitelistedDefinitions[key] + "}";
		}
		return text;
	}

	private string PrintWhitelistedIds()
	{
		string text = string.Empty;
		foreach (Identity key in whitelistedIdentities.Keys)
		{
			string text2 = text;
			text = text2 + "{" + key.Describe() + ":" + whitelistedIdentities[key] + "}";
		}
		return text;
	}

	public bool CheckExpansionAllowed(int did)
	{
		if (whitelistedExpansions.Count == 0)
		{
			return true;
		}
		return whitelistedExpansions.ContainsKey(did);
	}
}
