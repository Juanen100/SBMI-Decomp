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

	public AnimGroup FindAnimGroup(string state)
	{
		return null;
	}

	public void ApplyToGroups(ApplyDelegate apply)
	{
	}

	public void AddDisplayStateWithBlueprint(Dictionary<string, object> dict)
	{
	}

	public void CleanseAnimations(SkeletonCollection skeletons)
	{
	}
}
