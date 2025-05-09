#define ASSERTS_ON
using System;
using System.Collections;
using UnityEngine;

public abstract class SBGUIScrollableDialog : SBGUIScreen
{
	public SBGUIScrollRegion region;

	protected EntityManager entityMgr;

	protected ResourceManager resourceMgr;

	protected CostumeManager costumeMgr;

	protected SoundEffectManager soundEffectMgr;

	private Bounds viewBounds;

	private Vector3 windowPosition;

	private float windowHeight;

	private SBGUIImage windowSprite;

	protected override bool Muted
	{
		get
		{
			return base.Muted;
		}
		set
		{
			base.Muted = value;
			if (region != null)
			{
				region.MuteButtons(value);
			}
		}
	}

	public event Action ReadyEvent
	{
		add
		{
			if (region == null)
			{
				TFUtils.Assert(false, "Screen's region isn't defined");
			}
			else
			{
				region.ReadyEvent.AddListener(value);
			}
		}
		remove
		{
			if (region == null)
			{
				TFUtils.Assert(false, "Screen's region isn't defined");
			}
			else
			{
				region.ReadyEvent.RemoveListener(value);
			}
		}
	}

	public virtual void Start()
	{
		GUIMainView.GetInstance().Library.bShowingDialog = true;
		viewBounds = GUIMainView.GetInstance().ViewBounds();
		windowSprite = (SBGUIImage)FindChild("window");
		if (windowSprite == null)
		{
			windowSprite = (SBGUIAtlasImage)FindChild("window");
		}
		windowHeight = windowSprite.Size.y * 0.01f;
		windowPosition = windowSprite.WorldPosition;
		EnableButtons(false);
		StartCoroutine(AnimateIn(0.5f, delegate
		{
			EnableButtons(true);
			if (region != null)
			{
				region.MatchAndRegister();
			}
		}));
	}

	private IEnumerator AnimateIn(float duration, Action completeAction)
	{
		float interp = 0f;
		Vector3 origin = windowPosition;
		origin.y = viewBounds.min.y - windowHeight;
		windowSprite.WorldPosition = origin;
		while (interp <= 1f)
		{
			interp += Time.deltaTime / duration;
			Vector3 interpPos = Easing.Vector3Easing(origin, windowPosition, interp, Easing.EaseOutBack);
			windowSprite.WorldPosition = interpPos;
			yield return null;
		}
		windowSprite.WorldPosition = windowPosition;
		if (completeAction != null)
		{
			completeAction();
		}
	}

	public virtual void ShowScrollRegion(bool visible)
	{
		region.SetActive(visible);
	}

	public void SetManagers(EntityManager emgr, ResourceManager resMgr, SoundEffectManager sfxMgr, CostumeManager cosMgr)
	{
		entityMgr = emgr;
		resourceMgr = resMgr;
		soundEffectMgr = sfxMgr;
		costumeMgr = cosMgr;
	}

	public override void MuteButtons(bool mute)
	{
		base.MuteButtons(mute);
		if (region != null)
		{
			SBGUIElement componentInChildren = region.GetComponent<ScrollRegion>().subView.GetComponentInChildren<SBGUIElement>();
			if (componentInChildren != null)
			{
				componentInChildren.MuteButtons(mute);
			}
		}
	}

	protected virtual void Setup()
	{
		if (region != null)
		{
			region.ResetScroll();
			region.ResetToMinScroll();
		}
	}

	public override void Deactivate()
	{
		if (region != null)
		{
			region.ReadyEvent.ClearListeners();
		}
		GUIMainView.GetInstance().Library.bShowingDialog = false;
		base.Deactivate();
	}

	public override void OnDestroy()
	{
		if (region != null)
		{
			YGAtlasSprite[] componentsInChildren = region.GetComponentsInChildren<YGAtlasSprite>();
			YGAtlasSprite[] array = componentsInChildren;
			foreach (YGAtlasSprite yGAtlasSprite in array)
			{
				if (!string.IsNullOrEmpty(yGAtlasSprite.nonAtlasName))
				{
					base.View.Library.incrementTextureDuplicates(yGAtlasSprite.nonAtlasName);
				}
			}
			UnityEngine.Object.Destroy(region);
		}
		GUIMainView.GetInstance().Library.bShowingDialog = false;
		base.OnDestroy();
	}
}
