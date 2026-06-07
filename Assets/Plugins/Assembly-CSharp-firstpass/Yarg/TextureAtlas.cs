using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Yarg
{
	[Serializable]
	public sealed class TextureAtlas : ILoadable
	{
		public const string MaterialPath_Resources = "Materials/lod/";

		public const string MaterialJMatPath_Persistant = "Contents/Materials/jmat/";

		public const string MaterialJMatPath_Resources = "Materials/jmat/";

		public const string TexturePath_Persistant = "Contents/Textures/";

		public const string AtlasJsonPath_Resources = "Textures/AtlasCoordinates/";

		public const string AtlasPath_CMP = "Textures/AtlasCoordinates/cmp/";

		public const string AtlasPath_Persistant = "Contents/Textures/AtlasCoordinates/";

		public const string lowRez2Option = "_lr2";

		public const string lowRezOption = "_lr";

		public const int SPRITE_UV_ATLAS_VERSION = 2;

		public const int COMPATIBLE_ATLAS_VERSION = 2;

		public const int COMPILED_ATLAS_VERSION = 3;

		public string name;

		public string jsonFileName;

		public string texturePathName;

		public bool addToSpriteMap;

		public bool useDeviceTypeForMaterials;

		private Material material;

		public string[] materialTextures;

		public AtlasMetaData meta;

		public bool useSingleTexture;

		public bool useRenderTexture;

		[NonSerialized]
		[HideInInspector]
		private Dictionary<string, AtlasCoords> frames;

		[NonSerialized]
		[HideInInspector]
		private AtlasCoords[] frameArray;

		[NonSerialized]
		[HideInInspector]
		public string fullName;

		[NonSerialized]
		[HideInInspector]
		private string fullTexturePath;

		public string FullTexturePath
		{
			get
			{
				return null;
			}
		}

		public AtlasCoords Item
		{
			get
			{
				return null;
			}
		}

		public TextureAtlas()
		{
		}

		public TextureAtlas(Dictionary<string, object> source)
		{
		}

		public TextureAtlas(string fileName)
		{
		}

		public Material GetAtlasMaterial()
		{
			return null;
		}

		private void BuildDictionary()
		{
		}

		public ICollection<string> GetNames()
		{
			return null;
		}

		public void _Load(Dictionary<string, object> source)
		{
		}

		public void _Load(string fileName)
		{
		}

		public string FullName()
		{
			return null;
		}

		private Material LoadMaterial(string file, bool async_load = false)
		{
			return null;
		}

		private void RefreshLanguages()
		{
		}

		public void LoadMaterial()
		{
		}

		public void RefreshMaterial()
		{
		}

		public void Load()
		{
		}

		public void LoadJsonAtlas()
		{
		}

		public static TextureAtlas LoadJsonAtlas(string json)
		{
			return null;
		}

		public void AdjustUVsToFrame(AtlasCoords coords, ref float u0, ref float u1, ref float v0, ref float v1)
		{
		}

		public void GetUVs(AtlasCoords coords, ref Rect rect)
		{
		}

		public void Proccess(AtlasCoords coordData, string name)
		{
		}

		public int SpriteCount()
		{
			return 0;
		}

		public void AddAllTextureCoords(Dictionary<string, YGTextureLibrary.TextureTracker> textureData)
		{
		}

		public void UnloadAtlasResources()
		{
		}

		public static string _ReadString(BinaryReader reader)
		{
			return null;
		}
	}
}
