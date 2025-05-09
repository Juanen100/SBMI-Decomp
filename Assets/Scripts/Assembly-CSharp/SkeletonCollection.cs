#define ASSERTS_ON
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SkeletonCollection
{
	private Dictionary<string, GameObject> skeletons;

	public SkeletonCollection()
	{
		skeletons = new Dictionary<string, GameObject>();
	}

	public GameObject GetSkeleton(string key, bool createIfNotFound, out bool createdResource)
	{
		createdResource = false;
		GameObject value = null;
		if (!skeletons.TryGetValue(key, out value) && createIfNotFound)
		{
			Object obj = FileSystemCoordinator.LoadAsset(Path.GetFileName(key));
			if (obj != null)
			{
				value = (GameObject)Object.Instantiate(obj);
			}
			if (value == null)
			{
				value = UnityGameResources.Create(key);
			}
			skeletons.Add(key, value);
			createdResource = true;
		}
		TFUtils.Assert(value != null, "SkeletonCollection.GetSkeleton should not be null after creation");
		return value;
	}

	public void Cleanse(AnimationGroupManager.AnimGroup animGroup)
	{
		GameObject value = null;
		if (skeletons.TryGetValue(animGroup.skeletonName, out value))
		{
			animGroup.animModel.UnapplyAnimationSettings(value.GetComponent<Animation>());
			Object.Destroy(value);
			skeletons.Remove(animGroup.skeletonName);
		}
	}

	public void Cleanse(string key)
	{
		if (skeletons.ContainsKey(key))
		{
			Object.Destroy(skeletons[key]);
			skeletons.Remove(key);
		}
	}
}
