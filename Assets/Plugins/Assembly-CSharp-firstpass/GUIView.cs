using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using UnityEngine;
using Yarg;

[ExecuteInEditMode]
public class GUIView : MonoBehaviour
{
	public const string guiLayer = "__GUI__";

	public const float UNITS_PER_PIXEL = 0.01f;

	private static float RESOLUTION_FACTOR = -1f;

	private static int IPHONE_SCREEN_HEIGHT_LOCAL = -1;

	[HideInInspector]
	public int guiMask;

	protected Dictionary<int, ITouchable> touchables = new Dictionary<int, ITouchable>();

	protected List<ITouchable> targets = new List<ITouchable>(5);

	protected List<ITouchable> activeTargetSet = new List<ITouchable>(5);

	private RaycastHit[] hits;

	private YGTextureLibrary library;

	private volatile bool updateWorld;

	public ReadyEventDispatcher ReadyEvent = new ReadyEventDispatcher();

	protected YG2DWorld _world;

	protected float pixelScale;

	protected Dictionary<int, YGEvent> eventHistory = new Dictionary<int, YGEvent>();

	protected Camera _cam;

	private static int IPHONE_SCREEN_HEIGHT
	{
		get
		{
			if (IPHONE_SCREEN_HEIGHT_LOCAL < 0)
			{
				IPHONE_SCREEN_HEIGHT_LOCAL = 640;
			}
			return IPHONE_SCREEN_HEIGHT_LOCAL;
		}
	}

	public static bool RetinaDisplay
	{
		get
		{
			return Screen.dpi > 250f;
		}
	}

	public YGTextureLibrary Library
	{
		get
		{
			if (library == null)
			{
				library = GetComponent<YGTextureLibrary>();
				if (library == null)
				{
					GUIView parentView = GetParentView(base.transform.parent);
					library = ((!(parentView == null)) ? parentView.Library : null);
				}
			}
			return library;
		}
	}

	public YG2DWorld _2DWorld
	{
		get
		{
			if (_world == null)
			{
				_world = base.gameObject.GetComponent<YG2DWorld>();
				if (_world == null)
				{
					Debug.LogError("GUIView does not have a 2DWorld associated.");
				}
			}
			return _world;
		}
	}

	public Camera Cam
	{
		get
		{
			if (_cam == null)
			{
				_cam = GetComponent<Camera>();
				if (_cam == null)
				{
					_cam = base.gameObject.AddComponent<Camera>();
					_cam.orthographic = true;
					_cam.clearFlags = CameraClearFlags.Depth;
					_cam.cullingMask = 1 << LayerMask.NameToLayer("__GUI__");
				}
			}
			return _cam;
		}
	}

	private event Action refreshEvent;

	public event Action RefreshEvent
	{
		add
		{
			if (this.refreshEvent == null)
			{
				this.refreshEvent = (Action)Delegate.Combine(this.refreshEvent, value);
				return;
			}
			this.refreshEvent = (Action)Delegate.Remove(this.refreshEvent, value);
			this.refreshEvent = (Action)Delegate.Combine(this.refreshEvent, value);
		}
		remove
		{
			if (this.refreshEvent != null)
			{
				this.refreshEvent = (Action)Delegate.Remove(this.refreshEvent, value);
			}
		}
	}

	public static bool IsIPhone6s(ref bool plusSize)
	{
		bool result = false;
		plusSize = false;
		return result;
	}

	public void RefreshWorld()
	{
		updateWorld = true;
	}

	private void UpdateWorld()
	{
		if (updateWorld)
		{
			updateWorld = false;
			_2DWorld.World.Step(0f);
		}
	}

	public float GetPixelScale()
	{
		return pixelScale;
	}

	public Bounds GetTotalBounds()
	{
		MeshRenderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<MeshRenderer>();
		if (componentsInChildren == null || componentsInChildren.Length == 0)
		{
			return new Bounds(Vector3.zero, Vector3.zero);
		}
		Bounds bounds = componentsInChildren[0].bounds;
		for (int i = 1; i < componentsInChildren.Length; i++)
		{
			bounds.Encapsulate(componentsInChildren[i].bounds);
		}
		return bounds;
	}

	public void ReloadSprites()
	{
		SendRefreshEvent();
	}

	protected void ValidateTargets(List<ITouchable> prev, List<ITouchable> current)
	{
		if (prev.Count == 0)
		{
			return;
		}
		YGEvent yGEvent = new YGEvent();
		yGEvent.type = YGEvent.TYPE.RESET;
		for (int i = 0; i < prev.Count; i++)
		{
			if (current == null)
			{
				prev[i].TouchEvent(yGEvent);
			}
			else if (!current.Contains(prev[i]))
			{
				prev[i].TouchEvent(yGEvent);
			}
		}
		if (current == null)
		{
			prev.Clear();
		}
		else
		{
			prev = current;
		}
	}

