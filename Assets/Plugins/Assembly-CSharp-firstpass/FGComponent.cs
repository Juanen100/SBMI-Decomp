using UnityEngine;

public abstract class FGComponent : MonoBehaviour
{
	public delegate void EventDelegate<T>(T source) where T : FGComponent;

	protected virtual void Awake()
	{
	}

	protected virtual void Start()
	{
	}

	protected virtual void OnEnable()
	{
		FingerGestures.OnFingersUpdated += FingerGestures_OnFingersUpdated;
	}

	protected virtual void OnDisable()
	{
		FingerGestures.OnFingersUpdated -= FingerGestures_OnFingersUpdated;
	}

	private void FingerGestures_OnFingersUpdated()
	{
		OnUpdate(FingerGestures.Touches);
	}

	protected abstract void OnUpdate(FingerGestures.IFingerList touches);
}
