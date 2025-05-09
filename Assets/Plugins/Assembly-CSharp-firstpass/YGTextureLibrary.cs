using System;
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

	public const int INITIAL_TEXTURE_CAPACITY = 64;

	public TextureAtlas[] textureAtlases = new TextureAtlas[0];

	public TextAsset[] fontMaps = new TextAsset[0];

	public FontAtlas[] fontAtlases = new FontAtlas[0];

	public Material materialPrototype;

	private static Dictionary<string, TextureTracker> textures = new Dictionary<string, TextureTracker>();

	[HideInInspector]
	public bool bShowingDialog;

	private static TextureAtlas[] atlases = null;

	private void Awake()
	{
		atlases = new TextureAtlas[textureAtlases.Length];
		for (int i = 0; i < atlases.Length; i++)
		{
			atlases[i] = textureAtlases[i];
		}
		LoadAtlases();
	}

	public void incrementTextureDuplicates(string name)
	{
		TextureTracker value = null;
		if (textures.TryGetValue(ActualName(name), out value))
		{
			value.count++;
		}
	}

	public void ThrowTextureNotFoundException(string name)
	{
		throw new Exception("Texture Not Found: " + name);
	}

	public static string ActualName(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return name;
		}
		int length = name.Length;
		if (length < 4)
		{
			return name;
		}
		if (name[length - 4] == '.')
		{
			return name.Substring(0, length - 4);
		}
		return name;
	}

	public Texture LoadTexture(string name)
	{
		TextureTracker value = null;
		if (string.IsNullOrEmpty(name))
		{
			ThrowTextureNotFoundException("NULL");
		}
		string text = ActualName(name);
		bool flag = false;
		if (textures.TryGetValue(text, out value))
		{
			flag = value.texture == null;
		}
		if (flag)
		{
			if (CommonUtils.TextureLod() != CommonUtils.LevelOfDetail.High)
			{
				string path = value.atlasAndCoords.atlas.FullTexturePath + "/_lr/" + text;
				value.texture = (Texture)Resources.Load(path, typeof(Texture));
			}
			if (value.texture == null)
			{
				string path2 = value.atlasAndCoords.atlas.FullTexturePath + "/" + text;
				value.texture = (Texture)Resources.Load(path2, typeof(Texture));
				if (value.texture == null)
				{
					ThrowTextureNotFoundException(name);
				}
			}
			value.count = 1;
		}
		else
		{
			value.count++;
		}
		return value.texture;
	}

	public Texture LoadUnmanagedAtlasTexture(AtlasAndCoords coords)
	{
		if (coords == null)
		{
			Debug.LogError("Invalid Coordinate Data");
			return null;
		}
		Texture texture = null;
		if (CommonUtils.TextureLod() != CommonUtils.LevelOfDetail.High)
		{
			string path = coords.atlas.FullTexturePath + "/_lr/" + coords.atlasCoords.name;
			texture = (Texture)Resources.Load(path, typeof(Texture));
		}
		if (texture == null)
		{
			string path2 = coords.atlas.FullTexturePath + "/" + coords.atlasCoords.name;
			texture = (Texture)Resources.Load(path2, typeof(Texture));
			if (texture == null)
			{
				ThrowTextureNotFoundException(base.name);
			}
		}
		return texture;
	}

	public Texture LoadUnmanagedAtlasTexture(TextureTracker tracker)
	{
		if (tracker == null)
		{
			return null;
		}
		return LoadUnmanagedAtlasTexture(tracker.atlasAndCoords);
	}

	public Texture LoadUnmanagedAtlasTexture(string name)
	{
		Texture result = null;
		TextureTracker value = null;
		if (textures.TryGetValue(ActualName(name), out value))
		{
			result = LoadUnmanagedAtlasTexture(value.atlasAndCoords);
		}
		return result;
	}

	public void UnLoadTexture(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return;
		}
		int num = 0;
		TextureTracker value = null;
		string key = ActualName(name);
		if (!textures.TryGetValue(key, out value))
		{
			return;
		}
		if (value.count < 2)
		{
			if (value.texture != null)
			{
				Resources.UnloadAsset(value.texture);
				value.texture = null;
				value.count = 0;
			}
		}
		else
		{
			value.count--;
		}
	}

	public void LoadAtlases()
	{
		int num = 0;
		TextureAtlas[] array = textureAtlases;
		foreach (TextureAtlas textureAtlas in array)
		{
			textureAtlas.Load();
			if (textureAtlas.addToSpriteMap)
			{
				num += textureAtlas.SpriteCount();
			}
		}
		textures = new Dictionary<string, TextureTracker>(num + 1);
		TextureAtlas[] array2 = textureAtlases;
		foreach (TextureAtlas textureAtlas2 in array2)
		{
			if (textureAtlas2.addToSpriteMap)
			{
				textureAtlas2.AddAllTextureCoords(textures);
			}
		}
		if (fontMaps.Length > 0)
		{
			List<TextAsset> maps = new List<TextAsset>(fontMaps);
			maps.Sort((TextAsset x, TextAsset y) => x.name.CompareTo(y.name));
			fontMaps = maps.ToArray();
			List<FontAtlas> list = new List<FontAtlas>(fontAtlases);
			for (int i2 = 0; i2 < maps.Count; i2++)
			{
				if (list.Find((FontAtlas x) => x.fnt.name == maps[i2].name) == null)
				{
					FontAtlas fontAtlas = new FontAtlas();
					fontAtlas.fnt = maps[i2];
					list.Add(fontAtlas);
				}
			}
			fontAtlases = list.ToArray();
		}
		FontAtlas[] array3 = fontAtlases;
		foreach (FontAtlas fontAtlas2 in array3)
		{
			fontAtlas2.Load();
		}
	}

	public void LoadAtlasResources(string name)
	{
		TextureAtlas[] array = textureAtlases;
		foreach (TextureAtlas textureAtlas in array)
		{
			if (!(textureAtlas.name != name))
			{
				textureAtlas.LoadMaterial();
				textureAtlas.RefreshMaterial();
			}
		}
	}

	public static Material AtlasMaterial(string name)
	{
		TextureAtlas[] array = atlases;
		foreach (TextureAtlas textureAtlas in array)
		{
			if (!(textureAtlas.name != name) || !(textureAtlas.FullName() != name))
			{
				return textureAtlas.GetAtlasMaterial();
			}
		}
		return null;
	}

	public FoundMaterial FindSpriteMaterial(string asset)
	{
		FoundMaterial result = default(FoundMaterial);
		TextureTracker value = null;
		if (textures.TryGetValue(ActualName(asset), out value))
		{
			result.lib = this;
			result.material = value.atlasAndCoords.atlas.GetAtlasMaterial();
			for (int i = 0; i < textureAtlases.Length; i++)
			{
				if (value.atlasAndCoords.atlas == textureAtlases[i])
				{
					result.index = i;
					break;
				}
			}
			result.name = asset;
			result.coords = value.atlasAndCoords.atlasCoords;
		}
		else
		{
			result.index = -1;
		}
		return result;
	}

	public static AtlasAndCoords GetAtlasCoords(string spriteName)
	{
		TextureTracker value = null;
		if (textures.TryGetValue(ActualName(spriteName), out value))
		{
			return value.atlasAndCoords;
		}
		Debug.LogError("Failed to find sprite coordinates: " + spriteName);
		return null;
	}

	public static bool HasAtlasCoords(string spriteName)
	{
		return textures.ContainsKey(ActualName(spriteName));
	}

	public bool UnloadAtlasResources(string name)
	{
		for (int i = 0; i < textureAtlases.Length; i++)
		{
			if (textureAtlases[i].name == name)
			{
				textureAtlases[i].UnloadAtlasResources();
				return true;
			}
		}
		return false;
	}
}
