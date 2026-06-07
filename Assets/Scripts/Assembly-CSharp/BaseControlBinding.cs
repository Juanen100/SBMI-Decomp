using System;

public abstract class BaseControlBinding : IControlBinding
{
	private Action<Session> action;

	private SBGUIButton button;

	private Action callback;

	private string label;

	private string targetSessionActionToken;

	public Action<Session> Action
	{
		get
		{
			return null;
		}
	}

	public SBGUIButton DynamicButton
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public Action Callback
	{
		get
		{
			return null;
		}
	}

	public string Label
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public virtual void DynamicUpdate(Session session)
	{
	}

	protected void Initialize(Action<Session> action, Action callback, string targetSessionActionToken)
	{
	}

	public string DecorateSessionActionId(uint ownerDid)
	{
		return null;
	}
}
