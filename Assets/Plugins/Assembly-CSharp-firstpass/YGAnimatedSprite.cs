using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

[ExecuteInEditMode]
public class YGAnimatedSprite : YGSprite
{
	[Serializable]
	public class FrameLayout
	{
		public Vector2 size;

		public Vector2 layout;

		public int count;
	}

	public FrameLayout frameLayout;

	public int framesPerSecond;

	public bool startAutomatically;

	protected int currentFrame;

	protected Rect[] frames;

	public WrapMode wrapMode;

	private float sleep;

	private Func<IEnumerator> animFunc;

	public bool IsPlaying { get; protected set; }

	protected override void OnEnable()
	{
	}

	protected override void OnDisable()
	{
	}

	private void Start()
	{
	}

	public void StartAnimation()
	{
	}

	public void StopAnimation()
	{
	}

	[DebuggerHidden]
	private IEnumerator PlayForward(int startFrame)
	{
		return null;
	}

	[DebuggerHidden]
	private IEnumerator PlayBackward(int startFrame)
	{
		return null;
	}

	[DebuggerHidden]
	private IEnumerator AnimateDefault()
	{
		return null;
	}

	[DebuggerHidden]
	private IEnumerator AnimateClamp()
	{
		return null;
	}

	[DebuggerHidden]
	private IEnumerator AnimateLoop()
	{
		return null;
	}

	[DebuggerHidden]
	private IEnumerator AnimatePingPong()
	{
		return null;
	}

	public override void Load()
	{
	}

	public override void AssembleMesh()
	{
	}

	public Vector2[] FrameUVs(int frame)
	{
		return null;
	}

	protected void SetFrame(int frame)
	{
	}
}
