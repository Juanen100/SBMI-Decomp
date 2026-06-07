using UnityEngine;

public class Border
{
	public static bool BorderEnabled;

	private const int TransparentRenderQueueStart = 3000;

	private const string BORDER_OBJ_NAME = "borderObj";

	private static string borderDecorFile;

	private Vector3[] terrVertices;

	private Vector2[] terrUVs;

	private int terrVertCount;

	private GameObject stripBorderGO;

	private Vector3[] terrStripVertices;

	private Vector2[] terrStripUVs;

	private int terrStripVertCount;

	private float borderEpsilon;

	private float topBorderXOffset;

	private float nonTopBorderWidth;

	private float topBorderTileSize;

	private int topBackBorderRows;

	private void CreateTerrainBorder(Terrain terrain)
	{
	}

	private void AddTerrBorderVertex(float x, float y, float uvScale)
	{
	}

	public void UpdateTerrainBorderStrip(Terrain terrain)
	{
	}

	private void AddBorderStripVertex(float x, float y, float u, float v)
	{
	}

	public void CreateTerrainTopBorder(Terrain terrain, float tileSize, int numRows, bool front)
	{
	}

	private void CreateBorderObjects()
	{
	}

	public static void UpdateBorderObjects()
	{
	}

	public static void SaveBorderObjects()
	{
	}

	private static Material GetMeterialAndUVs(string sprite, ref Rect? rect)
	{
		return null;
	}

	public void Initialize(Terrain terrain)
	{
	}
}
