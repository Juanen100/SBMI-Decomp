using System;
using UnityEngine;

public class DeferredPulser
{
	private Vector2 restingSize;

	private float amplitude;

	private float period;

	private Action onUpdateCallback;

	private Action onCompleteCallback;

	private PulserMixin mixin;

	public Vector2 Size
	{
		get
		{
			return default(Vector2);
		}
	}

	public DeferredPulser(Vector2 restingSize, float amplitude, float period, Action onUpdateCallback, Action onCompleteCallback)
	{
	}

	public void PulseOneShot()
	{
	}

	public void PulseOneShot(int count)
	{
	}

	public void PulseStartLoop()
	{
	}

	public void PulseStopLoop()
	{
	}

	public void Destroy()
	{
	}

	private PulserMixin Create()
	{
		return null;
	}
}
