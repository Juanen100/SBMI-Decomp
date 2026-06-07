using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using UnityEngine;
using Yarg;

[ExecuteInEditMode]
public abstract class YG2DBody : MonoBehaviour, ITouchable
{
	public float density;

	public BodyType bodyType;

	protected Body body;

	public Vector2 offset;

	public YGEventDispatcher EventDispatch;

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
			return null;
		}
	}

	public Body Body
	{
		get
		{
			return null;
		}
	}

	public Transform tform
	{
		get
		{
			return null;
		}
	}

	protected GUIView View
	{
		get
		{
			return null;
		}
	}

	protected virtual Body GetBody(World world)
	{
		return null;
	}

	public virtual bool TouchEvent(YGEvent evt)
	{
		return false;
	}

	protected virtual void OnEnable()
	{
	}

	public void ReregisterTouchable()
	{
	}

	public void MatchTransform3D()
	{
	}

	private void Start()
	{
	}

	protected virtual void OnDisable()
	{
	}

	public void OnDestroy()
	{
	}

	public virtual void UpdateTransform()
	{
	}

	protected virtual void OnSeparation(Fixture f1, Fixture f2)
	{
	}

	protected virtual bool OnCollision(Fixture f1, Fixture f2, Contact contact)
	{
		return false;
	}
}
