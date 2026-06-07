using System;
using System.Collections.Generic;

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

	private readonly string[] grassTypeMaterialNames;

	private byte id;

	private byte cost;

	private byte mainTypeId;

	private string material;

	private string disabledMaterial;

	private List<KeyValuePair<int, float>> distribution;

	private string[] borderTypeMaterialNames;

	private bool canPave;

	private const byte TERRAIN_TYPE_PATH = 1;

	private const byte TERRAIN_TYPE_SAND = 2;

	private const byte TERRAIN_TYPE_MUD = 3;

	private const byte TERRAIN_TYPE_GRASS = 4;

	private const byte TERRAIN_TYPE_GOO = 5;

	public byte Id
	{
		get
		{
			return 0;
		}
	}

	public byte Cost
	{
		get
		{
			return 0;
		}
	}

	public string Material
	{
		get
		{
			return null;
		}
	}

	public byte MainTypeId
	{
		get
		{
			return 0;
		}
	}

	public TerrainType(Dictionary<string, object> data)
	{
	}

	public bool CanPave()
	{
		return false;
	}

	public bool IsPath()
	{
		return false;
	}

	public bool IsSand()
	{
		return false;
	}

	public bool IsMud()
	{
		return false;
	}

	public bool IsGrass()
	{
		return false;
	}

	public bool IsGoo()
	{
		return false;
	}

	public static byte GetPathTypeId()
	{
		return 0;
	}

	public string GetBorderMaterial(TileBorderType borderType)
	{
		return null;
	}

	public string GetGrassMaterial(GrassBorderType borderType)
	{
		return null;
	}

	public string GetPathMaterial(int offset)
	{
		return null;
	}

	public string GetDisabledMaterial()
	{
		return null;
	}

	public byte GenerateDecal(int seed)
	{
		return 0;
	}
}
