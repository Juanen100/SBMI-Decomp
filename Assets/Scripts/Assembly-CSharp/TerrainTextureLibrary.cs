using UnityEngine;
using Yarg;

public class TerrainTextureLibrary
{
	public const byte TT_INTERNAL = 0;

	public const byte TT_EDGE = 1;

	public const byte TT_CORNER = 2;

	public const byte TT_TYPE_SHIFT = 3;

	public const byte TT_ROTATION_MASK = 7;

	private byte[] tileEdges = new byte[16];

	private TextureAtlas atlas;

	private static string terrainAtlasFile = TFUtils.GetStreamingAssetsFileInDirectory("Terrain", "terrainsheet.txt");

	public TerrainTextureLibrary()
	{
		TFUtils.DebugLog("Loading Terrain Texture file: " + terrainAtlasFile);
		string json = TFUtils.ReadAllText(terrainAtlasFile);
		atlas = TextureAtlas.LoadJsonAtlas(json);
		tileEdges[0] = 0;
		tileEdges[1] = 11;
		tileEdges[2] = 10;
		tileEdges[3] = 18;
		tileEdges[4] = 9;
		tileEdges[5] = tileEdges[0];
		tileEdges[6] = 17;
		tileEdges[7] = tileEdges[0];
		tileEdges[8] = 8;
		tileEdges[9] = 19;
		tileEdges[10] = tileEdges[0];
		tileEdges[11] = tileEdges[0];
		tileEdges[12] = 16;
		tileEdges[13] = tileEdges[0];
		tileEdges[14] = tileEdges[0];
		tileEdges[15] = tileEdges[0];
	}

	public Rect? GetMaterialUVs(string material)
	{
		AtlasCoords atlasCoords = atlas[material];
		if (atlasCoords != null)
		{
			float x = atlas.meta.invScale.x;
			float y = atlas.meta.invScale.y;
			return new Rect
			{
				x = atlasCoords.frame.x * x,
				width = atlasCoords.frame.width * x,
				y = (atlas.meta.size.height - (atlasCoords.frame.y + atlasCoords.frame.height)) * y,
				height = atlasCoords.frame.height * y
			};
		}
		return null;
	}

	public byte GetTileTypeAndRotation(byte index)
	{
		return tileEdges[index];
	}
}
