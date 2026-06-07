using UnityEngine;

public class TerrainSector
{
	public const int SECTOR_TILE_SIZE = 6;

	private const int INVALID_IMAGE_INDEX = -1;

	private const int TILE_VERTEX_COUNT = 4;

	private const int TILE_INDEX_COUNT = 6;

	private const int SECTOR_TILE_MAXIMUM = 36;

	private static TerrainType defaultTerrain;

	private GridPosition position;

	private int vertexCount;

	private TerrainVertex[] vertex;

	private bool isHighlighted;

	private int indexCount;

	private int[] index;

	private GameObject gameObject;

	private Mesh mesh;

	private static bool useRotatedTiles;

	private static float[] originalUs;

	private static float[] originalVs;

	private static float[] rotatedUs;

	private static float[] rotatedVs;

	public bool Highlighted
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public static int TileMaximum
	{
		get
		{
			return 0;
		}
	}

	public TerrainSector(int renderOrder, int row, int col)
	{
	}

	public void Destroy()
	{
	}

	private void CreateQuad(float resolution, int row, int col, Rect? coords, byte rotationIndex)
	{
	}

	private int CreateVertex(float x, float y, float u, float v)
	{
		return 0;
	}

	private void UpdateMesh()
	{
	}

	public void Initialize(Terrain terrain, int sectorRow, int sectorCol)
	{
	}
}
