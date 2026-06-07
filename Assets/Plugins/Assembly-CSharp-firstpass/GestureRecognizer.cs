public abstract class GestureRecognizer : FGComponent
{
	public enum GestureState
	{
		Ready = 0,
		InProgress = 1,
		Failed = 2,
		Recognized = 3
	}

	public enum GestureResetMode
	{
		NextFrame = 0,
		EndOfTouchSequence = 1,
		StartOfTouchSequence = 2
	}

	public delegate bool CanBeginDelegate(GestureRecognizer gr, FingerGestures.IFingerList touches);

	private GestureState prevState;

	private GestureState state;

	private float startTime;

	public GestureResetMode ResetMode;

	private int lastTouchesCount;

	private CanBeginDelegate canBeginDelegate;

	private FingerGestures.ITouchFilter touchFilter;

	public GestureState PreviousState
	{
		get
		{
			return default(GestureState);
		}
	}

	public GestureState State
	{
		get
		{
			return default(GestureState);
		}
		protected set
		{
		}
	}

	public bool IsActive
	{
		get
		{
			return false;
		}
	}

	public float StartTime
	{
		get
		{
			return 0f;
		}
		protected set
		{
		}
	}

	public float ElapsedTime
	{
		get
		{
			return 0f;
		}
	}

	public FingerGestures.ITouchFilter TouchFilter
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public event EventDelegate<GestureRecognizer> OnStateChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	protected virtual void Reset()
	{
	}

	protected override void Start()
	{
	}

	protected virtual void OnTouchSequenceStarted()
	{
	}

	protected virtual void OnTouchSequenceEnded()
	{
	}

	protected override void OnUpdate(FingerGestures.IFingerList touches)
	{
	}

	protected virtual GestureState OnReady(FingerGestures.IFingerList touches)
	{
		return default(GestureState);
	}

	protected virtual bool ShouldFailFromReady(FingerGestures.IFingerList touches)
	{
		return false;
	}

	protected virtual bool CanBegin(FingerGestures.IFingerList touches)
	{
		return false;
	}

	public virtual bool CheckCanBeginDelegate(FingerGestures.IFingerList touches)
	{
		return false;
	}

	public void SetCanBeginDelegate(CanBeginDelegate f)
	{
	}

	public CanBeginDelegate GetCanBeginDelegate()
	{
		return null;
	}

	protected abstract int GetRequiredFingerCount();

	protected abstract void OnBegin(FingerGestures.IFingerList touches);

	protected abstract GestureState OnActive(FingerGestures.IFingerList touches);

	protected bool Young(FingerGestures.IFingerList touches)
	{
		return false;
	}
}
