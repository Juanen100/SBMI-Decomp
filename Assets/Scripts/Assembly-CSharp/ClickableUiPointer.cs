using System;
using UnityEngine;

public abstract class ClickableUiPointer : VisualSpawn
{
	private UiSpawnMixin uiMixin = new UiSpawnMixin();

	private SBGUIElement element;

	private SBGUIElement parentElement;

	public SBGUIElement Element
	{
		get
		{
			return element;
		}
	}

	protected SBGUIElement Parent
	{
		get
		{
			return parentElement;
		}
	}

	protected virtual void Initialize(Game game, SessionActionTracker action, Vector3 offset, float rotationCwDeg, float alpha, Vector2 scale, SBGUIElement elementTarget, SBGUIScreen containingScreen, string pointerPrefab)
	{
		base.Initialize(game, action, offset, rotationCwDeg, alpha, scale);
		uiMixin.OnRegisterNewInstance(action, containingScreen);
		parentElement = elementTarget;
		element = SBGUI.InstantiatePrefab(pointerPrefab);
		element.SetParent(parentElement);
		element.gameObject.transform.localPosition = Vector3.zero;
		element.gameObject.transform.localScale = scale;
		element.Alpha = base.Alpha;
		if (!base.ParentAction.ManualSuccess && base.ParentAction.Definition.UsingDefaultSucceedConditions)
		{
			SBGUIButton[] componentsInChildren = parentElement.GetComponentsInChildren<SBGUIButton>(true);
			foreach (SBGUIButton button in componentsInChildren)
			{
				Action succeedOnClick = null;
				succeedOnClick = delegate
				{
					if (this != null && base.ParentAction != null && base.ParentAction.Status != SessionActionTracker.StatusCode.FINISHED_FAILURE && base.ParentAction.Status != SessionActionTracker.StatusCode.OBLITERATED)
					{
						base.ParentAction.MarkSucceeded(false);
					}
					if (this != null && button != null)
					{
						button.ClickEvent -= succeedOnClick;
					}
				};
				button.ClickEvent += succeedOnClick;
			}
		}
		Bounds totalBounds = parentElement.TotalBounds;
		float widthOver = totalBounds.size.x * 0.5f * 0.01f;
		float heightOver = totalBounds.size.y * 0.5f * 0.01f;
		NormalizeRotationAndPushToEdge(widthOver, heightOver);
	}

	public override SessionActionManager.SpawnReturnCode OnUpdate(Game game)
	{
		if (base.ParentAction.Status == SessionActionTracker.StatusCode.STARTED && (!ElementIsInGoodState(parentElement) || !ElementIsInGoodState(element)))
		{
			base.ParentAction.MarkFailed();
		}
		return base.OnUpdate(game);
	}

	public bool ElementIsInGoodState(SBGUIElement element)
	{
		return element != null && element.gameObject != null && element.IsActive() && element.gameObject.GetComponent<Renderer>() != null && element.gameObject.GetComponent<Renderer>().enabled;
	}

	public override void Destroy()
	{
		uiMixin.Destroy();
		if (element != null && element.gameObject != null)
		{
			UnityEngine.Object.Destroy(element.gameObject);
		}
	}
}
