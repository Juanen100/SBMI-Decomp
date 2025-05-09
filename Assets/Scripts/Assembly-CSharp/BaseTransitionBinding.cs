using System;

public abstract class BaseTransitionBinding
{
	private Action<Session> action;

	protected void Initialize(Action<Session> action)
	{
		this.action = action;
	}

	public void Apply(Session session)
	{
		action(session);
	}
}
