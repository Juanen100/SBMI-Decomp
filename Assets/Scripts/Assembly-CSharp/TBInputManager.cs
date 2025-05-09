using UnityEngine;

[AddComponentMenu("FingerGestures/Toolbox/Input Manager")]
public class TBInputManager : MonoBehaviour
{
	public enum DragPlaneType
	{
		XY = 0,
		XZ = 1,
		ZY = 2,
		UseCollider = 3,
		Camera = 4
	}

	public bool trackFingerUp = true;

	public bool trackFingerDown = true;

	public bool trackDrag = true;

	public bool trackTap = true;

	public bool trackLongPress = true;

	public bool trackSwipe = true;

	public Camera raycastCamera;

	public LayerMask ignoreLayers = 0;

	public DragPlaneType dragPlaneType = DragPlaneType.Camera;

	public Collider dragPlaneCollider;

	public float dragPlaneOffset;

	private void Start()
	{
		if (!raycastCamera)
		{
			raycastCamera = Camera.main;
		}
	}

	private void OnEnable()
	{
		if (trackFingerDown)
		{
			FingerGestures.OnFingerDown += FingerGestures_OnFingerDown;
		}
		if (trackFingerUp)
		{
			FingerGestures.OnFingerUp += FingerGestures_OnFingerUp;
		}
		if (trackDrag)
		{
			FingerGestures.OnFingerDragBegin += FingerGestures_OnFingerDragBegin;
		}
		if (trackTap)
		{
			FingerGestures.OnFingerTap += FingerGestures_OnFingerTap;
			FingerGestures.OnFingerDoubleTap += FingerGestures_OnFingerDoubleTap;
		}
		if (trackLongPress)
		{
			FingerGestures.OnFingerLongPress += FingerGestures_OnFingerLongPress;
		}
		if (trackSwipe)
		{
			FingerGestures.OnFingerSwipe += FingerGestures_OnFingerSwipe;
		}
	}

	private void OnDisable()
	{
		FingerGestures.OnFingerDown -= FingerGestures_OnFingerDown;
		FingerGestures.OnFingerUp -= FingerGestures_OnFingerUp;
		FingerGestures.OnFingerDragBegin -= FingerGestures_OnFingerDragBegin;
		FingerGestures.OnFingerTap -= FingerGestures_OnFingerTap;
		FingerGestures.OnFingerDoubleTap -= FingerGestures_OnFingerDoubleTap;
		FingerGestures.OnFingerLongPress -= FingerGestures_OnFingerLongPress;
		FingerGestures.OnFingerSwipe -= FingerGestures_OnFingerSwipe;
	}

	private void FingerGestures_OnFingerUp(int fingerIndex, Vector2 fingerPos, float timeHeldDown)
	{
		TBFingerUp tBFingerUp = PickComponent<TBFingerUp>(fingerPos);
		if ((bool)tBFingerUp && tBFingerUp.enabled)
		{
			tBFingerUp.RaiseFingerUp(fingerIndex, fingerPos, timeHeldDown);
		}
	}

	private void FingerGestures_OnFingerDown(int fingerIndex, Vector2 fingerPos)
	{
		TBFingerDown tBFingerDown = PickComponent<TBFingerDown>(fingerPos);
		if ((bool)tBFingerDown && tBFingerDown.enabled)
		{
			tBFingerDown.RaiseFingerDown(fingerIndex, fingerPos);
		}
	}

	private void FingerGestures_OnFingerDragBegin(int fingerIndex, Vector2 fingerPos, Vector2 startPos)
	{
		TBDrag tBDrag = PickComponent<TBDrag>(startPos);
		if ((bool)tBDrag && tBDrag.enabled && !tBDrag.Dragging)
		{
			tBDrag.BeginDrag(fingerIndex, fingerPos);
			tBDrag.OnDragMove += draggable_OnDragMove;
			tBDrag.OnDragEnd += draggable_OnDragEnd;
		}
	}

