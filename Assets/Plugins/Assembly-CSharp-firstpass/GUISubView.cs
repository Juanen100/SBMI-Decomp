using System;
using System.Collections.Generic;
using UnityEngine;
using Yarg;

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
			return null;
		}
	}

	private GUIView ParentView
	{
		get
		{
			return null;
		}
	}

	public void SetRegion(ScrollRegion rgn)
	{
	}

	public virtual bool TouchEvent(YGEvent evt)
	{
		return false;
	}

	protected override List<ITouchable> RayHit(Vector2 pos)
	{
		return null;
	}

	protected override void OnDisable()
	{
	}

	private void OnDestroy()
	{
	}

	public static GUISubView Create(Transform parent)
	{
		return null;
	}

	public bool ContainsPoint(Vector2 point)
	{
		return false;
	}

	public void SetPortal(Rect p)
	{
	}

	public override void ResizePortal()
	{
	}

	private void OnDrawGizmos()
	{
	}
}
