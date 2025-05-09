using System;

namespace Yarg
{
	public class YGEventDispatcher
	{
		private event Func<YGEvent, bool> eventListener;

		public void AddListener(Func<YGEvent, bool> value)
		{
			if (value != null)
			{
				if (this.eventListener == null)
				{
					this.eventListener = (Func<YGEvent, bool>)Delegate.Combine(this.eventListener, value);
					return;
				}
				this.eventListener = (Func<YGEvent, bool>)Delegate.Remove(this.eventListener, value);
				this.eventListener = (Func<YGEvent, bool>)Delegate.Combine(this.eventListener, value);
			}
		}

		public void RemoveListener(Func<YGEvent, bool> value)
		{
			if (this.eventListener != null)
			{
				this.eventListener = (Func<YGEvent, bool>)Delegate.Remove(this.eventListener, value);
			}
		}

		public void ClearListeners()
		{
			this.eventListener = null;
		}

		public bool FireEvent(YGEvent evt)
		{
			bool flag = false;
			if (this.eventListener == null)
			{
				return flag;
			}
			Delegate[] invocationList = this.eventListener.GetInvocationList();
			if (invocationList == null)
			{
				return flag;
			}
			Delegate[] array = invocationList;
			foreach (Delegate obj in array)
			{
				flag |= ((Func<YGEvent, bool>)obj)(evt);
			}
			return flag;
		}
	}
}
