using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimationModel : ULAnimModel
{
	public static Dictionary<string, WrapMode> wrapModeDictionary = new Dictionary<string, WrapMode>
	{
		{
			"clamp",
			WrapMode.Once
		},
		{
			"clamp_forever",
			WrapMode.ClampForever
		},
		{
			"default",
			WrapMode.Default
		},
		{
			"loop",
			WrapMode.Loop
		},
		{
			"once",
			WrapMode.Once
		},
		{
			"pingpong",
			WrapMode.PingPong
		}
	};

	public static Dictionary<string, PlayMode> playModeDictionary = new Dictionary<string, PlayMode>
	{
		{
			"stop_all",
			PlayMode.StopAll
		},
		{
			"stop_same_layer",
			PlayMode.StopSameLayer
		}
	};

	public static Dictionary<string, AnimationBlendMode> blendModeDictionary = new Dictionary<string, AnimationBlendMode>
	{
		{
			"additive",
			AnimationBlendMode.Additive
		},
		{
			"blend",
			AnimationBlendMode.Blend
		}
	};

	public SkeletonAnimationSetting SkeletonSettings(string animName)
	{
		return (SkeletonAnimationSetting)animationHashtable[animName];
	}

	public string AnimationEventsKey(string animName)
	{
		return ((SkeletonAnimationSetting)animationHashtable[animName]).animationEventsKey;
	}

	public string ItemResource(string animName)
	{
		return ((SkeletonAnimationSetting)animationHashtable[animName]).itemResource;
	}

	public string ObjectResource(string animName)
	{
		return ((SkeletonAnimationSetting)animationHashtable[animName]).objectResource;
	}

	public Vector3 ItemScale(string animName)
	{
		return ((SkeletonAnimationSetting)animationHashtable[animName]).itemScale;
	}

	public Vector3 ObjectScale(string animName)
	{
		return ((SkeletonAnimationSetting)animationHashtable[animName]).objectScale;
	}

	public void AddAnimationDataWithBlueprint(Dictionary<string, object> dict)
	{
		string key = TFUtils.LoadString(dict, "name");
		string text = TFUtils.LoadString(dict, "animation_resource");
		Vector3 v = Vector3.one;
		Vector3 v2 = Vector3.one;
		SkeletonAnimationSetting skeletonAnimationSetting = new SkeletonAnimationSetting();
		object value = null;
		if (dict.TryGetValue("item_prop", out value))
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)value;
			skeletonAnimationSetting.itemResource = TFUtils.LoadString(dictionary, "item_resource");
			skeletonAnimationSetting.itemBone = TFUtils.LoadString(dictionary, "item_bone", "BN_ITEM");
			if (skeletonAnimationSetting.itemBone == "base")
			{
				skeletonAnimationSetting.itemBone = null;
			}
			TFUtils.LoadVector3(out v, (Dictionary<string, object>)dictionary["item_scale"]);
		}
		if (dict.TryGetValue("object_prop", out value))
		{
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)value;
			skeletonAnimationSetting.objectResource = TFUtils.LoadString(dictionary2, "object_resource");
			skeletonAnimationSetting.objectBone = TFUtils.LoadString(dictionary2, "object_bone", "BN_OBJECT");
			if (skeletonAnimationSetting.objectBone == "base")
			{
				skeletonAnimationSetting.objectBone = null;
			}
			TFUtils.LoadVector3(out v2, (Dictionary<string, object>)dictionary2["object_scale"]);
		}
		TFUtils.DebugLog("Loading resource " + text, TFUtils.LogFilter.Assets);
		skeletonAnimationSetting.resource = text;
		skeletonAnimationSetting.blendMode = AnimationBlendMode.Blend;
		skeletonAnimationSetting.wrapMode = WrapMode.Default;
		skeletonAnimationSetting.playMode = PlayMode.StopSameLayer;
		skeletonAnimationSetting.layer = 0;
		skeletonAnimationSetting.animationEventsKey = null;
		skeletonAnimationSetting.itemScale = v;
		skeletonAnimationSetting.objectScale = v2;
		if (dict.TryGetValue("blend_mode", out value))
		{
			skeletonAnimationSetting.blendMode = blendModeDictionary[(string)value];
		}
		if (dict.TryGetValue("wrap_mode", out value))
		{
			skeletonAnimationSetting.wrapMode = wrapModeDictionary[(string)value];
		}
		if (dict.TryGetValue("play_mode", out value))
		{
			skeletonAnimationSetting.playMode = playModeDictionary[(string)value];
		}
		if (dict.ContainsKey("layer"))
		{
			skeletonAnimationSetting.layer = TFUtils.LoadInt(dict, "layer");
		}
		if (dict.TryGetValue("animation_events", out value))
		{
			skeletonAnimationSetting.animationEventsKey = (string)value;
		}
		skeletonAnimationSetting.unloadable = TFUtils.LoadBool(dict, "unloadable", false);
		AddAnimationSetting(key, skeletonAnimationSetting);
	}
}
