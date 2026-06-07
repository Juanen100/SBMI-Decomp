using System;
using System.Collections.Generic;
using UnityEngine;

public class SBGUIElement : MonoBehaviour
{
	public string SessionActionId;

	protected static int instanceCount;

	protected bool muted;

	private GUIView _view;

	protected Rect rect;

	private Transform _tform;

	protected List<SBGUIElement> guiElements;

	public EventDispatcher<SBGUIEvent> EventListener;

	private DateTime startTime;

	protected static int InstanceID
	{
		get
		{
			return 0;
		}
	}

	protected GUIView View
	{
		get
		{
			return null;
		}
	}

	public virtual Vector3 WorldPosition
	{
		get
		{
			return default(Vector3);
		}
		set
		{
		}
	}

	protected virtual bool Muted
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public Transform tform
	{
		get
		{
			return null;
		}
	}

	public virtual Bounds TotalBounds
	{
		get
		{
			return default(Bounds);
		}
	}

	public float TotalWidth
	{
		get
		{
			return 0f;
		}
	}

	public bool Visible
	{
		get
		{
			return false;
		}
	}

	public double ElapsedTime
	{
		get
		{
			return 0.0;
		}
	}

	public float Alpha
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public void EnableRejectButton(bool enabled)
	{
	}

	public void EnableButtons(bool enabled)
	{
	}

	public virtual void MuteButtons(bool mute)
	{
	}

	public void SetTransformParent(Transform parent)
	{
	}

	protected void SetTransformParent(SBGUIElement parent)
	{
	}

	public static SBGUIElement Create()
	{
		return null;
	}

	public static SBGUIElement Create(SBGUIElement parent)
	{
		return null;
	}

	public void ReregisterColliders()
	{
	}

	public Transform GetParent()
	{
		return null;
	}

	public Dictionary<string, SBGUIElement> CacheChildren()
	{
		return null;
	}

	public SBGUIElement FindChild(string name)
	{
		return null;
	}

	public SBGUIElement FindChildSessionActionId(string sessionActionId, bool includeInactive)
	{
		return null;
	}

	public virtual SBGUIElement FindDynamicSubElementSessionActionId(string sessionActionId, bool includeInactive)
	{
		return null;
	}

	public bool IsActive()
	{
		return false;
	}

	public virtual void OnScreenStart(SBGUIScreen screen)
	{
	}

	public virtual void OnScreenEnd(SBGUIScreen screen)
	{
	}

	public virtual void SetVisible(bool viz)
	{
	}

	public virtual void SetActive(bool active)
	{
	}

	public virtual void SetScreenPosition(float pos_x, float pos_y)
	{
	}

	public void SetScreenPosition(Vector2 pos)
	{
	}

	public Vector2 GetScreenPosition()
	{
		return default(Vector2);
	}

	protected void UpdateColliderTransforms()
	{
	}

	public void SetPosition(float pos_x, float pos_y, float pos_z)
	{
	}

	public void SetPosition(Vector3 pos)
	{
	}

	public void SetLookAt(Vector3 position, Vector3 up)
	{
	}

	public virtual void SetParent(SBGUIElement element)
	{
	}

	public virtual void SetParent(SBGUIElement element, bool bEnforceMuteFromParent)
	{
	}

	public virtual void GUIUpdate()
	{
	}

	protected virtual void Awake()
	{
	}

	protected virtual void OnEnable()
	{
	}

	protected virtual void OnDisable()
	{
	}

	public virtual void AttachAnalyticsToButton(string buttonName, SBGUIButton button)
	{
	}

	public SBGUIButton AttachActionToButton(string buttonName, Action action)
	{
		return null;
	}

	public SBGUIButton AttachActionToButton(SBGUIButton button, Action action)
	{
		return null;
	}

	public void EnforceMuteFromParent(SBGUIElement element)
	{
	}

	public void ClearButtonActions(string buttonName)
	{
	}

	public void ClearButtonActions(SBGUIButton button)
	{
	}

	public void ReactivateButton(SBGUIButton button)
	{
	}

	public void UpdateCollider()
	{
	}

	public void StartTimer()
	{
	}

	public virtual void OnDestroy()
	{
	}
}
