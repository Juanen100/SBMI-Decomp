using UnityEngine;

namespace Yarg
{
	public class YGEvent
	{
		public enum TYPE
		{
			NULL = 0,
			TOUCH_BEGIN = 1,
			TOUCH_END = 2,
			TOUCH_CANCEL = 3,
			TOUCH_STAY = 4,
			TOUCH_MOVE = 5,
			HOVER = 6,
			DRAG = 7,
			FLICK = 8,
			SWIPE = 9,
			PINCH = 10,
			TAP = 11,
			RESET = 12,
			HOLD = 13,
			DISABLE = 14,
			ENABLE = 15
		}

		public enum DIRECTION
		{
			NULL = 0,
			UP = 1,
			DOWN = 2,
			LEFT = 3,
			RIGHT = 4
		}

		public const int MOUSE_LEFT = 98;

		public const int MOUSE_RIGHT = 99;

		public const float HOLD_DURATION = 1f;

		public const float HOLD_DRIFT_RADIUS_SQUARED = 64f;

		public int fingerId;

		public Vector2 position;

		public Vector2 deltaPosition;

		public Vector2 startPosition;

		public float distance;

		public float deltaTime;

		public float startTime;

		public int tapCount;

		public TYPE type;

		public DIRECTION direction;

		public static int touchCount;

		public object param;

		public bool used;

		private float holdStartTime;

		public bool Flick
		{
			get
			{
				return false;
			}
		}

		public bool Hold
		{
			get
			{
				return false;
			}
		}

		public YGEvent()
		{
		}

		public YGEvent(Touch t)
		{
		}

		public YGEvent(Event e)
		{
		}

		public YGEvent(YGEvent y)
		{
		}

		public YGEvent Update(YGEvent y)
		{
			return null;
		}

		public void UpdateFromMouseInput()
		{
		}

		public override string ToString()
		{
			return null;
		}
	}
}
