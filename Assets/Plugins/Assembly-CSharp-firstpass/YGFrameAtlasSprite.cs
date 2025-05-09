using UnityEngine;
using Yarg;

public class YGFrameAtlasSprite : YGAtlasSprite
{
	public RectOffset padding;

	private void Awake()
	{
		verts = YGFrameSprite.BuildVerts(size, padding, scale);
		colors = new Color[verts.Length];
	}

	protected override void OnEnable()
	{
		lockAspect = false;
		base.OnEnable();
	}

	public override void SetSize(Vector2 s)
	{
		size = s;
		update.verts = YGFrameSprite.BuildVerts(size, padding, scale);
		base.View.RefreshEvent += base.UpdateMesh;
	}

	public override void SetColor(Color c)
	{
		color = c;
		YGSprite.BuildColors(color, ref colors);
		update.colors = colors;
		base.View.RefreshEvent += base.UpdateMesh;
	}

	public override void SetAlpha(float alpha)
	{
		color.a = alpha;
		YGSprite.BuildColors(color, ref colors);
		update.colors = colors;
		base.View.RefreshEvent += base.UpdateMesh;
	}

	public override SpriteCoordinates LoadSprite(string name, Rect frame)
	{
		SpriteCoordinates spriteCoordinates = new SpriteCoordinates(name);
		spriteCoordinates.coords = frame;
		spriteCoordinates.verts = YGFrameSprite.BuildVerts(size, padding, scale);
		YGSprite.BuildColors(color, ref colors);
		spriteCoordinates.normals = YGSprite.BuildNormals(spriteCoordinates.verts.Length);
		spriteCoordinates.tris = YGFrameSprite.BuildTris();
		spriteCoordinates.color = colors;
		spriteCoordinates.uvs = YGFrameSprite.BuildUVs(spriteCoordinates.coords, padding, textureSize);
		return spriteCoordinates;
	}
}
