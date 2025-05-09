using System;
using UnityEngine;

public abstract class TBComponent : MonoBehaviour
{
	[Serializable]
	public class Message
	{
		public bool enabled = true;

		public string methodName = "MethodToCall";

		public GameObject target;

		public Message()
		{
		}

		public Message(string methodName)
		{
			this.methodName = methodName;
		}

		public Message(string methodName, bool enabled)
		{
			this.enabled = enabled;
			this.methodName = methodName;
		}
	}

	public delegate void EventHandler<T>(T sender) where T : TBComponent;

	private int fingerIndex = -1;

	private Vector2 fingerPos;

	public int FingerIndex
	{
		get
		{
			return fingerIndex;
		}
		protected set
		{
			fingerIndex = value;
		}
	}

	public Vector2 FingerPos
	{
		get
		{
			return fingerPos;
		}
		protected set
		{
			fingerPos = value;
		}
	}

	protected virtual void Start()
	{
		if (!base.GetComponent<Collider>())
		{
			Debug.LogError(base.name + " must have a valid collider.");
			base.enabled = false;
		}
	}

	protected bool Send(Message msg)
	{
		if (!base.enabled)
		{
			return false;
		}
		if (!msg.enabled)
		{
			return false;
		}
		GameObject target = msg.target;
		if (!target)
		{
			target = base.gameObject;
		}
		target.SendMessage(msg.methodName, SendMessageOptions.DontRequireReceiver);
		return true;
	}
}
