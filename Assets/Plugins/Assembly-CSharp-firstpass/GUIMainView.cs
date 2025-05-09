using System.Collections.Generic;
using UnityEngine;
using Yarg;

[RequireComponent(typeof(YG2DWorld))]
[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(YGTextureLibrary))]
public class GUIMainView : GUIView
{
	public const float DESKTOP_DPI_GUESS = 110f;

	private const float FINGER_DRAG_RADIUS_INCHES = 0.5f;

	public Vector2 defaultResolution = new Vector2(800f, 600f);

	private static GUIMainView instance;

	public EventDispatcher<YGEvent> FinalEventListener = new EventDispatcher<YGEvent>();

	private bool pauseFinalEventListener;

	private List<GUISubView> subViews = new List<GUISubView>();

	private static float FINGER_DRAG_RADIUS_SQR;

	private int? currentFinger;

	protected Vector2? startPosition;

	public static float EffectiveDPI
	{
		get
		{
			float num = ((Screen.dpi != 0f) ? Screen.dpi : 110f);
			if (Screen.height < 800 || Screen.width < 800)
			{
				num *= 0.5f;
			}
			return num;
		}
	}

	public void ClearFinalEventListener()
	{
		FinalEventListener.ClearListeners();
		pauseFinalEventListener = false;
	}

	public void PauseFinalEventListener(bool pause)
	{
		pauseFinalEventListener = pause;
	}

	public static GUIMainView GetInstance()
	{
		if (instance == null && Application.isPlaying)
		{
			SetInstance((GUIMainView)Object.FindObjectOfType(typeof(GUIMainView)));
			if (instance == null)
			{
				Debug.LogWarning("No GUIMainView in scene, creating one");
				GameObject gameObject = new GameObject("__GUIMainView__");
				SetInstance(gameObject.AddComponent<GUIMainView>());
			}
		}
		return instance;
	}

	private static bool SetInstance(GUIMainView inst)
	{
		if (instance != null && instance != inst)
		{
			return false;
		}
		instance = inst;
		return true;
	}

	protected override void OnEnable()
	{
		if (!SetInstance(this))
		{
			Object.DestroyImmediate(this);
			return;
		}
		base.useGUILayout = false;
		RegisterFingers(true);
		base.OnEnable();
	}

	protected override void OnDisable()
	{
		RegisterFingers(false);
		base.OnDisable();
	}

	protected override void Start()
	{
		FINGER_DRAG_RADIUS_SQR = Mathf.Pow(0.5f * EffectiveDPI, 2f);
		base.Start();
	}

	protected override void ResizePortal()
	{
		pixelScale = (float)Screen.height * 0.01f;
		base.ResizePortal();
	}

	public Bounds ViewBounds()
	{
		Camera cam = base.Cam;
		Vector3 min = cam.ViewportToWorldPoint(new Vector3(0f, 0f, cam.nearClipPlane));
		Vector3 max = cam.ViewportToWorldPoint(new Vector3(1f, 1f, cam.farClipPlane));
		Bounds result = new Bounds(Vector3.zero, Vector3.zero);
		result.SetMinMax(min, max);
		return result;
	}

	public GUISubView CreateSubView()
	{
		GUISubView gUISubView = GUISubView.Create(base.transform);
		Vector3 position = base.transform.position;
		position.z += base.Cam.farClipPlane + 1f;
		float num = base.Cam.depth + 1f;
		foreach (GUISubView subView in subViews)
		{
			position.z += subView.Cam.farClipPlane + 1f;
			num += 1f;
		}
		gUISubView.Cam.depth = num;
		gUISubView.transform.position = position;
		AddSubView(gUISubView);
		return gUISubView;
	}

	public bool AddSubView(GUISubView sub)
	{
		if (subViews.Contains(sub))
		{
			return false;
		}
		subViews.Add(sub);
		return true;
	}

	public bool RemoveSubView(GUISubView sub)
	{
		activeTargetSet.Remove(sub);
		targets.Remove(sub);
		return subViews.Remove(sub);
	}

	protected override List<ITouchable> RayHit(Vector2 pos)
	{
		targets = base.RayHit(pos);
		if (subViews != null)
		{
			foreach (GUISubView subView in subViews)
			{
				if (subView.ContainsPoint(ScreenToWorld(pos)))
				{
					targets.Add(subView);
				}
			}
		}
		return targets;
	}

