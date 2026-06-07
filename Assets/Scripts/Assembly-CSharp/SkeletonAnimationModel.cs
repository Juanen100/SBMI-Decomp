using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimationModel : ULAnimModel
{
	public static Dictionary<string, WrapMode> wrapModeDictionary;

	public static Dictionary<string, PlayMode> playModeDictionary;

	public static Dictionary<string, AnimationBlendMode> blendModeDictionary;

	public SkeletonAnimationModel()
		: base(null)
	{
	}

	public SkeletonAnimationSetting SkeletonSettings(string animName)
	{
		return null;
	}

	public string AnimationEventsKey(string animName)
	{
		return null;
	}

	public string ItemResource(string animName)
	{
		return null;
	}

	public string ObjectResource(string animName)
	{
		return null;
	}

	public Vector3 ItemScale(string animName)
	{
		return default(Vector3);
	}

	public Vector3 ObjectScale(string animName)
	{
		return default(Vector3);
	}

	public void AddAnimationDataWithBlueprint(Dictionary<string, object> dict)
	{
	}
}
