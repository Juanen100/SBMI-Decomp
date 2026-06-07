using System.Collections.Generic;
using UnityEngine;

public class Terrain
{
	public const byte INVALID_TERRAIN_COST = byte.MaxValue;

	public const byte OBSTACLE_COST = 120;

	public const byte UNPURCHASED_COST = 20;

	public const byte TERRAIN_TYPE_INVALID = byte.MaxValue;

	public const int TERRAIN_TILE_WORLDSIZE = 20;

	public static readonly string TERRAIN_PATH;

	public static Vector3 UP;

	public static float terrainTextureScaleU;

	public static float terrainTextureScaleV;

	public static float terrainTextureInvScaleU;

	public static float terrainTextureInvScaleV;

	public List<Cost> expansionCosts;

	public TerrainTextureLibrary terrainTextures;

	public HashSet<int> purchasedSlots;

	public TerrainSlot selectedSlot;

	private Dictionary<int, TerrainType> terrainTypes;

	private Dictionary<int, TerrainSlot> slots;

	private byte[,] tiles;

	private byte[,] nonPathTiles;

	private TerrainSector[,] sectors;

	private int terrainSeed;

	private byte backgroundTerrain;

	private int sectorWidth;

	private int sectorHeight;

	private Rect sectorInset;

	private AlignedBox worldExtent;

	private AlignedBox purchasedExtent;

	private AlignedBox footprintGuide;

	private AlignedBox cameraExtents;

	private bool[,] obstacles;

	private bool[,] purchasedSectors;

	private List<TerrainNode> foregroundOverrides;

	private List<KeyValuePair<int, float>> distribution;

	private bool meshesCreated;

	private static Material mTerrainMaterial;

	public static Material TerrainMaterial
	{
		get
		{
			return null;
		}
	}

	public TerrainType BackgroundTerrainType
	{
		get
		{
			return null;
		}
	}

	public int GridWidth
	{
		get
		{
			return 0;
		}
	}

	public int GridHeight
	{
		get
		{
			return 0;
		}
	}

	public int WorldWidth
	{
		get
		{
			return 0;
		}
	}

	public int WorldHeight
	{
		get
		{
			return 0;
		}
	}

	public AlignedBox WorldExtent
	{
		get
		{
			return null;
		}
	}

	public AlignedBox PurchasedExtent
	{
		get
		{
			return null;
		}
	}

	public AlignedBox CameraExtents
	{
		get
		{
			return null;
		}
	}

	public AlignedBox FootprintGuide
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public Dictionary<int, TerrainSlot> ExpansionSlots
	{
		get
		{
			return null;
		}
	}

	public Terrain(int terrainSeed)
	{
	}

	private void LoadTerrain()
	{
	}

	private void LoadTerrainFromSpread()
	{
	}

	private void LoadTerrainSlotsFromSpread()
	{
	}

	private void LoadTerrainTypesFromSpread()
	{
	}

	private void LoadTerrain(Dictionary<string, object> data)
	{
	}

	public void Initialize()
	{
	}

	public void CreateTerrainMeshes()
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

	public void Destroy()
	{
	}

	public bool ChangePath(GridPosition gpos)
	{
		return false;
	}

	public GridPosition ComputeGridPosition(Vector2 worldPosition)
	{
		return null;
	}

	public Vector2 ComputeWorldPosition(GridPosition gridPosition)
	{
		return default(Vector2);
	}

	public Vector3 ConstrainToAlignedBox(Vector3 position, AlignedBox footprint)
	{
		return default(Vector3);
	}

	public Vector3 CalculateNearestGridPosition(Vector3 position, AlignedBox footprint)
	{
		return default(Vector3);
	}

	public bool ComputeIntersection(Ray ray, out Vector3 point)
	{
		point = default(Vector3);
		return false;
	}

	public byte GetTerrainCost(int row, int col)
	{
		return 0;
	}

	public float GetTerrainCost(GridPosition gridPosition)
	{
		return 0f;
	}

