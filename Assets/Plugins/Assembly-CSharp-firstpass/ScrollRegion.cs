using UnityEngine;
using Yarg;

public class ScrollRegion : MonoBehaviour, ITouchable
{
	public Vector2 size = new Vector2(200f, 100f);

	private GUIMainView mainView;

	public GUISubView subView;

	private Rect worldRect;

	private Transform _tform;

	public ReadyEventDispatcher ReadyEvent = new ReadyEventDispatcher();

	public YGEventDispatcher ScrollEvent = new YGEventDispatcher();

	private bool mainViewReady;

	private bool subViewReady;

	public Transform tform
	{
		get
		{
			return (!(_tform != null)) ? (_tform = base.transform) : _tform;
		}
	}

	public Transform SubViewTform
	{
		get
		{
			return subView.transform;
		}
	}

	public bool Visible
	{
		get
		{
			return subView.Cam.enabled;
		}
		set
		{
			YG2DBody[] componentsInChildren = subView.gameObject.GetComponentsInChildren<YG2DBody>();
			YG2DBody[] array = componentsInChildren;
			foreach (YG2DBody yG2DBody in array)
			{
				yG2DBody.enabled = value;
			}
			subView.Cam.enabled = value;
		}
	}

	private void SendPostInitializationReadyEvent()
	{
		if (mainViewReady && subViewReady)
		{
			ReadyEvent.FireEvent();
		}
	}

	private void CreateSubView()
	{
		subView = mainView.CreateSubView();
		subView.ReadyEvent.AddListener(delegate
		{
			subViewReady = true;
			subView.SetRegion(this);
			MatchSubView();
			SendPostInitializationReadyEvent();
		});
	}

	private void OnEnable()
	{
		mainView = GUIMainView.GetInstance();
		mainView.ReadyEvent.AddListener(delegate
		{
			mainViewReady = true;
			if (subView == null)
			{
				CreateSubView();
			}
			else
			{
				subView.gameObject.SetActiveRecursively(true);
				mainView.AddSubView(subView);
				subView.RegisterTouchable(tform.GetInstanceID(), this);
			}
			SendPostInitializationReadyEvent();
		});
	}

	private void MoveChildrenToSubView()
	{
		foreach (Transform item in base.transform)
		{
			item.gameObject.SetActiveRecursively(false);
			item.parent = subView.tform;
			item.gameObject.SetActiveRecursively(true);
		}
	}

	private void OnDisable()
	{
		mainView.RemoveSubView(subView);
		if (subView != null)
		{
			subView.gameObject.SetActiveRecursively(false);
			subView.UnregisterTouchable(tform.GetInstanceID());
		}
	}

	private void OnDestroy()
	{
		if (subView != null)
		{
			Object.Destroy(subView.gameObject);
		}
	}

	public Bounds GetTotalBounds()
	{
		return subView.GetTotalBounds();
	}

	public Rect GetWorldRect()
	{
		return worldRect;
	}

	public Vector3 ScreenToWorld(Vector3 pos)
	{
		Vector3 result = subView.ScreenToWorld(pos);
		result.z = subView.tform.position.z;
		return result;
	}

	public void MatchSubView()
	{
		Vector2 vector = size * 0.01f;
		Vector3 position = base.transform.position;
		worldRect = new Rect(position.x, position.y, vector.x, vector.y);
		subView.SetPortal(worldRect);
	}

	public void ResetContents(YGEvent evt)
	{
		evt.type = YGEvent.TYPE.RESET;
		subView.TouchEvent(evt);
	}

	public virtual bool TouchEvent(YGEvent evt)
	{
		return ScrollEvent.FireEvent(evt);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;
		Vector2 vector = size * 0.01f;
		Vector3 position = base.transform.position;
		Rect rect = new Rect(position.x, position.y, vector.x, vector.y);
		Vector3 center = rect.center;
		center.z = position.z;
		Gizmos.DrawWireCube(size: new Vector3(rect.width, rect.height, 0f), center: center);
	}
}
