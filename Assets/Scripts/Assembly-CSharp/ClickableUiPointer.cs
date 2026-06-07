using UnityEngine;

public abstract class ClickableUiPointer : VisualSpawn
{
	private UiSpawnMixin uiMixin;

	private SBGUIElement element;

	private SBGUIElement parentElement;

	public SBGUIElement Element
	{
		get
		{
			return null;
		}
	}

	protected SBGUIElement Parent
	{
		get
		{
			return null;
		}
	}

	protected virtual void Initialize(Game game, SessionActionTracker action, Vector3 offset, float rotationCwDeg, float alpha, Vector2 scale, SBGUIElement elementTarget, SBGUIScreen containingScreen, string pointerPrefab)
	{
	}

	public override SessionActionManager.SpawnReturnCode OnUpdate(Game game)
	{
		return default(SessionActionManager.SpawnReturnCode);
	}

	public bool ElementIsInGoodState(SBGUIElement element)
	{
		return false;
	}

	public override void Destroy()
	{
	}
}
