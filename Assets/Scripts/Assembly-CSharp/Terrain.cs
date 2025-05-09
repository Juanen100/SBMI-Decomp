#define ASSERTS_ON
using System;
using System.Collections.Generic;
using UnityEngine;

public class Terrain
{
	public const byte INVALID_TERRAIN_COST = byte.MaxValue;

	public const byte OBSTACLE_COST = 120;

	public const byte UNPURCHASED_COST = 20;

	public const byte TERRAIN_TYPE_INVALID = byte.MaxValue;

	public const int TERRAIN_TILE_WORLDSIZE = 20;

	public static readonly string TERRAIN_PATH = "Terrain";

	public static Vector3 UP = new Vector3(0f, 0f, 1f);

	public static float terrainTextureScaleU = 0f;

	public static float terrainTextureScaleV = 0f;

	public static float terrainTextureInvScaleU = 0f;

	public static float terrainTextureInvScaleV = 0f;

	public List<Cost> expansionCosts;

	public TerrainTextureLibrary terrainTextures;

	public HashSet<int> purchasedSlots;

	public TerrainSlot selectedSlot;

	private Dictionary<int, TerrainType> terrainTypes = new Dictionary<int, TerrainType>();

	private Dictionary<int, TerrainSlot> slots = new Dictionary<int, TerrainSlot>();

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

	private static Material mTerrainMaterial = null;

