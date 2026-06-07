using UnityEngine;

public class TBDrag : TBComponent
{
	public Message dragBeginMessage;

	public Message dragMoveMessage;

	public Message dragEndMessage;

	private bool dragging;

	private Vector2 moveDelta;

	public bool Dragging
	{
		get
		{
			return false;
		}
		private set
		{
		}
	}

	public Vector2 MoveDelta
	{
		get
		{
			return default(Vector2);
		}
		private set
		{
		}
	}

	public event EventHandler<TBDrag> OnDragBegin
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler<TBDrag> OnDragMove
	{
		add
		{
		}
		remove
		{
		}
	}

	public event EventHandler<TBDrag> OnDragEnd
	{
		add
		{
		}
		remove
		{
		}
	}

	public bool BeginDrag(int fingerIndex, Vector2 fingerPos)
	{
		return false;
	}

	public bool EndDrag()
	{
		return false;
	}

	private void FingerGestures_OnDragMove(int fingerIndex, Vector2 fingerPos, Vector2 delta)
	{
	}

	private void FingerGestures_OnDragEnd(int fingerIndex, Vector2 fingerPos)
	{
	}

	private void OnDisable()
	{
	}
}
