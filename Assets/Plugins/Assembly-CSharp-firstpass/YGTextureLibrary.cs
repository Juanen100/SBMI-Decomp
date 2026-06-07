using System.Collections.Generic;
using UnityEngine;
using Yarg;

[ExecuteInEditMode]
public class YGTextureLibrary : MonoBehaviour
{
	public class TextureTracker
	{
		public AtlasAndCoords atlasAndCoords;

		public Texture texture;

		public int count;
	}

	public struct FoundMaterial
	{
		public Material material;

		public YGTextureLibrary lib;

		public int index;

		public string name;

		public AtlasCoords coords;
	}

	public TextureAtlas[] textureAtlases;

	public TextAsset[] fontMaps;

	public FontAtlas[] fontAtlases;

	public Material materialPrototype;

	private static Dictionary<string, TextureTracker> textures;

	public const int INITIAL_TEXTURE_CAPACITY = 64;

	[HideInInspector]
	public bool bShowingDialog;

	private static TextureAtlas[] atlases;

	private bool initializing;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	public void incrementTextureDuplicates(string name)
	{
	}

	public void ThrowTextureNotFoundException(string name)
	{
	}

	public static string ActualName(string name)
	{
		return null;
	}

	public Texture LoadTexture(string name)
	{
		return null;
	}

	public Texture LoadUnmanagedAtlasTexture(AtlasAndCoords coords)
	{
		return null;
	}

	public Texture LoadUnmanagedAtlasTexture(TextureTracker tracker)
	{
		return null;
	}

	public Texture LoadUnmanagedAtlasTexture(string name)
	{
		return null;
	}

	public void UnLoadTexture(string name)
	{
	}

	public void LoadAtlases()
	{
	}

	public void LoadAtlasResources(string name)
	{
	}

	public static Material AtlasMaterial(string name)
	{
		return null;
	}

	public FoundMaterial FindSpriteMaterial(string asset)
	{
		return default(FoundMaterial);
	}

	public static AtlasAndCoords GetAtlasCoords(string spriteName)
	{
		return null;
	}

	public static bool HasAtlasCoords(string spriteName)
	{
		return false;
	}

	public bool UnloadAtlasResources(string name)
	{
		return false;
	}
}
