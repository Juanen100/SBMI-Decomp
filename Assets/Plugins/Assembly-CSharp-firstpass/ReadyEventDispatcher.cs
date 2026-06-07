using System;

public class ReadyEventDispatcher : EventDispatcher
{
	private bool ready;

	public bool IsReady
	{
		get
		{
			return false;
		}
	}

	public override void AddListener(Action value)
	{
	}

	public override void FireEvent()
	{
	}
}
