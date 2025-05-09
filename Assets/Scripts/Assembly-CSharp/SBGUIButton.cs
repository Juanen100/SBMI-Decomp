#define ASSERTS_ON
using System;
using UnityEngine;

[RequireComponent(typeof(TapButton))]
[RequireComponent(typeof(YG2DRectangle))]
public class SBGUIButton : SBGUIImage
{
	protected YG2DBody body;

	protected TapButton button;

	protected bool collisions = true;

	public bool unmutable;

	public string analyticsTag;

	private Action QuestConditionAction;

	private Action AnalyticsAction;

	public override Vector3 WorldPosition
	{
		get
		{
			return base.WorldPosition;
		}
		set
		{
			if (!(base.tform.position == value))
			{
				base.tform.position = value;
				YGSprite.MeshUpdateHierarchy(base.gameObject);
			}
		}
	}

	public new bool enabled
	{
		get
		{
			return collisions;
		}
		set
		{
			collisions = value;
			body.enabled = collisions && !muted;
		}
	}

	protected override bool Muted
	{
		get
		{
			return muted;
		}
		set
		{
			if (!unmutable)
			{
				muted = value;
				body.enabled = collisions && !muted;
			}
		}
	}

	public event Action ClickEvent
	{
		add
		{
			TFUtils.Assert(button != null, "Trying to add or remove click events from a null button!");
			button.TapEvent.AddListener(value);
			AddQuestConditionToButton();
			AddAnalyticsToButton();
		}
		remove
		{
			TFUtils.Assert(button != null, "Trying to add or remove click events from a null button!");
			button.TapEvent.RemoveListener(value);
		}
	}

	public virtual void MockClick()
	{
		button.TapEvent.FireEvent();
	}

	public Vector2 ResetSize()
	{
		return base.sprite.ResetSize();
	}

	public void ClearClickEvents()
	{
		if (!(button == null))
		{
			button.TapEvent.ClearListeners();
			RemoveQuestConditionFromButton();
			RemoveAnalyticsFromButton();
		}
	}

	protected override void Awake()
	{
		button = base.gameObject.GetComponent<TapButton>();
		body = base.gameObject.GetComponent<YG2DBody>();
		base.Awake();
	}

	private void AddQuestConditionToButton()
	{
		if (QuestConditionAction != null || button == null || base.gameObject == null)
		{
			return;
		}
		Session pSession = null;
		SBGUIScreen sBGUIScreen = null;
		Transform parent = base.gameObject.transform;
		while (parent != null)
		{
			sBGUIScreen = parent.gameObject.GetComponent<SBGUIScreen>();
			if (sBGUIScreen != null)
			{
				pSession = sBGUIScreen.session;
				break;
			}
			parent = parent.parent;
		}
		if (pSession != null && pSession.TheGame != null && pSession.TheGame.simulation != null && base.gameObject != null)
		{
			QuestConditionAction = delegate
			{
				pSession.TheGame.simulation.ModifyGameState(new ButtonTapAction(base.gameObject.name));
			};
			ClickEvent += QuestConditionAction;
		}
	}

	private void RemoveQuestConditionFromButton()
	{
		if (QuestConditionAction != null)
		{
			ClickEvent -= QuestConditionAction;
			QuestConditionAction = null;
		}
	}

	private void AddAnalyticsToButton()
	{
		if (string.IsNullOrEmpty(analyticsTag) || AnalyticsAction != null || button == null || base.gameObject == null)
		{
			return;
		}
		Session pSession = null;
		SBGUIScreen sBGUIScreen = null;
		Transform parent = base.gameObject.transform;
		while (parent != null)
		{
			sBGUIScreen = parent.gameObject.GetComponent<SBGUIScreen>();
			if (sBGUIScreen != null)
			{
				pSession = sBGUIScreen.session;
				break;
			}
			parent = parent.parent;
		}
		if (pSession != null && pSession.TheGame != null && pSession.TheGame.simulation != null && base.gameObject != null)
		{
			AnalyticsAction = delegate
			{
				AnalyticsWrapper.LogUIInteraction(pSession.TheGame, analyticsTag, "button", "tap");
			};
			ClickEvent += AnalyticsAction;
		}
	}

	private void RemoveAnalyticsFromButton()
	{
		if (AnalyticsAction != null)
		{
			ClickEvent -= AnalyticsAction;
			AnalyticsAction = null;
		}
	}
}
