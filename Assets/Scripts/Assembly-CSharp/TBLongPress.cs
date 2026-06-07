using UnityEngine;

public class TBLongPress : TBComponent
{
	public Message message;

	public event EventHandler<TBLongPress> OnLongPress
	{
		add
		{
		}
		remove
		{
		}
	}

	public bool RaiseLongPress(int fingerIndex, Vector2 fingerPos)
	{
		return false;
	}
}
