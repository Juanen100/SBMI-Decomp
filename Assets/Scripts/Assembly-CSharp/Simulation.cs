using System;
using System.Collections.Generic;
using UnityEngine;

public class Simulation
{
	public class Indexer<Key, Value>
	{
		private Dictionary<Key, Value> values;

		public Value Item
		{
			get
			{
				return default(Value);
			}
			set
			{
			}
		}

		public Indexer(Dictionary<Key, Value> values)
		{
		}
	}

	public class WaypointIndexer
	{
		private Dictionary<string, Waypoint> dictionary;

		private List<Waypoint> list;

		public Waypoint Item
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public WaypointIndexer(Dictionary<string, Waypoint> dictionary, List<Waypoint> list)
		{
		}
	}

	public delegate void RecordBufferAction(PersistedActionBuffer.PersistedAction action);

	public delegate void ModifyGameStateSimulatedFunction(Simulated simulated, PersistedSimulatedAction action);

	public delegate void ModifyGameStateFunction(PersistedTriggerableAction action);

	public class Placement
	{
		public enum RESULT
		{
			VALID = 0,
			INVALID = 1,
			CONDITIONAL = 2
		}
	}

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

	private Identity.Equality identityComperer;

	private const float TIME_STEP = 0.1f;

	private const string WORKER = "worker_0";

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
			return 0.0;
		}
	}

	public float TimeStep
	{
		get
		{
			return 0f;
		}
	}

	public float Interpolant
	{
		get
		{
			return 0f;
		}
	}

	public CommandRouter Router
	{
		get
		{
			return null;
		}
	}

	public Terrain Terrain
	{
		get
		{
			return null;
		}
	}

	public Scene Scene
	{
		get
		{
			return null;
		}
	}

	public WaypointIndexer Waypoint
	{
		get
		{
			return null;
		}
	}

	public EntityManager EntityManager
	{
		get
		{
			return null;
		}
	}

	public ItemDropManager DropManager
	{
		get
		{
			return null;
		}
	}

	public Camera TheCamera
	{
		get
		{
			return null;
		}
	}

	public bool Whitelisted
	{
		get
		{
			return false;
		}
	}

	public Simulation(ModifyGameStateFunction modifyGameState, ModifyGameStateSimulatedFunction modifyGameStateSimulated, Action<Simulated> rushSimulated, RecordBufferAction recordAction, Game game, EntityManager entityManager, TriggerRouter triggerRouter, ResourceManager resourceManager, ItemDropManager dropManager, SoundEffectManager soundEffectManager, ResourceCalculatorManager resourceCalculatorManager, CraftingManager craftManager, MovieManager movieManager, FeatureManager featureManager, Catalog catalog, RewardCap rewardCap, Camera camera, Terrain terrain, int depth, SBAnalytics analytics, SBGUIScreen scratchScreen, EnclosureManager enclosureManager)
	{
	}

	public Simulated CreateSimulated(Entity entity, Simulated.StateAction initialState, Vector2 position)
	{
		return null;
	}

	public Simulated CreateSimulated(string blueprint, Vector2 position)
	{
		return null;
	}

	public Simulated CreateSimulated(EntityType types, int did, Vector2 position)
	{
		return null;
	}

	private Simulated SetSimulated(Simulated simulated)
	{
		return null;
	}

	public void AddSimulated(Simulated simulated)
	{
	}

	public void RemoveSimulated(Simulated simulated)
	{
	}

	public void SendPendingCommands()
	{
	}

	public Simulated FindSimulated(Identity id)
	{
		return null;
	}

	public Simulated FindSimulated(int? did)
	{
		return null;
	}

	public Simulated FindSimulated(int? did, EntityType type)
	{
		return null;
	}

	public List<Simulated> FindAllSimulateds(int did, EntityType? type = null)
	{
		return null;
	}

	public IEnumerable<Simulated> GetSimulateds()
	{
		return null;
	}

	public List<Simulated> GetSimulatedRaw()
	{
		return null;
	}

	public Simulated SpawnWorker(Simulated simulated)
	{
		return null;
	}

	public Waypoint GetRandomWaypoint()
	{
		return null;
	}

	public void Clear()
	{
	}

	public Vector2 ScreenPositionFromWorldPosition(Vector3 worldPosition)
	{
		return default(Vector2);
	}

	public void OnUpdate(Session session)
	{
	}

	public void OnUpdateVisitParkState(Session session)
	{
	}

	public TerrainPathing CreatePathing(Vector2 start, Vector2 goal)
	{
		return null;
	}

	public void ResetAllAffectedPaths(AlignedBox box)
	{
	}

	public void HandleIfShouldRecalculatePath(Simulated simulated, GridPosition min, GridPosition max)
	{
	}

	public Placement.RESULT PlacementQuery(Simulated selected, ref List<Simulated> collisions, bool debrisOnly = false)
	{
		return default(Placement.RESULT);
	}

	public Placement.RESULT PlacementQuery(AlignedBox box, ref List<Simulated> collisions, Identity id = null, bool debrisOnly = false)
	{
		return default(Placement.RESULT);
	}

	public Placement.RESULT PlacementQuery(AlignedBox box, Identity id = null, bool debrisOnly = false)
	{
		return default(Placement.RESULT);
	}

	public Placement.RESULT PlacementQuery(Simulated selected, bool debrisOnly = false)
	{
		return default(Placement.RESULT);
	}

	private void AddWorkerSpawner(Simulated workerSpawner)
	{
	}

	public void TryWorkerSpawnerCleanup(Identity id)
	{
	}

	public Simulated GetClosestWorkerSpawner(Vector2 location)
	{
		return null;
	}

	private void Animate()
	{
	}

	private void Simulate(Session session)
	{
	}

	public void UpdateControls()
	{
	}

	public void UpdateDebugHitBoxes()
	{
	}

	public void UpdateDebugFootprints()
	{
	}

	public void UpdateDebugExpansionBorders()
	{
	}

	private void AddWaypoint(Simulated sim)
	{
	}

	private void RemoveWaypoint(Simulated sim)
	{
	}

	public void ClearPendingTimebarsInSimulateds()
	{
	}

	public void ClearPendingNamebarsInSimulateds()
	{
	}

	public void WhitelistSimulated(Identity id)
	{
	}

	public void WhitelistSimulated(int definitionId)
	{
	}

	public void UnWhitelistSimulated(Identity id)
	{
	}

	public void UnWhitelistSimulated(int definitionId)
	{
	}

	public void WhitelistExpansion(int definitionId)
	{
	}

	public void UnWhitelistExpansion(int definitionId)
	{
	}

	public void WhitelistSimulateds(ref List<Simulated> result)
	{
	}

	public bool CheckWhitelist(Simulated simulated)
	{
		return false;
	}

	private string PrintWhitelistedDefs()
	{
		return null;
	}

	private string PrintWhitelistedIds()
	{
		return null;
	}

	public bool CheckExpansionAllowed(int did)
	{
		return false;
	}
}
