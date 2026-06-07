using System;

namespace Yarg
{
	public class YGEventDispatcher
	{
		private event Func<YGEvent, bool> eventListener
		{
			add
			{
			}
			remove
			{
			}
		}

		public void AddListener(Func<YGEvent, bool> value)
		{
		}

		public void RemoveListener(Func<YGEvent, bool> value)
		{
		}

		public void ClearListeners()
		{
		}

		public bool FireEvent(YGEvent evt)
		{
			return false;
		}
	}
}
