using UnityEngine;

[AddComponentMenu("FingerGestures/Toolbox/FingerDown")]
public class TBFingerDown : TBComponent
{
	public Message message = new Message("OnFingerDown");

	public event EventHandler<TBFingerDown> OnFingerDown;

	public bool RaiseFingerDown(int fingerIndex, Vector2 fingerPos)
	{
		base.FingerIndex = fingerIndex;
		base.FingerPos = fingerPos;
		if (this.OnFingerDown != null)
		{
			this.OnFingerDown(this);
		}
		Send(message);
		return true;
	}
}
