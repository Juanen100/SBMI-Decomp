using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class ULAnimController : ULAnimControllerInterface
{
	protected bool enabled;

	protected Animation animation;

	protected ULAnimModelInterface animationModel;

	public Animation UnityAnimation
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public ULAnimModelInterface AnimationModel
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	[DebuggerHidden]
	public static IEnumerator PlaySequence(Animation tgtAnimation, string[] sequence)
	{
		return null;
	}

	[DebuggerHidden]
	public static IEnumerator PlayRandom(Animation tgtAnimation, string[] domain)
	{
		return null;
	}

	public bool HasAnimation(string animationName)
	{
		return false;
	}

	public bool AnimationEnabled()
	{
		return false;
	}

	public void EnableAnimation(bool toEnabled)
	{
	}

	public void PlayAnimation(string animationName)
	{
	}

	public void StopAnimation(string animationName)
	{
	}

	public void StopAnimations()
	{
	}

	public void Sample(string animationName, float time)
	{
	}

	public void SampleWithNormalizedTime(string animationName, float normalizedTime)
	{
	}

	public float GetFrameRate(string animationName)
	{
		return 0f;
	}

	public float GetLength(string animationName)
	{
		return 0f;
	}

	public float NormalizedTimePerFrame(string animationName)
	{
		return 0f;
	}
}
