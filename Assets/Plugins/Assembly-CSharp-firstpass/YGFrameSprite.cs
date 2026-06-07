using UnityEngine;

public class YGFrameSprite : YGSprite
{
	public RectOffset padding;

	protected override void OnEnable()
	{
	}

	public new static int[] BuildTris()
	{
		return null;
	}

	public static Vector3[] BuildVerts(Vector2 size, RectOffset padding, Vector2 scale)
	{
		return null;
	}

	public static Vector2[] BuildUVs(Rect rect, RectOffset padding, Vector2 size)
	{
		return null;
	}

	public override void SetSize(Vector2 s)
	{
	}

	public override void SetColor(Color c)
	{
	}

	public override void SetAlpha(float alpha)
	{
	}

	public override void AssembleMesh()
	{
	}
}