	public bool ProjectScreenPointOnDragPlane(Vector3 refPos, Vector2 screenPos, out Vector3 worldPos)
	{
		worldPos = refPos;
		switch (dragPlaneType)
		{
		case DragPlaneType.XY:
			worldPos = raycastCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Mathf.Abs(refPos.z - raycastCamera.transform.position.z)));
			return true;
		case DragPlaneType.XZ:
			worldPos = raycastCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Mathf.Abs(refPos.y - raycastCamera.transform.position.y)));
			return true;
		case DragPlaneType.ZY:
			worldPos = raycastCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Mathf.Abs(refPos.x - raycastCamera.transform.position.x)));
			return true;
		case DragPlaneType.UseCollider:
		{
			Ray ray2 = raycastCamera.ScreenPointToRay(screenPos);
			RaycastHit hitInfo;
			if (!dragPlaneCollider.Raycast(ray2, out hitInfo, float.MaxValue))
			{
				return false;
			}
			worldPos = hitInfo.point + dragPlaneOffset * hitInfo.normal;
			return true;
		}
		case DragPlaneType.Camera:
		{
			Transform transform = raycastCamera.transform;
			Plane plane = new Plane(-transform.forward, refPos);
			Ray ray = raycastCamera.ScreenPointToRay(screenPos);
			float enter = 0f;
			if (!plane.Raycast(ray, out enter))
			{
				return false;
			}
			worldPos = ray.GetPoint(enter);
			return true;
		}
		default:
			return false;
		}
	}

	private void draggable_OnDragMove(TBDrag sender)
	{
		Vector2 screenPos = sender.FingerPos - sender.MoveDelta;
		Vector3 worldPos;
		Vector3 worldPos2;
		if (ProjectScreenPointOnDragPlane(sender.transform.position, screenPos, out worldPos) && ProjectScreenPointOnDragPlane(sender.transform.position, sender.FingerPos, out worldPos2))
		{
			Vector3 vector = worldPos2 - worldPos;
			sender.transform.position += vector;
		}
	}

	private void draggable_OnDragEnd(TBDrag source)
	{
		source.OnDragMove -= draggable_OnDragMove;
		source.OnDragEnd -= draggable_OnDragEnd;
	}

	private void FingerGestures_OnFingerTap(int fingerIndex, Vector2 fingerPos)
	{
		TBTap tBTap = PickComponent<TBTap>(fingerPos);
		if ((bool)tBTap && tBTap.enabled && tBTap.tapMode == TBTap.TapMode.SingleTap)
		{
			tBTap.RaiseTap(fingerIndex, fingerPos);
		}
	}

	private void FingerGestures_OnFingerDoubleTap(int fingerIndex, Vector2 fingerPos)
	{
		TBTap tBTap = PickComponent<TBTap>(fingerPos);
		if ((bool)tBTap && tBTap.enabled && tBTap.tapMode == TBTap.TapMode.DoubleTap)
		{
			tBTap.RaiseTap(fingerIndex, fingerPos);
		}
	}

	private void FingerGestures_OnFingerLongPress(int fingerIndex, Vector2 fingerPos)
	{
		TBLongPress tBLongPress = PickComponent<TBLongPress>(fingerPos);
		if ((bool)tBLongPress && tBLongPress.enabled)
		{
			tBLongPress.RaiseLongPress(fingerIndex, fingerPos);
		}
	}

	private void FingerGestures_OnFingerSwipe(int fingerIndex, Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity)
	{
		TBSwipe tBSwipe = PickComponent<TBSwipe>(startPos);
		if ((bool)tBSwipe && tBSwipe.enabled)
		{
			tBSwipe.RaiseSwipe(fingerIndex, startPos, direction, velocity);
		}
	}

	public GameObject PickObject(Vector2 screenPos)
	{
		Ray ray = raycastCamera.ScreenPointToRay(screenPos);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, float.MaxValue, ~(int)ignoreLayers))
		{
			return hitInfo.collider.gameObject;
		}
		return null;
	}

	public T PickComponent<T>(Vector2 screenPos) where T : TBComponent
	{
		GameObject gameObject = PickObject(screenPos);
		if (!gameObject)
		{
			return (T)null;
		}
		return gameObject.GetComponent<T>();
	}
}
