using UnityEngine;

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

	public bool trackFingerUp;

	public bool trackFingerDown;

	public bool trackDrag;

	public bool trackTap;

	public bool trackLongPress;

	public bool trackSwipe;

	public Camera raycastCamera;

	public LayerMask ignoreLayers;

	public DragPlaneType dragPlaneType;

	public Collider dragPlaneCollider;

	public float dragPlaneOffset;

	private void Start()
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void FingerGestures_OnFingerUp(int fingerIndex, Vector2 fingerPos, float timeHeldDown)
	{
	}

	private void FingerGestures_OnFingerDown(int fingerIndex, Vector2 fingerPos)
	{
	}

	private void FingerGestures_OnFingerDragBegin(int fingerIndex, Vector2 fingerPos, Vector2 startPos)
	{
	}

	public bool ProjectScreenPointOnDragPlane(Vector3 refPos, Vector2 screenPos, out Vector3 worldPos)
	{
		worldPos = default(Vector3);
		return false;
	}

	private void draggable_OnDragMove(TBDrag sender)
	{
	}

	private void draggable_OnDragEnd(TBDrag source)
	{
	}

	private void FingerGestures_OnFingerTap(int fingerIndex, Vector2 fingerPos)
	{
	}

	private void FingerGestures_OnFingerDoubleTap(int fingerIndex, Vector2 fingerPos)
	{
	}

	private void FingerGestures_OnFingerLongPress(int fingerIndex, Vector2 fingerPos)
	{
	}

	private void FingerGestures_OnFingerSwipe(int fingerIndex, Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity)
	{
	}

	public GameObject PickObject(Vector2 screenPos)
	{
		return null;
	}

	public T PickComponent<T>(Vector2 screenPos) where T : TBComponent
	{
		return null;
	}
}
