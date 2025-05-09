#define ASSERTS_ON
using System;
using System.Collections;
using UnityEngine;

public class PulserMixin : MonoBehaviour
{
	private static int nextId = 0;

	private Action updateCallback;

	private Action completeCallback;

	private Vector2 restingSize;

	private Vector2 currentSize;

	private float amplitude;

	private float period;

	private readonly object controlLock = new object();

	private int count;

	private bool isLooped;

	private bool isRunning;

	private static TFPool<GameObject> pool = new TFPool<GameObject>();

	public Vector2 Size
	{
		get
		{
			return currentSize;
		}
	}

	public Vector2 RestingSize
	{
		get
		{
			return restingSize;
		}
	}

	private PulserMixin()
	{
	}

	public static PulserMixin Create()
	{
		return pool.Create(delegate
		{
			GameObject gameObject = new GameObject("PulserMixin_" + nextId++);
			PulserMixin pulserMixin = gameObject.AddComponent<PulserMixin>();
			pulserMixin.currentSize = pulserMixin.restingSize;
			return gameObject;
		}).GetComponent<PulserMixin>();
	}

	public void Initialize(Vector2 restingSize, float amplitude, float period)
	{
		Initialize(restingSize, amplitude, period, null, null);
	}

	public void Initialize(Vector2 restingSize, float amplitude, float period, Action updateCallback, Action completeCallback)
	{
		TFUtils.Assert(restingSize.x >= 0f && restingSize.y >= 0f, "Should only pulse to non-negative scales");
		this.restingSize = restingSize;
		this.amplitude = amplitude;
		this.period = period;
		this.updateCallback = updateCallback;
		this.completeCallback = completeCallback;
	}

	public void Destroy()
	{
		if (!(this == null))
		{
			StopAllCoroutines();
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void Release()
	{
		if (!(this == null))
		{
			StopAllCoroutines();
			count = 0;
			isRunning = false;
			currentSize = restingSize;
			pool.Release(base.gameObject);
		}
	}

	public void PulseOneShot()
	{
		PulseOneShot(1);
	}

	public void PulseOneShot(int count)
	{
		lock (controlLock)
		{
			this.count += count;
			isLooped = false;
			StartPulseMachine();
		}
	}

	public void PulseStartLoop()
	{
		lock (controlLock)
		{
			count = 0;
			isLooped = true;
			StartPulseMachine();
		}
	}

	public void PulseStopLoop()
	{
		PulseStopLoop(false);
	}

	public void PulseStopLoop(bool hardStop)
	{
		lock (controlLock)
		{
			isLooped = false;
			count = 0;
			if (hardStop)
			{
				isRunning = false;
			}
		}
	}

	private void StartPulseMachine()
	{
		if (!isRunning)
		{
			isRunning = true;
			StartCoroutine(PulseMachineRun());
		}
	}

	private IEnumerator PulseMachineRun()
	{
		PeriodicPattern pattern = new Sinusoid(1f, amplitude, period, 0f);
		while (isRunning)
		{
			float t = 0f;
			while (isRunning && t <= period)
			{
				float v = pattern.ValueAtTime(t);
				t += Time.deltaTime;
				currentSize = restingSize * v;
				if (updateCallback != null)
				{
					updateCallback();
				}
				yield return null;
			}
			lock (controlLock)
			{
				count--;
				if (!isLooped && count <= 0)
				{
					isRunning = false;
					break;
				}
			}
		}
		currentSize = restingSize;
		if (updateCallback != null)
		{
			updateCallback();
		}
		if (completeCallback != null)
		{
			completeCallback();
		}
	}
}
