using UnityEngine;

public class GuideArrow : ClickableUiPointer
{
	private const string PREFAB_NAME = "Prefabs/GUI/Widgets/TutorialPointer";

	private JumpPattern bouncer;

	public void Spawn(Game game, SessionActionTracker parentAction, SBGUIElement elementTarget, SBGUIScreen containingScreen)
	{
	}

	protected void Initialize(Game game, SessionActionTracker action, Vector3 offset, float rotationCwDeg, float alpha, Vector2 scale, SBGUIElement elementTarget, SBGUIScreen containingScreen)
	{
	}

	public override SessionActionManager.SpawnReturnCode OnUpdate(Game game)
	{
		return default(SessionActionManager.SpawnReturnCode);
	}
}
