using System;

public class EventDispatcher<T, U>
{
	private event Action<T, U> eventListener
	{
		add
		{
		}
		remove
		{
		}
	}

	public Delegate[] GetInvocationList()
	{
		return null;
	}

	public void AddListener(Action<T, U> value)
	{
	}

	public void RemoveListener(Action<T, U> value)
	{
	}

	public void ClearListeners()
	{
	}

	public void FireEvent(T arg1, U arg2)
	{
	}
}
public class EventDispatcher<T, U, V>
{
	private event Action<T, U, V> eventListener
	{
		add
		{
		}
		remove
		{
		}
	}

	public Delegate[] GetInvocationList()
	{
		return null;
	}

	public void AddListener(Action<T, U, V> value)
	{
	}

	public void RemoveListener(Action<T, U, V> value)
	{
	}

	public void ClearListeners()
	{
	}

	public void FireEvent(T arg1, U arg2, V arg3)
	{
	}
}
public class EventDispatcher<T>
{
	private event Action<T> eventListener
	{
		add
		{
		}
		remove
		{
		}
	}

	public void SetListener(Action<T> value)
	{
	}

	public Action<T> GetListener()
	{
		return null;
	}

	public void AddListener(Action<T> value)
	{
	}

	public void RemoveListener(Action<T> value)
	{
	}

	public void ClearListeners()
	{
	}

	public void FireEvent(T message)
	{
	}
}
public class EventDispatcher
{
	public bool HasListeners
	{
		get
		{
			return false;
		}
	}

	private event Action eventListener
	{
		add
		{
		}
		remove
		{
		}
	}

	public virtual void AddListener(Action value)
	{
	}

	public virtual void RemoveListener(Action value)
	{
	}

	public void ClearListeners()
	{
	}

	public virtual void FireEvent()
	{
	}
}
