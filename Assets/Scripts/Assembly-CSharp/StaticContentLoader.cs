public class StaticContentLoader
{
	private CraftingManager _craftManager;

	private VendingManager _vendingManager;

	private TreasureManager _treasureManager;

	private PaytableManager _paytableManager;

	private MovieManager _movieManager;

	private Terrain _terrain;

	private Border _border;

	private ResourceManager _resourceManager;

	private Catalog _catalog;

	private LevelingManager _levelingManager;

	private FeatureManager _featureManager;

	private BuildingUnlockManager _buildingUnlockManager;

	private EnclosureManager _enclosureManager;

	private CommunityEventManager _communityEventManager;

	private TaskManager _taskManager;

	private MicroEventManager _microEventManager;

	private CostumeManager _costumeManager;

	private WishTableManager _wishTableManager;

	private EntityManager _entities;

	private QuestManager _questManager;

	private AutoQuestDatabase _autoQuestDatabase;

	public TaskManager TheTaskManager
	{
		get
		{
			return _taskManager;
		}
	}

	public MicroEventManager TheMicroEventManager
	{
		get
		{
			return _microEventManager;
		}
	}

	public CostumeManager TheCostumeManager
	{
		get
		{
			return _costumeManager;
		}
	}

	public WishTableManager TheWishTableManager
	{
		get
		{
			return _wishTableManager;
		}
	}

	public CraftingManager TheCraftingManager
	{
		get
		{
			return _craftManager;
		}
	}

	public VendingManager TheVendingManager
	{
		get
		{
			return _vendingManager;
		}
	}

	public TreasureManager TheTreasureManager
	{
		get
		{
			return _treasureManager;
		}
	}

	public CommunityEventManager TheCommunityEventManager
	{
		get
		{
			return _communityEventManager;
		}
	}

	public PaytableManager ThePaytableManager
	{
		get
		{
			return _paytableManager;
		}
	}

	public MovieManager TheMovieManager
	{
		get
		{
			return _movieManager;
		}
	}

	public Terrain TheTerrain
	{
		get
		{
			return _terrain;
		}
	}

	public Border TheBorder
	{
		get
		{
			return _border;
		}
	}

	public ResourceManager TheResourceManager
	{
		get
		{
			return _resourceManager;
		}
	}

	public Catalog TheCatalog
	{
		get
		{
			return _catalog;
		}
	}

	public EntityManager TheEntityManager
	{
		get
		{
			return _entities;
		}
	}

	public QuestManager TheQuestManager
	{
		get
		{
			return _questManager;
		}
	}

	public LevelingManager TheLevelingManager
	{
		get
		{
			return _levelingManager;
		}
	}

	public FeatureManager TheFeatureManager
	{
		get
		{
			return _featureManager;
		}
	}

	public BuildingUnlockManager TheBuildingUnlockManager
	{
		get
		{
			return _buildingUnlockManager;
		}
	}

	public EnclosureManager TheEnclosureManager
	{
		get
		{
			return _enclosureManager;
		}
	}

	public AutoQuestDatabase TheAutoQuestDatabase
	{
		get
		{
			return _autoQuestDatabase;
		}
	}

	public void LoadContent(Session session)
	{
		DatabaseManager.Instance.LoadDatabaseFromCSV();
		_resourceManager = new ResourceManager(session);
		_craftManager = new CraftingManager();
		_vendingManager = new VendingManager();
		_treasureManager = new TreasureManager(session);
		_paytableManager = new PaytableManager();
		_featureManager = new FeatureManager();
		_buildingUnlockManager = new BuildingUnlockManager();
		_movieManager = new MovieManager();
		_terrain = new Terrain(0);
		_border = new Border();
		_levelingManager = new LevelingManager();
		_catalog = new Catalog();
		_autoQuestDatabase = new AutoQuestDatabase();
		_questManager = new QuestManager();
		_entities = new EntityManager(session.InFriendsGame);
		_enclosureManager = new EnclosureManager();
		_communityEventManager = new CommunityEventManager(session);
		_taskManager = new TaskManager();
		_microEventManager = new MicroEventManager();
		_costumeManager = new CostumeManager();
		_wishTableManager = new WishTableManager();
	}

	public void Initialize()
	{
		_terrain.Initialize();
		_border.Initialize(_terrain);
	}

	public bool LoadNextBlueprint()
	{
		return _entities.IterateLoadOfBlueprints();
	}
}
