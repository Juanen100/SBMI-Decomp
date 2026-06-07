using UnityEngine;
using Yarg;

public class ScrollRegion : MonoBehaviour, ITouchable
{
	public Vector2 size;

	private GUIMainView mainView;

	public GUISubView subView;

	private Rect worldRect;

	private Transform _tform;

	public ReadyEventDispatcher ReadyEvent;

	public YGEventDispatcher ScrollEvent;

	private bool mainViewReady;

	private bool subViewReady;

	public Transform tform
	{
		get
		{
			return null;
		}
	}

	public Transform SubViewTform
	{
		get
		{
			return null;
		}
	}

	public bool Visible
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	private void SendPostInitializationReadyEvent()
	{
	}

	private void CreateSubView()
	{
	}

	private void OnEnable()
	{
	}

	private void MoveChildrenToSubView()
	{
	}

	private void OnDisable()
	{
	}

	private void OnDestroy()
	{
	}

	public Bounds GetTotalBounds()
	{
		return default(Bounds);
	}

	public Rect GetWorldRect()
	{
		return default(Rect);
	}

	public Vector3 ScreenToWorld(Vector3 pos)
	{
		return default(Vector3);
	}

	public void MatchSubView()
	{
	}

	public void ResetContents(YGEvent evt)
	{
	}

	public virtual bool TouchEvent(YGEvent evt)
	{
		return false;
	}

	private void OnDrawGizmosSelected()
	{
	}
}
