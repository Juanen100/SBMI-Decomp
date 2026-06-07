using UnityEngine;

public class FingerMotionDetector : FGComponent
{
	public enum MotionState
	{
		None = 0,
		Stationary = 1,
		Moving = 2
	}

	public float MoveThreshold;

	private FingerGestures.Finger finger;

	private MotionState state;

	private MotionState prevState;

	private int moves;

	private float stationaryStartTime;

	private Vector2 anchorPos;

	private bool wasDown;

	public virtual FingerGestures.Finger Finger
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	protected MotionState State
	{
		get
		{
			return default(MotionState);
		}
		private set
		{
		}
	}

	protected MotionState PreviousState
	{
		get
		{
			return default(MotionState);
		}
		private set
		{
		}
	}

	public int Moves
	{
		get
		{
			return 0;
		}
		private set
		{
		}
	}

	public bool Moved
	{
		get
		{
			return false;
		}
	}

	public bool WasMoving
	{
		get
		{
			return false;
		}
	}

	public bool Moving
	{
		get
		{
			return false;
		}
	}

	public float ElapsedStationaryTime
	{
		get
		{
			return 0f;
		}
	}

	public Vector2 AnchorPos
	{
		get
		{
			return default(Vector2);
		}
		private set
		{
		}
	}

	public event EventDelegate<FingerMotionDetector> OnMoveBegin
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventDelegate<FingerMotionDetector> OnMove
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventDelegate<FingerMotionDetector> OnMoveEnd
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventDelegate<FingerMotionDetector> OnStationaryBegin
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventDelegate<FingerMotionDetector> OnStationary
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventDelegate<FingerMotionDetector> OnStationaryEnd
	{
		add
		{
		}
		remove
		{
		}
	}

	protected override void OnUpdate(FingerGestures.IFingerList touches)
	{
	}

	private void RaiseEvents()
	{
	}

	protected void RaiseOnMoveBegin()
	{
	}

	protected void RaiseOnMove()
	{
	}

	protected void RaiseOnMoveEnd()
	{
	}

	protected void RaiseOnStationaryBegin()
	{
	}

	protected void RaiseOnStationary()
	{
	}

	protected void RaiseOnStationaryEnd()
	{
	}
}
