using UnityEngine;

[AddComponentMenu("FingerGestures/Toolbox/LongPress")]
public class TBLongPress : TBComponent
{
	public Message message = new Message("OnLongPress");

	public event EventHandler<TBLongPress> OnLongPress;

	public bool RaiseLongPress(int fingerIndex, Vector2 fingerPos)
	{
		base.FingerIndex = fingerIndex;
		base.FingerPos = fingerPos;
		if (this.OnLongPress != null)
		{
			this.OnLongPress(this);
		}
		Send(message);
		return true;
	}
}
