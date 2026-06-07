using UnityEngine;

public class TBFingerDown : TBComponent
{
	public Message message;

	public event EventHandler<TBFingerDown> OnFingerDown
	{
		add
		{
		}
		remove
		{
		}
	}

	public bool RaiseFingerDown(int fingerIndex, Vector2 fingerPos)
	{
		return false;
	}
}
