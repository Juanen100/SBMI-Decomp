using System.Collections;
using UnityEngine;

public class ULAnimModel : ULAnimModelInterface
{
	protected Hashtable animationHashtable;

	public ULAnimModel(Hashtable hashtable)
	{
	}

	public ULAnimModel()
	{
	}

	public void AddAnimationSetting(string key, ULAnimationSetting setting)
	{
	}

	public bool HasAnimation(string animName)
	{
		return false;
	}

	public AnimationClip AnimClip(string animName)
	{
		return null;
	}

	public AnimationBlendMode AnimBlendMode(string animName)
	{
		return default(AnimationBlendMode);
	}

	public WrapMode AnimWrapMode(string animName)
	{
		return default(WrapMode);
	}

	public PlayMode AnimPlayMode(string animName)
	{
		return default(PlayMode);
	}

	public int AnimLayer(string animName)
	{
		return 0;
	}

	public void ApplyAnimationSettings(Animation targetAnimation)
	{
	}

	public void UnapplyAnimationSettings(Animation targetAnimation)
	{
	}
}
