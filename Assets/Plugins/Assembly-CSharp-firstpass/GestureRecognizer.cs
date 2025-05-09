using UnityEngine;

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

	public GestureResetMode ResetMode = GestureResetMode.StartOfTouchSequence;

	private int lastTouchesCount;

	private CanBeginDelegate canBeginDelegate;

	private FingerGestures.ITouchFilter touchFilter;

	public GestureState PreviousState
	{
		get
		{
			return prevState;
		}
	}

	public GestureState State
	{
		get
		{
			return state;
		}
		protected set
		{
			if (state != value)
			{
				prevState = state;
				state = value;
				if (this.OnStateChanged != null)
				{
					this.OnStateChanged(this);
				}
			}
		}
	}

	public bool IsActive
	{
		get
		{
			return State == GestureState.InProgress;
		}
	}

	public float StartTime
	{
		get
		{
			return startTime;
		}
		protected set
		{
			startTime = value;
		}
	}

	public float ElapsedTime
	{
		get
		{
			return Time.time - startTime;
		}
	}

	public FingerGestures.ITouchFilter TouchFilter
	{
		get
		{
			return touchFilter;
		}
		set
		{
			touchFilter = value;
		}
	}

	public event EventDelegate<GestureRecognizer> OnStateChanged;

	protected virtual void Reset()
	{
		State = GestureState.Ready;
	}

	protected override void Start()
	{
		base.Start();
		Reset();
	}

	protected virtual void OnTouchSequenceStarted()
	{
		if (ResetMode == GestureResetMode.StartOfTouchSequence && (State == GestureState.Recognized || State == GestureState.Failed))
		{
			Reset();
		}
	}

	protected virtual void OnTouchSequenceEnded()
	{
		if (ResetMode == GestureResetMode.EndOfTouchSequence && (State == GestureState.Recognized || State == GestureState.Failed))
		{
			Reset();
		}
	}

	protected override void OnUpdate(FingerGestures.IFingerList touches)
	{
		if (touchFilter != null)
		{
			touches = touchFilter.Apply(touches);
		}
		if (touches.Count > 0 && lastTouchesCount == 0)
		{
			OnTouchSequenceStarted();
		}
		switch (State)
		{
		case GestureState.Failed:
		case GestureState.Recognized:
			if (ResetMode == GestureResetMode.NextFrame)
			{
				Reset();
			}
			break;
		case GestureState.Ready:
			State = OnReady(touches);
			break;
		case GestureState.InProgress:
			State = OnActive(touches);
			break;
		default:
			Debug.LogError(string.Concat(this, " - Unhandled state: ", State, ". Failing recognizer."));
			State = GestureState.Failed;
			break;
		}
		if (touches.Count == 0 && lastTouchesCount > 0)
		{
			OnTouchSequenceEnded();
		}
		lastTouchesCount = touches.Count;
	}

	protected virtual GestureState OnReady(FingerGestures.IFingerList touches)
	{
		if (ShouldFailFromReady(touches))
		{
			return GestureState.Failed;
		}
		if (CanBegin(touches))
		{
			StartTime = Time.time;
			OnBegin(touches);
			return GestureState.InProgress;
		}
		return GestureState.Ready;
	}

	protected virtual bool ShouldFailFromReady(FingerGestures.IFingerList touches)
	{
		if (touches.Count != GetRequiredFingerCount() && touches.Count > 0 && !Young(touches))
		{
			return true;
		}
		return false;
	}

	protected virtual bool CanBegin(FingerGestures.IFingerList touches)
	{
		if (touches.Count != GetRequiredFingerCount())
		{
			return false;
		}
		if (!CheckCanBeginDelegate(touches))
		{
			return false;
		}
		return true;
	}

	public virtual bool CheckCanBeginDelegate(FingerGestures.IFingerList touches)
	{
		if (canBeginDelegate != null && !canBeginDelegate(this, touches))
		{
			return false;
		}
		return true;
	}

	public void SetCanBeginDelegate(CanBeginDelegate f)
	{
		canBeginDelegate = f;
	}

	public CanBeginDelegate GetCanBeginDelegate()
	{
		return canBeginDelegate;
	}

	protected abstract int GetRequiredFingerCount();

	protected abstract void OnBegin(FingerGestures.IFingerList touches);

	protected abstract GestureState OnActive(FingerGestures.IFingerList touches);

	protected bool Young(FingerGestures.IFingerList touches)
	{
		FingerGestures.Finger oldest = touches.GetOldest();
		if (oldest == null)
		{
			return false;
		}
		float num = Time.time - oldest.StarTime;
		return num < 0.25f;
	}
}
