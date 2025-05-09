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
			return (!(mixin == null)) ? mixin.Size : Vector2.one;
		}
	}

	public DeferredPulser(Vector2 restingSize, float amplitude, float period, Action onUpdateCallback, Action onCompleteCallback)
	{
		this.restingSize = restingSize;
		this.amplitude = amplitude;
		this.period = period;
		this.onUpdateCallback = onUpdateCallback;
		this.onCompleteCallback = onCompleteCallback;
	}

	public void PulseOneShot()
	{
		PulseOneShot(1);
	}

	public void PulseOneShot(int count)
	{
		if (mixin == null)
		{
			mixin = Create();
		}
		mixin.PulseOneShot(count);
	}

	public void PulseStartLoop()
	{
		if (mixin == null)
		{
			mixin = Create();
		}
		mixin.PulseStartLoop();
	}

	public void PulseStopLoop()
	{
		if (mixin != null)
		{
			mixin.PulseStopLoop();
		}
	}

	public void Destroy()
	{
		if (mixin != null)
		{
			mixin.Release();
			mixin = null;
		}
	}

	private PulserMixin Create()
	{
		PulserMixin pulserMixin = PulserMixin.Create();
		pulserMixin.Initialize(restingSize, amplitude, period, onUpdateCallback, onCompleteCallback);
		return pulserMixin;
	}
}
