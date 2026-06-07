using UnityEngine;
using Yarg;

public class BaseButton : MonoBehaviour, ILoadable
{
	private YG2DBody body;

	private GUIView _view;

	private Transform _tform;

	private YGSprite _parent;

	protected virtual bool NeedsLoad
	{
		get
		{
			return false;
		}
	}

	protected GUIView View
	{
		get
		{
			return null;
		}
	}

	protected Transform tform
	{
		get
		{
			return null;
		}
	}

	protected YGSprite parent
	{
		get
		{
			return null;
		}
	}

	public void SetPosition(int x, int y)
	{
	}

	public virtual void Load()
	{
	}

	protected virtual void OnEnable()
	{
	}

	protected virtual void OnDisable()
	{
	}

	protected virtual bool TouchEventHandler(YGEvent evt)
	{
		return false;
	}

	public virtual void SetVisible(bool visible)
	{
	}
}