	public float GetTerrainCost(Vector2 worldPosition)
	{
		return 0f;
	}

	public void SetOrClearObstacle(AlignedBox box, bool isSet)
	{
	}

	private void SetObstacleAtCoords(float x, float y, bool isSet)
	{
	}

	public bool CheckIsPurchasedArea(AlignedBox box)
	{
		return false;
	}

	public bool CheckIsPurchasedArea(Vector2 point)
	{
		return false;
	}

	public bool CheckIsPurchasedArea(int row, int col)
	{
		return false;
	}

	public void MarkPurchase(TerrainSlot slot)
	{
	}

	private AlignedBox GetSectorBounds(int row, int col)
	{
		return null;
	}

	private AlignedBox GetCameraBounds(int row, int col)
	{
		return null;
	}

	public AlignedBox GetGridBounds(int row, int col)
	{
		return null;
	}

	public void AddExpansionSlot(int id)
	{
	}

	public void AddRandomAvailableSlot(Game game)
	{
	}

	public void AddAndClearExpansionSlot(Game pGame, int nDID)
	{
	}

	public bool IsBorderSlot(int id)
	{
		return false;
	}

	public void HighlightSelection(TerrainSlot slot)
	{
	}

	public void DropSelection(TerrainSlot slot)
	{
	}

	public void OutlineAvailableExpansionSlots(Game game)
	{
	}

	public void HideAvailableExpansionSlots()
	{
	}

	public void OutlineAllExpansionSlots()
	{
	}

	public void HideAllExpansionSlots()
	{
	}

	public void UpdateRealtySigns(DisplayControllerManager dcm, BillboardDelegate billboard, Game game)
	{
	}

	public List<TerrainSlot> UnpurchasedExpansionSlots()
	{
		return null;
	}

	public TerrainType GetTerrainType(int row, int col)
	{
		return null;
	}

	public bool ProcessTap(Ray ray, Game game)
	{
		return false;
	}

	public TerrainSlot CheckTap(Ray ray, Game game)
	{
		return null;
	}

	public Cost GetExpansionCost(TerrainSlot slot)
	{
		return null;
	}

	public bool IsTerrainSectorDisabled(int sectorRow, int sectorCol)
	{
		return false;
	}

	public bool IsTerrainSectorBoardwalk(int sectorRow, int sectorCol)
	{
		return false;
	}

	public TerrainType GetTerrainType(GridPosition gridPosition)
	{
		return null;
	}

	public TerrainType GetTerrainType(Vector2 worldPosition)
	{
		return null;
	}

	public TerrainType GetTerrainType(int type)
	{
		return null;
	}

	public int GetTerrainIdAt(int row, int col)
	{
		return 0;
	}

	private void Decal()
	{
	}

	private byte GenerateDecal(int row, int col)
	{
		return 0;
	}

	private bool ValidTileIndex(int row, int col)
	{
		return false;
	}

	private bool ValidSectorIndex(GridPosition pos)
	{
		return false;
	}

	private bool ValidSectorIndex(int row, int col)
	{
		return false;
	}

	private bool ValidSector(int sectorRow, int sectorCol)
	{
		return false;
	}

	private bool ValidTileIndex(GridPosition pos)
	{
		return false;
	}

	private bool HasObstacle(int row, int col)
	{
		return false;
	}

	private void UpdateSectors(int gridRow, int gridCol)
	{
	}

	private void UpdateSingleSector(int sectorRow, int sectorCol)
	{
	}

	private void UpdateAllSurroundingSectors(int sectorRow, int sectorCol)
	{
	}

	private AlignedBox ComputeVisibleBounds()
	{
		return null;
	}

	private byte GenerateTerrainTile(int row, int col)
	{
		return 0;
	}

	private int GetSeed(int row, int col)
	{
		return 0;
	}

	public static List<TerrainNode> LoadTerrainNodeData(List<object> data)
	{
		return null;
	}

	private void ProcessOverrides()
	{
	}
}
