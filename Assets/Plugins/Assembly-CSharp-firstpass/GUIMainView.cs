using System.Collections.Generic;
using UnityEngine;
using Yarg;

public class GUIMainView : GUIView
{
	public Vector2 defaultResolution;

	private static GUIMainView instance;

	public EventDispatcher<YGEvent> FinalEventListener;

	public const float DESKTOP_DPI_GUESS = 110f;

	private bool pauseFinalEventListener;

	private List<GUISubView> subViews;

	private const float FINGER_DRAG_RADIUS_INCHES = 0.5f;

	private static float FINGER_DRAG_RADIUS_SQR;

	private int? currentFinger;

	protected Vector2? startPosition;

	public static float EffectiveDPI
	{
		get
		{
			return 0f;
		}
	}

	public void ClearFinalEventListener()
	{
	}

	public void PauseFinalEventListener(bool pause)
	{
	}

	public static GUIMainView GetInstance()
	{
		return null;
	}

	private static bool SetInstance(GUIMainView inst)
	{
		return false;
	}

	protected override void OnEnable()
	{
	}

	protected override void OnDisable()
	{
	}

	protected override void Start()
	{
	}

	public override void ResizePortal()
	{
	}

	public Bounds ViewBounds()
	{
		return default(Bounds);
	}

	public GUISubView CreateSubView()
	{
		return null;
	}

	public bool AddSubView(GUISubView sub)
	{
		return false;
	}

	public bool RemoveSubView(GUISubView sub)
	{
		return false;
	}

	protected override List<ITouchable> RayHit(Vector2 pos)
	{
		return null;
	}

	private void RegisterFingers(bool active)
	{
	}

	private void FingerUp(int fingerIndex, Vector2 pos, float time)
	{
	}

	private void FingerDown(int fingerIndex, Vector2 pos)
	{
	}

	private void Tap(Vector2 pos)
	{
	}

	private void PinchBegin(Vector2 pos1, Vector2 pos2)
	{
	}

	private void PinchEnd(Vector2 pos1, Vector2 pos2)
	{
	}

	private void PinchMove(Vector2 pos1, Vector2 pos2, float delta)
	{
	}

	private void LongPress(Vector2 pos)
	{
	}

	private void DragBegin(Vector2 pos, Vector2 startPos)
	{
	}

	private void BroadcastEvent(Vector2 pos, YGEvent.TYPE type, Vector2? delta = null)
	{
	}

	private void DragMove(Vector2 pos, Vector2 delta)
	{
	}

	private void DragStationary(Vector2 pos)
	{
	}

	private void DragEnd(Vector2 pos)
	{
	}
}