	protected YGEvent UpdateAndSendEvent(YGEvent evt, List<ITouchable> targets)
	{
		YGEvent value = null;
		if (eventHistory.TryGetValue(evt.fingerId, out value) && value != null)
		{
			evt = value.Update(evt);
		}
		else
		{
			eventHistory[evt.fingerId] = evt;
		}
		if (evt.type == YGEvent.TYPE.TOUCH_END || evt.type == YGEvent.TYPE.TOUCH_CANCEL)
		{
			eventHistory.Remove(evt.fingerId);
		}
		else if (evt.type == YGEvent.TYPE.TOUCH_STAY && evt.Hold)
		{
			eventHistory[evt.fingerId] = evt;
		}
		targets.Sort(delegate(ITouchable a, ITouchable b)
		{
			float z = a.tform.position.z;
			float z2 = b.tform.position.z;
			return z.CompareTo(z2);
		});
		for (int num = 0; num < targets.Count; num++)
		{
			if ((!evt.used || (evt.type != YGEvent.TYPE.TAP && evt.type != YGEvent.TYPE.TOUCH_BEGIN && evt.type != YGEvent.TYPE.TOUCH_END)) && targets[num].TouchEvent(evt))
			{
				evt.used = true;
			}
		}
		return evt;
	}

	public static float ResolutionScaleFactor()
	{
		if (RESOLUTION_FACTOR < 0f)
		{
			RESOLUTION_FACTOR = (float)Screen.height / 380f;
		}
		return RESOLUTION_FACTOR;
	}

	protected virtual void ResizePortal()
	{
		Cam.orthographicSize = pixelScale / ResolutionScaleFactor();
	}

	public static GUIView GetParentView(Transform tf)
	{
		GUIView gUIView = null;
		while (tf != null)
		{
			gUIView = tf.GetComponent<GUIView>();
			if (gUIView != null)
			{
				return gUIView;
			}
			tf = tf.parent;
		}
		if (gUIView == null)
		{
			return GUIMainView.GetInstance();
		}
		return gUIView;
	}

	public Vector3 PixelsToWorld(Vector2 pixels)
	{
		Vector3 position = new Vector3(pixels.x, (float)Screen.height - pixels.y, 0f - Cam.transform.position.z);
		Vector3 result = Cam.ScreenToWorldPoint(position);
		result.z = 0f;
		return result;
	}

	public Vector3 ScreenToWorld(Vector2 screenPos)
	{
		Vector3 result = Cam.ScreenToWorldPoint(screenPos);
		result.z = 0f;
		return result;
	}

	public Vector3 WorldToScreen(Vector3 worldPos)
	{
		return Cam.WorldToScreenPoint(worldPos);
	}

	public virtual void RegisterTouchable(int t, ITouchable touchable)
	{
		touchables[t] = touchable;
	}

	public void UnregisterTouchable(int t)
	{
		touchables.Remove(t);
	}

	private void PixelSnapTransform(Transform transf)
	{
		Vector3 position = transf.position;
		position.x = (float)Mathf.RoundToInt(position.x / 0.01f) * 0.01f;
		position.y = (float)Mathf.RoundToInt(position.y / 0.01f) * 0.01f;
		position.z = (float)Mathf.RoundToInt(position.z / 0.01f) * 0.01f;
		transf.position = position;
		foreach (Transform item in transf)
		{
			PixelSnapTransform(item);
		}
	}

	public void PixelSnapSprites()
	{
		PixelSnapTransform(base.transform);
		YGAtlasSprite[] componentsInChildren = Cam.GetComponentsInChildren<YGAtlasSprite>(true);
		YGAtlasSprite[] array = componentsInChildren;
		foreach (YGAtlasSprite yGAtlasSprite in array)
		{
			yGAtlasSprite.PixelSnap();
		}
	}

	protected virtual void Awake()
	{
		base.gameObject.layer = LayerMask.NameToLayer("__GUI__");
		guiMask = 1 << LayerMask.NameToLayer("__GUI__");
	}

	protected virtual void OnEnable()
	{
		targets = new List<ITouchable>(10);
	}

	protected virtual void Start()
	{
		if (Cam.orthographic)
		{
			ResizePortal();
		}
		ReadyEvent.FireEvent();
	}

	protected virtual void OnDisable()
	{
		library = null;
	}

	public void SnapAnchors()
	{
		ScreenAnchor[] componentsInChildren = GetComponentsInChildren<ScreenAnchor>(true);
		ScreenAnchor[] array = componentsInChildren;
		foreach (ScreenAnchor screenAnchor in array)
		{
			screenAnchor.SnapAnchor(Cam);
		}
	}

	protected void SendRefreshEvent()
	{
		if (this.refreshEvent != null)
		{
			Delegate[] invocationList = this.refreshEvent.GetInvocationList();
			this.refreshEvent = null;
			for (int i = 0; i < invocationList.Length; i++)
			{
				((Action)invocationList[i])();
			}
			UpdateWorld();
		}
	}

	protected virtual void LateUpdate()
	{
		SendRefreshEvent();
	}

	protected virtual List<ITouchable> RayHit(Vector2 pos)
	{
		YG2DWorld yG2DWorld = _2DWorld;
		targets.Clear();
		if (!Application.isPlaying || yG2DWorld == null)
		{
			return targets;
		}
		List<Fixture> hitFixtures = yG2DWorld.GetHitFixtures(pos);
		if (hitFixtures.Count == 0)
		{
			return targets;
		}
		foreach (Fixture item in hitFixtures)
		{
			YG2DBody yG2DBody = item.Body.UserData as YG2DBody;
			if (!(yG2DBody == null))
			{
				int instanceID = yG2DBody.transform.GetInstanceID();
				if (touchables.ContainsKey(instanceID))
				{
					targets.Add(touchables[instanceID]);
				}
			}
		}
		return targets;
	}
}
