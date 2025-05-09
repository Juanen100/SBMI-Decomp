using UnityEngine;

namespace Yarg
{
	public interface ITouchable
	{
		Transform tform { get; }

		bool TouchEvent(YGEvent touch);
	}
}
