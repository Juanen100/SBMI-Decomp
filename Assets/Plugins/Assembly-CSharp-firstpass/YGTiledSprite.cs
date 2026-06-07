using UnityEngine;

public class YGTiledSprite : YGSprite
{
	public Vector2 tileSize;

	public Vector2 tileScale;

	public Vector2 tileOffset;

	protected override void OnEnable()
	{
	}

	public static int[] BuildTris(Vector3[] verts)
	{
		return null;
	}

	public static Vector3[] BuildVerts(Vector2 size, Vector2 tileSize, Vector2 scale)
	{
		return null;
	}

	public static Vector2[] BuildUVs(Rect rect, Vector2 size, Vector3[] verts)
	{
		return null;
	}

	public override void AssembleMesh()
	{
	}
}
