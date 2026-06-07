using System;

public abstract class BaseTransitionBinding
{
	private Action<Session> action;

	protected void Initialize(Action<Session> action)
	{
	}

	public void Apply(Session session)
	{
	}
}
