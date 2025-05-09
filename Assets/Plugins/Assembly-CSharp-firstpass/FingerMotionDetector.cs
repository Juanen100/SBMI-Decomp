using UnityEngine;

public class FingerMotionDetector : FGComponent
{
	public enum MotionState
	{
		None = 0,
		Stationary = 1,
		Moving = 2
	}

	public float MoveThreshold = 5f;

	private FingerGestures.Finger finger;

	private MotionState state;

	private MotionState prevState;

	private int moves;

	private float stationaryStartTime;

	private Vector2 anchorPos = Vector2.zero;

	private bool wasDown;

	public virtual FingerGestures.Finger Finger
	{
		get
		{
			return finger;
		}
		set
		{
			finger = value;
		}
	}

	protected MotionState State
	{
		get
		{
			return state;
		}
		private set
		{
			state = value;
		}
	}

	protected MotionState PreviousState
	{
		get
		{
			return prevState;
		}
		private set
		{
			prevState = value;
		}
	}

	public int Moves
	{
		get
		{
			return moves;
		}
		private set
		{
			moves = value;
		}
	}

	public bool Moved
	{
		get
		{
			return Moves > 0;
		}
	}

	public bool WasMoving
	{
		get
		{
			return PreviousState == MotionState.Moving;
		}
	}

	public bool Moving
	{
		get
		{
			return State == MotionState.Moving;
		}
	}

	public float ElapsedStationaryTime
	{
		get
		{
			return Time.time - stationaryStartTime;
		}
	}

	public Vector2 AnchorPos
	{
		get
		{
			return anchorPos;
		}
		private set
		{
			anchorPos = value;
		}
	}

	public event EventDelegate<FingerMotionDetector> OnMoveBegin;

	public event EventDelegate<FingerMotionDetector> OnMove;

	public event EventDelegate<FingerMotionDetector> OnMoveEnd;

	public event EventDelegate<FingerMotionDetector> OnStationaryBegin;

	public event EventDelegate<FingerMotionDetector> OnStationary;

	public event EventDelegate<FingerMotionDetector> OnStationaryEnd;

	protected override void OnUpdate(FingerGestures.IFingerList touches)
	{
		if (Finger.IsDown)
		{
			if (!wasDown)
			{
				Moves = 0;
				AnchorPos = Finger.Position;
				State = MotionState.Stationary;
			}
			if (Finger.Phase == FingerGestures.FingerPhase.Moved)
			{
				if (State != MotionState.Moving)
				{
					if ((Finger.Position - AnchorPos).sqrMagnitude >= MoveThreshold * MoveThreshold)
					{
						State = MotionState.Moving;
					}
					else
					{
						State = MotionState.Stationary;
					}
				}
			}
			else
			{
				State = MotionState.Stationary;
			}
		}
		else
		{
			State = MotionState.None;
		}
		RaiseEvents();
		PreviousState = State;
		wasDown = Finger.IsDown;
	}

	private void RaiseEvents()
	{
		if (State != PreviousState)
		{
			if (PreviousState == MotionState.Moving)
			{
				RaiseOnMoveEnd();
				AnchorPos = Finger.Position;
			}
			else if (PreviousState == MotionState.Stationary)
			{
				RaiseOnStationaryEnd();
			}
			if (State == MotionState.Moving)
			{
				RaiseOnMoveBegin();
				Moves++;
			}
			else if (State == MotionState.Stationary)
			{
				stationaryStartTime = Time.time;
				RaiseOnStationaryBegin();
			}
		}
		if (State == MotionState.Stationary)
		{
			RaiseOnStationary();
		}
		else if (State == MotionState.Moving)
		{
			RaiseOnMove();
		}
	}

	protected void RaiseOnMoveBegin()
	{
		if (this.OnMoveBegin != null)
		{
			this.OnMoveBegin(this);
		}
	}

	protected void RaiseOnMove()
	{
		if (this.OnMove != null)
		{
			this.OnMove(this);
		}
	}

	protected void RaiseOnMoveEnd()
	{
		if (this.OnMoveEnd != null)
		{
			this.OnMoveEnd(this);
		}
	}

	protected void RaiseOnStationaryBegin()
	{
		if (this.OnStationaryBegin != null)
		{
			this.OnStationaryBegin(this);
		}
	}

	protected void RaiseOnStationary()
	{
		if (this.OnStationary != null)
		{
			this.OnStationary(this);
		}
	}

	protected void RaiseOnStationaryEnd()
	{
		if (this.OnStationaryEnd != null)
		{
			this.OnStationaryEnd(this);
		}
	}
}
