using UnityEngine;

public class ScreenAnchor : MonoBehaviour
{
	public bool registerAnchorEvent = true;

	public SpritePivot anchor = SpritePivot.MiddleCenter;

	private GUIView view;

	private void OnEnable()
	{
		if (registerAnchorEvent)
		{
			view = GUIView.GetParentView(base.transform);
			view.RefreshEvent += view.SnapAnchors;
		}
	}

	private void OnDisable()
	{
		if (registerAnchorEvent)
		{
			view.RefreshEvent -= view.SnapAnchors;
		}
	}

	public void SnapAnchor(Camera cam)
	{
		Vector3 zero = Vector3.zero;
		switch (anchor)
		{
		case SpritePivot.LowerLeft:
		case SpritePivot.MiddleLeft:
		case SpritePivot.UpperLeft:
			zero.x = 0f;
			break;
		case SpritePivot.LowerCenter:
		case SpritePivot.MiddleCenter:
		case SpritePivot.UpperCenter:
			zero.x = 0.5f;
			break;
		case SpritePivot.LowerRight:
		case SpritePivot.MiddleRight:
		case SpritePivot.UpperRight:
			zero.x = 1f;
			break;
		}
		switch (anchor)
		{
		case SpritePivot.UpperCenter:
		case SpritePivot.UpperLeft:
		case SpritePivot.UpperRight:
			zero.y = 1f;
			break;
		case SpritePivot.MiddleCenter:
		case SpritePivot.MiddleLeft:
		case SpritePivot.MiddleRight:
			zero.y = 0.5f;
			break;
		case SpritePivot.LowerCenter:
		case SpritePivot.LowerLeft:
		case SpritePivot.LowerRight:
			zero.y = 0f;
			break;
		}
		zero.z = 0f - cam.transform.position.z;
		Vector3 position = cam.ViewportToWorldPoint(zero);
		position.z = cam.transform.position.z;
		base.transform.position = position;
	}
}
