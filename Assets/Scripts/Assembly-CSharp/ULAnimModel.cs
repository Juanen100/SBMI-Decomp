#define ASSERTS_ON
using System.Collections;
using System.IO;
using UnityEngine;

public class ULAnimModel : ULAnimModelInterface
{
	protected Hashtable animationHashtable;

	public ULAnimModel(Hashtable hashtable)
	{
		animationHashtable = hashtable;
	}

	public ULAnimModel()
	{
		animationHashtable = new Hashtable();
	}

	public void AddAnimationSetting(string key, ULAnimationSetting setting)
	{
		animationHashtable.Add(key, setting);
	}

	public bool HasAnimation(string animName)
	{
		return animationHashtable.ContainsKey(animName);
	}

	public AnimationClip AnimClip(string animName)
	{
		TFUtils.Assert(false, "We should not be loading anim clips from ULAnimModels");
		return null;
	}

	public AnimationBlendMode AnimBlendMode(string animName)
	{
		return ((ULAnimationSetting)animationHashtable[animName]).blendMode;
	}

	public WrapMode AnimWrapMode(string animName)
	{
		return ((ULAnimationSetting)animationHashtable[animName]).wrapMode;
	}

	public PlayMode AnimPlayMode(string animName)
	{
		return ((ULAnimationSetting)animationHashtable[animName]).playMode;
	}

	public int AnimLayer(string animName)
	{
		return ((ULAnimationSetting)animationHashtable[animName]).layer;
	}

	public void ApplyAnimationSettings(Animation targetAnimation)
	{
		foreach (string key in animationHashtable.Keys)
		{
			ULAnimationSetting uLAnimationSetting = (ULAnimationSetting)animationHashtable[key];
			Object obj = FileSystemCoordinator.LoadAsset(Path.GetFileName(uLAnimationSetting.resource));
			if (obj == null)
			{
				obj = Resources.Load(uLAnimationSetting.resource);
			}
			if (obj == null)
			{
				TFUtils.ErrorLog("Something went wrong trying to load " + uLAnimationSetting.resource);
				continue;
			}
			GameObject gameObject = Object.Instantiate(obj) as GameObject;
			targetAnimation.AddClip(gameObject.GetComponent<Animation>().clip, key);
			Object.DestroyImmediate(gameObject);
			targetAnimation[key].blendMode = uLAnimationSetting.blendMode;
			targetAnimation[key].wrapMode = uLAnimationSetting.wrapMode;
			targetAnimation[key].layer = uLAnimationSetting.layer;
		}
	}

	public void UnapplyAnimationSettings(Animation targetAnimation)
	{
		foreach (string key in animationHashtable.Keys)
		{
			AnimationClip clip = targetAnimation.GetClip(key);
			targetAnimation.RemoveClip(key);
			Object.DestroyImmediate(clip);
		}
	}
}
