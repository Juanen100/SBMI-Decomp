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
		TutorialHandDragGuide tutorialHandDragGuide = new TutorialHandDragGuide();
		tutorialHandDragGuide.Initialize(game, parentAction, offset, base.Rotation, base.Alpha, base.Scale, elementTarget, containingScreen, simulatedTarget, iconTexture, duration);
	}

	protected void Initialize(Game game, SessionActionTracker action, Vector3 offset, float rotationCwDeg, float alpha, Vector2 scale, SBGUIElement elementTarget, SBGUIScreen containingScreen, Simulated simulatedTarget, string iconTexture, float duration)
	{
		base.Initialize(game, action, offset, rotationCwDeg, alpha, scale, elementTarget, containingScreen, "Prefabs/GUI/Widgets/TutorialHandGuide");
		period = duration * 2f;
		sinusoid = new Sinusoid(0f, 1f, period, 0f);
		this.simulatedTarget = simulatedTarget;
		subHandTransform = base.Element.FindChild("hand");
		subIcon = base.Element.FindChild("icon").GetComponent<SBGUIPulseImage>();
		subIcon.InitializePulser(subIcon.Size, 2f, 0.2f);
		subIcon.SetTextureFromAtlas(iconTexture);
	}

	public override SessionActionManager.SpawnReturnCode OnUpdate(Game game)
	{
		if (base.Element != null && base.Element.gameObject != null)
		{
			Vector3 vector = TFUtils.ExpandVector(base.Parent.GetScreenPosition()) + offset;
			Vector3 vector2 = game.simulation.TheCamera.WorldToScreenPoint(simulatedTarget.DisplayController.Position);
			Vector3 to = new Vector3(vector2.x, SBGUI.GetScreenHeight() - vector2.y, 0f) + offset;
			Vector3 vector3 = Vector3.Lerp(vector, to, sinusoid.ValueAtTime(timeAccumulated));
			base.Element.SetScreenPosition(vector3);
			if (timeAccumulated > period / 2f)
			{
				base.Element.transform.localScale = Vector3.zero;
			}
			else
			{
				base.Element.transform.localScale = new Vector3(2f, 2f, 2f);
			}
			if (timeAccumulated == 0f)
			{
				subIcon.Pulser.PulseOneShot();
			}
			float num = timeAccumulated / period * 80f;
			subHandTransform.transform.localRotation = Quaternion.Euler(0f, 0f, num);
			subIcon.transform.localRotation = Quaternion.Euler(0f, 0f, 0f - num);
			timeAccumulated += Time.deltaTime;
			if (timeAccumulated > period)
			{
				timeAccumulated = 0f;
			}
		}
		return base.OnUpdate(game);
	}
}
