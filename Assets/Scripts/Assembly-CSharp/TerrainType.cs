#define ASSERTS_ON
using System;
using System.Collections.Generic;
using UnityEngine;

public class TerrainType
{
	[Flags]
	public enum TileBorderType
	{
		XX = 0,
		UX = 1,
		XR = 2,
		UR = 3,
		OuterCorner = 4,
		MAX = 5
	}

	public enum GrassBorderType
	{
		LLXX = 0,
		LLXL = 1,
		LLUX = 2,
		LLUL = 3,
		LDXX = 4,
		LDXL = 5,
		LDUX = 6,
		LDUL = 7,
		DLXX = 8,
		DLXL = 9,
		DLUX = 10,
		DLUL = 11,
		DDXX = 12,
		DDXL = 13,
		DDUX = 14,
		DDUL = 15,
		MAX = 16
	}

	private const byte TERRAIN_TYPE_PATH = 1;

	private const byte TERRAIN_TYPE_SAND = 2;

	private const byte TERRAIN_TYPE_MUD = 3;

	private const byte TERRAIN_TYPE_GRASS = 4;

	private const byte TERRAIN_TYPE_GOO = 5;

	private readonly string[] grassTypeMaterialNames = new string[16]
	{
		"XXXX", "XLXL", "LXUX", "LLUL", "XXXX", "XDXL", "LXUX", "LDUL", "XXXX", "XLXL",
		"DXUX", "DLUL", "XXXX", "XDXL", "DXUX", "DDUL"
	};

	private byte id;

	private byte cost;

	private byte mainTypeId;

	private string material;

	private string disabledMaterial;

	private List<KeyValuePair<int, float>> distribution;

	private string[] borderTypeMaterialNames = new string[5];

	private bool canPave;

	public byte Id
	{
		get
		{
			return id;
		}
	}

	public byte Cost
	{
		get
		{
			return cost;
		}
	}

	public string Material
	{
		get
		{
			return material;
		}
	}

	public byte MainTypeId
	{
		get
		{
			return mainTypeId;
		}
	}

	public TerrainType(Dictionary<string, object> data)
	{
		id = (byte)TFUtils.LoadInt(data, "id");
		cost = (byte)TFUtils.LoadInt(data, "move_cost");
		if (data.ContainsKey("material"))
		{
			material = (string)data["material"];
		}
		if (data.ContainsKey("disabled_material"))
		{
			disabledMaterial = (string)data["disabled_material"];
		}
		distribution = new List<KeyValuePair<int, float>>();
		if (data.ContainsKey("decal_distribution"))
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)data["decal_distribution"];
			foreach (string key2 in dictionary.Keys)
			{
				int key = Convert.ToInt32(key2);
				float value = TFUtils.LoadFloat(dictionary, key2);
				distribution.Add(new KeyValuePair<int, float>(key, value));
			}
		}
		if (data.ContainsKey("can_pave"))
		{
			canPave = TFUtils.LoadBool(data, "can_pave");
		}
		else
		{
			canPave = true;
		}
		object value2;
		if (data.TryGetValue("main_type", out value2))
		{
			mainTypeId = (byte)Convert.ToInt32(value2);
			if (mainTypeId == 0)
			{
				mainTypeId = id;
			}
		}
		else
		{
			mainTypeId = id;
		}
		int num = material.LastIndexOf(".png");
		TFUtils.Assert(num > 0, string.Format("can't find .png in Terrain material name \"{0}\"", material));
		string text = material.Substring(0, num);
		if (IsPath())
		{
			borderTypeMaterialNames[0] = text + "XX.png";
			borderTypeMaterialNames[1] = text + "UX.png";
			borderTypeMaterialNames[2] = text + "XR.png";
			borderTypeMaterialNames[3] = text + "UR.png";
			borderTypeMaterialNames[4] = text + "OuterCorner.png";
		}
		else if (IsGrass())
		{
			for (int i = 0; i < 16; i++)
			{
				grassTypeMaterialNames[i] = text + grassTypeMaterialNames[i] + ".png";
			}
		}
	}

	public bool CanPave()
	{
		return canPave;
	}

	public bool IsPath()
	{
		return mainTypeId == 1;
	}

	public bool IsSand()
	{
		return mainTypeId == 2;
	}

	public bool IsMud()
	{
		return mainTypeId == 3;
	}

	public bool IsGrass()
	{
		return mainTypeId == 4;
	}

	public bool IsGoo()
	{
		return mainTypeId == 5;
	}

	public static byte GetPathTypeId()
	{
		return 1;
	}

	public string GetBorderMaterial(TileBorderType borderType)
	{
		return borderTypeMaterialNames[(int)borderType];
	}

	public string GetGrassMaterial(GrassBorderType borderType)
	{
		return grassTypeMaterialNames[(int)borderType];
	}

	public string GetPathMaterial(int offset)
	{
		int length = material.LastIndexOf(".png");
		string text = material.Substring(0, length);
		return text + offset + ".png";
	}

	public string GetDisabledMaterial()
	{
		return disabledMaterial;
	}

	public byte GenerateDecal(int seed)
	{
		UnityEngine.Random.seed = seed;
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
}
