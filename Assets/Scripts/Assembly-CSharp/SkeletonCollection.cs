using System.Collections.Generic;
using UnityEngine;

public class SkeletonCollection
{
	private Dictionary<string, GameObject> skeletons;

	public GameObject GetSkeleton(string key, bool createIfNotFound, out bool createdResource)
	{
		createdResource = default(bool);
		return null;
	}

	public void Cleanse(AnimationGroupManager.AnimGroup animGroup)
	{
	}

	public void Cleanse(string key)
	{
	}
}
