#define ASSERTS_ON
using System.Collections.Generic;

public class AnimationGroupManager
{
	public class AnimGroup
	{
		public string skeletonName;

		public SkeletonAnimationModel animModel;
	}

	public delegate void ApplyDelegate(AnimGroup animGroup);

	private Dictionary<string, AnimGroup> animationGroups;

	public AnimationGroupManager()
	{
		animationGroups = new Dictionary<string, AnimGroup>();
	}

	public AnimGroup FindAnimGroup(string state)
	{
		AnimGroup result = null;
		foreach (string key in animationGroups.Keys)
		{
			AnimGroup animGroup = animationGroups[key];
			if (animGroup.animModel.HasAnimation(state))
			{
				result = animGroup;
				break;
			}
		}
		return result;
	}

	public void ApplyToGroups(ApplyDelegate apply)
	{
		foreach (string key in animationGroups.Keys)
		{
			apply(animationGroups[key]);
		}
	}

	public void AddDisplayStateWithBlueprint(Dictionary<string, object> dict)
	{
		if (!dict.ContainsKey("animation_resource") || !dict.ContainsKey("name"))
		{
			TFUtils.Assert(false, "Paperdoll.AddDisplayState(): dictionary does not contain required fields 'animation_resource' or 'name'");
			return;
		}
		string skeletonName = (string)dict["skeleton"];
		string key = (string)dict["group"];
		AnimGroup animGroup = null;
		SkeletonAnimationModel skeletonAnimationModel = null;
		if (!animationGroups.ContainsKey(key))
		{
			animGroup = new AnimGroup();
			animGroup.skeletonName = skeletonName;
			animGroup.animModel = new SkeletonAnimationModel();
			skeletonAnimationModel = animGroup.animModel;
			animationGroups.Add(key, animGroup);
		}
		else
		{
			animGroup = animationGroups[key];
			skeletonAnimationModel = animGroup.animModel;
		}
		skeletonAnimationModel.AddAnimationDataWithBlueprint(dict);
	}

	public void CleanseAnimations(SkeletonCollection skeletons)
	{
		foreach (AnimGroup value in animationGroups.Values)
		{
			skeletons.Cleanse(value);
		}
	}
}
