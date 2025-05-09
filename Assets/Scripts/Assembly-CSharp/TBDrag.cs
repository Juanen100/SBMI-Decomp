using UnityEngine;

[AddComponentMenu("FingerGestures/Toolbox/Drag")]
public class TBDrag : TBComponent
{
	public Message dragBeginMessage = new Message("OnDragBegin");

	public Message dragMoveMessage = new Message("OnDragMove", false);

	public Message dragEndMessage = new Message("OnDragEnd");

	private bool dragging;

	private Vector2 moveDelta;

	public bool Dragging
	{
		get
		{
			return dragging;
		}
		private set
		{
			if (dragging != value)
			{
				dragging = value;
				if (dragging)
				{
					FingerGestures.OnFingerDragMove += FingerGestures_OnDragMove;
					FingerGestures.OnFingerDragEnd += FingerGestures_OnDragEnd;
				}
				else
				{
					FingerGestures.OnFingerDragMove -= FingerGestures_OnDragMove;
					FingerGestures.OnFingerDragEnd -= FingerGestures_OnDragEnd;
				}
			}
		}
	}

	public Vector2 MoveDelta
	{
		get
		{
			return moveDelta;
		}
		private set
		{
			moveDelta = value;
		}
	}

	public event EventHandler<TBDrag> OnDragBegin;

	public event EventHandler<TBDrag> OnDragMove;

	public event EventHandler<TBDrag> OnDragEnd;

	public bool BeginDrag(int fingerIndex, Vector2 fingerPos)
	{
		if (Dragging)
		{
			return false;
		}
		base.FingerIndex = fingerIndex;
		base.FingerPos = fingerPos;
		Dragging = true;
		if (this.OnDragBegin != null)
		{
			this.OnDragBegin(this);
		}
		Send(dragBeginMessage);
		return true;
	}

	public bool EndDrag()
	{
		if (!Dragging)
		{
			return false;
		}
		if (this.OnDragEnd != null)
		{
			this.OnDragEnd(this);
		}
		Send(dragEndMessage);
		Dragging = false;
		base.FingerIndex = -1;
		return true;
	}

	private void FingerGestures_OnDragMove(int fingerIndex, Vector2 fingerPos, Vector2 delta)
	{
		if (Dragging && base.FingerIndex == fingerIndex)
		{
			base.FingerPos = fingerPos;
			MoveDelta = delta;
			if (this.OnDragMove != null)
			{
				this.OnDragMove(this);
			}
			Send(dragMoveMessage);
		}
	}

	private void FingerGestures_OnDragEnd(int fingerIndex, Vector2 fingerPos)
	{
		if (Dragging && base.FingerIndex == fingerIndex)
		{
			base.FingerPos = fingerPos;
			EndDrag();
		}
	}

	private void OnDisable()
	{
		if (Dragging)
		{
			EndDrag();
		}
	}
}
