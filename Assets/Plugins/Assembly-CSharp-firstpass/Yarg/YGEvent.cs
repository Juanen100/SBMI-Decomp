using System.Collections.Generic;
using MiniJSON;
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

		private float holdStartTime = -1f;

		public bool Flick
		{
			get
			{
				float num = deltaPosition.magnitude / deltaTime;
				if (num > 400f)
				{
					if (Mathf.Abs(deltaPosition.x) > Mathf.Abs(deltaPosition.y))
					{
						direction = ((!(deltaPosition.x < 0f)) ? DIRECTION.RIGHT : DIRECTION.LEFT);
					}
					else
					{
						direction = ((deltaPosition.y < 0f) ? DIRECTION.UP : DIRECTION.DOWN);
					}
					return true;
				}
				return false;
			}
		}

		public bool Hold
		{
			get
			{
				if (holdStartTime < 0f)
				{
					return false;
				}
				if ((startPosition - position).sqrMagnitude > 64f)
				{
					holdStartTime = -1f;
					return false;
				}
				if (Time.time - holdStartTime >= 1f)
				{
					type = TYPE.HOLD;
					holdStartTime = -1f;
					return true;
				}
				return false;
			}
		}

		public YGEvent()
		{
			holdStartTime = Time.time;
		}

		public YGEvent(Touch t)
			: this()
		{
			fingerId = t.fingerId;
			startPosition = (position = t.position);
			deltaPosition = t.deltaPosition;
			deltaPosition.y *= -1f;
			deltaTime = t.deltaTime;
			startTime = Time.time;
			tapCount = t.tapCount;
			switch (t.phase)
			{
			case TouchPhase.Began:
				type = TYPE.TOUCH_BEGIN;
				break;
			case TouchPhase.Canceled:
				type = TYPE.TOUCH_CANCEL;
				break;
			case TouchPhase.Ended:
				type = TYPE.TOUCH_END;
				break;
			case TouchPhase.Moved:
				type = TYPE.TOUCH_MOVE;
				break;
			case TouchPhase.Stationary:
				type = TYPE.TOUCH_STAY;
				break;
			}
		}

		public YGEvent(Event e)
			: this()
		{
			fingerId = 98;
			if (e != null && e.isMouse)
			{
				startPosition = (position = e.mousePosition);
				deltaPosition = e.delta;
				startTime = Time.time;
				deltaTime = Time.deltaTime;
				tapCount = e.clickCount;
				type = TYPE.NULL;
			}
		}

		public YGEvent(YGEvent y)
			: this()
		{
			if (y != null)
			{
				fingerId = y.fingerId;
				position = y.position;
				deltaPosition = y.deltaPosition;
				startPosition = y.startPosition;
				startTime = Time.time;
				deltaTime = y.deltaTime;
				tapCount = y.tapCount;
				type = y.type;
				used = y.used;
				holdStartTime = y.holdStartTime;
				direction = y.direction;
			}
		}

		public YGEvent Update(YGEvent y)
		{
			position = y.position;
			deltaPosition = y.deltaPosition;
			deltaTime = y.deltaTime;
			tapCount = y.tapCount;
			type = y.type;
			used = y.used;
			y.startPosition = startPosition;
			y.startTime = startTime;
			y.holdStartTime = holdStartTime;
			direction = y.direction;
			return y;
		}

		public void UpdateFromMouseInput()
		{
			startPosition = (position = Input.mousePosition);
			int num = 0;
			if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
			{
				type = TYPE.TOUCH_BEGIN;
				num = 1;
			}
			else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
			{
				type = TYPE.TOUCH_END;
				num = 1;
			}
			else if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
			{
				if (deltaPosition != Vector2.zero)
				{
					type = TYPE.TOUCH_MOVE;
				}
				else
				{
					type = TYPE.TOUCH_STAY;
				}
				num = 1;
			}
			else
			{
				type = TYPE.NULL;
			}
			if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonUp(1) || Input.GetMouseButton(1))
			{
				fingerId = 99;
			}
			touchCount = num;
		}

		public override string ToString()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["fingerId"] = fingerId;
			dictionary["position"] = position.ToString();
			dictionary["deltaPosition"] = deltaPosition.ToString();
			dictionary["startPosition"] = startPosition.ToString();
			dictionary["deltaTime"] = deltaTime;
			dictionary["startTime"] = startTime;
			dictionary["holdStartTime"] = holdStartTime;
			dictionary["tapCount"] = tapCount;
			dictionary["tapCount"] = tapCount;
			dictionary["type"] = type.ToString();
			dictionary["direction"] = direction.ToString();
			dictionary["touchCount"] = touchCount;
			dictionary["used"] = used;
			return Json.Serialize(dictionary);
		}
	}
}
