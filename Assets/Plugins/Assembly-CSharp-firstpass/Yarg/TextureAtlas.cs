using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MiniJSON;
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

		[NonSerialized]
		[HideInInspector]
		private RenderTextureBuffer textureBuffer;

		public string FullTexturePath
		{
			get
			{
				if (fullTexturePath == null)
				{
					fullTexturePath = "Textures/Atlases/" + texturePathName;
				}
				return fullTexturePath;
			}
		}

		public AtlasCoords this[string name]
		{
			get
			{
				BuildDictionary();
				AtlasCoords value = null;
				frames.TryGetValue(YGTextureLibrary.ActualName(name), out value);
				return value;
			}
		}

		public TextureAtlas()
		{
		}

		public TextureAtlas(Dictionary<string, object> source)
		{
			_Load(source);
		}

		public TextureAtlas(string fileName)
		{
			_Load(fileName);
		}

		public Material GetAtlasMaterial()
		{
			if (material == null)
			{
				LoadMaterial();
			}
			return material;
		}

		private void BuildDictionary()
		{
			if (frameArray == null || frameArray.Length == 0)
			{
				Load();
			}
			else if (frames != null && frames.Count == frameArray.Length)
			{
				return;
			}
			if (frames == null)
			{
				Debug.LogError("BuildDictionary: FAILED: NO FRAMES: SHOULD NEVER GET HERE");
			}
		}

		public ICollection<string> GetNames()
		{
			BuildDictionary();
			return frames.Keys;
		}

		public void _Load(Dictionary<string, object> source)
		{
			frames = new Dictionary<string, AtlasCoords>();
			if (source.ContainsKey("frames"))
			{
				Dictionary<string, object> dictionary = (Dictionary<string, object>)source["frames"];
				foreach (KeyValuePair<string, object> item in dictionary)
				{
					string key = YGTextureLibrary.ActualName(item.Key);
					frames[key] = new AtlasCoords(key, (Dictionary<string, object>)item.Value);
				}
				frameArray = new List<AtlasCoords>(frames.Values).ToArray();
			}
			if (source.ContainsKey("meta"))
			{
				meta = new AtlasMetaData((Dictionary<string, object>)source["meta"]);
				int num = meta.name.LastIndexOf('.');
				if (num >= 0)
				{
					name = meta.name.Substring(0, num);
				}
				else
				{
					name = meta.name;
				}
			}
			fullName = "Materials/lod/" + name;
			fullTexturePath = FullTexturePath;
		}

		public void _Load(string fileName)
		{
			BinaryReader binaryReader = null;
			try
			{
				string text = Application.persistentDataPath + "/Contents/Textures/AtlasCoordinates/cmp/";
				if (Language.CurrentLanguage() != LanguageCode.EN)
				{
					RefreshLanguages();
					string text2 = Language.LocalizedEnglishAssetName(fileName);
					string path = text + text2 + ".txt";
					if (File.Exists(path))
					{
						binaryReader = new BinaryReader(File.OpenRead(path));
					}
					else
					{
						TextAsset textAsset = (TextAsset)Resources.Load("Textures/AtlasCoordinates/cmp/" + text2, typeof(TextAsset));
						if (textAsset != null)
						{
							binaryReader = new BinaryReader(new MemoryStream(textAsset.bytes));
							Resources.UnloadAsset(textAsset);
						}
					}
				}
				if (binaryReader == null)
				{
					string path2 = text + fileName + ".txt";
					if (File.Exists(path2))
					{
						binaryReader = new BinaryReader(File.OpenRead(path2));
					}
					else
					{
						TextAsset textAsset2 = (TextAsset)Resources.Load("Textures/AtlasCoordinates/cmp/" + fileName, typeof(TextAsset));
						if (textAsset2 == null)
						{
							throw new Exception("Failed To Load Atlas: " + fileName);
						}
						binaryReader = new BinaryReader(new MemoryStream(textAsset2.bytes));
						Resources.UnloadAsset(textAsset2);
					}
				}
				int num = binaryReader.ReadInt32();
				if (num > 3 || num < 2)
				{
					throw new Exception("Invalid Atlas Version: " + num + " vs " + 3);
				}
				meta = new AtlasMetaData(binaryReader);
				int num2 = meta.name.LastIndexOf('.');
				if (num2 >= 0)
				{
					name = meta.name.Substring(0, num2);
				}
				else
				{
					name = meta.name;
				}
				fullName = "Materials/lod/" + name;
				int num3 = binaryReader.ReadInt32();
				frameArray = new AtlasCoords[num3];
				frames = new Dictionary<string, AtlasCoords>(num3);
				for (int i = 0; i < num3; i++)
				{
					frameArray[i] = new AtlasCoords(binaryReader, num);
					frames.Add(frameArray[i].name, frameArray[i]);
				}
				binaryReader.Close();
			}
			catch (Exception ex)
			{
				Debug.LogError("TextureAtlas: Error: Load: " + jsonFileName + " " + ex.Message + "\n" + ex.StackTrace);
			}
			binaryReader = null;
			fullTexturePath = FullTexturePath;
		}

		public string FullName()
		{
			if (fullName == null)
			{
				fullName = "Materials/lod/" + jsonFileName;
			}
			return fullName;
		}

		private Material LoadMaterial(string file, bool async_load = false)
		{
			return Resources.Load(file, typeof(Material)) as Material;
		}

		private void RefreshLanguages()
		{
			if (Language.CurrentLanguage() == LanguageCode.N)
			{
				Language.Init(Path.Combine(Application.persistentDataPath, "Contents"));
			}
		}

		public void LoadMaterial()
		{
			if (useSingleTexture)
			{
				return;
			}
			string text = "Materials/lod/";
			if (name.ToLower().Contains("localized_"))
			{
				RefreshLanguages();
				name = Language.LocalizedEnglishAssetName(name);
			}
			text += name;
			CommonUtils.LevelOfDetail levelOfDetail = CommonUtils.TextureLod();
			string text2 = string.Empty;
			switch (levelOfDetail)
			{
			case CommonUtils.LevelOfDetail.Low:
				text2 = "_lr2";
				break;
			case CommonUtils.LevelOfDetail.Standard:
				text2 = "_lr";
				break;
			}
			if (material != null)
			{
				return;
			}
			UnityEngine.Object obj = null;
			string text3 = CommonUtils.TextureForDeviceOverride(name);
			if (text3 != name)
			{
				Debug.LogError("LoadMaterialName: Override: " + text3);
				obj = FileSystemCoordinator.LoadAsset(text3);
				if (obj != null)
				{
					material = (Material)obj;
					return;
				}
				material = LoadMaterial("Materials/lod/" + text3);
				if (material != null)
				{
					return;
				}
			}
			obj = FileSystemCoordinator.LoadAsset(name + text2);
			if (obj != null)
			{
				material = (Material)obj;
				return;
			}
			material = LoadMaterial(text + text2);
			if (material == null && levelOfDetail != CommonUtils.LevelOfDetail.High)
			{
				material = LoadMaterial(text);
			}
		}

		public void RefreshMaterial()
		{
			if (material != null)
			{
				material.shader = Shader.Find(material.shader.name);
			}
		}

		public void Load()
		{
			TextureAtlas textureAtlas = new TextureAtlas(jsonFileName);
			frames = textureAtlas.frames;
			frameArray = textureAtlas.frameArray;
			meta = textureAtlas.meta;
		}

		public void LoadJsonAtlas()
		{
			TextAsset textAsset = null;
			TextureAtlas textureAtlas = LoadJsonAtlas(textAsset.text);
			frames = textureAtlas.frames;
			frameArray = textureAtlas.frameArray;
			meta = textureAtlas.meta;
		}

		public static TextureAtlas LoadJsonAtlas(string json)
		{
			Dictionary<string, object> source = (Dictionary<string, object>)Json.Deserialize(json);
			return new TextureAtlas(source);
		}

		public void AdjustUVsToFrame(AtlasCoords coords, ref float u0, ref float u1, ref float v0, ref float v1)
		{
			float x = meta.invScale.x;
			float y = meta.invScale.y;
			u0 = (coords.frame.x + u0 * coords.frame.width) * x;
			u1 = (coords.frame.x + u1 * coords.frame.width) * x;
			float num = meta.size.height - coords.frame.y - coords.frame.height;
			v0 = (num + coords.frame.height * v0) * y;
			v1 = (num + coords.frame.height * v1) * y;
		}

		public void GetUVs(AtlasCoords coords, ref Rect rect)
		{
			float x = meta.invScale.x;
			float y = meta.invScale.y;
			rect.xMin = coords.frame.x * x;
			rect.xMax = (coords.frame.x + coords.frame.width) * x;
			float num = meta.size.height - coords.frame.y - coords.frame.height;
			rect.yMin = num * y;
			rect.yMax = (num + coords.frame.height) * y;
		}

		public void Proccess(AtlasCoords coordData, string name)
		{
			if (useRenderTexture && coordData == null)
			{
				Debug.LogError("Null Coordinate Data: " + name);
			}
		}

		public int SpriteCount()
		{
			if (frameArray == null)
			{
				return 0;
			}
			return frameArray.Length;
		}

		public void AddAllTextureCoords(Dictionary<string, YGTextureLibrary.TextureTracker> textureData)
		{
			AtlasCoords[] array = frameArray;
			foreach (AtlasCoords atlasCoords in array)
			{
				AtlasAndCoords atlasAndCoords = new AtlasAndCoords();
				atlasAndCoords.atlas = this;
				atlasAndCoords.atlasCoords = atlasCoords;
				YGTextureLibrary.TextureTracker textureTracker = new YGTextureLibrary.TextureTracker();
				textureTracker.atlasAndCoords = atlasAndCoords;
				textureData.Add(atlasCoords.name, textureTracker);
			}
		}

		public void UnloadAtlasResources()
		{
			if (material == null)
			{
				return;
			}
			if (materialTextures != null)
			{
				for (int i = 0; i < materialTextures.Length; i++)
				{
					Texture texture = material.GetTexture(materialTextures[i]);
					if (!(texture == null))
					{
						Resources.UnloadAsset(texture);
					}
				}
				if (materialTextures.Length != 0)
				{
					material = null;
				}
			}
			if (material != null)
			{
				Resources.UnloadAsset(material.mainTexture);
			}
			material = null;
		}

		public static string _ReadString(BinaryReader reader)
		{
			byte b = reader.ReadByte();
			if (b == 0)
			{
				return string.Empty;
			}
			byte[] array = reader.ReadBytes(b);
			StringBuilder stringBuilder = new StringBuilder(b);
			for (int i = 0; i < b; i++)
			{
				stringBuilder.Append((char)array[i]);
			}
			return stringBuilder.ToString();
		}
	}
}
