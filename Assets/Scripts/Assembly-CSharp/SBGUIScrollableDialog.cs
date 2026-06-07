using System;
using System.Collections;
using System.Diagnostics;
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
			return false;
		}
		set
		{
		}
	}

	public event Action ReadyEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public virtual void Start()
	{
	}

	[DebuggerHidden]
	private IEnumerator AnimateIn(float duration, Action completeAction)
	{
		return null;
	}

	public virtual void ShowScrollRegion(bool visible)
	{
	}

	public void SetManagers(EntityManager emgr, ResourceManager resMgr, SoundEffectManager sfxMgr, CostumeManager cosMgr)
	{
	}

	public override void MuteButtons(bool mute)
	{
	}

	protected virtual void Setup()
	{
	}

	public override void Deactivate()
	{
	}

	public override void OnDestroy()
	{
	}
}
