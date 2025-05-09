using System;
using System.Collections.Generic;
using UnityEngine;
using Yarg;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(YG2DWorld))]
public class GUISubView : GUIView, ITouchable
{
	[NonSerialized]
	protected Transform _tform;

	private GUIView _parentView;

	private ScrollRegion region;

	private Rect viewRect;

	public Transform tform
	{
		get
		{
			return (!(_tform != null)) ? (_tform = base.transform) : _tform;
		}
	}

	private GUIView ParentView
	{
		get
		{
			if (_parentView == null)
			{
				_parentView = GUIView.GetParentView(base.transform.parent);
			}
			return _parentView;
		}
	}

	public void SetRegion(ScrollRegion rgn)
	{
		region = rgn;
	}

	public virtual bool TouchEvent(YGEvent evt)
	{
		targets = RayHit(evt.position);
		return UpdateAndSendEvent(evt, targets).used;
	}

	protected override List<ITouchable> RayHit(Vector2 pos)
	{
		targets = base.RayHit(pos);
		if (region != null)
		{
			targets.Add(region);
		}
		return targets;
	}

	protected override void OnDisable()
	{
		_parentView = null;
		base.OnDisable();
	}

	private void OnDestroy()
	{
		GUIMainView instance = GUIMainView.GetInstance();
		instance.RemoveSubView(this);
	}

	public static GUISubView Create(Transform parent)
	{
		GameObject gameObject = new GameObject();
		gameObject.name = string.Format("GUISubView_{0}", (uint)gameObject.GetInstanceID());
		gameObject.transform.parent = parent;
		GUISubView gUISubView = gameObject.AddComponent<GUISubView>();
		Camera cam = gUISubView.Cam;
		cam.orthographic = true;
		cam.farClipPlane = 20f;
		cam.nearClipPlane = 0f;
		cam.clearFlags = CameraClearFlags.Depth;
		cam.cullingMask = 1 << LayerMask.NameToLayer("__GUI__");
		return gUISubView;
	}

	public bool ContainsPoint(Vector2 point)
	{
		return viewRect.Contains(point);
	}

	public void SetPortal(Rect p)
	{
		viewRect = p;
		ResizePortal();
	}

	protected override void ResizePortal()
	{
		Camera cam = ParentView.Cam;
		Vector3 vector = cam.WorldToViewportPoint(new Vector2(viewRect.xMax, viewRect.yMax));
		Vector3 vector2 = cam.WorldToViewportPoint(new Vector2(viewRect.xMin, viewRect.yMin));
		Rect rect = new Rect(0f, 0f, 0f, 0f);
		rect.xMax = vector.x;
		rect.yMax = vector.y;
		rect.xMin = vector2.x;
		rect.yMin = vector2.y;
		base.Cam.rect = rect;
		pixelScale = (float)Screen.height * 0.01f * rect.height;
		base.ResizePortal();
	}

	private void OnDrawGizmos()
	{
		Bounds totalBounds = GetTotalBounds();
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireCube(totalBounds.center, totalBounds.size);
	}
}
