#define ASSERTS_ON
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

	private static bool useRotatedTiles = true;

	private static float[] originalUs = new float[4];

	private static float[] originalVs = new float[4];

	private static float[] rotatedUs = new float[4];

	private static float[] rotatedVs = new float[4];

	public bool Highlighted
	{
		get
		{
			return isHighlighted;
		}
		set
		{
			isHighlighted = value;
			if (isHighlighted)
			{
				gameObject.GetComponent<Renderer>().material.color = new Color(0.75f, 0.75f, 0.75f);
			}
			else
			{
				gameObject.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f);
			}
		}
	}

	public static int TileMaximum
	{
		get
		{
			return 36;
		}
	}

	public TerrainSector(int renderOrder, int row, int col)
	{
		int num = 144;
		vertexCount = 0;
		vertex = new TerrainVertex[num];
		int num2 = 216;
		indexCount = 0;
		index = new int[num2];
		isHighlighted = false;
		gameObject = new GameObject("TerrainSector" + row + "x" + col);
		UnityGameResources.AddGameObject(gameObject);
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		meshRenderer.sharedMaterial = Terrain.TerrainMaterial;
		mesh = new Mesh();
		meshFilter.mesh = mesh;
		if (Terrain.terrainTextureScaleU == 0f)
		{
			Texture mainTexture = meshRenderer.sharedMaterial.mainTexture;
			if (mainTexture.width > mainTexture.height)
			{
				Terrain.terrainTextureScaleU = 1f;
				Terrain.terrainTextureScaleV = mainTexture.width / mainTexture.height;
			}
			else
			{
				Terrain.terrainTextureScaleU = mainTexture.height / mainTexture.width;
				Terrain.terrainTextureScaleV = 1f;
			}
			Terrain.terrainTextureInvScaleU = 1f / Terrain.terrainTextureScaleU;
			Terrain.terrainTextureInvScaleV = 1f / Terrain.terrainTextureScaleV;
		}
		meshRenderer.material.renderQueue = renderOrder;
	}

	public void Destroy()
	{
		mesh.Clear();
	}

	private void CreateQuad(float resolution, int row, int col, Rect? coords, byte rotationIndex)
	{
		if (!coords.HasValue)
		{
			return;
		}
		Rect value = coords.Value;
		if (vertexCount + 4 > vertex.Length)
		{
			return;
		}
		if (rotationIndex == 5)
		{
			rotatedUs[0] = value.xMin;
			rotatedVs[0] = value.yMax;
			rotatedUs[1] = value.xMin;
			rotatedVs[1] = value.yMin;
			rotatedUs[2] = value.xMax;
			rotatedVs[2] = value.yMax;
			rotatedUs[3] = value.xMax;
			rotatedVs[3] = value.yMin;
		}
		else if (rotationIndex == 6)
		{
			rotatedUs[0] = value.xMax;
			rotatedVs[0] = value.yMin;
			rotatedUs[1] = value.xMax;
			rotatedVs[1] = value.yMax;
			rotatedUs[2] = value.xMin;
			rotatedVs[2] = value.yMin;
			rotatedUs[3] = value.xMin;
			rotatedVs[3] = value.yMax;
		}
		else if (rotationIndex == 7)
		{
			rotatedUs[0] = value.xMax;
			rotatedVs[0] = value.yMax;
			rotatedUs[1] = value.xMax;
			rotatedVs[1] = value.yMin;
			rotatedUs[2] = value.xMin;
			rotatedVs[2] = value.yMax;
			rotatedUs[3] = value.xMin;
			rotatedVs[3] = value.yMin;
		}
		else if (rotationIndex > 0)
		{
			float x = value.center.x;
			float y = value.center.y;
			originalUs[0] = (value.xMin - x) * Terrain.terrainTextureInvScaleU;
			originalVs[0] = (value.yMin - y) * Terrain.terrainTextureInvScaleV;
			originalUs[1] = (value.xMin - x) * Terrain.terrainTextureInvScaleU;
			originalVs[1] = (value.yMax - y) * Terrain.terrainTextureInvScaleV;
			originalUs[2] = (value.xMax - x) * Terrain.terrainTextureInvScaleU;
			originalVs[2] = (value.yMin - y) * Terrain.terrainTextureInvScaleV;
			originalUs[3] = (value.xMax - x) * Terrain.terrainTextureInvScaleU;
			originalVs[3] = (value.yMax - y) * Terrain.terrainTextureInvScaleV;
			float num;
			float num2;
			float num3;
			float num4;
			switch (rotationIndex)
			{
			case 1:
				num = 0f;
				num2 = Terrain.terrainTextureScaleV;
				num3 = 0f - Terrain.terrainTextureScaleU;
				num4 = 0f;
				break;
			case 2:
				num = 0f - Terrain.terrainTextureScaleU;
				num2 = 0f;
				num3 = 0f;
				num4 = 0f - Terrain.terrainTextureScaleV;
				break;
			case 3:
				num = 0f;
				num2 = 0f - Terrain.terrainTextureScaleV;
				num3 = Terrain.terrainTextureScaleU;
				num4 = 0f;
				break;
			default:
				num = 1f;
				num2 = 0f;
				num3 = 0f;
				num4 = 1f;
				break;
			}
			for (int i = 0; i < 4; i++)
			{
				rotatedUs[i] = originalUs[i] * num + originalVs[i] * num3 + x;
				rotatedVs[i] = originalUs[i] * num2 + originalVs[i] * num4 + y;
			}
		}
		else
		{
			rotatedUs[0] = value.xMin;
			rotatedVs[0] = value.yMin;
			rotatedUs[1] = value.xMin;
			rotatedVs[1] = value.yMax;
			rotatedUs[2] = value.xMax;
			rotatedVs[2] = value.yMin;
			rotatedUs[3] = value.xMax;
			rotatedVs[3] = value.yMax;
		}
		int num5 = CreateVertex((float)col * resolution, (float)row * resolution, rotatedUs[0], rotatedVs[0]);
		int num6 = CreateVertex((float)col * resolution, (float)(row + 1) * resolution, rotatedUs[1], rotatedVs[1]);
		int num7 = CreateVertex((float)(col + 1) * resolution, (float)row * resolution, rotatedUs[2], rotatedVs[2]);
		int num8 = CreateVertex((float)(col + 1) * resolution, (float)(row + 1) * resolution, rotatedUs[3], rotatedVs[3]);
		index[indexCount++] = num7;
		index[indexCount++] = num6;
		index[indexCount++] = num5;
		index[indexCount++] = num6;
		index[indexCount++] = num7;
		index[indexCount++] = num8;
	}

	private int CreateVertex(float x, float y, float u, float v)
	{
		int num = vertexCount;
		vertex[num].position.x = x;
		vertex[num].position.y = y;
		vertex[num].position.z = 0f;
		vertex[num].texcoord.x = u;
		vertex[num].texcoord.y = v;
		vertexCount++;
		return num;
	}

	private void UpdateMesh()
	{
		mesh.Clear();
		Vector3[] array = new Vector3[vertexCount];
		Vector2[] array2 = new Vector2[vertexCount];
		int[] array3 = new int[indexCount];
		for (int i = 0; i < vertexCount; i++)
		{
			array[i] = vertex[i].position;
			array2[i] = vertex[i].texcoord;
		}
		for (int j = 0; j < indexCount; j++)
		{
			array3[j] = index[j];
		}
		mesh.vertices = array;
		mesh.uv = array2;
		mesh.triangles = array3;
	}

	public void Initialize(Terrain terrain, int sectorRow, int sectorCol)
	{
		indexCount = 0;
		vertexCount = 0;
		int num = sectorRow * 6;
		int num2 = sectorCol * 6;
		gameObject.transform.position = new Vector3(20 * num2, 20 * num, 0f);
		bool flag = terrain.IsTerrainSectorDisabled(sectorRow, sectorCol);
		for (int i = 0; i < 6; i++)
		{
			int num3 = num + i;
			for (int j = 0; j < 6; j++)
			{
				int terrainIdAt = terrain.GetTerrainIdAt(num + i, num2 + j);
				TerrainType terrainType = terrain.GetTerrainType(terrainIdAt);
				TFUtils.Assert(terrainType != null, "Missing terrain type" + terrainIdAt);
				int num4 = num2 + j;
				string material = terrainType.Material;
				byte b = 0;
				if (terrainType.IsPath())
				{
					byte b2 = 0;
					if (terrain.GetTerrainType(num3 + 1, num4) != null && terrain.GetTerrainType(num3 + 1, num4).IsPath())
					{
						b2 |= 8;
					}
					if (terrain.GetTerrainType(num3, num4 + 1) != null && terrain.GetTerrainType(num3, num4 + 1).IsPath())
					{
						b2 |= 4;
					}
					if (terrain.GetTerrainType(num3 - 1, num4) != null && terrain.GetTerrainType(num3 - 1, num4).IsPath())
					{
						b2 |= 2;
					}
					if (terrain.GetTerrainType(num3, num4 - 1) != null && terrain.GetTerrainType(num3, num4 - 1).IsPath())
					{
						b2 |= 1;
					}
					material = terrainType.GetPathMaterial(b2);
				}
				else if (terrainType.IsGrass())
				{
					byte b3 = 0;
					b = 0;
					switch (i)
					{
					case 0:
					{
						TerrainType terrainType3 = terrain.GetTerrainType(num3 - 1, num4);
						if (terrainType3 == null || !terrainType3.IsGrass())
						{
							b3 |= 1;
							if (terrain.IsTerrainSectorDisabled(sectorRow - 1, sectorCol))
							{
								b3 |= 4;
							}
						}
						break;
					}
					case 5:
					{
						TerrainType terrainType2 = terrain.GetTerrainType(num3 + 1, num4);
						if (terrainType2 == null || !terrainType2.IsGrass())
						{
							b3 |= 1;
							b |= 5;
							if (terrain.IsTerrainSectorDisabled(sectorRow + 1, sectorCol))
							{
								b3 |= 4;
							}
						}
						break;
					}
					}
					switch (j)
					{
					case 0:
					{
						TerrainType terrainType5 = terrain.GetTerrainType(num3, num4 - 1);
						if (terrainType5 == null || !terrainType5.IsGrass())
						{
							b3 |= 2;
							if (terrain.IsTerrainSectorDisabled(sectorRow, sectorCol - 1))
							{
								b3 |= 8;
							}
						}
						break;
					}
					case 5:
					{
						TerrainType terrainType4 = terrain.GetTerrainType(num3, num4 + 1);
						if (terrainType4 == null || !terrainType4.IsGrass())
						{
							b3 |= 2;
							b |= 6;
							if (terrain.IsTerrainSectorDisabled(sectorRow, sectorCol + 1))
							{
								b3 |= 8;
							}
						}
						break;
					}
					}
					material = terrainType.GetGrassMaterial((TerrainType.GrassBorderType)b3);
				}
				else if (terrainType.IsSand() && flag)
				{
					if (useRotatedTiles)
					{
						byte b4 = 0;
						if (i == 0 && !terrain.IsTerrainSectorDisabled(sectorRow - 1, sectorCol))
						{
							b4 = 1;
						}
						else if (i == 5 && !terrain.IsTerrainSectorDisabled(sectorRow + 1, sectorCol))
						{
							b4 = 4;
						}
						if (j == 0 && !terrain.IsTerrainSectorDisabled(sectorRow, sectorCol - 1))
						{
							b4 |= 8;
						}
						else if (j == 5 && !terrain.IsTerrainSectorDisabled(sectorRow, sectorCol + 1))
						{
							b4 |= 2;
						}
						byte tileTypeAndRotation = terrain.terrainTextures.GetTileTypeAndRotation(b4);
						byte b5 = (byte)(tileTypeAndRotation >> 3);
						material = ((b5 == 0 || terrain.GetTerrainType(num3, num4 + 1).Material.ToString().Equals("grass.png") || terrain.GetTerrainType(num3, num4 - 1).Material.ToString().Equals("grass.png") || terrain.GetTerrainType(num3 + 1, num4).Material.ToString().Equals("grass.png") || terrain.GetTerrainType(num3 - 1, num4).Material.ToString().Equals("grass.png")) ? "disabled_terrainXX" : ((b5 != 1) ? "disabled_terrainUL" : "disabled_terrainXL"));
						b = (byte)(tileTypeAndRotation & 7);
					}
					else
					{
						char c = 'X';
						char c2 = 'X';
						if (i == 0 && !terrain.IsTerrainSectorDisabled(sectorRow - 1, sectorCol))
						{
							c = 'D';
						}
						else if (i == 5 && !terrain.IsTerrainSectorDisabled(sectorRow + 1, sectorCol))
						{
							c = 'U';
						}
						if (j == 0 && !terrain.IsTerrainSectorDisabled(sectorRow, sectorCol - 1))
						{
							c2 = 'L';
						}
						else if (j == 5 && !terrain.IsTerrainSectorDisabled(sectorRow, sectorCol + 1))
						{
							c2 = 'R';
						}
						material = "disabled_terrain" + c + c2;
					}
				}
				else if (terrainType.GetDisabledMaterial() != null && flag)
				{
					material = terrainType.GetDisabledMaterial();
				}
				CreateQuad(20f, i, j, terrain.terrainTextures.GetMaterialUVs(material), b);
			}
		}
		UpdateMesh();
	}
}
