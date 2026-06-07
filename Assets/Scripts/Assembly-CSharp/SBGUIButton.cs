using System;
using UnityEngine;

public class SBGUIButton : SBGUIImage
{
	protected YG2DBody body;

	protected TapButton button;

	protected bool collisions;

	public bool unmutable;

	public string analyticsTag;

	private Action QuestConditionAction;

	private Action AnalyticsAction;

	public override Vector3 WorldPosition
	{
		get
		{
			return default(Vector3);
		}
		set
		{
		}
	}

	public new bool enabled
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

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

	public event Action ClickEvent
	{
		add
		{
		}
		remove
		{
		}
	}

	public virtual void MockClick()
	{
	}

	public Vector2 ResetSize()
	{
		return default(Vector2);
	}

	public void ClearClickEvents()
	{
	}

	protected override void Awake()
	{
	}

	private void AddQuestConditionToButton()
	{
	}

	private void RemoveQuestConditionFromButton()
	{
	}

	private void AddAnalyticsToButton()
	{
	}

	private void RemoveAnalyticsFromButton()
	{
	}
}
