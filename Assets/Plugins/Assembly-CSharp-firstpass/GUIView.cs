using System;
using System.Collections.Generic;
using UnityEngine;
using Yarg;

[ExecuteInEditMode]
public class GUIView : MonoBehaviour
{
	private static float RESOLUTION_FACTOR;

	public const string guiLayer = "__GUI__";

	public const float UNITS_PER_PIXEL = 0.01f;

	[HideInInspector]
	public int guiMask;

	protected Dictionary<int, ITouchable> touchables;

	protected List<ITouchable> targets;

	protected List<ITouchable> activeTargetSet;

	private RaycastHit[] hits;

	private YGTextureLibrary library;

	private bool updateWorld;

	public ReadyEventDispatcher ReadyEvent;

	protected YG2DWorld _world;

	protected float pixelScale;

	protected Dictionary<int, YGEvent> eventHistory;

	protected Camera _cam;

	public YGTextureLibrary Library
	{
		get
		{
			return null;
		}
	}

	public YG2DWorld _2DWorld
	{
		get
		{
			return null;
		}
	}

	public Camera Cam
	{
		get
		{
			return null;
		}
	}

	private event Action refreshEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public event Action RefreshEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public void RefreshWorld()
	{
	}

	private void UpdateWorld()
	{
	}

	public float GetPixelScale()
	{
		return 0f;
	}

	public Bounds GetTotalBounds()
	{
		return default(Bounds);
	}

	public void ReloadSprites()
	{
	}

	protected void ValidateTargets(List<ITouchable> prev, List<ITouchable> current)
	{
	}

	protected YGEvent UpdateAndSendEvent(YGEvent evt, List<ITouchable> targets)
	{
		return null;
	}

	public static void ResetResolutionFactor()
	{
	}

	public static float ResolutionScaleFactor()
	{
		return 0f;
	}

	public virtual void ResizePortal()
	{
	}

	public static GUIView GetParentView(Transform tf)
	{
		return null;
	}

	public Vector3 PixelsToWorld(Vector2 pixels)
	{
		return default(Vector3);
	}

	public Vector3 ScreenToWorld(Vector2 screenPos)
	{
		return default(Vector3);
	}

	public Vector3 WorldToScreen(Vector3 worldPos)
	{
		return default(Vector3);
	}

	public virtual void RegisterTouchable(int t, ITouchable touchable)
	{
	}

	public void UnregisterTouchable(int t)
	{
	}

	private void PixelSnapTransform(Transform transf)
	{
	}

	public void PixelSnapSprites()
	{
	}

	protected virtual void Awake()
	{
	}

	protected virtual void OnEnable()
	{
	}

	protected virtual void Start()
	{
	}

	protected virtual void OnDisable()
	{
	}

	public void SnapAnchors()
	{
	}

	protected void SendRefreshEvent()
	{
	}

	protected virtual void LateUpdate()
	{
	}

	protected virtual List<ITouchable> RayHit(Vector2 pos)
	{
		return null;
	}
}
