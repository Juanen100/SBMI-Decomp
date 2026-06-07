using UnityEngine;

public class TBDragOrbit : MonoBehaviour
{
	public enum PanMode
	{
		Disabled = 0,
		OneFinger = 1,
		TwoFingers = 2
	}

	public Transform target;

	public float initialDistance;

	public float minDistance;

	public float maxDistance;

	public float yawSensitivity;

	public float pitchSensitivity;

	public bool clampPitchAngle;

	public float minPitch;

	public float maxPitch;

	public bool allowPinchZoom;

	public float pinchZoomSensitivity;

	public bool smoothMotion;

	public float smoothZoomSpeed;

	public float smoothOrbitSpeed;

	public bool allowPanning;

	public bool invertPanningDirections;

	public float panningSensitivity;

	public Transform panningPlane;

	public bool smoothPanning;

	public float smoothPanningSpeed;

	private float lastPanTime;

	private float distance;

	private float yaw;

	private float pitch;

	private float idealDistance;

	private float idealYaw;

	private float idealPitch;

	private Vector3 idealPanOffset;

	private Vector3 panOffset;

	public float Distance
	{
		get
		{
			return 0f;
		}
	}

	public float IdealDistance
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public float Yaw
	{
		get
		{
			return 0f;
		}
	}

	public float IdealYaw
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public float Pitch
	{
		get
		{
			return 0f;
		}
	}

	public float IdealPitch
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public Vector3 IdealPanOffset
	{
		get
		{
			return default(Vector3);
		}
		set
		{
		}
	}

	public Vector3 PanOffset
	{
		get
		{
			return default(Vector3);
		}
	}

	private void Start()
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void FingerGestures_OnDragMove(Vector2 fingerPos, Vector2 delta)
	{
	}

	private void FingerGestures_OnPinchMove(Vector2 fingerPos1, Vector2 fingerPos2, float delta)
	{
	}

	private void FingerGestures_OnTwoFingerDragMove(Vector2 fingerPos, Vector2 delta)
	{
	}

	private void Apply()
	{
	}

	private void LateUpdate()
	{
	}

	private static float ClampAngle(float angle, float min, float max)
	{
		return 0f;
	}

	public void ResetPanning()
	{
	}
}
