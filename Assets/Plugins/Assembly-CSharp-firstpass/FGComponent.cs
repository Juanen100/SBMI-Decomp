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
	}

	protected virtual void OnDisable()
	{
	}

	private void FingerGestures_OnFingersUpdated()
	{
	}

	protected abstract void OnUpdate(FingerGestures.IFingerList touches);
}
