using UnityEngine;

public class GuideArrow : ClickableUiPointer
{
	private const string PREFAB_NAME = "Prefabs/GUI/Widgets/TutorialPointer";

	private JumpPattern bouncer;

	public void Spawn(Game game, SessionActionTracker parentAction, SBGUIElement elementTarget, SBGUIScreen containingScreen)
	{
		GuideArrow guideArrow = new GuideArrow();
		guideArrow.Initialize(game, parentAction, offset + new Vector3(0f, 0f, -0.4f), base.Rotation, base.Alpha, base.Scale, elementTarget, containingScreen);
	}

	protected void Initialize(Game game, SessionActionTracker action, Vector3 offset, float rotationCwDeg, float alpha, Vector2 scale, SBGUIElement elementTarget, SBGUIScreen containingScreen)
	{
		base.Initialize(game, action, offset, rotationCwDeg, alpha, scale, elementTarget, containingScreen, "Prefabs/GUI/Widgets/TutorialPointer");
		bouncer = new JumpPattern(-7f, 0.55f, 0.18f, 0.3f, 0f, Time.time, base.Element.transform.localScale);
		Material sharedMaterial = base.Element.GetComponent<Renderer>().sharedMaterial;
		if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Standard)
		{
			base.Element.GetComponent<Renderer>().material = Resources.Load("Materials/lod/TutorialPointer_lr") as Material;
		}
		else if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Low)
		{
			base.Element.GetComponent<Renderer>().material = Resources.Load("Materials/lod/TutorialPointer_lr2") as Material;
		}
		else
		{
			base.Element.GetComponent<Renderer>().material = Resources.Load("Materials/lod/TutorialPointer") as Material;
		}
		if (sharedMaterial != null)
		{
			Resources.UnloadAsset(sharedMaterial);
		}
		Transform parent = elementTarget.transform.parent;
		SBGUIElement component = parent.GetComponent<SBGUIElement>();
		if (component != null)
		{
			component.EnableRejectButton(false);
		}
	}

	public override SessionActionManager.SpawnReturnCode OnUpdate(Game game)
	{
		float val;
		Vector2 squish;
		bouncer.ValueAndSquishAtTime(Time.time, out val, out squish);
		if (base.Element != null && base.Element.gameObject != null)
		{
			Vector3 vector = new Vector3(base.Direction.x * val, base.Direction.y * val, 0f);
			base.Element.gameObject.transform.localPosition = offset + vector;
			base.Element.gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f - base.Rotation);
			base.Element.gameObject.transform.localScale = TFUtils.ExpandVector(squish);
		}
		return base.OnUpdate(game);
	}
}
