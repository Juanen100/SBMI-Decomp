using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class PulserMixin : MonoBehaviour
{
	private static int nextId;

	private Action updateCallback;

	private Action completeCallback;

	private Vector2 restingSize;

	private Vector2 currentSize;

	private float amplitude;

	private float period;

	private readonly object controlLock;

	private int count;

	private bool isLooped;

	private bool isRunning;

	private static TFPool<GameObject> pool;

	public Vector2 Size
	{
		get
		{
			return default(Vector2);
		}
	}

	public Vector2 RestingSize
	{
		get
		{
			return default(Vector2);
		}
	}

	private PulserMixin()
	{
	}

	public static PulserMixin Create()
	{
		return null;
	}

	public void Initialize(Vector2 restingSize, float amplitude, float period)
	{
	}

	public void Initialize(Vector2 restingSize, float amplitude, float period, Action updateCallback, Action completeCallback)
	{
	}

	public void Destroy()
	{
	}

	public void Release()
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

	public void PulseStopLoop(bool hardStop)
	{
	}

	private void StartPulseMachine()
	{
	}

	[DebuggerHidden]
	private IEnumerator PulseMachineRun()
	{
		return null;
	}
}
