using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimationModel : ULSpriteAnimModel
{
	public SpriteAnimationModel()
		: base((ULSpriteAnimationSetting[])null)
	{
	}

	public void AddAnimationDataWithBlueprint(Dictionary<string, object> data)
	{
	}

	public bool HasQuadData(string animName)
	{
		return false;
	}

	public int Width(string animName)
	{
		return 0;
	}

	public int Height(string animName)
	{
		return 0;
	}

	public Vector3 Scale(string animName)
	{
		return default(Vector3);
	}
}
