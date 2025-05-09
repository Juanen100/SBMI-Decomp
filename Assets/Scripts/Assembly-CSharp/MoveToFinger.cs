using UnityEngine;

public class MoveToFinger : MonoBehaviour
{
	private void OnEnable()
	{
		FingerGestures.OnFingerDown += OnFingerDown;
	}

	private void OnDisable()
	{
		FingerGestures.OnFingerDown -= OnFingerDown;
	}

	private void OnFingerDown(int fingerIndex, Vector2 fingerPos)
	{
		base.transform.position = GetWorldPos(fingerPos);
	}

	private Vector3 GetWorldPos(Vector2 screenPos)
	{
		Camera main = Camera.main;
		return main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Mathf.Abs(base.transform.position.z - main.transform.position.z)));
	}
}
