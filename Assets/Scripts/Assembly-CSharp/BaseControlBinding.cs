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
			return action;
		}
	}

	public SBGUIButton DynamicButton
	{
		get
		{
			return button;
		}
		set
		{
			button = value;
		}
	}

	public Action Callback
	{
		get
		{
			return callback;
		}
	}

	public string Label
	{
		get
		{
			return label;
		}
		set
		{
			label = value;
		}
	}

	public virtual void DynamicUpdate(Session session)
	{
	}

	protected void Initialize(Action<Session> action, Action callback, string targetSessionActionToken)
	{
		this.action = action;
		this.callback = callback;
		this.targetSessionActionToken = targetSessionActionToken;
	}

	public string DecorateSessionActionId(uint ownerDid)
	{
		return SessionActionSimulationHelper.DecorateSessionActionId(ownerDid, targetSessionActionToken);
	}
}
