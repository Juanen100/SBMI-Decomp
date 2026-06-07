using UnityEngine;

public class TutorialHandDragGuide : ClickableUiPointer
{
	private const string PREFAB_NAME = "Prefabs/GUI/Widgets/TutorialHandGuide";

	private Sinusoid sinusoid;

	private Simulated simulatedTarget;

	private SBGUIElement subHandTransform;

	private SBGUIPulseImage subIcon;

	private float timeAccumulated;

	private float period;

	public void Spawn(Game game, SessionActionTracker parentAction, SBGUIElement elementTarget, SBGUIScreen containingScreen, Simulated simulatedTarget, string iconTexture, float duration)
	{
	}

	protected void Initialize(Game game, SessionActionTracker action, Vector3 offset, float rotationCwDeg, float alpha, Vector2 scale, SBGUIElement elementTarget, SBGUIScreen containingScreen, Simulated simulatedTarget, string iconTexture, float duration)
	{
	}

	public override SessionActionManager.SpawnReturnCode OnUpdate(Game game)
	{
		return default(SessionActionManager.SpawnReturnCode);
	}
}
