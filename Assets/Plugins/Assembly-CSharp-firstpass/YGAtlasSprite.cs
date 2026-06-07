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
	}

	public override Vector2 ResetSize()
	{
		return default(Vector2);
	}

	public virtual void SetUVs(SpriteCoordinates coords)
	{
	}

	protected virtual void LoadSprite()
	{
	}

	public virtual SpriteCoordinates LoadEmptySprite()
	{
		return null;
	}

	public virtual SpriteCoordinates LoadSprite(string name, Rect frame)
	{
		return null;
	}

	public TextureAtlas GetAtlas()
	{
		return null;
	}

	public virtual SpriteCoordinates LoadSpriteFromAtlas(string name, int atlasIndex)
	{
		return null;
	}

	public virtual SpriteCoordinates LoadSpriteFromAtlas(string name, int atlasIndex, YGTextureLibrary library)
	{
		return null;
	}

	public override void AssembleMesh()
	{
	}

	protected override Vector2 GetMainTextureSize(bool fromShared)
	{
		return default(Vector2);
	}

	public void SetNonAtlasName(string nonAtlasName)
	{
	}

	protected override void OnDisable()
	{
	}
}
