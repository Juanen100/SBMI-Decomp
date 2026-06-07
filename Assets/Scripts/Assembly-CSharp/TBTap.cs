using UnityEngine;

public class TBTap : TBComponent
{
	public enum TapMode
	{
		SingleTap = 0,
		DoubleTap = 1
	}

	public TapMode tapMode;

	public Message message;

	public event EventHandler<TBTap> OnTap
	{
		add
		{
		}
		remove
		{
		}
	}

	public bool RaiseTap(int fingerIndex, Vector2 fingerPos)
	{
		return false;
	}
}
