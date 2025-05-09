#define ASSERTS_ON
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(YGFrameAtlasSprite))]
public class SBGUIProgressMeter : SBGUIAtlasImage
{
	public SBGUIAtlasImage meter;

	public SBGUIAtlasImage fill;

	private float progress;

	private float targetProgress;

	public bool running { get; private set; }

	public float Progress
	{
		get
		{
			return progress;
		}
		set
		{
			if (meter == null || fill == null)
			{
				TFUtils.Assert(false, string.Format("Progress meter '{0}' is missing background or fill sprite", base.gameObject.name));
				return;
			}
			value = Mathf.Clamp01(value);
			if (progress != value)
			{
				Vector2 size = fill.Size;
				size.x = meter.Size.x * value;
				fill.Size = size;
				progress = value;
			}
		}
	}

	public void AnimatedProgress(float prog, float duration)
	{
		prog = Mathf.Clamp01(prog);
		if (targetProgress != prog)
		{
			targetProgress = prog;
			StartCoroutine(AnimatedProgressCoroutine(null, prog, duration));
		}
	}

	public void ForceAnimatedProgress(float start, float prog, float duration)
	{
		prog = Mathf.Clamp01(prog);
		if (targetProgress != prog)
		{
			targetProgress = prog;
			StartCoroutine(AnimatedProgressCoroutine(start, prog, duration));
		}
	}

	private IEnumerator AnimatedProgressCoroutine(float? start, float target, float duration)
	{
		running = true;
		if (!start.HasValue)
		{
			start = progress;
		}
		float elapsed = 0f;
		while (elapsed < duration)
		{
			if (targetProgress != target)
			{
				yield break;
			}
			elapsed += Time.deltaTime;
			Progress = Mathf.Lerp(start.Value, target, elapsed / duration);
			yield return null;
		}
		running = false;
	}
}
