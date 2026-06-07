using System;
using UnityEngine;

public abstract class TBComponent : MonoBehaviour
{
	public delegate void EventHandler<T>(T sender) where T : TBComponent;

	[Serializable]
	public class Message
	{
		public bool enabled;

		public string methodName;

		public GameObject target;

		public Message()
		{
		}

		public Message(string methodName)
		{
		}

		public Message(string methodName, bool enabled)
		{
		}
	}

	private int fingerIndex;

	private Vector2 fingerPos;

	public int FingerIndex
	{
		get
		{
			return 0;
		}
		protected set
		{
		}
	}

	public Vector2 FingerPos
	{
		get
		{
			return default(Vector2);
		}
		protected set
		{
		}
	}

	protected virtual void Start()
	{
	}

	protected bool Send(Message msg)
	{
		return false;
	}
}
