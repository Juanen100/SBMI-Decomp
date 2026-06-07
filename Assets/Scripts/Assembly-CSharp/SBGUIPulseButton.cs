using System;
using UnityEngine;

public class SBGUIPulseButton : SBGUIAtlasButton, IPulsable
{
	public Vector2 RestingSize;

	public float Amplitude;

	public float Period;

	private DeferredPulser pulser;

	public DeferredPulser Pulser
	{
		get
		{
			return null;
		}
	}

	private SBGUIPulseButton()
	{
	}

	protected override void Awake()
	{
	}

	public static SBGUIPulseButton Create(SBGUIElement parent, string asset, Vector2 restingSize, float amplitude, float period, Action OnCompleteCallback)
	{
		return null;
	}

	public void InitializePulser(Vector2 restingSize, float amplitude, float period)
	{
	}

	public void InitializePulser(Vector2 restingSize, float amplitude, float period, Action OnCompleteCallback)
	{
	}

	public override void OnDestroy()
	{
	}

	private void OnPulserUpdate()
	{
	}
}
