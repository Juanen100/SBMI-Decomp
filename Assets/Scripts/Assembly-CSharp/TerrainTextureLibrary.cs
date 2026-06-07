using UnityEngine;
using Yarg;

public class TerrainTextureLibrary
{
	public const byte TT_INTERNAL = 0;

	public const byte TT_EDGE = 1;

	public const byte TT_CORNER = 2;

	public const byte TT_TYPE_SHIFT = 3;

	public const byte TT_ROTATION_MASK = 7;

	private byte[] tileEdges;

	private TextureAtlas atlas;

	private static string terrainAtlasFile;

	public Rect? GetMaterialUVs(string material)
	{
		return null;
	}

	public byte GetTileTypeAndRotation(byte index)
	{
		return 0;
	}
}
