using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FingerGestures : MonoBehaviour
{
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

		private bool filteredOut;

		private float startTime;

		private FingerPhase phase;

		private Vector2 startPos;

		private Vector2 pos;

		private Vector2 prevPos;

		private Vector2 deltaPos;

		private float distFromStart;

		public int Index
		{
			get
			{
				return 0;
			}
		}

		public FingerPhase Phase
		{
			get
			{
				return default(FingerPhase);
			}
		}

		public bool IsDown
		{
			get
			{
				return false;
			}
		}

		public bool WasDown
		{
			get
			{
				return false;
			}
		}

		public float StarTime
		{
			get
			{
				return 0f;
			}
		}

		public Vector2 StartPosition
		{
			get
			{
				return default(Vector2);
			}
		}

		public Vector2 Position
		{
			get
			{
				return default(Vector2);
			}
		}

		public Vector2 PreviousPosition
		{
			get
			{
				return default(Vector2);
			}
		}

		public Vector2 DeltaPosition
		{
			get
			{
				return default(Vector2);
			}
		}

		public float DistanceFromStart
		{
			get
			{
				return 0f;
			}
		}

		public bool Filtered
		{
			get
			{
				return false;
			}
		}

		public event FingerEventDelegate OnDown
		{
			add
			{
			}
			remove
			{
			}
		}

		public event FingerEventDelegate OnUp
		{
			add
			{
			}
			remove
			{
			}
		}

		public Finger(int index)
		{
		}

		public override string ToString()
		{
			return null;
		}

		internal void Update(FingerPhase newPhase, Vector2 newPos)
		{
		}

		internal void PostUpdate()
		{
		}
	}

	public delegate void FingersUpdatedEventDelegate();

	public delegate bool GlobalTouchFilterDelegate(int fingerIndex, Vector2 position);

	[Serializable]
	public class DefaultComponentCreationFlags
	{
		[Serializable]
		public class PerFinger
		{
			public bool enabled;

			public bool touch;

			public bool motion;

			public bool longPress;

			public bool drag;

			public bool swipe;

			public bool tap;

			public bool doubleTap;
		}

		[Serializable]
		public class GlobalGestures
		{
			public bool enabled;

			public bool longPress;

			public bool drag;

			public bool swipe;

			public bool tap;

			public bool doubleTap;

			public bool pinch;

			public bool rotation;

			public bool twoFingerLongPress;

			public bool twoFingerDrag;

			public bool twoFingerSwipe;

			public bool twoFingerTap;
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
				return null;
			}
		}

		public DefaultComponents(int fingerCount)
		{
		}
	}

	public interface IFingerList : IEnumerable<Finger>, IEnumerable
	{
		Finger Item { get; }

		int Count { get; }

		Vector2 GetAveragePosition();

		Vector2 GetAveragePreviousPosition();

		float GetAverageDistanceFromStart();

		Finger GetOldest();
	}

	public class FingerList : IFingerList, IEnumerable<Finger>, IEnumerable
	{
		public delegate T FingerPropertyGetterDelegate<T>(Finger finger);

		private List<Finger> list;

		public Finger Item
		{
			get
			{
				return null;
			}
		}

		public int Count
		{
			get
			{
				return 0;
			}
		}

		public FingerList()
		{
		}

		public FingerList(List<Finger> list)
		{
		}

		public IEnumerator<Finger> GetEnumerator()
		{
			return null;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return null;
		}

		public void Add(Finger touch)
		{
		}

		public void Clear()
		{
		}

		public Vector2 AverageVector(FingerPropertyGetterDelegate<Vector2> getProperty)
		{
			return default(Vector2);
		}

		public float AverageFloat(FingerPropertyGetterDelegate<float> getProperty)
		{
			return 0f;
		}

		private static Vector2 GetFingerPosition(Finger finger)
		{
			return default(Vector2);
		}

		private static Vector2 GetFingerPreviousPosition(Finger finger)
		{
			return default(Vector2);
		}

		private static float GetFingerDistanceFromStart(Finger finger)
		{
			return 0f;
		}

		public Vector2 GetAveragePosition()
		{
			return default(Vector2);
		}

		public Vector2 GetAveragePreviousPosition()
		{
			return default(Vector2);
		}

		public float GetAverageDistanceFromStart()
		{
			return 0f;
		}

		public Finger GetOldest()
		{
			return null;
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
		private FingerList fingerList;

		private FingerList emptyList;

		private Finger finger;

		public Finger Finger
		{
			get
			{
				return null;
			}
		}

		public SingleFingerFilter(Finger finger)
		{
		}

		public IFingerList Apply(IFingerList touches)
		{
			return null;
		}
	}

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
			return null;
		}
	}

	public static IFingerList Touches
	{
		get
		{
			return null;
		}
	}

	public abstract int MaxFingers { get; }

	public static GlobalTouchFilterDelegate GlobalTouchFilter
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public static DefaultComponents Defaults
	{
		get
		{
			return null;
		}
	}

	public static event FingerDownEventHandler OnFingerDown
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event FingerUpEventHandler OnFingerUp
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event FingerStationaryBeginEventHandler OnFingerStationaryBegin
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event FingerStationaryEventHandler OnFingerStationary
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event FingerStationaryEndEventHandler OnFingerStationaryEnd
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event FingerMoveEventHandler OnFingerMoveBegin
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event FingerMoveEventHandler OnFingerMove
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event FingerMoveEventHandler OnFingerMoveEnd
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event FingerLongPressEventHandler OnFingerLongPress
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event FingerDragBeginEventHandler OnFingerDragBegin
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event FingerDragMoveEventHandler OnFingerDragMove
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event FingerDragEndEventHandler OnFingerDragStationary
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event FingerDragEndEventHandler OnFingerDragEnd
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event FingerTapEventHandler OnFingerTap
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event FingerTapEventHandler OnFingerDoubleTap
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event FingerSwipeEventHandler OnFingerSwipe
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event LongPressEventHandler OnLongPress
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event DragBeginEventHandler OnDragBegin
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event DragMoveEventHandler OnDragMove
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event DragEndEventHandler OnDragStationary
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event DragEndEventHandler OnDragEnd
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event TapEventHandler OnTap
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event TapEventHandler OnDoubleTap
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event SwipeEventHandler OnSwipe
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event PinchEventHandler OnPinchBegin
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event PinchMoveEventHandler OnPinchMove
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event PinchEventHandler OnPinchEnd
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event RotationBeginEventHandler OnRotationBegin
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event RotationMoveEventHandler OnRotationMove
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event RotationEndEventHandler OnRotationEnd
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event DragBeginEventHandler OnTwoFingerDragBegin
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event DragMoveEventHandler OnTwoFingerDragMove
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event DragEndEventHandler OnTwoFingerDragStationary
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event DragEndEventHandler OnTwoFingerDragEnd
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event TapEventHandler OnTwoFingerTap
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event SwipeEventHandler OnTwoFingerSwipe
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event LongPressEventHandler OnTwoFingerLongPress
	{
		add
		{
		}
		remove
		{
		}
	}

	public static event FingersUpdatedEventDelegate OnFingersUpdated
	{
		add
		{
		}
		remove
		{
		}
	}

	internal static void RaiseOnFingerDown(int fingerIndex, Vector2 fingerPos)
	{
	}

	internal static void RaiseOnFingerUp(int fingerIndex, Vector2 fingerPos, float timeHeldDown)
	{
	}

	internal static void RaiseOnFingerStationaryBegin(int fingerIndex, Vector2 fingerPos)
	{
	}

	internal static void RaiseOnFingerStationary(int fingerIndex, Vector2 fingerPos, float elapsedTime)
	{
	}

	internal static void RaiseOnFingerStationaryEnd(int fingerIndex, Vector2 fingerPos, float elapsedTime)
	{
	}

	internal static void RaiseOnFingerMoveBegin(int fingerIndex, Vector2 fingerPos)
	{
	}

	internal static void RaiseOnFingerMove(int fingerIndex, Vector2 fingerPos)
	{
	}

	internal static void RaiseOnFingerMoveEnd(int fingerIndex, Vector2 fingerPos)
	{
	}

	internal static void RaiseOnFingerLongPress(int fingerIndex, Vector2 fingerPos)
	{
	}

	internal static void RaiseOnFingerDragBegin(int fingerIndex, Vector2 fingerPos, Vector2 startPos)
	{
	}

	internal static void RaiseOnFingerDragMove(int fingerIndex, Vector2 fingerPos, Vector2 delta)
	{
	}

	internal static void RaiseOnFingerDragStationary(int fingerIndex, Vector2 fingerPos)
	{
	}

	internal static void RaiseOnFingerDragEnd(int fingerIndex, Vector2 fingerPos)
	{
	}

	internal static void RaiseOnFingerTap(int fingerIndex, Vector2 fingerPos)
	{
	}

	internal static void RaiseOnFingerDoubleTap(int fingerIndex, Vector2 fingerPos)
	{
	}

	internal static void RaiseOnFingerSwipe(int fingerIndex, Vector2 startPos, SwipeDirection direction, float velocity)
	{
	}

	internal static void RaiseOnLongPress(Vector2 fingerPos)
	{
	}

	internal static void RaiseOnDragBegin(Vector2 fingerPos, Vector2 startPos)
	{
	}

	internal static void RaiseOnDragMove(Vector2 fingerPos, Vector2 delta)
	{
	}

	internal static void RaiseOnDragEnd(Vector2 fingerPos)
	{
	}

	internal static void RaiseOnDragStationary(Vector2 fingerPos)
	{
	}

	internal static void RaiseOnTap(Vector2 fingerPos)
	{
	}

	internal static void RaiseOnDoubleTap(Vector2 fingerPos)
	{
	}

	internal static void RaiseOnSwipe(Vector2 startPos, SwipeDirection direction, float velocity)
	{
	}

	internal static void RaiseOnPinchBegin(Vector2 fingerPos1, Vector2 fingerPos2)
	{
	}

	internal static void RaiseOnPinchMove(Vector2 fingerPos1, Vector2 fingerPos2, float delta)
	{
	}

	internal static void RaiseOnPinchEnd(Vector2 fingerPos1, Vector2 fingerPos2)
	{
	}

	internal static void RaiseOnRotationBegin(Vector2 fingerPos1, Vector2 fingerPos2)
	{
	}

	internal static void RaiseOnRotationMove(Vector2 fingerPos1, Vector2 fingerPos2, float rotationAngleDelta)
	{
	}

	internal static void RaiseOnRotationEnd(Vector2 fingerPos1, Vector2 fingerPos2, float totalRotationAngle)
	{
	}

	internal static void RaiseOnTwoFingerLongPress(Vector2 fingerPos)
	{
	}

	internal static void RaiseOnTwoFingerDragBegin(Vector2 fingerPos, Vector2 startPos)
	{
	}

	internal static void RaiseOnTwoFingerDragMove(Vector2 fingerPos, Vector2 delta)
	{
	}

	internal static void RaiseOnTwoFingerDragStationary(Vector2 fingerPos)
	{
	}

	internal static void RaiseOnTwoFingerDragEnd(Vector2 fingerPos)
	{
	}

	internal static void RaiseOnTwoFingerTap(Vector2 fingerPos)
	{
	}

	internal static void RaiseOnTwoFingerSwipe(Vector2 startPos, SwipeDirection direction, float velocity)
	{
	}

	public static Finger GetFinger(int index)
	{
		return null;
	}

	protected virtual void Awake()
	{
	}

	protected virtual void OnEnable()
	{
	}

	protected virtual void Start()
	{
	}

	protected virtual void OnDisable()
	{
	}

	protected virtual void Update()
	{
	}

	protected abstract FingerPhase GetPhase(Finger finger);

	protected abstract Vector2 GetPosition(Finger finger);

	private void InitFingers(int count)
	{
	}

	private void UpdateFingers()
	{
	}

	protected bool ShouldProcessTouch(int fingerIndex, Vector2 position)
	{
		return false;
	}

	private T CreateDefaultComponent<T>(T prefab, Transform parent) where T : FGComponent
	{
		return null;
	}

	private T CreateDefaultGlobalComponent<T>(T prefab) where T : FGComponent
	{
		return null;
	}

	private T CreateDefaultFingerComponent<T>(Finger finger, T prefab) where T : FGComponent
	{
		return null;
	}

	private Transform CreateNode(string name, Transform parent)
	{
		return null;
	}

	private void InitDefaultComponents()
	{
	}

	private void InitGlobalGestures()
	{
	}

	private void InitDefaultComponents(Finger finger)
	{
	}

	private static Finger GetFingerFromTouchFilter(GestureRecognizer recognizer)
	{
		return null;
	}

	private void PerFinger_OnDown(Finger source)
	{
	}

	private void PerFinger_OnUp(Finger source)
	{
	}

	private void PerFinger_OnStationaryBegin(FingerMotionDetector source)
	{
	}

	private void PerFinger_OnStationary(FingerMotionDetector source)
	{
	}

	private void PerFinger_OnStationaryEnd(FingerMotionDetector source)
	{
	}

	private void PerFinger_OnMoveBegin(FingerMotionDetector source)
	{
	}

	private void PerFinger_OnMove(FingerMotionDetector source)
	{
	}

	private void PerFinger_OnMoveEnd(FingerMotionDetector source)
	{
	}

	private void PerFinger_OnDragBegin(DragGestureRecognizer source)
	{
	}

	private void PerFinger_OnDragMove(DragGestureRecognizer source)
	{
	}

	private void PerFinger_OnDragStationary(DragGestureRecognizer source)
	{
	}

	private void PerFinger_OnDragEnd(DragGestureRecognizer source)
	{
	}

	private void PerFinger_OnLongPress(LongPressGestureRecognizer source)
	{
	}

	private void PerFinger_OnSwipe(SwipeGestureRecognizer source)
	{
	}

	private void PerFinger_OnTap(TapGestureRecognizer source)
	{
	}

	private void PerFinger_OnDoubleTap(MultiTapGestureRecognizer source)
	{
	}

	public static SwipeDirection GetSwipeDirection(Vector3 dir, float tolerance)
	{
		return default(SwipeDirection);
	}

	public static bool AllFingersMoving(params Finger[] fingers)
	{
		return false;
	}

	public static bool FingersMovedInOppositeDirections(Finger finger0, Finger finger1, float minDOT)
	{
		return false;
	}

	public static float SignedAngle(Vector2 from, Vector2 to)
	{
		return 0f;
	}
}
