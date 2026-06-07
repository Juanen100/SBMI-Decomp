using UnityEngine;

public class TBPinchZoom : MonoBehaviour
{
	public enum ZoomMethod
	{
		Position = 0,
		FOV = 1
	}

	public ZoomMethod zoomMethod;

	public float zoomSpeed;

	public float minZoomAmount;

	public float maxZoomAmount;

	private Vector3 defaultPos;

	private float defaultFov;

	private float defaultOrthoSize;

	private float zoomAmount;

	public Vector3 DefaultPos
	{
		get
		{
			return default(Vector3);
		}
		set
		{
		}
	}

	public float DefaultFov
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public float DefaultOrthoSize
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public float ZoomAmount
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	private void Start()
	{
	}

	public void SetDefaults()
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void FingerGestures_OnPinchMove(Vector2 fingerPos1, Vector2 fingerPos2, float delta)
	{
	}
}
