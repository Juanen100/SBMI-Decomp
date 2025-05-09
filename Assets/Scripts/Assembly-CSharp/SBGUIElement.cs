#define ASSERTS_ON
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

	protected List<SBGUIElement> guiElements = new List<SBGUIElement>();

	public EventDispatcher<SBGUIEvent> EventListener = new EventDispatcher<SBGUIEvent>();

	private DateTime startTime;

	protected static int InstanceID
	{
		get
		{
			return instanceCount++;
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

	public virtual Vector3 WorldPosition
	{
		get
		{
			return tform.position;
		}
		set
		{
			if (!(tform.position == value))
			{
				tform.position = value;
				YGSprite.MeshUpdateHierarchy(base.gameObject);
			}
		}
	}

	protected virtual bool Muted
	{
		get
		{
			return muted;
		}
		set
		{
			muted = value;
		}
	}

	public Transform tform
	{
		get
		{
			return (!(_tform != null)) ? (_tform = base.transform) : _tform;
		}
	}

	public virtual Bounds TotalBounds
	{
		get
		{
			YGSprite[] componentsInChildren = GetComponentsInChildren<YGSprite>();
			if (componentsInChildren == null || componentsInChildren.Length == 0)
			{
				return new Bounds(Vector3.zero, Vector3.zero);
			}
			Bounds bounds = componentsInChildren[0].GetBounds();
			for (int i = 1; i < componentsInChildren.Length; i++)
			{
				bounds.Encapsulate(componentsInChildren[i].GetBounds());
			}
			YGSprite component = GetComponent<YGSprite>();
			if ((bool)component)
			{
				bounds.Encapsulate(component.size);
			}
			else
			{
				YGAtlasSprite component2 = GetComponent<YGAtlasSprite>();
				if ((bool)component2)
				{
					bounds.Encapsulate(component2.size);
				}
			}
			return bounds;
		}
	}

	public float TotalWidth
	{
		get
		{
			return TotalBounds.ToRect().width;
		}
	}

	public bool Visible
	{
		get
		{
			return base.gameObject.GetComponent<Renderer>() != null && base.gameObject.GetComponent<Renderer>().enabled;
		}
	}

	public double ElapsedTime
	{
		get
		{
			return (DateTime.UtcNow - startTime).TotalMilliseconds;
		}
	}

	public float Alpha
	{
		get
		{
			YGSprite component = GetComponent<YGSprite>();
			return component.color.a;
		}
		set
		{
			YGSprite component = GetComponent<YGSprite>();
			component.color.a = value;
		}
	}

	public void EnableRejectButton(bool enabled)
	{
		Transform[] componentsInChildren = base.gameObject.GetComponentsInChildren<Transform>();
		Transform[] array = componentsInChildren;
		foreach (Transform transform in array)
		{
			YGAtlasSprite component = transform.GetComponent<YGAtlasSprite>();
			if (component != null && component.sprite != null && component.sprite.name != null && component.sprite.name.Contains("ActionStrip_Cancel.png"))
			{
				if (enabled)
				{
					component.SetAlpha(1f);
				}
				else
				{
					component.SetAlpha(0.25f);
				}
				transform.GetComponent<SBGUIButton>().enabled = enabled;
			}
		}
	}

	public void EnableButtons(bool enabled)
	{
		SBGUIButton[] componentsInChildren = base.gameObject.GetComponentsInChildren<SBGUIButton>(enabled);
		SBGUIButton[] array = componentsInChildren;
		foreach (SBGUIButton sBGUIButton in array)
		{
			sBGUIButton.enabled = enabled;
		}
	}

	public virtual void MuteButtons(bool mute)
	{
		Muted = mute;
		if (this != null)
		{
			SBGUIElement[] componentsInChildren = base.gameObject.GetComponentsInChildren<SBGUIElement>(true);
			SBGUIElement[] array = componentsInChildren;
			foreach (SBGUIElement sBGUIElement in array)
			{
				sBGUIElement.Muted = mute;
			}
		}
	}

	public void SetTransformParent(Transform parent)
	{
		tform.parent = parent;
		base.gameObject.layer = 0;
	}

	protected void SetTransformParent(SBGUIElement parent)
	{
		Transform parent2 = ((!(parent == null)) ? parent.tform : GUIMainView.GetInstance().transform);
		tform.parent = parent2;
		base.gameObject.layer = View.gameObject.layer;
	}

	public static SBGUIElement Create()
	{
		GameObject gameObject = new GameObject(string.Format("SBGUIElement_{0}", InstanceID));
		SBGUIElement sBGUIElement = gameObject.AddComponent<SBGUIElement>();
		sBGUIElement.SetParent(null);
		return sBGUIElement;
	}

	public static SBGUIElement Create(SBGUIElement parent)
	{
		SBGUIElement sBGUIElement = Create();
		sBGUIElement.SetParent(parent);
		return sBGUIElement;
	}

	public void ReregisterColliders()
	{
		YG2DBody[] componentsInChildren = GetComponentsInChildren<YG2DBody>();
		YG2DBody[] array = componentsInChildren;
		foreach (YG2DBody yG2DBody in array)
		{
			if (yG2DBody.enabled)
			{
				yG2DBody.ReregisterTouchable();
			}
		}
	}

	public Transform GetParent()
	{
		return (!(_tform != null)) ? null : _tform.parent;
	}

	public Dictionary<string, SBGUIElement> CacheChildren()
	{
		Dictionary<string, SBGUIElement> dictionary = new Dictionary<string, SBGUIElement>();
		SBGUIElement[] componentsInChildren = base.gameObject.GetComponentsInChildren<SBGUIElement>(true);
		SBGUIElement[] array = componentsInChildren;
		foreach (SBGUIElement sBGUIElement in array)
		{
			if (!dictionary.ContainsKey(sBGUIElement.name) || !(sBGUIElement.gameObject == dictionary[sBGUIElement.name].gameObject))
			{
				dictionary.Add(sBGUIElement.name, sBGUIElement);
			}
		}
		return dictionary;
	}

	public SBGUIElement FindChild(string name)
	{
		SBGUIElement[] componentsInChildren = base.gameObject.GetComponentsInChildren<SBGUIElement>(true);
		SBGUIElement[] array = componentsInChildren;
		foreach (SBGUIElement sBGUIElement in array)
		{
			if (sBGUIElement.gameObject.name == name)
			{
				return sBGUIElement;
			}
		}
		return null;
	}

	public SBGUIElement FindChildSessionActionId(string sessionActionId, bool includeInactive)
	{
		SBGUIElement[] componentsInChildren = base.gameObject.GetComponentsInChildren<SBGUIElement>(includeInactive);
		SBGUIElement[] array = componentsInChildren;
		foreach (SBGUIElement sBGUIElement in array)
		{
			if (sBGUIElement.SessionActionId == sessionActionId)
			{
				return sBGUIElement;
			}
		}
		return null;
	}

	public virtual SBGUIElement FindDynamicSubElementSessionActionId(string sessionActionId, bool includeInactive)
	{
		return null;
	}

	public bool IsActive()
	{
		return base.gameObject.active;
	}

	public virtual void OnScreenStart(SBGUIScreen screen)
	{
		for (int i = 0; i < guiElements.Count; i++)
		{
			SBGUIElement sBGUIElement = guiElements[i];
			if ((bool)sBGUIElement)
			{
				sBGUIElement.OnScreenStart(screen);
			}
		}
	}

	public virtual void OnScreenEnd(SBGUIScreen screen)
	{
		MuteButtons(false);
		for (int i = 0; i < guiElements.Count; i++)
		{
			SBGUIElement sBGUIElement = guiElements[i];
			if ((bool)sBGUIElement)
			{
				sBGUIElement.OnScreenEnd(screen);
			}
		}
	}

	public virtual void SetVisible(bool viz)
	{
		if (base.gameObject.GetComponent<Renderer>() != null)
		{
			base.gameObject.GetComponent<Renderer>().enabled = viz;
		}
	}

	public virtual void SetActive(bool active)
	{
		base.gameObject.SetActiveRecursively(active);
	}

	public virtual void SetScreenPosition(float pos_x, float pos_y)
	{
		SetScreenPosition(new Vector2(pos_x, pos_y));
	}

	public void SetScreenPosition(Vector2 pos)
	{
		pos.y = (float)Screen.height - pos.y;
		Vector3 vector = View.ScreenToWorld(pos);
		Vector3 position = tform.position;
		position.x = vector.x;
		position.y = vector.y;
		tform.position = position;
		UpdateColliderTransforms();
	}

	public Vector2 GetScreenPosition()
	{
		Vector2 result = View.WorldToScreen(tform.position);
		result.y = (float)Screen.height - result.y;
		return result;
	}

	protected void UpdateColliderTransforms()
	{
		YG2DBody[] componentsInChildren = GetComponentsInChildren<YG2DBody>(true);
		YG2DBody[] array = componentsInChildren;
		foreach (YG2DBody yG2DBody in array)
		{
			yG2DBody.MatchTransform3D();
		}
	}

	public void SetPosition(float pos_x, float pos_y, float pos_z)
	{
		Vector3 localPosition = base.gameObject.transform.localPosition;
		localPosition.z = pos_z;
		base.gameObject.transform.localPosition = localPosition;
		SetScreenPosition(pos_x, pos_y);
	}

	public void SetPosition(Vector3 pos)
	{
		SetPosition(pos.x, pos.y, pos.z);
	}

	public void SetLookAt(Vector3 position, Vector3 up)
	{
		tform.LookAt(position, up);
	}

	public virtual void SetParent(SBGUIElement element)
	{
		SetTransformParent(element);
		EnforceMuteFromParent(element);
	}

	public virtual void SetParent(SBGUIElement element, bool bEnforceMuteFromParent)
	{
		SetTransformParent(element);
		if (bEnforceMuteFromParent)
		{
			EnforceMuteFromParent(element);
		}
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
		if (View != null)
		{
			View.RefreshEvent -= ReregisterColliders;
		}
	}

	public virtual void AttachAnalyticsToButton(string buttonName, SBGUIButton button)
	{
	}

	public SBGUIButton AttachActionToButton(string buttonName, Action action)
	{
		SBGUIButton button = FindChild(buttonName) as SBGUIButton;
		return AttachActionToButton(button, action);
	}

	public SBGUIButton AttachActionToButton(SBGUIButton button, Action action)
	{
		if (button == null)
		{
			TFUtils.Assert(false, string.Format("{0} doesn't have a child named {1}", base.gameObject.name, button.name));
			return null;
		}
		AttachAnalyticsToButton(button.name, button);
		button.ClickEvent += action;
		button.ClickEvent += delegate
		{
			ReactivateButton(button);
		};
		return button;
	}

	public void EnforceMuteFromParent(SBGUIElement element)
	{
		if (element != null && element.muted)
		{
			muted = true;
			MuteButtons(true);
		}
	}

	public void ClearButtonActions(string buttonName)
	{
		SBGUIButton sBGUIButton = FindChild(buttonName) as SBGUIButton;
		if (sBGUIButton == null)
		{
			TFUtils.Assert(false, string.Format("{0} doesn't have a child named {1}", base.gameObject.name, buttonName));
		}
		else
		{
			sBGUIButton.ClearClickEvents();
		}
	}

	public void ReactivateButton(SBGUIButton button)
	{
		TwoShadeButton component = button.GetComponent<TwoShadeButton>();
		if (component != null)
		{
			component.ResetHighlightState();
		}
	}

	public void UpdateCollider()
	{
		YG2DBody component = base.gameObject.GetComponent<YG2DBody>();
		if (component.enabled)
		{
			component.enabled = false;
			component.enabled = true;
		}
	}

	public void StartTimer()
	{
		startTime = DateTime.UtcNow;
	}

	public virtual void OnDestroy()
	{
		SBGUI currentInstance = SBGUI.GetCurrentInstance();
		if (currentInstance != null)
		{
			currentInstance.UnWhitelistElement(this);
		}
	}
}
