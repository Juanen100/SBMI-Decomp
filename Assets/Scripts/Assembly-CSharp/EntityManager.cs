using System.Collections;
using System.Collections.Generic;

public class EntityManager
{
	private delegate Blueprint BlueprintMarshaller(Dictionary<string, object> data, EntityManager mgr);

	private delegate void BlueprintAssetsInitializer(Blueprint blueprint, Dictionary<string, object> data, EntityManager mgr);

	private static readonly string BLUEPRINT_DIRECTORY_PATH;

	public static bool MustRegenerateStates;

	private static Dictionary<string, BlueprintMarshaller> TypeRegistry;

	private static Dictionary<string, BlueprintAssetsInitializer> AssetsInitializerTypeRegistry;

	public static Dictionary<string, Simulated.StateAction> BuildingActions;

	private static StateMachine<Simulated.StateAction, Command.TYPE> BuildingMachine;

	public static Dictionary<string, Simulated.StateAction> AnnexActions;

	private static StateMachine<Simulated.StateAction, Command.TYPE> AnnexMachine;

	public static Dictionary<string, Simulated.StateAction> DebrisActions;

	private static StateMachine<Simulated.StateAction, Command.TYPE> DebrisMachine;

	public static Dictionary<string, Simulated.StateAction> LandmarkActions;

	private static StateMachine<Simulated.StateAction, Command.TYPE> LandmarkMachine;

	public static Dictionary<string, Simulated.StateAction> ResidentActions;

	private static StateMachine<Simulated.StateAction, Command.TYPE> UnitMachine;

	public static Dictionary<string, Simulated.StateAction> TreasureActions;

	private static StateMachine<Simulated.StateAction, Command.TYPE> TreasureMachine;

	public static Dictionary<string, Simulated.StateAction> WorkerActions;

	private static StateMachine<Simulated.StateAction, Command.TYPE> WorkerMachine;

	public static Dictionary<string, Simulated.StateAction> WandererActions;

	private static StateMachine<Simulated.StateAction, Command.TYPE> WandererMachine;

	private static Dictionary<string, Blueprint> blueprints;

	public const string FOOTPRINT_MATERIAL = "Materials/unique/footprint";

	private const string DROPSHADOW_TEXTURE = "dropshadow.tga";

	private string[] blueprintFilePaths;

	private IEnumerator blueprintFileEnumerator;

	private static Dictionary<string, object> _pBpSpreadData;

	private Dictionary<Blueprint, Dictionary<string, object>> blueprintsToData;

	private Factory<string, Entity> factory;

	private Dictionary<Identity, Entity> entities;

	private Dictionary<string, int> entityCount;

	private DisplayControllerManager displayControllerManager;

	public DisplayControllerManager DisplayControllerManager
	{
		get
		{
			return null;
		}
	}

	public Dictionary<string, Blueprint> Blueprints
	{
		get
		{
			return null;
		}
	}

	static EntityManager()
	{
	}

	public EntityManager(bool friendMode)
	{
	}

	public static void GenerateStates(bool friendMode)
	{
	}

	private static void RegisterDisplayOffset(Dictionary<string, object> data, string theKey, Blueprint blueprint)
	{
	}

	private static void RegisterTextureOrigin(Dictionary<string, object> data, string theKey, Blueprint blueprint)
	{
	}

	private static void RegisterHitArea(Dictionary<string, object> data, QuadHitObject hitObject, string theKey, Blueprint blueprint)
	{
	}

	private static TFAnimatedSprite CreateAnimatedSpritePrototype(Dictionary<string, object> data, string theKey, Blueprint blueprint, Dictionary<string, object> fullData)
	{
		return null;
	}

	private static TFAnimatedSprite CreateAnimatedSpritePrototype(Dictionary<string, object> data, string theKey, Blueprint blueprint, float width, float height, Dictionary<string, object> fullData)
	{
		return null;
	}

	private static void RegisterShareableSpaceSnap(Dictionary<string, object> data, Blueprint blueprint)
	{
	}

	private static void RegisterMeshName(Dictionary<string, object> data, Blueprint blueprint, string theKey)
	{
	}

	private static IDisplayController CreatePaperdollPrototype(Dictionary<string, object> data, string theKey, Blueprint blueprint, Paperdoll.PaperdollType paperdollType)
	{
		return null;
	}

	private static void LoadCostumeFromBlueprint(Dictionary<string, object> data, string theKey, Blueprint blueprint, EntityManager mgr, Paperdoll.PaperdollType paperdollType)
	{
	}

	private static void LoadDisplayController(Dictionary<string, object> data, string theKey, Blueprint blueprint, EntityManager mgr, Paperdoll.PaperdollType paperdollType)
	{
	}

