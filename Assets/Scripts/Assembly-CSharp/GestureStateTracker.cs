using UnityEngine;

public class GestureStateTracker : MonoBehaviour
{
	public GestureRecognizer gesture;

	private void Awake()
	{
		if (!gesture)
		{
			gesture = GetComponent<GestureRecognizer>();
		}
	}

	private void OnEnable()
	{
		if ((bool)gesture)
		{
			gesture.OnStateChanged += gesture_OnStateChanged;
		}
	}

	private void OnDisable()
	{
		if ((bool)gesture)
		{
			gesture.OnStateChanged -= gesture_OnStateChanged;
		}
	}

	private void gesture_OnStateChanged(GestureRecognizer source)
	{
		Debug.Log(string.Concat("Gesture ", source, " changed from ", source.PreviousState, " to ", source.State));
	}
}