	private void RegisterFingers(bool active)
	{
		if (active)
		{
			FingerGestures.OnTap += Tap;
			FingerGestures.OnFingerDown += FingerDown;
			FingerGestures.OnFingerUp += FingerUp;
			FingerGestures.OnPinchBegin += PinchBegin;
			FingerGestures.OnPinchMove += PinchMove;
			FingerGestures.OnPinchEnd += PinchEnd;
			FingerGestures.OnLongPress += LongPress;
			FingerGestures.OnDragBegin += DragBegin;
			FingerGestures.OnDragEnd += DragEnd;
			FingerGestures.OnDragMove += DragMove;
			FingerGestures.OnDragStationary += DragStationary;
		}
		else
		{
			FingerGestures.OnTap -= Tap;
			FingerGestures.OnFingerDown -= FingerDown;
			FingerGestures.OnFingerUp -= FingerUp;
			FingerGestures.OnPinchBegin -= PinchBegin;
			FingerGestures.OnPinchMove -= PinchMove;
			FingerGestures.OnPinchEnd -= PinchEnd;
			FingerGestures.OnLongPress -= LongPress;
			FingerGestures.OnDragBegin -= DragBegin;
			FingerGestures.OnDragEnd -= DragEnd;
			FingerGestures.OnDragMove -= DragMove;
			FingerGestures.OnDragStationary -= DragStationary;
		}
	}

	private void FingerUp(int fingerIndex, Vector2 pos, float time)
	{
		if (currentFinger.HasValue && currentFinger.Value == fingerIndex)
		{
			BroadcastEvent(pos, YGEvent.TYPE.TOUCH_END);
			currentFinger = null;
		}
	}

	private void FingerDown(int fingerIndex, Vector2 pos)
	{
		int? num = currentFinger;
		if (!num.HasValue)
		{
			currentFinger = fingerIndex;
			BroadcastEvent(pos, YGEvent.TYPE.TOUCH_BEGIN);
		}
	}

	private void Tap(Vector2 pos)
	{
		startPosition = null;
		currentFinger = null;
		BroadcastEvent(pos, YGEvent.TYPE.TAP);
	}

	private void PinchBegin(Vector2 pos1, Vector2 pos2)
	{
		BroadcastEvent(pos1, YGEvent.TYPE.RESET);
		BroadcastEvent(pos2, YGEvent.TYPE.RESET);
		startPosition = null;
		currentFinger = null;
		PinchMove(pos1, pos2, 0f);
	}

	private void PinchEnd(Vector2 pos1, Vector2 pos2)
	{
		YGEvent yGEvent = new YGEvent();
		yGEvent.type = YGEvent.TYPE.TOUCH_END;
		yGEvent.startPosition = pos1;
		yGEvent.position = pos2;
		yGEvent.deltaPosition = pos1 - pos2;
		if (yGEvent != null && !yGEvent.used && !pauseFinalEventListener)
		{
			FinalEventListener.FireEvent(yGEvent);
		}
	}

	private void PinchMove(Vector2 pos1, Vector2 pos2, float delta)
	{
		YGEvent yGEvent = new YGEvent();
		yGEvent.type = YGEvent.TYPE.PINCH;
		yGEvent.startPosition = pos1;
		yGEvent.position = pos2;
		yGEvent.deltaPosition = yGEvent.position - yGEvent.startPosition;
		yGEvent.distance = delta;
		if (yGEvent != null && !yGEvent.used && !pauseFinalEventListener)
		{
			FinalEventListener.FireEvent(yGEvent);
		}
	}

	private void LongPress(Vector2 pos)
	{
		BroadcastEvent(pos, YGEvent.TYPE.HOLD);
	}

	private void DragBegin(Vector2 pos, Vector2 startPos)
	{
		startPosition = startPos;
	}

	private void BroadcastEvent(Vector2 pos, YGEvent.TYPE type, Vector2? delta = null)
	{
		List<ITouchable> list = RayHit(pos);
		YGEvent yGEvent = new YGEvent();
		yGEvent.type = type;
		yGEvent.position = (yGEvent.startPosition = pos);
		if (delta.HasValue)
		{
			yGEvent.deltaPosition = delta.Value;
		}
		yGEvent = UpdateAndSendEvent(yGEvent, list);
		if (yGEvent != null && !yGEvent.used && !pauseFinalEventListener)
		{
			FinalEventListener.FireEvent(yGEvent);
		}
	}

	private void DragMove(Vector2 pos, Vector2 delta)
	{
		if (startPosition.HasValue && (startPosition.Value - pos).sqrMagnitude > FINGER_DRAG_RADIUS_SQR)
		{
			List<ITouchable> list = RayHit(startPosition.Value);
			YGEvent yGEvent = new YGEvent();
			yGEvent.type = YGEvent.TYPE.RESET;
			yGEvent.position = (yGEvent.startPosition = pos);
			yGEvent.deltaPosition = delta;
			UpdateAndSendEvent(yGEvent, list);
			startPosition = null;
		}
		BroadcastEvent(pos, YGEvent.TYPE.TOUCH_MOVE, delta);
	}

	private void DragStationary(Vector2 pos)
	{
		BroadcastEvent(pos, YGEvent.TYPE.TOUCH_STAY);
	}

	private void DragEnd(Vector2 pos)
	{
		startPosition = null;
		currentFinger = null;
	}
}
