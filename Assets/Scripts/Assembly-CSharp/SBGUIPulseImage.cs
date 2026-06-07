using System;
using UnityEngine;

public class SBGUIPulseImage : SBGUIAtlasImage, IPulsable
{
	private DeferredPulser pulser;

	public DeferredPulser Pulser
	{
		get
		{
			return null;
		}
	}

	private SBGUIPulseImage()
	{
	}

	public static SBGUIPulseImage Create(SBGUIElement parent, string asset, Vector2 restingSize, float amplitude, float period, Action OnCompleteCallback)
	{
		return null;
	}

	public void InitializePulser(Vector2 restingSize, float amplitude, float period)
	{
	}

	public void InitializePulser(Vector2 restingSize, float amplitude, float period, Action OnCompleteCallback)
	{
	}

	public void Destroy()
	{
	}

	private void OnPulserUpdate()
	{
	}
}
