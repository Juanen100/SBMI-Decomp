using UnityEngine;
using Yarg;

public class YGAtlasSprite : YGSprite
{
	public SpriteCoordinates sprite;

	public string nonAtlasName;

	[HideInInspector]
	public int atlasIndex;

	protected override void OnEnable()
	{
		YGTextureLibrary library = base.View.Library;
		TextureAtlas textureAtlas = library.textureAtlases[atlasIndex];
		Material atlasMaterial = textureAtlas.GetAtlasMaterial();
		if (base.GetComponent<Renderer>().sharedMaterial != atlasMaterial)
		{
			SetMaterial(atlasMaterial);
		}
		base.OnEnable();
	}

	public override Vector2 ResetSize()
	{
		size.Set(sprite.coords.width, sprite.coords.height);
		YGSprite.BuildVerts(size, scale, ref verts);
		sprite.verts = verts;
		update.verts = sprite.verts;
		UpdateMesh();
		return size;
	}

	public virtual void SetUVs(SpriteCoordinates coords)
	{
		sprite = LoadSprite(coords.name, coords.coords);
		base.View.RefreshEvent += AssembleMesh;
	}

	protected virtual void LoadSprite()
	{
		nonAtlasName = string.Empty;
		sprite = LoadSprite(sprite.name, sprite.coords);
	}

	public virtual SpriteCoordinates LoadEmptySprite()
	{
		return LoadSprite(null, new Rect(0f, 0f, textureSize.x, textureSize.y));
	}

	public virtual SpriteCoordinates LoadSprite(string name, Rect frame)
	{
		SpriteCoordinates spriteCoordinates = new SpriteCoordinates(name);
		spriteCoordinates.coords = frame;
		YGSprite.BuildVerts(size, scale, ref verts);
		YGSprite.BuildColors(color, ref colors);
		YGSprite.BuildUVs(spriteCoordinates.coords, textureSize, ref uvs);
		spriteCoordinates.verts = verts;
		spriteCoordinates.normals = normals;
		spriteCoordinates.tris = tris;
		spriteCoordinates.color = colors;
		spriteCoordinates.uvs = uvs;
		return spriteCoordinates;
	}

	public TextureAtlas GetAtlas()
	{
		return base.View.Library.textureAtlases[atlasIndex];
	}

	public virtual SpriteCoordinates LoadSpriteFromAtlas(string name, int atlasIndex)
	{
		if (base.View == null)
		{
			Debug.LogError("No GUIView in scene");
			return new SpriteCoordinates(name);
		}
		return LoadSpriteFromAtlas(name, atlasIndex, base.View.Library);
	}

	public virtual SpriteCoordinates LoadSpriteFromAtlas(string name, int atlasIndex, YGTextureLibrary library)
	{
		name = name.Trim();
		TextureAtlas textureAtlas = library.textureAtlases[atlasIndex];
		if (textureAtlas == null)
		{
			Debug.LogError("Texture Atlas is null");
			SpriteCoordinates spriteCoordinates = LoadEmptySprite();
			spriteCoordinates.name = name;
			return spriteCoordinates;
		}
		AtlasCoords atlasCoords = textureAtlas[name];
		if (atlasCoords == null)
		{
			Debug.LogError(string.Format("Texture Atlas '{0}' does not contain '{1}'", textureAtlas.name, name));
			SpriteCoordinates spriteCoordinates2 = LoadEmptySprite();
			spriteCoordinates2.name = name;
			return spriteCoordinates2;
		}
		return LoadSprite(name, atlasCoords.frame);
	}

	public override void AssembleMesh()
	{
		if (sprite != null)
		{
			if (string.IsNullOrEmpty(sprite.name))
			{
				sprite = LoadEmptySprite();
				base.AssembleMesh();
			}
			else
			{
				LoadSprite();
				UpdateMesh(sprite.MeshUpdate);
			}
		}
	}

	protected override Vector2 GetMainTextureSize(bool fromShared)
	{
		TextureAtlas textureAtlas = base.View.Library.textureAtlases[atlasIndex];
		return new Vector2(textureAtlas.meta.size.width, textureAtlas.meta.size.height);
	}

	public void SetNonAtlasName(string nonAtlasName)
	{
		this.nonAtlasName = nonAtlasName;
	}

	protected override void OnDisable()
	{
		if (!string.IsNullOrEmpty(nonAtlasName))
		{
			base.View.Library.UnLoadTexture(nonAtlasName);
		}
		base.OnDisable();
	}
}