	private static void LoadEffects(Dictionary<string, object> data, Blueprint blueprint)
	{
	}

	private static Blueprint MarshallCommon(Dictionary<string, object> data, int width, int height, EntityManager mgr)
	{
		return null;
	}

	private static BasicSprite CreateDropShadow(float width, float height)
	{
		return null;
	}

	private static void LoadUnitsFromSpread()
	{
	}

	private void LoadAnnexesFromSpread()
	{
	}

	private void LoadCharacterBuildingsFromSpread()
	{
	}

	private void LoadDebrisFromSpread()
	{
	}

	private void LoadDecorationsFromSpread()
	{
	}

	private void LoadLandmarksFromSpread()
	{
	}

	private void LoadRentOnlyBuildingsFromSpread()
	{
	}

	private void LoadShopsFromSpread()
	{
	}

	private void LoadTreasureFromSpread()
	{
	}

	private void LoadTreesFromSpread()
	{
	}

	private void OverwriteBlueprintDataWithSpread(Dictionary<string, object> data)
	{
	}

	private static Blueprint MarshallUnit(Dictionary<string, object> data, EntityManager mgr)
	{
		return null;
	}

	private static Blueprint MarshallBuilding(Dictionary<string, object> data, EntityManager mgr)
	{
		return null;
	}

	private static Blueprint MarshallAnnex(Dictionary<string, object> data, EntityManager mgr)
	{
		return null;
	}

	private static Blueprint MarshallDebris(Dictionary<string, object> data, EntityManager mgr)
	{
		return null;
	}

	private static Blueprint MarshallWorker(Dictionary<string, object> data, EntityManager mgr)
	{
		return null;
	}

	private static Blueprint MarshallWanderer(Dictionary<string, object> data, EntityManager mgr)
	{
		return null;
	}

	private static Blueprint MarshallLandmark(Dictionary<string, object> data, EntityManager mgr)
	{
		return null;
	}

	private static Blueprint MarshallTreasure(Dictionary<string, object> data, EntityManager mgr)
	{
		return null;
	}

	private static void MarshallWishingInfo(ref Blueprint blueprint, Dictionary<string, object> data)
	{
	}

	private static void MarshallBonusInfo(ref Blueprint blueprint, Dictionary<string, object> data)
	{
	}

	private static void MarshalResidentInfo(ref Blueprint blueprint, Dictionary<string, object> data)
	{
	}

	private static void MarshallHubInfo(ref Blueprint blueprint, Dictionary<string, object> data)
	{
	}

	private static void MarshallShuntedCraftingInfo(ref Blueprint blueprint, Dictionary<string, object> data)
	{
	}

	private static void InitializeBlueprintAssets(Blueprint blueprint, Dictionary<string, object> data, EntityManager mgr)
	{
	}

	private static void InitializeUnitAssets(Blueprint blueprint, Dictionary<string, object> data, EntityManager mgr)
	{
	}

	private static void InitializeWorkerAssets(Blueprint blueprint, Dictionary<string, object> data, EntityManager mgr)
	{
	}

	public Entity Create(EntityType types, int did, bool incrementCount)
	{
		return null;
	}

	public Entity Create(EntityType types, int did, Identity id, bool incrementCount)
	{
		return null;
	}

	public Entity Create(string blueprint, bool incrementCount)
	{
		return null;
	}

	public Entity Create(string blueprint, Identity id, bool incrementCount)
	{
		return null;
	}

	public static Blueprint GetBlueprint(string primaryType, int did, bool ignoreNotFoundError = false)
	{
		return null;
	}

	public static Blueprint GetBlueprint(EntityType type, int did, bool ignoreNotFoundError = false)
	{
		return null;
	}

	public static List<string> GetAllBuildingBlueprintKeys()
	{
		return null;
	}

	public void Destroy(Identity id)
	{
	}

	public Entity GetEntity(Identity id)
	{
		return null;
	}

	public int GetEntityCount(EntityType primaryType, int did)
	{
		return 0;
	}

	public ICollection<Entity> GetEntities()
	{
		return null;
	}

	private void LoadBlueprintsFromFile(string filePath)
	{
	}

	private Blueprint LoadBlueprintFromDict(Dictionary<string, object> dict)
	{
		return null;
	}

	private void LoadResources(Blueprint blueprint, EntityManager mgr)
	{
	}

	public void LoadBlueprints()
	{
	}

	public bool IterateLoadOfBlueprints()
	{
		return false;
	}

	public void LoadBlueprintResources()
	{
	}

	private string[] GetFilesToLoad()
	{
		return null;
	}

	private string GetFilePathFromString(string filePath)
	{
		return null;
	}

	private void IncrementEntityCount(string blueprint)
	{
	}
}
