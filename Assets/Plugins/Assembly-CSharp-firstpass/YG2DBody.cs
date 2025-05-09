using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using UnityEngine;
using Yarg;

[ExecuteInEditMode]
public abstract class YG2DBody : MonoBehaviour, ITouchable
{
	public float density = 1f;

	public BodyType bodyType = BodyType.Kinematic;

	protected Body body;

	public Vector2 offset = Vector2.zero;

	public YGEventDispatcher EventDispatch = new YGEventDispatcher();

	protected YG2DWorld yargWorld;

	protected World world;

	private YGSprite sprite;

	[NonSerialized]
	protected Transform _tform;

	[NonSerialized]
	protected GUIView _view;

	protected bool touchInProgress;

	protected YG2DWorld YargWorld
	{
		get
		{
			if (yargWorld == null)
			{
				Transform parent = tform;
				do
				{
					if (parent == null)
					{
						yargWorld = GUIMainView.GetInstance()._2DWorld;
						if (yargWorld == null)
						{
							Debug.LogError(string.Format("{0} couldn't find 2d world", base.gameObject.name));
						}
						break;
					}
					yargWorld = parent.GetComponent<YG2DWorld>();
					parent = parent.parent;
				}
				while (yargWorld == null);
			}
			return yargWorld;
		}
	}

	public Body Body
	{
		get
		{
			if (body == null)
			{
				Debug.LogWarning(string.Format("No body has been created for {0}", base.gameObject.name));
			}
			return body;
		}
	}

	public Transform tform
	{
		get
		{
			return (!(_tform != null)) ? (_tform = base.transform) : _tform;
		}
	}

	protected GUIView View
	{
		get
		{
			if (_view == null)
			{
				_view = GUIView.GetParentView(tform);
			}
			return _view;
		}
	}

	protected virtual Body GetBody(World world)
	{
		return null;
	}

	public virtual bool TouchEvent(YGEvent evt)
	{
		switch (evt.type)
		{
		case YGEvent.TYPE.TOUCH_BEGIN:
			touchInProgress = true;
			break;
		case YGEvent.TYPE.TOUCH_CANCEL:
		case YGEvent.TYPE.RESET:
			touchInProgress = false;
			break;
		case YGEvent.TYPE.TOUCH_END:
			touchInProgress = false;
			break;
		}
		return EventDispatch.FireEvent(evt);
	}

	protected virtual void OnEnable()
	{
		GUIView view = View;
		World world = view._2DWorld.World;
		if (this.world != world)
		{
			if (body != null)
			{
				body.Dispose();
				body = null;
			}
			this.world = world;
		}
		if (body == null)
		{
			body = GetBody(this.world);
		}
		if (body == null)
		{
			Debug.LogError(string.Format("creating Yarg2DBody failed: {0}", base.gameObject.name));
			return;
		}
		body.BodyType = bodyType;
		body.OnCollision += OnCollision;
		body.OnSeparation += OnSeparation;
		body.UserData = this;
		view.RegisterTouchable(tform.GetInstanceID(), this);
		YGSprite component = GetComponent<YGSprite>();
		if (component != null)
		{
			component.MeshUpdateEvent.AddListener(MatchTransform3D);
			view.RefreshEvent += MatchTransform3D;
		}
		else
		{
			body.SetTransform(tform.position, tform.rotation.eulerAngles.z * ((float)Math.PI / 180f));
		}
		view.RefreshWorld();
	}

	public void ReregisterTouchable()
	{
		OnDisable();
		OnEnable();
	}

	public void MatchTransform3D()
	{
		if (!base.gameObject.active || !base.enabled)
		{
			return;
		}
		if (sprite == null)
		{
			sprite = base.gameObject.GetComponent<YGSprite>();
		}
		Vector3 vector = Vector3.zero;
		Quaternion rotation = tform.rotation;
		if (sprite != null)
		{
			Mesh sharedMesh = sprite.meshFilter.sharedMesh;
			if (sharedMesh != null)
			{
				vector = rotation * sharedMesh.bounds.center;
			}
		}
		body.SetTransform(tform.position + vector, rotation.eulerAngles.z * ((float)Math.PI / 180f));
		View.RefreshWorld();
	}

	private void Start()
	{
		View.RefreshEvent += MatchTransform3D;
	}

	protected virtual void OnDisable()
	{
		if (touchInProgress)
		{
			touchInProgress = false;
			YGEvent yGEvent = new YGEvent();
			yGEvent.type = YGEvent.TYPE.RESET;
			EventDispatch.FireEvent(yGEvent);
		}
		GUIView view = View;
		view.RefreshEvent -= MatchTransform3D;
		view.UnregisterTouchable(tform.GetInstanceID());
		YGSprite component = GetComponent<YGSprite>();
		if (component != null)
		{
			component.MeshUpdateEvent.RemoveListener(MatchTransform3D);
		}
		_view = null;
		if (body != null)
		{
			body.OnCollision -= OnCollision;
			body.OnSeparation -= OnSeparation;
		}
	}

	public void OnDestroy()
	{
		if (body != null)
		{
			body.Dispose();
			body = null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public virtual void UpdateTransform()
	{
		YG2DWorld.UpdateTransform(tform, body);
	}

	protected virtual void OnSeparation(Fixture f1, Fixture f2)
	{
	}

	protected virtual bool OnCollision(Fixture f1, Fixture f2, Contact contact)
	{
		return true;
	}
}
