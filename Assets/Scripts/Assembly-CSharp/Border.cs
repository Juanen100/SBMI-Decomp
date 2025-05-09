#define ASSERTS_ON
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;
using Yarg;

public class Border
{
	private const int TransparentRenderQueueStart = 3000;

	private const string BORDER_OBJ_NAME = "borderObj";

	public static bool BorderEnabled = false;

	private static string borderDecorFile = TFUtils.GetStreamingAssetsFileInDirectory("Border", "border_decor.json");

	private Vector3[] terrVertices;

	private Vector2[] terrUVs;

	private int terrVertCount;

	private GameObject stripBorderGO;

	private Vector3[] terrStripVertices;

	private Vector2[] terrStripUVs;

	private int terrStripVertCount;

	private float borderEpsilon = 0.01f;

	private float topBorderXOffset = 5f;

	private float nonTopBorderWidth = 400f;

	private float topBorderTileSize = 160f;

	private int topBackBorderRows = 2;

	private void CreateTerrainBorder(Terrain terrain)
	{
		if (BorderEnabled)
		{
			GameObject gameObject = UnityGameResources.Create("Prefabs/TerrainBorder");
			gameObject.name = "BorderFlatTerrain";
			if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Standard)
			{
				Material sharedMaterial = gameObject.GetComponent<Renderer>().sharedMaterial;
				gameObject.GetComponent<Renderer>().material = Resources.Load("Materials/lod/terrainBorder_lr") as Material;
				Resources.UnloadAsset(sharedMaterial);
			}
			else if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Low)
			{
				Material sharedMaterial2 = gameObject.GetComponent<Renderer>().sharedMaterial;
				gameObject.GetComponent<Renderer>().material = Resources.Load("Materials/lod/terrainBorder_lr2") as Material;
				Resources.UnloadAsset(sharedMaterial2);
			}
			Mesh mesh = new Mesh();
			gameObject.GetComponent<MeshFilter>().mesh = mesh;
			int num = 12;
			terrVertices = new Vector3[num];
			terrUVs = new Vector2[num];
			float num2 = nonTopBorderWidth;
			float num3 = terrain.WorldWidth;
			float num4 = terrain.WorldHeight;
			float num5 = 20f;
			float uvScale = 0.05f;
			terrVertCount = 0;
			AddTerrBorderVertex(num3 + num5 - borderEpsilon, 0f - num2, uvScale);
			AddTerrBorderVertex(num3 + num5 - borderEpsilon, borderEpsilon, uvScale);
			AddTerrBorderVertex(borderEpsilon, 0f - num2, uvScale);
			AddTerrBorderVertex(borderEpsilon, borderEpsilon, uvScale);
			AddTerrBorderVertex(0f - num2, 0f - num2, uvScale);
			AddTerrBorderVertex(0f - num2, borderEpsilon, uvScale);
			AddTerrBorderVertex(0f - num2, num4 - borderEpsilon, uvScale);
			AddTerrBorderVertex(0f - num2, num4 + num2, uvScale);
			AddTerrBorderVertex(borderEpsilon, num4 - borderEpsilon, uvScale);
			AddTerrBorderVertex(borderEpsilon, num4 + num2, uvScale);
			AddTerrBorderVertex(num3 + num5 - borderEpsilon, num4 - borderEpsilon, uvScale);
			AddTerrBorderVertex(num3 + num5 - borderEpsilon, num4 + num2, uvScale);
			int[] triangles = new int[18]
			{
				0, 1, 2, 3, 4, 5, 5, 3, 6, 8,
				8, 7, 7, 6, 9, 8, 11, 10
			};
			mesh.vertices = terrVertices;
			mesh.uv = terrUVs;
			mesh.SetTriangles(triangles, 0);
			terrVertices = null;
			terrUVs = null;
		}
	}

	private void AddTerrBorderVertex(float x, float y, float uvScale)
	{
		terrVertices[terrVertCount].x = x;
		terrVertices[terrVertCount].y = y;
		terrVertices[terrVertCount].z = 0f;
		terrUVs[terrVertCount].x = x * uvScale;
		terrUVs[terrVertCount].y = y * uvScale;
		terrVertCount++;
	}

	public void UpdateTerrainBorderStrip(Terrain terrain)
	{
		if (BorderEnabled)
		{
			Mesh mesh;
			if (stripBorderGO == null)
			{
				stripBorderGO = new GameObject("BorderStrip");
				MeshFilter meshFilter = stripBorderGO.AddComponent<MeshFilter>();
				mesh = (meshFilter.mesh = new Mesh());
				MeshRenderer meshRenderer = stripBorderGO.AddComponent<MeshRenderer>();
				meshRenderer.castShadows = false;
				meshRenderer.receiveShadows = false;
				meshRenderer.material = Terrain.TerrainMaterial;
				meshRenderer.material.renderQueue = 2998;
				stripBorderGO.transform.localPosition = new Vector3(terrain.WorldWidth, 0f, 0f);
			}
			else
			{
				mesh = stripBorderGO.GetComponent<MeshFilter>().mesh;
				mesh.Clear();
			}
			int gridHeight = terrain.GridHeight;
			int col = terrain.GridWidth - 1;
			int num = gridHeight * 4;
			int num2 = gridHeight * 6;
			terrStripVertices = new Vector3[num];
			terrStripUVs = new Vector2[num];
			int[] array = new int[num2];
			Rect? materialUVs = terrain.terrainTextures.GetMaterialUVs("disabled_terrainXL.png");
			Rect? materialUVs2 = terrain.terrainTextures.GetMaterialUVs("disabled_terrainXX.png");
			int num3 = 0;
			float num4 = 0f;
			int num5 = 0;
			terrStripVertCount = 0;
			for (int i = 0; i < gridHeight; i++)
			{
				Rect rect = ((!terrain.CheckIsPurchasedArea(i, col)) ? materialUVs2.Value : materialUVs.Value);
				AddBorderStripVertex(0f, num4, rect.xMin, rect.yMin);
				AddBorderStripVertex(20f, num4, rect.xMax, rect.yMin);
				AddBorderStripVertex(20f, num4 + 20f, rect.xMax, rect.yMax);
				AddBorderStripVertex(0f, num4 + 20f, rect.xMin, rect.yMax);
				num4 += 20f;
				array[num5++] = num3;
				array[num5++] = num3 + 1;
				array[num5++] = num3 + 2;
				array[num5++] = num3;
				array[num5++] = num3 + 2;
				array[num5++] = num3 + 3;
				num3 += 4;
			}
			TFUtils.Assert(num5 == num2, "bad indices in terrain strip");
			mesh.vertices = terrStripVertices;
			mesh.uv = terrStripUVs;
			mesh.triangles = array;
			terrStripVertices = null;
			terrStripUVs = null;
			array = null;
		}
	}

	private void AddBorderStripVertex(float x, float y, float u, float v)
	{
		terrStripVertices[terrStripVertCount].x = x;
		terrStripVertices[terrStripVertCount].y = y;
		terrStripVertices[terrStripVertCount].z = 0f;
		terrStripUVs[terrStripVertCount].x = u;
		terrStripUVs[terrStripVertCount].y = v;
		terrStripVertCount++;
	}

	public void CreateTerrainTopBorder(Terrain terrain, float tileSize, int numRows, bool front)
	{
		if (!BorderEnabled)
		{
			return;
		}
		GameObject gameObject = new GameObject((!front) ? "BorderTopBack" : "BorderTopFront");
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		Mesh mesh = (meshFilter.mesh = new Mesh());
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		meshRenderer.castShadows = false;
		meshRenderer.receiveShadows = false;
		float num = (float)terrain.WorldHeight + nonTopBorderWidth;
		float num2 = num + nonTopBorderWidth;
		float num3 = (float)terrain.WorldWidth + topBorderXOffset;
		if (front)
		{
			if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Standard)
			{
				meshRenderer.material = Resources.Load("Materials/lod/TerrainBorderTopFront_lr") as Material;
			}
			else if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Low)
			{
				meshRenderer.material = Resources.Load("Materials/lod/TerrainBorderTopFront_lr2") as Material;
			}
			else
			{
				meshRenderer.material = Resources.Load("Materials/lod/TerrainBorderTopFront") as Material;
			}
			meshRenderer.material.renderQueue = 2999;
		}
		else
		{
			if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Standard)
			{
				meshRenderer.material = Resources.Load("Materials/lod/TerrainBorderTopBack_lr") as Material;
			}
			else if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Low)
			{
				meshRenderer.material = Resources.Load("Materials/lod/TerrainBorderTopBack_lr2") as Material;
			}
			else
			{
				meshRenderer.material = Resources.Load("Materials/lod/TerrainBorderTopBack") as Material;
			}
			num3 += tileSize;
		}
		gameObject.transform.localPosition = new Vector3(num3, num, 0.3f);
		int num4 = (int)((num2 + 0.99f * tileSize) / tileSize);
		int num5 = num4 * 4;
		int num6 = num4 * 6;
		terrStripVertices = new Vector3[num5];
		terrStripUVs = new Vector2[num5];
		int[] array = new int[num6];
		int num7 = 0;
		float num8 = 0f;
		int num9 = 0;
		float x = tileSize * (float)numRows;
		terrStripVertCount = 0;
		for (int i = 0; i < num4; i++)
		{
			AddBorderStripVertex(0f, num8 - tileSize, 0f, 0f);
			AddBorderStripVertex(x, num8 - tileSize, 0f, numRows);
			AddBorderStripVertex(x, num8, 1f, numRows);
			AddBorderStripVertex(0f, num8, 1f, 0f);
			num8 -= tileSize;
			array[num9++] = num7;
			array[num9++] = num7 + 1;
			array[num9++] = num7 + 2;
			array[num9++] = num7;
			array[num9++] = num7 + 2;
			array[num9++] = num7 + 3;
			num7 += 4;
		}
		TFUtils.Assert(num9 == num6, "bad indices in terrain strip");
		mesh.vertices = terrStripVertices;
		mesh.uv = terrStripUVs;
		mesh.triangles = array;
		terrStripVertices = null;
		terrStripUVs = null;
		array = null;
	}

	private void CreateBorderObjects()
	{
		if (!BorderEnabled)
		{
			return;
		}
		string json = TFUtils.ReadAllText(borderDecorFile);
		List<object> list = (List<object>)Json.Deserialize(json);
		Camera main = Camera.main;
		Vector3 zero = Vector3.zero;
		int num = 0;
		Vector3 v = Vector3.zero;
		foreach (object item in list)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)item;
			float width = TFUtils.LoadFloat(dictionary, "width");
			float num2 = TFUtils.LoadFloat(dictionary, "height");
			string sprite = (string)dictionary["sprite"];
			TFUtils.LoadVector3(out v, (Dictionary<string, object>)dictionary["position"]);
			GameObject gameObject = UnityGameResources.CreateEmpty("borderObj");
			zero.y = (0f - num2) * 0.5f;
			Rect? rect = null;
			Material meterialAndUVs = GetMeterialAndUVs(sprite, ref rect);
			TFQuad.SetupQuad(gameObject, meterialAndUVs, width, num2, zero, rect);
			gameObject.transform.localPosition = v;
			gameObject.transform.LookAt(v - main.transform.forward, main.transform.up);
			num++;
		}
	}

	public static void UpdateBorderObjects()
	{
		if (BorderEnabled)
		{
		}
	}

	public static void SaveBorderObjects()
	{
		if (BorderEnabled)
		{
		}
	}

	private static Material GetMeterialAndUVs(string sprite, ref Rect? rect)
	{
		int num = sprite.LastIndexOf('/');
		Material result;
		if (num >= 0)
		{
			result = TextureLibrarian.LookUp(sprite);
		}
		else
		{
			AtlasAndCoords atlasCoords = YGTextureLibrary.GetAtlasCoords(sprite);
			result = TextureLibrarian.LookUp("Materials/lod/" + atlasCoords.atlas.name);
			Rect rect2 = default(Rect);
			atlasCoords.atlas.GetUVs(atlasCoords.atlasCoords, ref rect2);
			rect = rect2;
		}
		return result;
	}

	public void Initialize(Terrain terrain)
	{
		if (BorderEnabled)
		{
			CreateTerrainBorder(terrain);
			CreateTerrainTopBorder(terrain, topBorderTileSize, 1, true);
			CreateTerrainTopBorder(terrain, topBorderTileSize, topBackBorderRows, false);
			CreateBorderObjects();
		}
	}
}
