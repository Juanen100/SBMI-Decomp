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
			if (_view == null)
			{
				_view = GUIView.GetParentView(tform);
			}
			return _view;
		}
	}

	protected Transform tform
	{
		get
		{
			return (!(_tform != null)) ? (_tform = base.transform) : _tform;
		}
	}

	protected YGSprite parent
	{
		get
		{
			if (_parent == null)
			{
				_parent = GetComponent<YGSprite>();
			}
			return _parent;
		}
	}

	public void SetPosition(int x, int y)
	{
		Vector3 position = View.PixelsToWorld(new Vector2(x, y));
		tform.position = position;
	}

	public virtual void Load()
	{
	}

	protected virtual void OnEnable()
	{
		body = GetComponent<YG2DBody>();
		if (body == null)
		{
			body = base.gameObject.AddComponent<YG2DRectangle>();
		}
		body.EventDispatch.AddListener(TouchEventHandler);
		if (NeedsLoad)
		{
			View.RefreshEvent += Load;
		}
	}

	protected virtual void OnDisable()
	{
		if (body != null)
		{
			body.EventDispatch.RemoveListener(TouchEventHandler);
		}
		_view = null;
	}

	protected virtual bool TouchEventHandler(YGEvent evt)
	{
		return false;
	}

	public virtual void SetVisible(bool visible)
	{
		parent.enabled = visible;
	}
}
