using UnityEngine;

[AddComponentMenu("FingerGestures/Toolbox/Tap")]
public class TBTap : TBComponent
{
	public enum TapMode
	{
		SingleTap = 0,
		DoubleTap = 1
	}

	public TapMode tapMode;

	public Message message = new Message("OnTap");

	public event EventHandler<TBTap> OnTap;

	public bool RaiseTap(int fingerIndex, Vector2 fingerPos)
	{
		base.FingerIndex = fingerIndex;
		base.FingerPos = fingerPos;
		if (this.OnTap != null)
		{
			this.OnTap(this);
		}
		Send(message);
		return true;
	}
}
