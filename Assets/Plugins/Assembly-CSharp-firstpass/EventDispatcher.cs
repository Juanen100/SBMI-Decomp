using System;

public class EventDispatcher<T, U>
{
	private event Action<T, U> eventListener;

	public Delegate[] GetInvocationList()
	{
		return this.eventListener.GetInvocationList();
	}

	public void AddListener(Action<T, U> value)
	{
		if (value != null)
		{
			if (this.eventListener == null)
			{
				this.eventListener = (Action<T, U>)Delegate.Combine(this.eventListener, value);
				return;
			}
			this.eventListener = (Action<T, U>)Delegate.Remove(this.eventListener, value);
			this.eventListener = (Action<T, U>)Delegate.Combine(this.eventListener, value);
		}
	}

	public void RemoveListener(Action<T, U> value)
	{
		if (this.eventListener != null)
		{
			this.eventListener = (Action<T, U>)Delegate.Remove(this.eventListener, value);
		}
	}

	public void ClearListeners()
	{
		this.eventListener = null;
	}

	public void FireEvent(T arg1, U arg2)
	{
		if (this.eventListener != null)
		{
			this.eventListener(arg1, arg2);
		}
	}
}
public class EventDispatcher<T, U, V>
{
	private event Action<T, U, V> eventListener;

	public Delegate[] GetInvocationList()
	{
		return this.eventListener.GetInvocationList();
	}

	public void AddListener(Action<T, U, V> value)
	{
		if (value != null)
		{
			if (this.eventListener == null)
			{
				this.eventListener = (Action<T, U, V>)Delegate.Combine(this.eventListener, value);
				return;
			}
			this.eventListener = (Action<T, U, V>)Delegate.Remove(this.eventListener, value);
			this.eventListener = (Action<T, U, V>)Delegate.Combine(this.eventListener, value);
		}
	}

	public void RemoveListener(Action<T, U, V> value)
	{
		if (this.eventListener != null)
		{
			this.eventListener = (Action<T, U, V>)Delegate.Remove(this.eventListener, value);
		}
	}

	public void ClearListeners()
	{
		this.eventListener = null;
	}

	public void FireEvent(T arg1, U arg2, V arg3)
	{
		if (this.eventListener != null)
		{
			this.eventListener(arg1, arg2, arg3);
		}
	}
}
public class EventDispatcher<T>
{
	private event Action<T> eventListener;

	public void SetListener(Action<T> value)
	{
		this.eventListener = value;
	}

	public Action<T> GetListener()
	{
		return this.eventListener;
	}

	public void AddListener(Action<T> value)
	{
		if (value != null)
		{
			if (this.eventListener == null)
			{
				this.eventListener = (Action<T>)Delegate.Combine(this.eventListener, value);
				return;
			}
			this.eventListener = (Action<T>)Delegate.Remove(this.eventListener, value);
			this.eventListener = (Action<T>)Delegate.Combine(this.eventListener, value);
		}
	}

	public void RemoveListener(Action<T> value)
	{
		if (this.eventListener != null)
		{
			this.eventListener = (Action<T>)Delegate.Remove(this.eventListener, value);
		}
	}

	public void ClearListeners()
	{
		this.eventListener = null;
	}

	public void FireEvent(T message)
	{
		if (this.eventListener != null)
		{
			this.eventListener(message);
		}
	}
}
public class EventDispatcher
{
	public bool HasListeners
	{
		get
		{
			return this.eventListener != null;
		}
	}

	private event Action eventListener;

	public virtual void AddListener(Action value)
	{
		if (value != null)
		{
			if (this.eventListener == null)
			{
				this.eventListener = (Action)Delegate.Combine(this.eventListener, value);
				return;
			}
			this.eventListener = (Action)Delegate.Remove(this.eventListener, value);
			this.eventListener = (Action)Delegate.Combine(this.eventListener, value);
		}
	}

	public virtual void RemoveListener(Action value)
	{
		if (this.eventListener != null)
		{
			this.eventListener = (Action)Delegate.Remove(this.eventListener, value);
		}
	}

	public void ClearListeners()
	{
		this.eventListener = null;
	}

	public virtual void FireEvent()
	{
		if (this.eventListener != null)
		{
			this.eventListener();
		}
	}
}
