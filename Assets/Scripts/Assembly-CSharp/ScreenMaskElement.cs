using System.Collections.Generic;
using UnityEngine;

public class ScreenMaskElement : UiTargetingSessionActionDefinition
{
	public const string TYPE = "screenmask_element";

	private float radius;

	private Vector3 offset;

	private string texture;

	private const string RADIUS = "radius";

	private const string TEXTURE = "texture";

	private const string OFFSET = "offset";

	public static ScreenMaskElement Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		return null;
	}

	public override void Handle(Session session, SessionActionTracker action, SBGUIElement target, SBGUIScreen containingScreen)
	{
	}

	protected new void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
	}

	public override Dictionary<string, object> ToDict()
	{
		return null;
	}
}
