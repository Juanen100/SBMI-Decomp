using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FingerGestures : MonoBehaviour
{
	public enum FingerPhase
	{
		None = 0,
		Began = 1,
		Moved = 2,
		Stationary = 3,
		Ended = 4
	}

	public class Finger
	{
		public delegate void FingerEventDelegate(Finger finger);

		private int index;

		private bool wasDown;

		private bool down;

		private bool filteredOut = true;

		private float startTime;

		private FingerPhase phase;

		private Vector2 startPos = Vector2.zero;

		private Vector2 pos = Vector2.zero;

		private Vector2 prevPos = Vector2.zero;

		private Vector2 deltaPos = Vector2.zero;

		private float distFromStart;

		public int Index
		{
			get
			{
				return index;
			}
		}

		public FingerPhase Phase
		{
			get
			{
				return phase;
			}
		}

		public bool IsDown
		{
			get
			{
				return down;
			}
		}

		public bool WasDown
		{
			get
			{
				return wasDown;
			}
		}

		public float StarTime
		{
			get
			{
				return startTime;
			}
		}

		public Vector2 StartPosition
		{
			get
			{
				return startPos;
			}
		}

		public Vector2 Position
		{
			get
			{
				return pos;
			}
		}

		public Vector2 PreviousPosition
		{
			get
			{
				return prevPos;
			}
		}

		public Vector2 DeltaPosition
		{
			get
			{
				return deltaPos;
			}
		}

		public float DistanceFromStart
		{
			get
			{
				return distFromStart;
			}
		}

		public bool Filtered
		{
			get
			{
				return filteredOut;
			}
		}

		public event FingerEventDelegate OnDown;

		public event FingerEventDelegate OnUp;

		public Finger(int index)
		{
			this.index = index;
		}

		public override string ToString()
		{
			return "Finger" + index;
		}

		internal void Update(FingerPhase newPhase, Vector2 newPos)
		{
			if (filteredOut)
			{
				if (newPhase == FingerPhase.Ended || newPhase == FingerPhase.None)
				{
					filteredOut = false;
				}
				newPhase = FingerPhase.None;
			}
			if (phase != newPhase)
			{
				if (newPhase == FingerPhase.None && phase != FingerPhase.Ended)
				{
					if (loggingGestures)
					{
						Debug.LogWarning("Correcting bad FingerPhase transition (FingerPhase.Ended skipped)");
					}
					Update(FingerPhase.Ended, PreviousPosition);
					return;
				}
				if (!down && (newPhase == FingerPhase.Moved || newPhase == FingerPhase.Stationary))
				{
					if (loggingGestures)
					{
						Debug.LogWarning("Correcting bad FingerPhase transition (FingerPhase.Began skipped)");
					}
					Update(FingerPhase.Began, newPos);
					return;
				}
				if ((down && newPhase == FingerPhase.Began) || (!down && newPhase == FingerPhase.Ended))
				{
					if (loggingGestures)
					{
						Debug.LogWarning(string.Concat("Invalid state FingerPhase transition from ", phase, " to ", newPhase, " - Skipping."));
					}
					return;
				}
			}
			else if (newPhase == FingerPhase.Began || newPhase == FingerPhase.Ended)
			{
				if (loggingGestures)
				{
					Debug.LogWarning("Duplicated FingerPhase." + newPhase.ToString() + " - skipping.");
				}
				return;
			}
			if (newPhase == FingerPhase.Began && !instance.ShouldProcessTouch(index, newPos))
			{
				filteredOut = true;
				newPhase = FingerPhase.None;
			}
			if (newPhase != FingerPhase.None)
			{
				if (newPhase == FingerPhase.Ended)
				{
					down = false;
				}
				else
				{
					if (newPhase == FingerPhase.Began)
					{
						down = true;
						startPos = newPos;
						prevPos = newPos;
						startTime = Time.time;
					}
					prevPos = pos;
					pos = newPos;
					deltaPos = pos - prevPos;
					distFromStart = Vector3.Distance(startPos, pos);
				}
			}
			phase = newPhase;
		}

		internal void PostUpdate()
		{
			if (wasDown != down)
			{
				if (down)
				{
					if (this.OnDown != null)
					{
						this.OnDown(this);
					}
				}
				else if (this.OnUp != null)
				{
					this.OnUp(this);
				}
			}
			wasDown = down;
		}
	}

	[Serializable]
	public class DefaultComponentCreationFlags
	{
		[Serializable]
		public class PerFinger
		{
			public bool enabled = true;

			public bool touch = true;

			public bool motion = true;

			public bool longPress = true;

			public bool drag = true;

			public bool swipe = true;

			public bool tap = true;

			public bool doubleTap = true;
		}

		[Serializable]
		public class GlobalGestures
		{
			public bool enabled = true;

			public bool longPress = true;

			public bool drag = true;

			public bool swipe = true;

			public bool tap = true;

			public bool doubleTap = true;

			public bool pinch = true;

			public bool rotation = true;

			public bool twoFingerLongPress = true;

			public bool twoFingerDrag = true;

			public bool twoFingerSwipe = true;

			public bool twoFingerTap = true;
		}

		public PerFinger perFinger;

		public GlobalGestures globalGestures;
	}

	public class DefaultComponents
	{
		public class FingerComponents
		{
			public FingerMotionDetector Motion;

			public LongPressGestureRecognizer LongPress;

			public DragGestureRecognizer Drag;

			public TapGestureRecognizer Tap;

			public MultiTapGestureRecognizer DoubleTap;

			public SwipeGestureRecognizer Swipe;
		}

		private FingerComponents[] fingers;

		public LongPressGestureRecognizer LongPress;

		public DragGestureRecognizer Drag;

		public TapGestureRecognizer Tap;

		public MultiTapGestureRecognizer DoubleTap;

		public SwipeGestureRecognizer Swipe;

		public PinchGestureRecognizer Pinch;

		public RotationGestureRecognizer Rotation;

		public LongPressGestureRecognizer TwoFingerLongPress;

		public DragGestureRecognizer TwoFingerDrag;

		public TapGestureRecognizer TwoFingerTap;

		public SwipeGestureRecognizer TwoFingerSwipe;

		public FingerComponents[] Fingers
		{
			get
			{
				return fingers;
			}
		}

		public DefaultComponents(int fingerCount)
		{
			fingers = new FingerComponents[fingerCount];
			for (int i = 0; i < fingers.Length; i++)
			{
				fingers[i] = new FingerComponents();
			}
		}
	}

	public interface IFingerList : IEnumerable, IEnumerable<Finger>
	{
		Finger this[int index] { get; }

		int Count { get; }

		Vector2 GetAveragePosition();

		Vector2 GetAveragePreviousPosition();

		float GetAverageDistanceFromStart();

		Finger GetOldest();
	}

	public class FingerList : IEnumerable, IEnumerable<Finger>, IFingerList
	{
		public delegate T FingerPropertyGetterDelegate<T>(Finger finger);

		private List<Finger> list;

		public Finger this[int index]
		{
			get
			{
				return list[index];
			}
		}

		public int Count
		{
			get
			{
				return list.Count;
			}
		}

		public FingerList()
		{
			list = new List<Finger>();
		}

		public FingerList(List<Finger> list)
		{
			this.list = list;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<Finger> GetEnumerator()
		{
			return list.GetEnumerator();
		}

		public void Add(Finger touch)
		{
			list.Add(touch);
		}

		public void Clear()
		{
			list.Clear();
		}

		public Vector2 AverageVector(FingerPropertyGetterDelegate<Vector2> getProperty)
		{
			Vector2 zero = Vector2.zero;
			if (Count > 0)
			{
				foreach (Finger item in list)
				{
					zero += getProperty(item);
				}
				zero /= (float)Count;
			}
			return zero;
		}

		public float AverageFloat(FingerPropertyGetterDelegate<float> getProperty)
		{
			float num = 0f;
			if (Count > 0)
			{
				foreach (Finger item in list)
				{
					num += getProperty(item);
				}
				num /= (float)Count;
			}
			return num;
		}

		private static Vector2 GetFingerPosition(Finger finger)
		{
			return finger.Position;
		}

		private static Vector2 GetFingerPreviousPosition(Finger finger)
		{
			return finger.PreviousPosition;
		}

		private static float GetFingerDistanceFromStart(Finger finger)
		{
			return finger.DistanceFromStart;
		}

		public Vector2 GetAveragePosition()
		{
			return AverageVector(GetFingerPosition);
		}

		public Vector2 GetAveragePreviousPosition()
		{
			return AverageVector(GetFingerPreviousPosition);
		}

		public float GetAverageDistanceFromStart()
		{
			return AverageFloat(GetFingerDistanceFromStart);
		}

		public Finger GetOldest()
		{
			Finger finger = null;
			foreach (Finger item in list)
			{
				if (finger == null || item.StarTime < finger.StarTime)
				{
					finger = item;
				}
			}
			return finger;
		}
	}

	[Flags]
	public enum SwipeDirection
	{
		Right = 1,
		Left = 2,
		Up = 4,
		Down = 8,
		None = 0,
		All = 0xF,
		Vertical = 0xC,
		Horizontal = 3
	}

	public interface ITouchFilter
	{
		IFingerList Apply(IFingerList touches);
	}

	public class SingleFingerFilter : ITouchFilter
	{
		private FingerList fingerList = new FingerList();

		private FingerList emptyList = new FingerList();

		private Finger finger;

		public Finger Finger
		{
			get
			{
				return finger;
			}
		}

		public SingleFingerFilter(Finger finger)
		{
			this.finger = finger;
			fingerList.Add(finger);
		}

		public IFingerList Apply(IFingerList touches)
		{
			foreach (Finger touch in touches)
			{
				if (touch == Finger)
				{
					return fingerList;
				}
			}
			return emptyList;
		}
	}

	public delegate void FingerDownEventHandler(int fingerIndex, Vector2 fingerPos);

	public delegate void FingerUpEventHandler(int fingerIndex, Vector2 fingerPos, float timeHeldDown);

	public delegate void FingerStationaryBeginEventHandler(int fingerIndex, Vector2 fingerPos);

	public delegate void FingerStationaryEventHandler(int fingerIndex, Vector2 fingerPos, float elapsedTime);

	public delegate void FingerStationaryEndEventHandler(int fingerIndex, Vector2 fingerPos, float elapsedTime);

	public delegate void FingerMoveEventHandler(int fingerIndex, Vector2 fingerPos);

	public delegate void FingerLongPressEventHandler(int fingerIndex, Vector2 fingerPos);

	public delegate void FingerTapEventHandler(int fingerIndex, Vector2 fingerPos);

	public delegate void FingerSwipeEventHandler(int fingerIndex, Vector2 startPos, SwipeDirection direction, float velocity);

	public delegate void FingerDragBeginEventHandler(int fingerIndex, Vector2 fingerPos, Vector2 startPos);

	public delegate void FingerDragMoveEventHandler(int fingerIndex, Vector2 fingerPos, Vector2 delta);

	public delegate void FingerDragEndEventHandler(int fingerIndex, Vector2 fingerPos);

	public delegate void LongPressEventHandler(Vector2 fingerPos);

	public delegate void TapEventHandler(Vector2 fingerPos);

	public delegate void SwipeEventHandler(Vector2 startPos, SwipeDirection direction, float velocity);

	public delegate void DragBeginEventHandler(Vector2 fingerPos, Vector2 startPos);

	public delegate void DragMoveEventHandler(Vector2 fingerPos, Vector2 delta);

	public delegate void DragEndEventHandler(Vector2 fingerPos);

	public delegate void PinchEventHandler(Vector2 fingerPos1, Vector2 fingerPos2);

	public delegate void PinchMoveEventHandler(Vector2 fingerPos1, Vector2 fingerPos2, float delta);

	public delegate void RotationBeginEventHandler(Vector2 fingerPos1, Vector2 fingerPos2);

	public delegate void RotationMoveEventHandler(Vector2 fingerPos1, Vector2 fingerPos2, float rotationAngleDelta);

	public delegate void RotationEndEventHandler(Vector2 fingerPos1, Vector2 fingerPos2, float totalRotationAngle);

	public delegate void FingersUpdatedEventDelegate();

	public delegate bool GlobalTouchFilterDelegate(int fingerIndex, Vector2 position);

	protected static bool loggingGestures;

	private static FingerGestures instance;

	private Finger[] fingers;

	private FingerList touches;

	private GlobalTouchFilterDelegate globalTouchFilterFunc;

	public FingerGesturesPrefabs defaultPrefabs;

	private Transform globalComponentNode;

	private Transform[] fingerComponentNodes;

	public DefaultComponentCreationFlags defaultCompFlags;

	private DefaultComponents defaultComponents;

	public static FingerGestures Instance
	{
		get
		{
			return instance;
		}
	}

	public static IFingerList Touches
	{
		get
		{
			if (instance == null)
			{
				Debug.LogError("Null FG instance!");
			}
			if (instance.touches == null)
			{
				Debug.LogError("Null instance.touches!");
			}
			return instance.touches;
		}
	}

	public abstract int MaxFingers { get; }

	public static GlobalTouchFilterDelegate GlobalTouchFilter
	{
		get
		{
			return instance.globalTouchFilterFunc;
		}
		set
		{
			instance.globalTouchFilterFunc = value;
		}
	}

	public static DefaultComponents Defaults
	{
		get
		{
			return instance.defaultComponents;
		}
	}

	public static event FingerDownEventHandler OnFingerDown;

	public static event FingerUpEventHandler OnFingerUp;

	public static event FingerStationaryBeginEventHandler OnFingerStationaryBegin;

	public static event FingerStationaryEventHandler OnFingerStationary;

	public static event FingerStationaryEndEventHandler OnFingerStationaryEnd;

	public static event FingerMoveEventHandler OnFingerMoveBegin;

	public static event FingerMoveEventHandler OnFingerMove;

	public static event FingerMoveEventHandler OnFingerMoveEnd;

	public static event FingerLongPressEventHandler OnFingerLongPress;

	public static event FingerDragBeginEventHandler OnFingerDragBegin;

	public static event FingerDragMoveEventHandler OnFingerDragMove;

	public static event FingerDragEndEventHandler OnFingerDragStationary;

	public static event FingerDragEndEventHandler OnFingerDragEnd;

	public static event FingerTapEventHandler OnFingerTap;

	public static event FingerTapEventHandler OnFingerDoubleTap;

	public static event FingerSwipeEventHandler OnFingerSwipe;

	public static event LongPressEventHandler OnLongPress;

	public static event DragBeginEventHandler OnDragBegin;

	public static event DragMoveEventHandler OnDragMove;

	public static event DragEndEventHandler OnDragStationary;

	public static event DragEndEventHandler OnDragEnd;

	public static event TapEventHandler OnTap;

	public static event TapEventHandler OnDoubleTap;

	public static event SwipeEventHandler OnSwipe;

	public static event PinchEventHandler OnPinchBegin;

	public static event PinchMoveEventHandler OnPinchMove;

	public static event PinchEventHandler OnPinchEnd;

	public static event RotationBeginEventHandler OnRotationBegin;

	public static event RotationMoveEventHandler OnRotationMove;

	public static event RotationEndEventHandler OnRotationEnd;

	public static event DragBeginEventHandler OnTwoFingerDragBegin;

	public static event DragMoveEventHandler OnTwoFingerDragMove;

	public static event DragEndEventHandler OnTwoFingerDragStationary;

	public static event DragEndEventHandler OnTwoFingerDragEnd;

	public static event TapEventHandler OnTwoFingerTap;

	public static event SwipeEventHandler OnTwoFingerSwipe;

	public static event LongPressEventHandler OnTwoFingerLongPress;

	public static event FingersUpdatedEventDelegate OnFingersUpdated;

	internal static void RaiseOnFingerDown(int fingerIndex, Vector2 fingerPos)
	{
		if (FingerGestures.OnFingerDown != null)
		{
			FingerGestures.OnFingerDown(fingerIndex, fingerPos);
		}
	}

	internal static void RaiseOnFingerUp(int fingerIndex, Vector2 fingerPos, float timeHeldDown)
	{
		if (FingerGestures.OnFingerUp != null)
		{
			FingerGestures.OnFingerUp(fingerIndex, fingerPos, timeHeldDown);
		}
	}

	internal static void RaiseOnFingerStationaryBegin(int fingerIndex, Vector2 fingerPos)
	{
		if (FingerGestures.OnFingerStationaryBegin != null)
		{
			FingerGestures.OnFingerStationaryBegin(fingerIndex, fingerPos);
		}
	}

	internal static void RaiseOnFingerStationary(int fingerIndex, Vector2 fingerPos, float elapsedTime)
	{
		if (FingerGestures.OnFingerStationary != null)
		{
			FingerGestures.OnFingerStationary(fingerIndex, fingerPos, elapsedTime);
		}
	}

	internal static void RaiseOnFingerStationaryEnd(int fingerIndex, Vector2 fingerPos, float elapsedTime)
	{
		if (FingerGestures.OnFingerStationaryEnd != null)
		{
			FingerGestures.OnFingerStationaryEnd(fingerIndex, fingerPos, elapsedTime);
		}
	}

	internal static void RaiseOnFingerMoveBegin(int fingerIndex, Vector2 fingerPos)
	{
		if (FingerGestures.OnFingerMoveBegin != null)
		{
			FingerGestures.OnFingerMoveBegin(fingerIndex, fingerPos);
		}
	}

	internal static void RaiseOnFingerMove(int fingerIndex, Vector2 fingerPos)
	{
		if (FingerGestures.OnFingerMove != null)
		{
			FingerGestures.OnFingerMove(fingerIndex, fingerPos);
		}
	}

	internal static void RaiseOnFingerMoveEnd(int fingerIndex, Vector2 fingerPos)
	{
		if (FingerGestures.OnFingerMoveEnd != null)
		{
			FingerGestures.OnFingerMoveEnd(fingerIndex, fingerPos);
		}
	}

	internal static void RaiseOnFingerLongPress(int fingerIndex, Vector2 fingerPos)
	{
		if (FingerGestures.OnFingerLongPress != null)
		{
			FingerGestures.OnFingerLongPress(fingerIndex, fingerPos);
		}
	}

	internal static void RaiseOnFingerDragBegin(int fingerIndex, Vector2 fingerPos, Vector2 startPos)
	{
		if (FingerGestures.OnFingerDragBegin != null)
		{
			FingerGestures.OnFingerDragBegin(fingerIndex, fingerPos, startPos);
		}
	}

	internal static void RaiseOnFingerDragMove(int fingerIndex, Vector2 fingerPos, Vector2 delta)
	{
		if (FingerGestures.OnFingerDragMove != null)
		{
			FingerGestures.OnFingerDragMove(fingerIndex, fingerPos, delta);
		}
	}

	internal static void RaiseOnFingerDragStationary(int fingerIndex, Vector2 fingerPos)
	{
		if (FingerGestures.OnFingerDragStationary != null)
		{
			FingerGestures.OnFingerDragStationary(fingerIndex, fingerPos);
		}
	}

	internal static void RaiseOnFingerDragEnd(int fingerIndex, Vector2 fingerPos)
	{
		if (FingerGestures.OnFingerDragEnd != null)
		{
			FingerGestures.OnFingerDragEnd(fingerIndex, fingerPos);
		}
	}

	internal static void RaiseOnFingerTap(int fingerIndex, Vector2 fingerPos)
	{
		if (FingerGestures.OnFingerTap != null)
		{
			FingerGestures.OnFingerTap(fingerIndex, fingerPos);
		}
	}

	internal static void RaiseOnFingerDoubleTap(int fingerIndex, Vector2 fingerPos)
	{
		if (FingerGestures.OnFingerDoubleTap != null)
		{
			FingerGestures.OnFingerDoubleTap(fingerIndex, fingerPos);
		}
	}

	internal static void RaiseOnFingerSwipe(int fingerIndex, Vector2 startPos, SwipeDirection direction, float velocity)
	{
		if (FingerGestures.OnFingerSwipe != null)
		{
			FingerGestures.OnFingerSwipe(fingerIndex, startPos, direction, velocity);
		}
	}

	internal static void RaiseOnLongPress(Vector2 fingerPos)
	{
		if (FingerGestures.OnLongPress != null)
		{
			FingerGestures.OnLongPress(fingerPos);
		}
	}

	internal static void RaiseOnDragBegin(Vector2 fingerPos, Vector2 startPos)
	{
		if (FingerGestures.OnDragBegin != null)
		{
			FingerGestures.OnDragBegin(fingerPos, startPos);
		}
	}

	internal static void RaiseOnDragMove(Vector2 fingerPos, Vector2 delta)
	{
		if (FingerGestures.OnDragMove != null)
		{
			FingerGestures.OnDragMove(fingerPos, delta);
		}
	}

	internal static void RaiseOnDragEnd(Vector2 fingerPos)
	{
		if (FingerGestures.OnDragEnd != null)
		{
			FingerGestures.OnDragEnd(fingerPos);
		}
	}

	internal static void RaiseOnDragStationary(Vector2 fingerPos)
	{
		if (FingerGestures.OnDragStationary != null)
		{
			FingerGestures.OnDragStationary(fingerPos);
		}
	}

	internal static void RaiseOnTap(Vector2 fingerPos)
	{
		if (FingerGestures.OnTap != null)
		{
			FingerGestures.OnTap(fingerPos);
		}
	}

	internal static void RaiseOnDoubleTap(Vector2 fingerPos)
	{
		if (FingerGestures.OnDoubleTap != null)
		{
			FingerGestures.OnDoubleTap(fingerPos);
		}
	}

	internal static void RaiseOnSwipe(Vector2 startPos, SwipeDirection direction, float velocity)
	{
		if (FingerGestures.OnSwipe != null)
		{
			FingerGestures.OnSwipe(startPos, direction, velocity);
		}
	}

	internal static void RaiseOnPinchBegin(Vector2 fingerPos1, Vector2 fingerPos2)
	{
		if (FingerGestures.OnPinchBegin != null)
		{
			FingerGestures.OnPinchBegin(fingerPos1, fingerPos2);
		}
	}

	internal static void RaiseOnPinchMove(Vector2 fingerPos1, Vector2 fingerPos2, float delta)
	{
		if (FingerGestures.OnPinchMove != null)
		{
			FingerGestures.OnPinchMove(fingerPos1, fingerPos2, delta);
		}
	}

	internal static void RaiseOnPinchEnd(Vector2 fingerPos1, Vector2 fingerPos2)
	{
		if (FingerGestures.OnPinchEnd != null)
		{
			FingerGestures.OnPinchEnd(fingerPos1, fingerPos2);
		}
	}

	internal static void RaiseOnRotationBegin(Vector2 fingerPos1, Vector2 fingerPos2)
	{
		if (FingerGestures.OnRotationBegin != null)
		{
			FingerGestures.OnRotationBegin(fingerPos1, fingerPos2);
		}
	}

	internal static void RaiseOnRotationMove(Vector2 fingerPos1, Vector2 fingerPos2, float rotationAngleDelta)
	{
		if (FingerGestures.OnRotationMove != null)
		{
			FingerGestures.OnRotationMove(fingerPos1, fingerPos2, rotationAngleDelta);
		}
	}

	internal static void RaiseOnRotationEnd(Vector2 fingerPos1, Vector2 fingerPos2, float totalRotationAngle)
	{
		if (FingerGestures.OnRotationEnd != null)
		{
			FingerGestures.OnRotationEnd(fingerPos1, fingerPos2, totalRotationAngle);
		}
	}

	internal static void RaiseOnTwoFingerLongPress(Vector2 fingerPos)
	{
		if (FingerGestures.OnTwoFingerLongPress != null)
		{
			FingerGestures.OnTwoFingerLongPress(fingerPos);
		}
	}

	internal static void RaiseOnTwoFingerDragBegin(Vector2 fingerPos, Vector2 startPos)
	{
		if (FingerGestures.OnTwoFingerDragBegin != null)
		{
			FingerGestures.OnTwoFingerDragBegin(fingerPos, startPos);
		}
	}

	internal static void RaiseOnTwoFingerDragMove(Vector2 fingerPos, Vector2 delta)
	{
		if (FingerGestures.OnTwoFingerDragMove != null)
		{
			FingerGestures.OnTwoFingerDragMove(fingerPos, delta);
		}
	}

	internal static void RaiseOnTwoFingerDragStationary(Vector2 fingerPos)
	{
		if (FingerGestures.OnTwoFingerDragStationary != null)
		{
			FingerGestures.OnTwoFingerDragStationary(fingerPos);
		}
	}

	internal static void RaiseOnTwoFingerDragEnd(Vector2 fingerPos)
	{
		if (FingerGestures.OnTwoFingerDragEnd != null)
		{
			FingerGestures.OnTwoFingerDragEnd(fingerPos);
		}
	}

	internal static void RaiseOnTwoFingerTap(Vector2 fingerPos)
	{
		if (FingerGestures.OnTwoFingerTap != null)
		{
			FingerGestures.OnTwoFingerTap(fingerPos);
		}
	}

	internal static void RaiseOnTwoFingerSwipe(Vector2 startPos, SwipeDirection direction, float velocity)
	{
		if (FingerGestures.OnTwoFingerSwipe != null)
		{
			FingerGestures.OnTwoFingerSwipe(startPos, direction, velocity);
		}
	}

	public static Finger GetFinger(int index)
	{
		return instance.fingers[index];
	}

	protected virtual void Awake()
	{
	}

	protected virtual void OnEnable()
	{
		if (instance == null)
		{
			instance = this;
			InitFingers(MaxFingers);
		}
		else if (instance != this)
		{
			if (loggingGestures)
			{
				Debug.LogWarning("There is already an instance of FingerGestures created (" + instance.name + "). Destroying new one.");
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	protected virtual void Start()
	{
	}

	protected virtual void OnDisable()
	{
	}

	protected virtual void Update()
	{
		UpdateFingers();
		if (FingerGestures.OnFingersUpdated != null)
		{
			FingerGestures.OnFingersUpdated();
		}
	}

	protected abstract FingerPhase GetPhase(Finger finger);

	protected abstract Vector2 GetPosition(Finger finger);

	private void InitFingers(int count)
	{
		fingers = new Finger[count];
		for (int i = 0; i < count; i++)
		{
			fingers[i] = new Finger(i);
		}
		touches = new FingerList();
		InitDefaultComponents();
	}

	private void UpdateFingers()
	{
		touches.Clear();
		Finger[] array = fingers;
		foreach (Finger finger in array)
		{
			Vector2 newPos = Vector2.zero;
			FingerPhase phase = GetPhase(finger);
			if (phase != FingerPhase.None)
			{
				newPos = GetPosition(finger);
			}
			finger.Update(phase, newPos);
			if (finger.IsDown)
			{
				touches.Add(finger);
			}
		}
		Finger[] array2 = fingers;
		foreach (Finger finger2 in array2)
		{
			finger2.PostUpdate();
		}
	}

	protected bool ShouldProcessTouch(int fingerIndex, Vector2 position)
	{
		if (globalTouchFilterFunc != null)
		{
			return globalTouchFilterFunc(fingerIndex, position);
		}
		return true;
	}

	private T CreateDefaultComponent<T>(T prefab, Transform parent) where T : FGComponent
	{
		T result = UnityEngine.Object.Instantiate(prefab) as T;
		result.gameObject.name = prefab.name;
		result.transform.parent = parent;
		return result;
	}

	private T CreateDefaultGlobalComponent<T>(T prefab) where T : FGComponent
	{
		return CreateDefaultComponent(prefab, globalComponentNode);
	}

	private T CreateDefaultFingerComponent<T>(Finger finger, T prefab) where T : FGComponent
	{
		return CreateDefaultComponent(prefab, fingerComponentNodes[finger.Index]);
	}

	private Transform CreateNode(string name, Transform parent)
	{
		GameObject gameObject = new GameObject(name);
		gameObject.transform.parent = parent;
		return gameObject.transform;
	}

	private void InitDefaultComponents()
	{
		int num = fingers.Length;
		if ((bool)globalComponentNode)
		{
			UnityEngine.Object.Destroy(globalComponentNode.gameObject);
		}
		if (fingerComponentNodes != null)
		{
			Transform[] array = fingerComponentNodes;
			foreach (Transform transform in array)
			{
				UnityEngine.Object.Destroy(transform.gameObject);
			}
		}
		globalComponentNode = CreateNode("Global Components", base.transform);
		fingerComponentNodes = new Transform[num];
		for (int j = 0; j < fingerComponentNodes.Length; j++)
		{
			fingerComponentNodes[j] = CreateNode("Finger" + j, base.transform);
		}
		defaultComponents = new DefaultComponents(num);
		if (defaultCompFlags.globalGestures.enabled)
		{
			InitGlobalGestures();
		}
		if (defaultCompFlags.perFinger.enabled)
		{
			Finger[] array2 = fingers;
			foreach (Finger finger in array2)
			{
				InitDefaultComponents(finger);
			}
		}
	}

	private void InitGlobalGestures()
	{
		if (defaultCompFlags.globalGestures.longPress)
		{
			LongPressGestureRecognizer longPressGestureRecognizer = CreateDefaultGlobalComponent(defaultPrefabs.longPress);
			longPressGestureRecognizer.OnLongPress += delegate(LongPressGestureRecognizer rec)
			{
				RaiseOnLongPress(rec.Position);
			};
			defaultComponents.LongPress = longPressGestureRecognizer;
		}
		if (defaultCompFlags.globalGestures.twoFingerLongPress)
		{
			LongPressGestureRecognizer longPressGestureRecognizer2 = CreateDefaultGlobalComponent(defaultPrefabs.twoFingerLongPress);
			longPressGestureRecognizer2.RequiredFingerCount = 2;
			longPressGestureRecognizer2.OnLongPress += delegate(LongPressGestureRecognizer rec)
			{
				RaiseOnTwoFingerLongPress(rec.Position);
			};
			defaultComponents.TwoFingerLongPress = longPressGestureRecognizer2;
		}
		if (defaultCompFlags.globalGestures.drag)
		{
			DragGestureRecognizer dragGestureRecognizer = CreateDefaultGlobalComponent(defaultPrefabs.drag);
			dragGestureRecognizer.OnDragBegin += delegate(DragGestureRecognizer rec)
			{
				RaiseOnDragBegin(rec.Position, rec.StartPosition);
			};
			dragGestureRecognizer.OnDragMove += delegate(DragGestureRecognizer rec)
			{
				RaiseOnDragMove(rec.Position, rec.MoveDelta);
			};
			dragGestureRecognizer.OnDragStationary += delegate(DragGestureRecognizer rec)
			{
				RaiseOnDragStationary(rec.Position);
			};
			dragGestureRecognizer.OnDragEnd += delegate(DragGestureRecognizer rec)
			{
				RaiseOnDragEnd(rec.Position);
			};
			defaultComponents.Drag = dragGestureRecognizer;
		}
		if (defaultCompFlags.globalGestures.twoFingerDrag)
		{
			DragGestureRecognizer dragGestureRecognizer2 = CreateDefaultGlobalComponent(defaultPrefabs.twoFingerDrag);
			dragGestureRecognizer2.RequiredFingerCount = 2;
			dragGestureRecognizer2.OnDragBegin += delegate(DragGestureRecognizer rec)
			{
				RaiseOnTwoFingerDragBegin(rec.Position, rec.StartPosition);
			};
			dragGestureRecognizer2.OnDragMove += delegate(DragGestureRecognizer rec)
			{
				RaiseOnTwoFingerDragMove(rec.Position, rec.MoveDelta);
			};
			dragGestureRecognizer2.OnDragStationary += delegate(DragGestureRecognizer rec)
			{
				RaiseOnTwoFingerDragStationary(rec.Position);
			};
			dragGestureRecognizer2.OnDragEnd += delegate(DragGestureRecognizer rec)
			{
				RaiseOnTwoFingerDragEnd(rec.Position);
			};
			defaultComponents.TwoFingerDrag = dragGestureRecognizer2;
		}
		if (defaultCompFlags.globalGestures.swipe)
		{
			SwipeGestureRecognizer swipeGestureRecognizer = CreateDefaultGlobalComponent(defaultPrefabs.swipe);
			swipeGestureRecognizer.OnSwipe += delegate(SwipeGestureRecognizer rec)
			{
				RaiseOnSwipe(rec.StartPosition, rec.Direction, rec.Velocity);
			};
			defaultComponents.Swipe = swipeGestureRecognizer;
		}
		if (defaultCompFlags.globalGestures.twoFingerSwipe)
		{
			SwipeGestureRecognizer swipeGestureRecognizer2 = CreateDefaultGlobalComponent(defaultPrefabs.twoFingerSwipe);
			swipeGestureRecognizer2.RequiredFingerCount = 2;
			swipeGestureRecognizer2.OnSwipe += delegate(SwipeGestureRecognizer rec)
			{
				RaiseOnTwoFingerSwipe(rec.StartPosition, rec.Direction, rec.Velocity);
			};
			defaultComponents.TwoFingerSwipe = swipeGestureRecognizer2;
		}
		if (defaultCompFlags.globalGestures.tap)
		{
			TapGestureRecognizer tapGestureRecognizer = CreateDefaultGlobalComponent(defaultPrefabs.tap);
			tapGestureRecognizer.OnTap += delegate(TapGestureRecognizer rec)
			{
				RaiseOnTap(rec.Position);
			};
			defaultComponents.Tap = tapGestureRecognizer;
		}
		if (defaultCompFlags.globalGestures.doubleTap)
		{
			MultiTapGestureRecognizer multiTapGestureRecognizer = CreateDefaultGlobalComponent(defaultPrefabs.doubleTap);
			multiTapGestureRecognizer.OnTap += delegate(MultiTapGestureRecognizer rec)
			{
				RaiseOnDoubleTap(rec.Position);
			};
			defaultComponents.DoubleTap = multiTapGestureRecognizer;
		}
		if (defaultCompFlags.globalGestures.twoFingerTap)
		{
			TapGestureRecognizer tapGestureRecognizer2 = CreateDefaultGlobalComponent(defaultPrefabs.twoFingerTap);
			tapGestureRecognizer2.RequiredFingerCount = 2;
			tapGestureRecognizer2.OnTap += delegate(TapGestureRecognizer rec)
			{
				RaiseOnTwoFingerTap(rec.Position);
			};
			defaultComponents.TwoFingerTap = tapGestureRecognizer2;
		}
		if (defaultCompFlags.globalGestures.pinch)
		{
			PinchGestureRecognizer pinchGestureRecognizer = CreateDefaultGlobalComponent(defaultPrefabs.pinch);
			pinchGestureRecognizer.OnPinchBegin += delegate(PinchGestureRecognizer rec)
			{
				RaiseOnPinchBegin(rec.GetPosition(0), rec.GetPosition(1));
			};
			pinchGestureRecognizer.OnPinchMove += delegate(PinchGestureRecognizer rec)
			{
				RaiseOnPinchMove(rec.GetPosition(0), rec.GetPosition(1), rec.Delta);
			};
			pinchGestureRecognizer.OnPinchEnd += delegate(PinchGestureRecognizer rec)
			{
				RaiseOnPinchEnd(rec.GetPosition(0), rec.GetPosition(1));
			};
			defaultComponents.Pinch = pinchGestureRecognizer;
		}
		if (defaultCompFlags.globalGestures.rotation)
		{
			RotationGestureRecognizer rotationGestureRecognizer = CreateDefaultGlobalComponent(defaultPrefabs.rotation);
			rotationGestureRecognizer.OnRotationBegin += delegate(RotationGestureRecognizer rec)
			{
				RaiseOnRotationBegin(rec.GetPosition(0), rec.GetPosition(1));
			};
			rotationGestureRecognizer.OnRotationMove += delegate(RotationGestureRecognizer rec)
			{
				RaiseOnRotationMove(rec.GetPosition(0), rec.GetPosition(1), rec.RotationDelta);
			};
			rotationGestureRecognizer.OnRotationEnd += delegate(RotationGestureRecognizer rec)
			{
				RaiseOnRotationEnd(rec.GetPosition(0), rec.GetPosition(1), rec.TotalRotation);
			};
			defaultComponents.Rotation = rotationGestureRecognizer;
		}
	}

	private void InitDefaultComponents(Finger finger)
	{
		ITouchFilter touchFilter = new SingleFingerFilter(finger);
		DefaultComponents.FingerComponents fingerComponents = defaultComponents.Fingers[finger.Index];
		if (defaultCompFlags.perFinger.touch)
		{
			finger.OnDown += PerFinger_OnDown;
			finger.OnUp += PerFinger_OnUp;
		}
		if (defaultCompFlags.perFinger.motion)
		{
			FingerMotionDetector fingerMotionDetector = CreateDefaultFingerComponent(finger, defaultPrefabs.fingerMotion);
			fingerMotionDetector.Finger = finger;
			fingerMotionDetector.OnMoveBegin += PerFinger_OnMoveBegin;
			fingerMotionDetector.OnMove += PerFinger_OnMove;
			fingerMotionDetector.OnMoveEnd += PerFinger_OnMoveEnd;
			fingerMotionDetector.OnStationaryBegin += PerFinger_OnStationaryBegin;
			fingerMotionDetector.OnStationary += PerFinger_OnStationary;
			fingerMotionDetector.OnStationaryEnd += PerFinger_OnStationaryEnd;
			fingerComponents.Motion = fingerMotionDetector;
		}
		if (defaultCompFlags.perFinger.longPress)
		{
			LongPressGestureRecognizer longPressGestureRecognizer = CreateDefaultFingerComponent(finger, defaultPrefabs.fingerLongPress);
			longPressGestureRecognizer.TouchFilter = touchFilter;
			longPressGestureRecognizer.OnLongPress += PerFinger_OnLongPress;
			fingerComponents.LongPress = longPressGestureRecognizer;
		}
		if (defaultCompFlags.perFinger.drag)
		{
			DragGestureRecognizer dragGestureRecognizer = CreateDefaultFingerComponent(finger, defaultPrefabs.fingerDrag);
			dragGestureRecognizer.TouchFilter = touchFilter;
			dragGestureRecognizer.OnDragBegin += PerFinger_OnDragBegin;
			dragGestureRecognizer.OnDragMove += PerFinger_OnDragMove;
			dragGestureRecognizer.OnDragStationary += PerFinger_OnDragStationary;
			dragGestureRecognizer.OnDragEnd += PerFinger_OnDragEnd;
			fingerComponents.Drag = dragGestureRecognizer;
		}
		if (defaultCompFlags.perFinger.swipe)
		{
			SwipeGestureRecognizer swipeGestureRecognizer = CreateDefaultFingerComponent(finger, defaultPrefabs.fingerSwipe);
			swipeGestureRecognizer.TouchFilter = touchFilter;
			swipeGestureRecognizer.OnSwipe += PerFinger_OnSwipe;
			fingerComponents.Swipe = swipeGestureRecognizer;
		}
		if (defaultCompFlags.perFinger.tap)
		{
			TapGestureRecognizer tapGestureRecognizer = CreateDefaultFingerComponent(finger, defaultPrefabs.fingerTap);
			tapGestureRecognizer.TouchFilter = touchFilter;
			tapGestureRecognizer.OnTap += PerFinger_OnTap;
			fingerComponents.Tap = tapGestureRecognizer;
		}
		if (defaultCompFlags.perFinger.doubleTap)
		{
			MultiTapGestureRecognizer multiTapGestureRecognizer = CreateDefaultFingerComponent(finger, defaultPrefabs.fingerDoubleTap);
			multiTapGestureRecognizer.TouchFilter = touchFilter;
			multiTapGestureRecognizer.OnTap += PerFinger_OnDoubleTap;
			fingerComponents.DoubleTap = multiTapGestureRecognizer;
		}
	}

	private static Finger GetFingerFromTouchFilter(GestureRecognizer recognizer)
	{
		SingleFingerFilter singleFingerFilter = recognizer.TouchFilter as SingleFingerFilter;
		if (singleFingerFilter != null)
		{
			return singleFingerFilter.Finger;
		}
		return null;
	}

	private void PerFinger_OnDown(Finger source)
	{
		RaiseOnFingerDown(source.Index, source.Position);
	}

	private void PerFinger_OnUp(Finger source)
	{
		RaiseOnFingerUp(source.Index, source.Position, Time.time - source.StarTime);
	}

	private void PerFinger_OnStationaryBegin(FingerMotionDetector source)
	{
		RaiseOnFingerStationaryBegin(source.Finger.Index, source.AnchorPos);
	}

	private void PerFinger_OnStationary(FingerMotionDetector source)
	{
		RaiseOnFingerStationary(source.Finger.Index, source.Finger.Position, source.ElapsedStationaryTime);
	}

	private void PerFinger_OnStationaryEnd(FingerMotionDetector source)
	{
		RaiseOnFingerStationaryEnd(source.Finger.Index, source.Finger.PreviousPosition, source.ElapsedStationaryTime);
	}

	private void PerFinger_OnMoveBegin(FingerMotionDetector source)
	{
		RaiseOnFingerMoveBegin(source.Finger.Index, source.AnchorPos);
	}

	private void PerFinger_OnMove(FingerMotionDetector source)
	{
		RaiseOnFingerMove(source.Finger.Index, source.Finger.Position);
	}

	private void PerFinger_OnMoveEnd(FingerMotionDetector source)
	{
		RaiseOnFingerMoveEnd(source.Finger.Index, source.Finger.Position);
	}

	private void PerFinger_OnDragBegin(DragGestureRecognizer source)
	{
		Finger fingerFromTouchFilter = GetFingerFromTouchFilter(source);
		RaiseOnFingerDragBegin(fingerFromTouchFilter.Index, source.Position, source.StartPosition);
	}

	private void PerFinger_OnDragMove(DragGestureRecognizer source)
	{
		Finger fingerFromTouchFilter = GetFingerFromTouchFilter(source);
		RaiseOnFingerDragMove(fingerFromTouchFilter.Index, source.Position, source.MoveDelta);
	}

	private void PerFinger_OnDragStationary(DragGestureRecognizer source)
	{
		Finger fingerFromTouchFilter = GetFingerFromTouchFilter(source);
		RaiseOnFingerDragStationary(fingerFromTouchFilter.Index, source.Position);
	}

	private void PerFinger_OnDragEnd(DragGestureRecognizer source)
	{
		Finger fingerFromTouchFilter = GetFingerFromTouchFilter(source);
		RaiseOnFingerDragEnd(fingerFromTouchFilter.Index, source.Position);
	}

	private void PerFinger_OnLongPress(LongPressGestureRecognizer source)
	{
		Finger fingerFromTouchFilter = GetFingerFromTouchFilter(source);
		RaiseOnFingerLongPress(fingerFromTouchFilter.Index, source.Position);
	}

	private void PerFinger_OnSwipe(SwipeGestureRecognizer source)
	{
		Finger fingerFromTouchFilter = GetFingerFromTouchFilter(source);
		RaiseOnFingerSwipe(fingerFromTouchFilter.Index, source.StartPosition, source.Direction, source.Velocity);
	}

	private void PerFinger_OnTap(TapGestureRecognizer source)
	{
		Finger fingerFromTouchFilter = GetFingerFromTouchFilter(source);
		RaiseOnFingerTap(fingerFromTouchFilter.Index, source.Position);
	}

	private void PerFinger_OnDoubleTap(MultiTapGestureRecognizer source)
	{
		Finger fingerFromTouchFilter = GetFingerFromTouchFilter(source);
		RaiseOnFingerDoubleTap(fingerFromTouchFilter.Index, source.Position);
	}

	public static SwipeDirection GetSwipeDirection(Vector3 dir, float tolerance)
	{
		float num = Mathf.Clamp01(1f - tolerance);
		if (Vector2.Dot(dir, Vector2.right) >= num)
		{
			return SwipeDirection.Right;
		}
		if (Vector2.Dot(dir, -Vector2.right) >= num)
		{
			return SwipeDirection.Left;
		}
		if (Vector2.Dot(dir, Vector2.up) >= num)
		{
			return SwipeDirection.Up;
		}
		if (Vector2.Dot(dir, -Vector2.up) >= num)
		{
			return SwipeDirection.Down;
		}
		return SwipeDirection.None;
	}

	public static bool AllFingersMoving(params Finger[] fingers)
	{
		if (fingers.Length == 0)
		{
			return false;
		}
		foreach (Finger finger in fingers)
		{
			if (finger.Phase != FingerPhase.Moved)
			{
				return false;
			}
		}
		return true;
	}

	public static bool FingersMovedInOppositeDirections(Finger finger0, Finger finger1, float minDOT)
	{
		float num = Vector2.Dot(finger0.DeltaPosition.normalized, finger1.DeltaPosition.normalized);
		return num < minDOT;
	}

	public static float SignedAngle(Vector2 from, Vector2 to)
	{
		float y = from.x * to.y - from.y * to.x;
		return Mathf.Atan2(y, Vector2.Dot(from, to));
	}
}
