using UnityEngine;

[AddComponentMenu("FingerGestures/Toolbox/Misc/DragOrbit")]
public class TBDragOrbit : MonoBehaviour
{
	public enum PanMode
	{
		Disabled = 0,
		OneFinger = 1,
		TwoFingers = 2
	}

	public Transform target;

	public float initialDistance = 10f;

	public float minDistance = 1f;

	public float maxDistance = 20f;

	public float yawSensitivity = 80f;

	public float pitchSensitivity = 80f;

	public bool clampPitchAngle = true;

	public float minPitch = -20f;

	public float maxPitch = 80f;

	public bool allowPinchZoom = true;

	public float pinchZoomSensitivity = 2f;

	public bool smoothMotion = true;

	public float smoothZoomSpeed = 3f;

	public float smoothOrbitSpeed = 4f;

	public bool allowPanning;

	public bool invertPanningDirections;

	public float panningSensitivity = 1f;

	public Transform panningPlane;

	public bool smoothPanning = true;

	public float smoothPanningSpeed = 8f;

	private float lastPanTime;

	private float distance = 10f;

	private float yaw;

	private float pitch;

	private float idealDistance;

	private float idealYaw;

	private float idealPitch;

	private Vector3 idealPanOffset = Vector3.zero;

	private Vector3 panOffset = Vector3.zero;

	public float Distance
	{
		get
		{
			return distance;
		}
	}

	public float IdealDistance
	{
		get
		{
			return idealDistance;
		}
		set
		{
			idealDistance = Mathf.Clamp(value, minDistance, maxDistance);
		}
	}

	public float Yaw
	{
		get
		{
			return yaw;
		}
	}

	public float IdealYaw
	{
		get
		{
			return idealYaw;
		}
		set
		{
			idealYaw = value;
		}
	}

	public float Pitch
	{
		get
		{
			return pitch;
		}
	}

	public float IdealPitch
	{
		get
		{
			return idealPitch;
		}
		set
		{
			idealPitch = ((!clampPitchAngle) ? value : ClampAngle(value, minPitch, maxPitch));
		}
	}

	public Vector3 IdealPanOffset
	{
		get
		{
			return idealPanOffset;
		}
		set
		{
			idealPanOffset = value;
		}
	}

	public Vector3 PanOffset
	{
		get
		{
			return panOffset;
		}
	}

	private void Start()
	{
		if (!panningPlane)
		{
			panningPlane = base.transform;
		}
		Vector3 eulerAngles = base.transform.eulerAngles;
		float num = (IdealDistance = initialDistance);
		distance = num;
		num = (IdealYaw = eulerAngles.y);
		yaw = num;
		num = (IdealPitch = eulerAngles.x);
		pitch = num;
		if ((bool)base.GetComponent<Rigidbody>())
		{
			base.GetComponent<Rigidbody>().freezeRotation = true;
		}
		Apply();
	}

	private void OnEnable()
	{
		FingerGestures.OnDragMove += FingerGestures_OnDragMove;
		FingerGestures.OnPinchMove += FingerGestures_OnPinchMove;
		FingerGestures.OnTwoFingerDragMove += FingerGestures_OnTwoFingerDragMove;
	}

	private void OnDisable()
	{
		FingerGestures.OnDragMove -= FingerGestures_OnDragMove;
		FingerGestures.OnPinchMove -= FingerGestures_OnPinchMove;
		FingerGestures.OnTwoFingerDragMove -= FingerGestures_OnTwoFingerDragMove;
	}

	private void FingerGestures_OnDragMove(Vector2 fingerPos, Vector2 delta)
	{
		if (!(Time.time - lastPanTime < 0.25f) && (bool)target)
		{
			IdealYaw += delta.x * yawSensitivity * 0.02f;
			IdealPitch -= delta.y * pitchSensitivity * 0.02f;
		}
	}

	private void FingerGestures_OnPinchMove(Vector2 fingerPos1, Vector2 fingerPos2, float delta)
	{
		if (allowPinchZoom)
		{
			IdealDistance -= delta * pinchZoomSensitivity;
		}
	}

	private void FingerGestures_OnTwoFingerDragMove(Vector2 fingerPos, Vector2 delta)
	{
		if (allowPanning)
		{
			Vector3 vector = -0.02f * panningSensitivity * (panningPlane.right * delta.x + panningPlane.up * delta.y);
			if (invertPanningDirections)
			{
				IdealPanOffset -= vector;
			}
			else
			{
				IdealPanOffset += vector;
			}
			lastPanTime = Time.time;
		}
	}

	private void Apply()
	{
		if (smoothMotion)
		{
			distance = Mathf.Lerp(distance, IdealDistance, Time.deltaTime * smoothZoomSpeed);
			yaw = Mathf.Lerp(yaw, IdealYaw, Time.deltaTime * smoothOrbitSpeed);
			pitch = Mathf.Lerp(pitch, IdealPitch, Time.deltaTime * smoothOrbitSpeed);
		}
		else
		{
			distance = IdealDistance;
			yaw = IdealYaw;
			pitch = IdealPitch;
		}
		if (smoothPanning)
		{
			panOffset = Vector3.Lerp(panOffset, idealPanOffset, Time.deltaTime * smoothPanningSpeed);
		}
		else
		{
			panOffset = idealPanOffset;
		}
		base.transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
		base.transform.position = target.position + panOffset - distance * base.transform.forward;
	}

	private void LateUpdate()
	{
		Apply();
	}

	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}

	public void ResetPanning()
	{
		IdealPanOffset = Vector3.zero;
	}
}