	public static Material TerrainMaterial
	{
		get
		{
			if (mTerrainMaterial == null)
			{
				if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Standard)
				{
					mTerrainMaterial = Resources.Load("Materials/lod/terrainsheet_lr") as Material;
				}
				else if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Low)
				{
					mTerrainMaterial = Resources.Load("Materials/lod/terrainsheet_lr2") as Material;
				}
				else
				{
					mTerrainMaterial = Resources.Load("Materials/lod/terrainsheet") as Material;
				}
			}
			return mTerrainMaterial;
		}
	}

	public TerrainType BackgroundTerrainType
	{
		get
		{
			return terrainTypes[backgroundTerrain];
		}
	}

	public int GridWidth
	{
		get
		{
			return sectorWidth * 6;
		}
	}

	public int GridHeight
	{
		get
		{
			return sectorHeight * 6;
		}
	}

	public int WorldWidth
	{
		get
		{
			return GridWidth * 20;
		}
	}

	public int WorldHeight
	{
		get
		{
			return GridHeight * 20;
		}
	}

	public AlignedBox WorldExtent
	{
		get
		{
			return worldExtent;
		}
	}

	public AlignedBox PurchasedExtent
	{
		get
		{
			return purchasedExtent;
		}
	}

	public AlignedBox CameraExtents
	{
		get
		{
			return cameraExtents;
		}
	}

	public AlignedBox FootprintGuide
	{
		get
		{
			return footprintGuide;
		}
		set
		{
			footprintGuide = value;
		}
	}

	public Dictionary<int, TerrainSlot> ExpansionSlots
	{
		get
		{
			return slots;
		}
	}

	public Terrain(int terrainSeed)
	{
		this.terrainSeed = terrainSeed;
		terrainTextures = new TerrainTextureLibrary();
		LoadTerrain();
	}

	private void LoadTerrain()
	{
		LoadTerrainFromSpread();
		LoadTerrainSlotsFromSpread();
		LoadTerrainTypesFromSpread();
	}

	private void LoadTerrainFromSpread()
	{
		string text = "Terrain";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null)
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("type", "terrain");
		int columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		int columnIndexInSheet2 = instance.GetColumnIndexInSheet(sheetIndex, "expansion gold costs");
		int columnIndexInSheet3 = instance.GetColumnIndexInSheet(sheetIndex, "max width");
		int columnIndexInSheet4 = instance.GetColumnIndexInSheet(sheetIndex, "max height");
		int columnIndexInSheet5 = instance.GetColumnIndexInSheet(sheetIndex, "background terrain did");
		int columnIndexInSheet6 = instance.GetColumnIndexInSheet(sheetIndex, "camera frame x min");
		int columnIndexInSheet7 = instance.GetColumnIndexInSheet(sheetIndex, "camera frame x max");
		int columnIndexInSheet8 = instance.GetColumnIndexInSheet(sheetIndex, "camera frame y min");
		int columnIndexInSheet9 = instance.GetColumnIndexInSheet(sheetIndex, "camera frame y max");
		int columnIndexInSheet10 = instance.GetColumnIndexInSheet(sheetIndex, "camera color r");
		int columnIndexInSheet11 = instance.GetColumnIndexInSheet(sheetIndex, "camera color g");
		int columnIndexInSheet12 = instance.GetColumnIndexInSheet(sheetIndex, "camera color b");
		int columnIndexInSheet13 = instance.GetColumnIndexInSheet(sheetIndex, "camera alpha");
		int num2 = -1;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
			if (num2 < 0)
			{
				num2 = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet2);
			}
			dictionary.Add("max_height", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet4));
			dictionary.Add("max_width", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet3));
			dictionary.Add("background_terrain", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet5));
			dictionary.Add("camera_frame", new Dictionary<string, object>
			{
				{
					"xMin",
					instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet6)
				},
				{
					"xMax",
					instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet7)
				},
				{
					"yMin",
					instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet8)
				},
				{
					"yMax",
					instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet9)
				}
			});
			dictionary.Add("camera_color", new Dictionary<string, object>
			{
				{
					"r",
					instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet10)
				},
				{
					"g",
					instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet11)
				},
				{
					"b",
					instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet12)
				},
				{
					"a",
					instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet13)
				}
			});
			dictionary.Add("expansion_costs", new List<object>());
			for (int j = 1; j <= num2; j++)
			{
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, "expansion gold cost " + j);
				if (intCell > 0)
				{
					((List<object>)dictionary["expansion_costs"]).Add(new Dictionary<string, object> { { "3", intCell } });
				}
			}
			dictionary.Add("terrain_dist", new Dictionary<string, object>());
			dictionary.Add("background_tiles", new List<object>());
			dictionary.Add("foreground_tiles", new List<object>());
			dictionary.Add("decal_tiles", new List<object>());
		}
		text = "TerrainForeground";
		sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		int columnIndexInSheet14 = instance.GetColumnIndexInSheet(sheetIndex, "terrain type did");
		int columnIndexInSheet15 = instance.GetColumnIndexInSheet(sheetIndex, "position x");
		int columnIndexInSheet16 = instance.GetColumnIndexInSheet(sheetIndex, "position y");
		for (int k = 0; k < num; k++)
		{
			string rowName = k.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
			((List<object>)dictionary["foreground_tiles"]).Add(new Dictionary<string, object>(3)
			{
				{
					"did",
					instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet14)
				},
				{
					"x",
					instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet15)
				},
				{
					"y",
					instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet16)
				}
			});
		}
		LoadTerrain(dictionary);
	}

	private void LoadTerrainSlotsFromSpread()
	{
		string text = "TerrainSlots";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null)
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		Dictionary<int, Dictionary<string, object>> dictionary = new Dictionary<int, Dictionary<string, object>>();
		Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
		int columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		int columnIndexInSheet2 = instance.GetColumnIndexInSheet(sheetIndex, "did");
		int columnIndexInSheet3 = instance.GetColumnIndexInSheet(sheetIndex, "sector row");
		int columnIndexInSheet4 = instance.GetColumnIndexInSheet(sheetIndex, "sector column");
		int columnIndexInSheet5 = instance.GetColumnIndexInSheet(sheetIndex, "cost multiplier");
		int columnIndexInSheet6 = instance.GetColumnIndexInSheet(sheetIndex, "row");
		int columnIndexInSheet7 = instance.GetColumnIndexInSheet(sheetIndex, "column");
		int columnIndexInSheet8 = instance.GetColumnIndexInSheet(sheetIndex, "is boardwalk");
		int columnIndexInSheet9 = instance.GetColumnIndexInSheet(sheetIndex, "cost jelly");
		int columnIndexInSheet10 = instance.GetColumnIndexInSheet(sheetIndex, "cost gold");
		int columnIndexInSheet11 = instance.GetColumnIndexInSheet(sheetIndex, "required slots");
		string text2 = "n/a";
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
			int intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet2);
			int intCell2;
			if (dictionary.ContainsKey(intCell))
			{
				dictionary2 = dictionary[intCell];
				intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet3);
				intCell2 = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet4);
				if (intCell >= 0 && intCell2 >= 0)
				{
					((List<object>)dictionary2["sectors"]).Add(new Dictionary<string, object>
					{
						{ "row", intCell },
						{ "col", intCell2 }
					});
				}
				continue;
			}
			dictionary2 = new Dictionary<string, object>();
			dictionary.Add(intCell, dictionary2);
			dictionary2.Add("type", "slot");
			dictionary2.Add("did", intCell);
			dictionary2.Add("tier", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet5));
			dictionary2.Add("row", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet6));
			dictionary2.Add("col", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet7));
			dictionary2.Add("is_boardwalk", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet8) == 1);
			dictionary2.Add("cost", new Dictionary<string, object>());
			intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet9);
			if (intCell > 0)
			{
				((Dictionary<string, object>)dictionary2["cost"]).Add("2", intCell);
			}
			intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet10);
			if (intCell > 0)
			{
				((Dictionary<string, object>)dictionary2["cost"]).Add("3", intCell);
			}
			dictionary2.Add("sectors", new List<object>());
			intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet3);
			intCell2 = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet4);
			if (intCell >= 0 && intCell2 >= 0)
			{
				((List<object>)dictionary2["sectors"]).Add(new Dictionary<string, object>
				{
					{ "row", intCell },
					{ "col", intCell2 }
				});
			}
			dictionary2.Add("outline", new List<object>());
			string stringCell;
			for (int j = 1; j < 5; j++)
			{
				stringCell = instance.GetStringCell(sheetIndex, rowIndex, "outline corner " + j);
				if (string.IsNullOrEmpty(stringCell) || stringCell == text2)
				{
					continue;
				}
				string[] array = stringCell.Split('|');
				intCell = array.Length;
				if (intCell == 2)
				{
					bool flag = int.TryParse(array[0], out intCell);
					if (flag)
					{
						flag = int.TryParse(array[1], out intCell2);
					}
					if (flag)
					{
						((List<object>)dictionary2["outline"]).Add(new Dictionary<string, object>
						{
							{ "row", intCell },
							{ "col", intCell2 }
						});
					}
				}
			}
			dictionary2.Add("required_slots", new List<object>());
			stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet11);
			if (!string.IsNullOrEmpty(stringCell) && stringCell != text2)
			{
				string[] array = stringCell.Split('|');
				intCell = array.Length;
				for (int k = 0; k < intCell; k++)
				{
					if (int.TryParse(array[k], out intCell2))
					{
						((List<object>)dictionary2["required_slots"]).Add(intCell2);
					}
				}
			}
			dictionary2.Add("debris", new List<object>());
			dictionary2.Add("landmarks", new List<object>());
		}
		text = "DebrisPlacement";
		sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		int columnIndexInSheet12 = instance.GetColumnIndexInSheet(sheetIndex, "slot did");
		int columnIndexInSheet13 = instance.GetColumnIndexInSheet(sheetIndex, "debris did");
		int columnIndexInSheet14 = instance.GetColumnIndexInSheet(sheetIndex, "label");
		int columnIndexInSheet15 = instance.GetColumnIndexInSheet(sheetIndex, "position x");
		int columnIndexInSheet16 = instance.GetColumnIndexInSheet(sheetIndex, "position y");
		for (int l = 0; l < num; l++)
		{
			string rowName = l.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
			int intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet12);
			if (dictionary.ContainsKey(intCell))
			{
				dictionary2 = dictionary[intCell];
				if (!dictionary2.ContainsKey("debris"))
				{
					dictionary2.Add("debris", new List<object>());
				}
				((List<object>)dictionary2["debris"]).Add(new Dictionary<string, object>(4)
				{
					{
						"did",
						instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet13)
					},
					{
						"label",
						instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet14)
					},
					{
						"x",
						instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet15)
					},
					{
						"y",
						instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet16)
					}
				});
			}
		}
		text = "LandmarkPlacement";
		sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		columnIndexInSheet12 = instance.GetColumnIndexInSheet(sheetIndex, "slot did");
		int columnIndexInSheet17 = instance.GetColumnIndexInSheet(sheetIndex, "landmark did");
		columnIndexInSheet14 = instance.GetColumnIndexInSheet(sheetIndex, "label");
		columnIndexInSheet15 = instance.GetColumnIndexInSheet(sheetIndex, "position x");
		columnIndexInSheet16 = instance.GetColumnIndexInSheet(sheetIndex, "position y");
		for (int m = 0; m < num; m++)
		{
			string rowName = m.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
			int intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet12);
			if (dictionary.ContainsKey(intCell))
			{
				dictionary2 = dictionary[intCell];
				if (!dictionary2.ContainsKey("landmarks"))
				{
					dictionary2.Add("landmarks", new List<object>());
				}
				((List<object>)dictionary2["landmarks"]).Add(new Dictionary<string, object>(4)
				{
					{
						"did",
						instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet17)
					},
					{
						"label",
						instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet14)
					},
					{
						"x",
						instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet15)
					},
					{
						"y",
						instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet16)
					}
				});
			}
		}
		foreach (KeyValuePair<int, Dictionary<string, object>> item in dictionary)
		{
			TerrainSlot terrainSlot = new TerrainSlot(item.Value);
			slots.Add(terrainSlot.Id, terrainSlot);
		}
	}

	private void LoadTerrainTypesFromSpread()
	{
		string text = "TerrainTypes";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null)
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		string text2 = "n/a";
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
				continue;
			}
			int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, "id").ToString());
			dictionary.Clear();
			dictionary.Add("type", "type");
			dictionary.Add("id", instance.GetIntCell(text, rowName, "type did"));
			dictionary.Add("move_cost", instance.GetIntCell(text, rowName, "move cost"));
			dictionary.Add("name", instance.GetStringCell(text, rowName, "name"));
			dictionary.Add("can_pave", instance.GetIntCell(text, rowName, "can pave") == 1);
			int intCell = instance.GetIntCell(text, rowName, "derives from type did");
			if (intCell >= 0)
			{
				dictionary.Add("main_type", intCell);
			}
			string stringCell = instance.GetStringCell(text, rowName, "material");
			if (!string.IsNullOrEmpty(stringCell) && stringCell != text2)
			{
				dictionary.Add("material", stringCell);
			}
			stringCell = instance.GetStringCell(text, rowName, "disabled material");
			if (!string.IsNullOrEmpty(stringCell) && stringCell != text2)
			{
				dictionary.Add("disabled_material", stringCell);
			}
			TerrainType terrainType = new TerrainType(dictionary);
			terrainTypes.Add(terrainType.Id, terrainType);
		}
	}

	private void LoadTerrain(Dictionary<string, object> data)
	{
		sectorWidth = TFUtils.LoadInt(data, "max_width");
		sectorHeight = TFUtils.LoadInt(data, "max_height");
		backgroundTerrain = (byte)TFUtils.LoadInt(data, "background_terrain");
		sectorInset = new Rect(1f, 1f, sectorWidth - 1, sectorHeight - 1);
		if (data.ContainsKey("camera_frame"))
		{
			Dictionary<string, object> d = data["camera_frame"] as Dictionary<string, object>;
			sectorInset.xMin = TFUtils.LoadInt(d, "xMin");
			sectorInset.xMax = TFUtils.LoadInt(d, "xMax");
			sectorInset.yMin = TFUtils.LoadInt(d, "yMin");
			sectorInset.yMax = TFUtils.LoadInt(d, "yMax");
		}
		if (data.ContainsKey("camera_color"))
		{
			Color backgroundColor = default(Color);
			Dictionary<string, object> d2 = data["camera_color"] as Dictionary<string, object>;
			int num = TFUtils.LoadInt(d2, "r");
			if (num != 0)
			{
				backgroundColor.r = (float)num / 256f;
			}
			else
			{
				backgroundColor.r = 0f;
			}
			num = TFUtils.LoadInt(d2, "g");
			if (num != 0)
			{
				backgroundColor.g = (float)num / 256f;
			}
			else
			{
				backgroundColor.g = 0f;
			}
			num = TFUtils.LoadInt(d2, "b");
			if (num != 0)
			{
				backgroundColor.b = (float)num / 256f;
			}
			else
			{
				backgroundColor.b = 0f;
			}
			num = TFUtils.LoadInt(d2, "a");
			if (num != 0)
			{
				backgroundColor.a = (float)num / 256f;
			}
			else
			{
				backgroundColor.a = 0f;
			}
			Camera.main.backgroundColor = backgroundColor;
		}
		distribution = new List<KeyValuePair<int, float>>();
		Dictionary<string, object> dictionary = data["terrain_dist"] as Dictionary<string, object>;
		foreach (string key2 in dictionary.Keys)
		{
			int key = Convert.ToInt32(key2);
			float value = TFUtils.LoadFloat(dictionary, key2);
			distribution.Add(new KeyValuePair<int, float>(key, value));
		}
		expansionCosts = new List<Cost>();
		List<object> list = (List<object>)data["expansion_costs"];
		foreach (Dictionary<string, object> item in list)
		{
			expansionCosts.Add(Cost.FromDict(item));
		}
		foregroundOverrides = LoadTerrainNodeData((List<object>)data["foreground_tiles"]);
		worldExtent = new AlignedBox(0f, WorldWidth, 0f, WorldHeight);
	}

	public void Initialize()
	{
		tiles = new byte[GridHeight, GridWidth];
		nonPathTiles = new byte[GridHeight, GridWidth];
		obstacles = new bool[GridHeight, GridWidth];
		purchasedSectors = new bool[sectorHeight, sectorWidth];
		bool flag = distribution.Count == 0;
		for (int i = 0; i < GridHeight; i++)
		{
			for (int j = 0; j < GridWidth; j++)
			{
				if (flag)
				{
					tiles[i, j] = backgroundTerrain;
					continue;
				}
				byte b = GenerateTerrainTile(i, j);
				if (b == byte.MaxValue)
				{
					tiles[i, j] = backgroundTerrain;
				}
			}
		}
		ProcessOverrides();
		Decal();
		for (int k = 0; k < GridHeight; k++)
		{
			for (int l = 0; l < GridWidth; l++)
			{
				nonPathTiles[k, l] = tiles[k, l];
			}
		}
		sectors = new TerrainSector[sectorHeight, sectorWidth];
		for (int m = 0; m < sectorHeight; m++)
		{
			for (int n = 0; n < sectorWidth; n++)
			{
				sectors[m, n] = new TerrainSector(0, m, n);
			}
		}
	}

	public void CreateTerrainMeshes()
	{
		for (int i = 0; i < sectorHeight; i++)
		{
			for (int j = 0; j < sectorWidth; j++)
			{
				sectors[i, j].Initialize(this, i, j);
			}
		}
		meshesCreated = true;
	}

	private string[] GetFilesToLoad()
	{
		return Config.TERRAIN_PATH;
	}

	private string GetFilePathFromString(string filePath)
	{
		return filePath;
	}

	public void Destroy()
	{
		TerrainSector[,] array = sectors;
		int length = array.GetLength(0);
		int length2 = array.GetLength(1);
		for (int i = 0; i < length; i++)
		{
			for (int j = 0; j < length2; j++)
			{
				TerrainSector terrainSector = array[i, j];
				if (terrainSector != null)
				{
					terrainSector.Destroy();
				}
			}
		}
	}

	public bool ChangePath(GridPosition gpos)
	{
		TerrainType terrainType = GetTerrainType(gpos);
		if (terrainType == null)
		{
			return false;
		}
		if (terrainType.IsPath())
		{
			tiles[gpos.row, gpos.col] = nonPathTiles[gpos.row, gpos.col];
		}
		else
		{
			byte b = tiles[gpos.row, gpos.col];
			if (b > 60 || b == 6)
			{
				return false;
			}
			tiles[gpos.row, gpos.col] = TerrainType.GetPathTypeId();
			byte b2 = GenerateDecal(gpos.row, gpos.col);
			if (b2 != byte.MaxValue)
			{
				tiles[gpos.row, gpos.col] = b2;
			}
		}
		UpdateSectors(gpos.row, gpos.col);
		if (PathFinder2.IsInitialized())
		{
			PathFinder2.UpdateCost(gpos.row, gpos.col, GetTerrainCost(gpos.row, gpos.col));
		}
		return true;
	}

	public GridPosition ComputeGridPosition(Vector2 worldPosition)
	{
		return new GridPosition((int)(worldPosition.y / 20f), (int)(worldPosition.x / 20f));
	}

	public Vector2 ComputeWorldPosition(GridPosition gridPosition)
	{
		return new Vector2(((float)gridPosition.col + 0.5f) * 20f, ((float)gridPosition.row + 0.5f) * 20f);
	}

	public Vector3 ConstrainToAlignedBox(Vector3 position, AlignedBox footprint)
	{
		if (position.x < worldExtent.xmin)
		{
			position.x = worldExtent.xmin;
		}
		else if (position.x > worldExtent.xmax - footprint.xmax)
		{
			position.x = worldExtent.xmax - footprint.xmax;
		}
		if (position.y < worldExtent.ymin)
		{
			position.y = worldExtent.ymin;
		}
		else if (position.y > worldExtent.ymax - footprint.ymax)
		{
			position.y = worldExtent.ymax - footprint.ymax;
		}
		return position;
	}

	public Vector3 CalculateNearestGridPosition(Vector3 position, AlignedBox footprint)
	{
		Vector3 position2 = new Vector3(0f, 0f, 0f);
		position2.x = (float)Math.Round(position.x / 20f) * 20f;
		position2.y = (float)Math.Round(position.y / 20f) * 20f;
		return ConstrainToAlignedBox(position2, footprint);
	}

	public bool ComputeIntersection(Ray ray, out Vector3 point)
	{
		point = ray.origin;
		float num = Vector3.Dot(ray.direction, UP);
		if (num * num < 1.0000001E-06f)
		{
			return false;
		}
		float num2 = -1f / num;
		float num3 = num2 * Vector3.Dot(ray.origin, UP);
		if (num3 < 0f)
		{
			return false;
		}
		point = ray.origin + num3 * ray.direction;
		return true;
	}

	public byte GetTerrainCost(int row, int col)
	{
		if (HasObstacle(row, col))
		{
			return 120;
		}
		TerrainType terrainType = GetTerrainType(row, col);
		if (terrainType == null)
		{
			return byte.MaxValue;
		}
		byte b = 0;
		if (!purchasedSectors[row / 6, col / 6])
		{
			b = 20;
		}
		return (byte)(terrainType.Cost + b);
	}

	public float GetTerrainCost(GridPosition gridPosition)
	{
		return (int)GetTerrainCost(gridPosition.row, gridPosition.col);
	}

	public float GetTerrainCost(Vector2 worldPosition)
	{
		return GetTerrainCost(ComputeGridPosition(worldPosition));
	}

	public void SetOrClearObstacle(AlignedBox box, bool isSet)
	{
		float num = 20 * (int)(box.ymin / 20f);
		float num2 = box.ymin;
		bool flag = false;
		while (!flag)
		{
			float num3 = 0f;
			float num4 = num + 20f;
			if (box.ymax > num4)
			{
				num3 = num4 - num2;
				num = num4;
			}
			else
			{
				num3 = box.ymax - num2;
				flag = true;
			}
			float num5 = 20 * (int)(box.xmin / 20f);
			float num6 = box.xmin;
			while (true)
			{
				float num7 = num5 + 20f;
				if (!(box.xmax > num7))
				{
					break;
				}
				float num8 = num7 - num6;
				if (num8 * num3 >= 200f)
				{
					SetObstacleAtCoords(num6, num2, isSet);
				}
				num6 = num7;
				num5 = num7;
			}
			float num9 = box.xmax - num6;
			if (num9 * num3 >= 200f)
			{
				SetObstacleAtCoords(num6, num2, isSet);
			}
			num2 = num4;
		}
	}

	private void SetObstacleAtCoords(float x, float y, bool isSet)
	{
		GridPosition gridPosition = ComputeGridPosition(new Vector2(x, y));
		gridPosition.MakeValid(GridHeight - 1, GridWidth - 1);
		obstacles[gridPosition.row, gridPosition.col] = isSet;
		if (PathFinder2.IsInitialized())
		{
			PathFinder2.UpdateCost(gridPosition.row, gridPosition.col, GetTerrainCost(gridPosition.row, gridPosition.col));
		}
	}

	public bool CheckIsPurchasedArea(AlignedBox box)
	{
		int num = 6;
		GridPosition gridPosition = ComputeGridPosition(new Vector2(box.xmin, box.ymin));
		GridPosition gridPosition2 = ComputeGridPosition(new Vector2(box.xmax, box.ymax));
		gridPosition = new GridPosition(gridPosition.row / num, gridPosition.col / num);
		gridPosition2 = new GridPosition((gridPosition2.row - 1) / num, (gridPosition2.col - 1) / num);
		if (!ValidSectorIndex(gridPosition) || !ValidSectorIndex(gridPosition2))
		{
			return false;
		}
		for (int i = gridPosition.row; i <= gridPosition2.row; i++)
		{
			for (int j = gridPosition.col; j <= gridPosition2.col; j++)
			{
				if (!purchasedSectors[i, j])
				{
					return false;
				}
			}
		}
		return true;
	}

	public bool CheckIsPurchasedArea(Vector2 point)
	{
		int num = 120;
		int num2 = (int)point.y / num;
		int num3 = (int)point.x / num;
		if (!ValidSectorIndex(num2, num3))
		{
			return false;
		}
		return purchasedSectors[num2, num3];
	}

	public bool CheckIsPurchasedArea(int row, int col)
	{
		return purchasedSectors[row / 6, col / 6];
	}

	public void MarkPurchase(TerrainSlot slot)
	{
		foreach (GridPosition sector in slot.sectors)
		{
			purchasedSectors[sector.row, sector.col] = true;
			if (!ValidSectorIndex(sector))
			{
				continue;
			}
			sectors[sector.row, sector.col].Highlighted = false;
			UpdateAllSurroundingSectors(sector.row, sector.col);
			if (purchasedExtent == null)
			{
				purchasedExtent = GetSectorBounds(sector.row, sector.col);
				cameraExtents = GetCameraBounds(sector.row, sector.col);
			}
			else
			{
				purchasedExtent = AlignedBox.Union(purchasedExtent, GetSectorBounds(sector.row, sector.col));
				cameraExtents = AlignedBox.Union(cameraExtents, GetCameraBounds(sector.row, sector.col));
			}
			if (!PathFinder2.IsInitialized())
			{
				continue;
			}
			int num = sector.row * 6;
			int num2 = num + 6;
			int num3 = sector.col * 6;
			int num4 = num3 + 6;
			for (int i = num; i < num2; i++)
			{
				for (int j = num3; j < num4; j++)
				{
					PathFinder2.UpdateCost(i, j, GetTerrainCost(i, j));
				}
			}
		}
		selectedSlot = null;
		slot.ClearOutline();
		slot.ClearSign();
	}

	private AlignedBox GetSectorBounds(int row, int col)
	{
		return new AlignedBox(col * 6 * 20, (col + 1) * 6 * 20, row * 6 * 20, (row + 1) * 6 * 20);
	}

	private AlignedBox GetCameraBounds(int row, int col)
	{
		int num = (int)sectorInset.xMax;
		int num2 = (int)sectorInset.yMax;
		int num3 = (int)sectorInset.xMin;
		int num4 = (int)sectorInset.yMin;
		if (row > num2)
		{
			row = num2;
		}
		else if (row < num4)
		{
			row = num4;
		}
		if (col > num)
		{
			col = num;
		}
		else if (col < num3)
		{
			col = num3;
		}
		return new AlignedBox(col * 6 * 20, (col + 1) * 6 * 20, row * 6 * 20, (row + 1) * 6 * 20);
	}

	public AlignedBox GetGridBounds(int row, int col)
	{
		return new AlignedBox(col * 20, (col + 1) * 20, row * 20, (row + 1) * 20);
	}

	public void AddExpansionSlot(int id)
	{
		if (slots.ContainsKey(id))
		{
			purchasedSlots.Add(id);
			MarkPurchase(slots[id]);
		}
	}

	public void AddRandomAvailableSlot(Game game)
	{
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, TerrainSlot> slot in slots)
		{
			if (purchasedSlots.Contains(slot.Key) || !slot.Value.Available(purchasedSlots, game))
			{
				continue;
			}
			bool flag = false;
			foreach (GridPosition sector in slot.Value.sectors)
			{
				flag = IsTerrainSectorDisabled(sector.row, sector.col);
				if (!flag)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				list.Add(slot.Key);
			}
		}
		if (list.Count < 0)
		{
			TFUtils.DebugLog("Warning: Add Random Available Slot has no available slot to add");
			return;
		}
		int num = list[UnityEngine.Random.Range(0, list.Count)];
		purchasedSlots.Add(num);
		TerrainSlot terrainSlot = slots[num];
		MarkPurchase(terrainSlot);
		if (game.featureManager.CheckFeature("purchase_expansions"))
		{
			game.terrain.UpdateRealtySigns(game.entities.DisplayControllerManager, SBCamera.BillboardDefinition, game);
		}
		if (game.terrain.IsBorderSlot(terrainSlot.Id))
		{
			game.border.UpdateTerrainBorderStrip(game.terrain);
		}
		foreach (TerrainSlotObject landmark in terrainSlot.landmarks)
		{
			game.simulation.Router.Send(PurchaseCommand.Create(Identity.Null(), landmark.id));
		}
		foreach (TerrainSlotObject debri in terrainSlot.debris)
		{
			game.simulation.Router.Send(PurchaseCommand.Create(Identity.Null(), debri.id));
		}
		game.ModifyGameState(new NewExpansionAction(terrainSlot.Id, new Cost(), terrainSlot.debris, terrainSlot.landmarks));
		AnalyticsWrapper.LogExpansion(game, terrainSlot.Id, null);
		TFUtils.DebugLog("Rewared Random Expansion Slot: " + terrainSlot.Id, TFUtils.LogFilter.Terrain);
	}

	public void AddAndClearExpansionSlot(Game pGame, int nDID)
	{
		if (!slots.ContainsKey(nDID))
		{
			TFUtils.ErrorLog("AddAndClearExpansionSlot | no slot with id: " + nDID);
			return;
		}
		TerrainSlot terrainSlot = slots[nDID];
		if (!purchasedSlots.Contains(nDID))
		{
			purchasedSlots.Add(nDID);
			MarkPurchase(terrainSlot);
			if (pGame.featureManager.CheckFeature("purchase_expansions"))
			{
				pGame.terrain.UpdateRealtySigns(pGame.entities.DisplayControllerManager, SBCamera.BillboardDefinition, pGame);
			}
			if (pGame.terrain.IsBorderSlot(terrainSlot.Id))
			{
				pGame.border.UpdateTerrainBorderStrip(pGame.terrain);
			}
			pGame.ModifyGameState(new NewExpansionAction(terrainSlot.Id, new Cost(), new List<TerrainSlotObject>(), new List<TerrainSlotObject>()));
			AnalyticsWrapper.LogExpansion(pGame, terrainSlot.Id, null);
		}
		foreach (TerrainSlotObject landmark in terrainSlot.landmarks)
		{
			pGame.entities.Destroy(landmark.id);
			Simulated simulated = pGame.simulation.FindSimulated(landmark.id);
			if (simulated != null)
			{
				simulated.SetFootprint(pGame.simulation, false);
				pGame.simulation.RemoveSimulated(simulated);
			}
		}
		foreach (TerrainSlotObject debri in terrainSlot.debris)
		{
			pGame.entities.Destroy(debri.id);
			Simulated simulated = pGame.simulation.FindSimulated(debri.id);
			if (simulated != null)
			{
				simulated.SetFootprint(pGame.simulation, false);
				pGame.simulation.RemoveSimulated(simulated);
			}
		}
	}

	public bool IsBorderSlot(int id)
	{
		TerrainSlot value;
		if (slots.TryGetValue(id, out value))
		{
			foreach (GridPosition sector in value.sectors)
			{
				if (sector.col == sectorWidth - 1)
				{
					return true;
				}
			}
		}
		return false;
	}

	public void HighlightSelection(TerrainSlot slot)
	{
		TFUtils.Assert(!purchasedSlots.Contains(slot.Id), "Should not be selecting a purchased Slot");
		slot.DrawOutline();
		foreach (GridPosition sector in slot.sectors)
		{
			if (ValidSectorIndex(sector))
			{
				sectors[sector.row, sector.col].Highlighted = true;
				UpdateAllSurroundingSectors(sector.row, sector.col);
			}
		}
		selectedSlot = slot;
	}

	public void DropSelection(TerrainSlot slot)
	{
		TFUtils.Assert(!purchasedSlots.Contains(slot.Id), "Should not be deselecting a purchased Slot");
		foreach (GridPosition sector in slot.sectors)
		{
			if (ValidSectorIndex(sector))
			{
				sectors[sector.row, sector.col].Highlighted = false;
				UpdateAllSurroundingSectors(sector.row, sector.col);
			}
		}
		slot.ClearOutline();
		selectedSlot = null;
	}

	public void OutlineAvailableExpansionSlots(Game game)
	{
		foreach (TerrainSlot value in slots.Values)
		{
			if (value.Available(purchasedSlots, game))
			{
				value.DrawOutline();
			}
		}
	}

	public void HideAvailableExpansionSlots()
	{
		foreach (TerrainSlot value in slots.Values)
		{
			value.ClearOutline();
		}
	}

	public void OutlineAllExpansionSlots()
	{
		foreach (TerrainSlot value in slots.Values)
		{
			value.DrawOutline();
		}
	}

	public void HideAllExpansionSlots()
	{
		foreach (TerrainSlot value in slots.Values)
		{
			value.ClearOutline();
		}
	}

	public void UpdateRealtySigns(DisplayControllerManager dcm, BillboardDelegate billboard, Game game)
	{
		Camera main = Camera.main;
		foreach (TerrainSlot value in slots.Values)
		{
			if (value.Available(purchasedSlots, game))
			{
				value.Display(dcm, billboard);
				value.OnUpdate(main);
			}
		}
	}

	public List<TerrainSlot> UnpurchasedExpansionSlots()
	{
		List<TerrainSlot> list = new List<TerrainSlot>();
		foreach (TerrainSlot value in slots.Values)
		{
			if (!purchasedSlots.Contains(value.Id))
			{
				list.Add(value);
			}
		}
		return list;
	}

	public TerrainType GetTerrainType(int row, int col)
	{
		if (!ValidTileIndex(row, col))
		{
			return null;
		}
		byte key = tiles[row, col];
		return terrainTypes[key];
	}

	public bool ProcessTap(Ray ray, Game game)
	{
		TerrainSlot terrainSlot = CheckTap(ray, game);
		if (terrainSlot != null)
		{
			if (selectedSlot != null)
			{
				DropSelection(selectedSlot);
			}
			selectedSlot = terrainSlot;
			selectedSlot.HandleSelection();
			return true;
		}
		return false;
	}

	public TerrainSlot CheckTap(Ray ray, Game game)
	{
		if (!game.featureManager.CheckFeature("purchase_expansions"))
		{
			return null;
		}
		foreach (TerrainSlot value in slots.Values)
		{
			if (value.CheckTap(ray) && game.simulation.CheckExpansionAllowed(value.Id))
			{
				return value;
			}
		}
		return null;
	}

	public Cost GetExpansionCost(TerrainSlot slot)
	{
		if (slot.cost != null && slot.cost.ResourceAmounts.Count != 0)
		{
			return slot.cost;
		}
		Cost cost = ((purchasedSlots.Count < expansionCosts.Count) ? expansionCosts[purchasedSlots.Count] : expansionCosts[expansionCosts.Count - 1]);
		Cost result = new Cost();
		for (int i = 0; i < slot.Tier; i++)
		{
			result += cost;
		}
		return result;
	}

	public bool IsTerrainSectorDisabled(int sectorRow, int sectorCol)
	{
		bool result = true;
		if (ValidSector(sectorRow, sectorCol))
		{
			result = !purchasedSectors[sectorRow, sectorCol];
		}
		return result;
	}

	public bool IsTerrainSectorBoardwalk(int sectorRow, int sectorCol)
	{
		bool result = true;
		if (ValidSector(sectorRow, sectorCol))
		{
		}
		return result;
	}

	public TerrainType GetTerrainType(GridPosition gridPosition)
	{
		return GetTerrainType(gridPosition.row, gridPosition.col);
	}

	public TerrainType GetTerrainType(Vector2 worldPosition)
	{
		return GetTerrainType(ComputeGridPosition(worldPosition));
	}

	public TerrainType GetTerrainType(int type)
	{
		if (terrainTypes.ContainsKey(type))
		{
			return terrainTypes[type];
		}
		return null;
	}

	public int GetTerrainIdAt(int row, int col)
	{
		if (!ValidTileIndex(row, col))
		{
			return 255;
		}
		return tiles[row, col];
	}

	private void Decal()
	{
		for (int i = 0; i < GridHeight; i++)
		{
			for (int j = 0; j < GridWidth; j++)
			{
				byte b = GenerateDecal(i, j);
				if (b != byte.MaxValue)
				{
					tiles[i, j] = b;
				}
			}
		}
	}

	private byte GenerateDecal(int row, int col)
	{
		TerrainType terrainType = GetTerrainType(row, col);
		if (terrainType == null)
		{
			return byte.MaxValue;
		}
		int seed = GetSeed(row, col);
		return terrainType.GenerateDecal(seed);
	}

	private bool ValidTileIndex(int row, int col)
	{
		if (row < 0 || GridHeight <= row)
		{
			return false;
		}
		if (col < 0 || GridWidth <= col)
		{
			return false;
		}
		return true;
	}

	private bool ValidSectorIndex(GridPosition pos)
	{
		return ValidSectorIndex(pos.row, pos.col);
	}

	private bool ValidSectorIndex(int row, int col)
	{
		if (row < 0 || sectorHeight <= row)
		{
			return false;
		}
		if (col < 0 || sectorWidth <= col)
		{
			return false;
		}
		return true;
	}

	private bool ValidSector(int sectorRow, int sectorCol)
	{
		return sectorRow >= 0 && sectorRow < sectorHeight && sectorCol >= 0 && sectorCol < sectorWidth;
	}

	private bool ValidTileIndex(GridPosition pos)
	{
		return ValidTileIndex(pos.row, pos.col);
	}

	private bool HasObstacle(int row, int col)
	{
		if (!ValidTileIndex(row, col))
		{
			return false;
		}
		return obstacles[row, col];
	}

	private void UpdateSectors(int gridRow, int gridCol)
	{
		if (meshesCreated)
		{
			int num = gridRow / 6;
			int num2 = gridCol / 6;
			sectors[num, num2].Initialize(this, num, num2);
			bool flag = gridRow % 6 == 0 && gridRow > 0;
			bool flag2 = gridRow % 6 == 5 && num < sectorHeight - 1;
			bool flag3 = gridCol % 6 == 0 && gridCol > 0;
			bool flag4 = gridCol % 6 == 5 && num2 < sectorWidth - 1;
			if (flag)
			{
				sectors[num - 1, num2].Initialize(this, num - 1, num2);
			}
			else if (flag2)
			{
				sectors[num + 1, num2].Initialize(this, num + 1, num2);
			}
			if (flag3)
			{
				sectors[num, num2 - 1].Initialize(this, num, num2 - 1);
			}
			else if (flag4)
			{
				sectors[num, num2 + 1].Initialize(this, num, num2 + 1);
			}
			if (flag && flag3)
			{
				sectors[num - 1, num2 - 1].Initialize(this, num - 1, num2 - 1);
			}
			else if (flag2 && flag3)
			{
				sectors[num + 1, num2 - 1].Initialize(this, num + 1, num2 - 1);
			}
			else if (flag && flag4)
			{
				sectors[num - 1, num2 + 1].Initialize(this, num - 1, num2 + 1);
			}
			else if (flag2 && flag4)
			{
				sectors[num + 1, num2 + 1].Initialize(this, num + 1, num2 + 1);
			}
		}
	}

	private void UpdateSingleSector(int sectorRow, int sectorCol)
	{
		if (ValidSector(sectorRow, sectorCol))
		{
			sectors[sectorRow, sectorCol].Initialize(this, sectorRow, sectorCol);
		}
	}

	private void UpdateAllSurroundingSectors(int sectorRow, int sectorCol)
	{
		if (meshesCreated)
		{
			UpdateSingleSector(sectorRow, sectorCol);
			UpdateSingleSector(sectorRow + 1, sectorCol);
			UpdateSingleSector(sectorRow - 1, sectorCol);
			UpdateSingleSector(sectorRow, sectorCol + 1);
			UpdateSingleSector(sectorRow + 1, sectorCol + 1);
			UpdateSingleSector(sectorRow - 1, sectorCol + 1);
			UpdateSingleSector(sectorRow, sectorCol - 1);
			UpdateSingleSector(sectorRow + 1, sectorCol - 1);
			UpdateSingleSector(sectorRow - 1, sectorCol - 1);
		}
	}

	private AlignedBox ComputeVisibleBounds()
	{
		Camera main = Camera.main;
		Vector3 point;
		ComputeIntersection(main.ViewportPointToRay(new Vector3(0f, 0f, 0f)), out point);
		point /= 20f;
		Vector3 point2;
		ComputeIntersection(main.ViewportPointToRay(new Vector3(0f, 1f, 0f)), out point2);
		point2 /= 20f;
		Vector3 point3;
		ComputeIntersection(main.ViewportPointToRay(new Vector3(1f, 0f, 0f)), out point3);
		point3 /= 20f;
		Vector3 point4;
		ComputeIntersection(main.ViewportPointToRay(new Vector3(1f, 1f, 0f)), out point4);
		point4 /= 20f;
		int num = (int)(Math.Min(point.x, Math.Min(point2.x, Math.Min(point3.x, point4.x))) - 0.5f);
		int num2 = (int)(Math.Max(point.x, Math.Max(point2.x, Math.Max(point3.x, point4.x))) - 0.5f);
		int num3 = (int)(Math.Min(point.y, Math.Min(point2.y, Math.Min(point3.y, point4.y))) - 0.5f);
		int num4 = (int)(Math.Max(point.y, Math.Max(point2.y, Math.Max(point3.y, point4.y))) - 0.5f);
		return new AlignedBox(num, num2, num3, num4);
	}

	private byte GenerateTerrainTile(int row, int col)
	{
		UnityEngine.Random.seed = GetSeed(row, col);
		float num = UnityEngine.Random.value;
		foreach (KeyValuePair<int, float> item in distribution)
		{
			num -= item.Value;
			if (num < 0f)
			{
				return (byte)item.Key;
			}
		}
		return byte.MaxValue;
	}

	private int GetSeed(int row, int col)
	{
		int num = terrainSeed * 127;
		num += row * 127;
		return num + col;
	}

	public static List<TerrainNode> LoadTerrainNodeData(List<object> data)
	{
		List<TerrainNode> list = new List<TerrainNode>();
		foreach (Dictionary<string, object> datum in data)
		{
			list.Add(new TerrainNode
			{
				did = TFUtils.LoadInt(datum, "did"),
				x = TFUtils.LoadInt(datum, "x"),
				y = TFUtils.LoadInt(datum, "y")
			});
		}
		return list;
	}

	private void ProcessOverrides()
	{
		foreach (TerrainNode foregroundOverride in foregroundOverrides)
		{
			tiles[foregroundOverride.x, foregroundOverride.y] = (byte)foregroundOverride.did;
		}
		foregroundOverrides = null;
	}
}
