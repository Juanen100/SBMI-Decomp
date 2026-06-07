using System.Collections;
using System.Diagnostics;

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
			return 0f;
		}
		set
		{
		}
	}

	public void AnimatedProgress(float prog, float duration)
	{
	}

	public void ForceAnimatedProgress(float start, float prog, float duration)
	{
	}

	[DebuggerHidden]
	private IEnumerator AnimatedProgressCoroutine(float? start, float target, float duration)
	{
		return null;
	}
}
